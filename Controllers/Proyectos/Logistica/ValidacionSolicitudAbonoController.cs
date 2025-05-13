using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;
public class ValidacionSolicitudAbonoController : ODataController
{
    private readonly bdERP db;
    public ValidacionSolicitudAbonoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/ValidacionSolicitudAbono/getPrendasValidacionSolicitud")]
    [Authorize]
    public async Task<ActionResult> getPrendasValidacionSolicitud([FromODataUri] int? idSolicitudAbono, [FromODataUri] int? idEntidad)
    {

        var idEntidadFound = db.tblSolicitudAbono.Where(x => x.idSolicitudAbono == idSolicitudAbono).FirstOrDefault()?.idEntidad ?? idEntidad;
        var result = new List<prendaSolicitudAbono>();

        var prendasNEntidad = (from pre in db.tblPrenda
                               join prendaEnti in db.tblPrendaNEntidad_NuevoPedido on pre.idPrenda equals prendaEnti.idPrenda
                               where pre.activo == true && prendaEnti.idEntidad == idEntidadFound
                               orderby pre.codigoPrenda
                               select new
                               {
                                   pre.idPrenda,
                                   pre.codigoPrenda,
                                   denoPrenda = pre.denominacion,
                                   codigoElemTrans = pre.elementoPedidoNavigation.codigo,
                                   denoElemTransPedido = pre.elementoPedidoNavigation.denominacion,
                                   pre.udsXBacPedido,
                                   colorTapa = pre.idColorTapaNavigation.codigoHexadecimal,
                                   pre.idMarcaTapaNavigation.marca,
                                   cantidad = 0
                               }).ToList();


        foreach (var prenda in prendasNEntidad)
        {

            result.Add(new prendaSolicitudAbono()
            {
                idPrenda = prenda.idPrenda,
                codigoPrenda = prenda.codigoPrenda,
                denoPrenda = prenda.denoPrenda,
                codigoElemTrans = prenda.codigoElemTrans,
                denominacionElemTrans = prenda.denoElemTransPedido,
                udsXBacPedido = (short)prenda.udsXBacPedido,
                colorTapa = prenda.colorTapa,
                marca = prenda.marca,
                cantidad = 0
            });
        }

        if (idSolicitudAbono != null)
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
                                             codigoElemTrans = pre.elementoPedidoNavigation.codigo,
                                             denoElemTransPedido = pre.elementoPedidoNavigation.denominacion,
                                             pre.udsXBacPedido,
                                             cantidad = psa.cantidad / pre.udsXBacPedido,
                                             colorTapa = pre.idColorTapaNavigation.codigoHexadecimal,
                                             pre.idMarcaTapaNavigation.marca
                                         }).ToList();

            foreach (var prenda in result)
            {
                var prendaSolicitud = prendasSolicitudAbono.FirstOrDefault(x => x.idPrenda == prenda.idPrenda);
                if (prendaSolicitud != null)
                {
                    prenda.cantidad = prendaSolicitud.cantidad ?? 0;
                }
            }
        }

        return Ok(result);
    }
}

public class prendaSolicitudAbono
{
    public int idPrenda { get; set; }
    public string codigoPrenda { get; set; }
    public string denoPrenda { get; set; }
    public string codigoElemTrans { get; set; }
    public string denominacionElemTrans { get; set; }
    public short udsXBacPedido { get; set; }
    public string colorTapa { get; set; }
    public string marca { get; set; }
    public int cantidad { get; set; }
}

