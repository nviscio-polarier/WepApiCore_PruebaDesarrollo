using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;

public class tblEntidadCustomController : ODataController
{
    private readonly bdERP db;
    public tblEntidadCustomController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyQuality/GestionRetiro/GetEntidades")]
    [Authorize]
    public async Task<ActionResult> GetEntidades([FromODataUri] int idLavanderia)
    {
        if (idLavanderia == 14)
        {
            IQueryable<tblEntidad> entidades = Enumerable.Empty<tblEntidad>().AsQueryable();
            return Ok(entidades);
        }
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia);

        var compañiasVisibles = db.tblEntidad
                   .Where(x => idsEntidad.Contains(x.idEntidad))
                   .Select(x => new { x.idEntidad, x.denominacion })
                   .OrderBy(x => x.denominacion);

        return Ok(compañiasVisibles);
    }
}
