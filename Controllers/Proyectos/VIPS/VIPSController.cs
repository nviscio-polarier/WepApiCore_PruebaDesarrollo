using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using WebApiCore.Class.Proyectos.MyPolarier.Administracion;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.VIPS
{
    public class VIPSController : ODataController
    {
        private readonly bdERP db;
        private readonly IConfiguration configuration;
        private readonly string uri;
        private readonly string id;
        private readonly string token;
        private readonly HttpClient httpClient;
        public VIPSController(IConfiguration configuration, bdERP context)
        {
            db = context;
            this.configuration = configuration;
            uri = configuration.GetSection("VIPS:URI").Value;
            id = configuration.GetSection("VIPS:ID").Value;
            token = configuration.GetSection("VIPS:TOKEN").Value;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
        }


        [EnableQuery]
        [HttpGet("odata/VIPS/ImportData")]
        [Authorize]
        public async Task<ActionResult> ImportData([FromODataUri] Date fecha, [FromODataUri] TipoNominaRD idTipoNomina)
        {
            int number = 7;
            char type = idTipoNomina == TipoNominaRD.Regalia ? 'R' : 'G';
            string url = "v2/payroll?" +
                "ID=" + id +
                "&TOKEN=" + token +
                "&Number=" + number +
                "&Type=" + type +
                "&Year=" + fecha.Year +
                "&Month=" + (idTipoNomina == TipoNominaRD.Regalia ? "12" : fecha.Month.ToString("D2"));

            if (idTipoNomina != TipoNominaRD.Regalia) 
                url += "&Period=" + (idTipoNomina == TipoNominaRD.NominaQ1 ? "1Q" : "2Q");

            //httpClient.GetAsync(url);
            var result = await httpClient.GetAsync(url);
            var jsonBody = JsonConvert.DeserializeObject<list_JsonNominas_RD>(await result.Content.ReadAsStringAsync());

            if (jsonBody.data == null)
                return Ok(false);

            if (idTipoNomina == TipoNominaRD.Regalia)
            {
                var a = jsonBody.data
                    .GroupBy(x => x.id_VIPS);
                jsonBody.data = jsonBody.data
                    .GroupBy(x => x.id_VIPS)
                    .Select(x => x
                        .OrderByDescending(x => x.gratificacion == 0 ? x.totalIngresos : x.gratificacion)
                        .First(x => (x.gratificacion == 0 ? x.totalIngresos : x.gratificacion) - x.neto == 0)
                    )
                    .ToList();
            }

            Date[] fechasPeriodo = getFortnightDates(fecha, idTipoNomina);
            var nominasExistentes = db.tblNomina_RD.Where(x => x.inicioPeriodo == fechasPeriodo[0] && x.finPeriodo == fechasPeriodo[1]);
            List <tblNomina_RD> nominasPorInsertar = new();
            var tblPersona = db.tblPersona
                .Include(x => x.idCentroTrabajoNavigation)
                .Include(x => x.idTipoTrabajoNavigation)
                .Where(x => x.id_VIPS != null)
                .ToDictionary(x => x.id_VIPS, x => x);

            var idsNoEncontrados = jsonBody.data
                .Select(x => x.id_VIPS)
                .Where(x => !tblPersona.ContainsKey(x))
                .ToList();

            if (idsNoEncontrados.Any())
            {
                var VIPS = new VIPSService(db, configuration);
                await VIPS.UpdatePersonasVIPS(idsNoEncontrados);

                tblPersona = db.tblPersona
                    .Include(x => x.idCentroTrabajoNavigation)
                    .Include(x => x.idTipoTrabajoNavigation)
                    .Where(x => x.id_VIPS != null)
                    .ToDictionary(x => x.id_VIPS, x => x);
            }

            foreach ( var item in jsonBody.data ) {

                if (!tblPersona.ContainsKey(item.id_VIPS)) { 
                    continue;
                };
                var objPersona = tblPersona[item.id_VIPS];
                tblNomina_RD nuevaNomina = new ()
                {
                    inicioPeriodo = fechasPeriodo[0],
                    finPeriodo = fechasPeriodo[1],
                    idPersona = objPersona.idPersona,
                    idAdmCentroCoste = objPersona.idAdmCentroCoste,
                    idAdmElementoPEP = objPersona.idAdmElementoPEP,
                    idTipoNomina_RD = (short)idTipoNomina,
                    ahorroCoop = item.ahorroCoop,
                    anticipoNomina = item.anticipoNomina,
                    ayudaPorMuerte = item.ayudaPorMuerte,
                    ayudaPorNacimiento = item.ayudaPorNacimiento,
                    dependAdicionalesSFS = item.dependAdicionalesSFS,
                    descAFP = item.descAFP,
                    descSFS = item.descSFS,
                    descuentoDeLicenciaMedica = item.descuentoDeLicenciaMedica,
                    descuentosOtros = item.descuentosOtros,
                    diasFeriados = item.diasFeriados,
                    diasPropina = item.diasPropina,
                    // Hasta que no venga la gratificación en la respuesta de VIPS, se tomará el total de ingresos
                    gratificacion = idTipoNomina == TipoNominaRD.Regalia && item.gratificacion == 0 ? item.totalIngresos : item.gratificacion,
                    horasExtras35 = item.horasExtras35,
                    horasNocturnas = item.horasNocturnas,
                    impSobreLaRenta = item.impSobreLaRenta,
                    incentivos = item.incentivos,
                    infotep = item.infotep,
                    neto = item.neto,
                    nombreCompleto = item.nombreCompleto,
                    ordenDeCompra = item.ordenDeCompra,
                    otrosDescuentos = item.otrosDescuentos,
                    otrosIngresos = item.otrosIngresos,
                    prestamoCoop = item.prestamoCoop,
                    primaVacacional = item.primaVacacional,
                    reembolsoISR = item.reembolsoISR,
                    reembolsoOtrosDescuentos = item.reembolsoOtrosDescuentos,
                    salarioRetroactivo = item.salarioRetroactivo,
                    saldoAFavorISR = item.saldoAFavorISR,
                    seguroMedicoPrivado = item.seguroMedicoPrivado,
                    sindicato = item.sindicato,
                    subsidioPorMaternidad = item.subsidioPorMaternidad,
                    subsidPorEnferm = item.subsidPorEnferm,
                    sueldo = item.sueldo,
                    totalDescuentos = item.totalDescuentos,
                    totalIngresos = item.totalIngresos,
                    idAdmCuentaContable = objPersona?.idAdmCuentaContable_Salario,
                };
                nominasPorInsertar.Add(nuevaNomina);
                //db.tblNomina_RD.Add(nuevaNomina);
            }
            if (nominasExistentes.Any())
            {
                foreach (var nomina in nominasExistentes)
                {
                    if (nomina.contabilizado == true)
                    {
                        nominasPorInsertar = nominasPorInsertar.Where(x => x.idPersona != nomina.idPersona).ToList();
                        nominasExistentes = nominasExistentes.Where(x => x.idNomina_RD != nomina.idNomina_RD);
                    }
                }
                db.tblNomina_RD.RemoveRange(nominasExistentes);
            }
            db.tblNomina_RD.AddRange(nominasPorInsertar);
            db.SaveChanges();
            return Ok(true);
            //return Ok(JsonConvert.SerializeObject(aux));
        }

        private Date[] getFortnightDates(Date fecha, TipoNominaRD idTipoNomina)
        {
            Date[] dates = new Date[2];
            if (idTipoNomina == TipoNominaRD.NominaQ1)
            {
                dates[0] = new Date(fecha.Year, fecha.Month, 1);
                dates[1] = new Date(fecha.Year, fecha.Month, 15);
            }
            else if (idTipoNomina == TipoNominaRD.NominaQ2)
            {
                dates[0] = new Date(fecha.Year, fecha.Month, 16);
                dates[1] = new Date(fecha.Year, fecha.Month, DateTime.DaysInMonth(fecha.Year, fecha.Month));
            }
            else if (idTipoNomina == TipoNominaRD.Regalia)
            {
                dates[0] = new Date(fecha.Year, 1, 1);
                dates[1] = new Date(fecha.Year, 12, 31);
            }
            return dates;
        }

        /* SCRIPT TEMPORAL, BORRAR UNA VEZ SE HAYA EJECUTADO */
        #region SCRIPT TEMPORAL
        [EnableQuery]
        [HttpGet("odata/VIPS/OverwriteIDPerson")]
        [Authorize]
        public async Task<ActionResult> OverWrite()
        {
            var result = await httpClient.GetAsync("v2/employee?ID=A21482&TOKEN=ctDHQBOeQBgWRZzQMQ63v1mqw&Limit=10000");
            DeserializePersonaVIPS jsonBody = JsonConvert.DeserializeObject<DeserializePersonaVIPS>(await result.Content.ReadAsStringAsync());
            List<tblPersona> personas = db.tblPersona.ToList();
            foreach(tblPersona persona in personas)
            {
                if(persona.nombre != null)
                {
                    persona.nombre = Regex.Replace(persona.nombre, @"\s+", " ");
                    persona.nombre = persona.nombre.Trim().ToUpper();
                }
                if(persona.apellidos != null)
                {
                    persona.apellidos = Regex.Replace(persona.apellidos, @"\s+", " ");
                    persona.apellidos = persona.apellidos.Trim().ToUpper();
                }
            };
            foreach(PersonaVIPS personaVIP in jsonBody.Data)
            {
                if(personaVIP.Name != null)
                {
                    Encoding isoEncoding = Encoding.GetEncoding("ISO-8859-1");
                    Encoding utfEncoding = Encoding.UTF8;
                    byte[] bytesIso = utfEncoding.GetBytes(personaVIP.Name);
                    byte[] bytesUtf = Encoding.Convert(utfEncoding, isoEncoding, bytesIso);
                    personaVIP.Name = utfEncoding.GetString(bytesUtf);

                    personaVIP.Name = Regex.Replace(personaVIP.Name, @"\s+", " ");
                    personaVIP.Name = personaVIP.Name.Trim().ToUpper();
                }
                if(personaVIP.LastName != null)
                {
                    Encoding isoEncoding = Encoding.GetEncoding("ISO-8859-1");
                    Encoding utfEncoding = Encoding.UTF8;
                    byte[] bytesIso = utfEncoding.GetBytes(personaVIP.LastName);
                    byte[] bytesUtf = Encoding.Convert(utfEncoding, isoEncoding, bytesIso);
                    personaVIP.LastName = utfEncoding.GetString(bytesUtf);

                    personaVIP.LastName = Regex.Replace(personaVIP.LastName, @"\s+", " ");
                    personaVIP.LastName = personaVIP.LastName.Trim().ToUpper();
                }
            } 
            int total = 0;
            int encontrados = 0;
            List<string> personasRepetidasEnVIPS = new();
            List<tblPersona> personasAmbiguas = new();
            List<PersonaVIPS> personasAmbiguasDeVIPS = new();
            List<string> personasNoExistentes = new();
            foreach (PersonaVIPS personaVIP in jsonBody.Data)
            {
                total++;
                if (jsonBody.Data.Where(x => x.Name.Equals(personaVIP.Name) && x.LastName.Equals(personaVIP.LastName)).Count() > 1)
                {
                    personasRepetidasEnVIPS.Add(personaVIP.Name + " " + personaVIP.LastName);
                    continue;
                }
                tblPersona personaPol;
                List<tblPersona> aux = personas.Where(x => 
                    (x.nombre.Equals(personaVIP.Name) && x.apellidos.Equals(personaVIP.LastName)) ||
                    (x.nombre.Length >= 3 && x.apellidos?.Length >= 3 && personaVIP.Name.Contains(x.nombre) && personaVIP.LastName.Contains(x.apellidos))
                    ).ToList();

                if (aux.Count > 1 && aux.Where(x => x.eliminado == false).Count() == 1)
                {
                    aux = aux.Where(x => x.eliminado == false).ToList();
                }

                if(aux.Count == 1)
                {
                    personaPol = aux.Single();
                    encontrados++;
                    personaPol.id_VIPS = personaVIP.User_ID;
                }
                else if (aux.Count == 0)
                {
                    personasNoExistentes.Add(personaVIP.Name + " " + personaVIP.LastName);

                }
                else
                {
                    foreach (var personaAmbigua in aux)
                    {
                        personasAmbiguas.Add(personaAmbigua);
                    }
                    personasAmbiguasDeVIPS.Add(personaVIP);
                }
            }
            db.SaveChanges();
            return Ok();
        }

        private class DeserializePersonaVIPS
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public int Limit { get; set; }
            public List<PersonaVIPS> Data { get; set; }
        }

        private class PersonaVIPS
        {
            public int User_ID { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Identification { get; set; }
            public float Salay { get; set; }
            public string Currency { get; set; }
            public string Enabled { get; set; }
            public int Job_Table_ID { get; set; }
            public int Job_ID { get; set; }
            public string BirthDate { get; set; }
            public string BirthPlace { get; set; }
            public string MarialStatus { get; set; }
            public string Sex { get; set; }
            public string Town { get; set; }
            public string Phone { get; set; }
            public string Mobile { get; set; }
            public int Company { get; set; }
            public int Branch { get; set; }
            public int Departament { get; set; }
            public int Section { get; set; }
            public string HireDate { get; set; }
            public int FireDate { get; set; }
        }
        #endregion
        /* FIN SCRIPT TEMPORAL, BORRAR UNA VEZ SE HAYA EJECUTADO */

        private class list_JsonNominas_RD
        {
            public List<_JsonNominas_RD> data { get; set; }
        }
        private class _JsonNominas_RD
        {
            [JsonProperty("No empleado VIPS")]
            public int? id_VIPS { get; set; }
            [JsonProperty("Nombres")]
            public string? nombreCompleto { get; set; }
            [JsonProperty("SUELDO")]
            public decimal? sueldo { get; set; }
            [JsonProperty("DIAS PROPINA")]
            public decimal? diasPropina { get; set; }
            [JsonProperty("DIAS FERIADOS")]
            public decimal? diasFeriados { get; set; }
            [JsonProperty("HORAS NOCTURNAS")]
            public decimal? horasNocturnas { get; set; }
            [JsonProperty("HORAS EXTRAS 35%")]
            public decimal? horasExtras35 { get; set; }
            [JsonProperty("PRIMA VACACIONAL")]
            public decimal? primaVacacional { get; set; }
            [JsonProperty("GRATIFICACION")]
            public decimal? gratificacion { get; set; }
            [JsonProperty("OTROS INGRESOS")]
            public decimal? otrosIngresos { get; set; }
            [JsonProperty("SALARIO RETROACTIVO")]
            public decimal? salarioRetroactivo { get; set; }
            [JsonProperty("INCENTIVOS")]
            public decimal? incentivos { get; set; }
            [JsonProperty("SUBSID POR ENFERM")]
            public decimal? subsidPorEnferm { get; set; }
            [JsonProperty("AYUDA POR NACIMIENTO")]
            public decimal? ayudaPorNacimiento { get; set; }
            [JsonProperty("SUBSIDIO POR MATERNIDAD")]
            public decimal? subsidioPorMaternidad { get; set; }
            [JsonProperty("AYUDA POR MUERTE")]
            public decimal? ayudaPorMuerte { get; set; }
            [JsonProperty("REEMBOLSO OTROS DESCUENTOS")]
            public decimal? reembolsoOtrosDescuentos { get; set; }
            [JsonProperty("SALDO A FAVOR ISR")]
            public decimal? saldoAFavorISR { get; set; }
            [JsonProperty("REEMBOLSO ISR")]
            public decimal? reembolsoISR { get; set; }
            [JsonProperty("TOTAL INGRESOS")]
            public decimal? totalIngresos { get; set; }
            [JsonProperty("IMP. SOBRE LA RENTA")]
            public decimal? impSobreLaRenta { get; set; }
            [JsonProperty("SEGURO MEDICO PRIVADO")]
            public decimal? seguroMedicoPrivado { get; set; }
            [JsonProperty("DESCUENTO DE LICENCIA MEDICA")]
            public decimal? descuentoDeLicenciaMedica { get; set; }
            [JsonProperty("SINDICATO")]
            public int? sindicato { get; set; }
            [JsonProperty("ANTICIPO NOMINA")]
            public decimal? anticipoNomina { get; set; }
            [JsonProperty("DESC. A.F.P")]
            public decimal? descAFP { get; set; }
            [JsonProperty("DESC. S.F.S")]
            public decimal? descSFS { get; set; }
            [JsonProperty("DEPEND. ADICIONALES SFS")]
            public decimal? dependAdicionalesSFS { get; set; }
            [JsonProperty("OTROS DESCUENTOS")]
            public decimal? otrosDescuentos { get; set; }
            [JsonProperty("AHORRO COOP")]
            public decimal? ahorroCoop { get; set; }
            [JsonProperty("PRESTAMO COOP")]
            public decimal? prestamoCoop { get; set; }
            [JsonProperty("ORDEN DE COMPRA")]
            public decimal? ordenDeCompra { get; set; }
            [JsonProperty("INFOTEP")]
            public decimal? infotep { get; set; }
            [JsonProperty("DESCUENTOS OTROS")]
            public decimal? descuentosOtros { get; set; }
            [JsonProperty("TOTAL DESCUENTOS")]
            public decimal? totalDescuentos { get; set; }
            [JsonProperty("NETO")]
            public decimal? neto { get; set; }
        }
    }
}
