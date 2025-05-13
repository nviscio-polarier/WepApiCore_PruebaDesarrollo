using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;
using static WebApiCore.Controllers.AssistantController;

namespace WebApiCore.Controllers;
public class RecambiosController : ODataController
{
    private readonly bdERP db;
    private readonly AssistantController ac;
    public RecambiosController(bdERP context)
    {
        db = context;
        ac = new(context);
    }

    [HttpGet("odata/MyPolarier/Assistant/Recambios/Get_tblRecambioNAlmacenRecambios")]
    [Authorize]
    public async Task<ActionResult> Get_tblRecambioNAlmacenRecambios([FromODataUri] int idRecambio, [FromODataUri] DateTimeOffset fecha)
    {
        var getInfoActualResult = await ac.GetInfoActual(fecha, null, idRecambio);

        if (getInfoActualResult is OkObjectResult okObjectResult && okObjectResult.Value != null)
        {
            var infoActual = (List<Result_InfoActual>)okObjectResult.Value;

            var tblRecambioNAlmacenRecambios = db.tblRecambioNAlmacenRecambios.Where(rnar => rnar.idRecambio == idRecambio).ToList();

            var result = (
                from rnar in tblRecambioNAlmacenRecambios
                join ia in infoActual
                on new { rnar.idAlmacen, rnar.idRecambio } equals new { ia.idAlmacen, ia.idRecambio } into iaGroup
                from ia in iaGroup.DefaultIfEmpty()
                join ar in db.tblAlmacenRecambios
                on rnar.idAlmacen equals ar.idAlmacen
                join arP in db.tblAlmacenRecambios
                on ar.idAlmacenPadre equals arP.idAlmacen into arPGroup
                from arP in arPGroup.DefaultIfEmpty()
                join p in db.tblPais
                on ar.idPais equals p.idPais
                join m in db.tblMoneda
                on p.idMoneda equals m.idMoneda
                select new
                {
                    ar.idAlmacen,
                    idRecambio,
                    ar.idAlmacenPadre,
                    rnar.ubicacion,
                    cantidadPrincipal = ia != null ? ia.cantidadPrincipal : null,
                    cantidadSecundarios = ia != null ? ia.cantidadSecundarios : null,
                    precio = ia != null ? ia.precio : null,
                    codigoMoneda = m.codigo,
                    denoAlmacen = ar.denominacion,
                    denoAlmacenPadre = arP == null ? "GENERALES" : arP.denominacion,
                }
            );

            return Ok(result);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost("odata/MyPolarier/Assistant/Recambios/CheckReferencia")]
    [Authorize]
    public ActionResult CheckReferencia([FromODataUri] int? idRecambio, [FromODataUri] string referencia)
    {
        var exists = db.tblRecambio
            .Any(r =>
                r.referencia == referencia &&
                (idRecambio == null || r.idRecambio != idRecambio)
            );

        return Ok(!exists);
    }

    [HttpPost("odata/MyPolarier/Assistant/Recambios/CheckReferenciaInterna")]
    [Authorize]
    public ActionResult CheckReferenciaInterna([FromODataUri] int? idRecambio, [FromODataUri] string referenciaInterna)
    {
        var exists = db.tblRecambio
            .Any(r =>
                r.referenciaInterna == referenciaInterna &&
                (idRecambio == null || r.idRecambio != idRecambio)
            );

        return Ok(!exists);
    }
}
