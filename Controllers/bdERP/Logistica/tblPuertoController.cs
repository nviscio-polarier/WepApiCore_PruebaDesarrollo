using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPuertoController : ODataController
{
    private readonly bdERP db;

    public tblPuertoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblPuerto);
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(await db.tblPuerto.FindAsync(key));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblPuerto puerto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            db.tblPuerto.Add(puerto);
            await db.SaveChangesAsync();

            return Created(puerto);
        }
        catch
        {
            return BadRequest("Error de BDD");
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPuerto> puerto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var entity = await db.tblPuerto.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        puerto.ApplyTo(entity);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!tblPuertoExists(key))
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
        var entity = await db.tblPuerto.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblPuerto.Remove(entity);

        await db.SaveChangesAsync();
        return NoContent();
    }

    private bool tblPuertoExists(int id)
    {
        return db.tblPuerto.Count(e => e.idPuerto == id) > 0;
    }
}