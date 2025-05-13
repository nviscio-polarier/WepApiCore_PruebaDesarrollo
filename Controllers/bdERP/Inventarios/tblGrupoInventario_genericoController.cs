using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;
public class tblGrupoInventario_genericoController : ODataController
{
    private readonly bdERP db;
    public tblGrupoInventario_genericoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblGrupoInventario_generico);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblGrupoInventario_generico> grupoInventario_generico)
    {
        if (grupoInventario_generico == null) return BadRequest();

        var entity = await db.tblGrupoInventario_generico.FindAsync(key);
        if (entity == null)
            return NotFound();

        grupoInventario_generico.ApplyTo(entity);
        await db.SaveChangesAsync();
        return Updated(entity);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblGrupoInventario_generico grupoInventario_generico)
    {
        if (grupoInventario_generico == null) return BadRequest();

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        grupoInventario_generico.idUsuario = idUsuario;
        grupoInventario_generico.fechaReg = DateTime.UtcNow;

        db.tblGrupoInventario_generico.Add(grupoInventario_generico);

        await db.SaveChangesAsync();
        return Created(grupoInventario_generico);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var entity = await db.tblGrupoInventario_generico
            .Include(x => x.idInventario)
            .FirstOrDefaultAsync(x => x.idGrupoInventario_generico == key);
        if (entity == null)
            return NotFound();

        entity.idInventario.Clear();

        db.tblGrupoInventario_generico.Remove(entity);
        await db.SaveChangesAsync();
        return NoContent();
    }
}