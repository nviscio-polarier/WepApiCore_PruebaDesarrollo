using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

[Authorize]
public class tblTipoSalidaRecambioController : ODataController
{
    private readonly bdERP db;

    public tblTipoSalidaRecambioController(bdERP context)
    {
        db = context;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblTipoSalidaRecambio);
    }
}
