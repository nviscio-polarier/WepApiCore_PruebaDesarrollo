using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class CuentaContableController : ODataController
    {
        public async Task<HttpResponseMessage> Get_CuentasContables(string odata = "")
        {
            string endpoint = "YY1_CUENTASCONTABLES_CDS_PRD;v=1/YY1_CuentasContables";
            if (odata.Length > 0) endpoint += "?" + odata;
            return await SAPHttpService.Get_CDS(endpoint);
        }
    }
}
