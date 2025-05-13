using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTipoNomina_MXController : ODataController
{
    private readonly bdERP db;

    public tblTipoNomina_MXController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblTipoNomina_MX>> Get()
    {
        return db.tblTipoNomina_MX;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get([FromODataUri] short key)
    {
        return Ok(await db.tblTipoNomina_MX.FindAsync(key));
    }
}
