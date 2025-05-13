using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class CentrosdeCosteController : ODataController
    {
        public async Task<HttpResponseMessage> Get_CentrosCoste(string odata = "")
        {
            string endpoint = "YY1_CENTROSDECOSTE_CDS_PRD;v=1/YY1_CentrosdeCosteTypeSet";
            if (odata.Length > 0) endpoint += "?" + odata;

            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
