using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH
{
    public class JornadaController : ODataController
    {
        private readonly bdERP db;
        private readonly CalendarioController cc;

        public JornadaController(bdERP context)
        {
            db = context;
            cc = new(db);
        }

        [HttpPost("odata/MyPolarier/RRHH/Jornada/ModificarRegistrosDia")]
        [Authorize]
        public async Task<ActionResult> ModificarRegistrosDia([FromBody] payloadModificarRegistros payload, [FromODataUri] int idPersona, [FromODataUri] DateTime fecha)
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            tblPersona objPersona = db.tblPersona.FirstOrDefault(x => x.idPersona == idPersona);
            if (objPersona == null)
            {
                return BadRequest("No se ha encontrado la persona");
            }

            CuadrantePersonalController cp = new(db);

            await cp.IUD_CuadrantePersonal(new() { payload.cuadrante }, idUsuario);

            List<tblJornada> insertJornadas = new();

            foreach (var jornadaPatch in payload.jornadas)
            {
                var jornada = db.tblJornada
                    .Include(x => x.tblEventoPersona)
                    .FirstOrDefault(x => x.idJornada == jornadaPatch.idJornada) ?? new tblJornada();

                db.tblBalanceHorasExtra.RemoveRange(db.tblBalanceHorasExtra.Where(x => x.idJornada == jornadaPatch.idJornada));
                db.tblBalanceHoras.RemoveRange(db.tblBalanceHoras.Where(x => x.idJornada == jornadaPatch.idJornada));

                jornada.isRevisado = true;
                jornada.idUsuario_validacion = idUsuario;
                jornada.fecha_validacion = DateTimeOffset.UtcNow;
                jornada.idTipoTrabajo = (byte)objPersona.idTipoTrabajo;
                jornada.horasDiarias = objPersona.horasDiarias;

                var operations_horaFin = jornadaPatch.patch.Operations.FirstOrDefault(x => x.path == "/horaFin");
                if (operations_horaFin != null)
                {

                    var operations_horaIni = jornadaPatch.patch.Operations.FirstOrDefault(x => x.path == "/horaIni");
                    var horaIni = operations_horaIni != null ? TimeSpan.Parse((string)operations_horaIni.value) : jornada.horaIni.Value;
                    var horaFin = TimeSpan.Parse((string)operations_horaFin.value);

                    DesloguearMyRealData(idPersona, fecha, horaIni, horaFin);
                }

                var operations_idCuadrantePersonal = jornadaPatch.patch.Operations.FirstOrDefault(x => x.path == "/idCuadrantePersonal");
                if (operations_idCuadrantePersonal != null && (long)operations_idCuadrantePersonal.value == 0)
                {
                    operations_idCuadrantePersonal.value = db.tblCuadrantePersonal.FirstOrDefault(cp => cp.idPersona == idPersona && cp.fecha.Date == fecha.Date)?.idCuadrantePersonal;
                }

                jornadaPatch.patch.ApplyTo(jornada);

                if (jornadaPatch.idJornada == null) insertJornadas.Add(jornada);
            }

            db.tblJornada.AddRange(insertJornadas);

            List<tblJornada> deleteJornadas = db.tblJornada
                .Include(x => x.tblBalanceHoras)
                .Include(x => x.tblBalanceHorasExtra)
                .Include(x => x.tblEventoPersona)
                .Where(x => x.idPersona == idPersona && x.fecha == fecha && !payload.jornadas.Select(x => x.idJornada).Contains(x.idJornada))
                .ToList();

            foreach (var jornada in deleteJornadas)
            {
                var tblCalendarioPersonal = db.tblCalendarioPersonal.Where(cp => cp.idJornada == jornada.idJornada);

                foreach (var cape in tblCalendarioPersonal)
                {
                    cape.idJornada = null;
                }

                db.tblBalanceHoras.RemoveRange(jornada.tblBalanceHoras);
                db.tblBalanceHorasExtra.RemoveRange(jornada.tblBalanceHorasExtra);

                foreach (var evento in jornada.tblEventoPersona)
                {
                    evento.isRevisado = true;
                }

                jornada.tblEventoPersona.Clear();
            }

            db.tblJornada.RemoveRange(deleteJornadas);

            await db.SaveChangesAsync();

            var groupedJornadas = insertJornadas
                .GroupBy(j => new { j.idPersona, j.fecha.Date })
                .Select(g => g.First());

            foreach (var jornada in groupedJornadas)
            {
                var tblCalendarioPersonal = db.tblCalendarioPersonal.Where(cp => cp.idPersona == jornada.idPersona && cp.fecha.Date == jornada.fecha.Date);

                foreach (var eventoPersona in tblCalendarioPersonal)
                {
                    eventoPersona.idJornada = jornada.idJornada;
                }
            }

            await db.SaveChangesAsync();

            return Ok(true);
        }

        [HttpGet("odata/MyPolarier/RRHH/Jornada/DeclararAbsentismo")]
        [Authorize]
        public async Task<ActionResult> DeclararAbsentismo([FromODataUri] int idPersona, [FromODataUri] DateTime fecha, [FromODataUri] int idLavanderia, [FromODataUri] int idTurno)
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            List<tblJornada> deleteJornadas = db.tblJornada.Include(x => x.tblEventoPersona).Where(x => x.idPersona == idPersona && x.fecha == fecha).ToList();
            var eventos = deleteJornadas.Select(x => x.tblEventoPersona).SelectMany(x => x);

            foreach (var evento in eventos)
            {
                evento.idJornada = null;
            }

            tblCuadrantePersonal cuadrantePersonal =
                db.tblCuadrantePersonal.FirstOrDefault(cp => cp.idPersona == idPersona && cp.fecha.Date == fecha.Date)
                ?? new()
                {
                    idPersona = idPersona,
                    fecha = fecha,
                };

            cuadrantePersonal.idCalendario_Estado = (byte)idsCalendario_Estado.Absentismo;
            cuadrantePersonal.horaEntrada = null;
            cuadrantePersonal.horaSalida = null;
            cuadrantePersonal.idLavanderia = idLavanderia;
            cuadrantePersonal.idTurno = idTurno;
            cuadrantePersonal.idPosicionNAreaLavanderiaNLavanderia = null;
            cuadrantePersonal.idUsuario_validacion = idUsuario;
            cuadrantePersonal.fecha_validacion = DateTimeOffset.UtcNow;

            tblCalendarioPersonal eventoCalendarioPersonal = new()
            {
                fecha = fecha,
                idPersona = idPersona,
                idCalendario_Estado = (byte)idsCalendario_Estado.Absentismo,
                idLavanderia = idLavanderia,
                idUsuario_validacion = idUsuario,
                fecha_validacion = DateTimeOffset.UtcNow,
                idCuadrantePersonalNavigation = cuadrantePersonal
            };

            await cc.IUD_CalendarioPersonal(new() { eventoCalendarioPersonal }, idUsuario);

            db.tblJornada.RemoveRange(deleteJornadas);

            await db.SaveChangesAsync();

            return Ok(true);
        }

        private void DesloguearMyRealData(int idPersona, DateTime fecha, TimeSpan horaIni, TimeSpan horaFin)
        {
            var fechaIni = new DateTime(fecha.Year, fecha.Month, fecha.Day, horaIni.Hours, horaIni.Minutes, horaIni.Seconds);
            fechaIni = fechaIni.AddMinutes(-30); // Tiempo de gracia previo a inicio jornada
            var fechaFin = new DateTime(fecha.Year, fecha.Month, fecha.Day, horaFin.Hours, horaFin.Minutes, horaFin.Seconds);
            if (horaIni > horaFin)
            {
                fechaFin = fechaFin.AddDays(1);
            }

            var tblPersonaNMaquina = db.tblPersonaNMaquina
                .Where(x =>
                    x.idPersona == idPersona &&
                    x.fechaFin == null &&
                    x.fechaIni.Value >= fechaIni &&
                    x.fechaIni.Value <= fechaFin
                )
                .ToList();
            var tblPersonaNAreaNLavanderia = db.tblPersonaNAreaNLavanderia
                .Where(x =>
                    x.idPersona == idPersona &&
                    x.fechaFin == null &&
                    x.fechaIni >= fechaIni &&
                    x.fechaIni <= fechaFin
                )
                .ToList();


            foreach (var item in tblPersonaNMaquina)
            {
                item.fechaFin = fechaFin;
            }

            foreach (var item in tblPersonaNAreaNLavanderia)
            {
                item.fechaFin = fechaFin;
            }
        }
    }

    public class payloadModificarRegistros
    {
        public tblCuadrantePersonal cuadrante;
        public List<jornadaPatch> jornadas;
    }

    public class jornadaPatch
    {
        public int? idJornada;
        public JsonPatchDocument<tblJornada> patch;
    }
}