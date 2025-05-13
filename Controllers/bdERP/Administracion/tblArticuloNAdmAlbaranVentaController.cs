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
    public class tblArticuloNAdmAlbaranVentaController : ODataController
    {
        private readonly bdERP db;
        public tblArticuloNAdmAlbaranVentaController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(db.tblArticuloNAdmAlbaranVenta);
        }

        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblArticuloNAdmAlbaranVenta articuloNAdmAlbaranVenta)
        {
            db.tblArticuloNAdmAlbaranVenta.Add(articuloNAdmAlbaranVenta);
            await db.SaveChangesAsync();

            return Created(articuloNAdmAlbaranVenta);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblArticuloNAdmAlbaranVenta> articuloNAdmAlbaranVenta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.tblArticuloNAdmAlbaranVenta.FirstOrDefaultAsync(x => x.idArticuloNAdmAlbaranVenta == key);

            if (entity == null)
            {
                return NotFound();
            }

            articuloNAdmAlbaranVenta.ApplyTo(entity);
            await db.SaveChangesAsync();

            return Updated(entity);
        }
    }
}
