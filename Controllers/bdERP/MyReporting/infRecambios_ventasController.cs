using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class infRecambios_ventasController : ODataController
{
    private readonly bdERP db;
    public infRecambios_ventasController(bdERP context)
    {
        db = context;
    }

    [HttpGet("odata/MyReporting/infRecambios_ventas")]
    [Authorize]
    public async Task<ActionResult> infRecambios_ventas([FromODataUri] int idAlmacenPadre, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        var connection = db.Database.GetDbConnection();
        var result = await connection.QueryAsync("EXEC [MyReporting].[EF_infRecambios_ventas] @idAlmacenPadre, @fechaDesde, @fechaHasta",
            new { idAlmacenPadre, fechaDesde, fechaHasta });
        return Ok(result.ToList());
    }
}
