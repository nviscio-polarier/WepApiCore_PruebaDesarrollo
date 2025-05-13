using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using WebApiCore.Context;

namespace WebApiCore.Class.externos
{
    public class TimonHotelDataService
    {
        private readonly Context.bdERP db;

        //<Establecimiento, idEntidad>
        public static readonly Dictionary<int, int> tablaConversion = new()
                {
                    { 5, 273 },   //GRAN FIESTA
                    { 9, 274 },   //ALMA BEACH
                    { 2, 275 },   //TROPICAL
                    { 7, 276 },   //BALANGUERA BEACH
                    { 1, 301 },   //JAIME III
                    { 11, 302 },  //BALANGUERA
                    { 33, 439 },  //MAR BLAU 
                    { 29, 440 },  //WHALA ISABELA
                    { 12, 529 },  //WHALA BEACH
                    { 24, 530 },  //WHALA FUN
                    { 3, 538 },   //MARTINIQUE
                    { 35, 577 },  //PALMA BLANC
                    { 20, 584 }   //AYRON PARK
                };

        public readonly List<int> entidades = tablaConversion.Values.ToList();

        private DateTime fechaDesde;
        private DateTime fechaHasta;

        public TimonHotelDataService(Context.bdERP _db, DateTime _fechaDesde, DateTime _fechaHasta)
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
            catch
            {
            }
        }

        public async Task ImportarEstancias(List<tblCierreFactEntidad> cierres)
        {
            string estancias_url = "";
            try
            {
                List<timonHotel_estanciasResponse> listaEstancias = new List<timonHotel_estanciasResponse>();
                using (HttpClient client = new HttpClient())
                {
                    string token = "";

                    string login_url = "http://api.06361.timonhotel.com/api/v.1/json/login";
                    timonHotel_loginRequest login_requestObj = new timonHotel_loginRequest() { usuario = "polarier", password = "3i6pNtX34A" };
                    var login_payload = JsonConvert.SerializeObject(login_requestObj);
                    HttpContent login_contentRequest = new StringContent(login_payload, Encoding.UTF8, "application/json");
                    using (HttpResponseMessage res = await client.PostAsync(login_url, login_contentRequest))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            using (HttpContent contentResult = res.Content)
                            {
                                string data = await contentResult.ReadAsStringAsync();
                                token = JsonConvert.DeserializeObject<timonHotel_loginResponse>(data).token;
                            }
                        }
                    }

                    estancias_url = "http://api.06361.timonhotel.com/api/v.1/json/get/plano-habitaciones";
                    timonHotel_estanciasRequest estancias_requestObj = new timonHotel_estanciasRequest()
                    {
                        desdeFechaOcupacion = fechaDesde.ToString("yyyy-MM-dd"),
                        hastaFechaOcupacion = fechaHasta.ToString("yyyy-MM-dd")
                    };
                    var estancias_payload = JsonConvert.SerializeObject(estancias_requestObj);

                    client.DefaultRequestHeaders.Add("apiToken", token);
                    HttpContent estancias_contentRequest = new StringContent(estancias_payload, Encoding.UTF8, "application/json");
                    using (HttpResponseMessage res = await client.PostAsync(estancias_url, estancias_contentRequest))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            using (HttpContent contentResult = res.Content)
                            {
                                string data = await contentResult.ReadAsStringAsync();
                                listaEstancias = JsonConvert.DeserializeObject<List<timonHotel_estanciasResponse>>(data);
                            }
                        }
                    }
                }

                var estancias = listaEstancias
                    .Where(entidadEstancias =>
                        (entidadEstancias.tipo_hab == "ZZZ" && tablaConversion.ContainsKey(int.Parse(entidadEstancias.establecimiento))) &&
                        !cierres
                            .Where(cierre => cierre.idEntidad == tablaConversion[int.Parse(entidadEstancias.establecimiento)])
                            .Any(cierre => Convert.ToDateTime(entidadEstancias.fecha).Date >= cierre.fechaDesde.Date && Convert.ToDateTime(entidadEstancias.fecha).Date <= cierre.fechaHasta.Date)
                    )
                    .GroupBy(l => new { l.establecimiento, l.fecha })
                    .Select(cl => new estanciasPorEntidad
                    {
                        idEntidad = tablaConversion[int.Parse(cl.Key.establecimiento)],
                        fecha = Convert.ToDateTime(cl.Key.fecha),
                        estancias = cl.Sum(c => int.Parse(c.estancias)),
                    });

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
                        compania = "HM",
                        url = estancias_url,
                        fecha = DateTime.Now,
                        mensaje = ex.Message
                    })
                });

                // Lanzar excepcion para controlar asíncronamente
                // throw new ImportarEstanciaException(ex.Message, "HM", estancias_url, DateTime.Now);
            }
        }

        private class estanciasPorEntidad
        {
            public int idEntidad { get; set; }
            public DateTime fecha { get; set; }
            public int estancias { get; set; }
        }

        private class timonHotel_loginRequest
        {
            public string usuario { get; set; }
            public string password { get; set; }
        }

        private class timonHotel_loginResponse
        {
            public string token { get; set; }
        }

        private class timonHotel_estanciasRequest
        {
            public string desdeFechaOcupacion { get; set; }
            public string hastaFechaOcupacion { get; set; }
        }

        private class timonHotel_estanciasResponse
        {
            public string establecimiento { get; set; }
            public string fecha { get; set; }
            public string tipo_hab { get; set; }
            public string tipo_hab_descripcion { get; set; }
            public string capacidad { get; set; }
            public string ocupadas { get; set; }
            public string libres { get; set; }
            public string estancias { get; set; }
            public string estado { get; set; }
        }
    }

}
