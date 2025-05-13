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
    public class tblArticuloNAdmAlbaranCompraController : ODataController
    {
        private readonly bdERP db;
        public tblArticuloNAdmAlbaranCompraController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(db.tblArticuloNAdmAlbaranCompra);
        }

        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblArticuloNAdmAlbaranCompra articuloNAdmAlbaranCompra)
        {
            db.tblArticuloNAdmAlbaranCompra.Add(articuloNAdmAlbaranCompra);
            await db.SaveChangesAsync();

            return Created(articuloNAdmAlbaranCompra);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblArticuloNAdmAlbaranCompra> articuloNAdmAlbaranCompra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.tblArticuloNAdmAlbaranCompra.FirstOrDefaultAsync(x => x.idArticuloNAdmAlbaranCompra == key);

            if (entity == null)
            {
                return NotFound();
            }

            articuloNAdmAlbaranCompra.ApplyTo(entity);
            await db.SaveChangesAsync();

            return Updated(entity);
        }
    }
}
