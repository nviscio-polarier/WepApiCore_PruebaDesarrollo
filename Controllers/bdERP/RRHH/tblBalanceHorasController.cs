using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblBalanceHorasController : ODataController
{
    private readonly bdERP db;

    public tblBalanceHorasController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblBalanceHoras);
    }

    [HttpGet("odata/tblBalanceHoras/GetBalanceTotal")]
    [Authorize]
    public async Task<ActionResult> GetBalanceTotal([FromODataUri] int idPersona)
    {
        return Ok(db.tblBalanceHoras.Where(x => x.idPersona == idPersona).Select(x => x.minutos).Sum());
    }
}
