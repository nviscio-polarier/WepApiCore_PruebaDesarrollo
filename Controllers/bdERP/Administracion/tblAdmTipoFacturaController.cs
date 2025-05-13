using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmTipoFacturaController : ODataController
    {
        private readonly bdERP db;

        public tblAdmTipoFacturaController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmTipoFactura>> Get()
        {
            return db.tblAdmTipoFactura;
        }
    }
}
