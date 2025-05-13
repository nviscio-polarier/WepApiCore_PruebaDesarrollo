using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class CondicionesCobroController : ODataController
    {
        public async Task<HttpResponseMessage> Get_CondicionesCobro()
        {
            string endpoint = "YY1_CONDICIONESDEPAGOYCOBR_CDS_PRD;v=1/YY1_CondicionesdepagoycobrTypeSet";
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
