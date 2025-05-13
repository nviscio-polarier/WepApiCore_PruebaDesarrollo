using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class infControlAlmacenClienteController : ODataController
{
    private readonly bdERP db;
    public infControlAlmacenClienteController(bdERP context)
    {
        db = context;
    }

    [HttpGet("odata/MyPolarier/ControlCalidad/infControlAlmacenCliente")]
    [Authorize]
    public async Task<ActionResult> infControlAlmacenCliente([FromODataUri] int idCompañia, [FromODataUri] DateTime fechaIni,
        [FromODataUri] DateTime fechaFin, [FromODataUri] string group, [FromODataUri] int? idPrenda)
    {
        var connection = db.Database.GetDbConnection();
        var results = await connection.QueryAsync("EXEC [MyReporting].[EF_infControlAlmacenCliente] @idCompañia, @fechaIni, @fechaFin, @group, @idPrenda ",
            new { idCompañia = idCompañia, fechaIni = fechaIni, fechaFin = fechaFin, group = group, idPrenda = idPrenda });
        return Ok(results.ToList());
    }
}
