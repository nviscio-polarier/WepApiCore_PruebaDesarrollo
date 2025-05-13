using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class.bdERP.RRHH;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCalendarioPersonalController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;

    public tblCalendarioPersonalController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] bool todos = false)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        tblPersona objPersona = db.tblPersona.Where(x => x.idPersona == objUsuario.idPersona).FirstOrDefault();
        if (objPersona == null)
            return BadRequest();

        var items =
            (from x in db.tblCalendarioPersonal
             where x.idPersona == objPersona.idPersona
             select new
             {
                 x.idPersona,
                 fecha = (DateTimeOffset)x.fecha,
                 idCalendario_Estado = (int)x.idCalendario_Estado,
                 isLavanderia = false,
                 horaEntrada = "",
                 horaSalida = "",
                 idTurno = ""
             })
              .Concat
              (from y in db.tblJornadaPersona
               where y.idPersona == objPersona.idPersona
               select new
               {
                   y.idPersona,
                   fecha = (DateTimeOffset)y.fecha,
                   idCalendario_Estado = 3,
                   isLavanderia = false,
                   horaEntrada = y.horaEntrada.ToString(),
                   horaSalida = y.horaSalida.ToString(),
                   idTurno = y.idTurno.ToString()
               })
              .Concat
              (from l in db.tblCalendarioLavanderia
               where l.idLavanderia == objPersona.idLavanderia && l.idCalendario_Estado == (byte)idsCalendario_Estado.Festivo
               select new { idPersona = objPersona.idPersona, fecha = (DateTimeOffset)l.fecha, idCalendario_Estado = (int)l.idCalendario_Estado, isLavanderia = true, horaEntrada = "", horaSalida = "", idTurno = "" })
              .ToList();

        var groups = (from t in items
                      group t by t.fecha into grupo
                      orderby grupo.Key
                      select grupo);

        List<tblTurno> listTurnos = db.tblTurno.ToList();
        List<object> returnItems = new List<object>();
        foreach (var grupo in groups)
        {
            if (grupo.Count() == 1)
            {
                returnItems.AddRange(from x in grupo
                                     select new
                                     {
                                         fecha = x.fecha.UtcDateTime,
                                         x.idCalendario_Estado,
                                         horaEntrada = formatTimeSpan(x.horaEntrada),
                                         horaSalida = formatTimeSpan(x.horaSalida),
                                         turno = x.idTurno == null ? "Personalizado" : x.idTurno != "" ? listTurnos.First(t => t.idTurno == int.Parse(x.idTurno)).denominacion : null
                                     });
            }
            else
            {
                bool isFestivo = (from x in grupo where x.idCalendario_Estado == 8 select x).Count() > 0;
                bool isTrabajado = (from x in grupo where x.idCalendario_Estado == 3 select x).Count() > 0;
                var eventoTrabajador = (from x in grupo where x.isLavanderia == false select x).FirstOrDefault();

                returnItems.Add(new
                {
                    fecha = grupo.Key.UtcDateTime,
                    idCalendario_Estado = isFestivo && isTrabajado ? 4 : eventoTrabajador.idCalendario_Estado,
                    horaEntrada = formatTimeSpan(eventoTrabajador.horaEntrada),
                    horaSalida = formatTimeSpan(eventoTrabajador.horaSalida),
                    turno = eventoTrabajador.idTurno == null ? "Personalizado" : eventoTrabajador.idTurno != "" ? listTurnos.First(t => t.idTurno == int.Parse(eventoTrabajador.idTurno)).denominacion : null
                });
            }
        }

        return Ok(returnItems);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> fn_IU_tblCalendarioPersonal([FromODataUri] int idPersona, [FromBody] tblCalendarioPersonalAgrupado tblCalendarioPersonal)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        List<tblCalendarioPersonal> fechasSeleccionadas = new List<tblCalendarioPersonal>();

        foreach (var fechas in tblCalendarioPersonal.fechasCalendario)
        {
            for (var fecha = fechas.fechaDesde; fecha <= fechas.fechaHasta; fecha = fecha.AddDays(1))
            {
                fechasSeleccionadas.Add(new tblCalendarioPersonal
                {
                    fecha = fecha,
                    idCalendario_Estado = fechas.idCalendario_Estado
                });
            }
        }

        List<tblCalendarioPersonal> tblCalendarioPersonal_bd = db.tblCalendarioPersonal.Where(x => (fechasSeleccionadas.Select(x => x.fecha).Contains(x.fecha)) && (x.idPersona == idPersona)).ToList();
        List<tblCalendarioPersonal> removeDays = new List<tblCalendarioPersonal>();
        List<tblCalendarioPersonal> addDays = new List<tblCalendarioPersonal>();
        List<tblCalendarioPersonal> updateDays = new List<tblCalendarioPersonal>();

        foreach (var dia in fechasSeleccionadas)
        {
            if (dia.idCalendario_Estado == 255) //Máx tipo byte - codigo para eliminar jornada
            {
                tblJornadaPersona jornada = db.tblJornadaPersona.Where(x => x.idPersona == idPersona && x.fecha == dia.fecha).FirstOrDefault();
                if (jornada != null)
                    db.tblJornadaPersona.Remove(jornada);
            }
            else
            {
                var entity = tblCalendarioPersonal_bd.FirstOrDefault(x => x.idPersona.Equals(idPersona) && x.fecha.Equals(dia.fecha));
                if (entity != null)
                {
                    if (dia.idCalendario_Estado == 0)
                    {
                        removeDays.Add(entity);
                    }
                    else
                    {
                        var cuadrante = db.tblCuadrantePersonal.FirstOrDefault(x => x.idPersona == idPersona && x.fecha == dia.fecha);
                        // Editar estado del cuadrante si existe
                        if (cuadrante != null)
                        {
                            cuadrante.idCalendario_Estado = dia.idCalendario_Estado;
                        }
                        entity.idCalendario_Estado = dia.idCalendario_Estado;
                        updateDays.Add(entity);
                    }
                }
                else if (dia.idCalendario_Estado != 0)
                {
                    addDays.Add(new tblCalendarioPersonal()
                    {
                        fecha = dia.fecha,
                        idCalendario_Estado = dia.idCalendario_Estado,
                        idPersona = idPersona
                    });
                }
            }
        }

        db.tblCalendarioPersonal.RemoveRange(
            db.tblCalendarioPersonal.Where(x => (x.idPersona == idPersona) && (removeDays.Select(x => x.fecha).Contains(x.fecha)))
        );
        db.tblCalendarioPersonal.AddRange(addDays);
        db.tblCalendarioPersonal.UpdateRange(updateDays);

        await db.SaveChangesAsync();

        return Ok(true);
    }

    public string formatTimeSpan(string timeSpan)
    {
        if (timeSpan == null || timeSpan.Length == 0)
        {
            return null;
        }
        else
        {
            TimeSpan ts = TimeSpan.Parse(timeSpan);
            var hours = ts.Hours < 10 ? ("0" + ts.Hours) : ts.Hours.ToString();
            var minutes = ts.Minutes < 10 ? ("0" + ts.Minutes) : ts.Minutes.ToString();
            return hours + ":" + minutes;
        }
    }

    [EnableQuery]
    [HttpDelete("odata/tblCalendarioPersonal/{idPersona}/{fecha}")]
    [Authorize]
    public async Task<bool> Delete(int idPersona, DateTime fecha)
    {
        var entity = await db.tblCalendarioPersonal.FindAsync(fecha, idPersona);
        if (entity == null)
        {
            return false;
        }
        db.tblCalendarioPersonal.Remove(entity);
        await db.SaveChangesAsync();

        var entityPersona = await db.tblPersona.FindAsync(idPersona);
        if (entityPersona != null)
        {
            var idLavanderia = entityPersona.idLavanderia;
            _hubContext.Clients.Group("JornadaPersona_" + idLavanderia + "_" + fecha.ToString("yyyy-MM-dd")).SendAsync("JornadaPersona/signalR_refresh");
        }

        return true;
    }
}

