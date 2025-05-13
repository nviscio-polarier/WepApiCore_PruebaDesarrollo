using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.Administracion.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.Administracion;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.Administracion;

public class AdmPedidoProveedorController : ODataController
{
    private readonly bdERP db;
    private readonly AdministracionController ac;
    public AdmPedidoProveedorController(bdERP context)
    {
        db = context;
        ac = new (context);
    }

    [HttpPost]
    [Route("odata/Administracion/postAdmPedidoProveedor")]
    [Authorize]
    public async Task<ActionResult> PostAdmPedidoProveedor([FromBody] PedidoProveedorConArticulos pedidoProveedor)
    {
        //Array para almacenar las entities que se van a insertar
        tblAdmPedidoProveedor nuevoPedidoProveedor;
        List<tblAdmArticulo?>? listaArticulosAInsertar = null;

        int? idCentroTrabajo = null;
        int? idLavanderia = null;

        if(pedidoProveedor.idCentro != null)
        {
            var idCentroSplitted = pedidoProveedor.idCentro.Split("_", 2);
            if (int.Parse(idCentroSplitted[0]) == 0)
            {
                idCentroTrabajo = int.Parse(idCentroSplitted[1]);
            } else
            {
                idLavanderia = int.Parse(idCentroSplitted[1]);
            }
        }

        //Insercion o actualizacion de pedido proveedor
        tblAdmPedidoProveedor? pedidoProveedorExistente = db.tblAdmPedidoProveedor
            .Include(x => x.tblArticuloNAdmPedidoProveedor)
            .Where(x => x.idAdmPedidoProveedor == pedidoProveedor.idAdmPedidoProveedor)
            .FirstOrDefault();
        if (pedidoProveedorExistente == null)
        {
            //Se trata de una inserción
            nuevoPedidoProveedor = new()
            {
                //idAdmPedidoProveedor = pedidoProveedor.idAdmPedidoProveedor,
                idTipoPedido = pedidoProveedor.idTipoPedido,
                codigo = ac.GetFullCode("pedidoProveedor", pedidoProveedor.idEmpresaPolarier, pedidoProveedor.idTipoPedido, pedidoProveedor.fechaCreacion),//pedidoProveedor.codigo,
                fechaCreacion = pedidoProveedor.fechaCreacion,
                idAdmPedido_Estado = pedidoProveedor.idAdmPedido_Estado,
                idMoneda = pedidoProveedor.idMoneda,
                idAdmProveedor = pedidoProveedor.idAdmProveedor,
                tasaCambio = pedidoProveedor.tasaCambio == 0 ? 1 : pedidoProveedor.tasaCambio,
                idAdmFormaPago = pedidoProveedor.idAdmFormaPago,
                idAdmCentroCoste = pedidoProveedor.idAdmCentroCoste,
                idAdmElementoPEP = pedidoProveedor.idAdmElementoPEP,
                direccionEntrega = pedidoProveedor.direccionEntrega,
                idCentroTrabajo = idCentroTrabajo,
                idLavanderia = idLavanderia,
                descuento = pedidoProveedor.descuento,
                idAdmTipoDescuento = pedidoProveedor.idAdmTipoDescuento,
                idIncoterm = pedidoProveedor.idIncoterm,
                fechaEstimadaRecepcion = pedidoProveedor.fechaEstimadaRecepcion,
                numPresupuestoProveedor = pedidoProveedor.numPresupuestoProveedor,
                observaciones = pedidoProveedor.observaciones,
                idEmpresaPolarier = pedidoProveedor.idEmpresaPolarier,
                idAdmTipoCambio = pedidoProveedor.idAdmTipoCambio,
                tblArticuloNAdmPedidoProveedor = pedidoProveedor.tblArticuloNAdmPedidoProveedor
            };
            db.tblAdmPedidoProveedor.Add(nuevoPedidoProveedor);
            db.SaveChanges();
        }
        else
        {
            //Se trata de una actualización
            //idAdmPedidoProveedor = pedidoProveedor.idAdmPedidoProveedor,
            pedidoProveedorExistente.codigo = pedidoProveedorExistente.idTipoPedido != pedidoProveedor.idTipoPedido
                ? (ac.GetFullCode("pedidoProveedor", pedidoProveedor.idEmpresaPolarier, pedidoProveedor.idTipoPedido, pedidoProveedor.fechaCreacion) ?? pedidoProveedor.codigo)
                : pedidoProveedor.codigo;
            pedidoProveedorExistente.idTipoPedido = pedidoProveedor.idTipoPedido;
            pedidoProveedorExistente.fechaCreacion = pedidoProveedor.fechaCreacion;
            pedidoProveedorExistente.idAdmPedido_Estado = pedidoProveedor.idAdmPedido_Estado;
            pedidoProveedorExistente.idMoneda = pedidoProveedor.idMoneda;
            pedidoProveedorExistente.idAdmProveedor = pedidoProveedor.idAdmProveedor;
            pedidoProveedorExistente.tasaCambio = pedidoProveedor.tasaCambio == 0 ? 1 : pedidoProveedor.tasaCambio;
            pedidoProveedorExistente.idAdmFormaPago = pedidoProveedor.idAdmFormaPago;
            pedidoProveedorExistente.idAdmCentroCoste = pedidoProveedor.idAdmCentroCoste;
            pedidoProveedorExistente.idAdmElementoPEP = pedidoProveedor.idAdmElementoPEP;
            pedidoProveedorExistente.direccionEntrega = pedidoProveedor.direccionEntrega;
            pedidoProveedorExistente.idCentroTrabajo = idCentroTrabajo;
            pedidoProveedorExistente.idLavanderia = idLavanderia;
            pedidoProveedorExistente.descuento = pedidoProveedor.descuento;
            pedidoProveedorExistente.idAdmTipoDescuento = pedidoProveedor.idAdmTipoDescuento;
            pedidoProveedorExistente.idIncoterm = pedidoProveedor.idIncoterm;
            pedidoProveedorExistente.fechaEstimadaRecepcion = pedidoProveedor.fechaEstimadaRecepcion;
            pedidoProveedorExistente.numPresupuestoProveedor = pedidoProveedor.numPresupuestoProveedor;
            pedidoProveedorExistente.observaciones = pedidoProveedor.observaciones;
            pedidoProveedorExistente.idEmpresaPolarier = pedidoProveedor.idEmpresaPolarier;
            pedidoProveedorExistente.idAdmTipoCambio = pedidoProveedor.idAdmTipoCambio;
            db.tblArticuloNAdmPedidoProveedor.RemoveRange(pedidoProveedorExistente.tblArticuloNAdmPedidoProveedor);
            pedidoProveedor.tblArticuloNAdmPedidoProveedor.ForEach(item => item.idAdmPedidoProveedor = pedidoProveedorExistente.idAdmPedidoProveedor);
            pedidoProveedorExistente.tblArticuloNAdmPedidoProveedor = pedidoProveedor.tblArticuloNAdmPedidoProveedor;

            db.tblAdmPedidoProveedor.Update(pedidoProveedorExistente);
            db.SaveChanges();
            nuevoPedidoProveedor = pedidoProveedorExistente;
        }
        return Ok(nuevoPedidoProveedor);
    }
}
