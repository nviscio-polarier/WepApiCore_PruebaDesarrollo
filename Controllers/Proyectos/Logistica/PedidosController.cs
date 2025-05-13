using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;
public class PedidosController : ODataController
{
    private readonly bdERP db;
    public PedidosController(bdERP context)
    {
        db = context;
    }


    [EnableQuery]
    [HttpGet("odata/Pedidos/EjecutarPlanificador")]
    [Authorize]
    public async Task<ActionResult> EjecutarPlanificador([FromODataUri] int idEntidad, [FromODataUri] DateTime? fecha, [FromODataUri] int? numDias)
    {
        if (idEntidad == 0) return BadRequest("idEntidad no puede ser 0");
        //Estancias_Worker.PlanificarPedidos(db, idEntidad);

        DateTime fechaPedido = fecha ?? DateTime.Now;

        int diasDesde = -1 * (numDias ?? 3); // Días previos utilizados para calcular los nuevos pedidos, el valor mínimo es -2 para tener en cuenta 1 día a partir de ayer.
        int diasHasta = 1; // Cantidad de días - pedidos que se van a generar

        var fechaHasta = fechaPedido.Date.AddDays(diasHasta);
        var fechaDesde = fechaPedido.Date.AddDays(diasDesde);

        var entidades = db.tblEntidad.AsNoTracking()
            .Where(x => x.enablePlanificadorPedidos == true && (idEntidad == null || x.idEntidad == idEntidad))
            .Include(x => x.tblPrendaNEntidad_NuevoPedido)
            .Include(x => x.tblReparto.Where(r => r.fecha.Value.Date > fechaDesde.Date && r.fecha.Value.Date < fechaPedido.Date))
                .ThenInclude(x => x.tblPrendaNReparto)
            .Include(x => x.tblEstancia.Where(es => es.fecha.Date >= fechaDesde.Date && es.fecha.Date <= fechaHasta.Date));

        var prendas = db.tblPrenda
            .Where(x => x.eliminado == false && x.activo == true &&
                entidades.SelectMany(x => x.tblPrendaNEntidad_NuevoPedido).Select(x => x.idPrenda).Contains(x.idPrenda)
                )
            .ToDictionary(x => x.idPrenda, x => x.udsXBacPedido);

        var resultParameter = new SqlParameter
        {
            ParameterName = "@result",
            SqlDbType = SqlDbType.NVarChar,
            Direction = ParameterDirection.Output,
            Size = 8
        };

        db.Database.ExecuteSqlRaw("SET @result = (SELECT Logistica.EF_funCodigoPedido());", resultParameter);
        int lastCodigo = Int32.Parse((string)resultParameter.Value) - 1;

        for (int i = 0; i < diasHasta; i++)
        {
            var fecha_ = fechaPedido.AddDays(i);
            var isHoy = fecha_.Date == fechaPedido.Date;

            foreach (var entidad in entidades)
            {
                tblPedido pedido = db.tblPedido
                    .Include(x => x.tblPrendaNPedido)
                    .FirstOrDefault(x => x.isAutomatico == true && x.idEntidad == entidad.idEntidad && x.fecha.Date == fecha_.Date) ??
                    new tblPedido
                    {
                        idEntidad = entidad.idEntidad,
                        fecha = fechaPedido,
                        fechaRegistro = fechaPedido,
                        observaciones = "Pedido autogenerado por el planificador",
                        idEstadoPedido = 1, // Pendiente
                        idTipoPedido = 1, // Normal
                        isCerrado = false,
                        porcentaje = 0,
                        isApp = false,
                        isAutomatico = true,
                    };

                if (pedido.idEstadoPedido == 1)
                {
                    if (pedido.idPedido > 0)
                    {
                        pedido.fechaRegistro = fechaPedido;
                        db.tblPrendaNPedido.RemoveRange(pedido.tblPrendaNPedido);
                    }
                    else
                    {
                        lastCodigo++;
                        pedido.codigo = $"{lastCodigo}";
                    }

                    var tblPrendasNReparto = entidad.tblReparto
                        .SelectMany(x => x.tblPrendaNReparto)
                        .GroupBy(x => x.idPrenda);

                    int sumaEstanciasAnteriores = entidad.tblEstancia
                        .Where(x => isHoy ? x.fecha.Date < fechaPedido.Date : x.fecha.Date == fecha_.Date.AddDays(-1))
                        .Select(x => isHoy ? (x.estanciasReal > 0 ? x.estanciasReal : x.estanciasPrevistas) : x.estanciasPrevistas)
                        .Sum() ?? 0;

                    if (sumaEstanciasAnteriores > 0)
                    {
                        foreach (var prendaNEntidad in entidad.tblPrendaNEntidad_NuevoPedido)
                        {
                            decimal totalPrendaXEstancia = sumaEstanciasAnteriores * prendaNEntidad.ratio ?? 0;

                            int sumaUdsPrendaReparto = isHoy ? tblPrendasNReparto
                                .Where(x => x.Key == prendaNEntidad.idPrenda)
                                .Sum(x => x.Select(x => x.cantidad).Sum()) : 0;

                            decimal udsBacs = (totalPrendaXEstancia - sumaUdsPrendaReparto) / (prendas[prendaNEntidad.idPrenda] ?? 1);

                            int peticion = Convert.ToInt32(Math.Ceiling(udsBacs) * (prendas[prendaNEntidad.idPrenda] ?? 1));

                            pedido.tblPrendaNPedido.Add(new tblPrendaNPedido
                            {
                                idPrenda = prendaNEntidad.idPrenda,
                                peticion = peticion < 0 ? 0 : peticion,
                            });
                        }
                    }
                    else
                    {
                        pedido.isCerrado = true;
                    }

                    if (pedido.idPedido == 0) db.tblPedido.Add(pedido);
                }
            }
        }

        db.SaveChanges();

        return Ok();
    }

    [EnableQuery]
    [HttpGet("odata/Pedidos/GetPrendasNuevoPedido")]
    [Authorize]

    public async Task<ActionResult> GetPrendasNuevoPedido([FromODataUri] int idEntidad)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        return Ok(get_prendasNuevoPedido(idEntidad, idUsuario));
    }

    public IEnumerable<dynamic> get_prendasNuevoPedido([FromODataUri] int idEntidad, [FromODataUri] int idUsuario)
    {
        var objEntidad = db.tblEntidad
         .Where(x => x.idEntidad.Equals(idEntidad))
         .Select(x => new
         {
             x.idCompañia,
             x.isTodasPrendaNNuevoPedido_compañia,
             x.isTodasPrendaNNuevoPedido_entidad,
             idPrendaNavigation = x.tblPrendaNEntidad_NuevoPedido
                .Select(x => new tblPrenda { idPrenda = x.idPrenda, idEntidad = x.idPrendaNavigation.idEntidad, idCompañia = x.idPrendaNavigation.idCompañia }),
         }).FirstOrDefault();

        if (objEntidad == null) return Array.Empty<tblPrenda>().ToList();

        List<int> tblPrendaNUsuarioNEntidad_idPrendaFiltrados = db.tblPrendaNUsuarioNEntidad.Where(x => x.idUsuario == idUsuario && x.idEntidad == idEntidad).Select(x => x.idPrenda).ToList();

        List<tblPrenda> idsPrendaNEntidad_NuevoPedido = new List<tblPrenda>();
        idsPrendaNEntidad_NuevoPedido.AddRange(objEntidad.idPrendaNavigation);

        var prendasEntidad = idsPrendaNEntidad_NuevoPedido.Where(y => y.idEntidad == idEntidad).Select(y => y.idPrenda).ToList();
        var prendasCompañia = idsPrendaNEntidad_NuevoPedido.Where(y => y.idCompañia == objEntidad.idCompañia).Select(y => y.idPrenda).ToList();

        var prendas = db.tblPrenda.Where(x =>
            (
                (tblPrendaNUsuarioNEntidad_idPrendaFiltrados.Count() > 0 && tblPrendaNUsuarioNEntidad_idPrendaFiltrados.Contains(x.idPrenda)) ||
                tblPrendaNUsuarioNEntidad_idPrendaFiltrados.Count() == 0
            ) &&
            x.activo.Equals(true) &&
            x.eliminado.Equals(false)
             && (
                 (objEntidad.isTodasPrendaNNuevoPedido_compañia == true && x.idCompañia == objEntidad.idCompañia) ||
                 (objEntidad.isTodasPrendaNNuevoPedido_entidad == true && x.idEntidad == idEntidad) ||
                 (objEntidad.isTodasPrendaNNuevoPedido_entidad == null && prendasEntidad.Contains(x.idPrenda)) ||
                 (objEntidad.isTodasPrendaNNuevoPedido_compañia == null && prendasCompañia.Contains(x.idPrenda))
            ))
                .OrderBy(x => x.codigoPrenda)
        .Select(
            x => new
            {
                x.idPrenda,
                x.codigoPrenda,
                x.denominacion,
                x.udsXBacPedido,
                x.udsXBacReparto, //Se usa en AppLogisticaInterna (Modificar repartos)
                x.idEntidad,
                stockDefinido = x.tblPrendaNEntidad_NuevoPedido.Where(x => x.idEntidad == idEntidad).Select(x => x.stockDefinido).FirstOrDefault(),
                colorTapa_hexadecimal = x.idColorTapaNavigation != null ? x.idColorTapaNavigation.codigoHexadecimal : (string?)null,
                marcaTapa = x.idMarcaTapaNavigation != null ? x.idMarcaTapaNavigation.marca : (string?)null,
                codigoElemTransPedido = x.elementoPedidoNavigation != null ? x.elementoPedidoNavigation.codigo : null,
                denoElemTransPedido = x.elementoPedidoNavigation != null ? x.elementoPedidoNavigation.denominacion : null
            });

        return prendas;
    }

    public IEnumerable<dynamic> get_prendasNuevoPedido_v2([FromODataUri] int idEntidad, int idUsuario)
    {

        var objEntidad = db.tblEntidad
         .Where(x => x.idEntidad.Equals(idEntidad))
         .Select(x => new
         {
             x.idCompañia,
             x.isTodasPrendaNNuevoPedido_compañia,
             x.isTodasPrendaNNuevoPedido_entidad,
             idPrendaNavigation = x.tblPrendaNEntidad_NuevoPedido.Select(x => new tblPrenda { idPrenda = x.idPrenda, idEntidad = x.idPrendaNavigation.idEntidad, idCompañia = x.idPrendaNavigation.idCompañia }),
         }).FirstOrDefault();

        if (objEntidad == null) return Array.Empty<tblPrenda>().ToList();

        List<int> tblPrendaNUsuarioNEntidad_idPrendaFiltrados = db.tblPrendaNUsuarioNEntidad.Where(x => x.idUsuario == idUsuario && x.idEntidad == idEntidad).Select(x => x.idPrenda).ToList();

        List<tblPrenda> idsPrendaNEntidad_NuevoPedido = new List<tblPrenda>();
        idsPrendaNEntidad_NuevoPedido.AddRange(objEntidad.idPrendaNavigation);

        var prendasEntidad = idsPrendaNEntidad_NuevoPedido.Where(y => y.idEntidad == idEntidad).Select(y => y.idPrenda).ToList();
        var prendasCompañia = idsPrendaNEntidad_NuevoPedido.Where(y => y.idCompañia == objEntidad.idCompañia).Select(y => y.idPrenda).ToList();

        var prendas = db.tblPrenda.Where(x =>
            (
                (tblPrendaNUsuarioNEntidad_idPrendaFiltrados.Count() > 0 && tblPrendaNUsuarioNEntidad_idPrendaFiltrados.Contains(x.idPrenda)) ||
                tblPrendaNUsuarioNEntidad_idPrendaFiltrados.Count() == 0
            ) &&
            x.activo.Equals(true) &&
            x.eliminado.Equals(false)
             && (
                 (objEntidad.isTodasPrendaNNuevoPedido_compañia == true && x.idCompañia == objEntidad.idCompañia) ||
                 (objEntidad.isTodasPrendaNNuevoPedido_entidad == true && x.idEntidad == idEntidad) ||
                 (objEntidad.isTodasPrendaNNuevoPedido_entidad == null && prendasEntidad.Contains(x.idPrenda)) ||
                 (objEntidad.isTodasPrendaNNuevoPedido_compañia == null && prendasCompañia.Contains(x.idPrenda))
            ))
                .OrderBy(x => x.codigoPrenda)
        .Select(
            x => new
            {
                x.idPrenda,
                x.codigoPrenda,
                x.denominacion,
                x.udsXBacPedido,
                x.udsXBacReparto, //Se usa en AppLogisticaInterna (Modificar repartos)
                x.idEntidad,
                stockDefinido = x.tblPrendaNEntidad_NuevoPedido.Where(x => x.idEntidad == idEntidad).Select(x => x.stockDefinido).FirstOrDefault(),
                colorTapa = x.idColorTapaNavigation != null ? x.idColorTapaNavigation.codigoHexadecimal : (string?)null,
                marca = x.idMarcaTapaNavigation != null ? x.idMarcaTapaNavigation.marca : (string?)null,
                codigoElemTransPedido = x.elementoPedidoNavigation != null ? x.elementoPedidoNavigation.codigo : null,
                denoElemTransPedido = x.elementoPedidoNavigation != null ? x.elementoPedidoNavigation.denominacion : null
            });

        return prendas;
    }
}
