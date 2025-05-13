using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class ProveedoresController : ODataController
    {
        public async Task<HttpResponseMessage> Get_Proveedores()
        {
            string endpoint = "YY1_PROVEEDORES_CDS_PRD;v=1/YY1_ProveedoresTypeSet";
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
