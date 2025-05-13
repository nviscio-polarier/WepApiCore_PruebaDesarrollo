using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblEntidadController : ODataController
{
    private readonly bdERP db;

    public tblEntidadController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int? idLavanderia, [FromODataUri] DateTime? fechaEntidadActiva)
    {
        if (idLavanderia == -1)
        {
            idLavanderia = null;
        }
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia);

        return Ok(db.tblEntidad.Where(x => idsEntidad.Contains(x.idEntidad) &&
       (idLavanderia == null || (idLavanderia != null && x.idLavanderia.Count(l => l.idLavanderia.Equals(idLavanderia)) > 0)) &&
        (fechaEntidadActiva == null || x.tblCalendarioEntidad.Where(c => c.fecha < fechaEntidadActiva).OrderByDescending(c => c.fecha).First().idEstado.Equals(1)) //Apertura
        ));
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(db.tblEntidad.Where(x => x.idEntidad == key));
    }
}
