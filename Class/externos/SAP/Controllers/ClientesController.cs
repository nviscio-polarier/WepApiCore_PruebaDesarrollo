using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class ClientesController : ODataController
    {
        //[EnableQuery]
        //[HttpGet("odata/SAP/Get_Clientes")]
        public async Task<HttpResponseMessage> Get_Clientes()
        {
            string endpoint = "YY1_CLIENTES_CDS_PRD;v=1/YY1_ClientesTypeSet";
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
