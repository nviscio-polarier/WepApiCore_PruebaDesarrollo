using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class ViasPagoCobroController : ODataController
    {
        public async Task<HttpResponseMessage> Get_ViasPagoCobro()
        {
            string endpoint = "YY1_VIASDEPAGOYCOBRO_CDS_PRD;v=1/YY1_ViasdepagoycobroTypeSet?";
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
