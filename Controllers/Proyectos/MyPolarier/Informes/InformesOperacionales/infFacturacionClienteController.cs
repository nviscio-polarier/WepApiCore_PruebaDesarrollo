using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class infFacturacionClienteController : ODataController
{
    private readonly bdERP db;
    public infFacturacionClienteController(bdERP context)
    {
        db = context;
    }


    [EnableQuery]
    [HttpGet("odata/Informes/InformesOperacionales/InfFacturacionCliente/prendaFaltanDatos")]
    [Authorize]
    public async Task<ActionResult> prendaFaltanDatos([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        var entidadNLavanderia = db.tblEntidad
            .Where(x => x.activo == true &&
                        x.eliminado == false &&
                        x.idLavanderia.Any(l => l.idLavanderia == idLavanderia))
            .Select(e => e.idEntidad)
            .ToList();

        var repartos = db.tblReparto
            .Where(x => x.fecha.Value.Date >= fechaDesde &&
                        x.fecha.Value.Date <= fechaHasta &&
                        x.idRepartoEstado == 3 &&
                        x.idPedidoNavigation.idTipoProduccionNavigation.codigo != 1 &&
                        x.idPedidoNavigation.idTipoProduccionNavigation.codigo != 6 &&
                        entidadNLavanderia.Contains((int)x.idEntidad))
            .Include(x => x.tblPrendaNReparto)
            .Include(x => x.idEntidadNavigation)
            .ToList();

        List<auxPrendas> auxPrenda = new List<auxPrendas>();

        foreach (var rep in repartos)
        {
            foreach (var prenda in rep.tblPrendaNReparto)
            {
                var cantidadCalculada = (int)(prenda.cantidad +
                    (rep.idEntidadNavigation.reparteRechazoRetiro == 2 ? 0 : prenda.rechazo) +
                    (rep.idEntidadNavigation.reparteRechazoRetiro == 1 ? 0 : prenda.retiro));

                var prendaExist = auxPrenda.FirstOrDefault(x => x.idPrenda == prenda.idPrenda);

                if (prendaExist == null)
                {
                    auxPrenda.Add(new auxPrendas
                    {
                        cantidad = cantidadCalculada,
                        idPrenda = prenda.idPrenda,
                        idsEntidad = new List<int> { (int)rep.idEntidad }
                    });
                }
                else
                {
                    prendaExist.cantidad += cantidadCalculada;
                    prendaExist.idsEntidad.Add((int)rep.idEntidad);
                }
            }
        }


        var repartosOffice = db.tblRepartoOffice
            .Where(x => x.fecha.Date >= fechaDesde &&
                        x.fecha.Date <= fechaHasta &&
                        entidadNLavanderia.Contains((int)x.idEntidad)
                        )
            .Include(x => x.tblPrendaNRepartoOffice)
            .ToList();


        foreach (var ro in repartosOffice)
        {
            foreach (var pro in ro.tblPrendaNRepartoOffice)
            {
                var prendaExist = auxPrenda.FirstOrDefault(x => x.idPrenda == pro.idPrenda);

                if (prendaExist == null)
                {
                    auxPrenda.Add(new auxPrendas
                    {
                        cantidad = (int)pro.cantidad,
                        idPrenda = pro.idPrenda,
                        idsEntidad = new List<int> { (int)ro.idEntidad }
                    });
                }
                else
                {
                    prendaExist.cantidad += (int)pro.cantidad;
                    prendaExist.idsEntidad.Add((int)ro.idEntidad);
                }
            }
        }

        var abonos = db.tblAbono
            .Where(x => x.fecha.Value.Date >= fechaDesde &&
                        x.fecha.Value.Date <= fechaHasta &&
                       (x.idTipoAbono == 1 || x.idTipoAbono == 2) &&
                        entidadNLavanderia.Contains((int)x.idEntidad))
            .Include(x => x.tblPrendaNAbono)
            .ToList();

        foreach (var ab in abonos)
        {
            foreach (var pab in ab.tblPrendaNAbono)
            {
                var prendaExist = auxPrenda.FirstOrDefault(x => x.idPrenda == pab.idPrenda);

                if (prendaExist == null)
                {
                    auxPrenda.Add(new auxPrendas
                    {
                        cantidad = (int)pab.cantidad,
                        idPrenda = pab.idPrenda,
                        idsEntidad = new List<int> { (int)ab.idEntidad }
                    });
                }
                else
                {
                    prendaExist.cantidad += (int)pab.cantidad;
                    prendaExist.idsEntidad.Add((int)ab.idEntidad);
                }
            }
        }

        var idsPrenda = auxPrenda
            .Where(x => x.cantidad > 0)
            .Select(x => x.idPrenda).ToList();

        var tblPrenda = db.tblPrenda
            .Where(x => idsPrenda.Contains(x.idPrenda))
            .Include(x => x.tblPrenda_historico_peso)
            .Include(x => x.tblPrenda_historico_idTipoFacturacion)
            .Include(x => x.tblPrecioLavadoPrenda)
            .ToList();

        var formatData = tblPrenda.Select(x => new PrendaFaltanDatos
        {
            idPrenda = x.idPrenda,
            idsEntidad = auxPrenda.FirstOrDefault(y => y.idPrenda == x.idPrenda)?.idsEntidad.Distinct().ToList(),
            idCompañia = x.idCompañia != null ? x.idCompañia : x?.idEntidadNavigation?.idCompañia,
            hasPrecio = x.tblPrecioLavadoPrenda.Where(x => x.fecha.Date <= fechaHasta).OrderByDescending(x => x.fecha).FirstOrDefault() != null &&
                        x.tblPrecioLavadoPrenda.Where(x => x.fecha.Date <= fechaHasta).OrderByDescending(x => x.fecha).FirstOrDefault()?.precio != 0,
            hasPeso = x.tblPrenda_historico_peso.Where(x => x.fecha.Date <= fechaHasta).OrderByDescending(x => x.fecha).FirstOrDefault() != null &&
                      x.tblPrenda_historico_peso.Where(x => x.fecha.Date <= fechaHasta).OrderByDescending(x => x.fecha).FirstOrDefault()?.peso != 0,
            hasTipoFacturacion = x.tblPrenda_historico_idTipoFacturacion.Where(x => x.fecha.Date <= fechaHasta).OrderByDescending(x => x.fecha).FirstOrDefault() != null,
            idTipoFacturacion = x.tblPrenda_historico_idTipoFacturacion.Where(x => x.fecha.Date <= fechaHasta)?.OrderByDescending(x => x.fecha)?.FirstOrDefault()?.idTipoFacturacion
        });

        var filteredFormatData = formatData
                                .Where(x => x.hasPeso == false || //Sin peso
                                            (x.hasPrecio == false && x.idTipoFacturacion != 2) || // Sin precio y no es por estancia
                                            x.hasTipoFacturacion == false) // Sin tipo facturación
                                .ToList();

        return Ok(filteredFormatData);
    }


    [EnableQuery]
    [HttpGet("odata/Informes/InformesOperacionales/InfFacturacionCliente/EF_infFacturacion_Agrupado_Prendas")]
    [Authorize]
    public async Task<ActionResult> EF_infFacturacion_Agrupado_Prendas([FromODataUri] int idEntidad, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] string idsRepartos, [FromODataUri] string idsRepartosOffice, [FromODataUri] string idsProducciones, [FromODataUri] string idsAbonos)
    {
        //Si no hay filtro se envia todo NULL, pero si hay un solo filtro los demás se envian como "" para que no los confunda con null y solo devuelva los registros filtrados.
        bool hayFiltro = (idsRepartos.Length > 0 || idsRepartosOffice.Length > 0 || idsProducciones.Length > 0 || idsAbonos.Length > 0);

        var idsRep = !hayFiltro ? null : idsRepartos.Length > 0 ? idsRepartos : "";
        var idsRepOf = !hayFiltro ? null : idsRepartosOffice.Length > 0 ? idsRepartosOffice : "";
        var idsProd = !hayFiltro ? null : idsProducciones.Length > 0 ? idsProducciones : "";
        var idsAbo = !hayFiltro ? null : idsAbonos.Length > 0 ? idsAbonos : "";

        var connection = db.Database.GetDbConnection();
        var results = await connection.QueryAsync("EXEC [MyReporting].[EF_infFacturacion_Agrupado_Prendas] @idEntidad, @fechaDesde, @fechaHasta, @idsRepartos, @idsRepartosOffice, @idsProducciones, @idsAbonos",
            new { idEntidad, fechaDesde, fechaHasta, idsRepartos = idsRep, idsRepartosOffice = idsRepOf, idsProducciones = idsProd, idsAbonos = idsAbo });

        return Ok(results);
    }


    class PrendaFaltanDatos
    {
        public int idPrenda { get; set; }
        public List<int>? idsEntidad { get; set; }
        public int? idCompañia { get; set; }
        public bool hasPrecio { get; set; }
        public bool hasPeso { get; set; }
        public bool hasTipoFacturacion { get; set; }
        public int? idTipoFacturacion { get; set; }
    }

    class auxPrendas
    {
        public int cantidad { get; set; }
        public int idPrenda { get; set; }
        public List<int>? idsEntidad { get; set; }
    }
}