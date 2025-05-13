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
    public class tblAdmPedidoProveedorController : ODataController
    {
        private readonly bdERP db;
        public tblAdmPedidoProveedorController(bdERP context)
        {
            db = context;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize]
        public async Task<IQueryable<tblAdmPedidoProveedor>> Get()
        {
            return db.tblAdmPedidoProveedor;
        }

        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblAdmPedidoProveedor pedidoProveedor)
        {
            db.tblAdmPedidoProveedor.Add(pedidoProveedor);
            await db.SaveChangesAsync();

            return Created(pedidoProveedor);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblAdmPedidoProveedor> pedidoProveedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.tblAdmPedidoProveedor.Include(x => x.tblAdmArticuloNAdmPedidoProveedor).FirstOrDefaultAsync(x => x.idAdmPedidoProveedor == key);

            if (entity == null)
            {
                return NotFound();
            }

            pedidoProveedor.ApplyTo(entity);
            return Updated(entity);
        }
    }
}
