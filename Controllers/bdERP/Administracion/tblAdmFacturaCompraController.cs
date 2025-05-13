using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmFacturaCompraController : ODataController
    {
        private readonly bdERP db;

        public tblAdmFacturaCompraController(bdERP context)
        {
            db = context;
        }

        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(db.tblAdmFacturaCompra);
        }
    }
}
