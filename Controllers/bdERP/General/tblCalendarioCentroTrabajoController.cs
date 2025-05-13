using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class.bdERP.General;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCalendarioCentroTrabajoController : ODataController
{
    private readonly bdERP db;

    public tblCalendarioCentroTrabajoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int idCentroTrabajo, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        return Ok(db.tblCalendarioCentroTrabajo.Where(x => x.idCentroTrabajo == idCentroTrabajo &&
            (x.fecha.Date >= fechaDesde.Date &&
            x.fecha.Date <= fechaHasta.Date)));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> fn_IU_tblCalendarioCentroTrabajo([FromODataUri] int idCentroTrabajo, [FromBody] tblCalendarioCentroTrabajoAgrupado tblCalendarioCentroTrabajo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        List<tblCalendarioCentroTrabajo> fechasSeleccionadas = new List<tblCalendarioCentroTrabajo>();

        foreach (var fechas in tblCalendarioCentroTrabajo.fechasCalendario)
        {
            for (var fecha = fechas.fechaDesde; fecha <= fechas.fechaHasta; fecha = fecha.AddDays(1))
            {
                fechasSeleccionadas.Add(new tblCalendarioCentroTrabajo
                {
                    fecha = fecha,
                    idCalendario_Estado = fechas.idCalendario_Estado
                });
            }
        }

        List<tblCalendarioCentroTrabajo> tblCalendarioCentroTrabajo_bd = db.tblCalendarioCentroTrabajo.Where(x => (fechasSeleccionadas.Select(x => x.fecha).Contains(x.fecha)) && (x.idCentroTrabajo == idCentroTrabajo)).ToList();
        List<tblCalendarioCentroTrabajo> removeDays = new List<tblCalendarioCentroTrabajo>();
        List<tblCalendarioCentroTrabajo> addDays = new List<tblCalendarioCentroTrabajo>();
        List<tblCalendarioCentroTrabajo> updateDays = new List<tblCalendarioCentroTrabajo>();

        foreach (var dia in fechasSeleccionadas)
        {
            var entity = tblCalendarioCentroTrabajo_bd.FirstOrDefault(x => x.idCentroTrabajo.Equals(idCentroTrabajo) && x.fecha.Equals(dia.fecha));
            if (entity != null)
            {
                if (dia.idCalendario_Estado == 0)
                {
                    removeDays.Add(entity);
                }
                else
                {
                    removeDays.Add(entity);
                    addDays.Add(new tblCalendarioCentroTrabajo()
                    {
                        fecha = dia.fecha,
                        idCalendario_Estado = dia.idCalendario_Estado,
                        idCentroTrabajo = idCentroTrabajo
                    });
                }
            }
            else if (dia.idCalendario_Estado != 0)
            {
                addDays.Add(new tblCalendarioCentroTrabajo()
                {
                    fecha = dia.fecha,
                    idCalendario_Estado = dia.idCalendario_Estado,
                    idCentroTrabajo = idCentroTrabajo
                });
            }
        }

        db.tblCalendarioCentroTrabajo.RemoveRange(
            db.tblCalendarioCentroTrabajo.Where(x => (x.idCentroTrabajo == idCentroTrabajo) && (removeDays.Select(x => x.fecha).Contains(x.fecha)))
        );
        db.tblCalendarioCentroTrabajo.AddRange(addDays);
        db.tblCalendarioCentroTrabajo.UpdateRange(updateDays);

        await db.SaveChangesAsync();

        return Ok(true);
    }

    public string formatTimeSpan(string timeSpan)
    {
        if (timeSpan.Length == 0)
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
}

