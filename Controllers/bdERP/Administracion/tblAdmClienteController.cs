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
    public class tblAdmClienteController : ODataController
    {
        private readonly bdERP db;

        public tblAdmClienteController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmCliente>> Get()
        {
            return db.tblAdmCliente;
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblAdmCliente> cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = db.tblAdmCliente
                .Include(x => x.tblImagenNCliente)
                .FirstOrDefault(x => x.idAdmCliente == key);

            if (entity == null)
            {
                return NotFound();
            }

            var operations_Imagen = cliente.Operations.Where(x => x.path == "/tblImagenNCliente").FirstOrDefault();
            if (operations_Imagen != null)
            {
                db.tblImagenNCliente.RemoveRange(entity.tblImagenNCliente);
            }

            cliente.ApplyTo(entity);

            await db.SaveChangesAsync();

            return Updated(entity);
        }
    }
}
