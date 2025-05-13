using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class.bdERP.General;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCalendarioLavanderiaController : ODataController
{
    private readonly bdERP db;

    public tblCalendarioLavanderiaController(bdERP context)
    {
        db = context;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> fn_IU_tblCalendarioLavanderia([FromODataUri] int idLavanderia, [FromBody] tblCalendarioLavanderiaAgrupado tblCalendarioLavanderia)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        List<tblCalendarioLavanderia> fechasSeleccionadas = new();
        List<tblCuadrantePersonal> cuadrantes = new();

        foreach (var fechas in tblCalendarioLavanderia.fechasCalendario)
        {
            for (var fecha = fechas.fechaDesde; fecha <= fechas.fechaHasta; fecha = fecha.AddDays(1))
            {
                fechasSeleccionadas.Add(new tblCalendarioLavanderia
                {
                    fecha = fecha,
                    idCalendario_Estado = fechas.idCalendario_Estado
                });
            }
        }

        List<tblCalendarioLavanderia> tblCalendarioLavanderia_bd = db.tblCalendarioLavanderia.Where(x => (fechasSeleccionadas.Select(x => x.fecha).Contains(x.fecha)) && (x.idLavanderia == idLavanderia)).ToList();
        List<tblCalendarioLavanderia> removeDays = new List<tblCalendarioLavanderia>();
        List<tblCalendarioLavanderia> addDays = new List<tblCalendarioLavanderia>();
        List<tblCalendarioLavanderia> updateDays = new List<tblCalendarioLavanderia>();

        foreach (var dia in fechasSeleccionadas)
        {
            var entity = tblCalendarioLavanderia_bd.FirstOrDefault(x => x.idLavanderia.Equals(idLavanderia) && x.fecha.Equals(dia.fecha));
            if (entity != null)
            {
                if (dia.idCalendario_Estado == 0)
                {
                    removeDays.Add(entity);
                }
                else
                {
                    removeDays.Add(entity);
                    addDays.Add(new tblCalendarioLavanderia()
                    {
                        fecha = dia.fecha,
                        idCalendario_Estado = dia.idCalendario_Estado,
                        idLavanderia = idLavanderia
                    });
                }
            }
            else if (dia.idCalendario_Estado != 0)
            {
                addDays.Add(new tblCalendarioLavanderia()
                {
                    fecha = dia.fecha,
                    idCalendario_Estado = dia.idCalendario_Estado,
                    idLavanderia = idLavanderia
                });

            }
        }

        db.tblCuadrantePersonal.RemoveRange(cuadrantes);
        db.tblCalendarioLavanderia.RemoveRange(
                db.tblCalendarioLavanderia.Where(x =>
                (x.idLavanderia == idLavanderia)
                && (removeDays.Select(x => x.fecha).Contains(x.fecha)
                && new List<int>() { (byte)idsCalendario_Estado.Festivo, (byte)idsCalendario_Estado.DiaCierre }.Contains(x.idCalendario_Estado)
            ))
        );
        db.tblCalendarioLavanderia.AddRange(addDays);
        db.tblCalendarioLavanderia.UpdateRange(updateDays);

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

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int idLavanderia)
    {
        List<tblCalendarioLavanderia> tblCalendarioLavanderia_bd = db.tblCalendarioLavanderia.Where(x => (x.fecha >= fechaDesde) && (x.fecha <= fechaHasta) && (x.idLavanderia == idLavanderia)).ToList();
        //tblCalendarioLavanderia_bd.AddRange(db.tblCuadrantePersonal.Where(x => (x.fecha >= fechaDesde) && (x.fecha <= fechaHasta) && (x.idLavanderia == idLavanderia)).Select(x => new tblCalendarioLavanderia { fecha = x.fecha, idLavanderia = (int)x.idLavanderia, idCalendario_Estado = 19 }).ToList().GroupBy(x => x.fecha).Select(x => x.First()));

        return Ok(db.tblCalendarioLavanderia.Where(x => (x.fecha >= fechaDesde) && (x.fecha <= fechaHasta) && (x.idLavanderia == idLavanderia)));
    }
}

