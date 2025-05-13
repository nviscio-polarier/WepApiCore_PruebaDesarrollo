using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

[Authorize]
public class tblReunionController : ODataController
{
    private readonly bdERP db;

    public tblReunionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxExpansionDepth = 0)]
    [HttpGet]
    public async Task<IQueryable<tblReunion>> Get()
    {
        return db.tblReunion;
    }

    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] tblReunion reunion)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();

        if (objUsuario == null)
            return BadRequest();

        reunion.idUsuario = idUsuario;

        db.tblReunion.Add(reunion);

        await db.SaveChangesAsync();

        return Created(reunion);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblReunion> reunion)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();

        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblReunion
            .Include(r => r.tblParticipantesNReunion)
            .FirstOrDefault(x => x.idReunion.Equals(key));

        var operations_tblParticipantesNReunion = reunion.Operations.Where(x => x.path == "/tblParticipantesNReunion").FirstOrDefault();

        if (operations_tblParticipantesNReunion != null)
        {
            db.tblParticipantesNReunion.RemoveRange(entity.tblParticipantesNReunion);
        }

        reunion.ApplyTo(entity);

        await db.SaveChangesAsync();

        List<tblReunion> result = new List<tblReunion>();

        result.Add(entity);

        return Ok(result);
    }


    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblReunion.FindAsync(key);
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
