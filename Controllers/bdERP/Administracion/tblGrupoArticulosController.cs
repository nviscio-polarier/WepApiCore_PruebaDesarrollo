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
    public class tblGrupoArticulosController : ODataController
    {
        private readonly bdERP db;
        public tblGrupoArticulosController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(db.tblGrupoArticulos);
        }

        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblGrupoArticulos grupoArticulos)
        {
            db.tblGrupoArticulos.Add(grupoArticulos);
            await db.SaveChangesAsync();

            return Created(grupoArticulos);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblGrupoArticulos> articuloLibre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.tblGrupoArticulos.FirstOrDefaultAsync(x => x.idGrupoArticulos == key);

            if (entity == null)
            {
                return NotFound();
            }

            articuloLibre.ApplyTo(entity);
            await db.SaveChangesAsync();

            return Updated(entity);
        }
    }
}
