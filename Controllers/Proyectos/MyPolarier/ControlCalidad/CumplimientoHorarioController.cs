using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.bdERP.ControlCalidad;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class CumplimientoHorarioController : ODataController
{
    private readonly bdERP db;
    public CumplimientoHorarioController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/ControlCalidad/CumplimientoHorario/compañia")]
    [Authorize]
    public ActionResult compañia([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] int idCompañia, [FromODataUri] int idEntidad)
    {
        var detallado = getDetallado(idLavanderia, fechaIni, fechaFin, idEntidad, idCompañia).ToList();

        return Ok(
            detallado.GroupBy(x => new { x.idCompañia, x.denoCompa }).Select(x => new
            {
                id = "idCompañia_" + x.Key.idCompañia,
                denominacion = x.Key.denoCompa,
                totalEntregas = x.Count(),
                entregaHorarioCumplido = x.Sum(y => y.entregaHorarioCumplido),
                entregaFueraHora = x.Count() - x.Sum(y => y.entregaHorarioCumplido),
                cumplimiento = x.Sum(y => y.entregaHorarioCumplido) * 1.0 / x.Count()
            })
       );
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/ControlCalidad/CumplimientoHorario/entidad")]
    [Authorize]
    public ActionResult entidad([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] int idCompañia)
    {
        var detallado = getDetallado(idLavanderia, fechaIni, fechaFin, null, idCompañia).ToList();

        return Ok(
            detallado.GroupBy(x => new { x.idEntidad, x.denoEnti }).Select(x => new
            {
                id = "idEntidad_" + x.Key.idEntidad,
                denominacion = x.Key.denoEnti,
                totalEntregas = x.Count(),
                entregaHorarioCumplido = x.Sum(y => y.entregaHorarioCumplido),
                entregaFueraHora = x.Count() - x.Sum(y => y.entregaHorarioCumplido),
                cumplimiento = x.Sum(y => y.entregaHorarioCumplido) * 1.0 / x.Count()
            })
       );
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/ControlCalidad/CumplimientoHorario/detallado")]
    [Authorize]
    public ActionResult detallado([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] int idEntidad)
    {
        var detallado = getDetallado(idLavanderia, fechaIni, fechaFin, idEntidad, null).Select(x => new
        {
            id = "idEntidad_" + x.idEntidad,
            x.fechaLlegada,
            x.fechaSalida,
            x.horarioInicioReparto,
            x.horarioFinReparto,
            x.entregaHorarioCumplido,
            x.nombreTransportista
        }).ToList();

        return Ok(detallado);
    }

    private IQueryable<CumplimientoHorario> getDetallado(int idLavanderia, DateTime fechaIni, DateTime fechaFin, int? idEntidad, int? idCompañia)
    {
        bool isFiltroEntidad = idCompañia != null && idEntidad == null;
        bool isFiltroDetallado = idEntidad != null && idCompañia == null;

        return (from ppt in db.tblParadaNParteTransporte
                join ent in db.tblEntidad on ppt.idEntidad equals ent.idEntidad
                join comp in db.tblCompañia on ent.idCompañia equals comp.idCompañia
                where
                     ((isFiltroEntidad && comp.idCompañia == idCompañia) || !isFiltroEntidad)
                     && ((isFiltroDetallado && ent.idEntidad == idEntidad) || !isFiltroDetallado)
                    && ent.idLavanderia.Select(x => x.idLavanderia).Contains(idLavanderia)
                    && ppt.fechaLlegada.Value.Date >= fechaIni.Date && ppt.fechaSalida.Value.Date <= fechaFin.Date
                let src = (from hrp in db.tblHorarioRepartoNEntidad
                           where hrp.idEntidad == ent.idEntidad && hrp.fecha <= ppt.fechaLlegada.Value
                           orderby hrp.fecha descending
                           select new
                           {
                               hrp.fecha,
                               hrp.horarioInicioReparto,
                               hrp.horarioFinReparto
                           }).FirstOrDefault()
                select new CumplimientoHorario
                {
                    idCompañia = comp.idCompañia,
                    denoCompa = comp.denominacion,
                    idEntidad = ent.idEntidad,
                    denoEnti = ent.denominacion,
                    fechaLlegada = ppt.fechaLlegada.Value.LocalDateTime,
                    fechaSalida = ppt.fechaSalida.Value.LocalDateTime,
                    horarioInicioReparto = src.horarioInicioReparto,
                    horarioFinReparto = src.horarioFinReparto,
                    nombreTransportista = ppt.idParteTransporteNavigation.idUsuarioResponsableNavigation.idPersonaNavigation.nombre + ' ' +
                    ppt.idParteTransporteNavigation.idUsuarioResponsableNavigation.idPersonaNavigation.apellidos,
                    entregaHorarioCumplido = (
                         ppt.fechaLlegada.Value.LocalDateTime.TimeOfDay >= src.horarioInicioReparto && ppt.fechaLlegada.Value.LocalDateTime.TimeOfDay <= src.horarioFinReparto
                    ) ? 1 : 0
                });
    }
}
