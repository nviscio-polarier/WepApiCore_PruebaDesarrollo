using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblFotoNArticuloEnvioController : ODataController
{
    private readonly bdERP db;

    public tblFotoNArticuloEnvioController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post(tblFotoNArticuloEnvio foto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        db.tblFotoNArticuloEnvio.Add(foto);
        await db.SaveChangesAsync();
        return Created(foto);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int keyidFoto, [FromODataUri] int keyidArticuloEnvio)
    {
        var entity = await db.tblFotoNArticuloEnvio.FindAsync(keyidFoto, keyidArticuloEnvio);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblFotoNArticuloEnvio.Remove(entity);

        await db.SaveChangesAsync();
        return NoContent();
    }
}