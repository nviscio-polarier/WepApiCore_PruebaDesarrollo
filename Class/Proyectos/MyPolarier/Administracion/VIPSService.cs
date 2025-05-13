using Newtonsoft.Json;
using System.Text;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;

namespace WebApiCore.Class.Proyectos.MyPolarier.Administracion
{
    public class VIPSService
    {

        private static readonly Dictionary<int?, int> tablaConversionBranchLavanderia = new() {
            { 1, 3 },   // Estructura_RD 
            { 2, 11 },  // BE LIVE PUNTA CANA
            { 3, 12 },  // BE LIVE CANOA
            { 4, 13 },  // BE LIVE HAMACA
            { 5, 2 },   // MAJESTIC
            { 6, 24 },  // BAHIA PRINCIPE
            { 7, 36 },  // BE LIVE MARIEN
            { 8, 38 },  // DREAMS MACAO
            { 9, 50 },  // LIVE AQUA
            { 10, 63 }, // CAYO LEVANTADO
            { 11, 64 }, // IBEROSTAR COSTA DORADA
            { 12, 68 }  // SECRETS PLAYA ESMERALDA
        };

        private static readonly Dictionary<int, int> tablaConversionSeccionCentroCoste = new() {
            { 11, 4 },  // ADMINISTRACION
            { 12, 16 }, // CONTABILIDAD
            { 13, 78 }, // ESTRUCTURA COMPAÑÍA
            { 21, 71 }, // ESTRUCTURA COORD OPERACIONES
            { 22, 127 },// RRHH
            { 23, 11 }, // COMPRAS
            { 31, 91 },  // ASSISTANT
            { 51, 63 }, // INGENIERIA OPERACIONES
            { 52, 102 },// INGENIERIA MANTENIMIENTO
            { 61, 138 },// TECNOLOGIA
        };

        private static readonly Dictionary<int, byte> tablaConversionSeccionTipotrabajo = new()
        {
            {41, 1},     // CASO PARTICULAR -> resuelve por defecto en produccion
            {42, 2},     //logistica - almacenes y pedidos en SQL
            {43, 6},     //Transporte
            {44, 1},     //produccion
            {45, 4},     //Mantenimiento
            {46, 5},     // VALET 
            {47, 3},     //Administracion
        };

        private readonly Dictionary<int, int?> tableElementosPEP;

        private readonly Dictionary<int, tblCuentaContableNCentroTrabajo> tablaCuentaContableNCentroTrabajo;
        private readonly Dictionary<byte, tblCuentaContableNTipoTrabajo> tablaCuentaContableNTipoTrabajo;

        private readonly Context.bdERP db;
        private readonly IConfiguration configuration;

        private readonly Encoding isoEncoding = Encoding.GetEncoding("ISO-8859-1");
        private readonly Encoding utfEncoding = Encoding.UTF8;

        public VIPSService(Context.bdERP db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;

            var idsLavanderia = tablaConversionBranchLavanderia.Where(x => x.Key != 1).Select(x => x.Value).ToList();
            tableElementosPEP = db.tblLavanderia
                .Where(x => idsLavanderia.Contains(x.idLavanderia))
                .ToDictionary(x => x.idLavanderia, x => x.idAdmElementoPEP);

            tablaCuentaContableNCentroTrabajo = db.tblCuentaContableNCentroTrabajo
                .Where(x => x.idAdmCuentaContable_Salario != null || x.idAdmCuentaContable_SSEmpresa != null)
                .ToDictionary(x => x.idCentroTrabajo, x => x);

            tablaCuentaContableNTipoTrabajo = db.tblCuentaContableNTipoTrabajo
                .Where(x => x.idAdmCuentaContable_Salario != null || x.idAdmCuentaContable_SSEmpresa != null)
                .ToDictionary(x => x.idTipoTrabajo, x => x);
        }

        /// <summary>
        /// Actualiza todas las personas activadas en VIPS en nuestra BBDD
        /// </summary>
        /// <returns></returns>
        public async Task UpdatePersonasVIPS()
        {
            try
            {
                string uri = configuration.GetSection("VIPS:URI").Value;
                string id = configuration.GetSection("VIPS:ID").Value;
                string token = configuration.GetSection("VIPS:TOKEN").Value;
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(uri)
                };
                string url = "v2/employee?" +
                    "ID=" + id +
                    "&TOKEN=" + token +
                    "&Limit=10000"; // Es necesario establecer limite por que por defecto es 100
                var result = await httpClient.GetAsync(url);

                var jsonBody = JsonConvert.DeserializeObject<DeserializePersonaVIPS>(await result.Content.ReadAsStringAsync());
                List<tblPersona> personas_RD = db.tblPersona.Where(x => x.id_VIPS != null).ToList();

                var personasNuevas = IU_tblPersona(personas_RD, jsonBody.Data.Where(x => x.User_ID != 0).ToList());
                if (personasNuevas.Any()) db.tblPersona.AddRange(personasNuevas);

                db.SaveChanges();
            }
            catch (Exception e)
            {
                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "Importar personas VIPS",
                    error = JsonConvert.SerializeObject(new
                    {
                        fecha = DateTime.Now
                    }),
                });
                if (e.Message.Contains("Unexpected character encountered while parsing"))
                    throw new Exception("Error al recibir datos de VIPS");
                else throw;
            }
        }

        /// <summary>
        /// Actualiza todas las personas con los ids_VIPS especificados, estén habilitadas o no en VIPS
        /// </summary>
        /// <param name="ids_VIPS">ids de VIPS a actualizar</param>
        /// <returns></returns>
        public async Task UpdatePersonasVIPS(List<int?> ids_VIPS)
        {
            try
            {
                string uri = configuration.GetSection("VIPS:URI").Value;
                string id = configuration.GetSection("VIPS:ID").Value;
                string token = configuration.GetSection("VIPS:TOKEN").Value;
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(uri)
                };
                string url = "v2/employee?" +
                    "ID=" + id +
                    "&TOKEN=" + token +
                    "&Limit=10000";
                var result = await httpClient.GetAsync(url);

                var jsonBody = JsonConvert.DeserializeObject<DeserializePersonaVIPS>(await result.Content.ReadAsStringAsync());
                List<tblPersona> personas_RD = db.tblPersona.Where(x => ids_VIPS.Contains(x.id_VIPS)).ToList();
                var personasVIPS = jsonBody.Data.Where(x => ids_VIPS.Contains(x.User_ID)).ToList();

                var personasNuevas = IU_tblPersona(personas_RD, personasVIPS);
                if (personasNuevas.Any()) db.tblPersona.AddRange(personasNuevas);

                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "Importar personas VIPS nóminas",
                    error = JsonConvert.SerializeObject(new
                    {
                        fecha = DateTime.Now,
                        ids_VIPS
                    }),
                });
            }
        }

        private List<tblPersona> IU_tblPersona(List<tblPersona> personas_RD, List<PersonaVIPS> personasVIPS)
        {
            var tblPersonaNTipoContrato = db.tblPersonaNTipoContrato.Where(pntc => personas_RD.Select(p => p.idPersona).Contains(pntc.idPersona)).ToList();

            List<tblPersona> personasNuevas = new();
            foreach (PersonaVIPS personaVIPS in personasVIPS)
            {
                var hireDate = string.IsNullOrEmpty(personaVIPS.HireDate) || personaVIPS.HireDate == "0" ? (DateTime?)null : DateTime.ParseExact(personaVIPS.HireDate, "yyyyMMdd", null);
                var fireDate = string.IsNullOrEmpty(personaVIPS.FireDate) || personaVIPS.FireDate == "0" ? (DateTime?)null : DateTime.ParseExact(personaVIPS.FireDate, "yyyyMMdd", null);

                var persona = personas_RD.Where(x => x.id_VIPS == personaVIPS.User_ID).FirstOrDefault();
                var isActivo = (hireDate == null || hireDate <= DateTime.Today) &&
                            (fireDate == null || DateTime.Today <= fireDate);
                if (persona == null)
                /* Insert */
                {
                    if (!isActivo) continue;

                    byte[] bytesIsoName = utfEncoding.GetBytes(personaVIPS.Name);
                    byte[] bytesUtfName = Encoding.Convert(utfEncoding, isoEncoding, bytesIsoName);

                    byte[] bytesIsoLastName = utfEncoding.GetBytes(personaVIPS.LastName);
                    byte[] bytesUtfLastName = Encoding.Convert(utfEncoding, isoEncoding, bytesIsoLastName);

                    persona = new tblPersona
                    {
                        nombre = utfEncoding.GetString(bytesUtfName),
                        apellidos = utfEncoding.GetString(bytesUtfLastName),
                        id_VIPS = personaVIPS.User_ID,
                        fechaNacimiento = DateTime.ParseExact(personaVIPS.BirthDate, "yyyyMMdd", null),
                        idGenero = (byte?)(personaVIPS.Sex == "Male" ? 2 : 1),
                        idEstadoCivil = (personaVIPS.MarialStatus == "Single" ? 1 : personaVIPS.MarialStatus == "Married" ? 2 : null),
                        telefono = personaVIPS.Phone,
                    };

                    if (hireDate != null)
                    {
                        AddContrato((short)idsTipoContrato.FijoDiscontinuo);

                        persona.activo = isActivo;
                    }

                    AssignCenter(persona, personaVIPS);
                    AssignTipoTrabajo(persona, personaVIPS);
                    personasNuevas.Add(persona);
                }
                else
                /* Update */
                {
                    if (hireDate != null || fireDate != null)
                    {
                        if (hireDate != null)
                        {
                            var tblPersonaNTipoContrato_persona = tblPersonaNTipoContrato.Where(pntc => pntc.idPersona == persona.idPersona).ToList();

                            var contratosConConflictos = tblPersonaNTipoContrato_persona
                                .Where(pntc =>
                                    (
                                        fireDate == null
                                        && (pntc.fechaAltaContrato >= hireDate || (pntc.fechaBajaContrato == null || pntc.fechaBajaContrato >= hireDate))
                                    )
                                    || (
                                        fireDate != null
                                        && (
                                            (pntc.fechaAltaContrato <= hireDate && (pntc.fechaBajaContrato == null || pntc.fechaBajaContrato >= fireDate))
                                            || (pntc.fechaAltaContrato >= hireDate && pntc.fechaAltaContrato <= fireDate)
                                            || (pntc.fechaBajaContrato != null && pntc.fechaBajaContrato >= hireDate && pntc.fechaBajaContrato <= fireDate)
                                        )
                                    )
                                )
                                .OrderByDescending(pntc => pntc.fechaAltaContrato);

                            db.tblPersonaNTipoContrato.RemoveRange(contratosConConflictos);

                            var idTipoContrato = contratosConConflictos.FirstOrDefault()?.idTipoContrato ?? (short)idsTipoContrato.FijoDiscontinuo;
                            AddContrato(idTipoContrato);
                        }

                        persona.activo = isActivo;
                    } else
                    {
                        persona.activo = false;
                    }

                    persona.eliminado = !persona.activo && persona.eliminado;

                    byte[] bytesIsoName = utfEncoding.GetBytes(personaVIPS.Name);
                    byte[] bytesUtfName = Encoding.Convert(utfEncoding, isoEncoding, bytesIsoName);
                    persona.nombre = utfEncoding.GetString(bytesUtfName);

                    byte[] bytesIsoLastName = utfEncoding.GetBytes(personaVIPS.LastName);
                    byte[] bytesUtfLastName = Encoding.Convert(utfEncoding, isoEncoding, bytesIsoLastName);
                    persona.apellidos = utfEncoding.GetString(bytesUtfLastName);

                    persona.fechaNacimiento = DateTime.ParseExact(personaVIPS.BirthDate, "yyyyMMdd", null);
                    persona.idGenero = (byte?)(personaVIPS.Sex == "Male" ? 2 : 1);
                    persona.idEstadoCivil = (personaVIPS.MarialStatus == "Single" ? 1 : personaVIPS.MarialStatus == "Married" ? 2 : null);
                    persona.telefono = personaVIPS.Phone;

                    AssignCenter(persona, personaVIPS);
                    AssignTipoTrabajo(persona, personaVIPS);
                }

                void AddContrato(short idTipoContrato)
                {
                    persona.tblPersonaNTipoContrato.Add(new tblPersonaNTipoContrato
                    {
                        idTipoContrato = idTipoContrato,
                        fechaAltaContrato = (DateTime)hireDate,
                        fechaBajaContrato = fireDate,
                    });
                }
            }
            return personasNuevas;
        }

        private void AssignCenter(tblPersona persona, PersonaVIPS personaVIPS)
        {
            if (tablaConversionBranchLavanderia.TryGetValue(personaVIPS.Branch, out int centro))
            {
                if (personaVIPS.Branch != 1)
                {
                    persona.idLavanderia = centro;
                    persona.idCentroTrabajo = null;
                    persona.idAdmElementoPEP = tableElementosPEP[(int)persona.idLavanderia];
                    persona.idAdmCentroCoste = null;

                }
                else
                {
                    if ((int)personaVIPS.Departament != 4)
                    {
                        string section = String.Concat(personaVIPS.Section.ToString().TakeLast(2));
                        if (tablaConversionSeccionCentroCoste.TryGetValue(int.Parse(section), out int idCentroCoste))
                        {
                            persona.idCentroTrabajo = centro; //Estructura_RD
                            persona.idLavanderia = null;
                            persona.idAdmCentroCoste = idCentroCoste;
                            persona.idAdmElementoPEP = null;

                            persona.idAdmCuentaContable_Salario = tablaCuentaContableNCentroTrabajo[centro]?.idAdmCuentaContable_Salario;
                            persona.idAdmCuentaContable_SSEmpresa = tablaCuentaContableNCentroTrabajo[centro]?.idAdmCuentaContable_SSEmpresa;
                        }
                    }
                }
            }
        }

        private void AssignTipoTrabajo(tblPersona persona, PersonaVIPS personaVIPS)
        {
            string section = String.Concat(personaVIPS.Section.ToString().TakeLast(2));
            if (tablaConversionSeccionTipotrabajo.TryGetValue(int.Parse(section), out byte idTipoTrabajo) && persona.idCentroTrabajo == null)
            {
                persona.idTipoTrabajo = idTipoTrabajo;

                persona.idAdmCuentaContable_Salario = tablaCuentaContableNTipoTrabajo[idTipoTrabajo]?.idAdmCuentaContable_Salario;
                persona.idAdmCuentaContable_SSEmpresa = tablaCuentaContableNTipoTrabajo[idTipoTrabajo]?.idAdmCuentaContable_SSEmpresa;
            }
            else
            {
                persona.idTipoTrabajo = null;
            }
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
            public int? User_ID { get; set; }
            public string? Name { get; set; }
            public string? LastName { get; set; }
            public string? Identification { get; set; }
            public float? Salay { get; set; }
            public string? Currency { get; set; }
            public string? Enabled { get; set; }
            public int? Job_Table_ID { get; set; }
            public int? Job_ID { get; set; }
            public string? BirthDate { get; set; }
            public string? BirthPlace { get; set; }
            public string? MarialStatus { get; set; }
            public string? Sex { get; set; }
            public string? Town { get; set; }
            public string? Phone { get; set; }
            public string? Mobile { get; set; }
            public int? Company { get; set; }
            public int? Branch { get; set; }
            public int? Departament { get; set; }
            public int? Section { get; set; }
            public string? HireDate { get; set; }
            public string? FireDate { get; set; }
        }
    }
}
