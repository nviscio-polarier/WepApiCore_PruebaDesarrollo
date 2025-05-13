using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblAlmacenRecambiosController : ODataController
{
    private readonly bdERP db;

    public tblAlmacenRecambiosController(bdERP context)
    {
        db = context;
    }

    [HttpGet]
    [EnableQuery]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int? idAlmacen)
    {
        var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var usuario = db.tblUsuario.FirstOrDefault(u => u.idUsuario == idUsuario);

        List<short> idsCargoCustom = new()
        {
            (short)idsCargo.Desarrollador,
            (short)idsCargo.Master
        };

        if (usuario?.idPersona == null)
        {
            return BadRequest("El usuario no tiene una persona asociada.");
        }

        var idsAlmacenRecambio = db.tblAlmacenRecambiosNPersona.Where(arnp => arnp.idPersona == usuario.idPersona).Select(arnp => arnp.idAlmacen);

      var almacenes =  db.tblAlmacenRecambios.Where(ar =>
        (
            idsCargoCustom.Contains(usuario.idCargo) || idsAlmacenRecambio.Contains(ar.idAlmacen)
        ) && (idAlmacen == null || ar.idAlmacen == idAlmacen) && ar.activo == true && !ar.eliminado).ToList();

        return Ok(almacenes);
    }

    [HttpGet]
    [EnableQuery]
    [Authorize]
    public ActionResult Get([FromODataUri] int key)
    {
        var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var usuario = db.tblUsuario.FirstOrDefault(u => u.idUsuario == idUsuario);

        if (usuario?.idPersona == null)
        {
            return BadRequest("El usuario no tiene una persona asociada.");
        }

        List<short> idsCargoCustom = new()
        {
            (short)idsCargo.Desarrollador,
            (short)idsCargo.Master
        };

        if (!idsCargoCustom.Contains(usuario.idCargo))
        {
            var idsAlmacenRecambio = db.tblAlmacenRecambiosNPersona.Where(arnp => arnp.idPersona == usuario.idPersona && arnp.idAlmacenNavigation.activo == true && !arnp.idAlmacenNavigation.eliminado).Select(arnp => arnp.idAlmacen);

            if (!idsAlmacenRecambio.Contains(key))
            {
                return BadRequest("El usuario no tiene acceso a este almacén.");
            }
        }

        return Ok(db.tblAlmacenRecambios.Where(ar => ar.idAlmacen == key));
    }
}
