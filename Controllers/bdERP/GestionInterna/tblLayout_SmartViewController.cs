using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblLayout_SmartViewController : ODataController
{
    private readonly bdERP db;

    public tblLayout_SmartViewController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<tblUsuario>> Get()
    {
        return Ok(db.tblLayout_SmartView);
    }

}
