using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using WebApiCore.Class;
using WebApiCore.Class.bdERP.Administracion;
using WebApiCore.Class.ftp;
using WebApiCore.Context;
using WebApiCore.Enums.General;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.Contabilidad
{
    public class AsientosFacturasController : ODataController
    {
        private readonly bdERP db;
        public AsientosFacturasController(bdERP context)
        {
            db = context;
        }

        [HttpPost("odata/MyPolarier/Contabilidad/AsientosFacturas/GenerarAsientosFacturaVentaSAP")]
        [Authorize]
        public async Task<ActionResult> GenerarAsientosFacturaVentaSAP([FromODataUri] int idAdmFacturaVenta)
        {
            var aux = db.tblAdmFacturaVenta.Include(x => x.idEmpresaPolarierNavigation).FirstOrDefault(x => x.idAdmFacturaVenta == idAdmFacturaVenta);
            var facturaVenta = db.tblAdmFacturaVenta
                .Include(x => x.idEmpresaPolarierNavigation)
                .FirstOrDefault(x => x.idAdmFacturaVenta == idAdmFacturaVenta);

            if (facturaVenta == null)
            {
                return NotFound();
            }

            //if (facturaVenta.isCerrado) Habilitar cuando se abra a todos los usuarios
            {
                int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
                var objUsuario = db.tblUsuario.Find(idUsuario);
                List<int> idWhitelist = db.tblUsuario
                    .Where(x => x.idPermiso.Any(x => x.codigo == idsPermiso.ContabilizarSAP))
                    .Select(x => x.idUsuario).ToList();

                if (!idWhitelist.Contains(idUsuario) && objUsuario.idCargo != (short)idsCargo.Desarrollador)
                {
                    return BadRequest();
                }
            }

            var idsAdmFacturaVenta = new List<int> { idAdmFacturaVenta };

            var asientos = await GetAsientosFacturaVenta(idsAdmFacturaVenta, (int)facturaVenta.idEmpresaPolarierNavigation.idPais);

            var random = new Random();
            random.Next(10, 100);

            var nombreArchivo = "asientoFacturaVenta_" + facturaVenta.codigo + "_" + facturaVenta.idEmpresaPolarierNavigation.companyCode_SAP + "_" + random.Next(10, 100) + ".csv";

            if (await UploadAsiento(nombreArchivo, asientos))
            {
                await CerrarFacturasVenta(idsAdmFacturaVenta);
                return Ok(true);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("odata/MyPolarier/Contabilidad/AsientosFacturas/GenerarAsientosFacturaCompraSAP")]
        [Authorize]
        public async Task<ActionResult> GenerarAsientosFacturaCompraSAP([FromODataUri] int idAdmFacturaCompra)
        {
            var facturaCompra = db.tblAdmFacturaCompra.Find(idAdmFacturaCompra);

            if (facturaCompra == null)
            {
                return NotFound();
            }

            if (facturaCompra.isCerrado)
            {
                return BadRequest();
            }

            var idsAdmFacturaCompra = new List<int> { idAdmFacturaCompra };

            var asientos = await GetAsientosFacturaCompra(idsAdmFacturaCompra);

            var nombreArchivo = "asientoFacturaCompra_" + facturaCompra.codigo + ".csv";

            if (await UploadAsiento(nombreArchivo, asientos))
            {
                await CerrarFacturasCompra(idsAdmFacturaCompra);
                return Ok(true);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Sube los asientos a SAP
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="asientos"></param>
        /// <returns>Booleano que confirma si ha ido bien.</returns>
        private static async Task<bool> UploadAsiento(string nombreArchivo, List<EntradaAsientoContable> asientos)
        {
            var csv = EntradaAsientoContable.GenerateCSV(asientos);
            var objStream = new MemoryStream(Encoding.ASCII.GetBytes(csv));

            var ftp = new FTPConnection();
            var ftpResponse = await ftp.UploadFile(nombreArchivo, objStream.ToArray());

            return ftpResponse.StatusCode == System.Net.FtpStatusCode.ClosingData;
        }

        private async Task<List<EntradaAsientoContable>> GetAsientosFacturaVenta(List<int> idsAdmFacturaVenta, int idPais)
        {
            DataTable ttIdsAdmFacturaVenta = new();
            ttIdsAdmFacturaVenta.SetTypeName("General.ttIdSimple");
            ttIdsAdmFacturaVenta.Columns.Add("id", typeof(int));

            foreach (var idAdmFacturaVenta in idsAdmFacturaVenta)
            {
                var row = ttIdsAdmFacturaVenta.NewRow();
                row["id"] = idAdmFacturaVenta;
                ttIdsAdmFacturaVenta.Rows.Add(row);
            }

            var connection = db.Database.GetDbConnection();

            var procedure = idPais == (int)idsPais.México ? "EF_spSelectAsientosFacturaVenta_Mexico" : "EF_spSelectAsientosFacturaVenta";

            var result = (
                await connection.QueryAsync<EntradaAsientoContable>(
                    $"EXEC [Administracion].[{procedure}] @idsAdmFacturaVenta",
                    new { idsAdmFacturaVenta = ttIdsAdmFacturaVenta }
                )
            ).ToList();

            return result;
        }

        private async Task<List<EntradaAsientoContable>> GetAsientosFacturaCompra(List<int> idsAdmFacturaCompra)
        {
            DataTable ttIdsAdmFacturaCompra = new();
            ttIdsAdmFacturaCompra.SetTypeName("General.ttIdSimple");
            ttIdsAdmFacturaCompra.Columns.Add("id", typeof(int));

            foreach (var idAdmFacturaCompra in idsAdmFacturaCompra)
            {
                var row = ttIdsAdmFacturaCompra.NewRow();
                row["id"] = idAdmFacturaCompra;
                ttIdsAdmFacturaCompra.Rows.Add(row);
            }

            var connection = db.Database.GetDbConnection();

            var result = (
                await connection.QueryAsync<EntradaAsientoContable>(
                    "EXEC [Administracion].[EF_spSelectAsientosFacturaCompra] @idsAdmFacturaCompra",
                    new { idsAdmFacturaCompra = ttIdsAdmFacturaCompra }
                )
            ).ToList();

            return result;
        }

        /// <summary>
        /// Cierra las facturas de venta y relacionados.
        /// </summary>
        /// <param name="idsAdmFacturaVenta"></param>
        private async Task CerrarFacturasVenta(List<int> idsAdmFacturaVenta)
        {
            var tblAdmFacturaVenta = db.tblAdmFacturaVenta
                .Where(fv => idsAdmFacturaVenta.Contains(fv.idAdmFacturaVenta));

            var tblAdmAlbaranVenta = db.tblAdmAlbaranVenta
                .Where(av => av.idAdmFacturaVenta.Any(avnfv => idsAdmFacturaVenta.Contains(avnfv.idAdmFacturaVenta)));

            var idsAdmAlbaranVenta = tblAdmAlbaranVenta.Select(av => av.idAdmAlbaranVenta);

            var tblAdmPedidoCliente = db.tblAdmPedidoCliente
                .Where(pc => pc.tblAdmAlbaranVenta.Any(pcvav => idsAdmAlbaranVenta.Contains(pcvav.idAdmAlbaranVenta)));

            var idsAdmPedidoCliente = tblAdmPedidoCliente.Select(pc => pc.idAdmPedidoCliente);

            var tblAdmPresupuestoVenta = db.tblAdmPresupuestoVenta
                .Where(pv => pv.tblAdmPedidoCliente.Any(pvpc => idsAdmPedidoCliente.Contains(pvpc.idAdmPedidoCliente)));

            foreach (var fv in tblAdmFacturaVenta)
            {
                fv.isCerrado = true;
                fv.idAdmFactura_Estado = 2;
            }

            foreach (var av in tblAdmAlbaranVenta)
            {
                av.isCerrado = true;
            }

            foreach (var pc in tblAdmPedidoCliente)
            {
                pc.isCerrado = true;
            }

            foreach (var pv in tblAdmPresupuestoVenta)
            {
                pv.isCerrado = true;
            }

            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Cierra las facturas de compra y relacionados.
        /// </summary>
        /// <param name="idsAdmFacturaCompra"></param>
        private async Task CerrarFacturasCompra(List<int> idsAdmFacturaCompra)
        {
            var tblAdmFacturaCompra = db.tblAdmFacturaCompra
                .Where(fv => idsAdmFacturaCompra.Contains(fv.idAdmFacturaCompra));

            var tblAdmAlbaranCompra = db.tblAdmAlbaranCompra
                .Where(av => av.idAdmFacturaCompra.Any(avnfv => idsAdmFacturaCompra.Contains(avnfv.idAdmFacturaCompra)));

            var idsAdmAlbaranCompra = tblAdmAlbaranCompra.Select(av => av.idAdmAlbaranCompra);

            var tblAdmPedidoProveedor = db.tblAdmPedidoProveedor
                .Where(pc => pc.tblAdmAlbaranCompra.Any(pcvav => idsAdmAlbaranCompra.Contains(pcvav.idAdmAlbaranCompra)));

            foreach (var fv in tblAdmFacturaCompra)
            {
                fv.isCerrado = true;
            }

            foreach (var av in tblAdmAlbaranCompra)
            {
                av.isCerrado = true;
            }

            foreach (var pp in tblAdmPedidoProveedor)
            {
                pp.isCerrado = true;
            }

            await db.SaveChangesAsync();
        }
    }
}
