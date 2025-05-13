using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.IdentityModel.Tokens;
using WebApiCore.Context;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Security;

namespace WebApiCore.Controllers
{
    public class tblAdmElementoPEPController : ODataController
    {
        private readonly bdERP db;

        public tblAdmElementoPEPController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get(bool? todas = true)
        {
            if (todas == true)
            {
                return Ok(db.tblAdmElementoPEP);
            }
            else
            {
                var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
                var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
                if (objUsuario == null)
                {
                    return BadRequest();
                }

                //Revisa si el usuario tiene permisos para ver todos los elementos PEP (tblPermisoNUsuario)
                var permisoConcedido = !db.tblUsuario.Where(x =>
                    x.idUsuario == idUsuario &&
                    x.idPermiso.Any(x => x.codigo == idsPermiso.LecturaTotalControlPresupuestario)
                    ).IsNullOrEmpty();

                var idsElementoPEPVisibles = db.tblAdmElementoPEP
                    .Where(x =>
                    permisoConcedido
                    || objUsuario.isDepartamentoControl == true
                    || objUsuario.idCargo == 1 // Desarrollador
                    || x.idUsuario.Any(y => y.idUsuario == idUsuario)) // Revisa si el usuario tiene permiso para ver el elemento PEP en especifico (tblAdmElementoPEPNUsuario)
                    .Select(x => x.idAdmElementoPEP)
                    .ToList();

                return Ok(db.tblAdmElementoPEP.Where(x => 
                    idsElementoPEPVisibles.Contains(x.idAdmElementoPEP) ||
                    (x.idAdmElementoPEPPadre != null && idsElementoPEPVisibles.Contains((int)x.idAdmElementoPEPPadre)))
                );
            }
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IQueryable<tblAdmElementoPEP>> Get([FromODataUri] int key)
        {
            return db.tblAdmElementoPEP.Where(x => x.idAdmElementoPEP == key);
        }
    }
}
