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
    public class tblAdmPedidoClienteController : ODataController
    {
        private readonly bdERP db;
        public tblAdmPedidoClienteController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmPedidoCliente>> Get()
        {
            return db.tblAdmPedidoCliente;
        }

        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblAdmPedidoCliente pedidoCliente)
        {
            db.tblAdmPedidoCliente.Add(pedidoCliente);
            await db.SaveChangesAsync();

            return Created(pedidoCliente);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblAdmPedidoCliente> pedidoCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.tblAdmPedidoCliente.Include(x => x.tblAdmArticuloNAdmPedidoCliente).FirstOrDefaultAsync(x => x.idAdmPedidoCliente == key);

            if (entity == null)
            {
                return NotFound();
            }

            pedidoCliente.ApplyTo(entity);
            return Updated(entity);
        }
    }
}
