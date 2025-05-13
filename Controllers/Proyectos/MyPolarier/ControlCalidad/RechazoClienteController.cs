using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class.bdERP.ControlCalidad;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class RechazoClienteController : ODataController
{
    private readonly bdERP db;
    public RechazoClienteController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/ControlCalidad/RechazoCliente/compañia")]
    [Authorize]
    public ActionResult compañia([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] int idCompañia, [FromODataUri] int idEntidad)
    {
        var result = (from rep in db.tblReparto
                      join pnr in db.tblPrendaNReparto on rep.idReparto equals pnr.idReparto
                      join enti in db.tblEntidad on rep.idEntidad equals enti.idEntidad
                      join compa in db.tblCompañia on enti.idCompañia equals compa.idCompañia
                      join pre in db.tblPrenda on pnr.idPrenda equals pre.idPrenda
                      where
                         enti.idLavanderia.Select(x => x.idLavanderia).Contains(idLavanderia)
                         && rep.fecha.Value.Date >= fechaIni.Date && rep.fecha.Value.Date <= fechaFin.Date
                      select new RechazoCliente
                      {
                          idPrenda = pnr.idPrenda,
                          denoPrenda = pre.denominacion,
                          fechaString = rep.fecha.ToString(),
                          idEntidad = enti.idEntidad,
                          denoEnti = enti.denominacion,
                          idCompañia = compa.idCompañia,
                          denoCompa = compa.denominacion,
                          cantidadReparto = (int?)pnr.cantidad,
                          cantidadAbono = (int?)null,
                          cantAbonoCalidad = (int?)null,
                          cantAbonoError = (int?)null,
                      })
                    .Concat(from abo in db.tblAbono
                           join pna in db.tblPrendaNAbono on abo.idAbono equals pna.idAbono
                           join enti in db.tblEntidad on abo.idEntidad equals enti.idEntidad
                           join compa in db.tblCompañia on enti.idCompañia equals compa.idCompañia
                           join pre in db.tblPrenda on pna.idPrenda equals pre.idPrenda
                           where
                                 enti.idLavanderia.Select(x => x.idLavanderia).Contains(idLavanderia)
                                 && abo.fecha.Value.Date >= fechaIni.Date && abo.fecha.Value.Date <= fechaFin.Date
                                 && abo.idCategoriaAbono != 3
                           select new RechazoCliente
                           {
                               idPrenda = pna.idPrenda,
                               denoPrenda = pre.denominacion,
                               fechaString = abo.fecha.ToString(),
                               idEntidad = enti.idEntidad,
                               denoEnti = enti.denominacion,
                               idCompañia = compa.idCompañia,
                               denoCompa = compa.denominacion,
                               cantidadReparto = (int?)null,
                               cantidadAbono = (int?)pna.cantidad,
                               cantAbonoCalidad = abo.idCategoriaAbono == 1 ? pna.cantidad : 0,
                               cantAbonoError = abo.idCategoriaAbono == 2 ? pna.cantidad : 0,
                           })
                    .GroupBy(x => new { x.idCompañia, x.denoCompa }).Select(x => new
                    {
                        id = "idCompañia_" + x.Key.idCompañia,
                        denominacion = x.Key.denoCompa,
                        udsEntregadas = x.Sum(y => y.cantidadReparto),
                        udsCorrectas = x.Sum(y => y.cantidadReparto) - x.Sum(y => y.cantidadAbono),
                        udsAbonadas = x.Sum(y => y.cantidadAbono),
                        cantAbonoCalidad = x.Sum(y => y.cantAbonoCalidad),
                        cantAbonoError = x.Sum(y => y.cantAbonoError),
                        cumplimiento = x.Sum(y => y.cantidadReparto * 1.0) == 0
                                        ? 0
                                        : ((x.Sum(y => y.cantidadReparto) - x.Sum(y => y.cantidadAbono)) * 1.0) / x.Sum(y => y.cantidadReparto * 1.0)
                    });
        return Ok(result);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/ControlCalidad/RechazoCliente/entidad")]
    [Authorize]
    public ActionResult entidad([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] int idCompañia)
    {
        var result = (from rep in db.tblReparto
                      join pnr in db.tblPrendaNReparto on rep.idReparto equals pnr.idReparto
                      join enti in db.tblEntidad on rep.idEntidad equals enti.idEntidad
                      join compa in db.tblCompañia on enti.idCompañia equals compa.idCompañia
                      join pre in db.tblPrenda on pnr.idPrenda equals pre.idPrenda
                      where
                         compa.idCompañia == idCompañia
                         && enti.idLavanderia.Select(x => x.idLavanderia).Contains(idLavanderia)
                         && rep.fecha.Value.Date >= fechaIni.Date && rep.fecha.Value.Date <= fechaFin.Date
                      select new RechazoCliente
                      {
                          idPrenda = pnr.idPrenda,
                          denoPrenda = pre.denominacion,
                          fechaString = rep.fecha.ToString(),
                          idEntidad = enti.idEntidad,
                          denoEnti = enti.denominacion,
                          idCompañia = compa.idCompañia,
                          denoCompa = compa.denominacion,
                          cantidadReparto = (int?)pnr.cantidad,
                          cantidadAbono = (int?)null,
                          cantAbonoCalidad = (int?)null,
                          cantAbonoError = (int?)null,
                      })
            .Concat(from abo in db.tblAbono
                   join pna in db.tblPrendaNAbono on abo.idAbono equals pna.idAbono
                   join enti in db.tblEntidad on abo.idEntidad equals enti.idEntidad
                   join compa in db.tblCompañia on enti.idCompañia equals compa.idCompañia
                   join pre in db.tblPrenda on pna.idPrenda equals pre.idPrenda
                   where
                         compa.idCompañia == idCompañia
                         && enti.idLavanderia.Select(x => x.idLavanderia).Contains(idLavanderia)
                         && abo.fecha.Value.Date >= fechaIni.Date && abo.fecha.Value.Date <= fechaFin.Date
                         && abo.idCategoriaAbono != 3
                   select new RechazoCliente
                   {
                       idPrenda = pna.idPrenda,
                       denoPrenda = pre.denominacion,
                       fechaString = abo.fecha.ToString(),
                       idEntidad = enti.idEntidad,
                       denoEnti = enti.denominacion,
                       idCompañia = compa.idCompañia,
                       denoCompa = compa.denominacion,
                       cantidadReparto = (int?)null,
                       cantidadAbono = (int?)pna.cantidad,
                       cantAbonoCalidad = abo.idCategoriaAbono == 1 ? pna.cantidad : 0,
                       cantAbonoError = abo.idCategoriaAbono == 2 ? pna.cantidad : 0,
                   })
            .GroupBy(x => new { x.idEntidad, x.denoEnti }).Select(x => new
            {
                id = "idEntidad_" + x.Key.idEntidad,
                denominacion = x.Key.denoEnti,
                udsEntregadas = x.Sum(y => y.cantidadReparto),
                udsCorrectas = x.Sum(y => y.cantidadReparto) - x.Sum(y => y.cantidadAbono),
                udsAbonadas = x.Sum(y => y.cantidadAbono),
                cantAbonoCalidad = x.Sum(y => y.cantAbonoCalidad),
                cantAbonoError = x.Sum(y => y.cantAbonoError),
                cumplimiento = x.Sum(y => y.cantidadReparto * 1.0) == 0
                                ? 0
                                : ((x.Sum(y => y.cantidadReparto) - x.Sum(y => y.cantidadAbono)) * 1.0) / x.Sum(y => y.cantidadReparto * 1.0)
            });
        return Ok(result);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/ControlCalidad/RechazoCliente/detallado")]
    [Authorize]
    public ActionResult detallado([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni,
        [FromODataUri] DateTime fechaFin, [FromODataUri] int? idEntidad, [FromODataUri] int? idCompañia, [FromODataUri] string tipoAgrupacion)
    {
        var detallado = getDetallado(idLavanderia, fechaIni, fechaFin, idEntidad, idCompañia);
        if (tipoAgrupacion == "prenda")
        {
            return Ok(
                detallado
                .GroupBy(x => new { x.idPrenda, x.denoPrenda })
                .Select(x => new
                {
                    id = "idPrenda_" + x.Key.idPrenda,
                    denominacion = x.Key.denoPrenda,
                    udsEntregadas = x.Sum(y => y.udsEntregadas),
                    udsCorrectas = x.Sum(y => y.udsEntregadas) - x.Sum(y => y.udsAbonadas),
                    udsAbonadas = x.Sum(y => y.udsAbonadas),
                    cantAbonoCalidad = x.Sum(y => y.cantAbonoCalidad),
                    cantAbonoError = x.Sum(y => y.cantAbonoError),
                    cumplimiento = x.Sum(y => y.udsEntregadas * 1.0) == 0
                                ? 0
                                : ((x.Sum(y => y.udsEntregadas) - x.Sum(y => y.udsAbonadas)) * 1.0) / x.Sum(y => y.udsEntregadas * 1.0)
                })
                .ToList()
            );
        }
        else
        {
            return Ok(
                detallado.ToList()
                .GroupBy(x => x.fecha.Date)
                .Select(x => new
                {
                    id = "idFecha_" + x.Key.ToShortDateString(),
                    fecha = x.Key,
                    udsEntregadas = x.Sum(y => y.udsEntregadas),
                    udsCorrectas = x.Sum(y => y.udsEntregadas) - x.Sum(y => y.udsAbonadas),
                    udsAbonadas = x.Sum(y => y.udsAbonadas),
                    cantAbonoCalidad = x.Sum(y => y.cantAbonoCalidad),
                    cantAbonoError = x.Sum(y => y.cantAbonoError),
                    cumplimiento = x.Sum(y => y.udsEntregadas * 1.0) == 0
                                ? 0
                                : ((x.Sum(y => y.udsEntregadas) - x.Sum(y => y.udsAbonadas)) * 1.0) / x.Sum(y => y.udsEntregadas * 1.0)
                }).ToList()
           );
        }
    }

    private IQueryable<RechazoCliente> getDetallado(int idLavanderia, DateTime fechaIni, DateTime fechaFin, int? idEntidad, int? idCompañia)
    {
        return (from rep in db.tblReparto
                join pnr in db.tblPrendaNReparto on rep.idReparto equals pnr.idReparto
                join enti in db.tblEntidad on rep.idEntidad equals enti.idEntidad
                join compa in db.tblCompañia on enti.idCompañia equals compa.idCompañia
                join pre in db.tblPrenda on pnr.idPrenda equals pre.idPrenda
                where
                   ((idCompañia != null && compa.idCompañia == idCompañia) || idCompañia == null)
                    && ((idEntidad != null && enti.idEntidad == idEntidad) || idEntidad == null)
                   && enti.idLavanderia.Select(x => x.idLavanderia).Contains(idLavanderia)
                   && rep.fecha.Value.Date >= fechaIni.Date && rep.fecha.Value.Date <= fechaFin.Date
                select new RechazoCliente
                {
                    idPrenda = pnr.idPrenda,
                    denoPrenda = pre.denominacion,
                    fechaString = rep.fecha.ToString(),
                    idEntidad = enti.idEntidad,
                    denoEnti = enti.denominacion,
                    idCompañia = compa.idCompañia,
                    denoCompa = compa.denominacion,
                    cantidadReparto = (int?)pnr.cantidad,
                    cantidadAbono = (int?)null,
                    cantAbonoCalidad = (int?)null,
                    cantAbonoError = (int?)null
                })
            .Concat(from abo in db.tblAbono
                   join pna in db.tblPrendaNAbono on abo.idAbono equals pna.idAbono
                   join enti in db.tblEntidad on abo.idEntidad equals enti.idEntidad
                   join compa in db.tblCompañia on enti.idCompañia equals compa.idCompañia
                   join pre in db.tblPrenda on pna.idPrenda equals pre.idPrenda
                   where ((idCompañia != null && compa.idCompañia == idCompañia) || idCompañia == null)
                         && ((idEntidad != null && enti.idEntidad == idEntidad) || idEntidad == null)
                         && enti.idLavanderia.Select(x => x.idLavanderia).Contains(idLavanderia)
                         && abo.fecha.Value.Date >= fechaIni.Date && abo.fecha.Value.Date <= fechaFin.Date
                         && abo.idCategoriaAbono != 3
                   select new RechazoCliente
                   {
                       idPrenda = pna.idPrenda,
                       denoPrenda = pre.denominacion,
                       fechaString = abo.fecha.ToString(),
                       idEntidad = enti.idEntidad,
                       denoEnti = enti.denominacion,
                       idCompañia = compa.idCompañia,
                       denoCompa = compa.denominacion,
                       cantidadReparto = (int?)null,
                       cantidadAbono = (int?)pna.cantidad,
                       cantAbonoCalidad = abo.idCategoriaAbono == 1 ? pna.cantidad : 0,
                       cantAbonoError = abo.idCategoriaAbono == 2 ? pna.cantidad : 0,
                   })
            .Select(x => new RechazoCliente
            {
                idPrenda = x.idPrenda,
                denoPrenda = x.denoPrenda,
                fecha = DateTime.Parse(x.fechaString).Date,
                idEntidad = x.idEntidad,
                idCompañia = x.idCompañia,
                denoCompa = x.denoCompa,
                denoEnti = x.denoEnti,
                udsEntregadas = x.cantidadReparto,
                udsAbonadas = x.cantidadAbono ?? 0,
                cantAbonoCalidad = x.cantAbonoCalidad ?? 0,
                cantAbonoError = x.cantAbonoError ?? 0
            });
    }
}
