using FluentFTP;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using System.Xml.Serialization;
using WebApiCore.Class.externos.SAP;
using WebApiCore.Class.externos.SAP.Context;
using WebApiCore.Class.externos.SAP.Controllers;
using WebApiCore.Class.ftp;
using WebApiCore.Context;
using WebApiCore.Enums.General;

namespace WebApiCore.Class.externos.TimbradoMX
{

    public class TimbradoMXService
    {
        private readonly Context.bdERP db;
        private AsyncFtpClient ftp;
        private string initialPath;
        private readonly SAPWrap sap = new();

        private List<tblLogError> errors = new();

        public TimbradoMXService(Context.bdERP db)
        {
            this.db = db;
        }

        private async Task CatchFileError(Exception e, FtpListItem file)
        {
            errors.Add(new tblLogError
            {
                denominacion = "TimbradoMX",
                error = JsonSerializer.Serialize(new
                {
                    fecha = DateTime.UtcNow,
                    mensaje = e.Message,
                    archivo = file.Name
                })
            });
            var newPath = file.FullName.Replace("pendientes/", "error/Polarier_");
            await MoverArchivo(file, newPath);
        }

        public async Task<Task> checkPendientes()
        {
            errors.Clear();

            ftp = await FTPConnection.CreateClientAndConnect(FTPUser.timbradomx, Utils.isProduccion() ? "" : "/home/timbradomx/test");
            initialPath = await ftp.GetWorkingDirectory();

            var files = await ftp.GetListing(initialPath+"/pendientes");

            Dictionary<FtpListItem, Comprobante> comprobantes = new();
            foreach (var file in files)
            {
                try
                {
                    using MemoryStream str = new();
                    var downloaded = await ftp.DownloadStream(str, file.FullName);
                    if (downloaded)
                    {
                        str.Position = 0;

                        using var StreamReader = new StreamReader(str);
                        string xmlString = StreamReader.ReadToEnd();
                        using var StringReader = new StringReader(xmlString);

                        XmlSerializer serializer = new(typeof(Comprobante));
                        Comprobante xml = (Comprobante)serializer.Deserialize(StringReader)
                            ?? throw new Exception("Error al leer el archivo XML");

                        comprobantes.Add(file, xml);
                    }
                    else
                    {
                        throw new Exception("El archivo no se pudo descargar");
                    }
                }
                catch (Exception e)
                {
                    await CatchFileError(e, file);
                }
            }

            var UUIDs = comprobantes.Select(x => x.Value.Complemento.TimbreFiscalDigital.UUID).ToList();
            var tblFacturaVenta = db.tblAdmFacturaVenta
                .Include(x => x.tblTimbradoMXNAdmFacturaVenta)
                .Include(x => x.idAdmClienteNavigation)
                .Include(x => x.idEmpresaPolarierNavigation)
                .Where(x => x.idEmpresaPolarierNavigation.idPais == (int)idsPais.México && (UUIDs.Contains(x.tblTimbradoMXNAdmFacturaVenta.folioFiscal) || x.tblTimbradoMXNAdmFacturaVenta == null))
                .ToList();

            foreach (var item in comprobantes)
            {
                var file = item.Key;
                var xml = item.Value;

                try
                {
                    var objFactura = tblFacturaVenta.FirstOrDefault(x => x.tblTimbradoMXNAdmFacturaVenta?.folioFiscal == xml.Complemento.TimbreFiscalDigital.UUID && x.idEmpresaPolarierNavigation?.CIF == xml.Emisor.Rfc);

                    if (objFactura == null)
                    {

                        EInvoiceMexico modelo = await SAPUtils.GetResponseAndSerialize<EInvoiceMexico>(sap.eInvoiceMexicoController.Get(xml.Complemento.TimbreFiscalDigital.UUID));
                        var invoiceList = modelo.entry.GroupBy(x => x.id).Select(x => x.First().content.properties).Distinct();
                        var eInvoiceMexico = invoiceList.FirstOrDefault()
                            ?? throw new Exception("No se encontró la factura en SAP");

                        objFactura = db.tblAdmFacturaVenta.FirstOrDefault(x => x.codigo == eInvoiceMexico.AccountingDocumentHeaderText && x.idEmpresaPolarierNavigation.companyCode_SAP == eInvoiceMexico.CompanyCode)
                            ?? throw new Exception($"No se encontró la factura \"{ eInvoiceMexico.AccountingDocumentHeaderText }\" en la base de datos");
                    }

                    if (objFactura.tblTimbradoMXNAdmFacturaVenta != null)
                    {
                        objFactura.tblTimbradoMXNAdmFacturaVenta.formaPago = xml.FormaPago;
                        objFactura.tblTimbradoMXNAdmFacturaVenta.folioFiscal = xml.Complemento.TimbreFiscalDigital.UUID;
                        objFactura.tblTimbradoMXNAdmFacturaVenta.certificacion = xml.Complemento.TimbreFiscalDigital.FechaTimbrado;
                        objFactura.tblTimbradoMXNAdmFacturaVenta.serieCertificadoSAT = xml.Complemento.TimbreFiscalDigital.NoCertificadoSAT;
                        objFactura.tblTimbradoMXNAdmFacturaVenta.serieCertificado = xml.NoCertificado;
                        objFactura.tblTimbradoMXNAdmFacturaVenta.selloCFD = xml.Complemento.TimbreFiscalDigital.SelloCFD;
                        objFactura.tblTimbradoMXNAdmFacturaVenta.selloSAT = xml.Complemento.TimbreFiscalDigital.SelloSAT;
                        objFactura.tblTimbradoMXNAdmFacturaVenta.linkQR = GetLinkQR(objFactura, xml);
                    }
                    else
                    {
                        tblTimbradoMXNAdmFacturaVenta timbrado = new()
                        {
                            idAdmFacturaVenta = objFactura.idAdmFacturaVenta,
                            formaPago = xml.FormaPago,
                            folioFiscal = xml.Complemento.TimbreFiscalDigital.UUID,
                            certificacion = xml.Complemento.TimbreFiscalDigital.FechaTimbrado,
                            serieCertificadoSAT = xml.Complemento.TimbreFiscalDigital.NoCertificadoSAT,
                            serieCertificado = xml.NoCertificado,
                            selloCFD = xml.Complemento.TimbreFiscalDigital.SelloCFD,
                            selloSAT = xml.Complemento.TimbreFiscalDigital.SelloSAT,
                            linkQR = GetLinkQR(objFactura, xml)
                        };

                        db.tblTimbradoMXNAdmFacturaVenta.Add(timbrado);
                    }

                    var newPath = file.FullName.Replace("pendientes/", $"procesados/{DateTime.UtcNow:yyyy}/{DateTime.UtcNow:MM}/");

                    await MoverArchivo(file, newPath);
                }
                catch (Exception e)
                {
                    await CatchFileError(e, file);
                }
            }

            if (errors.Count > 0)
            {
                db.tblLogError.AddRange(errors);
            }

            await db.SaveChangesAsync();

            await ftp.Disconnect();

            return Task.CompletedTask;
        }

        private async Task<bool> MoverArchivo(FtpListItem file, string newPath, bool crearCarpetaSiNoExiste = true)
        {
            try
            {
                var a = newPath.LastIndexOf('_');
                var b = newPath.LastIndexOf('.');
                var lastDate = newPath.Substring(a + 1, b - a - 1);
                var isDate = DateTime.TryParseExact(lastDate, "yyyyMMddHHmmssfffffff", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                if (isDate)
                    newPath = newPath.Replace(lastDate, DateTime.UtcNow.ToString("yyyyMMddHHmmssfffffff"));
                else
                    newPath = newPath.Insert(newPath.IndexOf('.'), $"_{DateTime.UtcNow:yyyyMMddHHmmssfffffff}");

                await ftp.MoveFile(file.FullName, newPath);
                return true;
            }
            catch (Exception e)
            {
                if (e.Message == "Code: 550 Message: Rename failed." && crearCarpetaSiNoExiste)
                {
                    var pathMes = newPath.Remove(newPath.LastIndexOf('/'));
                    if (!await ftp.DirectoryExists(pathMes))
                    {
                        try
                        {
                            var created = await ftp.CreateDirectory(pathMes, true);
                            if (created) return await MoverArchivo(file, newPath, false);
                            else throw new Exception("Error al crear la carpeta");
                        }
                        catch (Exception ex)
                        {
                            await CatchFileError(ex, file);
                        }
                    }

                }

                errors.Add(new tblLogError
                {
                    denominacion = "TimbradoMX",
                    error = JsonSerializer.Serialize(new
                    {
                        fecha = DateTime.UtcNow,
                        mensaje = "Error al mover el archivo: " + e.Message,
                        archivo = file.Name
                    })
                });
            }
            return false;
        }

        private static string GetLinkQR(tblAdmFacturaVenta objFactura, Comprobante xml)
        {
            return "http://www.sat.gob.mx/TimbreFiscalDigital" +
                    "?re=" + objFactura.idEmpresaPolarierNavigation?.CIF +
                    "&rr=" + objFactura.idAdmClienteNavigation?.CIF +
                    "&tt=" + xml.Impuestos.Traslados.Traslado.Base.ToString("0000000000.000000").Replace(',','.') +
                    "&id=" + xml.Complemento.TimbreFiscalDigital.UUID;
        }
    }
}
