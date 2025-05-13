using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmAlbaran_EstadoController : ODataController
    {
        private readonly bdERP db;

        public tblAdmAlbaran_EstadoController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmAlbaran_Estado>> Get()
        {
            return db.tblAdmAlbaran_Estado;
        }
    }
}
