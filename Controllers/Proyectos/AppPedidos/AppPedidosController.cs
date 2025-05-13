using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class;
using WebApiCore.Context;
using AuthorizeAttribute = WebApiCore.Security.AuthorizeAttribute;

namespace WebApiCore.Controllers;
public class AppPedidosController : ODataController
{
    private readonly bdERP db;
    public AppPedidosController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/AppPedidos/entidades")]
    [Authorize]
    public ActionResult Get()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        bool tieneAcceso = db.tblFormularioNUsuario.Where(x => x.idUsuario.Equals(idUsuario) && x.idFormulario.Equals(30)).ToList().Count() > 0;
        if (tieneAcceso)
        {
            List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, null);
            return Ok(db.tblEntidad.Where(x => idsEntidad.Contains(x.idEntidad))
                .Select(x => new
                {
                    x.idEntidad,
                    x.denominacion,
                    x.enableObservacionesPedido
                })
                .OrderBy(x => x.denominacion));
        }
        return BadRequest();
    }
}