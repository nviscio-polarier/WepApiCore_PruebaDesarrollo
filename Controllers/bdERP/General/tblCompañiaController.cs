using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCompañiaController : ODataController
{
    private readonly bdERP db;

    public tblCompañiaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int? idLavanderia, [FromODataUri] bool todas = false, [FromODataUri] bool showGenerico = false)
    {
        if (idLavanderia == -1)
        {
            idLavanderia = null;
        }
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        if (!todas)
        {
            List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia, showGenerico);

            List<int> idsCompañiasVisibles = db.tblEntidad
                       .Where(x => idsEntidad.Contains(x.idEntidad))
                       .Select(x => x.idCompañia).ToList();

            return Ok(db.tblCompañia.Where(x => idsCompañiasVisibles.Contains(x.idCompañia)));
        }
        else
        {
            if (idLavanderia == null)
                return Ok(db.tblCompañia);

            return Ok(db.tblCompañia
                .Join(db.tblEntidad, c => c.idCompañia, ent => ent.idCompañia, (c, ent) => new { c = c, ent = ent }).Select(x => x)
                .Where(x =>
                       ((x.c.tblEntidad.Count > 0 && x.ent.idLavanderia.Contains(db.tblLavanderia.Where(l => l.idLavanderia == idLavanderia).FirstOrDefault())) ||
                        x.c.idLavanderia == idLavanderia)
                      )
                .Select(x => x.c)
                .Distinct()
                .ToList()
            );
        }
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(db.tblCompañia.Where(x => x.idCompañia == key));
    }
}
