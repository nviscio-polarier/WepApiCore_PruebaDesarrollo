using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.Administracion.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.Administracion;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.Administracion;

public class AdmFacturaCompraController : ODataController
{
    private readonly bdERP db;
    private readonly AdministracionController ac;
    public AdmFacturaCompraController(bdERP context)
    {
        db = context;
        ac = new (context);
    }

    [HttpPost]
    [Route("odata/Administracion/IU_AdmFacturaCompra")]
    [Authorize]
    public async Task<ActionResult> IU_AdmFacturaCompra([FromBody] FacturaCompra facturaCompra)
    {
        tblAdmFacturaCompra newFacturaCompra;

        var tblAdmAlbaranCompra = db.tblAdmAlbaranCompra
            .Where(ac => facturaCompra.idAdmAlbaranCompra != null && facturaCompra.idAdmAlbaranCompra.Contains(ac.idAdmAlbaranCompra))
            .ToList();

        var facturaCompraExistente = db.tblAdmFacturaCompra
            .Include(x => x.idAdmAlbaranCompra)
            .Include(x => x.tblAdmConceptoCompra)
            .Where(x => x.idAdmFacturaCompra == facturaCompra.idAdmFacturaCompra)
            .FirstOrDefault();

        if (facturaCompraExistente == null)
        {
            newFacturaCompra = new()
            {
                idAdmFactura_Estado = facturaCompra.idAdmFactura_Estado,
                idTipoAlbaran = facturaCompra.idTipoAlbaran,
                idMoneda = facturaCompra.idMoneda,
                idAdmTipoCambio = facturaCompra.idAdmTipoCambio,
                idAdmTipoDescuento = facturaCompra.idAdmTipoDescuento,
                idTipoFactura = facturaCompra.idTipoFactura,
                idIncoterm = facturaCompra.idIncoterm,
                codigo = ac.GetFullCode("facturaCompra", facturaCompra.idEmpresaPolarier, facturaCompra.idTipoAlbaran, facturaCompra.fecha),//si, tipoAlbaran //facturaCompra.codigo,
                fecha = facturaCompra.fecha,
                tasaCambio = facturaCompra.tasaCambio == 0 ? 1 : facturaCompra.tasaCambio,
                descuento = facturaCompra.descuento,
                idAdmProveedor = facturaCompra.idAdmProveedor,
                idAdmFormaPago = facturaCompra.idAdmFormaPago,
                observaciones = facturaCompra.observaciones,
                numFacturaProveedor = facturaCompra.numFacturaProveedor,
                idAdmCentroCoste = facturaCompra.idAdmCentroCoste,
                idAdmElementoPEP = facturaCompra.idAdmElementoPEP,
                idAdmCondicionPago = facturaCompra.idAdmCondicionPago,
                idAdmAlbaranCompra = tblAdmAlbaranCompra,
                idEmpresaPolarier = facturaCompra.idEmpresaPolarier,
                tblAdmConceptoCompra = facturaCompra.tblAdmConceptoCompra
            };
            db.tblAdmFacturaCompra.Add(newFacturaCompra);
        }
        else
        {
            facturaCompraExistente.codigo = facturaCompraExistente.idTipoAlbaran != facturaCompra.idTipoAlbaran
                ? (ac.GetFullCode("facturaCompra", facturaCompra.idEmpresaPolarier, facturaCompra.idTipoAlbaran, facturaCompra.fecha) ?? facturaCompra.codigo)
                : facturaCompra.codigo;
            facturaCompraExistente.idAdmFactura_Estado = facturaCompra.idAdmFactura_Estado;
            facturaCompraExistente.idTipoAlbaran = facturaCompra.idTipoAlbaran;
            facturaCompraExistente.idMoneda = facturaCompra.idMoneda;
            facturaCompraExistente.idAdmTipoCambio = facturaCompra.idAdmTipoCambio;
            facturaCompraExistente.idAdmTipoDescuento = facturaCompra.idAdmTipoDescuento;
            facturaCompraExistente.idTipoFactura = facturaCompra.idTipoFactura;
            facturaCompraExistente.idIncoterm = facturaCompra.idIncoterm;
            facturaCompraExistente.fecha = facturaCompra.fecha;
            facturaCompraExistente.tasaCambio = facturaCompra.tasaCambio == 0 ? 1 : facturaCompra.tasaCambio;
            facturaCompraExistente.descuento = facturaCompra.descuento;
            facturaCompraExistente.idAdmProveedor = facturaCompra.idAdmProveedor;
            facturaCompraExistente.idAdmFormaPago = facturaCompra.idAdmFormaPago;
            facturaCompraExistente.observaciones = facturaCompra.observaciones;
            facturaCompraExistente.numFacturaProveedor = facturaCompra.numFacturaProveedor;
            facturaCompraExistente.idAdmCentroCoste = facturaCompra.idAdmCentroCoste;
            facturaCompraExistente.idAdmElementoPEP = facturaCompra.idAdmElementoPEP;
            facturaCompraExistente.idAdmCondicionPago = facturaCompra.idAdmCondicionPago;
            facturaCompraExistente.idAdmAlbaranCompra.Clear();
            facturaCompraExistente.idAdmAlbaranCompra = tblAdmAlbaranCompra;
            facturaCompraExistente.idEmpresaPolarier = facturaCompra.idEmpresaPolarier;

            db.tblAdmConceptoCompra.RemoveRange(facturaCompraExistente.tblAdmConceptoCompra);
            facturaCompra.tblAdmConceptoCompra.ForEach(item => item.idAdmFacturaCompra = facturaCompraExistente.idAdmFacturaCompra);
            db.tblAdmConceptoCompra.AddRange(facturaCompra.tblAdmConceptoCompra);

            db.tblAdmFacturaCompra.Update(facturaCompraExistente);
            newFacturaCompra = facturaCompraExistente;
        }

        await db.SaveChangesAsync();

        return Ok(newFacturaCompra);
    }
}
