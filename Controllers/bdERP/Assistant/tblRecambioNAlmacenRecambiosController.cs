using Dapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

[Authorize]
public class tblRecambioNAlmacenRecambiosController : ODataController
{
    private readonly bdERP db;

    public tblRecambioNAlmacenRecambiosController(bdERP context)
    {
        db = context;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<IQueryable<tblRecambioNAlmacenRecambios>> Get([FromODataUri] int idRecambio, [FromODataUri] bool isBusquedaRecambio)
    {
        return db.tblRecambioNAlmacenRecambios.Where(x => x.idRecambio == idRecambio);
    }

    [HttpPatch]
    [EnableQuery]
    public async Task<ActionResult> Patch([FromODataUri] int keyidAlmacen, [FromODataUri] int keyidRecambio, [FromBody] JsonPatchDocument<tblRecambioNAlmacenRecambios> tblRecambioNAlmacenRecambios)
    {
        var entity = await db.tblRecambioNAlmacenRecambios.FirstOrDefaultAsync(rnar => rnar.idAlmacen == keyidAlmacen && rnar.idRecambio == keyidRecambio);

        if (entity == null)
        {
            return NotFound();
        }

        tblRecambioNAlmacenRecambios.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Updated(entity);
    }

    [HttpGet("odata/tblRecambioNAlmacenRecambios/GetPrecioCalculado")]
    [EnableQuery]
    public async Task<List<tblRecambioNAlmacenRecambios>> GetPrecioCalculado([FromODataUri] string idsRecambios, [FromODataUri] DateTime fecha, [FromODataUri] int? idTipoMovimientoRecambio, [FromODataUri] int? idAlmacenOrigen, [FromODataUri] int? idAlmacenDestino, [FromODataUri] string? filterString)
    {
        string[] numberStrings = idsRecambios.Split("|");
        List<int> recambios = Array.ConvertAll(numberStrings, int.Parse).ToList();

        #region Tabla recambios
        DataTable dtIdSimple = new DataTable();
        dtIdSimple.SetTypeName("General.ttIdSimple");
        dtIdSimple.Columns.Add("id", typeof(int));

        foreach (int idRecambio in recambios)
        {
            var rowRec = dtIdSimple.NewRow();
            rowRec["id"] = idRecambio;
            dtIdSimple.Rows.Add(rowRec);
        }
        #endregion


        var connection = db.Database.GetDbConnection();
        var result = await connection.QueryAsync<tblRecambioNAlmacenRecambios>("EXEC [Assistant].[EF_recambios_spGetPrecioMedioPondRecambio] @idAlmacenDestino, @idAlmacenOrigen, @fecha, 1, @idsRecambios, NULL",
            new
            {
                fecha = fecha,
                idAlmacenOrigen = idAlmacenOrigen,
                idAlmacenDestino = idAlmacenDestino,
                getLastInfo = true,
                idsRecambios = dtIdSimple
            });

        List<tblRecambioNAlmacenRecambios> resultFormat = result.Join(
            db.tblRecambio,
            x => x.idRecambio,
            y => y.idRecambio,
            (x, y) => new tblRecambioNAlmacenRecambios
            {
                idAlmacen = x.idAlmacen,
                idRecambio = x.idRecambio,
                cantidad = x.cantidad,
                ubicacion = x.ubicacion,
                precioMedioPonderado = x.precioMedioPonderado,
                idRecambioNavigation = new tblRecambio
                {
                    denominacion = y.denominacion,
                    referencia = y.referencia,
                    referenciaInterna = y.referenciaInterna
                }
            }).ToList();

        return resultFormat;
    }


    [HttpGet("odata/tblRecambioNAlmacenRecambios/GetPrecioHistoricoRecambio")]
    [EnableQuery]
    public async Task<string> GetPrecioHistoricoRecambio([FromODataUri] string idsRecambios, [FromODataUri] DateTime? fecha, [FromODataUri] int? idTipoMovimientoRecambio, [FromODataUri] int? idAlmacenOrigen, [FromODataUri] int? idAlmacenDestino, [FromODataUri] string? filterString, [FromODataUri] int? getLastInfo, [FromODataUri] int? getHistorico)
    {
        string[] numberStrings = idsRecambios.Split("|");
        List<int> recambios = Array.ConvertAll(numberStrings, int.Parse).ToList();

        #region Tabla recambios
        DataTable dtIdSimple = new DataTable();
        dtIdSimple.SetTypeName("General.ttIdSimple");
        dtIdSimple.Columns.Add("id", typeof(int));

        foreach (int idRecambio in recambios)
        {
            var rowRec = dtIdSimple.NewRow();
            rowRec["id"] = idRecambio;
            dtIdSimple.Rows.Add(rowRec);
        }
        #endregion

        var connection = db.Database.GetDbConnection();
        var result = await connection.QueryAsync<dynamic>("EXEC [Assistant].[EF_recambios_spGetPrecioMedioPondRecambio] @idAlmacenDestino, @idAlmacenOrigen, @fecha, @getLastInfo, @idsRecambios, @getHistorico",
            new
            {
                idAlmacenDestino = idAlmacenDestino,
                idAlmacenOrigen = idAlmacenOrigen,
                fecha = fecha,
                getLastInfo = getLastInfo != null ? getLastInfo : 1,
                idsRecambios = dtIdSimple,
                getHistorico = getHistorico
            });

        var formatedResult = JsonSerializer.Serialize(result);
        return formatedResult;
    }
}
