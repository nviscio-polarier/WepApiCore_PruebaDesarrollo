using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPrendaNEntidad_NuevoPedidoController : ODataController
{
    private readonly bdERP db;
    public tblPrendaNEntidad_NuevoPedidoController(bdERP context)
    {
        db = context;
    }

    //Eliminar pedidos 1.0.11 a pasado a pedidosController(en apps cambiar nombre a proyectos)
    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int idEntidad)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objEntidad = db.tblEntidad
            .Where(x => x.idEntidad.Equals(idEntidad))
            .Select(x => new
            {
                x.idCompañia,
                x.isTodasPrendaNNuevoPedido_compañia,
                x.isTodasPrendaNNuevoPedido_entidad,
                idPrendaNavigation = x.tblPrendaNEntidad_NuevoPedido.Select(x => x.idPrendaNavigation),
                tblPrendaNEntidad_NuevoPedido = x.tblPrendaNEntidad_NuevoPedido,
            }).FirstOrDefault();

        if (objEntidad == null) return Ok(Array.Empty<tblPrenda>().ToList());

        List<int> tblPrendaNUsuarioNEntidad_idPrendaFiltrados = db.tblPrendaNUsuarioNEntidad.Where(x => x.idUsuario == idUsuario && x.idEntidad == idEntidad).Select(x => x.idPrenda).ToList();

        List<tblPrenda> idsPrendaNEntidad_NuevoPedido = new List<tblPrenda>();
        idsPrendaNEntidad_NuevoPedido.AddRange(objEntidad.tblPrendaNEntidad_NuevoPedido.Select(x => x.idPrendaNavigation));

        var prendasEntidad = idsPrendaNEntidad_NuevoPedido.Where(y => y.idEntidad == idEntidad).Select(y => y.idPrenda).ToList();
        var prendasCompañia = idsPrendaNEntidad_NuevoPedido.Where(y => y.idCompañia == objEntidad.idCompañia).Select(y => y.idPrenda).ToList();

        return Ok(db.tblPrenda.Where(x =>
        (
            (tblPrendaNUsuarioNEntidad_idPrendaFiltrados.Count() > 0 && tblPrendaNUsuarioNEntidad_idPrendaFiltrados.Contains(x.idPrenda)) ||
            tblPrendaNUsuarioNEntidad_idPrendaFiltrados.Count() == 0
        ) &&
        x.activo.Equals(true) &&
        x.eliminado.Equals(false)
         && ((objEntidad.isTodasPrendaNNuevoPedido_compañia == true && x.idCompañia == objEntidad.idCompañia) ||
             (objEntidad.isTodasPrendaNNuevoPedido_entidad == true && x.idEntidad == idEntidad) ||
             (objEntidad.isTodasPrendaNNuevoPedido_entidad == null && prendasEntidad.Contains(x.idPrenda)) ||
             (objEntidad.isTodasPrendaNNuevoPedido_compañia == null && prendasCompañia.Contains(x.idPrenda))
        )));
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int keyidEntidad, [FromODataUri] int keyidPrenda,
        [FromBody] JsonPatchDocument<tblPrendaNEntidad_NuevoPedido> prendaNEntidad_NuevoPedido)
    {
        var entity = db.tblPrendaNEntidad_NuevoPedido.FirstOrDefault(x => x.idPrenda == keyidPrenda && x.idEntidad == keyidEntidad);

        if (entity != null)
        {
            prendaNEntidad_NuevoPedido.ApplyTo(entity);
            await db.SaveChangesAsync();
        }
        else
        {
            return BadRequest();
        }


        return Ok(true);
    }

}

