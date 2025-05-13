using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Linq.Dynamic.Core;
using WebApiCore.Context;
using WebApiCore.Enums.General;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.Dashboard.SmartView
{
    public class InformeBalanceHorasController : ODataController
    {

        public enum TiposVista : int
        {
            BolsaHoras = 1,
            HorasExtra = 2,
            Nocturnidad = 3,
            Absentismo = 4,
            Festivo = 5,
            Impuntualidad = 6,
        }

        private readonly bdERP db;
        public InformeBalanceHorasController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [Authorize]
        [HttpGet("odata/InformeJornada/Cierres")]
        public async Task<ActionResult> Cierres([FromODataUri] int? idLavanderia, [FromODataUri] DateTime? fecha, [FromODataUri] bool entreCierres)
        {
            if (idLavanderia == null || fecha == null) return BadRequest("No se han proporcionado idLavanderia o fecha");

            return Ok(GetCierres((int)idLavanderia, fecha.Value, entreCierres));
        }

        private FechasCierres GetCierres(int idLavanderia, DateTime fecha, bool entreCierres)
        {

            DateTime fechaHasta = fecha.AddMonths(1).AddDays(-1);
            DateTime fechaDesde = fecha;

            if (!entreCierres)
                return new FechasCierres
                {
                    fechaDesde = fechaDesde, 
                    fechaHasta = fechaHasta 
                };

            var fechasCierreNomina = db.tblCalendarioLavanderia
                    .Where(x =>
                        x.idLavanderia == idLavanderia &&
                        x.idCalendario_Estado == 14 && // Estado de cierre de nómina
                        x.fecha <= fechaHasta
                        )
                    .OrderByDescending(x => x.fecha)
                    .Select(x => x.fecha)
                    .Take(2)
                    .ToList();

            var fechaHastaCierre = fechasCierreNomina.FirstOrDefault();
            var fechaDesdeCierre = fechasCierreNomina.LastOrDefault().AddDays(1);

            if (fechaHastaCierre.Month == fechaHasta.Month && fechaDesdeCierre.Month == fechaDesde.Month - 1)
            {
                fechaHasta = fechaHastaCierre;
                fechaDesde = fechaDesdeCierre;
            }
            if (fechaHastaCierre.Month == fechaHasta.Month - 1)
            {
                fechaDesde = fechaHastaCierre.AddDays(1);
            }

            return new FechasCierres
            {
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta
            };
        }

        private class FechasCierres
        {
            public DateTime fechaDesde { get; set; }
            public DateTime fechaHasta { get; set; }
        }

        [EnableQuery]
        [Authorize]
        [HttpGet("odata/InformeJornada")]
        public async Task<ActionResult> InformeJornada([FromODataUri] int idLavanderia, [FromODataUri] DateTime fecha, [FromODataUri] TiposVista tipo, [FromODataUri] bool entreCierres)
        {

            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            var objUsuario = db.tblUsuario
                .Include(x => x.idLavanderia)
                .FirstOrDefault(x => x.idUsuario == idUsuario);

            if (objUsuario == null) return BadRequest("No se ha encontrado el usuario");

            DateTime fechaHasta;
            DateTime fechaDesde;

            if (tipo == TiposVista.BolsaHoras)
            {
                fechaHasta = new DateTime(fecha.Year, 12, 31);
                fechaDesde = new DateTime(fecha.Year, 1, 1);
            }
            else
            {
                var cierres = GetCierres(idLavanderia, fecha, entreCierres);

                fechaHasta = cierres.fechaHasta;
                fechaDesde = cierres.fechaDesde;
            }

            List<int> idsPersona = new();

            var tblPersona = db.tblPersona.Where(pers =>
                        pers.idLavanderia == idLavanderia &&
                        pers.tblPersonaNTipoContrato.Any(contrato =>
                            contrato.fechaAltaContrato <= fechaHasta &&
                            (contrato.fechaBajaContrato == null || contrato.fechaBajaContrato >= fechaDesde)
                            ) &&
                        (
                            pers.idUsuario_validacion_nomina == idUsuario ||
                            db.tblTipoTrabajoNUsuario.Any(ttnu =>
                                ttnu.idUsuario == idUsuario &&
                                pers.idLavanderia == ttnu.idLavanderia &&
                                pers.idTipoTrabajo == ttnu.idTipoTrabajo
                                ) ||
                            (
                                objUsuario.enableDatosRRHH &&
                                db.tblUsuario.Any(x => x.idLavanderia.Any(lav => lav.idLavanderia == idLavanderia)) &&
                                pers.idLavanderiaNavigation.idPais == (int)idsPais.España
                            )
                        )
                    );

            var a = tblPersona.ToQueryString();

            idsPersona.AddRange(tblPersona.Select(x => x.idPersona));

            return tipo switch
            {
                TiposVista.BolsaHoras => Ok(InformeBolsaHoras(idsPersona, fecha)),
                TiposVista.HorasExtra => Ok(InformeHorasExtra(idsPersona, fechaDesde, fechaHasta)),
                TiposVista.Nocturnidad => Ok(InformeNocturnidad(idsPersona, fechaDesde, fechaHasta)),
                TiposVista.Absentismo => Ok(InformeAbsentismo(idsPersona, fechaDesde, fechaHasta)),
                TiposVista.Festivo => Ok(InformeFestivo(idsPersona, idLavanderia, fechaDesde, fechaHasta)),
                TiposVista.Impuntualidad => Ok(InformeImpuntualidad(idsPersona, fechaDesde, fechaHasta)),
                _ => BadRequest(),
            };
        }

        private IQueryable<RegistroInforme> InformeBolsaHoras(List<int> idsPersona, DateTime fecha)
        {
            return db.tblPersona
                    .Where(x => idsPersona.Contains(x.idPersona))
                    .Select(pers => new RegistroInforme
                    {
                        idPersona = pers.idPersona,
                        nombreCompleto = pers.nombre + " " + pers.apellidos,
                        registros = pers.tblBalanceHoras
                            .Where(y => y.fecha.Year == fecha.Year)
                            .GroupBy(x => new { x.idJornada, x.fecha })
                            .Select(x => new DesgloseBalanceJornada
                            {
                                idJornada = x.Key.idJornada,
                                registros = x
                                    .Where(j => j.idJornada == x.Key.idJornada)
                                    .Select(x => new RegistroBalance
                                    {
                                        idLavanderia = x.idJornadaNavigation.idLavanderia,
                                        jornadaIni = x.idJornadaNavigation.horaIni,
                                        jornadaFin = x.idJornadaNavigation.horaFin,
                                        cuadranteIni = x.idJornadaNavigation.idCuadrantePersonalNavigation.horaEntrada,
                                        cuadranteFin = x.idJornadaNavigation.idCuadrantePersonalNavigation.horaSalida
                                    })
                                    .First(),
                                minutos = x.Sum(x => x.minutos),
                                fecha = x.Key.fecha,
                            })
                            .OrderBy(x => x.fecha)
                            .ToDynamicList()
                    });
        }

        private IQueryable<RegistroInforme> InformeHorasExtra(List<int> idsPersona, DateTime fechaDesde, DateTime fechaHasta)
        {
            return db.tblPersona
                    .Where(x => idsPersona.Contains(x.idPersona))
                    .Select(pers => new RegistroInforme
                    {
                        idPersona = pers.idPersona,
                        nombreCompleto = pers.nombre + " " + pers.apellidos,
                        registros = pers.tblBalanceHorasExtra
                            .Where(x => x.fecha >= fechaDesde && x.fecha <= fechaHasta)
                            .GroupBy(x => new { x.idJornada, x.fecha })
                            .Select(x => new DesgloseBalanceJornada
                            {
                                idJornada = x.Key.idJornada,
                                registros = x
                                    .Where(j => j.idJornada == x.Key.idJornada)
                                    .Select(x => new RegistroBalance
                                    {
                                        idLavanderia = x.idJornadaNavigation.idLavanderia,
                                        jornadaIni = x.idJornadaNavigation.horaIni,
                                        jornadaFin = x.idJornadaNavigation.horaFin,
                                        cuadranteIni = x.idJornadaNavigation.idCuadrantePersonalNavigation.horaEntrada,
                                        cuadranteFin = x.idJornadaNavigation.idCuadrantePersonalNavigation.horaSalida
                                    })
                                    .First(),
                                minutos = x.Sum(x => x.minutos),
                                fecha = x.Key.fecha,
                            })
                            .OrderBy(x => x.fecha)
                            .ToDynamicList(),
                    });
        }

        private IEnumerable<RegistroInforme> InformeNocturnidad(List<int> idsPersona, DateTime fechaDesde, DateTime fechaHasta)
        {

            TimeSpan horaIni_noc = new TimeSpan(22, 0, 0) - TimeSpan.FromDays(1);
            TimeSpan horaFin_noc = new TimeSpan(6, 0, 0);

            var tblNocturnidad = db.tblPersona
                    .Where(x => idsPersona.Contains(x.idPersona))
                    .Select(pers => new RegistroInforme
                    {
                        idPersona = pers.idPersona,
                        nombreCompleto = pers.nombre + " " + pers.apellidos,
                        registros = pers.tblJornada
                            .Where(jorn => 
                                jorn.fecha >= fechaDesde && 
                                jorn.fecha <= fechaHasta //&& 
                                //(jorn.horaIni > jorn.horaFin || jorn.horaFin > horaIni_noc)
                            )
                            .GroupBy(x => new { x.idJornada, x.fecha })
                            .Select(x => new DesgloseBalanceJornada
                            {
                                idJornada = x.Key.idJornada,
                                registros = x
                                    .Where(j => j.idJornada == x.Key.idJornada)
                                    .Select(x => new RegistroBalance
                                    {
                                        idLavanderia = x.idLavanderia,
                                        jornadaIni = x.horaIni,
                                        jornadaFin = x.horaFin,
                                        cuadranteIni = x.idCuadrantePersonalNavigation.horaEntrada,
                                        cuadranteFin = x.idCuadrantePersonalNavigation.horaSalida
                                    })
                                    .First(),
                                minutos = 0,
                                fecha = x.Key.fecha,
                            })
                            .OrderBy(x => x.fecha)
                            .ToDynamicList(),
                    })
                    .ToList();

            foreach (var item in tblNocturnidad)
            {
                foreach (var x in item.registros)
                {
                    TimeSpan? horaIni = x.registros.jornadaIni;
                    TimeSpan? horaFin = x.registros.jornadaFin;


                    if (horaIni > horaFin)
                    {
                        horaIni = horaIni - TimeSpan.FromDays(1);
                    }

                    if (horaIni <= horaFin_noc && horaFin >= horaIni_noc)
                    {
                        if (horaIni < horaIni_noc)
                            horaIni = horaIni_noc;
                        if (horaFin > horaFin_noc)
                            horaFin = horaFin_noc;

                        var mins = Convert.ToDecimal((horaFin - horaIni)?.TotalMinutes);

                        x.minutos += Convert.ToInt32((Math.Floor(mins / 60)*60) + (mins / 60 % 1 >= (decimal)0.75 ? 60 : 0));

                    }

                }
                item.registros = item.registros.Where(x => x.minutos > 0).ToList();
            }

            return tblNocturnidad;
        }

        private IQueryable<RegistroInforme> InformeAbsentismo(List<int> idsPersona, DateTime fechaDesde, DateTime fechaHasta)
        {
            return db.tblPersona
                    .Where(x => idsPersona.Contains(x.idPersona))
                    .Select(pers => new RegistroInforme
                    {
                        idPersona = pers.idPersona,
                        nombreCompleto = pers.nombre + " " + pers.apellidos,
                        registros = pers.tblCuadrantePersonal
                            .Where(cuad =>
                                cuad.fecha >= fechaDesde &&
                                cuad.fecha <= fechaHasta &&
                                cuad.fecha < DateTime.Today &&
                                pers.tblCalendarioPersonal.Any(cp =>
                                    cp.idCalendario_Estado == (byte)idsCalendario_Estado.Absentismo &&
                                    cuad.fecha == cp.fecha
                                )
                            )
                            .GroupBy(x => new { x.idCuadrantePersonal, x.fecha })
                            .Select(x => new DesgloseBalanceCuadrante
                            {
                                idCuadrantePersonal = x.Key.idCuadrantePersonal,
                                registros = x
                                    .Where(j => j.idCuadrantePersonal == x.Key.idCuadrantePersonal)
                                    .Select(x => new RegistroBalance
                                    {
                                        idLavanderia = x.idLavanderia,
                                        jornadaIni = null,
                                        jornadaFin = null,
                                        cuadranteIni = x.horaEntrada,
                                        cuadranteFin = x.horaSalida
                                    })
                                    .First(),
                                minutos = x.Count(),
                                fecha = x.Key.fecha,
                            })
                            .OrderBy(x => x.fecha)
                            .ToDynamicList(),
                    });
        }

        private IQueryable<RegistroInforme> InformeFestivo(List<int> idsPersona, int idLavanderia, DateTime fechaDesde, DateTime fechaHasta)
        {

            var tblFestivo = db.tblCalendarioLavanderia
                .Where(x => 
                    x.idLavanderia == idLavanderia &&
                    x.fecha >= fechaDesde && 
                    x.fecha <= fechaHasta && 
                    x.idCalendario_Estado == (byte)idsCalendario_Estado.Festivo)
                .Select(x => x.fecha)
                .ToList();

            return db.tblPersona
                    .Where(x => idsPersona.Contains(x.idPersona))
                    .Select(pers => new RegistroInforme
                    {
                        idPersona = pers.idPersona,
                        nombreCompleto = pers.nombre + " " + pers.apellidos,
                        registros = pers.tblJornada
                            .Where(jorn => jorn.fecha >= fechaDesde && jorn.fecha <= fechaHasta && tblFestivo.Contains(jorn.fecha))
                            .GroupBy(x => new { x.idJornada, x.fecha })
                            .Select(x => new DesgloseBalanceJornada
                            {
                                idJornada = x.Key.idJornada,
                                registros = x
                                    .Where(j => j.idJornada == x.Key.idJornada)
                                    .Select(x => new RegistroBalance
                                    {
                                        idLavanderia = x.idLavanderia,
                                        jornadaIni = x.horaIni,
                                        jornadaFin = x.horaFin,
                                        cuadranteIni = x.idCuadrantePersonalNavigation.horaEntrada,
                                        cuadranteFin = x.idCuadrantePersonalNavigation.horaSalida
                                    })
                                    .First(),
                                minutos = x.Count(),
                                fecha = x.Key.fecha,
                            })
                            .OrderBy(x => x.fecha)
                            .ToDynamicList(),
                    });
        }

        private IQueryable<RegistroInforme> InformeImpuntualidad(List<int> idsPersona, DateTime fechaDesde, DateTime fechaHasta)
        {
            return db.tblPersona
                    .Where(x => idsPersona.Contains(x.idPersona))
                    .Select(pers => new RegistroInforme
                    {
                        idPersona = pers.idPersona,
                        nombreCompleto = pers.nombre + " " + pers.apellidos,
                        registros = pers.tblJornada
                            .Where(jorn => jorn.fecha >= fechaDesde && jorn.fecha <= fechaHasta && jorn.idMotivoIncumplimiento_horaIni == (int)idsMotivoIncumplimientoJornada.Impuntualidad)
                            .GroupBy(x => new { x.idJornada, x.fecha })
                            .Select(x => new DesgloseBalanceJornada
                            {
                                idJornada = x.Key.idJornada,
                                registros = x
                                    .Where(j => j.idJornada == x.Key.idJornada)
                                    .Select(x => new RegistroBalance
                                    {
                                        idLavanderia = x.idLavanderia,
                                        jornadaIni = x.horaIni,
                                        jornadaFin = x.horaFin,
                                        cuadranteIni = x.idCuadrantePersonalNavigation.horaEntrada,
                                        cuadranteFin = x.idCuadrantePersonalNavigation.horaSalida
                                    })
                                    .First(),
                                minutos = x.Sum(x => (((x.horaIni.Value.Hours)*60) + x.horaIni.Value.Minutes) - ((x.idCuadrantePersonalNavigation.horaEntrada.Value.Hours * 60) + x.idCuadrantePersonalNavigation.horaEntrada.Value.Minutes)),
                                fecha = x.Key.fecha,
                            })
                            .OrderBy(x => x.fecha)
                            .ToDynamicList(),
                    });
        }

        private class RegistroInforme
        {
            public int idPersona { get; set; }
            public string nombreCompleto { get; set; }
            public List<dynamic> registros { get; set; }
        }

        private class DesgloseBalance
        {
            public RegistroBalance? registros { get; set; }
            public int minutos { get; set; }
            public DateTime fecha { get; set; }
        }

        private class DesgloseBalanceJornada : DesgloseBalance
        {
            public int? idJornada { get; set; }
        }

        private class DesgloseBalanceCuadrante : DesgloseBalance
        {
            public int? idCuadrantePersonal { get; set; }
        }

        private class RegistroBalance
        {
            public int? idLavanderia { get; set; }
            public TimeSpan? jornadaIni { get; set; }
            public TimeSpan? jornadaFin { get; set; }
            public TimeSpan? cuadranteIni { get; set; }
            public TimeSpan? cuadranteFin { get; set; }
        }
    }
}
