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
    public class tblArticuloNAdmPedidoProveedorController : ODataController
    {
        private readonly bdERP db;
        public tblArticuloNAdmPedidoProveedorController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(db.tblArticuloNAdmPedidoProveedor);
        }

        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblArticuloNAdmPedidoProveedor articuloNAdmPedidoProveedor)
        {
            db.tblArticuloNAdmPedidoProveedor.Add(articuloNAdmPedidoProveedor);
            await db.SaveChangesAsync();

            return Created(articuloNAdmPedidoProveedor);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblArticuloNAdmPedidoProveedor> articuloNAdmPedidoProveedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.tblArticuloNAdmPedidoProveedor.FirstOrDefaultAsync(x => x.idArticuloNAdmPedidoProveedor == key);

            if (entity == null)
            {
                return NotFound();
            }

            articuloNAdmPedidoProveedor.ApplyTo(entity);
            await db.SaveChangesAsync();

            return Updated(entity);
        }
    }
}
