using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblJornadaPersonaController : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblJornadaPersonaController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int? idLavanderia)
    {
        return Ok(db.tblJornadaPersona.Where(x => x.idLavanderia.Equals(idLavanderia)));
    }

    [EnableQuery]
    [HttpGet("odata/tblJornadaPersona/GetPersonas")]
    [Authorize]
    public ActionResult Get([FromODataUri] DateTime fecha, [FromODataUri] int? idLavanderia, [FromODataUri] int? idCategoriaInterna, [FromODataUri] int? idTurno)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var objUsuario = db.tblUsuario
        .Where(x => x.idUsuario == idUsuario && !x.isEliminado)
         .Select(x => new
         {
             x.idCategoriaInterna
         })
        .FirstOrDefault();

        if (objUsuario == null)
            return BadRequest();

        var hasUsuarioCategorias = objUsuario.idCategoriaInterna.Count > 0;

        var idsCategoriasNUsuario = from
                                       cat in db.tblCategoriaInterna
                                    where
                                       (idCategoriaInterna == null && ((hasUsuarioCategorias && cat.idUsuario.Select(x => x.idUsuario).Contains(idUsuario)) || !hasUsuarioCategorias)) ||
                                       (idCategoriaInterna != null && cat.idCategoriaInterna == idCategoriaInterna)
                                    select new
                                    { cat.idCategoriaInterna };

        var idsLavanderiaEspaña = from
                                    lav in db.tblLavanderia
                                  where
                                    lav.idPais == 1 && lav.enableControlHorario == true && lav.idUsuario.Select(x => x.idUsuario).Contains(idUsuario)
                                  select new
                                  { lav.idLavanderia };
        var idsTurnosEspaña = from
                                tur in db.tblTurno
                              join
                                lav in idsLavanderiaEspaña on tur.idLavanderia equals lav.idLavanderia
                              select new
                              { tur.idTurno };

        var datosPersonas = from persona in db.tblPersona
                            .Where(x =>
                                x.activo && !x.eliminado &&
                                (x.idCategoriaInternaNavigation.idCategoriaConvenioNavigation.isOficina == false || x.idCategoriaInternaNavigation == null) &&
                                ((idTurno != null && idTurno != -1 && (x.idTurno == idTurno || x.idTurnoNavigation.idTurnoPadre == idTurno)) || idTurno == null ||
                                (idTurno == -1 && x.idTurno == null))
                            )
                            join catUsuarioPersona in idsCategoriasNUsuario on persona.idCategoriaInterna equals catUsuarioPersona.idCategoriaInterna into _catUsuarioPersona
                            from catUsuarioPersona in _catUsuarioPersona.DefaultIfEmpty()
                            join idsLavanderiaEspañaPersona in idsLavanderiaEspaña on persona.idLavanderia equals idsLavanderiaEspañaPersona.idLavanderia
                            join idsTurnosEspañaPersona in idsTurnosEspaña on persona.idTurno equals idsTurnosEspañaPersona.idTurno into _idsTurnosEspañaPersona
                            from idsTurnosEspañaPersona in _idsTurnosEspañaPersona.DefaultIfEmpty()
                            where catUsuarioPersona.idCategoriaInterna != null && (persona.idTurno == null || idsTurnosEspañaPersona.idTurno == persona.idTurno)
                            select persona;

        var personasJornada = from persona in datosPersonas
                              join cuadrante in db.tblCuadrantePersonal
                              on new { idPersona = persona.idPersona, fecha } equals new { cuadrante.idPersona, cuadrante.fecha } into _cuadrante
                              from cuadrante in _cuadrante.DefaultIfEmpty()
                              join jornada in db.tblJornadaPersona
                              on new { persona.idPersona, fecha } equals new { jornada.idPersona, jornada.fecha }
                              into _jornada
                              from jornada in _jornada.DefaultIfEmpty()
                              from turno in db.tblTurno
                              join calendario in db.tblCalendarioPersonal on new { fecha, persona.idPersona } equals new { calendario.fecha, calendario.idPersona } into _calendario
                              from calendario in _calendario.DefaultIfEmpty()
                              join estadoCalendario in db.tblCalendario_Estado
                              on calendario.idCalendario_Estado equals estadoCalendario.idCalendario_Estado into _estadoCalendario
                              from estadoCalendario in _estadoCalendario.DefaultIfEmpty()
                              join pntc in db.tblPersonaNTipoContrato on persona.idPersona equals pntc.idPersona
                              from catGrupo in db.tblCategoria_Grupo
                              where
                                ((jornada.idPersona == null && (pntc.fechaAltaContrato <= fecha && (pntc.fechaBajaContrato == null || pntc.fechaBajaContrato >= fecha))) ||
                                jornada.idPersona != null) &&
                                (calendario.idCalendario_Estado != 3 && calendario.idCalendario_Estado != 4) &&
                                (
                                    (idLavanderia != null &&
                                        (
                                            jornada.idPersona == null && // Persona sin Jornada filtrada por lav persona
                                            persona.idLavanderia == idLavanderia
                                        ) ||
                                        (jornada.idPersona != null && jornada.idLavanderia == idLavanderia) // Tiene dia trabajado y la jornada es de la lavanderia seleccionada
                                    ) ||
                                    idLavanderia == null
                                )
                              select new
                              {
                                  idPersona = persona.idPersona,
                                  nombre = persona.nombre,
                                  apellidos = persona.apellidos,
                                  idTipoTrabajo = persona.idTipoTrabajo,
                                  idTurno = jornada.idPersona != null ? jornada.idTurno : persona.idTurno,
                                  idLavanderiaOrigen = persona.idLavanderia,
                                  horaEntradaParam = cuadrante.horaEntrada,
                                  horaSalidaParam = cuadrante.horaSalida,
                                  tiempoDescansoParam = persona.idTurnoNavigation != null ? (TimeSpan?)persona.idTurnoNavigation.descanso : null,
                                  horasDiarias = persona.horasDiarias,
                                  tblCalendarioPersonal = calendario.idPersona == null ? null : new
                                  {
                                      idCalendario_Estado = estadoCalendario.idCalendario_Estado,
                                      traduccion = estadoCalendario.traduccion,
                                      colorHexa = estadoCalendario.colorHexa
                                  },
                                  tblJornadaPersona = jornada.idPersona == null ? null : new
                                  {
                                      fecha = jornada.fecha,
                                      horaEntrada = jornada.horaEntrada != null ? jornada.fecha + jornada.horaEntrada : null,
                                      horaSalida = jornada.horaSalida != null ? jornada.fecha + jornada.horaSalida : null,
                                      idLavanderia = jornada.idLavanderia,
                                      idTipoTrabajo = persona.idTipoTrabajo,
                                      isRegManual = jornada.isRegManual,
                                      isRegValido = jornada.isRegValido,
                                      isRevisado_horaEntrada = jornada.isRevisado_horaEntrada,
                                      isRevisado_horaSalida = jornada.isRevisado_horaSalida,
                                      isRevisado_tiempoDescanso = jornada.isRevisado_tiempoDescanso,
                                      reg_horaEntrada = jornada.reg_horaEntrada,
                                      reg_horaFinDescanso = jornada.reg_horaFinDescanso,
                                      reg_horaInicioDescanso = jornada.reg_horaInicioDescanso,
                                      reg_horaSalida = jornada.reg_horaSalida,
                                      tiempoDescanso = jornada.tiempoDescanso != null ? jornada.fecha + jornada.tiempoDescanso : null,
                                      tiempoTipoTrabajo1 = jornada.fecha + jornada.tiempoTipoTrabajo1,
                                      tiempoTipoTrabajo2 = jornada.fecha + jornada.tiempoTipoTrabajo2,
                                      tiempoTipoTrabajo3 = jornada.fecha + jornada.tiempoTipoTrabajo3,
                                      tiempoTipoTrabajo4 = jornada.fecha + jornada.tiempoTipoTrabajo4,
                                      tiempoTipoTrabajo5 = jornada.fecha + jornada.tiempoTipoTrabajo5,
                                      horaEntrada_turno = jornada.horaEntrada_turno,
                                      horaSalida_turno = jornada.horaSalida_turno,
                                      descanso_turno = jornada.descanso_turno
                                  }
                              };
        return Ok(personasJornada.Distinct());
    }

    [EnableQuery]
    [HttpPost("odata/tblJornadaPersona/PostMasivo")]
    [Authorize]
    public async Task<ActionResult> PostMasivo([FromBody] List<tblJornadaPersona> jornada)
    {
        try
        {
            #region Eliminación de registros de calendario persona.
            foreach (tblJornadaPersona objJornada in jornada)
            {
                var entity = await db.tblCalendarioPersonal.FindAsync(objJornada.fecha.Date, objJornada.idPersona);
                if (entity != null)
                {
                    db.tblCalendarioPersonal.Remove(entity);
                }
            }
            #endregion

            db.tblJornadaPersona.AddRange(jornada);
            await db.SaveChangesAsync();

            var fecha = jornada.FirstOrDefault().fecha.ToString("yyyy-MM-dd");
            var idLavanderia = jornada.FirstOrDefault().idLavanderia;
            _hubContext.Clients.Group("JornadaPersona_" + idLavanderia + "_" + fecha).SendAsync("JornadaPersona/signalR_refresh");

            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest("Error de DB");
        }
    }

    [EnableQuery]
    [HttpPatch("odata/tblJornadaPersona/{idPersona}/{fecha}")]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int idPersona, [FromODataUri] DateTime fecha, [FromBody] JsonPatchDocument<tblJornadaPersona> jornada)
    {
        try
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
            if (objUsuario == null)
                return BadRequest();

            var entity = db.tblJornadaPersona.Where(x => x.idPersona == idPersona && x.fecha == fecha).FirstOrDefault();

            jornada.ApplyTo(entity);

            var operationHoraEntrada = jornada.Operations.Where(x => x.path.Equals("/horaEntrada"));
            if (operationHoraEntrada.Count() > 0)
            {
                if (operationHoraEntrada.FirstOrDefault().value != null)
                {
                    entity.isRevisado_horaEntrada = true;
                }
                else
                {
                    entity.isRevisado_horaEntrada = null;
                    entity.isRegValido = false;
                    entity.tiempoTipoTrabajo1 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo2 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo3 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo4 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo5 = TimeSpan.Zero;
                }
            }

            var operationHoraSalida = jornada.Operations.Where(x => x.path.Equals("/horaSalida"));
            if (operationHoraSalida.Count() > 0)
            {
                if (operationHoraSalida.FirstOrDefault().value != null)
                {
                    entity.isRevisado_horaSalida = true;
                }
                else
                {
                    entity.isRevisado_horaSalida = null;
                    entity.isRegValido = false;
                    entity.tiempoTipoTrabajo1 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo2 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo3 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo4 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo5 = TimeSpan.Zero;
                }
            }

            var operationTiempoDescanso = jornada.Operations.Where(x => x.path.Equals("/tiempoDescanso"));
            if (operationTiempoDescanso.Count() > 0)
            {
                if (operationTiempoDescanso.FirstOrDefault().value != null)
                {
                    entity.isRevisado_tiempoDescanso = true;
                }
                else
                {
                    entity.isRevisado_tiempoDescanso = null;
                    entity.isRegValido = false;
                    entity.tiempoTipoTrabajo1 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo2 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo3 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo4 = TimeSpan.Zero;
                    entity.tiempoTipoTrabajo5 = TimeSpan.Zero;
                }
            }

            var operationValido = jornada.Operations.Where(x => x.path.Equals("/isRegValido"));
            if (operationValido.Count() > 0)
            {
                entity.isRevisado_horaEntrada = entity.isRevisado_horaSalida = entity.isRevisado_tiempoDescanso = true;
            }

            await db.SaveChangesAsync();

            var idLavanderia = entity.idLavanderia;
            _hubContext.Clients.Group("JornadaPersona_" + idLavanderia + "_" + fecha.ToString("yyyy-MM-dd")).SendAsync("JornadaPersona/signalR_refresh");

            return Ok(true);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException;
            return Ok(error);
        }
    }

    [EnableQuery]
    [HttpDelete("odata/tblJornadaPersona/{idPersona}/{fecha}")]
    [Authorize]
    public async Task<bool> Delete(int idPersona, DateTime fecha)
    {
        var entity = await db.tblJornadaPersona.FindAsync(idPersona, fecha);
        if (entity == null)
        {
            return false;
        }
        db.tblJornadaPersona.Remove(entity);
        await db.SaveChangesAsync();

        var idLavanderia = entity.idLavanderia;
        _hubContext.Clients.Group("JornadaPersona_" + idLavanderia + "_" + fecha.ToString("yyyy-MM-dd")).SendAsync("JornadaPersona/signalR_refresh");

        return true;
    }
}