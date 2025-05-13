using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApiCore.Class.externos.SAP;
using WebApiCore.Class.externos.SAP.Context;
using WebApiCore.Class.externos.SAP.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.SAP
{
    public class SAPInfoDumpController : ODataController
    {
        private readonly SAPWrap sap = new();
        private readonly bdERP db;
        List<tblEmpresasPolarier> tblEmpresasPolarier = new();
        List<tblPais> tblPais = new();
        List<tblMoneda> tblMoneda = new();
        List<tblAdmFormaPago> tblAdmFormaPago = new();
        List<tblAdmCondicionPago> tblAdmCondicionPago = new();

        public SAPInfoDumpController(bdERP context)
        {
            db = context;
            tblEmpresasPolarier = db.tblEmpresasPolarier.ToList();
            tblPais = db.tblPais.ToList();
            tblMoneda = db.tblMoneda.ToList();
            tblAdmFormaPago = db.tblAdmFormaPago.ToList();
            tblAdmCondicionPago = db.tblAdmCondicionPago.ToList();
        }

        #region CentrosCoste

        [EnableQuery]
        [HttpGet("odata/SAP/update_CentrosCoste")]
        [Authorize]
        public async Task<ActionResult> update_CentrosCoste()
        {
            CentrodeCoste.Model modelo = await SAPUtils.GetResponseAndSerialize<CentrodeCoste.Model>(sap.centrosdeCosteController.Get_CentrosCoste());
            string jsonString = JsonConvert.SerializeObject(modelo.entry.Select(x => x.content.properties).ToArray());
            List<CentroCosteJSON> registrosSAP = JsonConvert.DeserializeObject<List<CentroCosteJSON>>(jsonString)
                .Where(x => !string.IsNullOrEmpty(x.CompanyCode)).Where(x => x.codigo.Length > 0).DistinctBy(x => new { x.codigo, x.CompanyCode }).OrderBy(x => x.denominacion).ToList();

            List<tblAdmCentroCoste> enBBDD = db.tblAdmCentroCoste.Include(x => x.idEmpresaPolarierNavigation).ToList().DistinctBy(x => new
            {
                x.codigo,
                x.idEmpresaPolarierNavigation.companyCode_SAP
            }).ToList();

            List<tblAdmCentroCoste> registrosInsertados = enBBDD.Where(x => registrosSAP.Any(y => y.codigo == x.codigo && (x.idEmpresaPolarierNavigation.companyCode_SAP.Equals(y.CompanyCode)))).ToList(); ;
            List<CentroCosteJSON> registrosNoInsertados = registrosSAP
                .Where(x => !registrosInsertados
                .Select(y => new
                {
                    y.codigo,
                    CompanyCode = y.idEmpresaPolarierNavigation.companyCode_SAP
                }).Any(y => y.CompanyCode == x.CompanyCode && y.codigo == x.codigo)).ToList();
            List<tblAdmCentroCoste> registrosEliminados = enBBDD.Where(x => !registrosSAP.Any(y => y.codigo == x.codigo && (x.idEmpresaPolarierNavigation.companyCode_SAP.Equals(y.CompanyCode)))).ToList(); ;

            int id = 0;
            foreach (var reg in registrosInsertados)
            {
                var registroSAP = registrosSAP
                   .Where(x => x.codigo == reg.codigo && x.CompanyCode == reg.idEmpresaPolarierNavigation.companyCode_SAP)
                   .Select(x => new { x.denominacion, x.CompanyCode })
                   .FirstOrDefault();

                reg.denominacion = registroSAP.denominacion;
                reg.idEmpresaPolarier = tblEmpresasPolarier
                    .Where(x => x.companyCode_SAP == registroSAP.CompanyCode)
                    .Select(x => x.idEmpresaPolarier)
                    .FirstOrDefault();

                if (reg.idAdmCentroCoste > id)
                {
                    id = reg.idAdmCentroCoste;
                }
            }

            id++;

            foreach (var reg in registrosNoInsertados)
            {
                short? idEmpresaPolarier = tblEmpresasPolarier.Where(x => x.companyCode_SAP == reg.CompanyCode).Select(x => x.idEmpresaPolarier).FirstOrDefault();
                if (idEmpresaPolarier != null && idEmpresaPolarier > 0) //Vienen clientes sin empresa asociada
                {
                    db.tblAdmCentroCoste.Add(new tblAdmCentroCoste
                    {
                        idAdmCentroCoste = id,
                        codigo = reg.codigo,
                        denominacion = reg.denominacion,
                        idEmpresaPolarier = tblEmpresasPolarier.Where(x => x.companyCode_SAP == reg.CompanyCode).Select(x => x.idEmpresaPolarier).FirstOrDefault()
                    });
                    id++;
                }
            }
            foreach (var reg in registrosEliminados)
            {
                reg.isEliminado = true;
            }
            db.SaveChanges();
            return Ok();
        }

        private class CentroCosteJSON
        {
            [JsonProperty("CostCenterName")]
            public string? denominacion { get; set; }
            public string? CompanyCode { get; set; }
            [JsonProperty("CostCenter")]
            public string? codigo { get; set; }

            public CentroCosteJSON()
            {
                CompanyCode = null;
            }
        }
        #endregion

        #region ElementosPEP
        [EnableQuery]
        [HttpGet("odata/SAP/update_ElementosPEP")]
        [Authorize]
        public async Task<ActionResult> update_ElementosPEP()
        {
            ElementoPEP.Model modelo = await SAPUtils.GetResponseAndSerialize<ElementoPEP.Model>(sap.elementosPEPController.Get_ElementosPEP());
            string jsonString = JsonConvert.SerializeObject(modelo.entry.Select(x => x.content.properties).ToArray());
            List<ElementoPEPJSON> registrosSAP = JsonConvert.DeserializeObject<List<ElementoPEPJSON>>(jsonString)
                .Where(x => !string.IsNullOrEmpty(x.CompanyCode)).Where(x => x.codigo.Length > 0).DistinctBy(x => new { x.codigo, x.CompanyCode }).OrderBy(x => x.denominacion).ToList();

            List<tblAdmElementoPEP> enBBDD = db.tblAdmElementoPEP.Include(x => x.idEmpresaPolarierNavigation).ToList().DistinctBy(x => new
            {
                x.codigo,
                x.idEmpresaPolarierNavigation.companyCode_SAP
            }).ToList();

            List<tblAdmElementoPEP> registrosInsertados = enBBDD.Where(x => registrosSAP.Any(y => y.codigo == x.codigo && (x.idEmpresaPolarierNavigation.companyCode_SAP.Equals(y.CompanyCode)))).ToList();
            List<ElementoPEPJSON> registrosNoInsertados = registrosSAP
                .Where(x => !registrosInsertados
                .Select(y => new
                {
                    y.codigo,
                    CompanyCode = y.idEmpresaPolarierNavigation.companyCode_SAP,
                }).Any(y => y.CompanyCode == x.CompanyCode && y.codigo == x.codigo)).ToList();
            List<tblAdmElementoPEP> registrosEliminados = enBBDD.Where(x => !registrosSAP.Any(y => y.codigo == x.codigo && (x.idEmpresaPolarierNavigation.companyCode_SAP.Equals(y.CompanyCode)))).ToList();
            List<tblAdmCentroBeneficio> centrosBeneficio = db.tblAdmCentroBeneficio.Where(x => registrosSAP.Select(y => y.codigoCentroBeneficio).Contains(x.codigo)).ToList();
            int id = 0;
            foreach (var reg in registrosInsertados)
            {
                var registroSAP = registrosSAP
                   .Where(x => x.codigo == reg.codigo && x.CompanyCode == reg.idEmpresaPolarierNavigation.companyCode_SAP)
                   .Select(x => new { x.denominacion, x.CompanyCode, x.codigoCentroBeneficio })
                   .FirstOrDefault();

                reg.denominacion = registroSAP.denominacion;
                reg.idEmpresaPolarier = tblEmpresasPolarier
                    .Where(x => x.companyCode_SAP == registroSAP.CompanyCode)
                    .Select(x => x.idEmpresaPolarier)
                    .FirstOrDefault();
                reg.idAdmCentroBeneficio = centrosBeneficio.Where(x => x.codigo == registroSAP.codigoCentroBeneficio).Select(x => x.idAdmCentroBeneficio).FirstOrDefault();

                if (reg.idAdmElementoPEP > id)
                {
                    id = reg.idAdmElementoPEP;
                }
            }

            id++;

            foreach (var reg in registrosNoInsertados)
            {
                short? idEmpresaPolarier = tblEmpresasPolarier.Where(x => x.companyCode_SAP == reg.CompanyCode).Select(x => x.idEmpresaPolarier).FirstOrDefault();
                if (idEmpresaPolarier != null && idEmpresaPolarier > 0) //Vienen clientes sin empresa asociada
                {
                    db.tblAdmElementoPEP.Add(new tblAdmElementoPEP
                    {
                        idAdmElementoPEP = id,
                        codigo = reg.codigo,
                        denominacion = reg.denominacion,
                        idEmpresaPolarier = tblEmpresasPolarier.Where(x => x.companyCode_SAP == reg.CompanyCode).Select(x => x.idEmpresaPolarier).FirstOrDefault(),
                        idAdmCentroBeneficio = centrosBeneficio.Where(x => x.codigo == reg.codigoCentroBeneficio).Select(x => x.idAdmCentroBeneficio).FirstOrDefault()
                    });
                    id++;
                }
            }
            foreach (var reg in registrosEliminados)
            {
                reg.isEliminado = true;
            }

            db.SaveChanges();

            var tblElementoPEP = db.tblAdmElementoPEP.ToList();
            foreach (var item in registrosSAP)
            {
                var elementoPEP = tblElementoPEP.FirstOrDefault(x => x.isEliminado.Equals(false) && item.codigo == x.codigo);
                var proyecto = tblElementoPEP.FirstOrDefault(x => x.isEliminado.Equals(false) && item.codigoProyecto == x.codigo && item.codigo != x.codigo);

                if (elementoPEP != null && proyecto == null)
                {
                    elementoPEP.idAdmElementoPEPPadre = null;
                }
                else if (elementoPEP != null && proyecto != null)
                {
                    elementoPEP.idAdmElementoPEPPadre = proyecto.idAdmElementoPEP;
                }
            }

            db.SaveChanges();

            return Ok();
        }
        private class ElementoPEPJSON
        {
            [JsonProperty("WBSDescription")]
            public string? denominacion { get; set; }
            public string? CompanyCode { get; set; }
            [JsonProperty("WBSElement")]
            public string? codigo { get; set; }
            [JsonProperty("ProfitCenter")]
            public string? codigoCentroBeneficio { get; set; }
            [JsonProperty("ProjectExternalID")]
            public string? codigoProyecto { get; set; }
        }
        #endregion

        #region Clientes
        [EnableQuery]
        [HttpGet("odata/SAP/update_Clientes")]
        [Authorize]
        public async Task<ActionResult> update_Clientes()
        {
            Cliente.Model modelo = await SAPUtils.GetResponseAndSerialize<Cliente.Model>(sap.clientesController.Get_Clientes());
            string jsonString = JsonConvert.SerializeObject(modelo.entry.Select(x => x.content.properties).ToArray());
            List<ClienteJSON> registrosSAP = JsonConvert.DeserializeObject<List<ClienteJSON>>(jsonString)
                .Where(x => !string.IsNullOrEmpty(x.CompanyCode)).Where(x => x.codigo.Length > 0).DistinctBy(x => new { x.codigo, x.CompanyCode }).OrderBy(x => x.nombreFiscal).ToList();

            List<tblAdmCliente> enBBDD = db.tblAdmCliente.Include(x => x.idEmpresaPolarierNavigation).ToList()
                .DistinctBy(x => new
                {
                    x.codigo,
                    x.idEmpresaPolarierNavigation.companyCode_SAP
                }).ToList();
            List<tblAdmCliente> registrosInsertados = enBBDD.Where(x => registrosSAP.Any(y => y.codigo == x.codigo && (x.idEmpresaPolarierNavigation.companyCode_SAP.Equals(y.CompanyCode)))).ToList();
            List<ClienteJSON> registrosNoInsertados = registrosSAP.Where(x => !registrosInsertados.Select(y => new
            {
                y.codigo,
                CompanyCode = y.idEmpresaPolarierNavigation.companyCode_SAP
            }).Any(y => y.CompanyCode == x.CompanyCode && y.codigo == x.codigo)).ToList();
            List<tblAdmCliente> registrosEliminados = enBBDD.Where(x => !registrosSAP.Any(y => y.codigo == x.codigo && (x.idEmpresaPolarierNavigation.companyCode_SAP.Equals(y.CompanyCode)))).ToList();

            foreach (var reg in registrosInsertados)
            {
                var registroSAP = registrosSAP
                    .Where(x => x.codigo == reg.codigo && x.CompanyCode == reg.idEmpresaPolarierNavigation.companyCode_SAP)
                    .Select(x => new { x.nombreFiscal, x.CompanyCode, x.CIF, x.direccion, x.codigoPostal, x.poblacion, x.codigoPais, x.tipoRetencion, x.codigoRetencion })
                    .FirstOrDefault();

                reg.denominacion = registroSAP.nombreFiscal;
                reg.nombreFiscal = registroSAP.nombreFiscal;
                reg.CIF = registroSAP.CIF;
                reg.tipoRetencion = registroSAP.tipoRetencion == "" ? null : registroSAP.tipoRetencion;
                reg.codigoRetencion = decimal.TryParse(registroSAP.codigoRetencion, out decimal result) ? result / 100 : null;
                reg.direccion = registroSAP.direccion;
                reg.codigoPostal = registroSAP.codigoPostal;
                reg.poblacion = registroSAP.poblacion;
                reg.idPais = db.tblPais.FirstOrDefault(p => p.codigo == registroSAP.codigoPais)?.idPais;
                reg.idEmpresaPolarier = tblEmpresasPolarier
                .Where(x => x.companyCode_SAP == registroSAP.CompanyCode)
                .Select(x => x.idEmpresaPolarier)
                .FirstOrDefault();
            }

            foreach (var reg in registrosNoInsertados)
            {
                short? idEmpresaPolarier = tblEmpresasPolarier.Where(x => x.companyCode_SAP == reg.CompanyCode).Select(x => x.idEmpresaPolarier).FirstOrDefault();
                if (idEmpresaPolarier != null && idEmpresaPolarier > 0) //Vienen clientes sin empresa asociada
                {
                    db.tblAdmCliente.Add(new tblAdmCliente
                    {
                        idEmpresaPolarier = (short)idEmpresaPolarier,
                        codigo = reg.codigo,
                        denominacion = reg.nombreFiscal,
                        nombreFiscal = reg.nombreFiscal,
                        nombreComercial = "",
                        CIF = reg.CIF,
                        direccion = reg.direccion,
                        codigoPostal = reg.codigoPostal,
                        poblacion = reg.poblacion,
                        idPais = db.tblPais.FirstOrDefault(p => p.codigo == reg.codigoPais)?.idPais,
                        tipoRetencion = reg.tipoRetencion == "" ? null : reg.tipoRetencion,
                        codigoRetencion = decimal.TryParse(reg.codigoRetencion, out decimal result) ? result / 100 : null,
                        idMoneda = db.tblEmpresasPolarier.FirstOrDefault(ep => ep.idEmpresaPolarier == idEmpresaPolarier)?.idMoneda
                    });
                }

            }
            foreach (var reg in registrosEliminados)
            {
                reg.isEliminado = true;
            }

            db.SaveChanges();
            return Ok();
        }

        private class ClienteJSON
        {
            public string? CompanyCode { get; set; }
            [JsonProperty("Customer")]
            public string? codigo { get; set; }
            [JsonProperty("CustomerName")]
            public string? nombreFiscal { get; set; }
            [JsonProperty("TaxNumber1")]
            public string? CIF { get; set; }

            [JsonProperty("StreetName")]
            public string? direccion { get; set; }

            [JsonProperty("PostalCode")]
            public string? codigoPostal { get; set; }

            [JsonProperty("CityName")]
            public string? poblacion { get; set; }

            [JsonProperty("Country")]
            public string? codigoPais { get; set; }

            [JsonProperty("WithholdingTaxType")]
            public string? tipoRetencion { get; set; }

            [JsonProperty("WithholdingTaxCode")]
            public string? codigoRetencion { get; set; }

            public ClienteJSON()
            {
                CompanyCode = null;
            }
        }
        #endregion

        #region Proveedores
        [EnableQuery]
        [HttpGet("odata/SAP/update_Proveedores")]
        [Authorize]
        public async Task<ActionResult> update_Proveedores()
        {
            Proveedor.Model modelo = await SAPUtils.GetResponseAndSerialize<Proveedor.Model>(sap.proveedoresController.Get_Proveedores());
            string jsonString = JsonConvert.SerializeObject(modelo.entry.Select(x => x.content.properties).ToArray());

            List<ProveedorJSON> registrosSAP = JsonConvert.DeserializeObject<List<ProveedorJSON>>(jsonString)
                .Where(x => !string.IsNullOrEmpty(x.CompanyCode)).Where(x => x.codigo.Length > 0).DistinctBy(x => new { x.codigo, x.CompanyCode }).OrderBy(x => x.nombreFiscal).ToList();

            List<tblAdmProveedor> enBBDD = db.tblAdmProveedor.Include(x => x.idEmpresaPolarierNavigation).ToList()
                .DistinctBy(x => new
                {
                    x.codigo,
                    x.idEmpresaPolarierNavigation.companyCode_SAP
                }).ToList();
            List<tblAdmProveedor> registrosInsertados = enBBDD.Where(x => registrosSAP.Any(y => y.codigo == x.codigo && (x.idEmpresaPolarierNavigation.companyCode_SAP.Equals(y.CompanyCode)))).ToList();
            List<ProveedorJSON> registrosNoInsertados = registrosSAP
                .Where(x => !registrosInsertados
                .Select(y => new
                {
                    y.codigo,
                    CompanyCode = y.idEmpresaPolarierNavigation.companyCode_SAP
                }).Any(y => y.CompanyCode == x.CompanyCode && y.codigo == x.codigo)).ToList();
            List<tblAdmProveedor> registrosEliminados = enBBDD.Where(x => !registrosSAP.Any(y => y.codigo == x.codigo && (x.idEmpresaPolarierNavigation.companyCode_SAP.Equals(y.CompanyCode)))).ToList();

            foreach (var reg in registrosInsertados)
            {
                var registroSAP = registrosSAP
                    .Where(x => x.codigo == reg.codigo && x.CompanyCode == reg.idEmpresaPolarierNavigation.companyCode_SAP)
                    .Select(x => new { 
                        x.nombreFiscal,
                        x.CompanyCode,
                        x.formaPago,
                        x.condicionPago,
                        x.codigoMoneda,
                        x.pais,
                        x.provincia,
                        x.poblacion,
                        x.codigoPostal,
                        x.direccion,
                        x.telfMovil,
                        x.telfFijo,
                        x.email
                    })
                    .FirstOrDefault();

                int? idPais = tblPais.FirstOrDefault(x => x.denominacion == registroSAP.pais)?.idPais;

                reg.nombreFiscal = registroSAP.nombreFiscal;
                reg.idEmpresaPolarier = tblEmpresasPolarier
                .Where(x => x.companyCode_SAP == registroSAP.CompanyCode)
                .Select(x => x.idEmpresaPolarier)
                .FirstOrDefault();
                reg.idAdmFormaPago = tblAdmFormaPago.FirstOrDefault(x => x.codigo == registroSAP.formaPago && x.idPais == idPais)?.idAdmFormaPago; 
                reg.idAdmCondicionPago = tblAdmCondicionPago.FirstOrDefault(x => x.codigoCondicion == registroSAP.condicionPago)?.idAdmCondicionPago;
                reg.idMoneda = tblMoneda.FirstOrDefault(x => x.codigo == registroSAP.codigoMoneda)?.idMoneda;
                reg.direccion = registroSAP.direccion;
                reg.idPais = idPais;
                reg.provincia = registroSAP.provincia;
                reg.poblacion = registroSAP.poblacion;
                reg.codigoPostal = registroSAP.codigoPostal;
                reg.telfMovil = registroSAP.telfMovil;
                reg.telfFijo = registroSAP.telfFijo;
                reg.email = registroSAP.email;
            }

            foreach (var reg in registrosNoInsertados)
            {
                short? idEmpresaPolarier = tblEmpresasPolarier.Where(x => x.companyCode_SAP == reg.CompanyCode).Select(x => x.idEmpresaPolarier).FirstOrDefault();
                int? idPais = tblPais.FirstOrDefault(x => x.denominacion == reg.pais)?.idPais;
                if (idEmpresaPolarier != null && idEmpresaPolarier > 0) //Vienen clientes sin empresa asociada
                {
                    db.tblAdmProveedor.Add(new tblAdmProveedor
                    {
                        codigo = reg.codigo,
                        nombreFiscal = reg.nombreFiscal,
                        nombreComercial = "",
                        CIF = reg.CIF,
                        direccion = reg.direccion,
                        idEmpresaPolarier = (short)idEmpresaPolarier,
                        idAdmFormaPago = tblAdmFormaPago.FirstOrDefault(x => x.codigo == reg.formaPago && x.idPais == idPais)?.idAdmFormaPago,
                        idAdmCondicionPago = tblAdmCondicionPago.FirstOrDefault(x => x.codigoCondicion == reg.condicionPago)?.idAdmCondicionPago,
                        idMoneda = tblMoneda.FirstOrDefault(x => x.codigo == reg.codigoMoneda)?.idMoneda,
                        idPais = idPais,
                        provincia = reg.provincia,
                        poblacion = reg.poblacion,
                        codigoPostal = reg.codigoPostal,
                        telfMovil = reg.telfMovil,
                        telfFijo = reg.telfFijo,
                        email = reg.email
                    });
                }
            }
            foreach (var reg in registrosEliminados)
            {
                reg.isEliminado = true;
            }

            db.SaveChanges();
            return Ok();
        }
        private class ProveedorJSON
        {
            public string? CompanyCode { get; set; }
            [JsonProperty("SupplierName")]
            public string? nombreFiscal { get; set; }
            [JsonProperty("Supplier")]
            public string? codigo { get; set; }
            [JsonProperty("TaxNumber1")]
            public string? CIF { get; set; }
            [JsonProperty("PaymentMethodsList")]
            public string? formaPago { get; set; }
            [JsonProperty("PaymentTerms")]
            public string? condicionPago { get; set; }
            [JsonProperty("PurchaseOrderCurrency")]
            public string? codigoMoneda { get; set; }
            [JsonProperty("CountryName")]
            public string? pais { get; set; }
            [JsonProperty("RegionName")]
            public string? provincia { get; set; }
            [JsonProperty("CityName")]
            public string? poblacion { get; set; }
            [JsonProperty("PostalCode")]
            public string? codigoPostal { get; set; }
            [JsonProperty("BPAddrStreetName")]
            public string? direccion { get; set; }
            [JsonProperty("PhoneNumber2")]
            public string? telfMovil { get; set; }
            [JsonProperty("PhoneNumber1")]
            public string? telfFijo { get; set; }
            [JsonProperty("EmailAddress")]
            public string? email { get; set; }

            public ProveedorJSON()
            {
                CompanyCode = null;
            }
        }

        #endregion

        #region FormasPago
        [EnableQuery]
        [HttpGet("odata/SAP/update_FormasPago")]
        [Authorize]
        public async Task<ActionResult> update_FormasPago()
        {
            ViasPagoCobro.Model modelo = await SAPUtils.GetResponseAndSerialize<ViasPagoCobro.Model>(sap.viasPagoCobroController.Get_ViasPagoCobro());
            string jsonString = JsonConvert.SerializeObject(modelo.entry.Select(x => x.content.properties).ToArray());
            List<FormaPagoJSON> registrosSAP = JsonConvert.DeserializeObject<List<FormaPagoJSON>>(jsonString).OrderBy(x => x.denominacion).ToList();

            List<tblAdmFormaPago> registrosInsertados = db.tblAdmFormaPago
                .Include(x => x.idPaisNavigation)
                .Where(x => registrosSAP.Select(y => y.codigo).Contains(x.codigo) && registrosSAP.Select(y => y.CodigoPaisAbr).Contains(x.idPaisNavigation.codigo))
                .ToList();
            List<FormaPagoJSON> registrosNoInsertados = registrosSAP
                .Where(x => !registrosInsertados.Select(y => y.codigo).Contains(x.codigo) || !registrosInsertados.Select(y => y.idPaisNavigation.codigo).Contains(x.CodigoPaisAbr))
                .ToList();

            foreach (var reg in registrosInsertados)
            {
                var denominacion = registrosSAP
                     .Where(x => x.codigo == reg.codigo)
                     .Select(x => x.denominacion)
                     .FirstOrDefault();

                reg.denominacion = denominacion;
            }

            foreach (var reg in registrosNoInsertados)
            {
                string codigoPais = reg.CodigoPaisAbr;
                int? idPais = db.tblPais.Where(x => x.codigo == codigoPais).Select(x => x.idPais).FirstOrDefault();
                if (idPais != null)
                {
                    db.tblAdmFormaPago.Add(new tblAdmFormaPago
                    {
                        codigo = reg.codigo,
                        denominacion = reg.denominacion,
                        idPais = (int)idPais
                    }); ;
                }
            }
            db.SaveChanges();
            return Ok();
        }
        private class FormaPagoJSON
        {
            [JsonProperty("PaymentMethodName")]
            public string? denominacion { get; set; }

            [JsonProperty("PaymentMethod")]
            public string? codigo { get; set; }

            [JsonProperty("Country")]
            public string? CodigoPaisAbr { get; set; }
        }
        #endregion

        #region CuentaContable
        [EnableQuery]
        [HttpGet("odata/SAP/update_CuentaContable")]
        [Authorize]
        public async Task<ActionResult> update_CuentaContable()
        {
            CuentaContable.Model modelo = await SAPUtils.GetResponseAndSerialize<CuentaContable.Model>(sap.cuentaContableController.Get_CuentasContables());
            string jsonString = JsonConvert.SerializeObject(modelo.entry.Select(x => x.content.properties).ToArray());
            List<CuentasContableJSON> registrosSAP = JsonConvert.DeserializeObject<List<CuentasContableJSON>>(jsonString)
                .Where(x => x.codigo.Length > 0).DistinctBy(x => x.codigo).OrderBy(x => x.denominacion).ToList();
            List<tblAdmCuentaContable> registrosInsertados = db.tblAdmCuentaContable.Where(x => registrosSAP.Select(y => y.codigo).Contains(x.codigo)).ToList();
            List<CuentasContableJSON> registrosNoInsertados = registrosSAP.Where(x => !registrosInsertados.Select(y => y.codigo).Contains(x.codigo)).ToList();
            //List<tblAdmCuentasContable> registrosEliminados = db.tblAdmCuentasContable.Where(x => !registrosSAP.Select(y => y.codigo).Contains(x.codigo)).ToList();

            foreach (var reg in registrosInsertados)
            {
                string denominacion = registrosSAP
                    .Where(x => x.codigo == reg.codigo)
                    .Select(x => x.denominacion)
                    .FirstOrDefault();

                reg.denominacion = denominacion;
            }

            foreach (var reg in registrosNoInsertados)
            {
                db.tblAdmCuentaContable.Add(new tblAdmCuentaContable
                {
                    codigo = reg.codigo,
                    denominacion = reg.denominacion
                });
            }
            //foreach (var reg in registrosEliminados)
            //{
            //    reg.isEliminado = true;
            //}
            db.SaveChanges();
            return Ok();
        }
        private class CuentasContableJSON
        {
            [JsonProperty("GLAccountLongName")]
            public string? denominacion { get; set; }
            [JsonProperty("GLAccount")]
            public string? codigo { get; set; }
        }
        #endregion

        #region GrupoArticulos
        [EnableQuery]
        [HttpGet("odata/SAP/update_GrupoArticulos")]
        [Authorize]
        public async Task<ActionResult> update_GrupoArticulos()
        {
            GrupoArticulos.Model modelo = await SAPUtils.GetResponseAndSerialize<GrupoArticulos.Model>(sap.grupoArticulosController.Get_GrupoArticulos());
            string jsonString = JsonConvert.SerializeObject(modelo.entry.Select(x => x.content.properties).ToArray());
            List<GrupoArticulosJSON> registrosSAP = JsonConvert.DeserializeObject<List<GrupoArticulosJSON>>(jsonString);
            List<tblGrupoArticulos> registrosInsertados = db.tblGrupoArticulos.Where(x => registrosSAP.Select(y => y.codigo).Contains(x.codigo)).ToList();
            List<GrupoArticulosJSON> registrosNoInsertados = registrosSAP.Where(x => !registrosInsertados.Select(y => y.codigo).Contains(x.codigo)).ToList();
            List<tblGrupoArticulos> registrosEliminados = db.tblGrupoArticulos.Where(x => !registrosSAP.Select(y => y.codigo).Contains(x.codigo)).ToList();

            foreach (var reg in registrosInsertados)
            {
                var registroSAP = registrosSAP
                    .Where(x => x.codigo == reg.codigo)
                    .Select(x => new
                    {
                        x.denominacion,
                        x.codigo
                    })
                    .FirstOrDefault();

                int idAdmCuentaContableCompra = db.tblAdmCuentaContable.Where(x => x.codigo == registroSAP.codigo).Select(x => x.idAdmCuentaContable).FirstOrDefault();

                reg.denominacion = registroSAP.denominacion;
                reg.idAdmCuentaContableCompra = idAdmCuentaContableCompra;
            }

            foreach (var reg in registrosNoInsertados)
            {
                int? idAdmCuentaContable = db.tblAdmCuentaContable.Where(x => x.codigo == reg.codigo).Select(x => x.idAdmCuentaContable).FirstOrDefault();
                if (idAdmCuentaContable != null && idAdmCuentaContable > 0)
                {
                    db.tblGrupoArticulos.Add(new tblGrupoArticulos
                    {
                        codigo = reg.codigo,
                        denominacion = reg.denominacion,
                        idAdmCuentaContableCompra = (int)idAdmCuentaContable
                    });
                }
            }

            foreach (var reg in registrosEliminados)
            {
                reg.isEliminado = true;
            }

            db.SaveChanges();
            return Ok();
        }
        private class GrupoArticulosJSON
        {
            [JsonProperty("MaterialGroupName")]
            public string? denominacion { get; set; }
            [JsonProperty("MaterialGroup")]
            public string? codigo { get; set; }
        }
        #endregion

        #region CondicionPago
        [EnableQuery]
        [HttpGet("odata/SAP/update_CondicionPago")]
        [Authorize]
        public async Task<ActionResult> update_CondicionPago()
        {
            CondicionPago.Model modelo = await SAPUtils.GetResponseAndSerialize<CondicionPago.Model>(sap.condicionPagoController.Get());
            string jsonString = JsonConvert.SerializeObject(modelo.entry.Select(x => x.content.properties).ToArray());
            List<CondicionPagoJSON> registrosSAP = JsonConvert.DeserializeObject<List<CondicionPagoJSON>>(jsonString).Where(x => !x.denominacionCondicion.ToLower().Contains("no usar")).ToList();

            List<tblAdmCondicionPago> registrosInsertados = db.tblAdmCondicionPago.Where(x => registrosSAP.Select(x => x.codigoCondicion).Contains(x.codigoCondicion) && registrosSAP.Select(x => x.codigoTermino).Contains(x.codigoTermino)).ToList();
            List<CondicionPagoJSON> registrosNoInsertados = registrosSAP.Where(x => !registrosInsertados.Select(x => x.codigoCondicion).Contains(x.codigoCondicion) && !registrosInsertados.Select(x => x.codigoTermino).Contains(x.codigoTermino)).ToList();
            //List<tblAdmCondicionPago> registrosEliminados = db.tblAdmCondicionPago.Where(x => registrosSAP.Where(y => y.codigoCondicion == x.codigoCondicion && y.codigoTermino == x.codigoTermino).Count() == 0).ToList();

            foreach (var reg in registrosInsertados)
            {
                var registroSAP = registrosSAP
                    .Where(x => x.codigoTermino == reg.codigoTermino && x.codigoCondicion == reg.codigoCondicion)
                    .Select(x => new { x.denominacionTermino, x.denominacionCondicion, x.codigoTermino, x.codigoCondicion })
                    .FirstOrDefault();

                reg.denominacionTermino = registroSAP.denominacionTermino;
                reg.denominacionCondicion = registroSAP.denominacionCondicion;
                reg.numDiasPago = !string.IsNullOrEmpty(registroSAP.codigoTermino) && short.TryParse(registroSAP.codigoTermino?[1..], out short diasTermino)
                    ? diasTermino
                    : !string.IsNullOrEmpty(registroSAP.codigoCondicion) && short.TryParse(registroSAP.codigoCondicion?[1..], out short diasCondicion)
                        ? diasCondicion
                        : (short)0;
            }

            db.tblAdmCondicionPago.AddRange(registrosNoInsertados.Select(x => new tblAdmCondicionPago
            {
                denominacionCondicion = x.denominacionCondicion,
                codigoCondicion = x.codigoCondicion,
                denominacionTermino = x.denominacionTermino,
                codigoTermino = x.codigoTermino,
                numDiasPago = short.TryParse(x.codigoTermino?[1..], out short diasTermino)
                    ? diasTermino
                    : short.TryParse(x.codigoCondicion?[1..], out short diasCondicion)
                        ? diasCondicion
                        : (short)0
            }));

            //foreach (var reg in registrosEliminados)
            //{
            //    reg.isEliminado = true;
            //}

            db.SaveChanges();
            return Ok();
        }
        private class CondicionPagoJSON
        {
            [JsonProperty("PaymentTerms")]
            public string? codigoCondicion { get; set; }
            [JsonProperty("InstallmentItemPaymentTerms")]
            public string? codigoTermino { get; set; }
            [JsonProperty("PaymentTermsConditionDesc")]
            public string? denominacionCondicion { get; set; }
            [JsonProperty("PaymentTermsName")]
            public string? denominacionTermino { get; set; }
        }
        #endregion

        #region CentrosBeneficio
        [EnableQuery]
        [HttpGet("odata/SAP/update_CentrosBeneficio")]
        [Authorize]
        public async Task<ActionResult> update_CentrosBeneficio()
        {
            CentrosBeneficio.Model modelo = await SAPUtils.GetResponseAndSerialize<CentrosBeneficio.Model>(sap.centrosBeneficioController.Get_CentrosBeneficio());
            string jsonString = JsonConvert.SerializeObject(modelo.entry.Select(x => x.content.properties).ToArray());
            List<CentrosBeneficioJSON> registrosSAP = JsonConvert.DeserializeObject<List<CentrosBeneficioJSON>>(jsonString)
                .Where(x => x.codigo.Length > 0).DistinctBy(x => x.codigo).OrderBy(x => x.denominacion).ToList();
            List<tblAdmCentroBeneficio> registrosInsertados = db.tblAdmCentroBeneficio.Where(x => registrosSAP.Select(y => y.codigo).Contains(x.codigo)).ToList();
            List<CentrosBeneficioJSON> registrosNoInsertados = registrosSAP.Where(x => !registrosInsertados.Select(y => y.codigo).Contains(x.codigo)).ToList();
            //List<tblAdmCuentasContable> registrosEliminados = db.tblAdmCuentasContable.Where(x => !registrosSAP.Select(y => y.codigo).Contains(x.codigo)).ToList();

            foreach (var reg in registrosInsertados)
            {
                string denominacion = registrosSAP
                    .Where(x => x.codigo == reg.codigo)
                    .Select(x => x.denominacion)
                    .FirstOrDefault();

                reg.denominacion = denominacion;
            }

            foreach (var reg in registrosNoInsertados)
            {
                db.tblAdmCentroBeneficio.Add(new tblAdmCentroBeneficio
                {
                    codigo = reg.codigo,
                    denominacion = reg.denominacion
                });
            }
            //foreach (var reg in registrosEliminados)
            //{
            //    reg.isEliminado = true;
            //}
            db.SaveChanges();
            return Ok();
        }
        private class CentrosBeneficioJSON
        {
            [JsonProperty("ProfitCenterLongName")]
            public string? denominacion { get; set; }
            [JsonProperty("ProfitCenter")]
            public string? codigo { get; set; }
        }
        #endregion

        public async Task UpdateAll()
        {
            await update_CentrosBeneficio();
            await update_CentrosCoste();
            await update_ElementosPEP();
            await update_Clientes();
            await update_Proveedores();
            await update_FormasPago();
            await update_GrupoArticulos();
            await update_CuentaContable();
            await update_CondicionPago();
        }
    }
}