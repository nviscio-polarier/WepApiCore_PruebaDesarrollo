using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Data;
using WebApiCore.Class.externos;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
[ExternalAuth]
public class ExternosControlller : ODataController
{
    private readonly bdERP db;

    public ExternosControlller(bdERP context)
    {
        db = context;
    }

    private readonly Dictionary<string, int> tablaConversion = new()
    {
        { "58206a60157cd214a475ad39", 660 }, //Ikos Portopetro
    };

    [EnableQuery]
    [HttpGet("Externos/getEstancias/{idEntidad}")]
    public async Task<ActionResult> getEstancias(string idEntidad)
    {
        int idEnt = tablaConversion[idEntidad];
        return Ok(db.tblEstancia.Where(x => x.idEntidad.Equals(idEnt)).Select(x => new { x.fecha, x.estanciasReal }));
    }

    [EnableQuery]
    [HttpPost("Externos/setEstancias")]
    public async Task<ActionResult> setEstancias([FromBody] List<Estancias> estancias)
    {
        try
        {
            db.tblLogError.Add(new tblLogError()
            {
                denominacion = "Externos/setEstancias",
                error = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")
            });

            await db.SaveChangesAsync();

            List<int> idsEntidad = estancias.Select(x => tablaConversion[x.idEntidad]).Distinct().ToList();
            var cierres = db.tblCierreFactEntidad.Where(cierre => idsEntidad.Contains(cierre.idEntidad)).ToList();

            foreach (Estancias estancia in estancias)
            {
                int idEntidad = tablaConversion[estancia.idEntidad];

                foreach (tblEstancia est in estancia.estancias)
                {
                    est.idEntidad = idEntidad;

                    bool diaCerrado = cierres.Where(x => x.idEntidad.Equals(est.idEntidad) && x.fechaDesde.Date <= est.fecha.Date && x.fechaHasta.Date >= est.fecha.Date).Count() > 0;
                    if (!diaCerrado)
                    {
                        var est_delete = db.tblEstancia.Find(idEntidad, est.fecha);
                        if (est_delete != null)
                        {
                            db.tblEstancia.Remove(est_delete);
                        }
                        db.tblEstancia.Add(est);
                    }
                }
            }

            await db.SaveChangesAsync();

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}

