using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;
public class tblInventarioController : ODataController
{
    private readonly bdERP db;
    public tblInventarioController(bdERP context)
    {
        db = context;
    }
    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblInventario> inventario)
    {
        var entity = await db.tblInventario.FindAsync(key);
        if (entity == null)
            return NotFound();
        inventario.ApplyTo(entity);
        await db.SaveChangesAsync();
        return Updated(entity);
    }
}