using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class CondicionPagoController : ODataController
    {
        //[EnableQuery]
        //[HttpGet("odata/SAP/Get_GrupoArticulos")]
        public async Task<HttpResponseMessage> Get(string odata = "")
        {
            string endpoint = "YY1_CONDICIONESDEPAGOYCOBR_CDS_PRD;v=1/YY1_CondicionesdepagoycobrTypeSet";
            if (odata.Length > 0) endpoint += "?" + odata;
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
