using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblProyectoController : ODataController
{
    private readonly bdERP db;

    public tblProyectoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    //[Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblProyecto);
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(await db.tblProyecto.FindAsync(key));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblProyecto proyecto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            db.tblProyecto.Add(proyecto);
            await db.SaveChangesAsync();

            return Created(proyecto);
        }
        catch
        {
            return BadRequest("Error de BDD");
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblProyecto> proyecto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var entity = await db.tblProyecto.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        proyecto.ApplyTo(entity);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!tblProyectoExists(key))
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
        var entity = await db.tblProyecto.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblProyecto.Remove(entity);

        await db.SaveChangesAsync();
        return NoContent();
    }

    private bool tblProyectoExists(int id)
    {
        return db.tblProyecto.Count(e => e.idProyecto == id) > 0;
    }
}
