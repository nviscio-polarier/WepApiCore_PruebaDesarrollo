using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class tblPuntoRevisionController : ODataController
{
    private readonly bdMyAudit db;
    public tblPuntoRevisionController(bdMyAudit context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblPuntoRevision);
    }


    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPuntoRevision> puntoRevision)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = db.tblPuntoRevision.FirstOrDefault(x => x.idPuntoRevision == key);

        if (entity == null)
        {
            return NotFound();
        }

        puntoRevision.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Updated(entity);
    }

}
