using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using System.Globalization;
using WebApiCore.Context;
using WebApiCore.Enums.General;
using WebApiCore.Security;

namespace WebApiCore.Controllers.externos;

public class temporalController : ODataController
{
    private readonly bdERP db;

    public temporalController(bdERP context)
    {
        db = context;
    }

    private void PostTasaCambio(string monedaDestino, string fecha, string formatoFecha, decimal tasaCambio)
    {
        if (monedaDestino == null || fecha == null || tasaCambio == 0)
        {
            throw new Exception("Debe especificar la moneda destino, fecha y tasa de cambio");
        }

        if (db.tblTasaCambio.Any(x => x.idMonedaDestino == db.tblMoneda.Where(x => x.codigo == monedaDestino).Select(x => x.idMoneda).Single() && x.fecha.Equals((Date)DateTime.ParseExact(fecha, formatoFecha, CultureInfo.InvariantCulture))))
        {
            throw new Exception("Ya existe una tasa de cambio para la moneda destino y fecha especificada");
        }

        var idMonedaDestino = db.tblMoneda.Where(x => x.codigo == monedaDestino).Select(x => x.idMoneda).Single();
        var b = (Date)DateTime.ParseExact(fecha, formatoFecha, CultureInfo.InvariantCulture);
        var c = tasaCambio;
        tblTasaCambio entity = new()
        {
            idMonedaOrigen = (byte)idsMoneda.Euro, // Todo: Cambiar por la moneda origen REAL
            idMonedaDestino = db.tblMoneda.Where(x => x.codigo == monedaDestino).Select(x => x.idMoneda).Single(),
            fecha = (Date)DateTime.ParseExact(fecha, formatoFecha, CultureInfo.InvariantCulture),
            tasaCambio = tasaCambio
        };
        db.tblTasaCambio.Add(entity);
    }

    // Copiar registros de produccion a produccion generica
    /*
    [EnableQuery]
    [HttpGet("odata/ConvertirProduccionGenerica")]
    [Authorize]
    public async Task<ActionResult> temp()
    {

        var result = db.tblProduccion
            .Include(prod => prod.tblPrendaNProduccion)
                .ThenInclude(pre => pre.idPrendaNavigation)
            .Include(prod => prod.tblPrendaNProduccion)
                .ThenInclude(pre => pre.tblRechazoNProduccion)
            .Where(prod =>
            prod.idCompañia == 92 &&
            prod.fecha.Year == 2024 &&
            prod.fecha.Month == 8 &&
            prod.tblPrendaNProduccion.Any(pre => pre.idPrendaNavigation.idPlantillaPrenda_generica != null)
            )
            .Select(x => new tblProduccion
            {
                fecha = x.fecha,
                idxGsbs = x.idxGsbs,
                idTipoProduccion = x.idTipoProduccion,
                idEntidad = null,
                idCompañia = null,
                idMaquina = x.idMaquina,
                mac = x.mac,
                idProduccionPantalla = x.idProduccionPantalla,
                idLavanderia = x.idLavanderia,
                idTurno = x.idTurno,
                idGrupoPlantillaPrenda_generica = 1,
                tblPrendaNProduccion = x.tblPrendaNProduccion
                    .Where(pre => pre.idPrendaNavigation.idPlantillaPrenda_generica != null)
                    .Select(y => new tblPrendaNProduccion
                    {
                        idPrenda = null,
                        hora = y.hora,
                        idLineaPantalla = y.idLineaPantalla,
                        Producido = y.Producido,
                        Rechazo = y.Rechazo,
                        Retiro = y.Retiro,
                        unidadesRepartidas = y.unidadesRepartidas,
                        idPlantillaPrenda_generica = y.idPrendaNavigation.idPlantillaPrenda_generica,
                        tblRechazoNProduccion = y.tblRechazoNProduccion.Select(x => new tblRechazoNProduccion
                        {
                            idTipoRechazo = x.idTipoRechazo,
                            Cantidad = x.Cantidad,
                        }).ToList(),
                    }).ToList()
            });

        var str = result.ToQueryString();

        db.tblProduccion.AddRange(result);

        db.SaveChanges();

        return Ok(new { str, result});
    }
    */

    [EnableQuery]
    [HttpGet("odata/Get_MXN")]
    [Authorize]
    public async Task<ActionResult> GetMXN()
    {
        HttpClient client = new()
        {
            BaseAddress = new Uri("https://www.banxico.org.mx/SieAPIRest/service/v1/series/SF46410/datos/2016-01-01/" + DateTime.Now.ToString("yyyy-MM-dd")),
            DefaultRequestHeaders =
            {
                { "Bmx-Token", "9f185bafb9f62fa4ddc0956fba36c3de1806581a0f77b49184b118f50c0c38be"}
            }
        };

        var result = await client.GetAsync("");
        var jsonbody = JsonConvert.DeserializeObject<BanxicoResponse.Rootobject>(result.Content.ReadAsStringAsync().Result);

        if (jsonbody == null || jsonbody.bmx.series.Length == 0)
        {
            return BadRequest("No se encontraron datos");
        }

        foreach (var dato in jsonbody.bmx.series[0].datos)
        {
            if(dato.dato == "N/E")
            {
                continue;
            };
            decimal tasaCambio = decimal.Parse(dato.dato, CultureInfo.InvariantCulture);
            PostTasaCambio("MXN", dato.fecha, "dd/MM/yyyy", tasaCambio);
        }

        db.SaveChanges();
        return Ok(jsonbody.bmx.series[0].datos);
    }

    [EnableQuery]
    [HttpGet("odata/Get_USD")]
    [Authorize]
    public async Task<ActionResult> GetUSD()
    {
        HttpClient client = new()
        {
            BaseAddress = new Uri("https://data-api.ecb.europa.eu/service/data/EXR/D.USD.EUR.SP00.A?format=jsondata&detail=dataonly&startPeriod=2016-01-01")
        };

        var result = await client.GetAsync("");
        var jsonbody = JsonConvert.DeserializeObject<EurCentResponse.Root>(result.Content.ReadAsStringAsync().Result);

        if (jsonbody == null)
        {
            return BadRequest("No se encontraron datos");
        }

        foreach (var observations in jsonbody.dataSets[0].series.Values)
        {
            foreach (var keyValuePair in observations.observations)
            {
                PostTasaCambio("USD", jsonbody.structure.dimensions.observation[0].values[int.Parse(keyValuePair.Key)].id, "yyyy-MM-dd", keyValuePair.Value[0]);
            }
        }

        db.SaveChanges();
        return Ok(jsonbody);
    }

    [EnableQuery]
    [HttpGet("odata/Get_GBP")]
    [Authorize]
    public async Task<ActionResult> GetGBP()
    {
        HttpClient client = new()
        {
            BaseAddress = new Uri("https://data-api.ecb.europa.eu/service/data/EXR/D.GBP.EUR.SP00.A?format=jsondata&detail=dataonly&startPeriod=2016-01-01")
        };

        var result = await client.GetAsync("");
        var jsonbody = JsonConvert.DeserializeObject<EurCentResponse.Root>(result.Content.ReadAsStringAsync().Result);

        if (jsonbody == null)
        {
            return BadRequest("No se encontraron datos");
        }

        foreach (var observations in jsonbody.dataSets[0].series.Values)
        {
            foreach (var keyValuePair in observations.observations)
            {
                PostTasaCambio("GBP", jsonbody.structure.dimensions.observation[0].values[int.Parse(keyValuePair.Key)].id, "yyyy-MM-dd", keyValuePair.Value[0]);
            }
        }

        db.SaveChanges();
        return Ok(jsonbody);
    }

    [EnableQuery]
    [HttpGet("odata/Get_RD")]
    [Authorize]
    public async Task<ActionResult> GetRD()
    {
        //HttpClient client = new()
        //{
        //    BaseAddress = new Uri("https://www.banxico.org.mx/SieAPIRest/service/v1/series/SF46410/datos"),
        //    DefaultRequestHeaders =
        //    {
        //        { "Bmx-Token", "9f185bafb9f62fa4ddc0956fba36c3de1806581a0f77b49184b118f50c0c38be"}
        //    }
        //};

        //var result = await client.GetAsync("");
        //var jsonbody = JsonConvert.DeserializeObject<BanxicoResponse.Rootobject>(result.Content.ReadAsStringAsync().Result);

        //if (jsonbody == null || jsonbody.bmx.series.Length == 0)
        //{
        //    return BadRequest("No se encontraron datos");
        //}

        //foreach (var dato in jsonbody.bmx.series[0].datos)
        //{
        //    decimal tasaCambio = decimal.Parse(dato.dato);
        //    PostTasaCambio("MXN", dato.fecha, tasaCambio);
        //}

        //return Ok(jsonbody.bmx.series[0].datos);
        return Ok();
    }

    private class BanxicoResponse
    {

        public class Rootobject
        {
            public Bmx bmx { get; set; }
        }

        public class Bmx
        {
            public Series[] series { get; set; }
        }

        public class Series
        {
            public string idSerie { get; set; }
            public string titulo { get; set; }
            public Dato[] datos { get; set; }
        }

        public class Dato
        {
            public string fecha { get; set; }
            public string dato { get; set; }
        }

    }

    private class EurCentResponse
    {
        public class Header
        {
            public string id { get; set; }
            public bool test { get; set; }
            public DateTime prepared { get; set; }
            public Sender sender { get; set; }
        }

        public class Sender
        {
            public string id { get; set; }
        }

        public class Series
        {
            public Dictionary<string, List<decimal>> observations { get; set; }
        }

        public class DataSet
        {
            public string action { get; set; }
            public DateTime validFrom { get; set; }
            public Dictionary<string, Series> series { get; set; }
        }

        public class Link
        {
            public string title { get; set; }
            public string rel { get; set; }
            public string href { get; set; }
        }

        public class Value
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class SeriesDimension
        {
            public string id { get; set; }
            public string name { get; set; }
            public List<Value> values { get; set; }
        }

        public class ObservationDimension
        {
            public string id { get; set; }
            public string name { get; set; }
            public string role { get; set; }
            public List<Value> values { get; set; }
        }

        public class Dimensions
        {
            public List<SeriesDimension> series { get; set; }
            public List<ObservationDimension> observation { get; set; }
        }

        public class Structure
        {
            public List<Link> links { get; set; }
            public string name { get; set; }
            public Dimensions dimensions { get; set; }
        }

        public class Root
        {
            public Header header { get; set; }
            public List<DataSet> dataSets { get; set; }
            public Structure structure { get; set; }
        }
    }
}
