using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using WebApiCore.Context;

namespace WebApiCore.Class.externos
{
    public class UniversalDataService
    {
        private readonly Context.bdERP db;

        //<Establecimiento, idEntidad>

        public static readonly Dictionary<string, int> tablaConversion = new Dictionary<string, int>() {
            { "AQUAMARIN", 219},  // AQUAMARIN
            { "VILLAS", 219 }, // AQUAMARIN
            { "DON CAMILO", 220 },   // DON CAMILO
            { "LIDO", 221 }, // LIDO PARK
            { "FLORIDA", 222 },  // FLORIDA
            { "FLORIDITA", 222 },  // FLORIDA
            { "MARQUES", 223 },  // MARQUES
            { "ROMANTICA", 224 }, // ROMANTICA
            { "BIKINI", 225 },// BIKINI
            { "PERLA", 226 }, // PERLA 
            { "LAGUNA", 227 },// LAGUNA
            { "CASTELL", 228 },  // CASTELL ROYAL
            { "CBLANCO", 229 },  // CABO BLANCO   
            { "ELISA", 252 }, // APARTHOTEL ELISA  
            { "VILLAMAR", 644 }, // VILLA MARQUESA    
            { "APTLAGGAR", 645 },// LAGUNA APARTAMENTOS
            { "DON LEON", 654 } // DON LEÓN
        };

        public readonly List<int> entidades = tablaConversion.Values.ToList();

        private DateTime fechaDesde;
        private DateTime fechaHasta;

        public UniversalDataService(Context.bdERP _db, DateTime _fechaDesde, DateTime _fechaHasta)
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
                    .Where(cierre => entidades.Contains(cierre.idEntidad)
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
            string estancias_url = "";
            try
            {
                var response = new List<universal_estanciasResponse>();

                string token = "";
                using (HttpClient client = new HttpClient())
                {
                    //Autenticación
                    string login_url = "https://guestapi.universalbeachhotels.com/users/login?Username=polarier@universalhotels.es&Password=polarier.2024!";
                    using (HttpResponseMessage res = await client.PostAsync(login_url, null))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            using (HttpContent contentResult = res.Content)
                            {
                                string data = await contentResult.ReadAsStringAsync();
                                token = "Bearer " + JsonConvert.DeserializeObject<universal_loginResponse>(data).accessToken;
                            }
                        }
                    }
                    client.DefaultRequestHeaders.Add("Authorization", token);

                    foreach (KeyValuePair<string, int> ent in tablaConversion)
                    {
                        estancias_url = "https://guestapi.universalbeachhotels.com/stays/staysxpax?CenterCode={0}&DateFrom={1}&DateUntil={2}&Real=true&CustomerTypes=Cliente";
                        estancias_url = String.Format(estancias_url, ent.Key, fechaDesde.ToString("yyyy-MM-dd"), fechaHasta.ToString("yyyy-MM-dd"));

                        using (HttpResponseMessage res = await client.GetAsync(estancias_url))
                        {
                            if (res.IsSuccessStatusCode && res.StatusCode.Equals(HttpStatusCode.OK))
                            {
                                using (HttpContent contentResult = res.Content)
                                {
                                    string data = await contentResult.ReadAsStringAsync();
                                    response.AddRange(JsonConvert.DeserializeObject<List<universal_estanciasResponse>>(data));
                                }
                            }
                        }
                    }
                }

                var responseAgrupada = response
                .GroupBy(r => new { idEntidad = tablaConversion[r.center], fecha = r.date.Date })
                .Select(g => new
                {
                    g.Key.idEntidad,
                    g.Key.fecha,
                    ad = g.Sum(r => r.ad),
                    jr = g.Sum(r => r.jr),
                    ch = g.Sum(r => r.ch),
                    bb = g.Sum(r => r.bb)
                }).ToList();

                var estancias = responseAgrupada
                                    .Where(entidadEstancias =>
                                        !cierres.Any(cierre =>
                                        cierre.idEntidad.Equals(entidadEstancias.idEntidad) &&
                                        entidadEstancias.fecha >= cierre.fechaDesde.Date &&
                                        entidadEstancias.fecha <= cierre.fechaHasta.Date)
                                    )
                                    .Select(cl => new
                                    {
                                        idEntidad = cl.idEntidad,
                                        fecha = cl.fecha,
                                        estancias = cl.ad + cl.jr + cl.ch,
                                    }).ToList();

                var estFactAbierta = (from est in db.tblEstancia  // Estancias que se borrarán para su posterior inserción, cuya fecha esta fuera del cierre de facturación
                                      where entidades.Contains(est.idEntidad) && est.fecha >= fechaDesde && est.fecha <= fechaHasta
                                      select est).ToList()
                                            .Where(est => !cierres
                                                .Where(cierre => cierre.idEntidad == est.idEntidad)
                                                .Any(cierre => est.fecha.Date >= cierre.fechaDesde.Date && est.fecha.Date <= cierre.fechaHasta.Date)
                                            );

                db.tblEstancia.RemoveRange(estFactAbierta);

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

                db.tblEstancia.UpdateRange(estanciasExistentes);
                db.tblEstancia.AddRange(estanciasNuevas);

            }
            catch (Exception ex)
            {
                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        compania = "Universal",
                        url = estancias_url,
                        fecha = DateTime.Now,
                        mensaje = ex.Message
                    })
                });

                // Lanzar excepcion para controlar asíncronamente
                // throw new ImportarEstanciaException(ex.Message, "Universal", estancias_url, DateTime.Now);
            }
        }
        private class universal_loginResponse
        {
            public string accessToken { get; set; }
            public string refreshToken { get; set; }
        }

        private class universal_estanciasResponse
        {
            public int center_id_fk { get; set; }
            public string center { get; set; }
            public DateTime date { get; set; }
            public int ad { get; set; }
            public int jr { get; set; }
            public int ch { get; set; }
            public int bb { get; set; }
        }
    }
}
