using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmFacturaVentaController : ODataController
    {
        private readonly bdERP db;

        public tblAdmFacturaVentaController(bdERP context)
        {
            db = context;
        }

        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(db.tblAdmFacturaVenta);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public ActionResult Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblAdmFacturaVenta> facturaVenta)
        {
            var entity = db.tblAdmFacturaVenta.Find(key);
            if (entity == null)
            {
                return NotFound();
            }

            facturaVenta.ApplyTo(entity);
            db.SaveChanges();

            return Updated(entity);
        }
    }
}
