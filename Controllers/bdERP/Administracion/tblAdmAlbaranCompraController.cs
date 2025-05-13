using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmAlbaranCompraController : ODataController
    {
        private readonly bdERP db;
        public tblAdmAlbaranCompraController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmAlbaranCompra>> Get()
        {
            return db.tblAdmAlbaranCompra;
        }
    }
}
