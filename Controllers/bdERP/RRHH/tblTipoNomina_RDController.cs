using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTipoNomina_RDController : ODataController
{
    private readonly bdERP db;

    public tblTipoNomina_RDController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblTipoNomina_RD>> Get()
    {
        return db.tblTipoNomina_RD;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get([FromODataUri] short key)
    {
        return Ok(await db.tblTipoNomina_RD.FindAsync(key));
    }
}
