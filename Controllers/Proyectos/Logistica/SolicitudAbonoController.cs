using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;
public class SolicitudAbonoController : ODataController
{
    private readonly bdERP db;
    public SolicitudAbonoController(bdERP context)
    {
        db = context;
    }


    [EnableQuery]
    [HttpGet("odata/SolicitudAbono/getPrendasNNuevoPedido")]
    [Authorize]
    public async Task<ActionResult> getPrendasNNuevoPedido([FromODataUri] int? idEntidad)
    {

        var prendaNEntidad_nuevoPedido = (from pre in db.tblPrenda
                                          join prendaEnti in db.tblPrendaNEntidad_NuevoPedido on pre.idPrenda equals prendaEnti.idPrenda
                                          where pre.activo == true && prendaEnti.idEntidad == idEntidad
                                          orderby pre.codigoPrenda
                                          select new
                                          {
                                              pre.idPrenda,
                                              pre.codigoPrenda,
                                              denoPrenda = pre.denominacion,
                                              codigoElemTrans = 1,
                                              denoElemTransPedido = "POR UDS.",
                                              pre.udsXBacPedido,
                                              colorTapa = "#AEAAAA",// pre.idColorTapaNavigation.codigoHexadecimal,
                                              pre.idMarcaTapaNavigation.marca,
                                              cantidad = 0
                                          }).ToList();

        return Ok(prendaNEntidad_nuevoPedido);
    }

    [EnableQuery]
    [HttpGet("odata/SolicitudAbono/getPrendasSolicitudAbono")]
    [Authorize]
    public async Task<ActionResult> getPrendasSolicitudAbono([FromODataUri] int? idSolicitudAbono)
    {
        var prendasSolicitudAbono = (from pre in db.tblPrenda
                                     join psa in db.tblPrendaNSolicitudAbono on pre.idPrenda equals psa.idPrenda into psaGroup
                                     from psa in psaGroup.DefaultIfEmpty()
                                     where psa.idSolicitudAbono == idSolicitudAbono
                                     select new
                                     {
                                         pre.idPrenda,
                                         pre.codigoPrenda,
                                         denoPrenda = pre.denominacion,
                                         codigoElemTrans = 1,
                                         denoElemTransPedido = "POR UDS.",
                                         pre.udsXBacPedido,
                                         psa.cantidad,
                                         colorTapa = "#AEAAAA",// pre.idColorTapaNavigation.codigoHexadecimal,
                                         pre.idMarcaTapaNavigation.marca
                                     }).ToList();

        return Ok(prendasSolicitudAbono);
    }

    [EnableQuery]
    [HttpGet("odata/SolicitudAbono/getPrendasAbono")]
    [Authorize]
    public async Task<ActionResult> getPrendasAbono([FromODataUri] int? idAbono)
    {
        var prendasSolicitudAbono = (from pre in db.tblPrenda
                                     join pa in db.tblPrendaNAbono on pre.idPrenda equals pa.idPrenda into paGroup
                                     from pa in paGroup.DefaultIfEmpty()
                                     where pa.idAbono == idAbono
                                     select new
                                     {
                                         pre.idPrenda,
                                         pre.codigoPrenda,
                                         denoPrenda = pre.denominacion,
                                         codigoElemTrans = 1,
                                         denoElemTransPedido = "POR UDS.",
                                         pre.udsXBacPedido,
                                         pa.cantidad,
                                         colorTapa = "#AEAAAA",// pre.idColorTapaNavigation.codigoHexadecimal,
                                         pre.idMarcaTapaNavigation.marca
                                     }).ToList();

        return Ok(prendasSolicitudAbono);
    }

    [EnableQuery]
    [HttpGet("odata/SolicitudAbono/getDatosAbonos")]
    [Authorize]
    public async Task<ActionResult> getDatosAbonos([FromODataUri] int? idLavanderia)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia);

        var tblAbono = db.tblAbono
            .Include(x => x.idEntidadNavigation)
            .Include(x => x.idCategoriaAbonoNavigation)
            .Include(x => x.idTipoAbonoNavigation)
            .Where(x => idsEntidad.Contains((int)x.idEntidad));

        var tblSolicitudAbono = db.tblSolicitudAbono
            .Include(x => x.idEntidadNavigation)
            .Include(x => x.idCategoriaAbonoNavigation)
            .Include(x => x.idEstadoSolicitudAbonoNavigation)
            .Where(x => idsEntidad.Contains(x.idEntidad)).ToList();

        var idsAbono = tblAbono.Select(x => x.idAbono).ToList();

        var tblCierreFactEntidadFilter = db.tblCierreFactEntidad
                                        .Where(x => idsAbono.Contains(x.idEntidad))
                                        .Select(x => new { x.idEntidad, x.fechaHasta, x.fechaDesde }).ToList();

        List<itemAbono> itemsAbono = new List<itemAbono>();

        foreach (var solicitud in tblSolicitudAbono)
        {
            itemAbono item = new itemAbono();
            item.idSolicitudAbono = solicitud.idSolicitudAbono;
            item.idAbono = solicitud.idAbono;
            item.fechaAbono = solicitud.idAbonoNavigation?.fecha;
            item.fechaSolicitudAbono = solicitud.fecha;
            item.codigo = solicitud.codigo;

            item.tblEntidad = new itemEntidad
            {
                idEntidad = solicitud.idEntidadNavigation.idEntidad,
                denominacion = solicitud.idEntidadNavigation.denominacion,
                idTipoConsumoLenceria = solicitud.idEntidadNavigation.idTipoConsumoLenceria

            };

            item.tblCategoriaAbono = new itemCategoriaAbono
            {
                idCategoriaAbono = solicitud.idCategoriaAbonoNavigation.idCategoriaAbono,
                denominacion = solicitud.idCategoriaAbonoNavigation.denominacion
            };


            if (solicitud.idEstadoSolicitudAbonoNavigation != null)
            {
                item.tblEstadoSolicitudAbono = new itemEstadoSolicitudAbono
                {
                    idEstadoSolicitudAbono = solicitud.idEstadoSolicitudAbonoNavigation.idEstadoSolicitudAbono,
                    denominacion = solicitud.idEstadoSolicitudAbonoNavigation.denominacion
                };
            }


            itemsAbono.Add(item);
        }

        foreach (var abono in tblAbono)
        {

            var isInSolicitud = itemsAbono.Where(x => x.idAbono == abono.idAbono).FirstOrDefault();
            itemAbono item = new itemAbono();

            if (isInSolicitud == null)
            {
                item.idAbono = abono.idAbono;
                item.fechaAbono = (DateTimeOffset)abono.fecha;
                item.codigo = abono.codigo;
                item.observaciones = abono.observaciones;

                item.tblEntidad = new itemEntidad
                {
                    idEntidad = abono.idEntidadNavigation.idEntidad,
                    denominacion = abono.idEntidadNavigation.denominacion,
                    idTipoConsumoLenceria = abono.idEntidadNavigation.idTipoConsumoLenceria
                };


                item.tblCategoriaAbono = new itemCategoriaAbono
                {
                    idCategoriaAbono = abono.idCategoriaAbonoNavigation.idCategoriaAbono,
                    denominacion = abono.idCategoriaAbonoNavigation.denominacion
                };

                item.tblTipoAbono = new itemTipoAbono
                {
                    idTipoAbono = abono.idTipoAbonoNavigation.idTipoAbono,
                    denominacion = abono.idTipoAbonoNavigation.denominacion
                };

                if (tblCierreFactEntidadFilter
                    .Where(x => x.idEntidad == abono.idEntidad &&
                                abono.fecha <= x.fechaHasta &&
                                abono.fecha >= x.fechaDesde)
                    .FirstOrDefault() != null)
                {
                    item.isFactCerrada = true;
                }

                itemsAbono.Add(item);
            }
            else
            {
                isInSolicitud.tblTipoAbono = new itemTipoAbono
                {
                    idTipoAbono = abono.idTipoAbonoNavigation.idTipoAbono,
                    denominacion = abono.idTipoAbonoNavigation.denominacion
                };
                isInSolicitud.observaciones = abono.observaciones;
                isInSolicitud.fechaAbono = (DateTimeOffset)abono.fecha;
            }

        }

        return Ok(itemsAbono);
    }

}

public class itemAbono
{
    public int? idSolicitudAbono { get; set; }
    public int? idAbono { get; set; }
    public string? codigo { get; set; }
    public string? observaciones { get; set; }
    public DateTimeOffset? fechaAbono { get; set; }
    public DateTimeOffset? fechaSolicitudAbono { get; set; }
    public itemCategoriaAbono? tblCategoriaAbono { get; set; }
    public itemTipoAbono? tblTipoAbono { get; set; }
    public itemEntidad? tblEntidad { get; set; }
    public itemEstadoSolicitudAbono? tblEstadoSolicitudAbono { get; set; }
    public bool? isFactCerrada { get; set; }
}
public class itemEntidad
{
    public int idEntidad { get; set; }
    public string? denominacion { get; set; }
    public int? idTipoConsumoLenceria { get; set; }
}

public class itemTipoAbono
{
    public byte idTipoAbono { get; set; }
    public string? denominacion { get; set; }
}

public class itemCategoriaAbono
{
    public byte idCategoriaAbono { get; set; }
    public string? denominacion { get; set; }
}

public class itemEstadoSolicitudAbono
{
    public byte idEstadoSolicitudAbono { get; set; }
    public string? denominacion { get; set; }
}


