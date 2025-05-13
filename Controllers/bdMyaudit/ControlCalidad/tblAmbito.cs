using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class tblAmbitoController : ODataController
{
    private readonly bdMyAudit db;
    public tblAmbitoController(bdMyAudit context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblAmbito);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblAmbito> ambito)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = db.tblAmbito.FirstOrDefault(x => x.idAmbito == key);

        if (entity == null)
        {
            return NotFound();
        }

        ambito.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Updated(entity);
    }


}
