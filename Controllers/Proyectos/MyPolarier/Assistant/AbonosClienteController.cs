using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class AbonosClienteController : ODataController
{
    private readonly bdERP db;
    private readonly tblMovimientoRecambioController mrc;
    private readonly AssistantController ac;
    public AbonosClienteController(bdERP context)
    {
        db = context;
        mrc = new(context);
        ac = new(context);
    }

    [HttpPost("odata/MyPolarier/Assistant/AbonosCliente/GetRecambios_MovimientosAsociados")]
    [Authorize]
    public async Task<ActionResult> GetRecambios_MovimientosAsociados([FromODataUri] int idAlmacenDestino, [FromODataUri] DateTimeOffset fecha, [FromODataUri] int? idMovimientoRecambio, [FromBody] List<int> idsMovimientoRecambioAsociados)
    {
        var tblRecambioNMovimientoRecambio_real = db.tblRecambioNMovimientoRecambio.Where(rnmr => rnmr.idMovimientoRecambio == idMovimientoRecambio);

        var tblRecambioNMovimientoRecambio_teorico = await SPGet_CantPrecioUbicacion_AbonoCliente(idAlmacenDestino, fecha, idMovimientoRecambio, idsMovimientoRecambioAsociados);

        var result = (
                from rnmr_t in tblRecambioNMovimientoRecambio_teorico
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
                        idMovimientoRecambio = rnmr_t.idMovimientoRecambioAsociado
                    }
                }
            );

        return Ok(result);
    }

    public async Task<List<dynamic>> SPGet_CantPrecioUbicacion_AbonoCliente(int idAlmacenDestino, DateTimeOffset fecha, int? idMovimientoRecambio, List<int> idsMovimientoRecambioAsociados)
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
            "[Assistant].[EF_recambios_spGet_CantPrecioUbicacion_AbonoCliente]",
            new
            {
                idsMovimientoRecambioAsociados = dtIdsMovimientosAsociados,
                idAlmacenDestino,
                fecha,
                idMovimientoRecambio
            }, commandType: CommandType.StoredProcedure);

        return result.ToList();
    }
}
