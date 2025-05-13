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
    public class tblArticuloNAdmPresupuestoVentaController : ODataController
    {
        private readonly bdERP db;
        public tblArticuloNAdmPresupuestoVentaController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(db.tblArticuloNAdmPresupuestoVenta);
        }

        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblArticuloNAdmPresupuestoVenta articuloNAdmPresupuestoVenta)
        {
            db.tblArticuloNAdmPresupuestoVenta.Add(articuloNAdmPresupuestoVenta);
            await db.SaveChangesAsync();

            return Created(articuloNAdmPresupuestoVenta);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblArticuloNAdmPresupuestoVenta> articuloNAdmPresupuestoVenta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.tblArticuloNAdmPresupuestoVenta.FirstOrDefaultAsync(x => x.idArticuloNAdmPresupuestoVenta == key);

            if (entity == null)
            {
                return NotFound();
            }

            articuloNAdmPresupuestoVenta.ApplyTo(entity);
            await db.SaveChangesAsync();

            return Updated(entity);
        }
    }
}
