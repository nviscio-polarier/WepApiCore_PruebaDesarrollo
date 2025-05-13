using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTipoContratoController : ODataController
{
    private readonly bdERP db;

    public tblTipoContratoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblTipoContrato>> Get()
    {
        return db.tblTipoContrato;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get([FromODataUri] short key)
    {
        return Ok(await db.tblTipoContrato.FindAsync(key));
    }
}
