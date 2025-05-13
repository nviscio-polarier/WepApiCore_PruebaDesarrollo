using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Enums.Assistant;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class MovimientoRecambioController : ODataController
{
    private readonly bdERP db;
    private readonly tblMovimientoRecambioController mrc;
    private readonly AssistantController ac;
    public MovimientoRecambioController(bdERP context)
    {
        db = context;
        mrc = new(context);
        ac = new(context);
    }

    [HttpGet("odata/MyPolarier/Assistant/MovimientoRecambio/GetRecambios_MovimientoRecambioCreado")]
    [Authorize]
    public async Task<ActionResult> GetRecambios_MovimientoRecambioCreado([FromODataUri] int idMovimientoRecambio)
    {
        var tblRecambioNMovimientoRecambio_real = db.tblRecambioNMovimientoRecambio.Where(rnmr => rnmr.idMovimientoRecambio == idMovimientoRecambio);

        var tblRecambioNMovimientoRecambio_teorico = await mrc.spGet_CantPrecioUbicacion_MovimientoRecambio(null, null, null, null, null, null, idMovimientoRecambio, true);

        var result = (
                from rnmr_t in tblRecambioNMovimientoRecambio_teorico
                join rnmr_r in tblRecambioNMovimientoRecambio_real
                on rnmr_t.idRecambio equals rnmr_r.idRecambio into rnmr_rJoin
                from rnmr_r in rnmr_rJoin.DefaultIfEmpty()
                join r in db.tblRecambio on rnmr_t.idRecambio equals r.idRecambio into rJoin
                from r in rJoin.DefaultIfEmpty()
                select new
                {
                    idMovimientoRecambio,
                    rnmr_t.idAlmacen,
                    rnmr_t.idRecambio,
                    r.referenciaInterna,
                    r.referencia,
                    rnmr_r.referenciaProveedor,
                    r.denominacion,
                    rnmr_t.ubicacion,
                    rnmr_t.min,
                    rnmr_t.max,
                    rnmr_r.cantidad,
                    rnmr_r.cantidadTeorico,
                    rnmr_t.precio,
                }
            );

        return Ok(result);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Assistant/MovimientoRecambio/GetRecambio_MovimientoRecambio")]
    [Authorize]
    public async Task<ActionResult> GetRecambio_MovimientoRecambio([FromODataUri] int? idAlmacenOrigen, [FromODataUri] int? idAlmacenDestino, [FromODataUri] int? idRecambio, [FromODataUri] int? idRecambioNProveedor, [FromODataUri] DateTimeOffset? fecha, [FromODataUri] int? idTipoMovimientoRecambio, [FromODataUri] int? idProveedor, [FromODataUri] int? idMovimientoRecambio)
    {
        var connection = db.Database.GetDbConnection();

        List<dynamic> cantPrecioUbicacion_MovimientoRecambio = new();

        if (idTipoMovimientoRecambio == (int)idsTipoMovimientoRecambio.Regularizacion)
        {
            cantPrecioUbicacion_MovimientoRecambio = (await connection.QueryAsync("EXEC [Assistant].[EF_recambios_spGet_CantUbicacion_Regularizacion] @idAlmacenDestino, @fecha, @campoBusqueda, @idRecambio, @idMovimientoRecambio",
                new
                {
                    idAlmacenDestino,
                    fecha,
                    campoBusqueda = (string?)null,
                    idRecambio,
                    idMovimientoRecambio,
                })).ToList();
        }
        else
        {
            cantPrecioUbicacion_MovimientoRecambio = (await connection.QueryAsync("EXEC [Assistant].[EF_recambios_spGetFiltrado_CantPrecioUbicacion_MovimientoRecambio] @idAlmacenOrigen, @idAlmacenDestino, @idRecambio, @idRecambioNProveedor, @fecha, @idTipoMovimientoRecambio, @idProveedor, @idMovimientoRecambio, @isConsultaMovimiento",
                new
                {
                    idAlmacenOrigen,
                    idAlmacenDestino,
                    idRecambio,
                    idRecambioNProveedor,
                    fecha,
                    idTipoMovimientoRecambio,
                    idProveedor,
                    idMovimientoRecambio,
                    isConsultaMovimiento = false
                })).ToList();
        }

        var idsRecambio = cantPrecioUbicacion_MovimientoRecambio.Select(cpu => cpu.idRecambio);

        var idsRecambioNProveedor = cantPrecioUbicacion_MovimientoRecambio.Select(cpu => cpu.idRecambioNProveedor);

        var tblRecambio = await db.tblRecambio
            .Where(r => idsRecambio.Contains(r.idRecambio))
            .ToListAsync();

        var tblRecambioNProveedor = await db.tblRecambioNProveedor
            .Include(rnp => rnp.idProveedorNavigation)
            .Include(rnp => rnp.idPaisNavigation.idMonedaNavigation)
            .Where(rnp => idsRecambioNProveedor.Contains(rnp.idRecambioNProveedor))
            .ToListAsync();

        var result = (
            from cpu in cantPrecioUbicacion_MovimientoRecambio
            join r in tblRecambio
            on cpu.idRecambio equals r.idRecambio
            join rnp in tblRecambioNProveedor
            on cpu.idRecambioNProveedor equals rnp.idRecambioNProveedor into rnpJoin
            from rnp in rnpJoin.DefaultIfEmpty()
            select new
            {
                cpu.idAlmacen,
                cpu.idRecambio,
                cpu.idRecambioNProveedor,
                cpu?.cantidadTeorico,
                cpu.min,
                cpu.max,
                cpu.precio,
                cpu.ubicacion,
                r.referenciaInterna,
                r.referencia,
                denoRecambio = r.denominacion,
                referenciaProveedor = rnp?.referencia,
                denoProveedor = rnp?.idProveedorNavigation.nombreComercial ?? "",
                codigoMoneda = rnp?.idPaisNavigation?.idMonedaNavigation?.codigo,
            }
        );

        return Ok(result);
    }


    [EnableQuery]
    [HttpPost("odata/MyPolarier/Assistant/MovimientoRecambio/GetRecambios_MovimientoRecambio")]
    [Authorize]
    public async Task<ActionResult> GetRecambios_MovimientoRecambio([FromODataUri] int? idAlmacenOrigen, [FromODataUri] int? idAlmacenDestino, [FromODataUri] int? idProveedor, [FromODataUri] int idTipoMovimientoRecambio, [FromODataUri] string campoBusqueda, [FromBody] List<int> idsRecambio, [FromODataUri] bool isBusquedaExacta = false)
    {
        var idsRecambioBuscados = ac.BuscarRecambios(campoBusqueda, isBusquedaExacta);

        var tblRecambio = await db.tblRecambio
            .Where(r => idsRecambioBuscados.Contains(r.idRecambio) && !idsRecambio.Contains(r.idRecambio) && r.activo == true && !r.eliminado)
            .ToListAsync();

        var tblAlmacenRecambios = await db.tblAlmacenRecambios.Where(ar => ar.idAlmacen == idAlmacenOrigen || (ar.idAlmacen == idAlmacenDestino && idTipoMovimientoRecambio != (int)idsTipoMovimientoRecambio.Traspaso)).ToListAsync();

        var tblRecambioNProveedor = idTipoMovimientoRecambio == (int)idsTipoMovimientoRecambio.Entrada
            ? await db.tblRecambioNProveedor
                .Include(rnp => rnp.idProveedorNavigation)
                .Include(rnp => rnp.idPaisNavigation.idMonedaNavigation)
                .Where(rnp => rnp.activo == true && rnp.idProveedor == idProveedor)
                .ToListAsync()
            : new();

        var result = (
            from r in tblRecambio
            from ar in tblAlmacenRecambios
            join rnp in tblRecambioNProveedor
            on new { r.idRecambio, ar.idPais } equals new { rnp.idRecambio, rnp.idPais } into rnpJoin
            from rnp in rnpJoin.DefaultIfEmpty()
            select new
            {
                r.idRecambio,
                r.denominacion,
                referencia = idTipoMovimientoRecambio == (int)idsTipoMovimientoRecambio.Entrada ? rnp?.referencia : r.referencia,
                r.referenciaInterna,
                rnp?.idRecambioNProveedor,
                orden = rnp?.orden ?? 0,
                denoProveedor = rnp?.idProveedorNavigation.nombreComercial,
                precio = rnp?.ultimoPrecio ?? 0,
                codigoMoneda = rnp?.idPaisNavigation?.idMonedaNavigation?.codigo,
            }
        );

        return Ok(result);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Assistant/HistoricoRecambio")]
    [Authorize]
    public async Task<ActionResult> Get_HistoricoRecambio([FromODataUri] int idAlmacen, [FromODataUri] int idRecambio, [FromODataUri] DateTimeOffset? fecha, [FromODataUri] bool todos, [FromODataUri] DateTime? fechaCierre = null)
    {
        DataTable dtIdsRecambio = new();
        dtIdsRecambio.Columns.Add("id", typeof(int));

        var rowRec = dtIdsRecambio.NewRow();
        rowRec["id"] = idRecambio;
        dtIdsRecambio.Rows.Add(rowRec);

        var connection = db.Database.GetDbConnection();
        var historicoRecambio = await connection.QueryAsync("[Assistant].[EF_recambios_spGet_HistoricoRecambio]", new { idAlmacen, idsRecambio = dtIdsRecambio, fecha, fechaCierre, todos, includeRechazado = false }, commandType: CommandType.StoredProcedure);

        var resultado = (from hr in historicoRecambio
                         join ar in db.tblAlmacenRecambios on hr.idAlmacen equals ar.idAlmacen into arJoin
                         from ar in arJoin.DefaultIfEmpty()
                         join r in db.tblRecambio on hr.idRecambio equals r.idRecambio into rJoin
                         from r in rJoin.DefaultIfEmpty()
                         join pt in db.tblParteTrabajo on hr.idParteTrabajo equals pt.idParteTrabajo into ptJoin
                         from pt in ptJoin.DefaultIfEmpty()
                         let idLavanderia = pt != null ? pt.idLavanderia : (int?)null
                         join l in db.tblLavanderia on idLavanderia equals l.idLavanderia into lJoin
                         from l in lJoin.DefaultIfEmpty()
                         join tmr in db.tblTipoMovimientoRecambio on hr.idTipoMovimientoRecambioOriginal equals tmr.idTipoMovimientoRecambio into tmrJoin
                         from tmr in tmrJoin.DefaultIfEmpty()
                         join mr in db.tblMovimientoRecambio on hr.idMovimientoRecambio equals mr.idMovimientoRecambio into mrJoin
                         from mr in mrJoin.DefaultIfEmpty()
                         select new
                         {
                             ID = hr.idTipoMovimientoRecambioOriginal == 7 ? null : hr.idTipoMovimientoRecambioOriginal == 6 ? pt.codigo : hr.idMovimientoRecambio.ToString(),
                             fecha = hr.idTipoMovimientoRecambioOriginal == 7 ? new DateTime(hr.fecha.Year, hr.fecha.Month, 1).AddMonths(1).AddMilliseconds(-1) : hr.fecha.DateTime,
                             hr.origen,
                             hr.destino,
                             denoLavanderia = l != null ? l.denominacion : null,
                             hr.cantidad,
                             hr.precio,
                             importe = hr.cantidad * hr.precio,
                             idAlmacen = (ar != null) ? ar.idAlmacen : (int?)null,
                             idAlmacenPadre = (ar != null) ? ar.idAlmacenPadre : (int?)null,
                             idTipoSalidaRecambio = ar.idAlmacenPadreNavigation?.idTipoSalidaRecambio ?? ar.idTipoSalidaRecambio,
                             denoAlmacen = (ar != null) ? ar.denominacion : null,
                             idRecambio = (r != null) ? r.idRecambio : (int?)null,
                             denoRecambio = (r != null) ? r.denominacion : null,
                             tipoMovimiento = hr.idTipoMovimientoRecambioOriginal == (int)idsTipoMovimientoRecambio.Traspaso
                                ? hr.idTipoMovimientoRecambio == (int)idsTipoMovimientoRecambio.Entrada
                                    ? "TRASPASO - ENTRADA"
                                    : "TRASPASO - SALIDA"
                                : hr.idTipoMovimientoRecambioOriginal == (int)idsTipoMovimientoRecambio.Regularizacion && mr.isInventario
                                    ? "INVENTARIO"
                                    : tmr.denominacion.ToUpper(),
                             hr.idTipoMovimientoRecambio,
                             hr.idTipoMovimientoRecambioOriginal
                         }).ToList();

        return Ok(resultado);
    }
}
