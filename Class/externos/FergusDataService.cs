using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApiCore.Context;

namespace WebApiCore.Class.externos
{
    public class FergusDataService
    {
        private readonly Context.bdERP db;

        //<idEntidad, hotelId>
        private static readonly Dictionary<int, string> tablaConversion = new()
        {
            {636, "TENT CAPI"},
            {637, "TENT PALMANOVA"},
            {638, "TENT PLAYA DE PALMA"},
            {639, "TAL"},
            {640, "BAHIA DE PALMA"},
            {650, "BAHAMAS"},
            {658, "TOBAGO"},
            {659, "CALA BLANCA"},
            {689, "BERMUDAS"},
            {696, "CLUB MALLORCA"}, //MCA. WATERPARK SPLASH EDF1
            {694, "CLUB PALMANOVA PARK"},
            {693, "SOLLER BEACH"},
            {698, "TENT CALVIA BEACH"},
            {699, "TENT MOJITO SUITES"}
        };

        public readonly List<int> entidades = tablaConversion.Keys.ToList();

        private DateTime fechaDesde;
        private DateTime fechaHasta;

        public FergusDataService(Context.bdERP _db, DateTime _fechaDesde, DateTime _fechaHasta)
        {
            db = _db;
            fechaDesde = _fechaDesde;
            fechaHasta = _fechaHasta;
        }
        public async Task ImportarEstancias()
        {
            try
            {

                var cierres = db.tblCierreFactEntidad   // Calculo de fechas de cierre facturación entidad
                    .Where(cierre =>
                        entidades.Contains(cierre.idEntidad)
                        && (
                            (cierre.fechaDesde >= fechaDesde && cierre.fechaDesde <= fechaHasta)
                            || (cierre.fechaHasta >= fechaDesde && cierre.fechaHasta <= fechaHasta)
                            || (fechaDesde >= cierre.fechaDesde && fechaDesde <= cierre.fechaHasta)
                            || (fechaHasta >= cierre.fechaDesde && fechaHasta <= cierre.fechaHasta)
                            )
                    ).ToList();

                await ImportarEstancias(cierres);

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
        }

        public async Task ImportarEstancias(List<tblCierreFactEntidad> cierres)
        {
            foreach (KeyValuePair<int, string> ent in tablaConversion)
            {
                string estancias_url = "https://wsavalon.fergushotels.com:10349/AWWebAPI/api/aw/awa/sta/GetEstancias?hotel={0}&fechaDesde={1}&fechaHasta={2}";
                estancias_url = String.Format(estancias_url, ent.Value, fechaDesde.ToString("yyyy-MM-dd"), fechaHasta.ToString("yyyy-MM-dd"));

                try
                {
                    var response = new fergus_estanciasResponse();
                    using (HttpClient client = new HttpClient())
                    {
                        using (HttpResponseMessage res = await client.GetAsync(estancias_url))
                        {
                            if (res.IsSuccessStatusCode)
                            {
                                using (HttpContent contentResult = res.Content)
                                {
                                    string data = await contentResult.ReadAsStringAsync();
                                    response = JsonConvert.DeserializeObject<fergus_estanciasResponse>(data);
                                }
                            }
                        }
                    }

                    if (response.LSEstancias == null || response.LSEstancias.Length == 0)
                    {
                        continue;
                    }

                    var estancias = response.LSEstancias
                                        .Where(entidadEstancias =>
                                            !cierres
                                            .Where(cierre => cierre.idEntidad == ent.Key)
                                            .Any(cierre => entidadEstancias.Fecha.Date >= cierre.fechaDesde.Date && entidadEstancias.Fecha.Date <= cierre.fechaHasta.Date)
                                        )
                                        .Select(cl => new
                                        {
                                            idEntidad = ent.Key,
                                            fecha = cl.Fecha.Date,
                                            estancias = cl.AD + cl.JR + cl.NI,
                                        }).ToList();

                    var estanciasExistentes = await db.tblEstancia
                    .Where(x => entidades.Contains(x.idEntidad) && x.fecha >= fechaDesde && x.fecha <= fechaHasta)
                    .ToListAsync();

                    foreach (var estanciaExistente in estanciasExistentes)
                    {
                        var newEstancia = estancias.FirstOrDefault(cl => estanciaExistente.idEntidad == cl.idEntidad && estanciaExistente.fecha == cl.fecha);
                        if (newEstancia != null)
                        {
                            if (newEstancia.fecha < DateTime.Now.Date)
                            {
                                estanciaExistente.estanciasReal = newEstancia.estancias;
                            }
                            else
                            {
                                estanciaExistente.estanciasPrevistas = newEstancia.estancias;
                            }
                        }
                    }

                    var estanciasNuevas = estancias.Where(cl => !estanciasExistentes.Any(x => x.idEntidad == cl.idEntidad && x.fecha == cl.fecha))
                        .Select(cl => new tblEstancia()
                        {
                            idEntidad = cl.idEntidad,
                            fecha = cl.fecha,
                            estanciasReal = cl.fecha < DateTime.Now.Date ? cl.estancias : 0,
                            estanciasPrevistas = cl.fecha >= DateTime.Now.Date ? cl.estancias : 0,
                            salidas = 0
                        }).ToList();

                    db.tblEstancia.AddRange(estanciasNuevas);
                }
                catch (Exception ex)
                {

                    db.tblLogError.Add(new tblLogError
                    {
                        denominacion = "EstanciasWorker - Importar",
                        error = JsonConvert.SerializeObject(new
                        {
                            compania = "Fergus",
                            entidad = ent.Key,
                            url = estancias_url,
                            fecha = DateTime.Now,
                            mensaje = ex.Message
                        })
                    });

                    // Lanzar excepcion para controlar asíncronamente
                    // throw new ImportarEstanciaException(ex.Message, "Fergus", estancias_url, DateTime.Now, ent.Key);
                }
            }
        }

        private class fergus_estanciasResponse
        {
            public Estancia[] LSEstancias { get; set; }
            public string Warning { get; set; }
        }

        public class Estancia
        {
            public DateTime Fecha { get; set; }
            public int AD { get; set; }
            public int JR { get; set; }
            public int NI { get; set; }
            public int CU { get; set; }
        }
    }
}
