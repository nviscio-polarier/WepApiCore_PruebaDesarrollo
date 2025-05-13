using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblPrendaNSolicitudAbonoController : ODataController
    {

        private readonly bdERP db;

        public tblPrendaNSolicitudAbonoController(bdERP context)
        {
            db = context;
        }

        [EnableQuery(MaxExpansionDepth = 6)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            return Ok(db.tblPrendaNSolicitudAbono);
        }
    }
}





