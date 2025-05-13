using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class AllOriginalsController : ODataController
    {
        public async Task<HttpResponseMessage> Get(string LinkedSAPObjectKey, string BusinessObjectTypeName = "BKPF")
        {
            string endpoint = $"GetAllOriginals?" +
                $"LinkedSAPObjectKey={LinkedSAPObjectKey}&" +
                $"BusinessObjectTypeName={BusinessObjectTypeName}";
            return await SAPHttpService.Get_Attachments(endpoint);
        }
    }
}
