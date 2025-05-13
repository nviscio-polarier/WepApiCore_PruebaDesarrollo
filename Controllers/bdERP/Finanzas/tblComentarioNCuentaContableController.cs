using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using WebApiCore.Context;
using WebApiCore.Controllers.auth;
using WebApiCore.Hubs;
using AuthorizeAttribute = WebApiCore.Security.AuthorizeAttribute;

namespace WebApiCore.Controllers;
public class tblComentarioNCuentaContableController : ODataController
{
    private readonly bdERP db;
    public tblComentarioNCuentaContableController(bdERP context)
    {
        db = context;
    }

    private bool userHasAccess(string codigo)
    {
        var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
        if (objUsuario == null)
        {
            return false;
        }
        var permisoConcedido = !db.tblUsuario.Where(x => x.idUsuario == idUsuario && x.idPermiso.Any(y => y.codigo == codigo)).IsNullOrEmpty(); //Revisa si el usuario tiene el permiso asociado a su cuenta (tblPermisoNUsuario)
        return permisoConcedido || objUsuario.idCargo == 1; // Desarrollador
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblComentarioNCuentaContable>> Get()
    {
        return db.tblComentarioNCuentaContable;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(db.tblComentarioNCuentaContable.Where(x => x.idComentarioNCuentaContable == key));
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblComentarioNCuentaContable> comentarioNCuentaContable)
    {
        var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
        if (objUsuario == null)
        {
            return BadRequest();
        }
        if (!objUsuario.isDepartamentoControl && !userHasAccess("escrituraControlPresupuestario"))
        {
            return BadRequest();
        }
        var entity = await db.tblComentarioNCuentaContable.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        comentarioNCuentaContable.ApplyTo(entity);

        await db.SaveChangesAsync();
        return Ok(true);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblComentarioNCuentaContable tblComentarioNCuentaContable)
    {
        var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
        if (objUsuario == null)
        {
            return BadRequest();
        }
        if (!objUsuario.isDepartamentoControl && !userHasAccess("escrituraControlPresupuestario"))
        {
            return BadRequest();
        }

        tblComentarioNCuentaContable.idUsuario = idUsuario;
        tblComentarioNCuentaContable.fecha = DateTime.Now;

        db.tblComentarioNCuentaContable.Add(tblComentarioNCuentaContable);
        await db.SaveChangesAsync();
        return Ok(tblComentarioNCuentaContable);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
        if (objUsuario == null)
        {
            return BadRequest();
        }
        if (!objUsuario.isDepartamentoControl && !userHasAccess("escrituraControlPresupuestario"))
        {
            return BadRequest();
        }
        var entity = await db.tblComentarioNCuentaContable.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblComentarioNCuentaContable.Remove(entity);
        await db.SaveChangesAsync();
        return Ok(true);
    }
}