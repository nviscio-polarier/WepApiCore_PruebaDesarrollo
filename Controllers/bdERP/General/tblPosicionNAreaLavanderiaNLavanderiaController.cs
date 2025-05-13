using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPosicionNAreaLavanderiaNLavanderiaController : ODataController
{
    private readonly bdERP db;

    public tblPosicionNAreaLavanderiaNLavanderiaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblPosicionNAreaLavanderiaNLavanderia);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPosicionNAreaLavanderiaNLavanderia> posicionPatchDocument)
    {

        tblPosicionNAreaLavanderiaNLavanderia posicion = db.tblPosicionNAreaLavanderiaNLavanderia.Where(x => x.idPosicionNAreaLavanderiaNLavanderia == key).First();

        if (posicion == null) { return BadRequest(); }

        posicionPatchDocument.ApplyTo(posicion);
        await db.SaveChangesAsync();

        return Ok(posicion);
    }
}
