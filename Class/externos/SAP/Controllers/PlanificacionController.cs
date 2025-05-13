using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class PlanificacionController : ODataController
    {
        //[EnableQuery]
        //[HttpGet("odata/SAP/Get_Clientes")]
        public async Task<HttpResponseMessage> Get(string odata = "")
        {
            string endpoint = "YY1_PLANIFICACION_CDS_PRD;v=1/YY1_PlanificacionTypeSet";
            if (!string.IsNullOrEmpty(odata))
            {
                endpoint += "?" + odata;
            }
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
