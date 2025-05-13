using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;


namespace WebApiCore.Controllers
{
    public class tblAdmBancoController : ODataController
    {
        private readonly bdERP db;

        public tblAdmBancoController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmBanco>> Get()
        {
            return db.tblAdmBanco;
        }
    }
}
