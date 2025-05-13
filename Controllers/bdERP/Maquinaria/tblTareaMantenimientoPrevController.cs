using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTareaMantenimientoPrevController : ODataController
{
    private readonly bdERP db;

    public tblTareaMantenimientoPrevController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblTareaMantenimientoPrev);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblTareaMantenimientoPrev tarea)
    {
        try
        {
            db.tblTareaMantenimientoPrev.Add(tarea);
            await db.SaveChangesAsync();
            return Created(tarea);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblTareaMantenimientoPrev> tarea)
    {
        var entity = db.tblTareaMantenimientoPrev.FirstOrDefault(x => x.idTareaMantenimientoPrev == key);

        if (entity == null)
        {
            return NotFound();
        }

        tarea.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Updated(entity);
    }
}
