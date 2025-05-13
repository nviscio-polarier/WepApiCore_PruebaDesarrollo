using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.Administracion.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.Administracion;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.Administracion;

public class AdmAlbaranVentaController : ODataController
{
    private readonly bdERP db;
    private readonly AdministracionController ac;
    public AdmAlbaranVentaController(bdERP context)
    {
        db = context;
        ac = new (context);
    }

    [HttpPost]
    [Route("odata/Administracion/postAdmAlbaranVenta")]
    [Authorize]
    public async Task<ActionResult> PostAdmAlbaranVenta([FromBody] AlbaranVentaConArticulos albaranVenta)
    {
        //Array para almacenar las entities que se van a insertar
        tblAdmAlbaranVenta nuevoAlbaranVenta;
        List<tblAdmArticulo?>? listaArticulosAInsertar = null;

        //Insercion o actualizacion de albaran de venta
        tblAdmAlbaranVenta? albaranVentaExistente = db.tblAdmAlbaranVenta
            .Include(x => x.tblArticuloNAdmAlbaranVenta)
            .Where(x => x.idAdmAlbaranVenta == albaranVenta.idAdmAlbaranVenta)
            .FirstOrDefault();
        if (albaranVentaExistente == null)
        {
            //Se trata de una inserción
            nuevoAlbaranVenta = new()
            {
                //idAdmAlbaranVenta = albaranVenta.idAdmAlbaranVenta,
                codigo = ac.GetFullCode("albaranVenta", albaranVenta.idEmpresaPolarier, albaranVenta.idTipoAlbaran, albaranVenta.fechaCreacion),//albaranVenta.codigo,
                fechaCreacion = albaranVenta.fechaCreacion,
                idTipoAlbaran = albaranVenta.idTipoAlbaran,
                idAdmAlbaran_Estado = albaranVenta.idAdmAlbaran_Estado,
                idAdmCliente = albaranVenta.idAdmCliente,
                idMoneda = albaranVenta.idMoneda,
                tasaCambio = albaranVenta.tasaCambio == 0 ? 1 : albaranVenta.tasaCambio,
                descuento = albaranVenta.descuento,
                idAdmTipoDescuento = albaranVenta.idAdmTipoDescuento,
                idAdmFormaPago = albaranVenta.idAdmFormaPago,
                idAdmPedidoCliente = albaranVenta.idAdmPedidoCliente,
                idTipoFactura = albaranVenta.idTipoFactura,
                observaciones = albaranVenta.observaciones,
                idIvaNPais = albaranVenta.idIvaNPais,
                idAdmTipoCambio = albaranVenta.idAdmTipoCambio,
                idEmpresaPolarier = albaranVenta.idEmpresaPolarier,
                idAdmCentroCoste = albaranVenta.idAdmCentroCoste,
                idAdmElementoPEP = albaranVenta.idAdmElementoPEP,
                idIncoterm = albaranVenta.idIncoterm,
                tblArticuloNAdmAlbaranVenta = albaranVenta.tblArticuloNAdmAlbaranVenta
            };
            db.tblAdmAlbaranVenta.Add(nuevoAlbaranVenta);
            db.SaveChanges();
        }
        else
        {
            //Se trata de una actualización
            //albaranVentaExistente.idAdmAlbaranVenta = albaranVenta.idAdmAlbaranVenta;
            albaranVentaExistente.codigo = albaranVentaExistente.idTipoAlbaran != albaranVenta.idTipoAlbaran
                ? (ac.GetFullCode("albaranVenta", albaranVenta.idEmpresaPolarier, albaranVenta.idTipoAlbaran, albaranVenta.fechaCreacion) ?? albaranVenta.codigo)
                : albaranVenta.codigo;
            albaranVentaExistente.fechaCreacion = albaranVenta.fechaCreacion;
            albaranVentaExistente.idTipoAlbaran = albaranVenta.idTipoAlbaran;
            albaranVentaExistente.idAdmAlbaran_Estado = albaranVenta.idAdmAlbaran_Estado;
            albaranVentaExistente.idAdmCliente = albaranVenta.idAdmCliente;
            albaranVentaExistente.idMoneda = albaranVenta.idMoneda;
            albaranVentaExistente.tasaCambio = albaranVenta.tasaCambio == 0 ? 1 : albaranVenta.tasaCambio;
            albaranVentaExistente.descuento = albaranVenta.descuento;
            albaranVentaExistente.idAdmTipoDescuento = albaranVenta.idAdmTipoDescuento;
            albaranVentaExistente.idAdmFormaPago = albaranVenta.idAdmFormaPago;
            albaranVentaExistente.idAdmPedidoCliente = albaranVenta.idAdmPedidoCliente;
            albaranVentaExistente.idTipoFactura = albaranVenta.idTipoFactura;
            albaranVentaExistente.observaciones = albaranVenta.observaciones;
            albaranVentaExistente.idIvaNPais = albaranVenta.idIvaNPais;
            albaranVentaExistente.idAdmTipoCambio = albaranVenta.idAdmTipoCambio;
            albaranVentaExistente.idEmpresaPolarier = albaranVenta.idEmpresaPolarier;
            albaranVentaExistente.idAdmCentroCoste = albaranVenta.idAdmCentroCoste;
            albaranVentaExistente.idAdmElementoPEP = albaranVenta.idAdmElementoPEP;
            albaranVentaExistente.idIncoterm = albaranVenta.idIncoterm;
            db.tblArticuloNAdmAlbaranVenta.RemoveRange(albaranVentaExistente.tblArticuloNAdmAlbaranVenta);
            albaranVenta.tblArticuloNAdmAlbaranVenta.ForEach(item => item.idAdmAlbaranVenta = albaranVentaExistente.idAdmAlbaranVenta);
            albaranVentaExistente.tblArticuloNAdmAlbaranVenta = albaranVenta.tblArticuloNAdmAlbaranVenta;

            db.tblAdmAlbaranVenta.Update(albaranVentaExistente);
            db.SaveChanges();
            nuevoAlbaranVenta = albaranVentaExistente;
        }
        return Ok(nuevoAlbaranVenta);
    }
}
