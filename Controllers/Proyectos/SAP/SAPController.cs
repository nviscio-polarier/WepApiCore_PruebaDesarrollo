using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Newtonsoft.Json;
using WebApiCore.Class.externos.SAP;
using WebApiCore.Class.externos.SAP.Context;
using WebApiCore.Class.externos.SAP.Controllers;
using WebApiCore.Security;


namespace WebApiCore.Controllers.Proyectos.SAP
{
    public class SAPController : ODataController
    {
        private readonly SAPWrap sap = new();

        [EnableQuery]
        [HttpGet("odata/SAP/Get_Clientes")]
        [Authorize]
        public async Task<ActionResult> Get_Clientes()
        {
            Cliente.Model cliente = await SAPUtils.GetResponseAndSerialize<Cliente.Model>(sap.clientesController.Get_Clientes());
            string jsonString = JsonConvert.SerializeObject(cliente.entry.Select(x => x.content.properties).ToArray());
            return Content(jsonString, "application/JSON");
        }

        [EnableQuery]
        [HttpGet("odata/SAP/Get_Proveedores")]
        [Authorize]
        public async Task<ActionResult> Get_Proveedores()
        {
            Proveedor.Model proveedor = await SAPUtils.GetResponseAndSerialize<Proveedor.Model>(sap.proveedoresController.Get_Proveedores());
            string jsonString = JsonConvert.SerializeObject(proveedor.entry.Select(x => x.content.properties).ToArray());
            return Content(jsonString, "application/JSON");
        }

        [EnableQuery]
        [HttpGet("odata/SAP/Get_CondicionesCobro")]
        [Authorize]
        public async Task<ActionResult> Get_CondicionesCobro()
        {
            CondicionesCobro.Model condicionesCobro = await SAPUtils.GetResponseAndSerialize<CondicionesCobro.Model>(sap.condicionesCobroController.Get_CondicionesCobro());
            string jsonString = JsonConvert.SerializeObject(condicionesCobro.entry.Select(x => x.content.properties).ToArray());
            return Content(jsonString, "application/JSON");
        }

        [EnableQuery]
        [HttpGet("odata/SAP/Get_ViasPagoCobro")]
        [Authorize]
        public async Task<ActionResult> Get_ViasPagoCobro()
        {
            ViasPagoCobro.Model viasPagoCobro = await SAPUtils.GetResponseAndSerialize<ViasPagoCobro.Model>(sap.viasPagoCobroController.Get_ViasPagoCobro());
            string jsonString = JsonConvert.SerializeObject(viasPagoCobro.entry.Select(x => x.content.properties).ToArray());
            return Content(jsonString, "application/JSON");
        }

        [EnableQuery]
        [HttpGet("odata/SAP/Get_CentrosCoste")]
        [Authorize]
        public async Task<ActionResult> Get_CentrosCoste()
        {
            CentrodeCoste.Model centrodeCoste = await SAPUtils.GetResponseAndSerialize<CentrodeCoste.Model>(sap.centrosdeCosteController.Get_CentrosCoste());
            string jsonString = JsonConvert.SerializeObject(centrodeCoste.entry.Select(x => x.content.properties).ToArray());
            return Content(jsonString, "application/JSON");
        }

        [EnableQuery]
        [HttpGet("odata/SAP/Get_ElementosPEP")]
        [Authorize]
        public async Task<ActionResult> Get_ElementosPEP()
        {
            ElementoPEP.Model elementoPEP = await SAPUtils.GetResponseAndSerialize<ElementoPEP.Model>(sap.elementosPEPController.Get_ElementosPEP());
            string jsonString = JsonConvert.SerializeObject(elementoPEP.entry.Select(x => x.content.properties).ToArray());
            return Content(jsonString, "application/JSON");
        }

        [EnableQuery]
        [HttpGet("odata/SAP/Get_GruposDeArticulos")]
        [Authorize]
        public async Task<ActionResult> Get_GruposDeArticulos()
        {
            GruposDeArticulos.Model gruposDeArticulos = await SAPUtils.GetResponseAndSerialize<GruposDeArticulos.Model>(sap.gruposDeArticulosController.Get_GruposDeArticulos());
            string jsonString = JsonConvert.SerializeObject(gruposDeArticulos.entry.Select(x => x.content.properties).ToArray());
            return Content(jsonString, "application/JSON");

        }
    }
}
