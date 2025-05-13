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
    public class tblArticuloNAdmPedidoClienteController : ODataController
    {
        private readonly bdERP db;
        public tblArticuloNAdmPedidoClienteController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(db.tblArticuloNAdmPedidoCliente);
        }

        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblArticuloNAdmPedidoCliente articuloNAdmPedidoCliente)
        {
            db.tblArticuloNAdmPedidoCliente.Add(articuloNAdmPedidoCliente);
            await db.SaveChangesAsync();

            return Created(articuloNAdmPedidoCliente);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblArticuloNAdmPedidoCliente> articuloNAdmPedidoCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.tblArticuloNAdmPedidoCliente.FirstOrDefaultAsync(x => x.idArticuloNAdmPedidoCliente == key);

            if (entity == null)
            {
                return NotFound();
            }

            articuloNAdmPedidoCliente.ApplyTo(entity);
            await db.SaveChangesAsync();

            return Updated(entity);
        }
    }
}
