using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class GrupoArticulosController : ODataController
    {
        //[EnableQuery]
        //[HttpGet("odata/SAP/Get_GrupoArticulos")]
        public async Task<HttpResponseMessage> Get_GrupoArticulos()
        {
            string endpoint = "YY1_GRUPOSDEARTICULOS_CDS_PRD;v=1/YY1_GruposdeArticulosTypeSet";
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
