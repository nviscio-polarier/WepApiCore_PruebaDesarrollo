
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;


namespace WebApiCore.Controllers
{
    public class tblSolicitudAbonoController : ODataController
    {

        private readonly bdERP db;

        public tblSolicitudAbonoController(bdERP context)
        {
            db = context;
        }

        [EnableQuery(MaxExpansionDepth = 6)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get([FromODataUri] int? idLavanderia)
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia);

            return Ok(db.tblSolicitudAbono.Where(x => idsEntidad.Contains(x.idEntidad)));
        }

        [EnableQuery]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblSolicitudAbono> solicitudAbono)
        {
            var entity = db.tblSolicitudAbono.Include(x => x.tblPrendaNSolicitudAbono).FirstOrDefault(x => x.idSolicitudAbono == key);

            if (entity == null)
                return NotFound();

            var operations_Imagen = solicitudAbono.Operations.Where(x => x.path == "/tblPrendaNSolicitudAbono").FirstOrDefault();
            if (operations_Imagen != null)
                db.tblPrendaNSolicitudAbono.RemoveRange(entity.tblPrendaNSolicitudAbono);

            solicitudAbono.ApplyTo(entity);

            await db.SaveChangesAsync();

            return Updated(entity);
        }


        [EnableQuery]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] tblSolicitudAbono solicitudAbono)
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            try
            {
                solicitudAbono.codigo = new Utils().GenerarCodigoAbono(db);
                solicitudAbono.fecha = DateTime.Now;
                solicitudAbono.idUsuario = idUsuario;

                db.tblSolicitudAbono.Add(solicitudAbono);

                await db.SaveChangesAsync();

                return Ok(solicitudAbono);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de BDD: " + ex.Message);
            }
        }

        [EnableQuery]
        [HttpDelete]
        [Authorize]
        public async Task<bool> Delete([FromODataUri] int key)
        {
            var entity = await db.tblSolicitudAbono.FindAsync(key);
            if (entity == null)
                return false;

            var prendasNSolicitud = db.tblPrendaNSolicitudAbono.Where(x => x.idSolicitudAbono == key);

            if (prendasNSolicitud.Any())
                db.tblPrendaNSolicitudAbono.RemoveRange(prendasNSolicitud);
            db.Remove(entity);

            await db.SaveChangesAsync();
            return true;
        }

    }
}





