using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCantidadNMovimientoElemLogController : ODataController
{
    private readonly bdERP db;

    public tblCantidadNMovimientoElemLogController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblCantidadNMovimientoElemLog);
    }

}
