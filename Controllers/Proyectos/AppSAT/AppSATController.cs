using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class AppSATController : ODataController
{
    private readonly bdERP db;

    public AppSATController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/AppSAT/almacenes")]
    [Authorize]
    public ActionResult Get()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario user = db.tblUsuario
                            .Where(x => x.idUsuario == idUsuario && !x.isEliminado)
                            .Select(x => new tblUsuario { idPersona = x.idPersona, idCargo = x.idCargo, idLavanderia = x.idLavanderia }).FirstOrDefault();
        bool isMasterDev = ((user.idCargo == 1 || user.idCargo == 2));

        var alms = db.tblAlmacenRecambiosNPersona.Where(x => x.idPersona == user.idPersona &&
         x.idAlmacenNavigation.idAlmacenPadre != null && //Solo hijos
         x.idAlmacenNavigation.activo == true && x.idAlmacenNavigation.eliminado == false)
             .Select(y => new { y.idAlmacen, y.idAlmacenNavigation.idAlmacenPadre }).ToList();

        var alms_padres = alms.Select(x => x.idAlmacenPadre);
        var alms_hijos = alms.Select(x => x.idAlmacen);

        return Ok(db.tblAlmacenRecambios
                    .Where(x =>
                        x.activo == true && x.eliminado == false && x.idAlmacenPadre == null &&
                        (!isMasterDev && (
                            (x.idAlmacenPadre == null && (alms_padres.Contains(x.idAlmacen)))
                            || x.idAlmacenPadre != null)
                        || isMasterDev)
                    )
                    .Select(
                        almPadre => new
                        {
                            idAlmacen = almPadre.idAlmacen,
                            denominacion = almPadre.denominacion,
                            almacenesHijos = db.tblAlmacenRecambios
                                .Where(x => x.idAlmacenPadre == almPadre.idAlmacen && alms_hijos.Contains(x.idAlmacen))
                                .Select(
                                    x => new
                                    {
                                        idAlmacen = x.idAlmacen,
                                        denominacion = x.denominacion,
                                        isSalida = x.tblAlmacenRecambiosNPersona.FirstOrDefault(x => x.idPersona.Equals(user.idPersona)).isSalida,
                                        tblRecambios = x.tblRecambioNAlmacenRecambios
                                            .Select(
                                                y => new
                                                {
                                                    idRecambio = y.idRecambio,
                                                    denominacion = y.idRecambioNavigation.denominacion,
                                                    referencia = y.idRecambioNavigation.referencia,
                                                    cantidad = y.cantidad
                                                }
                                            ).ToList()
                                    }).ToList()
                        }).ToList());
    }

    [EnableQuery]
    [HttpGet("odata/AppSAT/tecnicos")]
    [Authorize]
    public ActionResult Get(int idLavanderia)
    {
        return Ok(db.tblPersona
                    .Where(
                            x => x.activo == true && x.eliminado == false &&
                            (x.idCentroTrabajo == 2) &&
                            x.tblUsuario.First().idLavanderia.Where(x => x.idLavanderia.Equals(idLavanderia)).Count() > 0
                    ).Select(x => new
                    {
                        idPersona = x.idPersona,
                        nombreCompleto = x.nombre + ' ' + x.apellidos
                    }).ToList()
                    .OrderBy(x => x.nombreCompleto)
        );
    }
}
