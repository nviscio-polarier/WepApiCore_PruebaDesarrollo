using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCierreFactEntidadController : ODataController
{
    private readonly bdERP db;

    public tblCierreFactEntidadController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxNodeCount = 3000)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblCierreFactEntidad);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblCierreFactEntidad cierre)
    {
        var entidadEntity = await db.tblEntidad.FindAsync(cierre.idEntidad);
        //ICollection<tblPrendaCierreFact> prendasEntity = db.tblPrenda
        //    .Where(x => x.idEntidad == cierre.idEntidad)
        //    .Select(x => new tblPrendaCierreFact { 
        //        idPrenda = x.idPrenda, 
        //        tipoFact = x.tipoFact, 
        //        percPrecioDesmanche = x.percPrecioDesmanche, 
        //        peso = x.peso, 
        //        precioRefacturado = db.tblPrendaPrecioRefact.Where(ttp => ttp.idPrenda == x.idPrenda).Select(x=>x.precioRefacturado).FirstOrDefault(),
        //        precioLavadoPrenda = db.tblPrecioLavadoPrenda.Where(plp => x.idPrenda == plp.idPrenda && plp.fecha <= cierre.fechaHasta).Select(x => x.precio).FirstOrDefault()
        //    }).ToList();


        var fechasCierre = db.tblCierreFactEntidad
                            .Where(
                                x => x.idEntidad == cierre.idEntidad &&
                                ((x.fechaDesde >= cierre.fechaDesde && x.fechaDesde <= cierre.fechaHasta) ||
                                (x.fechaHasta >= cierre.fechaDesde && x.fechaDesde <= cierre.fechaHasta))
                            );

        List<tblCierreFactEntidad> fechasCierreLocal = fechasCierre.ToList();

        fechasCierreLocal.Add(cierre);
        tblCierreFactEntidad fechaFinalAgrupada = fechasCierreLocal
                                                        .GroupBy(item => item.idEntidad)
                                                        .Select(x => new tblCierreFactEntidad
                                                        {
                                                            idEntidad = x.Key,
                                                            fechaDesde = x.Min(y => y.fechaDesde),
                                                            fechaHasta = x.Max(y => y.fechaHasta)
                                                            //idTipoConsumoLenceria = entidadEntity.idTipoConsumoLenceria,
                                                            //reparteRechazoRetiro = entidadEntity.reparteRechazoRetiro,
                                                            //idTipoFacturacionCliente = entidadEntity.idTipoFacturacionCliente,
                                                            //costeEstancia = entidadEntity.costeEstancia,
                                                            //idMoneda = entidadEntity.idMoneda,
                                                            //tblPrendaCierreFact = prendasEntity
                                                        })
                                                        .First();

        db.tblCierreFactEntidad.RemoveRange(fechasCierre);
        db.tblCierreFactEntidad.Add(fechaFinalAgrupada);

        await db.SaveChangesAsync();

        return Created(fechaFinalAgrupada);
    }

    [EnableQuery]
    [HttpDelete("odata/tblCierreFactEntidad/{idEntidad}/{fechaDesde}/{fechaHasta}")]
    [Authorize]
    public async Task<bool> Delete(int idEntidad, DateTime fechaDesde, DateTime fechaHasta)
    {
        var entidadEntity = await db.tblEntidad.FindAsync(idEntidad);
        //ICollection<tblPrendaCierreFact> prendasEntity = db.tblPrenda
        //    .Where(x => x.idEntidad == idEntidad)
        //    .Select(x => new tblPrendaCierreFact { 
        //        idPrenda = x.idPrenda, 
        //        tipoFact = x.tipoFact, 
        //        percPrecioDesmanche = x.percPrecioDesmanche, 
        //        peso = x.peso,
        //        precioRefacturado = db.tblPrendaPrecioRefact.Where(ttp => ttp.idPrenda == x.idPrenda).Select(x => x.precioRefacturado).FirstOrDefault(),
        //        precioLavadoPrenda = db.tblPrecioLavadoPrenda.Where(plp => x.idPrenda == plp.idPrenda && plp.fecha <= fechaHasta).Select(x => x.precio).FirstOrDefault()
        //    }).ToList();

        var fechasCierre = db.tblCierreFactEntidad
                          .Where(
                              x => x.idEntidad == idEntidad &&
                              ((fechaDesde >= x.fechaDesde && fechaDesde <= x.fechaHasta) ||
                              (fechaHasta >= x.fechaDesde && fechaHasta <= x.fechaHasta) ||
                              (x.fechaDesde >= fechaDesde && x.fechaDesde <= fechaHasta) ||
                              (x.fechaHasta >= fechaDesde && x.fechaHasta <= fechaHasta))
                          );
        List<tblCierreFactEntidad> fechasCierreLocal = fechasCierre.ToList();

        List<DateTime> fechasNoBorradas = new List<DateTime>();
        foreach (tblCierreFactEntidad cierre in fechasCierreLocal)
        {
            for (var day = cierre.fechaDesde; day <= cierre.fechaHasta; day = day.AddDays(1))
            {
                if (!(day >= fechaDesde && day <= fechaHasta)) // Si día no está comprendido entra la fecha a borrar
                    fechasNoBorradas.Add(day);
            }
        }

        List<tblCierreFactEntidad> cierres = new List<tblCierreFactEntidad>();
        foreach (var agrupacionConsecutiva in Utils.GroupConsecutiveDates(fechasNoBorradas))
        {
            tblCierreFactEntidad cierreFactEntidad = new tblCierreFactEntidad()
            {
                idEntidad = idEntidad,
                fechaDesde = agrupacionConsecutiva.Min(),
                fechaHasta = agrupacionConsecutiva.Max()
                //idTipoConsumoLenceria = entidadEntity.idTipoConsumoLenceria,
                //reparteRechazoRetiro = entidadEntity.reparteRechazoRetiro,
                //idTipoFacturacionCliente = entidadEntity.idTipoFacturacionCliente,
                //costeEstancia = entidadEntity.costeEstancia,
                //idMoneda = entidadEntity.idMoneda,
                //tblPrendaCierreFact = prendasEntity
            };
            cierres.Add(cierreFactEntidad);
        }

        db.tblCierreFactEntidad.RemoveRange(fechasCierre);
        db.tblCierreFactEntidad.AddRange(cierres);

        await db.SaveChangesAsync();
        return true;
    }
}
