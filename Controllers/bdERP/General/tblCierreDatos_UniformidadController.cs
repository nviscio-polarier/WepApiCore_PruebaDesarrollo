using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCierreDatos_UniformidadController : ODataController
{
    private readonly bdERP db;

    public tblCierreDatos_UniformidadController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblCierreDatos_Uniformidad);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblCierreDatos_Uniformidad cierreDatos)
    {
        try
        {
            tblLavanderia lav = db.tblLavanderia.Find(cierreDatos.idLavanderia);
            cierreDatos.precioUnidad_MyUniform = (decimal)(lav.precioUnidad_MyUniform ?? 0);

            db.tblCierreDatos_Uniformidad.Add(cierreDatos);
            await db.SaveChangesAsync();

            return Created(cierreDatos);
        }
        catch
        {
            return BadRequest("Error de BDD");
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int keyidLavanderia, [FromODataUri] short keyaño, [FromODataUri] byte keymes, [FromBody] JsonPatchDocument<tblCierreDatos_Uniformidad> cierreDatos)
    {
        var entity = await db.tblCierreDatos_Uniformidad.FindAsync(keyidLavanderia, keyaño, keymes);
        if (entity == null)
        {
            return NotFound();
        }

        cierreDatos.ApplyTo(entity);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int keyidLavanderia, [FromODataUri] short keyaño, [FromODataUri] byte keymes)
    {
        var entity = await db.tblCierreDatos_Uniformidad.FindAsync(keyidLavanderia, keyaño, keymes);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblCierreDatos_Uniformidad.Remove(entity);

        await db.SaveChangesAsync();
        return NoContent();
    }
}
