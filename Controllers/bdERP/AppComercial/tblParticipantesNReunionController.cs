using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblParticipantesNReunionController : ODataController
{
    private readonly bdERP db;

    public tblParticipantesNReunionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblParticipantesNReunion>> Get()
    {
        return db.tblParticipantesNReunion;
    }

    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] tblParticipantesNReunion participante)
    {
        db.tblParticipantesNReunion.Add(participante);
        await db.SaveChangesAsync();

        return Created(participante);
    }


    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblParticipantesNReunion> participante)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblParticipantesNReunion.Where(x => x.idParticipanteNReunion.Equals(key)).FirstOrDefault();

        participante.ApplyTo(entity);

        await db.SaveChangesAsync();

        List<tblParticipantesNReunion> result = new List<tblParticipantesNReunion>();
        result.Add(entity);
        return Ok(result);
    }


    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblParticipantesNReunion.FindAsync(key);
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
