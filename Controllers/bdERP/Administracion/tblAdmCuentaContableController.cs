using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;


namespace WebApiCore.Controllers
{
    public class tblAdmCuentaContableController : ODataController
    {
        private readonly bdERP db;
        public tblAdmCuentaContableController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmCuentaContable>> Get()
        {
            return db.tblAdmCuentaContable;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmCuentaContable>> Get([FromODataUri] int key)
        {
            return db.tblAdmCuentaContable.Where(x => x.idAdmCuentaContable.Equals(key));
        }
    }
}
