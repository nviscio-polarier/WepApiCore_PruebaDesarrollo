using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class ClasificacionSucioController : ODataController
{
    private readonly bdERP db;
    public ClasificacionSucioController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/ControlCalidad/ClasificacionSucio/compañia")]
    [Authorize]
    public ActionResult compañia([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin)
    {
        var compañia = (from mel in db.tblMovimientoElemLog
                        join cmel in db.tblCantidadNMovimientoElemLog on mel.idMovimientoElemLog equals cmel.idMovimientoElemLog
                        join ent in db.tblEntidad on mel.idEntidad equals ent.idEntidad
                        join comp in db.tblCompañia on ent.idCompañia equals comp.idCompañia
                        where mel.idLavanderia == idLavanderia && mel.fecha.Date >= fechaIni.Date && mel.fecha.Date <= fechaFin.Date
                        group cmel by new { comp.idCompañia, comp.denominacion } into grp
                        select new
                        {
                            id = "idCompañia_" + grp.Key.idCompañia,
                            grp.Key.denominacion,
                            carrosContados = grp.Sum(x => x.idTipoElemLog == 5 ? x.cantidad : 0),
                            carrosErroneos = grp.Sum(x => x.isMezcla == true && x.idTipoElemLog == 5 ? x.cantidad : 0),
                            carrosCorrectos = grp.Sum(x => x.isMezcla == false && x.idTipoElemLog == 5 ? x.cantidad : 0),
                            sacasContadas = grp.Sum(x => x.idTipoElemLog == 4 ? x.cantidad : 0),
                            sacasErroneas = grp.Sum(x => x.isMezcla == true && x.idTipoElemLog == 4 ? x.cantidad : 0),
                            sacasCorrectas = grp.Sum(x => x.isMezcla == false && x.idTipoElemLog == 4 ? x.cantidad : 0),
                            cumplimientoSacas = grp.Sum(x => x.idTipoElemLog == 4 ? x.cantidad : 0) == 0 ? 0
                                : (grp.Sum(x => (x.isMezcla == false && x.idTipoElemLog == 4) ? x.cantidad : 0)) / (1.0 * grp.Sum(x => x.idTipoElemLog == 4 ? x.cantidad : 0)),
                            cumplimientoCarros = grp.Sum(x => x.idTipoElemLog == 5 ? x.cantidad : 0) == 0 ? 0
                                : (grp.Sum(x => (x.isMezcla == false && x.idTipoElemLog == 5) ? x.cantidad : 0)) / (1.0 * grp.Sum(x => x.idTipoElemLog == 5 ? x.cantidad : 0))
                        }).ToList();

        return Ok(compañia);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/ControlCalidad/ClasificacionSucio/entidad")]
    [Authorize]
    public ActionResult entidad([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] int idCompañia)
    {
        var entidad = (from mel in db.tblMovimientoElemLog
                       join cmel in db.tblCantidadNMovimientoElemLog on mel.idMovimientoElemLog equals cmel.idMovimientoElemLog
                       join ent in db.tblEntidad on mel.idEntidad equals ent.idEntidad
                       where mel.idLavanderia == idLavanderia && mel.fecha.Date >= fechaIni.Date && mel.fecha.Date <= fechaFin.Date && ent.idCompañia == idCompañia
                       group cmel by new { ent.idEntidad, ent.denominacion } into grp
                       select new
                       {
                           id = "idEntidad_" + grp.Key.idEntidad,
                           grp.Key.denominacion,
                           carrosContados = grp.Sum(x => x.idTipoElemLog == 5 ? x.cantidad : 0),
                           carrosErroneos = grp.Sum(x => x.isMezcla == true && x.idTipoElemLog == 5 ? x.cantidad : 0),
                           carrosCorrectos = grp.Sum(x => x.isMezcla == false && x.idTipoElemLog == 5 ? x.cantidad : 0),
                           sacasContadas = grp.Sum(x => x.idTipoElemLog == 4 ? x.cantidad : 0),
                           sacasErroneas = grp.Sum(x => x.isMezcla == true && x.idTipoElemLog == 4 ? x.cantidad : 0),
                           sacasCorrectas = grp.Sum(x => x.isMezcla == false && x.idTipoElemLog == 4 ? x.cantidad : 0),
                           cumplimientoSacas = grp.Sum(x => x.idTipoElemLog == 4 ? x.cantidad : 0) == 0 ? 0 :
                            (grp.Sum(x => x.isMezcla == false && x.idTipoElemLog == 4 ? x.cantidad : 0)) / (1.0 * grp.Sum(x => x.idTipoElemLog == 4 ? x.cantidad : 0)),
                           cumplimientoCarros = grp.Sum(x => x.idTipoElemLog == 5 ? x.cantidad : 0) == 0 ? 0 :
                           (grp.Sum(x => x.isMezcla == false && x.idTipoElemLog == 5 ? x.cantidad : 0)) / (1.0 * grp.Sum(x => x.idTipoElemLog == 5 ? x.cantidad : 0))
                       }).ToList();

        return Ok(entidad);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/ControlCalidad/ClasificacionSucio/detallado")]
    [Authorize]
    public ActionResult detallado([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] int idEntidad)
    {
        var detallado = (from mel in db.tblMovimientoElemLog
                         join cmel in db.tblCantidadNMovimientoElemLog on mel.idMovimientoElemLog equals cmel.idMovimientoElemLog
                         join ent in db.tblEntidad on mel.idEntidad equals ent.idEntidad
                         where mel.idLavanderia == idLavanderia
                            && mel.fecha.Date >= fechaIni.Date
                            && mel.fecha.Date <= fechaFin.Date
                            && ent.idEntidad == idEntidad
                         group cmel by new { ent.idEntidad, ent.denominacion, fecha = mel.fecha.Date } into grp
                         select new
                         {
                             id = "idEntidad_" + grp.Key.idEntidad,
                             grp.Key.denominacion,
                             grp.Key.fecha,
                             carrosContados = grp.Sum(x => x.idTipoElemLog == 5 ? x.cantidad : 0),
                             carrosErroneos = grp.Sum(x => x.isMezcla == true && x.idTipoElemLog == 5 ? x.cantidad : 0),
                             carrosCorrectos = grp.Sum(x => x.isMezcla == false && x.idTipoElemLog == 5 ? x.cantidad : 0),
                             sacasContadas = grp.Sum(x => x.idTipoElemLog == 4 ? x.cantidad : 0),
                             sacasErroneas = grp.Sum(x => x.isMezcla == true && x.idTipoElemLog == 4 ? x.cantidad : 0),
                             sacasCorrectas = grp.Sum(x => x.isMezcla == false && x.idTipoElemLog == 4 ? x.cantidad : 0),
                             cumplimientoSacas = grp.Sum(x => x.idTipoElemLog == 4 ? x.cantidad : 0) == 0 ? 0
                             : (grp.Sum(x => x.isMezcla == false && x.idTipoElemLog == 4 ? x.cantidad : 0)) / (1.0 * grp.Sum(x => x.idTipoElemLog == 4 ? x.cantidad : 0)),
                             cumplimientoCarros = grp.Sum(x => x.idTipoElemLog == 5 ? x.cantidad : 0) == 0 ? 0
                             : (grp.Sum(x => x.isMezcla == false && x.idTipoElemLog == 5 ? x.cantidad : 0)) / (1.0 * grp.Sum(x => x.idTipoElemLog == 5 ? x.cantidad : 0))
                         }).ToList();

        return Ok(detallado);
    }
}
