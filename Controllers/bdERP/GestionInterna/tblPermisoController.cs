using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

[AllowAnonymous]
public class tblPermisoController : ODataController
{
    private readonly bdERP db;

    public tblPermisoController(bdERP context)
    {
        db = context;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblPermiso);
    }
}
