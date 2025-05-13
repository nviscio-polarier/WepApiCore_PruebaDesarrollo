using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblMezclaSucioClienteController : ODataController
{
    private readonly bdERP db;

    public tblMezclaSucioClienteController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get(int key)
    {
        return Ok(db.tblMezclaSucioCliente.Find(key));
    }


    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblMezclaSucioCliente mezclaSucioCliente)
    {
        try
        {
            mezclaSucioCliente.fecha = DateTimeOffset.UtcNow;
            db.tblMezclaSucioCliente.Add(mezclaSucioCliente);
            await db.SaveChangesAsync();

            return Created(mezclaSucioCliente);


        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblMezclaSucioCliente> mezclaSucioCliente)
    {
        var entity = db.tblMezclaSucioCliente.FirstOrDefault(x => x.idMezclaSucioCliente.Equals(key));
        if (entity == null)
            return BadRequest();

        mezclaSucioCliente.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }
}
