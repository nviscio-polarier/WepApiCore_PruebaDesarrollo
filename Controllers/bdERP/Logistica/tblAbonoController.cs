using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblAbonoController : ODataController
{
    private readonly bdERP db;

    public tblAbonoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblAbono);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblAbono abono)
    {
        try
        {
            int idLavanderia = (int)abono.idLavanderia;
            DateTime fechaAbono = (DateTime)abono?.fecha;
            var currentServerDate = DateTimeOffset.UtcNow;

            abono.codigo = abono.codigo ?? new Utils().GenerarCodigoAbono(db);
            abono.fecha = fechaAbono != null ? fechaAbono : currentServerDate.DateTime;
            abono.fechaRegistro = currentServerDate;

            db.tblAbono.Add(abono);

            await db.SaveChangesAsync();

            return Ok(abono);
        }
        catch (Exception ex)
        {
            return BadRequest("Error de BDD: " + ex.Message);
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblAbono> abono)
    {
        var entity = db.tblAbono.Include(x => x.tblPrendaNAbono).FirstOrDefault(x => x.idAbono == key);

        if (entity == null)
            return NotFound();


        var fechaFromBody = abono.Operations.Where(x => x.path == "/fecha").FirstOrDefault();
        if (fechaFromBody != null)
        {
            var fecha = (DateTime)fechaFromBody.value;
            var currentServerDate = DateTimeOffset.UtcNow;
            entity.fecha = fecha;
            entity.fechaRegistro = currentServerDate;
        }

        var operations_Imagen = abono.Operations.Where(x => x.path == "/tblPrendaNAbono").FirstOrDefault();
        if (operations_Imagen != null)
            db.tblPrendaNAbono.RemoveRange(entity.tblPrendaNAbono);

        abono.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Updated(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblAbono.FindAsync(key);
        if (entity == null)
            return false;

        var solicitudAbono = db.tblSolicitudAbono.FirstOrDefault(x => x.idAbono == key);

        if (solicitudAbono != null)
        {
            solicitudAbono.idAbono = null;
            solicitudAbono.idEstadoSolicitudAbono = 1;
        }

        var prendasNAbono = db.tblPrendaNAbono.Where(x => x.idAbono == key);

        if (prendasNAbono.Any())
            db.tblPrendaNAbono.RemoveRange(prendasNAbono);
        db.SaveChanges();
        db.Remove(entity);

        await db.SaveChangesAsync();
        return true;
    }
}
