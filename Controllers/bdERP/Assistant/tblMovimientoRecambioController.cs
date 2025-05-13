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

public class tblMovimientoRecambioController : ODataController
{
    private readonly bdERP db;

    public tblMovimientoRecambioController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public ActionResult Get([FromODataUri] int? idTipoMovimientoRecambio)
    {
        var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var user = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();

        if (user == null)
        {
            return BadRequest("Usuario no encontrado.");
        }

        var idPersona = db.tblUsuario.FirstOrDefault(u => u.idUsuario == idUsuario)?.idPersona;

        if (idPersona == null)
        {
            return BadRequest("El usuario no tiene una persona asociada.");
        }

        bool isMasterDev = user.idCargo == (short)idsCargo.Desarrollador || user.idCargo == (short)idsCargo.Master;

        List<int> idsAlmacenRecambiosNPersona = db.tblAlmacenRecambiosNPersona.Where(arnp => arnp.idPersona == idPersona).Select(arnp => arnp.idAlmacen).ToList();

        return Ok(db.tblMovimientoRecambio.Where(mr =>
            (idTipoMovimientoRecambio == null || mr.idTipoMovimientoRecambio == idTipoMovimientoRecambio)
            && (
                    isMasterDev || (
                        (mr.idAlmacenOrigen == null || idsAlmacenRecambiosNPersona.Contains((int)mr.idAlmacenOrigen)) &&
                        (mr.idAlmacenDestino == null || idsAlmacenRecambiosNPersona.Contains((int)mr.idAlmacenDestino))
                    )
                )
            )
        );
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblMovimientoRecambio movimiento)
    {
        #region isValidado

        var idsAlmacen = db.tblAlmacenRecambios // Almacenes de la familia de POLARIER GENERAL
            .Where(ar => ar.idAlmacen == (int)idsAlmacenRecambios.PolarierGeneral || ar.idAlmacenPadre == (int)idsAlmacenRecambios.PolarierGeneral)
            .Select(ar => ar.idAlmacen);

        if ( // Si almacenes del trasvase son de la familia de POLARIER GENERAL...
            movimiento.idTipoMovimientoRecambio == (int)idsTipoMovimientoRecambio.Traspaso
            && idsAlmacen.Any(idAlmacen => idAlmacen == movimiento.idAlmacenOrigen)
            && idsAlmacen.Any(idAlmacen => idAlmacen == movimiento.idAlmacenDestino)
        )
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            var idPersona = db.tblUsuario.FirstOrDefault(p => p.idUsuario == idUsuario)?.idPersona;

            var idCategoriaInterna = db.tblPersona.FirstOrDefault(x => x.idPersona == idPersona)?.idCategoriaInterna;

            movimiento.isValidado = idCategoriaInterna == 24; // Si es JEFE DE MANTENIMIENTO (24) se valida automáticamente
        }
        else
        {
            movimiento.isValidado = null;
        }

        #endregion

        Response_IUD_tblMovimientoRecambio response = await spIUD_tblMovimientoRecambio(movimiento, "post");

        if (!response.resultado)
            return BadRequest();

        HandleCambioEstadoMovimientoRecambio(response.idMovimientoRecambio, movimiento);

        await db.SaveChangesAsync();

        return Ok(response.idMovimientoRecambio);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] tblMovimientoRecambio movimiento, [FromODataUri] bool isReplaceRecambio = true, [FromODataUri] bool modificarRecambios = true)
    {
        HandleCambioEstadoMovimientoRecambio(key, movimiento);

        if (!modificarRecambios)
        {
            var movimientoActual = db.tblMovimientoRecambio.FirstOrDefault(x => x.idMovimientoRecambio == key);

            if (movimientoActual == null)
            {
                return BadRequest();
            }

            movimientoActual.idAlmacenOrigen = movimiento.idAlmacenOrigen;
            movimientoActual.idAlmacenDestino = movimiento.idAlmacenDestino;
            movimientoActual.idProveedor = movimiento.idProveedor;
            movimientoActual.clienteDestino = movimiento.clienteDestino;
            movimientoActual.fecha = movimiento.fecha;
            movimientoActual.idTipoMovimientoRecambio = movimiento.idTipoMovimientoRecambio;
            movimientoActual.isInventario = movimiento.isInventario;
            movimientoActual.observaciones = movimiento.observaciones;
            movimientoActual.numPedidoAsociado = movimiento.numPedidoAsociado;
            movimientoActual.codigoAlbaranProveedor = movimiento.codigoAlbaranProveedor;
            movimientoActual.numRegistro = movimiento.numRegistro;
            movimientoActual.isValidado = movimiento.isValidado;
            movimientoActual.idEstadoMovimientoRecambio = movimiento.idEstadoMovimientoRecambio;

            await db.SaveChangesAsync();

            return Ok(key);
        }

        Response_IUD_tblMovimientoRecambio response = await spIUD_tblMovimientoRecambio(movimiento, "patch", isReplaceRecambio);

        if (!response.resultado)
            return BadRequest();

        await db.SaveChangesAsync();

        return Ok(response.idMovimientoRecambio);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var movimiento = db.tblMovimientoRecambio.Include(mr => mr.tblRecambioNMovimientoRecambio).FirstOrDefault(x => x.idMovimientoRecambio == key);

        if (movimiento == null)
        {
            return BadRequest();
        }

        var response = await spIUD_tblMovimientoRecambio(movimiento, "delete");

        if (!response.resultado)
        {
            return BadRequest();
        }

        return Ok(response.idMovimientoRecambio);
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult> fn_isNumPedidoAsociadoExists([FromODataUri] string numPedidoAsociado, [FromODataUri] int? idMovimientoRecambio, [FromODataUri] int? idAlmacen, [FromODataUri] int idTipoMovimientoRecambio)
    {
        var movRec = db.tblMovimientoRecambio
            .Include(mr => mr.idAlmacenOrigenNavigation)
            .Include(mr => mr.idAlmacenDestinoNavigation)
            .FirstOrDefault(mr => mr.idMovimientoRecambio == idMovimientoRecambio);

        int? idAlmacenPadre;

        if (movRec == null)
        {
            var almacen = db.tblAlmacenRecambios.FirstOrDefault(ar => ar.idAlmacen == idAlmacen);

            idAlmacenPadre = almacen?.idAlmacenPadre;

            idAlmacenPadre ??= almacen?.idAlmacen;
        }
        else
        {
            idAlmacenPadre = movRec?.idAlmacenOrigenNavigation?.idAlmacenPadre ?? movRec?.idAlmacenDestinoNavigation?.idAlmacenPadre;

            idAlmacenPadre ??= movRec?.idAlmacenOrigen ?? movRec?.idAlmacenDestino;
        }

        if (idAlmacenPadre == null)
        {
            return BadRequest();
        }

        bool isNumPedidoAsociadoExists = db.tblMovimientoRecambio.Any(mr =>
            mr.numPedidoAsociado == numPedidoAsociado
            && (idMovimientoRecambio == null || mr.idMovimientoRecambio != idMovimientoRecambio)
            && mr.idTipoMovimientoRecambio == idTipoMovimientoRecambio
            && (
                (mr.idAlmacenOrigenNavigation != null && (mr.idAlmacenOrigenNavigation.idAlmacenPadre ?? mr.idAlmacenOrigen) == idAlmacenPadre)
                || (mr.idAlmacenDestinoNavigation != null && (mr.idAlmacenDestinoNavigation.idAlmacenPadre ?? mr.idAlmacenDestino) == idAlmacenPadre)
            )
        );

        return Ok(isNumPedidoAsociadoExists);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> fn_isNumRegistroExists([FromODataUri] string numRegistro, [FromODataUri] int? idMovimientoRecambio)
    {
        return Ok(db.tblMovimientoRecambio.Where(x => x.numRegistro == numRegistro && ((idMovimientoRecambio != null && x.idMovimientoRecambio != idMovimientoRecambio) || idMovimientoRecambio == null)).Count() > 0);
    }

    public async Task<List<Result_spGet_CantPrecio_MovimientoRecambio>> spGet_CantPrecioUbicacion_MovimientoRecambio(int? idAlmacenOrigen, int? idAlmacenDestino, string? campoBusqueda, DateTimeOffset? fecha, int? idTipoMovimientoRecambio, int? idProveedor, int? idMovimientoRecambio, bool isConsultaMovimiento = false)
    {

        /// Almacen destino, fecha , tipo movimiento , idMovimientoRecambio
        var connection = db.Database.GetDbConnection();
        var result = (await connection.QueryAsync<Result_spGet_CantPrecio_MovimientoRecambio>("EXEC [Assistant].[EF_recambios_spGet_CantPrecioUbicacion_MovimientoRecambio] @idAlmacenOrigen, @idAlmacenDestino, @campoBusqueda, @fecha, @idTipoMovimientoRecambio, @idProveedor, @idMovimientoRecambio, @isConsultaMovimiento",
            new
            {
                idAlmacenOrigen,
                idAlmacenDestino,
                campoBusqueda,
                fecha = fecha?.DateTime,
                idTipoMovimientoRecambio,
                idProveedor,
                idMovimientoRecambio,
                isConsultaMovimiento
            })).ToList();

        return result;
    }

    private void HandleCambioEstadoMovimientoRecambio(int key, tblMovimientoRecambio movimiento)
    {
        int idUsuario = int.Parse(HttpContext.Items["idUsuario"].ToString());

        List<int> idsTipoMovimientoRecambioCustom = new()
        {
            (int)idsTipoMovimientoRecambio.Abono,
            (int)idsTipoMovimientoRecambio.Garantia,
            (int)idsTipoMovimientoRecambio.Regularizacion
        };

        if (!idsTipoMovimientoRecambioCustom.Contains(movimiento.idTipoMovimientoRecambio))
        {
            return;
        }

        var entity = db.tblMovimientoRecambio.FirstOrDefault(mr => mr.idMovimientoRecambio == key);

        if (entity == null || entity.idEstadoMovimientoRecambio == null || movimiento.idEstadoMovimientoRecambio == null || (entity.idEstadoMovimientoRecambio == movimiento.idEstadoMovimientoRecambio && movimiento.idMovimientoRecambio != 0))
        {
            return;
        }

        db.tblEstadoMovimientoRecambioNMovimientoRecambio.Add(new()
        {
            idEstadoMovimientoRecambio = (byte)movimiento.idEstadoMovimientoRecambio,
            idMovimientoRecambio = key,
            fecha = DateTimeOffset.Now,
            idUsuario = idUsuario
        });
    }

    public async Task<Response_IUD_tblMovimientoRecambio> spIUD_tblMovimientoRecambio(tblMovimientoRecambio movimiento, string tipoAccion, bool isReplaceRecambio = true)
    {
        #region Tabla Movimiento
        DataTable dtMovimientoRecambio = new DataTable();
        dtMovimientoRecambio.SetTypeName("Assistant.ttMovimientoRecambio");
        dtMovimientoRecambio.Columns.Add("idMovimientoRecambio", typeof(int));
        dtMovimientoRecambio.Columns.Add("idEstadoMovimientoRecambio", typeof(byte));
        dtMovimientoRecambio.Columns.Add("idAlmacenOrigen", typeof(int));
        dtMovimientoRecambio.Columns.Add("idProveedor", typeof(int));
        dtMovimientoRecambio.Columns.Add("idAlmacenDestino", typeof(int));
        dtMovimientoRecambio.Columns.Add("clienteDestino", typeof(string));
        dtMovimientoRecambio.Columns.Add("fecha", typeof(DateTimeOffset));
        dtMovimientoRecambio.Columns.Add("idTipoMovimientoRecambio", typeof(int));
        dtMovimientoRecambio.Columns.Add("isInventario", typeof(bool));
        dtMovimientoRecambio.Columns.Add("observaciones", typeof(string));
        dtMovimientoRecambio.Columns.Add("numPedidoAsociado", typeof(string));
        dtMovimientoRecambio.Columns.Add("codigoAlbaranProveedor", typeof(string));
        dtMovimientoRecambio.Columns.Add("numRegistro", typeof(string));
        dtMovimientoRecambio.Columns.Add("isValidado", typeof(bool));

        var row = dtMovimientoRecambio.NewRow();
        row["idMovimientoRecambio"] = tipoAccion == "post" ? -1 : movimiento.idMovimientoRecambio;
        if (movimiento.idEstadoMovimientoRecambio != null) row["idEstadoMovimientoRecambio"] = movimiento.idEstadoMovimientoRecambio;
        if (movimiento.idAlmacenOrigen != null) row["idAlmacenOrigen"] = movimiento.idAlmacenOrigen;
        if (movimiento.idProveedor != null) row["idProveedor"] = movimiento.idProveedor;
        if (movimiento.idAlmacenDestino != null) row["idAlmacenDestino"] = movimiento.idAlmacenDestino;
        if (movimiento.clienteDestino != null) row["clienteDestino"] = movimiento.clienteDestino;
        if (movimiento.fecha != null) row["fecha"] = movimiento.fecha;
        if (movimiento.idTipoMovimientoRecambio != null) row["idTipoMovimientoRecambio"] = movimiento.idTipoMovimientoRecambio;
        if (movimiento.isInventario != null) row["isInventario"] = movimiento.isInventario;
        if (movimiento.observaciones != null) row["observaciones"] = movimiento.observaciones;
        if (movimiento.numPedidoAsociado != null) row["numPedidoAsociado"] = movimiento.numPedidoAsociado;
        if (movimiento.codigoAlbaranProveedor != null) row["codigoAlbaranProveedor"] = movimiento.codigoAlbaranProveedor;
        if (movimiento.numRegistro != null) row["numRegistro"] = movimiento.numRegistro;
        if (movimiento.isValidado != null) row["isValidado"] = movimiento.isValidado;
        dtMovimientoRecambio.Rows.Add(row);
        #endregion

        #region Tabla RecambioNMovimientoRecambio
        DataTable dtRecambioNMovimientoRecambio = new DataTable();
        dtRecambioNMovimientoRecambio.SetTypeName("Assistant.ttRecambioNMovimientoRecambio");
        dtRecambioNMovimientoRecambio.Columns.Add("idRecambioNMovimientoRecambioAsociado", typeof(int));
        dtRecambioNMovimientoRecambio.Columns.Add("idRecambio", typeof(int));
        dtRecambioNMovimientoRecambio.Columns.Add("idMovimientoRecambio", typeof(int));
        dtRecambioNMovimientoRecambio.Columns.Add("cantidad", typeof(int));
        dtRecambioNMovimientoRecambio.Columns.Add("ubicacion", typeof(string));
        dtRecambioNMovimientoRecambio.Columns.Add("referenciaProveedor", typeof(string));
        dtRecambioNMovimientoRecambio.Columns.Add("precio", typeof(decimal));
        dtRecambioNMovimientoRecambio.Columns.Add("cantidadTeorico", typeof(int));
        dtRecambioNMovimientoRecambio.Columns.Add("isApp", typeof(bool));
        dtRecambioNMovimientoRecambio.Columns.Add("fecha", typeof(DateTimeOffset));
        dtRecambioNMovimientoRecambio.Columns.Add("idUsuario", typeof(int));

        foreach (var recambio in movimiento.tblRecambioNMovimientoRecambio)
        {
            var rowRec = dtRecambioNMovimientoRecambio.NewRow();
            if (recambio.idRecambioNMovimientoRecambioAsociado != null) rowRec["idRecambioNMovimientoRecambioAsociado"] = recambio.idRecambioNMovimientoRecambioAsociado;
            rowRec["idRecambio"] = recambio.idRecambio;
            rowRec["idMovimientoRecambio"] = tipoAccion == "post" ? -1 : movimiento.idMovimientoRecambio;
            if (recambio.cantidad != null) rowRec["cantidad"] = recambio.cantidad;
            if (recambio.ubicacion != null) rowRec["ubicacion"] = recambio.ubicacion;
            if (recambio.referenciaProveedor != null) rowRec["referenciaProveedor"] = recambio.referenciaProveedor;
            if (recambio.precio != null) rowRec["precio"] = recambio.precio;
            if (recambio.cantidadTeorico != null) rowRec["cantidadTeorico"] = recambio.cantidadTeorico;
            rowRec["isApp"] = recambio.isApp;
            if (recambio.fecha != null) rowRec["fecha"] = recambio.fecha;
            if (recambio.idUsuario != null) rowRec["idUsuario"] = recambio.idUsuario;
            dtRecambioNMovimientoRecambio.Rows.Add(rowRec);
        }
        #endregion

        var connection = db.Database.GetDbConnection();

        Response_IUD_tblMovimientoRecambio response = new();

        using (var result = await connection.QueryMultipleAsync("EXEC [Assistant].[EF_recambios_spIUD_tblMovimientoRecambio] @ttMovimientoRecambio, @ttRecambioNMovimientoRecambio, @tipoAccion, @isReplaceRecambio", new { ttMovimientoRecambio = dtMovimientoRecambio, ttRecambioNMovimientoRecambio = dtRecambioNMovimientoRecambio, tipoAccion, isReplaceRecambio }))
        {
            response.resultado = result.Read<Resultado>().ToList().FirstOrDefault()?.resultado ?? false;
            response.idMovimientoRecambio = result.Read<IdMovimientoRecambio>().ToList().FirstOrDefault()?.idMovimientoRecambio ?? -1;
            response.historicoRecambio = result.Read<HistoricoRecambio>().ToList();
        }

        return response;
    }

    public class Result_spGet_CantPrecio_MovimientoRecambio
    {
        public int idAlmacen { get; set; }
        public int idRecambio { get; set; }
        public int? idRecambioNProveedor { get; set; }
        public int? min { get; set; }
        public int? max { get; set; }
        public decimal precio { get; set; }
        public string? ubicacion { get; set; }
    }

    public class Response_IUD_tblMovimientoRecambio
    {
        public bool resultado { get; set; }
        public int idMovimientoRecambio { get; set; }
        public List<HistoricoRecambio>? historicoRecambio { get; set; }
    }

    private class Resultado
    {
        public bool resultado { get; set; }
    }

    private class IdMovimientoRecambio
    {
        public int idMovimientoRecambio { get; set; }
    }

    public class HistoricoRecambio
    {
        public string? ID { get; set; }
        public DateTimeOffset? fecha { get; set; }
        public string? origen { get; set; }
        public string? destino { get; set; }
        public string? denoLavanderia { get; set; }
        public int? cantidad { get; set; }
        public decimal? precio { get; set; }
        public decimal? importe { get; set; }
        public int? idAlmacen { get; set; }
        public string? denoAlmacen { get; set; }
        public int? idRecambio { get; set; }
        public string? denoRecambio { get; set; }
        public string? tipoMovimiento { get; set; }
        public int? idTipoMovimientoRecambioOriginal { get; set; }
        public int? idTipoMovimientoRecambio { get; set; }
        public string? codigoMoneda { get; set; }
    }
}
