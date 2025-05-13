using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPrendaNRevisionController : ODataController
{
    private readonly bdERP db;

    public tblPrendaNRevisionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get(int? idRevision)
    {
        return Ok(db.tblPrendaNRevision.Where(pnr => pnr.idRevision == idRevision || idRevision == null));
    }
}
