using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;

public class tblCompañiaCustomController : ODataController
{
    private readonly bdERP db;
    public tblCompañiaCustomController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyQuality/GestionRetiro/GetCompañias")]
    [Authorize]
    public async Task<ActionResult> GetCompañias([FromODataUri] int idLavanderia)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia);

        List<int> idsCompañiasVisibles = db.tblEntidad
                   .Where(x => idsEntidad.Contains(x.idEntidad))
                   .Select(x => x.idCompañia).ToList();

        var compañias = db.tblCompañia.Where(x => idsCompañiasVisibles.Contains(x.idCompañia)).Select(x => new { x.idCompañia, x.denominacion }).OrderBy(x => x.denominacion);
        if (idLavanderia == 14)
        {
            int[] idsCompañiasNSonCastello = {92, 110, 216, 107, 201, 88}; //Compañias solicitadas para Son Castelló
            return Ok(compañias.Where(x => idsCompañiasNSonCastello.Contains(x.idCompañia)));
        }

        return Ok(compañias);
    }
}
