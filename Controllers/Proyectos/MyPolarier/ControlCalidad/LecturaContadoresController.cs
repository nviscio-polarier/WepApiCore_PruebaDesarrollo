using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class LecturaContadoresController : ODataController
{
    private readonly bdERP db;
    public LecturaContadoresController(bdERP context)
    {
        db = context;
    }

    [HttpGet("odata/MyPolarier/ControlCalidad/selectLecturaContadores")]
    [Authorize]
    public async Task<ActionResult> lecturaContadores([FromODataUri] string string_IdsRecursoContador, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        var connection = db.Database.GetDbConnection();
        var results = await connection.QueryAsync("EXEC [MyRealData].[EF_lecturaContador_spSelectLecturaContadores] @string_IdsRecursoContador, @fechaDesde, @fechaHasta",
            new { string_IdsRecursoContador = string_IdsRecursoContador, fechaDesde = fechaDesde, fechaHasta = fechaHasta });
        return Ok(results.ToList());
    }
}
