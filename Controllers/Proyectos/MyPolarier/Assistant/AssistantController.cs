using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Enums.Assistant;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class AssistantController : ODataController
{
    private readonly bdERP db;
    private readonly tblMovimientoRecambioController mrc;

    public AssistantController(bdERP context)
    {
        db = context;
        mrc = new(db);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Assistant/GetInfoRecambio")]
    [Authorize]
    public ActionResult GetInfoRecambio([FromODataUri] string campoBusqueda, [FromODataUri] int idMovimientoRecambio, [FromODataUri] bool isEqual)
    {
        int almacenMovimiento = 0;

        var movimiento = db.tblMovimientoRecambio.FirstOrDefault(x => x.idMovimientoRecambio == idMovimientoRecambio);

        if (movimiento != null)
        {
            almacenMovimiento = (int)movimiento.idAlmacenDestino;
        }

        var result = (
             from r in db.tblRecambio
             join rnp in db.tblRecambioNProveedor on r.idRecambio equals rnp.idRecambio into rnpJoin
             from rnp in rnpJoin.DefaultIfEmpty()
             join p in db.tblProveedor on rnp.idProveedor equals p.idProveedor into pJoin
             from p in pJoin.DefaultIfEmpty()
             where
              (isEqual ?
              (
                (r.referenciaInterna ?? "").Equals(campoBusqueda) ||
                (r.referencia ?? "").Equals(campoBusqueda) ||
                r.denominacion.Equals(campoBusqueda) ||
                p.nombreComercial.Equals(campoBusqueda) ||
                (rnp.referencia ?? "").Equals(campoBusqueda) ||
                (rnp.codigoBarras ?? "").Equals(campoBusqueda) ||
                (rnp.fabricante ?? "").Equals(campoBusqueda) ||
                (rnp.refFabricante ?? "").Equals(campoBusqueda) ||
                (rnp.codigoFabricante ?? "").Equals(campoBusqueda) ||
                (rnp.codigoBarrasFabricante ?? "").Equals(campoBusqueda) ||
                (rnp.observaciones ?? "").Equals(campoBusqueda))
            :
                (r.referenciaInterna ?? "").Contains(campoBusqueda) ||
                (r.referencia ?? "").Contains(campoBusqueda) ||
                r.denominacion.Contains(campoBusqueda) ||
                p.nombreComercial.Contains(campoBusqueda) ||
                (rnp.referencia ?? "").Contains(campoBusqueda) ||
                (rnp.codigoBarras ?? "").Contains(campoBusqueda) ||
                (rnp.fabricante ?? "").Contains(campoBusqueda) ||
                (rnp.refFabricante ?? "").Contains(campoBusqueda) ||
                (rnp.codigoFabricante ?? "").Contains(campoBusqueda) ||
                (rnp.codigoBarrasFabricante ?? "").Contains(campoBusqueda) ||
                (rnp.observaciones ?? "").Contains(campoBusqueda)
              ) && r.activo == true && r.eliminado == false
             select new
             {
                 r.idRecambio,
                 r.denominacion,
                 r.tblRecambioNAlmacenRecambios.FirstOrDefault(x => x.idAlmacen == almacenMovimiento).ubicacion,
                 r.referencia,
                 r.referenciaInterna,
                 r.peso
             }
         )
         .Distinct()
         .ToList();

        return Ok(result);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Assistant/BuscarRecambios")]
    [Authorize]
    public List<int> BuscarRecambios([FromODataUri] string campoBusqueda, [FromODataUri] bool isBusquedaExacta = false)
    {
        if (isBusquedaExacta)
        {
            var result = (
                from r in db.tblRecambio
                join rnp in db.tblRecambioNProveedor on r.idRecambio equals rnp.idRecambio into rnpJoin
                from rnp in rnpJoin.DefaultIfEmpty()
                join p in db.tblProveedor on rnp.idProveedor equals p.idProveedor into pJoin
                from p in pJoin.DefaultIfEmpty()
                where
                    (r.referenciaInterna ?? "").Equals(campoBusqueda) ||
                    (r.referencia ?? "").Equals(campoBusqueda) ||
                    r.denominacion.Equals(campoBusqueda) ||
                    p.nombreComercial.Equals(campoBusqueda) ||
                    (rnp.referencia ?? "").Equals(campoBusqueda) ||
                    (rnp.codigoBarras ?? "").Equals(campoBusqueda) ||
                    (rnp.fabricante ?? "").Equals(campoBusqueda) ||
                    (rnp.refFabricante ?? "").Equals(campoBusqueda) ||
                    (rnp.codigoFabricante ?? "").Equals(campoBusqueda) ||
                    (rnp.codigoBarrasFabricante ?? "").Equals(campoBusqueda) ||
                    (rnp.observaciones ?? "").Equals(campoBusqueda)
                select r.idRecambio
            )
            .Distinct()
            .ToList();

            return result;
        }
        else
        {
            var result = (
                from r in db.tblRecambio
                join rnp in db.tblRecambioNProveedor on r.idRecambio equals rnp.idRecambio into rnpJoin
                from rnp in rnpJoin.DefaultIfEmpty()
                join p in db.tblProveedor on rnp.idProveedor equals p.idProveedor into pJoin
                from p in pJoin.DefaultIfEmpty()
                where
                    (r.referenciaInterna ?? "").Contains(campoBusqueda) ||
                    (r.referencia ?? "").Contains(campoBusqueda) ||
                    r.denominacion.Contains(campoBusqueda) ||
                    p.nombreComercial.Contains(campoBusqueda) ||
                    (rnp.referencia ?? "").Contains(campoBusqueda) ||
                    (rnp.codigoBarras ?? "").Contains(campoBusqueda) ||
                    (rnp.fabricante ?? "").Contains(campoBusqueda) ||
                    (rnp.refFabricante ?? "").Contains(campoBusqueda) ||
                    (rnp.codigoFabricante ?? "").Contains(campoBusqueda) ||
                    (rnp.codigoBarrasFabricante ?? "").Contains(campoBusqueda) ||
                    (rnp.observaciones ?? "").Contains(campoBusqueda)
                select r.idRecambio
            )
            .Distinct()
            .ToList();

            return result;
        }
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Assistant/GetRecambiosInventario")]
    [Authorize]
    public async Task<ActionResult> GetRecambiosInventario([FromODataUri] int? idMovimientoRecambio, [FromODataUri] DateTimeOffset fecha, [FromODataUri] int? idAlmacenDestino = null)
    {
        // Es necesario al menos un campo
        if (idAlmacenDestino == null && idMovimientoRecambio == null)
        {
            return BadRequest();
        }

        var inventario = db.tblMovimientoRecambio.Where(mr => mr.idMovimientoRecambio == idMovimientoRecambio && mr.idTipoMovimientoRecambio == (int)idsTipoMovimientoRecambio.Regularizacion && mr.isInventario).FirstOrDefault();

        // Si el inventario no existe
        if (inventario == null && idMovimientoRecambio != null)
        {
            return BadRequest();
        }

        idAlmacenDestino = inventario?.idAlmacenDestino ?? idAlmacenDestino;

        // Si no hay un almacén destino seleccionado
        if (idAlmacenDestino == null)
        {
            return BadRequest();
        }

        var cantPrecioUbicacion = Get_CantUbicacion_Regularizacion(idAlmacenDestino, null, null, fecha, idMovimientoRecambio).Result.ToList();

        var historicoRecambio = await Get_HistoricoRecambio((int)idAlmacenDestino, null, fecha, null, true);

        var idsRecambio = historicoRecambio.Where(hr => hr.idAlmacen == idAlmacenDestino).Select(hr => hr.idRecambio).Distinct().ToList();

        var tblRecambioNMovimientoRecambio = db.tblRecambioNMovimientoRecambio.Include(x => x.idUsuarioNavigation)
            .Where(rnmr => rnmr.idMovimientoRecambio == idMovimientoRecambio).ToList();

        var tblRecambio = db.tblRecambio.Where(x => idsRecambio.Contains(x.idRecambio)).ToList();

        var result = (
            from r in tblRecambio
            join rnmr in tblRecambioNMovimientoRecambio
            on r.idRecambio equals rnmr.idRecambio into rnmrGroup
            from rnmr in rnmrGroup.DefaultIfEmpty()
            join cpu in cantPrecioUbicacion
            on r.idRecambio equals cpu.idRecambio into cpuGroup
            from cpu in cpuGroup.DefaultIfEmpty()
            select new
            {
                r.idRecambio,
                r.denominacion,
                r.referenciaInterna,
                r.referencia,
                cpu?.ubicacion,
                cantidadTeorico = cpu?.cantidadTeorico ?? 0,
                rnmr?.cantidad,
                rnmr?.idUsuario,
                nombreUsuario = rnmr?.idUsuarioNavigation?.nombre,
                minStock = cpu?.min,
                max = (int?)null,
                rnmr?.fecha,
                r.peso,
                isApp = rnmr?.isApp ?? false
            }
        );

        return Ok(result);
    }

    [HttpGet("odata/MyPolarier/Assistant/GetInfoActual")]
    [Authorize]
    public async Task<ActionResult> GetInfoActual([FromODataUri] DateTimeOffset fecha, [FromODataUri] int? idAlmacen, [FromODataUri] int? idRecambio)
    {
        var connection = db.Database.GetDbConnection();

        var infoActual = (await connection.QueryAsync<Result_InfoActual>("EXEC [Assistant].[EF_recambios_spGet_infoActual] @idAlmacen, @idRecambio, @fecha", new { idAlmacen, idRecambio, fecha }));

        return Ok(infoActual);
    }

    [HttpGet("odata/MyPolarier/Assistant/GetAlmacenRecambiosNPersona")]
    [Authorize]
    public ActionResult GetAlmacenRecambiosNPersona([FromODataUri] int? idUsuario = null)
    {
        idUsuario ??= int.Parse(HttpContext.Items["idUsuario"].ToString());

        var usuario = db.tblUsuario.FirstOrDefault(u => u.idUsuario == idUsuario);

        if (usuario == null || usuario.idPersona == null)
        {
            return BadRequest();
        }

        var tblAlmacenRecambios = db.tblAlmacenRecambios.Where(ar =>
            ar.activo == true
            && !ar.eliminado
        ).ToList();

        if (usuario.idCargo == (short)idsCargo.Desarrollador || usuario.idCargo == (short)idsCargo.Master)
        {
            var result = tblAlmacenRecambios.Select(ar => new
            {
                ar.idAlmacen,
                ar.idPais,
                idPersona = (int)usuario.idPersona,
                isConsulta = true,
                isSalida = true,
            });

            return Ok(result);
        }
        else
        {
            var result = db.tblAlmacenRecambiosNPersona.Where(arnp =>
                arnp.idPersona == usuario.idPersona
                && tblAlmacenRecambios.Select(ar => ar.idAlmacen).Contains(arnp.idAlmacen)
            ).Select(arnp => new
            {
                arnp.idAlmacen,
                arnp.idAlmacenNavigation.idPais,
                arnp.idPersona,
                arnp.isConsulta,
                arnp.isSalida
            });

            return Ok(result);
        }
    }

    public async Task<List<dynamic>> Get_HistoricoRecambio(int idAlmacen, DataTable? dtIdsRecambio, DateTimeOffset fecha, DateTime? fechaCierre, bool todos)
    {
        if (dtIdsRecambio == null)
        {
            dtIdsRecambio = new();
            dtIdsRecambio.Columns.Add("id", typeof(int));
        }

        var connection = db.Database.GetDbConnection();
        var historicoRecambio = await connection.QueryAsync("[Assistant].[EF_recambios_spGet_HistoricoRecambio]", new { idAlmacen, idsRecambio = dtIdsRecambio, fecha, fechaCierre, todos, includeRechazado = false }, commandType: CommandType.StoredProcedure);

        return historicoRecambio.ToList();
    }

    private async Task<List<dynamic>> Get_CantUbicacion_Regularizacion(int? idAlmacenDestino, string? campoBusqueda, int? idRecambio, DateTimeOffset? fecha, int? idMovimientoRecambio)
    {
        var connection = db.Database.GetDbConnection();
        var result = (await connection.QueryAsync("EXEC [Assistant].[EF_recambios_spGet_CantUbicacion_Regularizacion] @idAlmacenDestino, @fecha, @campoBusqueda, @idRecambio, @idMovimientoRecambio",
            new
            {
                idAlmacenDestino,
                campoBusqueda,
                idRecambio,
                fecha,
                idMovimientoRecambio,
            })).ToList();

        return result;
    }

    public class Result_InfoActual
    {
        public int idAlmacen { get; set; }
        public int idRecambio { get; set; }
        public int? cantidadPrincipal { get; set; }
        public int? cantidadSecundarios { get; set; }
        public decimal? precio { get; set; }
        public string? ubicacion { get; set; }
    }
}
