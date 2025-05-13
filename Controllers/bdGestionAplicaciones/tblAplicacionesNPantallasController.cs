using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;

namespace WebApiCore.Controllers;
public class tblAplicacionesNPantallasController : ODataController
{
    private readonly bdGestionAplicaciones db;
    public tblAplicacionesNPantallasController(bdGestionAplicaciones context)
    {
        db = context;
    }

    [HttpPatch]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblAplicacionesNPantallas> aplicacionesNPantallas)
    {
        var entity = db.tblAplicacionesNPantallas.FirstOrDefault(x => x.IdAplicacionesNPantalla.Equals(key));
        if (entity == null)
            return BadRequest();

        aplicacionesNPantallas.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }
}
