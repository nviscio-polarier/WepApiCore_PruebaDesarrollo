using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCarpetaDocumentosController : ODataController
{
    private readonly bdERP db;

    public tblCarpetaDocumentosController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] bool todos = false)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        if (todos)
        {
            if (!objUsuario.enableDatosRRHH) //No tienes permisos para acceder a los datos de todos los usuarios
            {
                return BadRequest();
            }

            return Ok(db.tblCarpetaDocumentos);
        }
        else
        {
            return Ok(db.tblCarpetaDocumentos);
        }
    }
}
