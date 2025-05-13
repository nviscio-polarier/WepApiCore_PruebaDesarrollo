using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;

namespace WebApiCore.Controllers;

public class tblTipoPedidoController : ODataController
{
    private readonly bdERP db;
    public tblTipoPedidoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get([FromODataUri] int? idEntidad)
    {
        return Ok(db.tblTipoPedido.Where(x =>
        (
            (idEntidad != null && x.idEntidad.Select(y => y.idEntidad).Contains((int)idEntidad)) ||
            idEntidad == null
        )));
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get([FromODataUri] byte key)
    {
        return Ok(await db.tblTipoPedido.FindAsync(key));
    }
}
