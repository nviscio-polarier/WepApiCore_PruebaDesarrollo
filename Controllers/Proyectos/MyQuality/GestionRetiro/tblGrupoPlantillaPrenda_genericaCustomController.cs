using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;

public class tblGrupoPlantillaPrenda_genericaCustomController : ODataController
{
    private readonly bdERP db;

    public tblGrupoPlantillaPrenda_genericaCustomController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyQuality/GestionRetiro/GetGrupoPlantillaPrenda")]
    [Authorize]
    public async Task<ActionResult> GetGrupoPlantillaPrenda([FromODataUri] int idLavanderia)
    {
        var idsPrendasVisibles = Utils.selectPrendasGenericasVisibles(db, idLavanderia);
        var idsGruposVisibles = db.tblPlantillaPrenda_generica
            .Where(x => idsPrendasVisibles.Contains(x.idPlantillaPrenda_generica))
            .Select(x => x.idGrupoPlantillaPrenda_generica)
            .Distinct();
        var result = db.tblGrupoPlantillaPrenda_generica
            .Where(x => idsGruposVisibles.Contains(x.idGrupoPlantillaPrenda_generica))
            .OrderBy(x => x.denominacion);
        return Ok(result);
    }
}
