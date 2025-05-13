using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class CentrosBeneficioController : ODataController
    {
        public async Task<HttpResponseMessage> Get_CentrosBeneficio(string odata = "")
        {
            string endpoint = "YY1_CENTROSDEBENEFICIO_CDS_PRD;v=1/YY1_CentrosdeBeneficioTypeSet";
            if (odata.Length > 0) endpoint += "?" + odata;

            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
