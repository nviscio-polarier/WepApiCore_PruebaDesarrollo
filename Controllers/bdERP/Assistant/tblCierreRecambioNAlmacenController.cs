using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCierreRecambioNAlmacenController : ODataController
{
    private readonly bdERP db;

    public tblCierreRecambioNAlmacenController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblCierreRecambioNAlmacen);
    }

    [EnableQuery]
    [HttpGet("odata/tblCierreRecambioNAlmacen/GetFechasNAlmacen")]
    [Authorize]
    public ActionResult GetFechasNAlmacen([FromODataUri] bool todas = false)
    {
        var query = from cierreRecambioNAlmacen in db.tblCierreRecambioNAlmacen
                    join almacenRecambio in db.tblAlmacenRecambios on cierreRecambioNAlmacen.idAlmacen equals almacenRecambio.idAlmacen
                    where almacenRecambio.idAlmacenPadre == null
                    select new
                    {
                        cierreRecambioNAlmacen.fecha,
                        cierreRecambioNAlmacen.idAlmacen,
                        isMaxDate = db.tblCierreRecambioNAlmacen
                            .Where(x => x.idAlmacen == cierreRecambioNAlmacen.idAlmacen)
                            .Max(x => x.fecha) == cierreRecambioNAlmacen.fecha
                    };

        var result = query.Where(x => todas || x.isMaxDate == true).Select(x => new { x.fecha, x.idAlmacen }).Distinct();

        return Ok(result);
    }

    [EnableQuery]
    [HttpGet("odata/tblCierreRecambioNAlmacen/SetPrecioCierreRecambioNAlmacen")]
    [Authorize]
    public async Task<ActionResult> SetPrecioCierreRecambioNAlmacen([FromODataUri] DateTime fecha, [FromODataUri] int idAlmacen)
    {
        var connection = db.Database.GetDbConnection();
        await connection.QueryAsync("EXEC [Assistant].[EF_recambios_spSet_PrecioCierreRecambioNAlmacen] @idAlmacen, @fecha",
            new
            {
                idAlmacen,
                fecha
            });
        return Ok(true);
    }
}
