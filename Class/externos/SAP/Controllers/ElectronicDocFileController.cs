using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class ElectronicDocFileController : ODataController
    {
        public async Task<HttpResponseMessage> Get(string UUID)
        {
            string endpoint = $"getElectronicDocFile";//?$filter=ElectronicDocUUID eq '{UUID}'";
            return await SAPHttpService.Get_ElectronicDocFile(endpoint);
        }
    }
}
