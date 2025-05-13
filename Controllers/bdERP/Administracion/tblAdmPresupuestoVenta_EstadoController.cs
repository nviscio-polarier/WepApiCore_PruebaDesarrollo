using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmPresupuestoVenta_EstadoController : ODataController
    {
        private readonly bdERP db;

        public tblAdmPresupuestoVenta_EstadoController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmPresupuestoVenta_Estado>> Get()
        {
            return db.tblAdmPresupuestoVenta_Estado;
        }
    }
}
