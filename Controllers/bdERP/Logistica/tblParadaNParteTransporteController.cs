using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblParadaNParteTransporteController : ODataController
{
    private readonly bdERP db;

    public tblParadaNParteTransporteController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get(int idParteTransporte)
    {
        return Ok(db.tblParadaNParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte)));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblParadaNParteTransporte paradaRuta)
    {
        try
        {
            db.tblParadaNParteTransporte.Add(paradaRuta);
            await db.SaveChangesAsync();

            return Ok(Created(paradaRuta).Entity.idParteTransporte);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblParadaNParteTransporte> paradaRuta)
    {
        var entity = db.tblParadaNParteTransporte.FirstOrDefault(x => x.idParadaNParteTransporte.Equals(key));
        if (entity == null)
            return BadRequest();

        paradaRuta.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblParadaNParteTransporte.FindAsync(key);
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
