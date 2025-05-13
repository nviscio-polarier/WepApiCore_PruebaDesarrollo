using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.Administracion.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.Administracion;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.Administracion;

public class AdmAlbaranCompraController : ODataController
{
    private readonly bdERP db;
    private readonly AdministracionController ac;
    public AdmAlbaranCompraController(bdERP context)
    {
        db = context;
        ac = new (context);
    }

    [HttpPost]
    [Route("odata/Administracion/postAdmAlbaranCompra")]
    [Authorize]
    public async Task<ActionResult> PostAdmAlbaranCompra([FromBody] AlbaranCompraConArticulos albaranCompra)
    {
        //Array para almacenar las entities que se van a insertar
        tblAdmAlbaranCompra nuevoAlbaranCompra;
        List<tblAdmArticulo?>? listaArticulosAInsertar = null;

        //Insercion o actualizacion de albaran de compra
        tblAdmAlbaranCompra? albaranCompraExistente = db.tblAdmAlbaranCompra
            .Include(x => x.tblArticuloNAdmAlbaranCompra)
            .Where(x => x.idAdmAlbaranCompra == albaranCompra.idAdmAlbaranCompra)
            .FirstOrDefault();
        if (albaranCompraExistente == null)
        {
            //Se trata de una inserción
            nuevoAlbaranCompra = new()
            {
                //idAdmAlbaranCompra = albaranCompra.idAdmAlbaranCompra,
                codigo = ac.GetFullCode("albaranCompra", albaranCompra.idEmpresaPolarier, albaranCompra.idTipoAlbaran, albaranCompra.fechaCreacion),//albaranCompra.codigo,
                fechaCreacion = albaranCompra.fechaCreacion,
                idTipoAlbaran = albaranCompra.idTipoAlbaran,
                idAdmAlbaran_Estado = albaranCompra.idAdmAlbaran_Estado,
                idAdmProveedor = albaranCompra.idAdmProveedor,
                idMoneda = albaranCompra.idMoneda,
                tasaCambio = albaranCompra.tasaCambio == 0 ? 1 : albaranCompra.tasaCambio,
                descuento = albaranCompra.descuento,
                idAdmTipoDescuento = albaranCompra.idAdmTipoDescuento,
                idAdmFormaPago = albaranCompra.idAdmFormaPago,
                idIncoterm = albaranCompra.idIncoterm,
                idAdmPedidoProveedor = albaranCompra.idAdmPedidoProveedor,
                observaciones = albaranCompra.observaciones,
                numAlbaranProveedor = albaranCompra.numAlbaranProveedor,
                idAdmTipoCambio = albaranCompra.idAdmTipoCambio,
                idEmpresaPolarier = albaranCompra.idEmpresaPolarier,
                idAdmCentroCoste = albaranCompra.idAdmCentroCoste,
                idAdmElementoPEP = albaranCompra.idAdmElementoPEP,
                tblArticuloNAdmAlbaranCompra = albaranCompra.tblArticuloNAdmAlbaranCompra
            };
            db.tblAdmAlbaranCompra.Add(nuevoAlbaranCompra);
            db.SaveChanges();
        }
        else
        {
            //Se trata de una actualización
            //albaranCompraExistente.idAdmAlbaranCompra = albaranCompra.idAdmAlbaranCompra;
            albaranCompraExistente.codigo = albaranCompraExistente.idTipoAlbaran != albaranCompra.idTipoAlbaran
                ? (ac.GetFullCode("albaranCompra", albaranCompra.idEmpresaPolarier, albaranCompra.idTipoAlbaran, albaranCompra.fechaCreacion) ?? albaranCompra.codigo)
                : albaranCompra.codigo;
            albaranCompraExistente.fechaCreacion = albaranCompra.fechaCreacion;
            albaranCompraExistente.idTipoAlbaran = albaranCompra.idTipoAlbaran;
            albaranCompraExistente.idAdmAlbaran_Estado = albaranCompra.idAdmAlbaran_Estado;
            albaranCompraExistente.idAdmProveedor = albaranCompra.idAdmProveedor;
            albaranCompraExistente.idMoneda = albaranCompra.idMoneda;
            albaranCompraExistente.tasaCambio = albaranCompra.tasaCambio == 0 ? 1 : albaranCompra.tasaCambio;
            albaranCompraExistente.descuento = albaranCompra.descuento;
            albaranCompraExistente.idAdmTipoDescuento = albaranCompra.idAdmTipoDescuento;
            albaranCompraExistente.idAdmFormaPago = albaranCompra.idAdmFormaPago;
            albaranCompraExistente.idIncoterm = albaranCompra.idIncoterm;
            albaranCompraExistente.idAdmPedidoProveedor = albaranCompra.idAdmPedidoProveedor;
            albaranCompraExistente.observaciones = albaranCompra.observaciones;
            albaranCompraExistente.numAlbaranProveedor = albaranCompra.numAlbaranProveedor;
            albaranCompraExistente.idAdmTipoCambio = albaranCompra.idAdmTipoCambio;
            albaranCompraExistente.idEmpresaPolarier = albaranCompra.idEmpresaPolarier;
            albaranCompraExistente.idAdmCentroCoste = albaranCompra.idAdmCentroCoste;
            albaranCompraExistente.idAdmElementoPEP = albaranCompra.idAdmElementoPEP;
            db.tblArticuloNAdmAlbaranCompra.RemoveRange(albaranCompraExistente.tblArticuloNAdmAlbaranCompra);
            albaranCompra.tblArticuloNAdmAlbaranCompra.ForEach(item => item.idAdmAlbaranCompra = albaranCompraExistente.idAdmAlbaranCompra);
            albaranCompraExistente.tblArticuloNAdmAlbaranCompra = albaranCompra.tblArticuloNAdmAlbaranCompra;

            db.tblAdmAlbaranCompra.Update(albaranCompraExistente);
            db.SaveChanges();
            nuevoAlbaranCompra = albaranCompraExistente;
        }
        return Ok(nuevoAlbaranCompra);
    }
}
