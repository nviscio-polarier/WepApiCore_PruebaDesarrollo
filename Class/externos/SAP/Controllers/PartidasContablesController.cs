using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class PartidasContablesController : ODataController
    {
        //[EnableQuery]
        //[HttpGet("odata/SAP/Get_Clientes")]
        public async Task<HttpResponseMessage> Get(string odata = "")
        {
            string endpoint = "YY1_PARTIDASCONTABLES_CDS_PROD;v=1/YY1_PartidasContablesTypeSet";
            if (odata.Length > 0) endpoint += "?" + odata;
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
