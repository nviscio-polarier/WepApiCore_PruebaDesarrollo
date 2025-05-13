using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

[Authorize]
public class tblKgLavadosLavadoraController : ODataController
{
    private readonly bdERP db;
    public tblKgLavadosLavadoraController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPatch]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblKgLavadosLavadora> kgLavadosLavadora)
    {
        var entity = await db.tblKgLavadosLavadora.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        kgLavadosLavadora.ApplyTo(entity);

        await db.SaveChangesAsync();
        return Updated(entity);
    }

    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] tblKgLavadosLavadora kgLavadosLavadora)
    {
        if (kgLavadosLavadora == null)
        {
            return BadRequest();
        }

        if (db.tblKgLavadosLavadora.Any(x =>
            x.idEntidad == kgLavadosLavadora.idEntidad &&
            x.idCompañia == kgLavadosLavadora.idCompañia &&
            x.idGrupoPlantillaPrenda_generica == kgLavadosLavadora.idGrupoPlantillaPrenda_generica &&
            x.idMaquina == kgLavadosLavadora.idMaquina &&
            x.fecha == kgLavadosLavadora.fecha
        ))
        {
            return BadRequest("Registro duplicado");
        }

        db.tblKgLavadosLavadora.Add(kgLavadosLavadora);
        await db.SaveChangesAsync();

        return Created(kgLavadosLavadora);
    }
}