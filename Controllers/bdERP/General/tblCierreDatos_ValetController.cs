using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCierreDatos_ValetController : ODataController
{
    private readonly bdERP db;

    public tblCierreDatos_ValetController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblCierreDatos_Valet);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblCierreDatos_Valet cierreDatos)
    {
        try
        {
            db.tblCierreDatos_Valet.Add(cierreDatos);
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
    public async Task<ActionResult> Patch([FromODataUri] int keyidLavanderia, [FromODataUri] short keyaño, [FromODataUri] byte keymes, [FromBody] JsonPatchDocument<tblCierreDatos_Valet> cierreDatos)
    {
        var entity = await db.tblCierreDatos_Valet.FindAsync(keyidLavanderia, keyaño, keymes);
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
        var entity = await db.tblCierreDatos_Valet.FindAsync(keyidLavanderia, keyaño, keymes);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblCierreDatos_Valet.Remove(entity);

        await db.SaveChangesAsync();
        return NoContent();
    }
}
