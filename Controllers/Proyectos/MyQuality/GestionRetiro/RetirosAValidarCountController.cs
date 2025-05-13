using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;

public class RetirosAValidarCountController : ODataController
{
    private readonly bdERP db;

    public RetirosAValidarCountController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyQuality/GestionRetiro/GetRetirosAValidarCount")]
    [Authorize]
    public async Task<ActionResult> GetRetirosAValidarCount([FromODataUri] int idLavanderia)
    {
        if (idLavanderia == null)
        {
            return BadRequest();
        }

        try
        {
            var retirosAValidarPorCompañia = db.tblGestionRetiro
                .Where(x => x.idLavanderia == idLavanderia && x.isValidado == false)
                .GroupBy(x => new { x.idCompañia, x.idEntidad, x.idGrupoPlantillaPrenda_generica })
                .Select(x => new
                {
                    idCompañia = x.Key.idCompañia,
                    idEntidad = x.Key.idEntidad,
                    idGrupoPlantillaPrenda_generica = x.Key.idGrupoPlantillaPrenda_generica,
                    count = x.Count()
                }).ToList();
            return Ok(retirosAValidarPorCompañia);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}
