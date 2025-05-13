using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmFactura_EstadoController : ODataController
    {
        private readonly bdERP db;

        public tblAdmFactura_EstadoController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmFactura_Estado>> Get()
        {
            return db.tblAdmFactura_Estado;
        }
    }
}
