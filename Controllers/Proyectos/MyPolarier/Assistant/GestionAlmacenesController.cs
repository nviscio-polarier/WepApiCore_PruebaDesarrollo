using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class GestionAlmacenesController : ODataController
{
    private readonly bdERP db;
    public GestionAlmacenesController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost("odata/MyPolarier/Assistant/GestionAlmacenes/setIsValidado")]
    [Authorize]
    public async Task<ActionResult> setIsValidado([FromODataUri] int idMovimientoRecambio)
    {
        var entity = db.tblMovimientoRecambio.FirstOrDefault(mr => mr.idMovimientoRecambio == idMovimientoRecambio);

        if (entity == null)
            return NotFound();

        if (entity.isValidado == null) // No es un trasvase o es un trasvase que no pertenece a la familia de POLARIER GENERAL
            return BadRequest();

        entity.isValidado = !entity.isValidado;

        await db.SaveChangesAsync();

        return Ok(true);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Assistant/GestionAlmacenes/GetRecambios")]
    [Authorize]
    public async Task<ActionResult> GetRecambios([FromODataUri] int idAlmacen, [FromODataUri] DateTimeOffset fecha)
    {
        var dtIdsRecambio = new DataTable();
        dtIdsRecambio.Columns.Add("id", typeof(int));

        var connection = db.Database.GetDbConnection();
        var historicoRecambio = await connection.QueryAsync("[Assistant].[EF_recambios_spGet_HistoricoRecambio]", new { idAlmacen, idsRecambio = dtIdsRecambio, fecha, fechaCierre = (DateTime?)null, todos = true, includeRechazado = false }, commandType: CommandType.StoredProcedure);

        var idsRecambio = historicoRecambio.Where(hr => hr.idAlmacen == idAlmacen).Select(hr => hr.idRecambio).Distinct();

        var tblRecambio = db.tblRecambio.Where(r => idsRecambio.Contains(r.idRecambio));

        var tblRecambioNAlmacenRecambios = db.tblRecambioNAlmacenRecambios.Where(rnar => rnar.idAlmacen == idAlmacen && idsRecambio.Contains(rnar.idRecambio));

        var result = (
            from r in tblRecambio
            join rnar in tblRecambioNAlmacenRecambios
            on r.idRecambio equals rnar.idRecambio into rnarGroup
            from rnar in rnarGroup.DefaultIfEmpty()
            select new
            {
                r.idRecambio,
                r.denominacion,
                r.referencia,
                r.referenciaInterna,
                r.descripcionArticulo,
                ubicacion = rnar != null ? rnar.ubicacion : null,
            }
        );

        return Ok(result);
    }
}
