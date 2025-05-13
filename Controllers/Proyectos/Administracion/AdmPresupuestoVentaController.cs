using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.Administracion.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.Administracion;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class AdmPresupuestoVentaController : ODataController
{
    private readonly bdERP db;
    private readonly AdministracionController ac;
    public AdmPresupuestoVentaController(bdERP context)
    {
        db = context;
        ac = new (context);
    }

    //[EnableQuery]
    [HttpPost]
    [Route("odata/Administracion/postAdmPresupuestoVenta")]
    [Authorize]
    public async Task<ActionResult> PostAdmPresupuestoVenta([FromBody] PresupuestoVentaConArticulos presupuestoVenta)
    {
        //Array para almacenar las entities que se van a insertar
        tblAdmPresupuestoVenta nuevoPresupuestoVenta;
        List<tblAdmArticulo?>? listaArticulosAInsertar = null;

        //Insercion o actualizacion de presupuesto de venta
        tblAdmPresupuestoVenta? presupuestoVentaExistente = db.tblAdmPresupuestoVenta
            .Include(x => x.tblArticuloNAdmPresupuestoVenta)
            .Where(x => x.idAdmPresupuestoVenta == presupuestoVenta.idAdmPresupuestoVenta)
            .FirstOrDefault();

        if (presupuestoVentaExistente == null)
        {
            //Se trata de una inserción
            nuevoPresupuestoVenta = new()
            {
                codigo = ac.GetFullCode("presupuestoVenta", presupuestoVenta.idEmpresaPolarier, presupuestoVenta.idTipoPresupuesto, presupuestoVenta.fechaCreacion),//  presupuestoVenta.codigo,
                idAdmCliente = presupuestoVenta.idAdmCliente,
                fechaCreacion = presupuestoVenta.fechaCreacion,
                idMoneda = presupuestoVenta.idMoneda,
                tasaCambio = presupuestoVenta.tasaCambio == 0 ? 1 : presupuestoVenta.tasaCambio,
                idAdmFormaPago = presupuestoVenta.idAdmFormaPago,
                descuento = presupuestoVenta.descuento,
                idAdmTipoDescuento = presupuestoVenta.idAdmTipoDescuento,
                idTipoPresupuesto = presupuestoVenta.idTipoPresupuesto,
                observaciones = presupuestoVenta.observaciones,
                idAdmPresupuestoVenta_Estado = presupuestoVenta.idAdmPresupuestoVenta_Estado,
                idIvaNPais = presupuestoVenta.idIvaNPais,
                idEmpresaPolarier = presupuestoVenta.idEmpresaPolarier,
                idAdmTipoCambio = presupuestoVenta.idAdmTipoCambio,
                tblArticuloNAdmPresupuestoVenta = presupuestoVenta.tblArticuloNAdmPresupuestoVenta
            };
            db.tblAdmPresupuestoVenta.Add(nuevoPresupuestoVenta);
            db.SaveChanges();
        }
        else
        {
            //Se trata de una actualización
            presupuestoVentaExistente.codigo = presupuestoVentaExistente.idTipoPresupuesto != presupuestoVenta.idTipoPresupuesto
                ? (ac.GetFullCode("presupuestoVenta", presupuestoVenta.idEmpresaPolarier, presupuestoVenta.idTipoPresupuesto, presupuestoVenta.fechaCreacion) ?? presupuestoVenta.codigo)
                : presupuestoVenta.codigo;
            presupuestoVentaExistente.idAdmCliente = presupuestoVenta.idAdmCliente;
            presupuestoVentaExistente.fechaCreacion = presupuestoVenta.fechaCreacion;
            presupuestoVentaExistente.idMoneda = presupuestoVenta.idMoneda;
            presupuestoVentaExistente.tasaCambio = presupuestoVenta.tasaCambio == 0 ? 1 : presupuestoVenta.tasaCambio;
            presupuestoVentaExistente.idAdmFormaPago = presupuestoVenta.idAdmFormaPago;
            presupuestoVentaExistente.descuento = presupuestoVenta.descuento;
            presupuestoVentaExistente.idAdmTipoDescuento = presupuestoVenta.idAdmTipoDescuento;
            presupuestoVentaExistente.idTipoPresupuesto = presupuestoVenta.idTipoPresupuesto;
            presupuestoVentaExistente.observaciones = presupuestoVenta.observaciones;
            presupuestoVentaExistente.idAdmPresupuestoVenta_Estado = presupuestoVenta.idAdmPresupuestoVenta_Estado;
            presupuestoVentaExistente.idIvaNPais = presupuestoVenta.idIvaNPais;
            presupuestoVentaExistente.idEmpresaPolarier = presupuestoVenta.idEmpresaPolarier;
            presupuestoVentaExistente.idAdmTipoCambio = presupuestoVenta.idAdmTipoCambio;
            db.tblArticuloNAdmPresupuestoVenta.RemoveRange(presupuestoVentaExistente.tblArticuloNAdmPresupuestoVenta);
            presupuestoVenta.tblArticuloNAdmPresupuestoVenta.ForEach(item => item.idAdmPresupuestoVenta = presupuestoVentaExistente.idAdmPresupuestoVenta);
            presupuestoVentaExistente.tblArticuloNAdmPresupuestoVenta = presupuestoVenta.tblArticuloNAdmPresupuestoVenta;

            db.tblAdmPresupuestoVenta.Update(presupuestoVentaExistente);
            db.SaveChanges();
            nuevoPresupuestoVenta = presupuestoVentaExistente;
        }

        //Insercion de articulos
        //if (presupuestoVenta.articuloNPresupuestoVenta != null)
        //{

        //    //se hace una lista de los articulos que se van a insertar
        //    //se buscan los articulos que ya existen en la base de datos basandose en el código del articulo
        //    //Se insertan solo los articulos que no existen

        //    List<tblAdmArticulo?> listaArticulosNuevos = presupuestoVenta.articuloNPresupuestoVenta
        //        .Select(x => x.admArticulo)
        //        .ToList();

        //    if (listaArticulosNuevos != null && listaArticulosNuevos.Count > 0)
        //    {
        //        //Es un poco una tonteria usar una lista de articulos existentes si de normal va a encontrar entre 0 y 1 articulos, pero al depender de un campo que no es clave primaria, es posible que haya mas de un articulo con el mismo codigo
        //        listaArticulosAInsertar = listaArticulosNuevos;
        //        List<tblAdmArticulo> listaArticulosExistentes = db.tblAdmArticulo.Where(x => listaArticulosNuevos.Select(x => x.codigoArticulo).Contains(x.codigoArticulo)).ToList();

        //        if (listaArticulosExistentes.Count > 0)
        //        {
        //            // se buscan las diferencias entre los articulos que se quieren insertar y los que existen con el fin de saber cuales se deben insertar
        //            listaArticulosAInsertar = listaArticulosNuevos.Where(x => !listaArticulosExistentes.Select(y => y.codigoArticulo).Contains(x.codigoArticulo)).ToList();
        //        }
        //        if(listaArticulosAInsertar != null && listaArticulosAInsertar.Count > 0)
        //        {
        //            db.tblAdmArticulo.AddRange(listaArticulosAInsertar);
        //            db.SaveChanges();
        //        }
        //    }

        //    //Actualizacion de la tabla intermedia

        //    //Se buscan las relaciones que ya existen en la base de datos relacionadas con el documento
        //    //Si existen relaciones, se eliminan y se insertan las nuevas, sin mirar atrás
        //    //Si no existen relaciones, se insertan las nuevas directamente, sin mirar atrás otra vez

        //    List<tblAdmArticuloNAdmPresupuestoVenta> tablaIntermedia = db.tblAdmArticuloNAdmPresupuestoVenta
        //        .Where(x => x.idAdmPresupuestoVenta == nuevoPresupuestoVenta.idAdmPresupuestoVenta)
        //        .ToList();

        //    if(listaArticulosNuevos != null)
        //    {
        //        //obtenemos los artículos, ahora con id, que deberian estar asociados al pedido, pues estos pueden no haber sido insertados en este momento y por lo tanto las id no se han asignado
        //        List<tblAdmArticulo?> listaArticulosPorAsociar = db.tblAdmArticulo.Where(x => listaArticulosNuevos.Select(x => x.codigoArticulo).Contains(x.codigoArticulo)).ToList();

        //        presupuestoVenta.articuloNPresupuestoVenta.ForEach(x =>
        //        {
        //            x.admArticulo = listaArticulosPorAsociar.Where(y => y.codigoArticulo == x.admArticulo.codigoArticulo).FirstOrDefault();
        //        });

        //        List<tblAdmArticuloNAdmPresupuestoVenta> listaIntermediaAInsertar = presupuestoVenta.articuloNPresupuestoVenta
        //            .Select(x => new tblAdmArticuloNAdmPresupuestoVenta
        //            {
        //                idAdmArticulo = x.admArticulo.idAdmArticulo,
        //                idAdmPresupuestoVenta = nuevoPresupuestoVenta.idAdmPresupuestoVenta,
        //                cantidad = x.cantidad,
        //                descuento = x.descuento,
        //                precio = x.precio,
        //                iva = x.iva
        //            })
        //            .ToList();
        //        if (tablaIntermedia.Count > 0)
        //        {
        //            db.tblAdmArticuloNAdmPresupuestoVenta.RemoveRange(tablaIntermedia);
        //            db.tblAdmArticuloNAdmPresupuestoVenta.AddRange(listaIntermediaAInsertar);
        //        } else
        //        {
        //            db.tblAdmArticuloNAdmPresupuestoVenta.AddRange(listaIntermediaAInsertar);
        //        }
        //    } else
        //    {
        //        if(tablaIntermedia.Count > 0)
        //        {
        //              db.tblAdmArticuloNAdmPresupuestoVenta.RemoveRange(tablaIntermedia);
        //        }
        //    }
        //    db.SaveChanges();
        //}
        return Ok(nuevoPresupuestoVenta);
    }
}