using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class infRecambios_comprasController : ODataController
{
    private readonly bdERP db;
    public infRecambios_comprasController(bdERP context)
    {
        db = context;
    }

    [HttpGet("odata/MyReporting/infRecambios_compras")]
    [Authorize]
    public async Task<ActionResult> infRecambios_compras([FromODataUri] int idAlmacenPadre, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        var connection = db.Database.GetDbConnection();
        var result = await connection.QueryAsync("EXEC [MyReporting].[EF_infRecambios_compras] @idAlmacenPadre, @fechaDesde, @fechaHasta",
            new { idAlmacenPadre, fechaDesde, fechaHasta });
        return Ok(result.ToList());
    }
}
