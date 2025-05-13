using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using System.Globalization;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTasaCambioController : ODataController
{
    private readonly bdERP db;

    public tblTasaCambioController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public ActionResult Get()
    {
        return Ok(db.tblTasaCambio);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromODataUri] string monedaOrigen, [FromODataUri] string monedaDestino, [FromODataUri] string fecha, [FromODataUri] decimal tasaCambio)
    {
        if(monedaOrigen == null || monedaDestino == null || fecha == null || tasaCambio == 0)
        {
            return BadRequest("Debe especificar la moneda origen, moneda destino, fecha y tasa de cambio");
        }

        if(db.tblTasaCambio.Any(x =>
            x.idMonedaDestino == db.tblMoneda.Where(x => x.codigo == monedaDestino).Select(x => x.idMoneda).Single()
            && x.idMonedaOrigen == db.tblMoneda.Where(x => x.codigo == monedaOrigen).Select(x => x.idMoneda).Single()
            && x.fecha.Equals((Date)DateTime.ParseExact(fecha, "yyyyMMdd", CultureInfo.InvariantCulture))
            ))
        {
            return BadRequest("Ya existe una tasa de cambio para la moneda origen, moneda destino y fecha especificada");
        }

        tblTasaCambio entity = new()
        {
            idMonedaOrigen = db.tblMoneda.Where(x => x.codigo == monedaOrigen).Select(x => x.idMoneda).Single(),
            idMonedaDestino = db.tblMoneda.Where(x => x.codigo == monedaDestino).Select(x => x.idMoneda).Single(),
            fecha = (Date)DateTime.ParseExact(fecha, "yyyyMMdd", CultureInfo.InvariantCulture),
            tasaCambio = tasaCambio
        };
        db.tblTasaCambio.Add(entity);
        await db.SaveChangesAsync();

        return Ok();
    }
}
