using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblRutaExpedicionController : ODataController
{
    private readonly bdERP db;

    public tblRutaExpedicionController(bdERP context)
    {
        db = context;
    }

    [HttpGet]
    [Authorize]
    [EnableQuery]
    public async Task<ActionResult> Get([FromODataUri] int? idPais)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        List<int> idsLavanderia = db.tblUsuario.Where(x => x.idUsuario.Equals(idUsuario))
            .Select(x => x.idLavanderia.Where(l => idPais == null || (idPais != null && l.idPais.Equals(idPais)))
            .Select(l => l.idLavanderia)).FirstOrDefault().ToList();

        return Ok(db.tblRutaExpedicion.Where(x => idsLavanderia.Contains((int)x.idLavanderia) ||
            x.tblParadaNRutaExpedicion.Count(p => p.idLavanderia != null && idsLavanderia.Contains((int)p.idLavanderia)) > 0));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblRutaExpedicion rutaExpedicion)
    {
        try
        {
            rutaExpedicion.activo = true;
            rutaExpedicion.eliminado = false;

            db.tblRutaExpedicion.Add(rutaExpedicion);
            await db.SaveChangesAsync();

            return Ok(Created(rutaExpedicion).Entity);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblRutaExpedicion> rutaExpedicion)
    {
        var entity = db.tblRutaExpedicion.FirstOrDefault(x => x.idRutaExpedicion.Equals(key));
        if (entity == null)
            return BadRequest();

        rutaExpedicion.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblRutaExpedicion.FindAsync(key);
        if (entity == null)
            return false;

        if (db.tblParteTransporte.Where(x => x.idRutaExpedicion == key).Count() == 0)
        {
            db.RemoveRange(db.tblParadaNRutaExpedicion.Where(x => x.idRutaExpedicion == key));
            db.RemoveRange(db.tblEntidadNRutaExpedicion.Where(x => x.idRutaExpedicion == key));
            await db.SaveChangesAsync();

            db.Remove(entity);
        }
        else
        {
            entity.eliminado = true;
            entity.activo = false;
        }

        await db.SaveChangesAsync();
        return true;
    }
}
