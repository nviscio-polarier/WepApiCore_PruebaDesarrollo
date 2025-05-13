using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblEmbarcadorController : ODataController
{
    private readonly bdERP db;

    public tblEmbarcadorController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblEmbarcador);
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(await db.tblEmbarcador.FindAsync(key));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblEmbarcador embarcador)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            db.tblEmbarcador.Add(embarcador);
            await db.SaveChangesAsync();

            return Created(embarcador);
        }
        catch
        {
            return BadRequest("Error de BDD");
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblEmbarcador> embarcador)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var entity = await db.tblEmbarcador.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        embarcador.ApplyTo(entity);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!tblEmbarcadorExists(key))
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
        var entity = await db.tblEmbarcador.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblEmbarcador.Remove(entity);

        await db.SaveChangesAsync();
        return NoContent();
    }

    private bool tblEmbarcadorExists(int id)
    {
        return db.tblEmbarcador.Count(e => e.idEmbarcador == id) > 0;
    }
}
