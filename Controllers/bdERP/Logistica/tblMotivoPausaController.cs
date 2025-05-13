using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblMotivoPausaController : ODataController
{
    private readonly bdERP db;

    public tblMotivoPausaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblMotivoPausa);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblMotivoPausa motivoPausa)
    {
        try
        {
            db.tblMotivoPausa.Add(motivoPausa);
            await db.SaveChangesAsync();

            return Ok(Created(motivoPausa).Entity.idMotivoPausa);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblMotivoPausa> motivoPausa)
    {
        var entity = db.tblMotivoPausa.FirstOrDefault(x => x.idMotivoPausa.Equals(key));
        if (entity == null)
            return BadRequest();

        motivoPausa.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblMotivoPausa.FindAsync(key);
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
