using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTipoContenedorController : ODataController
{
    private readonly bdERP db;

    public tblTipoContenedorController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblTipoContenedor);
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(await db.tblTipoContenedor.FindAsync(key));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblTipoContenedor tipoContenedor)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            db.tblTipoContenedor.Add(tipoContenedor);
            await db.SaveChangesAsync();

            return Created(tipoContenedor);
        }
        catch
        {
            return BadRequest("Error de BDD");
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblTipoContenedor> tipoContenedor)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var entity = await db.tblTipoContenedor.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        tipoContenedor.ApplyTo(entity);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!tblTipoContenedorExists(key))
            {
                return NotFound();
            }
            throw;
        }
        return Updated(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var entity = await db.tblTipoContenedor.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblTipoContenedor.Remove(entity);

        await db.SaveChangesAsync();
        return NoContent();
    }

    private bool tblTipoContenedorExists(int id)
    {
        return db.tblTipoContenedor.Count(e => e.idTipoContenedor == id) > 0;
    }
}