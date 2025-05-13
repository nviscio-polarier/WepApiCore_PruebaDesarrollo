using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmPedido_EstadoController : ODataController
    {
        private readonly bdERP db;

        public tblAdmPedido_EstadoController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmPedido_Estado>> Get()
        {
            return db.tblAdmPedido_Estado;
        }
    }
}
