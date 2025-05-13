using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class MovimientoController : ODataController
{
    private readonly bdERP db;

    public MovimientoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost("odata/Inventarios/Movimientos/infMovimientos_spSelectPrendaNMovimiento")]
    [Authorize]
    public async Task<ActionResult> infMovimientos_spSelectPrendaNMovimiento([FromBody] List<int> idsMovimiento_)
    {
        var connection = db.Database.GetDbConnection();
        string idsMovimiento = String.Join("|", idsMovimiento_.Select(x => x.ToString()).ToArray());
        var result = (await connection.QueryAsync("EXEC [Inventarios].[EF_infMovimientos_spSelectPrendaNMovimiento] @idsMovimiento", new { idsMovimiento })).ToList();
        return Ok(result);
    }

    [EnableQuery]
    [HttpPost("odata/Inventarios/Movimientos/fn_isCodigoAlbaranExists")]
    [Authorize]
    public async Task<ActionResult> fn_isCodigoAlbaranExists([FromODataUri] int? idMovimiento, [FromODataUri] string codigoAlbaran)
    {
        bool isCodigoAlbaranExists = db.tblMovimiento.Where(m => (idMovimiento == null || m.idMovimiento != idMovimiento) && m.codigoAlbaran == codigoAlbaran).Count() > 0;
        return Ok(isCodigoAlbaranExists);
    }
}
