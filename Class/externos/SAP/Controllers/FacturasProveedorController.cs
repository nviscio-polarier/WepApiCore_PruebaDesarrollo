using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class FacturasProveedorController : ODataController
    {
        //[EnableQuery]
        //[HttpGet("odata/SAP/Get_Clientes")]
        public async Task<HttpResponseMessage> Get(string odata = "")
        {
            string endpoint = "YY1_FACTURASPROVEEDOR_CDS_PRD;v=1/YY1_FacturasProveedorTypeSet";
            if (odata.Length > 0) endpoint += "?" + odata;
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
