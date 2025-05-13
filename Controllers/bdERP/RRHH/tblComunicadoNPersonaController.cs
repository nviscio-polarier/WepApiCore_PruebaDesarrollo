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
    public class tblComunicadoNPersonaController : ODataController
    {
        private readonly bdERP db;

        public tblComunicadoNPersonaController(bdERP context)
        {
            db = context;
        }

        


        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get([FromODataUri] object keyidPersona, [FromODataUri] int keyidComunicado)
        {
            try
            {

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [EnableQuery]
        [HttpPatch]
        public async Task<ActionResult> Patch([FromODataUri] int keyidPersona, [FromODataUri] int keyidComunicado, [FromBody] JsonPatchDocument<tblComunicadoNPersona> comunicadoNPersona)
        {

            var entity = db.tblComunicadoNPersona.Where(x => x.idComunicado.Equals(keyidComunicado)
                                                                && x.idPersona.Equals(keyidPersona)
                                                                ).FirstOrDefault();

            comunicadoNPersona.ApplyTo(entity);

            await db.SaveChangesAsync();
            return Ok();
        }
    }
}