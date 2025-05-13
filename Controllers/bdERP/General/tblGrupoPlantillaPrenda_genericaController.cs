using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblGrupoPlantillaPrenda_genericaController : ODataController
{
    private readonly bdERP db;

    public tblGrupoPlantillaPrenda_genericaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblGrupoPlantillaPrenda_generica>> Get([FromODataUri] int idLavanderia, [FromODataUri] bool todos = false)
    {
        return db.tblGrupoPlantillaPrenda_generica.Where(grupo => todos == true ||
            grupo.tblPlantillaPrenda_generica.Any(plantilla => 
                plantilla.tblPrenda.Any(pre => 
                    pre.idCompañiaNavigation.tblEntidad.Any(entidad => 
                        entidad.idLavanderia.Any(entNLav => entNLav.idLavanderia == idLavanderia)
                    )
                )
            )
        );
    }

    [EnableQuery]
    [HttpGet("odata/tblGrupoPlantillaPrenda_generica({key})")]
    [Authorize]
    public async Task<tblGrupoPlantillaPrenda_generica> Get([FromODataUri] int key)
    {
        return db.tblGrupoPlantillaPrenda_generica.FirstOrDefault(x => x.idGrupoPlantillaPrenda_generica == key);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] tblGrupoPlantillaPrenda_generica entity)
    {
        if (entity == null)
        {
            return BadRequest();
        }

        db.tblGrupoPlantillaPrenda_generica.Add(entity);
        await db.SaveChangesAsync();
        return Created(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete([FromODataUri] byte key)
    {
        var entity = await db.tblGrupoPlantillaPrenda_generica.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblGrupoPlantillaPrenda_generica.Remove(entity);
        await db.SaveChangesAsync();
        return NoContent();
    }

}
