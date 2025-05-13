using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.bdERP.Incidencias;
using WebApiCore.Context;

namespace WebApiCore.Controllers;

public class TipoIncidenciaNLavanderiaController : ODataController
{
    private readonly bdERP db;

    public TipoIncidenciaNLavanderiaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        return Ok(db.tblTipoIncidencia.SelectMany(x => db.tblLavanderia.Where(l => l.idUsuario.Select(u => u.idUsuario).Contains(idUsuario)).Select(y => new TipoIncidenciaNLavanderia
        {
            idLavanderia = y.idLavanderia,
            denominacion = y.denominacion,
            idTipoIncidencia = x.idTipoIncidencia,
            icon = x.icon,
            numIncidencias = y.tblIncidencia.Where(z => z.idSubTipoIncidenciaNavigation.idTipoIncidencia == x.idTipoIncidencia && z.idLavanderia == y.idLavanderia && z.estado == false).Count()
        })));
    }
}
