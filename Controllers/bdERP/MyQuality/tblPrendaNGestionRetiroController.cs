using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPrendaNGestionRetiroController : ODataController
{
    private readonly bdERP db;
    public tblPrendaNGestionRetiroController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get(/*[FromODataUri] int idLavanderia*/)
    {
        return Ok(db.tblPrendaNGestionRetiro/*.Where(x => x.idMaquinaNavigation.idLavanderia == idLavanderia)*/);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblPrendaNGestionRetiro prendaNGestionRetiro)
    {
        try
        {
            db.tblPrendaNGestionRetiro.Add(prendaNGestionRetiro);
            await db.SaveChangesAsync();

            return Created(prendaNGestionRetiro);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int[] key, [FromBody] JsonPatchDocument<tblPrendaNGestionRetiro> prendaNGestionRetiro)
    {
        var entity = db.tblPrendaNGestionRetiro.FirstOrDefault(x => x.idGestionRetiro.Equals(key[0]) && x.idPrenda.Equals(key[1]) && x.idTipoRetiro.Equals(key[2]));
        if (entity == null)
            return BadRequest();

        prendaNGestionRetiro.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }
}
