using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPrendaNPedidoExtraController : ODataController
{
    private readonly bdERP db;

    public tblPrendaNPedidoExtraController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get(int? idPedidoExtra)
    {
        return Ok(db.tblPrendaNPedidoExtra.Where(pnpe => pnpe.idPedidoExtra == idPedidoExtra || idPedidoExtra == null));
    }
}
