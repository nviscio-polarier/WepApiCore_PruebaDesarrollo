using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;


namespace WebApiCore.Controllers
{
    public class tblIvaNPaisController : ODataController
    {
        private readonly bdERP db;

        public tblIvaNPaisController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public IQueryable<tblIvaNPais> Get()
        {
            return db.tblIvaNPais;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public IQueryable<tblIvaNPais> Get([FromODataUri] int key)
        {
            return db.tblIvaNPais.Where(x => x.idIvaNPais == key);
        }
    }
}
