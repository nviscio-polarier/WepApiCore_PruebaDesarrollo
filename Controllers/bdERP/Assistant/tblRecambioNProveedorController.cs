using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

[Authorize]
public class tblRecambioNProveedorController : ODataController
{
    private readonly bdERP db;

    public tblRecambioNProveedorController(bdERP context)
    {
        db = context;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<IQueryable<tblRecambioNProveedor>> Get([FromODataUri] int idRecambio)
    {
        return db.tblRecambioNProveedor.Where(x => x.idRecambio == idRecambio);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblRecambioNProveedor recNProv)
    {
        db.tblRecambioNProveedor.Add(recNProv);

        await db.SaveChangesAsync();

        return Ok(1);
    }

}
