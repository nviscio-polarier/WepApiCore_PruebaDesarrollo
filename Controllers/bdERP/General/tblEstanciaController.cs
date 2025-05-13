using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblEstanciaController : ODataController
{
    private bdERP db = new bdERP();

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public ActionResult Get([FromODataUri] int idEntidad, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        return Ok(db.tblEstancia.Where(x =>
            x.idEntidad == idEntidad &&
            (x.fecha.Year >= fechaDesde.Year && x.fecha.Month >= fechaDesde.Month && x.fecha.Day >= fechaDesde.Day) &&
            (x.fecha.Year <= fechaHasta.Year && x.fecha.Month <= fechaHasta.Month && x.fecha.Day <= fechaHasta.Day)));
    }

    //[EnableQuery]
    //[HttpPatch]
    //public async Task<ActionResult> Patch([FromODataUri] int idEntidad, [FromODataUri] DateTime key, Delta<tblEstancia> estancia)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    var entity = db.tblEstancia.Where(x => x.idEntidad == idEntidad && (x.fecha.Year == key.Year) && (x.fecha.Month == key.Month) && (x.fecha.Day == key.Day)).FirstOrDefault();
    //    if (entity == null)
    //    {
    //        entity = new tblEstancia() { idEntidad = idEntidad, fecha = key, estanciasPrevistas = 0, estanciasReal = 0, salidas = 0 };
    //        db.tblEstancia.Add(entity);
    //        db.SaveChanges();
    //    }

    //    estancia.Patch(entity);
    //    await db.SaveChangesAsync();
    //    return Updated(entity);
    //}

    //[EnableQuery]
    //[HttpDelete]
    //public async Task<ActionResult> Delete([FromODataUri] DateTime key, [FromODataUri] int idEntidad)
    //{
    //    var entity = db.tblEstancia.Where(x => (x.fecha.Year == key.Year) && (x.fecha.Month == key.Month) && (x.fecha.Day == key.Day) && x.idEntidad == idEntidad).FirstOrDefault();
    //    if (entity != null)
    //    {
    //        db.tblEstancia.Remove(entity);
    //        await db.SaveChangesAsync();
    //        return StatusCode(HttpStatusCode.NoContent);
    //    }
    //    return BadRequest();
    //}
    //protected override void Dispose(bool disposing)
    //{
    //    if (disposing)
    //    {
    //        db.Dispose();
    //    }
    //    base.Dispose(disposing);
    //}

    //private bool tblEstanciaExists(DateTime fecha, int idEntidad)
    //{
    //    return db.tblEstancia.Count(x => x.fecha.Year == fecha.Year && x.fecha.Month == fecha.Month && x.fecha.Day == fecha.Day && x.idEntidad == idEntidad) > 0;
    //}
}
