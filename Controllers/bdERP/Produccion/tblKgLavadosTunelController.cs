using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

[Authorize]
public class tblKgLavadosTunelController : ODataController
{
    private readonly bdERP db;
    public tblKgLavadosTunelController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPatch]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblKgLavadosTunel> kgLavadosTunel)
    {
        var entity = await db.tblKgLavadosTunel.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        kgLavadosTunel.ApplyTo(entity);
        
        await db.SaveChangesAsync();

        return Updated(entity);
    }

    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] tblKgLavadosTunel kgLavadosTunel)
    {
        if (kgLavadosTunel == null)
        {
            return BadRequest();
        }

        if (db.tblKgLavadosTunel.Any(x => 
            x.idEntidad == kgLavadosTunel.idEntidad &&
            x.idCompañia == kgLavadosTunel.idCompañia &&
            x.idGrupoPlantillaPrenda_generica == kgLavadosTunel.idGrupoPlantillaPrenda_generica &&
            x.idMaquina == kgLavadosTunel.idMaquina &&
            x.fecha == kgLavadosTunel.fecha
        ))
        {
            return BadRequest("Registro duplicado");
        }

        db.tblKgLavadosTunel.Add(kgLavadosTunel);
        await db.SaveChangesAsync();

        return Created(kgLavadosTunel);
    }
}