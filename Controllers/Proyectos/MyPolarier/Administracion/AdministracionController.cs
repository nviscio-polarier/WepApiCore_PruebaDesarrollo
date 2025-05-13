using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.Administracion;
public class AdministracionController : ODataController
{
    private readonly bdERP db;
    public AdministracionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Route("odata/MyPolarier/Administracion/GetLastCode")]
    [Authorize]
    public ActionResult GetLastCode([FromODataUri] string tipoDocumento, int idEmpresa, byte tipoElemento, DateTime fecha)
    {
        var codigo = GetFullCode(tipoDocumento, idEmpresa, tipoElemento, fecha);

        if (codigo == null)
        {
            return BadRequest();
        }

        return Ok(codigo);
    }

    public string? GetFullCode(string tipoDocumento, int idEmpresa, byte? tipoElemento, DateTime? fecha)
    {
        if (fecha == null)
        {
            fecha = DateTime.Now;
        }
        var prefijo = tipoDocumento == "NCF" ? db.tblAdmTipoNCF.FirstOrDefault(x => x.idAdmTipoNCF == tipoElemento)?.prefijo : db.tblAdmTipoElemento.FirstOrDefault(x => x.idAdmTipoElemento == tipoElemento)?.prefijo;
        int serial = 0; /*tipoDocumento == "NCF" ? 00000000 : 0000;*/

        switch (tipoDocumento)
        {
            case "pedidoProveedor":
                serial = db.tblAdmPedidoProveedor
                    .Where(x => x.idEmpresaPolarier == idEmpresa && x.idTipoPedido == tipoElemento && x.fechaCreacion.Value.Year == fecha.Value.Year && x.fechaCreacion.Value.Month == fecha.Value.Month)
                    .OrderByDescending(x => x.codigo.Substring(x.codigo.Length - 4))
                    .Select(x => int.Parse(String.Join("", x.codigo.TakeLast(4))))
                    .FirstOrDefault();
                break;
            case "pedidoCliente":
                serial = db.tblAdmPedidoCliente
                    .Where(x => x.idEmpresaPolarier == idEmpresa && x.idTipoPedido == tipoElemento && x.fechaCreacion.Value.Year == fecha.Value.Year && x.fechaCreacion.Value.Month == fecha.Value.Month)
                    .OrderByDescending(x => x.codigo.Substring(x.codigo.Length - 4))
                    .Select(x => int.Parse(String.Join("", x.codigo.TakeLast(4))))
                    .FirstOrDefault();
                break;
            case "presupuestoVenta":
                serial = db.tblAdmPresupuestoVenta
                    .Where(x => x.idEmpresaPolarier == idEmpresa && x.idTipoPresupuesto == tipoElemento && x.fechaCreacion.Value.Year == fecha.Value.Year && x.fechaCreacion.Value.Month == fecha.Value.Month)
                    .OrderByDescending(x => x.codigo.Substring(x.codigo.Length - 4))
                    .Select(x => int.Parse(String.Join("", x.codigo.TakeLast(4))))
                    .FirstOrDefault();
                break;
            case "albaranCompra":
                serial = db.tblAdmAlbaranCompra
                    .Where(x => x.idEmpresaPolarier == idEmpresa && x.idTipoAlbaran == tipoElemento && x.fechaCreacion.Value.Year == fecha.Value.Year && x.fechaCreacion.Value.Month == fecha.Value.Month)
                    .OrderByDescending(x => x.codigo.Substring(x.codigo.Length - 4))
                    .Select(x => int.Parse(String.Join("", x.codigo.TakeLast(4))))
                    .FirstOrDefault();
                break;
            case "albaranVenta":
                serial = db.tblAdmAlbaranVenta
                    .Where(x => x.idEmpresaPolarier == idEmpresa && x.idTipoAlbaran == tipoElemento && x.fechaCreacion.Value.Year == fecha.Value.Year && x.fechaCreacion.Value.Month == fecha.Value.Month)
                    .OrderByDescending(x => x.codigo.Substring(x.codigo.Length - 4))
                    .Select(x => int.Parse(String.Join("", x.codigo.TakeLast(4))))
                    .FirstOrDefault();
                break;
            case "facturaCompra":
                serial = db.tblAdmFacturaCompra
                    .Where(x => x.idEmpresaPolarier == idEmpresa && x.idTipoAlbaran == tipoElemento && x.fecha.Value.Year == fecha.Value.Year && x.fecha.Value.Month == fecha.Value.Month)
                    .OrderByDescending(x => x.codigo.Substring(x.codigo.Length - 4))
                    .Select(x => int.Parse(String.Join("", x.codigo.TakeLast(4))))
                    .FirstOrDefault();
                break;
            case "facturaVenta":
                serial = db.tblAdmFacturaVenta
                    .Where(x => x.idEmpresaPolarier == idEmpresa && x.idTipoAlbaran == tipoElemento && x.fecha.Value.Year == fecha.Value.Year && x.fecha.Value.Month == fecha.Value.Month)
                    .OrderByDescending(x => x.codigo.Substring(x.codigo.Length - 4))
                    .Select(x => int.Parse(String.Join("", x.codigo.TakeLast(4))))
                    .FirstOrDefault();
                break;
            case "NCF":
                serial = db.tblAdmFacturaVenta
                    .Where(x => x.idEmpresaPolarier == idEmpresa && x.idAdmTipoNCF == tipoElemento && x.idAdmFactura_Estado != 3)
                    .OrderByDescending(x => x.NCF.Substring(x.NCF.Length - 8))
                    .Select(x => int.Parse(String.Join("", x.NCF.TakeLast(8))))
                    .FirstOrDefault();
                if (idEmpresa == 4)
                {
                    switch (tipoElemento)
                    {
                        case 1:
                            if (serial <= 00002336)
                            {
                                serial = 00002336;
                            }
                            break;
                        case 2:
                            if (serial <= 00000196)
                            {
                                serial = 00000196;
                            }
                            break;
                        case 4:
                            if (serial <= 00000010)
                            {
                                serial = 00000010;
                            }
                            break;
                        case 5:
                            if (serial <= 00000030)
                            {
                                serial = 00000030;
                            }
                            break;
                    }
                }
                break;
            default:
                return null;
        }
        serial++;

        return prefijo + (tipoDocumento == "NCF" ? serial.ToString("00000000") : fecha.Value.ToString("yyMM") + serial.ToString("0000"));
    }
}
