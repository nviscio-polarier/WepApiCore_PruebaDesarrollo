using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class infCumplimientoPedidosController : ODataController
{
    private readonly bdERP db;
    public infCumplimientoPedidosController(bdERP context)
    {
        db = context;
    }


    [HttpGet("odata/MyPolarier/ControlCalidad/infCumplimientoPedidos")]
    [Authorize]
    public async Task<ActionResult> infCumplimientoPedidos([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] string group, [FromODataUri] int? idCompañia, [FromODataUri] int? idEntidad)
    {
        var connection = db.Database.GetDbConnection();
        var results = await connection.QueryAsync("EXEC [MyReporting].[EF_infCumplimientoPedidos] @idLavanderia, @fechaIni, @fechaFin, @group, @idCompañia, @idEntidad",
            new { idLavanderia = idLavanderia, fechaIni = fechaIni, fechaFin = fechaFin, group = group, idCompañia = idCompañia, idEntidad = idEntidad });
        return Ok(results.ToList());
    }
}
