using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class tblPrendasHoraController : ODataController
{
    private readonly bdERP db;
    public tblPrendasHoraController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int idLavanderia)
    {
        return Ok(db.tblPrendasHora.Where(x => x.idMaquinaNavigation.idLavanderia == idLavanderia));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblPrendasHora prendasHora)
    {
        try
        {
            db.tblPrendasHora.Add(prendasHora);
            await db.SaveChangesAsync();

            return Created(prendasHora);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPrendasHora> prendasHora)
    {
        var entity = db.tblPrendasHora.FirstOrDefault(x => x.idPrendasHora.Equals(key));
        if (entity == null)
            return BadRequest();

        prendasHora.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }


    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblPrendasHora.FindAsync(key);
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
