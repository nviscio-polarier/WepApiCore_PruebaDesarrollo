using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class spSelectDashboardEjecutivo_spGetExcelKpiIberostarController : ODataController
{
    private readonly bdERP db;
    public spSelectDashboardEjecutivo_spGetExcelKpiIberostarController(bdERP context)
    {
        db = context;
    }

    [HttpGet("odata/Dashboard/spGetExcelKpiIberostar")]
    [Authorize]
    public async Task<ActionResult> spGetExcelKpiIberostar([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int idPais)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var connection = db.Database.GetDbConnection();
        var results = await connection.QueryAsync("EXEC [Dashboard].[EF_spSelectDashboardEjecutivo_spGetExcelKpiIberostar] @fechaDesde, @fechaHasta, @idPais",
            new { fechaDesde = fechaDesde, fechaHasta = fechaHasta, idPais = idPais });
        return Ok(results.ToList());
    }
}
