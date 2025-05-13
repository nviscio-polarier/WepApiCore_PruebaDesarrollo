using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class AbonosProveedorController : ODataController
{
    private readonly bdERP db;
    private readonly tblMovimientoRecambioController mrc;
    private readonly AssistantController ac;
    public AbonosProveedorController(bdERP context)
    {
        db = context;
        mrc = new(context);
        ac = new(context);
    }

    [HttpPost("odata/MyPolarier/Assistant/AbonosProveedor/GetRecambios_MovimientosAsociados")]
    [Authorize]
    public async Task<ActionResult> GetRecambios_MovimientosAsociados([FromODataUri] int idAlmacenOrigen, [FromODataUri] DateTimeOffset fecha, [FromODataUri] int? idMovimientoRecambio, [FromBody] List<int> idsMovimientoRecambioAsociados)
    {
        var tblRecambioNMovimientoRecambio_real = db.tblRecambioNMovimientoRecambio.Where(rnmr => rnmr.idMovimientoRecambio == idMovimientoRecambio);

        var tblRecambioNMovimientoRecambio_teorico = await SPGet_CantPrecioUbicacion_AbonoProveedor(idAlmacenOrigen, fecha, idMovimientoRecambio, idsMovimientoRecambioAsociados);

        var idsMovimientoRecambio = tblRecambioNMovimientoRecambio_teorico.Select(rnmr_t => rnmr_t.idMovimientoRecambioAsociado).Distinct();

        var tblMovimientoRecambio_teorico = db.tblMovimientoRecambio.Where(mr => idsMovimientoRecambio.Contains(mr.idMovimientoRecambio));

        var result = (
                from rnmr_t in tblRecambioNMovimientoRecambio_teorico
                join mr_t in tblMovimientoRecambio_teorico
                on rnmr_t.idMovimientoRecambioAsociado equals mr_t.idMovimientoRecambio
                join rnmr_r in tblRecambioNMovimientoRecambio_real
                on rnmr_t.idRecambio equals rnmr_r.idRecambio into rnmr_rGroup
                from rnmr_r in rnmr_rGroup.DefaultIfEmpty()
                join r in db.tblRecambio on rnmr_t.idRecambio equals r.idRecambio into rGroup
                from r in rGroup.DefaultIfEmpty()
                select new
                {
                    idMovimientoRecambio = idMovimientoRecambio ?? 0,
                    idRecambioNMovimientoRecambioAsociado = rnmr_t.idRecambioNMovimientoRecambio,
                    rnmr_t.idRecambio,
                    r.referenciaInterna,
                    r.referencia,
                    rnmr_r?.referenciaProveedor,
                    r.denominacion,
                    rnmr_t.ubicacion,
                    rnmr_t.min,
                    rnmr_t.max,
                    cantidad = rnmr_r?.cantidad ?? 0,
                    rnmr_t.precio,
                    idRecambioNMovimientoRecambioAsociadoNavigation = new
                    {
                        idMovimientoRecambio = rnmr_t.idMovimientoRecambioAsociado,
                        mr_t.codigoAlbaranProveedor
                    }
                }
            );

        return Ok(result);
    }

    public async Task<List<dynamic>> SPGet_CantPrecioUbicacion_AbonoProveedor(int idAlmacenOrigen, DateTimeOffset fecha, int? idMovimientoRecambio, List<int> idsMovimientoRecambioAsociados)
    {
        DataTable dtIdsMovimientosAsociados = new();
        dtIdsMovimientosAsociados.Columns.Add("id", typeof(int));

        foreach (var idMovimientoAsociado in idsMovimientoRecambioAsociados)
        {
            var rowMov = dtIdsMovimientosAsociados.NewRow();
            rowMov["id"] = idMovimientoAsociado;
            dtIdsMovimientosAsociados.Rows.Add(rowMov);
        }

        var connection = db.Database.GetDbConnection();
        var result = await connection.QueryAsync(
            "[Assistant].[EF_recambios_spGet_CantPrecioUbicacion_AbonoProveedor]",
            new
            {
                idsMovimientoRecambioAsociados = dtIdsMovimientosAsociados,
                idAlmacenOrigen,
                fecha,
                idMovimientoRecambio
            }, commandType: CommandType.StoredProcedure);

        return result.ToList();
    }
}
