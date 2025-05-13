using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmAlbaranVentaController : ODataController
    {
        private readonly bdERP db;
        public tblAdmAlbaranVentaController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmAlbaranVenta>> Get()
        {
            return db.tblAdmAlbaranVenta;
        }

        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblAdmAlbaranVenta albaranVenta)
        {
            db.tblAdmAlbaranVenta.Add(albaranVenta);
            await db.SaveChangesAsync();

            return Created(albaranVenta);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblAdmAlbaranVenta> albaranVenta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.tblAdmAlbaranVenta.Include(x => x.tblAdmArticuloNAdmAlbaranVenta).FirstOrDefaultAsync(x => x.idAdmAlbaranVenta == key);

            if (entity == null)
            {
                return NotFound();
            }

            albaranVenta.ApplyTo(entity);
            return Updated(entity);
        }
    }
}
