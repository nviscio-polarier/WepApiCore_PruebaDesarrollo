using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPlantillaTareaMantenimientoPrevController : ODataController
{
    private readonly bdERP db;

    public tblPlantillaTareaMantenimientoPrevController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblPlantillaTareaMantenimientoPrev);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblPlantillaTareaMantenimientoPrev plantilla)
    {
        try
        {
            db.tblPlantillaTareaMantenimientoPrev.Add(plantilla);

            if (plantilla.tblTareaMantenimientoPrev != null)
            {
                foreach (var tarea in plantilla.tblTareaMantenimientoPrev)
                {
                    db.tblTareaMantenimientoPrev.Add(tarea);
                }
            }

            await db.SaveChangesAsync();
            return Created(plantilla);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPlantillaTareaMantenimientoPrev> plantilla)
    {
        var entity = db.tblPlantillaTareaMantenimientoPrev.FirstOrDefault(x => x.idPlantillaTareaMantenimientoPrev == key);

        if (entity == null)
        {
            return NotFound();
        }

        plantilla.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Updated(entity);
    }
}
