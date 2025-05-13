using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;

namespace WebApiCore.Controllers;
public class tblConfigPTMYPRController : ODataController
{
    private readonly bdGestionAplicaciones db;
    public tblConfigPTMYPRController(bdGestionAplicaciones context)
    {
        db = context;
    }

    [HttpPatch]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblConfigPTMYPR> configPTMYPR)
    {
        var entity = db.tblConfigPTMYPR.FirstOrDefault(x => x.IdConfig.Equals(key));
        if (entity == null)
            return BadRequest();

        configPTMYPR.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }
}
