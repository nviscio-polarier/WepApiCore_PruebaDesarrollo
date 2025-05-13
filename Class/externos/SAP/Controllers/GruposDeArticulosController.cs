using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class GruposDeArticulosController : ODataController
    {
        public async Task<HttpResponseMessage> Get_GruposDeArticulos()
        {
            string endpoint = "YY1_GRUPOSDEARTICULOS_CDS_PRD;v=1/YY1_GruposdeArticulosTypeSet?";
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
