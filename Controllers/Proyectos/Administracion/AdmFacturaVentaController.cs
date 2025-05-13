using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.Administracion.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.Administracion;
using WebApiCore.Enums.Administracion;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.Administracion;

public class AdmFacturaVentaController : ODataController
{
    private readonly bdERP db;
    private readonly AdministracionController ac;
    private static readonly object _lockObject = new();

    public AdmFacturaVentaController(bdERP context)
    {
        db = context;
        ac = new (context);
    }

    [HttpPost]
    [Route("odata/Administracion/IU_AdmFacturaVenta")]
    [Authorize]
    public async Task<ActionResult> IU_AdmFacturaVenta([FromBody] FacturaVenta facturaVenta)
    {
        tblAdmFacturaVenta newFacturaVenta;

        var tblAdmAlbaranVenta = db.tblAdmAlbaranVenta
            .Where(ac => facturaVenta.idAdmAlbaranVenta != null && facturaVenta.idAdmAlbaranVenta.Contains(ac.idAdmAlbaranVenta))
            .ToList();

        var facturaVentaExistente = db.tblAdmFacturaVenta
            .Include(x => x.idAdmAlbaranVenta)
            .Include(x => x.tblAdmConceptoVenta)
            .Include(x => x.InverseidReferenciaFacturaVentaNavigation)
            .Where(x => x.idAdmFacturaVenta == facturaVenta.idAdmFacturaVenta)
            .FirstOrDefault();

        lock (_lockObject)
        {
            if (facturaVentaExistente == null)
            {
                newFacturaVenta = new()
                {
                    idAdmFactura_Estado = facturaVenta.idAdmFactura_Estado,
                    idTipoAlbaran = facturaVenta.idTipoAlbaran,
                    idMoneda = facturaVenta.idMoneda,
                    idAdmTipoCambio = facturaVenta.idAdmTipoCambio,
                    idTipoFactura = facturaVenta.idTipoFactura,
                    idIncoterm = facturaVenta.idIncoterm,
                    codigo = ac.GetFullCode("facturaVenta", facturaVenta.idEmpresaPolarier, facturaVenta.idTipoAlbaran, facturaVenta.fecha),//si, tipoAlbaran //facturaVenta.codigo,
                    fecha = facturaVenta.fecha,
                    fechaVencimiento = facturaVenta.fechaVencimiento,
                    idAdmTipoNCF = facturaVenta.idAdmTipoNCF,
                    NCF = facturaVenta.idAdmTipoNCF != null ? ac.GetFullCode("NCF", facturaVenta.idEmpresaPolarier, facturaVenta.idAdmTipoNCF, facturaVenta.fecha) : null,
                    comentario = facturaVenta.comentario,
                    idAdmCliente = facturaVenta.idAdmCliente,
                    idEmpresaPolarier = facturaVenta.idEmpresaPolarier,
                    idIvaNPais = facturaVenta.idIvaNPais,
                    idCuentaBancaria = facturaVenta.idCuentaBancaria,
                    idAdmFormaCobro = facturaVenta.idAdmFormaCobro,
                    tasaCambio = facturaVenta.tasaCambio == 0 ? 1 : facturaVenta.tasaCambio,
                    idAdmTipoDescuento = facturaVenta.idAdmTipoDescuento,
                    descuento = facturaVenta.descuento,
                    observaciones = facturaVenta.observaciones,
                    numPedido = facturaVenta.numPedido,
                    idAdmCentroCoste = facturaVenta.idAdmCentroCoste,
                    idAdmElementoPEP = facturaVenta.idAdmElementoPEP,
                    idAdmCondicionPago = facturaVenta.idAdmCondicionPago,
                    tipoRetencion = facturaVenta.tipoRetencion,
                    codigoRetencion = facturaVenta.codigoRetencion,
                    aplicaRetencion = facturaVenta.aplicaRetencion,
                    idAdmAlbaranVenta = tblAdmAlbaranVenta,
                    tblAdmConceptoVenta = facturaVenta.tblAdmConceptoVenta,
                    idReferenciaFacturaVenta = facturaVenta.idReferenciaFacturaVenta,
                    sociedadGL = facturaVenta.sociedadGL,
                    formaPagoMXN = facturaVenta.formaPagoMXN,
                    usoCFDI = facturaVenta.usoCFDI,
                    cantidad = facturaVenta.cantidad,
                    unidadMedida = facturaVenta.unidadMedida,
                    producto = facturaVenta.producto,
                    clvRef1 = string.IsNullOrEmpty(facturaVenta.clvRef1) ? null : facturaVenta.clvRef1,
                    clvRef2 = string.IsNullOrEmpty(facturaVenta.clvRef2) ? null : facturaVenta.clvRef2,
                    clvRef3 = string.IsNullOrEmpty(facturaVenta.clvRef3) ? null : facturaVenta.clvRef3
                };
                db.tblAdmFacturaVenta.Add(newFacturaVenta);
            }
            else
            {
                facturaVentaExistente.codigo = facturaVentaExistente.idTipoAlbaran != facturaVenta.idTipoAlbaran
                    ? (ac.GetFullCode("facturaVenta", facturaVenta.idEmpresaPolarier, facturaVenta.idTipoAlbaran, facturaVenta.fecha) /*?? facturaVenta.codigo*/)
                    : facturaVenta.codigo;
                facturaVentaExistente.NCF = facturaVentaExistente.idAdmTipoNCF != facturaVenta.idAdmTipoNCF
                    ? (ac.GetFullCode("NCF", facturaVenta.idEmpresaPolarier, facturaVenta.idAdmTipoNCF, facturaVenta.fecha) /*?? facturaVenta.NCF*/)
                    : facturaVenta.NCF;
                facturaVentaExistente.idAdmFactura_Estado = facturaVenta.idAdmFactura_Estado;
                facturaVentaExistente.idTipoAlbaran = facturaVenta.idTipoAlbaran;
                facturaVentaExistente.idMoneda = facturaVenta.idMoneda;
                facturaVentaExistente.idAdmTipoCambio = facturaVenta.idAdmTipoCambio;
                facturaVentaExistente.idTipoFactura = facturaVenta.idTipoFactura;
                facturaVentaExistente.idIncoterm = facturaVenta.idIncoterm;
                facturaVentaExistente.fecha = facturaVenta.fecha;
                facturaVentaExistente.fechaVencimiento = facturaVenta.fechaVencimiento;
                facturaVentaExistente.idAdmTipoNCF = facturaVenta.idAdmTipoNCF;
                facturaVentaExistente.comentario = facturaVenta.comentario;
                facturaVentaExistente.idAdmCliente = facturaVenta.idAdmCliente;
                facturaVentaExistente.idCuentaBancaria = facturaVenta.idCuentaBancaria;
                facturaVentaExistente.idAdmFormaCobro = facturaVenta.idAdmFormaCobro;
                facturaVentaExistente.tasaCambio = facturaVenta.tasaCambio == 0 ? 1 : facturaVenta.tasaCambio;
                facturaVentaExistente.idAdmTipoDescuento = facturaVenta.idAdmTipoDescuento;
                facturaVentaExistente.descuento = facturaVenta.descuento;
                facturaVentaExistente.observaciones = facturaVenta.observaciones;
                facturaVentaExistente.numPedido = facturaVenta.numPedido;
                facturaVentaExistente.tipoRetencion = facturaVenta.tipoRetencion;
                facturaVentaExistente.codigoRetencion = facturaVenta.codigoRetencion;
                facturaVentaExistente.aplicaRetencion = facturaVenta.aplicaRetencion;
                facturaVentaExistente.idAdmCentroCoste = facturaVenta.idAdmCentroCoste;
                facturaVentaExistente.idAdmElementoPEP = facturaVenta.idAdmElementoPEP;
                facturaVentaExistente.idAdmCondicionPago = facturaVenta.idAdmCondicionPago;
                facturaVentaExistente.idAdmAlbaranVenta.Clear();
                facturaVentaExistente.idAdmAlbaranVenta = tblAdmAlbaranVenta;
                facturaVentaExistente.idEmpresaPolarier = facturaVenta.idEmpresaPolarier;
                facturaVentaExistente.idIvaNPais = facturaVenta.idIvaNPais;
                facturaVentaExistente.idReferenciaFacturaVenta = facturaVenta.idReferenciaFacturaVenta;
                facturaVentaExistente.sociedadGL = facturaVenta.sociedadGL;
                facturaVentaExistente.formaPagoMXN = facturaVenta.formaPagoMXN;
                facturaVentaExistente.usoCFDI = facturaVenta.usoCFDI;
                facturaVentaExistente.cantidad = facturaVenta.cantidad;
                facturaVentaExistente.unidadMedida = facturaVenta.unidadMedida;
                facturaVentaExistente.producto = facturaVenta.producto;
                facturaVentaExistente.clvRef1 = string.IsNullOrEmpty(facturaVenta.clvRef1) ? null : facturaVenta.clvRef1;
                facturaVentaExistente.clvRef2 = string.IsNullOrEmpty(facturaVenta.clvRef2) ? null : facturaVenta.clvRef2;
                facturaVentaExistente.clvRef3 = string.IsNullOrEmpty(facturaVenta.clvRef3) ? null : facturaVenta.clvRef3;

                db.tblAdmConceptoVenta.RemoveRange(facturaVentaExistente.tblAdmConceptoVenta);
                facturaVenta.tblAdmConceptoVenta.ForEach(item => item.idAdmFacturaVenta = facturaVentaExistente.idAdmFacturaVenta);
                db.tblAdmConceptoVenta.AddRange(facturaVenta.tblAdmConceptoVenta);

                db.tblAdmFacturaVenta.Update(facturaVentaExistente);
                newFacturaVenta = facturaVentaExistente;

                foreach (var factura in facturaVentaExistente.InverseidReferenciaFacturaVentaNavigation)
                {
                    factura.idMoneda = facturaVentaExistente.idMoneda;
                    factura.idAdmTipoCambio = (byte)idsAdmTipoCambio.Manual;
                    factura.tasaCambio = facturaVentaExistente.tasaCambio;
                }
            }

            db.SaveChanges();
            return Ok(newFacturaVenta);
        }
    }
}
