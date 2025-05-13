using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApiCore.Context;

namespace WebApiCore.Class.externos
{
    public class AllSunDataService
    {
        private readonly Context.bdERP db;

        //<hotelId, idEntidad>
        private static readonly Dictionary<string, int> tablaConversion = new()
                {
                    { "58206a60157cd214a475ad39", 54 },  // Orient Beach
                    { "58206c10157cd214a475ad4f", 61 },  // Sumba
                    { "57f23d4eb700f334db4815a9", 235 }, // Pil·Lari Playa
                    { "582069b0157cd214a475ad30", 236 }, // Kontiki Playa
                    { "58164fc3157cd214a475ad23", 238 }, // Estrella & Coral
                    { "58206aae157cd214a475ad3d", 240 }, // Orquidea Playa
                    { "58206b09157cd214a475ad41", 247 }, // Bella Paguera
                    { "5833300fd3a8420bf40fa187", 248 }, // Cormorán
                    { "58206ae7157cd214a475ad3f", 250 }, // Allsun Paguera
                    { "58206bd9157cd214a475ad4b", 253 }, // Paguera Park
                    { "587f9da2d3a84212e4ad495e", 259 }, // Cristóbal Colón
                    { "587f9dd2d3a84212e4ad495f", 260 }, // Riviera Playa
                    { "5816502e157cd214a475ad27", 261 }, // Eden Playa
                    { "58164fea157cd214a475ad25", 262 }, // Eden Alcudia
                    { "58206981157cd214a475ad2e", 263 }, // Illot Park
                    { "582069d3157cd214a475ad31", 264 }, // Lago Playa / Park
                    { "587f9b42d3a84212e4ad495d", 266 }, // Lux de Mar
                    { "58164d42157cd214a475ad1f", 303 }, // Bahía del Este
                    { "58332fa6d3a8420bf40fa186", 306 }, // Borneo
                    { "5bd03c8033d418e7ed592ac7", 313 }, // Marena Beach
                    //{ "58165062157cd214a475ad2b", }, // Esquinzo Beach
                    //{ "58206937157cd214a475ad2d", }, // Los Hibiscos
                    //{ "5816504c157cd214a475ad29", }, // Espléndido
                    //{ "58164ee0157cd214a475ad21", }, // Barlovento
                    //{ "58164c60157cd214a475ad1b", }, // Albatros
                    //{ "582069ef157cd214a475ad33", }, // Lucana
                    //{ "GRE", }, // Greece Central
                    //{ "ZOR", }, // Zorbas Village
                    //{ "CAR", }, // Carolina Mare
                    //{ "CEN", }, // CENTRAL
                };

        public List<int> entidades = tablaConversion.Values.ToList();

        private DateTime fechaDesde;
        private DateTime fechaHasta;

        public AllSunDataService(Context.bdERP _db, DateTime _fechaDesde, DateTime _fechaHasta)
        {
            db = _db;
            fechaDesde = _fechaDesde;
            fechaHasta = _fechaHasta;
        }

        public async Task ImportarEstancias()
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

        public async Task ImportarEstancias(List<tblCierreFactEntidad> cierres)
        {
            string token = "poln?854hiu{)0|N*b@1,;&MfTd43us9cqHd?mp4HB%P|ow4lH^X2~}aK-)|,=Q";
            string estancias_url = "https://api-net.allsun.online:444/apa/v1/hotel/occupancy?startDate={0}&endDate={1}";

            estancias_url = String.Format(estancias_url, fechaDesde.ToString("dd-MM-yyyy"), fechaHasta.ToString("dd-MM-yyyy"));

            try
            {

                var response = new allSun_estanciasResponse();
                using (HttpClient client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Add("secret", token);
                    using (HttpResponseMessage res = await client.GetAsync(estancias_url))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            using (HttpContent contentResult = res.Content)
                            {
                                string data = await contentResult.ReadAsStringAsync();
                                response = JsonConvert.DeserializeObject<allSun_estanciasResponse>(data);
                            }
                        }
                    }
                }

                var estancias = response.data
                                    .Where(entidadEstancias => tablaConversion.ContainsKey(entidadEstancias.hotelId) &&
                                        !cierres
                                            .Where(cierre => cierre.idEntidad == tablaConversion[entidadEstancias.hotelId])
                                            .Any(cierre => entidadEstancias.timestamp.Date >= cierre.fechaDesde.Date && entidadEstancias.timestamp.Date <= cierre.fechaHasta.Date)
                                    )
                                    .Select(cl => new
                                    {
                                        idEntidad = tablaConversion[cl.hotelId],
                                        fecha = cl.timestamp.Date,
                                        estancias = cl.stays,
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
                        compania = "AllSun",
                        url = estancias_url,
                        fecha = DateTime.Now,
                        mensaje = ex.Message
                    })
                });
                // Lanzar excepcion para controlar asíncronamente
                //throw new ImportarEstanciaException(ex.Message, "AllSun", estancias_url, DateTime.Now);
            }
        }

        private class allSun_estanciasResponse
        {
            public bool success { get; set; }
            public string errorMessage { get; set; }

            public List<HotelOccupancy> data { get; set; }
            public string errorType { get; set; }
        }

        private class HotelOccupancy
        {

            public string hotelId { get; set; }
            public string hotelCode { get; set; }
            public string hotelName { get; set; }
            public DateTime timestamp { get; set; }
            public int roomsUsed { get; set; }
            public int stays { get; set; }
        }
    }
}
