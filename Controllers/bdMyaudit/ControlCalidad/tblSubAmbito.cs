using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class tblSubAmbitoController : ODataController
{
    private readonly bdMyAudit db;
    public tblSubAmbitoController(bdMyAudit context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblSubAmbito);
    }


    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblSubAmbito> subAmbito)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = db.tblSubAmbito.FirstOrDefault(x => x.idSubAmbito == key);

        if (entity == null)
        {
            return NotFound();
        }

        subAmbito.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Updated(entity);
    }

}
