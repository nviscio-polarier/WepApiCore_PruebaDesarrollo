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
    public class tblArticuloMaquinariaController : ODataController
    {
        private readonly bdERP db;
        public tblArticuloMaquinariaController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return Ok(db.tblArticuloMaquinaria);
        }

        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblArticuloMaquinaria articuloMaquinaria)
        {
            db.tblArticuloMaquinaria.Add(articuloMaquinaria);
            await db.SaveChangesAsync();

            return Created(articuloMaquinaria);
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblArticuloMaquinaria> articuloMaquinaria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.tblArticuloMaquinaria.FirstOrDefaultAsync(x => x.idArticuloMaquinaria == key);

            if (entity == null)
            {
                return NotFound();
            }

            articuloMaquinaria.ApplyTo(entity);
            await db.SaveChangesAsync();

            return Updated(entity);
        }
    }
}
