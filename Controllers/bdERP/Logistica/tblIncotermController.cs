using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblIncotermController : ODataController
{
    private readonly bdERP db;

    public tblIncotermController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblIncoterm);
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(await db.tblIncoterm.FindAsync(key));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblIncoterm incoterm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            db.tblIncoterm.Add(incoterm);
            await db.SaveChangesAsync();

            return Created(incoterm);
        }
        catch
        {
            return BadRequest("Error de BDD");
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblIncoterm> incoterm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var entity = await db.tblIncoterm.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        incoterm.ApplyTo(entity);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!tblIncotermExists(key))
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
        var entity = await db.tblIncoterm.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblIncoterm.Remove(entity);

        await db.SaveChangesAsync();
        return NoContent();
    }

    private bool tblIncotermExists(int id)
    {
        return db.tblIncoterm.Count(e => e.idIncoterm == id) > 0;
    }
}