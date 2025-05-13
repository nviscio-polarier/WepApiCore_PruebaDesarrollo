using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.Administracion.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.Administracion;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.Administracion;

public class AdmPedidoClienteController : ODataController
{
    private readonly bdERP db;
    private readonly AdministracionController ac;
    public AdmPedidoClienteController(bdERP context)
    {
        db = context;
        ac = new (context);
    }

    [HttpPost]
    [Route("odata/Administracion/postAdmPedidoCliente")]
    [Authorize]
    public async Task<ActionResult> PostAdmPedidoCliente([FromBody] PedidoClienteConArticulos pedidoCliente)
    {
        //Array para almacenar las entities que se van a insertar
        tblAdmPedidoCliente nuevoPedidoCliente;
        List<tblAdmArticulo?>? listaArticulosAInsertar = null;

        //Insercion o actualizacion de pedido cliente
        tblAdmPedidoCliente? pedidoClienteExistente = db.tblAdmPedidoCliente
            .Include(x => x.tblArticuloNAdmPedidoCliente)
            .Where(x => x.idAdmPedidoCliente == pedidoCliente.idAdmPedidoCliente)
            .FirstOrDefault();
        if (pedidoClienteExistente == null)
        {
            //Se trata de una inserción
            nuevoPedidoCliente = new()
            {
                //idAdmPedidoCliente = pedidoCliente.idAdmPedidoCliente,
                codigo = ac.GetFullCode("pedidoCliente", pedidoCliente.idEmpresaPolarier, pedidoCliente.idTipoPedido, pedidoCliente.fechaCreacion),//pedidoCliente.codigo,
                fechaCreacion = pedidoCliente.fechaCreacion,
                idAdmPedido_Estado = pedidoCliente.idAdmPedido_Estado,
                idMoneda = pedidoCliente.idMoneda,
                idAdmCliente = pedidoCliente.idAdmCliente,
                tasaCambio = pedidoCliente.tasaCambio == 0 ? 1 : pedidoCliente.tasaCambio,
                numPedidoCliente = pedidoCliente.numPedidoCliente,
                idAdmFormaPago = pedidoCliente.idAdmFormaPago,
                idTipoPedido = pedidoCliente.idTipoPedido,
                idAdmCentroCoste = pedidoCliente.idAdmCentroCoste,
                idAdmElementoPEP = pedidoCliente.idAdmElementoPEP,
                idIncoterm = pedidoCliente.idIncoterm,
                descuento = pedidoCliente.descuento,
                idAdmTipoDescuento = pedidoCliente.idAdmTipoDescuento,
                idAdmPresupuestoVenta = pedidoCliente.idAdmPresupuestoVenta,
                observaciones = pedidoCliente.observaciones,
                idIvaNPais = pedidoCliente.idIvaNPais,
                idEmpresaPolarier = pedidoCliente.idEmpresaPolarier,
                idAdmTipoCambio = pedidoCliente.idAdmTipoCambio,
                tblArticuloNAdmPedidoCliente = pedidoCliente.tblArticuloNAdmPedidoCliente
            };
            db.tblAdmPedidoCliente.Add(nuevoPedidoCliente);
            db.SaveChanges();
        }
        else
        {
            //Se trata de una actualización
            //idAdmPedidoCliente = pedidoCliente.idAdmPedidoCliente,
            pedidoClienteExistente.codigo = pedidoClienteExistente.idTipoPedido != pedidoCliente.idTipoPedido
                ? (ac.GetFullCode("pedidoCliente", pedidoCliente.idEmpresaPolarier, pedidoCliente.idTipoPedido, pedidoCliente.fechaCreacion) ?? pedidoCliente.codigo)
                : pedidoCliente.codigo;
            pedidoClienteExistente.fechaCreacion = pedidoCliente.fechaCreacion;
            pedidoClienteExistente.idAdmPedido_Estado = pedidoCliente.idAdmPedido_Estado;
            pedidoClienteExistente.idMoneda = pedidoCliente.idMoneda;
            pedidoClienteExistente.idAdmCliente = pedidoCliente.idAdmCliente;
            pedidoClienteExistente.tasaCambio = pedidoCliente.tasaCambio == 0 ? 1 : pedidoCliente.tasaCambio;
            pedidoClienteExistente.numPedidoCliente = pedidoCliente.numPedidoCliente;
            pedidoClienteExistente.idAdmFormaPago = pedidoCliente.idAdmFormaPago;
            pedidoClienteExistente.idTipoPedido = pedidoCliente.idTipoPedido;
            pedidoClienteExistente.idAdmCentroCoste = pedidoCliente.idAdmCentroCoste;
            pedidoClienteExistente.idAdmElementoPEP = pedidoCliente.idAdmElementoPEP;
            pedidoClienteExistente.idIncoterm = pedidoCliente.idIncoterm;
            pedidoClienteExistente.descuento = pedidoCliente.descuento;
            pedidoClienteExistente.idAdmTipoDescuento = pedidoCliente.idAdmTipoDescuento;
            pedidoClienteExistente.idAdmPresupuestoVenta = pedidoCliente.idAdmPresupuestoVenta;
            pedidoClienteExistente.observaciones = pedidoCliente.observaciones;
            pedidoClienteExistente.idIvaNPais = pedidoCliente.idIvaNPais;
            pedidoClienteExistente.idEmpresaPolarier = pedidoCliente.idEmpresaPolarier;
            pedidoClienteExistente.idAdmTipoCambio = pedidoCliente.idAdmTipoCambio;
            db.tblArticuloNAdmPedidoCliente.RemoveRange(pedidoClienteExistente.tblArticuloNAdmPedidoCliente);
            pedidoCliente.tblArticuloNAdmPedidoCliente.ForEach(item => item.idAdmPedidoCliente = pedidoClienteExistente.idAdmPedidoCliente);
            pedidoClienteExistente.tblArticuloNAdmPedidoCliente = pedidoCliente.tblArticuloNAdmPedidoCliente;

            db.tblAdmPedidoCliente.Update(pedidoClienteExistente);
            db.SaveChanges();
            nuevoPedidoCliente = pedidoClienteExistente;
        }
        return Ok(nuevoPedidoCliente);
    }
}
