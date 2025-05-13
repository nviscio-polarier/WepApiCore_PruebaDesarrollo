using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApiCore.Context;
using WebApiCore.Security;
using static WebApiCore.Controllers.tblMovimientoRecambioController;

namespace WebApiCore.Controllers;

[Authorize]
public class tblRecambioController : ODataController
{
    private readonly bdERP db;

    public tblRecambioController(bdERP context)
    {
        db = context;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<IQueryable<tblRecambio>> Get()
    {
        return db.tblRecambio;
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblRecambio recambio)
    {
        db.tblRecambio.Add(recambio);

        await db.SaveChangesAsync();

        return Created(recambio);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblRecambio> recambio)
    {
        var entity = db.tblRecambio
            .Include(r => r.tblRecambioNAlmacenRecambios)
            .Include(r => r.tblRecambioNProveedor)
            .FirstOrDefault(r => r.idRecambio == key);

        if (entity == null)
        {
            return NotFound();
        }

        if (recambio.Operations.Any(x => x.path == "/tblRecambioNAlmacenRecambios"))
        {
            db.tblRecambioNAlmacenRecambios.RemoveRange(entity.tblRecambioNAlmacenRecambios);
        }

        if (recambio.Operations.Any(x => x.path == "/tblRecambioNProveedor"))
        {
            db.tblRecambioNProveedor.RemoveRange(entity.tblRecambioNProveedor);
        }

        recambio.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Updated(recambio);
    }

    [HttpDelete]
    [EnableQuery]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var recambio = db.tblRecambio.Find(key);

        if (recambio == null)
        {
            return NotFound();
        }

        recambio.eliminado = true;

        await db.SaveChangesAsync();

        return Ok(true);
    }
}
