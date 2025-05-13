using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblAjustePresupuestarioController : ODataController
{
    private readonly bdERP db;

    public tblAjustePresupuestarioController(bdERP context)
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
    public async Task<IQueryable<tblAjustePresupuestario>> Get()
    {
        return db.tblAjustePresupuestario;
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblAjustePresupuestario> ajuste)
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
        if (ajuste == null) return BadRequest();

        var entity = db.tblAjustePresupuestario
            .Include(x => x.tblMesNAjustePresupuestario)
            .FirstOrDefault(x => x.idAjustePresupuestario == key);
        if (entity == null) return NotFound();
        
        var tblMesNAjustePresupuestario = ajuste.Operations.FirstOrDefault(x => x.path == "/tblMesNAjustePresupuestario");
        if (tblMesNAjustePresupuestario != null)
        {
            db.tblMesNAjustePresupuestario.RemoveRange(entity.tblMesNAjustePresupuestario);
        }

        ajuste.ApplyTo(entity);
        await db.SaveChangesAsync();

        return Updated(entity);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblAjustePresupuestario ajuste)
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
        if (ajuste == null) return BadRequest();

        db.tblAjustePresupuestario.Add(ajuste);
        await db.SaveChangesAsync();

        return Created(ajuste);
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
        var entity = db.tblAjustePresupuestario
            .Include(x => x.tblMesNAjustePresupuestario)
            .FirstOrDefault(x => x.idAjustePresupuestario == key);
        if (entity == null) return NotFound();

        db.tblMesNAjustePresupuestario.RemoveRange(entity.tblMesNAjustePresupuestario);
        db.tblAjustePresupuestario.Remove(entity);
        await db.SaveChangesAsync();

        return NoContent();
    }
}

