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
    public class tblAdmProveedorController : ODataController
    {
        private readonly bdERP db;

        public tblAdmProveedorController(bdERP context)
        {
            db = context;
        }

        [EnableQuery(MaxNodeCount = 3000)]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmProveedor>> Get()
        {
            return db.tblAdmProveedor;
        }


        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblAdmProveedor> proveedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = db.tblAdmProveedor
                .Include(x => x.tblImagenNProveedor)
                .FirstOrDefault(x => x.idAdmProveedor == key);

            if (entity == null)
            {
                return NotFound();
            }

            var operations_Imagen = proveedor.Operations.Where(x => x.path == "/tblImagenNProveedor").FirstOrDefault();
            if (operations_Imagen != null)
            {
                db.tblImagenNProveedor.RemoveRange(entity.tblImagenNProveedor);
            }

            proveedor.ApplyTo(entity);

            await db.SaveChangesAsync();

            return Updated(entity);
        }
    }
}
