using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblDiasLibresPersonal_LlamamientoController : ODataController
{
    private readonly bdERP db;

    public tblDiasLibresPersonal_LlamamientoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return (Ok(db.tblDiasLibresPersonal_Llamamiento));
    }
}
