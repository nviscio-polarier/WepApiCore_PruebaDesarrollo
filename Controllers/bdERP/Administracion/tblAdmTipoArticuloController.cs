using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmTipoArticuloController : ODataController
    {
        private readonly bdERP db;
        public tblAdmTipoArticuloController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(db.tblAdmTipoArticulo);
        }
    }
}
