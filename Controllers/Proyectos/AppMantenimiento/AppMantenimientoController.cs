using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Enums.Assistant;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class AppMantenimientoController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> hubContext;
    private readonly tblMovimientoRecambioController mrc;

    public AppMantenimientoController(bdERP context, IHubContext<NotificacionesHub> _hubContext)
    {
        db = context;
        hubContext = _hubContext;
        mrc = new(context);
    }

    [HttpGet("odata/AppMantenimiento/almacenes")]
    [Authorize]
    public async Task<ActionResult> almacenes([FromODataUri] DateTimeOffset fecha, [FromODataUri] int? idParteTrabajo)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var usuario = db.tblUsuario
            .Where(x => x.idUsuario == idUsuario && !x.isEliminado)
            .Select(x => new tblUsuario { idPersona = x.idPersona, idCargo = x.idCargo, idLavanderia = x.idLavanderia })
            .FirstOrDefault();

        if (usuario == null)
        {
            return BadRequest();
        }

        bool isMasterDev = (short)idsCargo.Desarrollador == usuario.idCargo || (short)idsCargo.Master == usuario.idCargo;

        var alms = db.tblAlmacenRecambiosNPersona
            .Where(x =>
                x.idPersona == usuario.idPersona &&
                x.idAlmacenNavigation.idAlmacenPadre != null && //Solo hijos
                x.idAlmacenNavigation.activo == true && x.idAlmacenNavigation.eliminado == false
            )
            .Select(y => new { y.idAlmacen, y.idAlmacenNavigation.idAlmacenPadre })
            .ToList();

        var alms_padres = alms.Select(x => x.idAlmacenPadre).Distinct();
        var alms_hijos = alms.Select(x => x.idAlmacen);

        string idsAlmacen = string.Join('|', alms_hijos);

        tblParteTrabajoController ptc = new(db);

        var cantPrecioRecambio = await ptc.spGet_CantPrecio_ParteTrabajo(fecha, idsAlmacen, idParteTrabajo);

        var almacenesPadre = db.tblAlmacenRecambios
            .Where(ar =>
                ar.activo == true
                && ar.eliminado == false
                && ar.idAlmacenPadre == null
                && (
                    isMasterDev
                    || (ar.idAlmacenPadre != null || alms_padres.Contains(ar.idAlmacen))
                )
            )
            .ToList();

        var almacenesHijo = db.tblAlmacenRecambios
            .Include(ar => ar.tblAlmacenRecambiosNPersona)
            .Where(ar => ar.idAlmacenPadre != null && alms_hijos.Contains(ar.idAlmacen))
            .ToList();

        var result = almacenesPadre
            .Select(almP => new
            {
                almP.idAlmacen,
                almP.idPais,
                almP.denominacion,
                almacenesHijos = almacenesHijo
                    .Where(almH => almH.idAlmacenPadre == almP.idAlmacen)
                    .Select(almH => new
                    {
                        almH.idAlmacen,
                        almH.denominacion,
                        almH.tblAlmacenRecambiosNPersona.FirstOrDefault(arnp => arnp.idPersona == usuario.idPersona).isSalida,
                        tblRecambios = cantPrecioRecambio
                            .Where(cpr => cpr.idAlmacen == almH.idAlmacen)
                            .Join(db.tblRecambio, cpr => cpr.idRecambio, r => r.idRecambio, (cpr, r) => new
                            {
                                cpr.idRecambio,
                                cpr.max,
                                cpr.precio,
                                r.denominacion,
                                r.referencia,
                                r.referenciaInterna
                            })
                            .OrderByDescending(cpr => cpr.max)
                    })
                    .OrderByDescending(almH => almH.isSalida)
                    .ThenBy(almH => almH.denominacion)
            });

        return Ok(result);
    }

    [EnableQuery]
    [HttpGet("odata/AppMantenimiento/tecnicos")]
    [Authorize]
    public ActionResult tecnicos(int idLavanderia)
    {
        int idTipoTrabajoMantenimiento = 4;

        return Ok(db.tblPersona
        .Where(x =>
            x.activo == true
            && x.eliminado == false
            && (x.idTipoTrabajo == idTipoTrabajoMantenimiento || x.idCategoria == 4)
            && x.tblUsuario.First().idLavanderia.Where(x => x.idLavanderia.Equals(idLavanderia)).Count() > 0
        ).Select(x => new
        {
            x.idPersona,
            nombreCompleto = x.nombre + ' ' + x.apellidos
        }).ToList()
        .OrderBy(x => x.nombreCompleto)
        );
    }

    [EnableQuery]
    [HttpGet("odata/AppMantenimiento/recambiosTrasvase")]
    [Authorize]
    public async Task<ActionResult> recambiosTrasvase([FromODataUri] string campoBusqueda)
    {
        return await GetRecambiosTrasvase(null, null, campoBusqueda, DateTime.Now);
    }

    [EnableQuery]
    [HttpGet("odata/AppMantenimiento/GetRecambiosTrasvase")]
    [Authorize]
    public async Task<ActionResult> GetRecambiosTrasvase([FromODataUri] int? idAlmacenOrigen, [FromODataUri] int? idAlmacenDestino, [FromODataUri] string campoBusqueda, [FromODataUri] DateTimeOffset fecha)
    {
        var cantPrecioUbicacionRecambio = await mrc.spGet_CantPrecioUbicacion_MovimientoRecambio(idAlmacenOrigen, idAlmacenDestino, campoBusqueda, fecha, (int)idsTipoMovimientoRecambio.Traspaso, null, null);

        var tblAlmacenRecambios = db.tblAlmacenRecambios.Select(ar => new { ar.idAlmacen, ar.denominacion, ar.idPais }).ToList();
        var tblRecambio = db.tblRecambio
            .Where(r => cantPrecioUbicacionRecambio.Select(x => x.idRecambio).Contains(r.idRecambio))
            .ToList();

        var result = tblRecambio
            .Select(r => new
            {
                r.idRecambio,
                r.denominacion,
                r.referencia,
                r.referenciaInterna,
                tblRecambioNAlmacenRecambios = cantPrecioUbicacionRecambio
                    .Where(cpur => cpur.idRecambio == r.idRecambio)
                    .Select(cpur => new
                    {
                        cantidad = cpur.max,
                        precioMedioPonderado = cpur.precio,
                        cpur.ubicacion,
                        idAlmacenNavigation = tblAlmacenRecambios.FirstOrDefault(ar => ar.idAlmacen == cpur.idAlmacen)
                    })
            })
            .OrderBy(r => r.denominacion)
            .ToList();

        return Ok(result);
    }
}
