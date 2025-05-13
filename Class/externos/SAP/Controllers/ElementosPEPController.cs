using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class ElementosPEPController : ODataController
    {
        public async Task<HttpResponseMessage> Get_ElementosPEP()
        {
            string endpoint = "YY1_ELEMENTOSPEP_CDS_PRD;v=1/YY1_ElementosPEPTypeSet";
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
