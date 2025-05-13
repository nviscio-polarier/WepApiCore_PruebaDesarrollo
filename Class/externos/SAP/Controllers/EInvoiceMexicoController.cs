using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class EInvoiceMexicoController : ODataController
    {
        public async Task<HttpResponseMessage> Get(string UUID)
        {
            string endpoint = "YY1_API_EINVOICE_MEXICOTYPESET_PRD;v=1/YY1_API_eInvoice_MexicoTypeSet?$filter=" +
                $"JrnlEntryCntrySpecificRef1 eq '{UUID}'";
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
