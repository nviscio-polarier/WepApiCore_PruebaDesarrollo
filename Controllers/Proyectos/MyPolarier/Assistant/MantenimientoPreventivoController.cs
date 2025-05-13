using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class MantenimientoPreventivoController : ODataController
{
    private readonly bdERP db;
    public MantenimientoPreventivoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Assistant/MantenimientoPreventivo/getMaquinasNTareaMantenimiento")]
    [Authorize]
    public async Task<ActionResult> getMaquinasNTareaMantenimiento([FromODataUri] int idLavanderia)
    {
        var fechaActual = DateTime.Now;

        var query = from maq in db.tblMaquina
                    join tmc in db.tblTipoMaquinaNCategoriaMaquina on maq.idTipoMaquinaNCategoriaMaquina equals tmc.idTipoMaquinaNCategoriaMaquina into tmcJoin
                    from tmc in tmcJoin.DefaultIfEmpty()
                    join cmq in db.tblCategoriaMaquina on tmc.idCategoriaMaquina equals cmq.idCategoriaMaquina into cmqJoin
                    from cmq in cmqJoin.DefaultIfEmpty()
                    join smq in db.tblSistemaMaquina on cmq.idSistemaMaquina equals smq.idSistemaMaquina into smqJoin
                    from smq in smqJoin.DefaultIfEmpty()
                    join psl in db.tblPesoNSistemaNLavanderia on new { smq.idSistemaMaquina, maq.idLavanderia } equals new { psl.idSistemaMaquina, psl.idLavanderia } into pslJoin
                    from psl in pslJoin.DefaultIfEmpty()
                    join pcl in db.tblPesoNCategoriaNLavanderia on new { cmq.idCategoriaMaquina, maq.idLavanderia } equals new { pcl.idCategoriaMaquina, pcl.idLavanderia } into pclJoin
                    from pcl in pclJoin.DefaultIfEmpty()
                    from maqTarea in db.tblPlantillaTareaMantenimientoPrev.Where(ptmp => ptmp.idPlantillaTareaMantenimientoPrev == maq.idPlantillaTareaMantenimientoPrev).DefaultIfEmpty()
                    where maq.activo == true && maq.eliminado == false && maq.idLavanderia == idLavanderia && maq.idPlantillaTareaMantenimientoPrev != null
                    orderby maq.denominacion
                    select new
                    {
                        maq.idMaquina,
                        maq.denominacion,
                        maq.etiqueta,
                        maq.numSerie,
                        pesoTotal = (maq.peso / 100 * pcl.peso / 100 * psl.peso / 100) * 100,
                        tareasUrgentes = maqTarea.tblTareaMantenimientoPrev
                            .Where(tm => tm.tblMantenimientoPrev.Where(mp => mp.idMaquina == maq.idMaquina).Any() &&
                                            (tm.tblMantenimientoPrev.Where(mp => mp.idMaquina == maq.idMaquina).Max(m => m.fecha).Date.AddDays(tm.cadencia) < fechaActual.Date ||// Que la ultima fecha + cadencia o próximo mantenimiento sea menor que hoy
                                             tm.tblMantenimientoPrev.Where(mp => mp.idMaquina == maq.idMaquina).Max(m => m.proxMantenimiento).Value.Date < fechaActual.Date))
                            .Count(),
                        tareasActuales = maqTarea.tblTareaMantenimientoPrev
                            .Where(tm => !tm.tblMantenimientoPrev.Where(mp => mp.idMaquina == maq.idMaquina).Any() || // Sin mantenimiento
                                        (tm.tblMantenimientoPrev.Where(mp => mp.idMaquina == maq.idMaquina).Max(m => m.fecha).Date != fechaActual.Date && // Que la fecha del último mantenimiento no sea hoy
                                        (tm.tblMantenimientoPrev.Where(mp => mp.idMaquina == maq.idMaquina).Max(m => m.fecha).Date.AddDays(tm.cadencia) == fechaActual.Date ||
                                         tm.tblMantenimientoPrev.Where(mp => mp.idMaquina == maq.idMaquina).Max(m => m.proxMantenimiento).Value.Date == fechaActual.Date))) // Que la fecha del próximo mantenimiento sea hoy
                            .Count(),
                        tareasCompletadas = maqTarea.tblTareaMantenimientoPrev
                            .Where(tm => tm.tblMantenimientoPrev.Where(mp => mp.idMaquina == maq.idMaquina).Any() &&
                                tm.tblMantenimientoPrev.Where(mp => mp.idMaquina == maq.idMaquina).Max(m => m.fecha).Date == fechaActual.Date)
                            .Count()
                    };

        var result = query.OrderByDescending(x => x.pesoTotal).Select(x => new
        {
            x.idMaquina,
            x.denominacion,
            x.etiqueta,
            x.numSerie,
            x.tareasUrgentes,
            x.tareasActuales,
            allComplete = x.tareasCompletadas > 0 && x.tareasUrgentes == 0 && x.tareasActuales == 0
        });
        return Ok(result);
    }


    [EnableQuery]
    [HttpGet("odata/MyPolarier/Assistant/MantenimientoPreventivo/getTareasMantenimiento")]
    [Authorize]
    public async Task<ActionResult> getTareasMantenimiento([FromODataUri] int idMaquina)
    {
        var idPlantillaTareaMantenimientoPreventivo = db.tblMaquina.FirstOrDefault(x => x.idMaquina == idMaquina)?.idPlantillaTareaMantenimientoPrev;
        var plantillaTareaMantenimientoPreventivo = db.tblPlantillaTareaMantenimientoPrev
                                                        .Include(x => x.tblTareaMantenimientoPrev)
                                                            .ThenInclude(x => x.tblMantenimientoPrev)
                                                                .ThenInclude(y => y.idPersona)
                                                        .FirstOrDefault(x => x.idPlantillaTareaMantenimientoPrev == idPlantillaTareaMantenimientoPreventivo);
        var fechaActual = DateTime.Now;

        var tareasNPlantilla =
            plantillaTareaMantenimientoPreventivo?.tblTareaMantenimientoPrev
            .Select(x => new
            {
                x.idTareaMantenimientoPrev,
                x.denominacion,
                x.tiempo,
                fechaLimite = x.tblMantenimientoPrev.Where(mp => mp.idMaquina == idMaquina).OrderByDescending(mp => mp.fecha).FirstOrDefault() is tblMantenimientoPrev mp
                                ? (DateTimeOffset?)(mp.proxMantenimiento.HasValue ? mp.proxMantenimiento.Value.Date : mp.fecha.AddDays(x.cadencia)) : fechaActual.Date,
                isUrgente = x.tblMantenimientoPrev.Where(mp => mp.idMaquina == idMaquina).OrderByDescending(x => x.fecha).FirstOrDefault() is tblMantenimientoPrev mantUrg &&
                    (mantUrg.proxMantenimiento.HasValue ? mantUrg.proxMantenimiento.Value.Date < fechaActual.Date : mantUrg.fecha.AddDays(x.cadencia).Date < fechaActual.Date),
                isActual = x.tblMantenimientoPrev.Where(mp => mp.idMaquina == idMaquina).OrderByDescending(x => x.fecha).FirstOrDefault() is tblMantenimientoPrev mantActual &&
                    (mantActual.proxMantenimiento.HasValue ? mantActual.proxMantenimiento.Value.Date == fechaActual.Date : mantActual.fecha.AddDays(x.cadencia).Date == fechaActual.Date)
                    || x.tblMantenimientoPrev?.Where(mp => mp.idMaquina == idMaquina).Any() == false,
                isCompleta = x.tblMantenimientoPrev
                    .Any(mp => mp.idMaquina == idMaquina && mp.fecha.Date == fechaActual.Date),
                x.numTecnicos,
                tecnicos = x.tblMantenimientoPrev?
                    .Where(mp => mp.idMaquina == idMaquina && mp.fecha.Date == fechaActual.Date)
                    .LastOrDefault(mp => mp.idPersona != null)?
                    .idPersona?.Select(p => new
                    {
                        p.nombre,
                        p.apellidos,
                        p.idPersona,
                    }).ToList()
            })
            // Filtrar solo las tareas que sean urgentes, actuales o completas
            .Where(x => x.isUrgente || x.isActual || x.isCompleta)
            .ToList();

        var sortedResult = tareasNPlantilla?
                        .OrderByDescending(x => x.isUrgente && !x.isActual && !x.isCompleta)
                        .ThenByDescending(x => !x.isUrgente && !x.isActual && !x.isCompleta)
                        .ThenBy(x => x.fechaLimite.HasValue ? x.fechaLimite.Value : DateTimeOffset.MaxValue)
                        .ThenBy(x => x.denominacion);

        return Ok(sortedResult);
    }
}
