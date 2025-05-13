using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class infRecambiosController : ODataController
{
    private readonly bdERP db;
    public infRecambiosController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/Assistant/infInventario")]
    [Authorize]
    public async Task<ActionResult> InfInventario([FromODataUri] int idAlmacenPadre, [FromODataUri] DateTimeOffset fechaHasta, [FromODataUri] int idMovimientoRecambio, [FromODataUri] DateTime? fechaCierre = null)
    {
        var connection = db.Database.GetDbConnection();
        var results = await connection.QueryAsync("EXEC [MyReporting].[EF_infRecambios_inventario] @idAlmacenPadre, @fechaHasta, @fechaCierre, @idMovimientoRecambio",
            new { idAlmacenPadre, fechaHasta, fechaCierre, idMovimientoRecambio });
        return Ok(results.ToList());
    }

    [EnableQuery]
    [HttpGet("odata/Assistant/InfRegularizaciones")]
    [Authorize]
    public async Task<ActionResult> InfRegularizaciones([FromODataUri] int idAlmacenPadre, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        var connection = db.Database.GetDbConnection();
        var results = await connection.QueryAsync("EXEC [MyReporting].[EF_infRecambios_regularizaciones] @idAlmacenPadre, @fechaDesde, @fechaHasta",
            new { idAlmacenPadre, fechaDesde, fechaHasta });
        return Ok(results.ToList());
    }
}
