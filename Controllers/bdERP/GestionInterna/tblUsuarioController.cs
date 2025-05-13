using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblUsuarioController : ODataController
{
    private readonly bdERP db;

    public tblUsuarioController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxAnyAllExpressionDepth = 5)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<tblUsuario>> Get()
    {
        var usuario = (from usu in db.tblUsuario
                       select new tblUsuario
                       {
                           idUsuario = usu.idUsuario,
                           usuario = usu.usuario,
                           password = "",
                           cambiaPassword = usu.cambiaPassword,
                           nombre = usu.nombre,
                           email = usu.email,
                           idIdioma = usu.idIdioma,
                           idCargo = usu.idCargo,
                           detallesCargo = usu.detallesCargo,
                           refactura = usu.refactura,
                           idFormularioInicio = usu.idFormularioInicio,
                           idLavanderiaInicio = usu.idLavanderiaInicio,
                           idLocalizacion = usu.idLocalizacion,
                           idTipoUsuario = usu.idTipoUsuario,
                           idCompañia = usu.idCompañia,
                           idPersona = usu.idPersona,
                           enableDatosRRHH = usu.enableDatosRRHH,
                           fechaCreacion = usu.fechaCreacion,
                           subtipoUsuario = usu.subtipoUsuario,
                           tblFormularioNUsuario = usu.tblFormularioNUsuario,
                           idEntidad = usu.idEntidad,
                           tblPrendaNUsuarioNEntidad = usu.tblPrendaNUsuarioNEntidad
                       }).ToList();
        return Ok(usuario);
    }

    [EnableQuery]
    [HttpGet("odata/tblUsuario/GetByEmail")]
    [Authorize]
    public async Task<ActionResult<tblUsuario>> GetByEmail(string email)
    {
        var entity = db.tblUsuario.Where(y => y.email == email && !y.isEliminado);
        bool usuario_tienePersona = db.tblUsuario.Where(y => y.email == email && y.idPersona != null).FirstOrDefault() != null;

        return Ok(entity.Select(x => new
        {
            x.nombre,
            usuario_tienePersona = usuario_tienePersona
        }));
    }

    [EnableQuery]
    [HttpGet("odata/tblUsuario/GetIdUsuario")]
    [Authorize]
    public async Task<ActionResult<tblUsuario>> GetIdUsuario()
    {
        int? idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
        if (objUsuario == null)
        {
            return BadRequest();
        }
        return Ok(idUsuario);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblUsuario> usuario)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblUsuario.Where(x => x.idUsuario.Equals(key)).FirstOrDefault();

        usuario.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(objUsuario.enableDatosRRHH ? entity : true);
    }
}
