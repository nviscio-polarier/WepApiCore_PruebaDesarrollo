using WebApiCore.Class;
using WebApiCore.Class.externos.SAP.Context;
using WebApiCore.Class.externos.SAP;
using WebApiCore.Class.externos.SAP.Controllers;
using WebApiCore.Context;
using Newtonsoft.Json;
using WebApiCore.Class.ftp;
using static WebApiCore.Class.externos.SAP.Context.AllOriginals;
using System.Collections.Concurrent;
using FluentFTP;

namespace WebApiCore.Services
{
    public class DocumentosSAP_RD_Worker : IHostedService, IDisposable
    {
        private Timer? timer = null;

        private readonly bdERP db;
        private readonly SAPWrap sap = new();
        private AsyncFtpClient ftp;

        private readonly int horaDelDia = 0;
        private readonly TimeSpan intervalo = TimeSpan.FromHours(6);

        public DocumentosSAP_RD_Worker(IServiceProvider serviceProvider)
        {
            IServiceScope scope = serviceProvider.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<bdERP>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            var runTime = new TimeSpan(horaDelDia, 0, 0);
            var currentTime = DateTime.Now.TimeOfDay;

            // Si runTime aún no ha ocurrido hoy, resta la hora actual de runTime
            // De lo contrario, calcula el tiempo hasta runTime de mañana

            TimeSpan delay = CalcularProximoIntervalo(runTime, currentTime);

            timer = new Timer(DailyTask, null, delay, intervalo);

            return Task.CompletedTask;
        }

        public TimeSpan CalcularProximoIntervalo(TimeSpan runTime, TimeSpan currentTime)
        {
            // Calcula el tiempo transcurrido desde el runTime hasta la hora actual
            var tiempoTranscurrido = currentTime - runTime;

            // Si el tiempo transcurrido es negativo, significa que el runTime aún no ha ocurrido hoy
            if (tiempoTranscurrido < TimeSpan.Zero)
            {
                tiempoTranscurrido += TimeSpan.FromDays(1);
            }

            // Calcula cuántos intervalos de 6 horas han pasado desde el runTime
            var intervalosPasados = (int)(tiempoTranscurrido.TotalHours / intervalo.TotalHours);

            // Calcula la hora del próximo intervalo
            var proximoIntervalo = runTime + TimeSpan.FromHours((intervalosPasados + 1) * intervalo.TotalHours);
            return proximoIntervalo - currentTime;
        }

        private async void DailyTask(object? state)
        {
            ftp = await FTPConnection.CreateClientAndConnect(FTPUser.amb, Utils.isProduccion() ? "/files/amb/prd/" : "/files/amb/test/");

            await AsientosFactura();
            await PartidasContables();

            await ftp.Disconnect();
        }

        private async Task AsientosFactura()
        {
            db.tblLogError.Add(new tblLogError
            {
                denominacion = "DocumentosSAP",
                error = JsonConvert.SerializeObject(new
                {
                    fecha = DateTime.Now,
                    message = "Iniciando proceso de subida de documentos - FACTURAS",
                })
            });

            await db.SaveChangesAsync();

            #region Obtener los asientos contables de los últimos 3 meses

            // Todo: añadir filtro de últimos 3 meses cuando este disponible
            var odata = "$filter=CompanyCode eq 'DO01'"
                + "&$select=CompanyCode,SupplierInvoice,FiscalYear,DocumentDate,InvoicingParty,SupplierPostingLineItemText";

            var facturas = await SAPUtils.GetResponseAndSerialize<FacturasProveedor.Model>(sap.facturasProveedorController.Get(odata));
            string partidasJsonString = JsonConvert.SerializeObject(facturas.entry.Select(x => x.content.properties).ToArray());
            List<Factura> listaFacturas = JsonConvert.DeserializeObject<List<Factura>>(partidasJsonString)
                .Where(x => x.CompanyCode == "DO01")
                .GroupBy(x => new { x.SupplierInvoice, x.FiscalYear, FiscalPeriod = $"0{x.DocumentDate:MM}", x.BP, NCF = x.NCF.Replace('/','|') })
                .Select(x => new Factura
                {
                    SupplierInvoice = x.Key.SupplierInvoice,
                    FiscalYear = x.Key.FiscalYear,
                    FiscalPeriod = x.Key.FiscalPeriod,
                    BP = x.Key.BP,
                    NCF = x.Key.NCF
                })
                .ToList();

            #endregion

            db.tblLogError.Add(new tblLogError
            {
                denominacion = "DocumentosSAP",
                error = JsonConvert.SerializeObject(new
                {
                    fecha = DateTime.Now,
                    message = $"Facturas obtenidas: {listaFacturas.Count}",
                })
            });

            await db.SaveChangesAsync();

            #region Buscar los documentos asociados a los asientos en SAP

            var documentosConcurrentes = new ConcurrentBag<Documento>();

            foreach (var facturasChunk in listaFacturas.Chunk(100))
            {
                var tasksSAP = facturasChunk.Select(async factura =>
                {
                    try
                    {
                        var response = await sap.allOriginalsController.Get($"{factura.SupplierInvoice}{factura.FiscalYear}", "BUS2081");
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            var asientoSAP = await SAPUtils.DeserializeResponseAsync<AllOriginals.AttachmentContentSet>(response);

                            documentosConcurrentes.Add(new Documento
                            {
                                metadata = factura,
                                AttachmentContent = asientoSAP.AttachmentContent
                            });
                        }

                        response.Dispose();
                    }
                    catch (Exception ex)
                    {
                        lock (db)
                        {
                            db.tblLogError.Add(new tblLogError
                            {
                                denominacion = "DocumentosSAP",
                                error = JsonConvert.SerializeObject(new List<string>
                                {
                                    $"Error al procesar el factura {factura.SupplierInvoice}{factura.FiscalYear}",
                                    $"Error: {ex.Message}"
                                }),
                            });
                        }
                    }
                    return Task.CompletedTask;
                });

                await Task.WhenAll(tasksSAP);
            }


            // Convertir ConcurrentBag a List para mantener la compatibilidad con el resto del código
            var documentos = documentosConcurrentes.ToList();

            #endregion

            db.tblLogError.Add(new tblLogError
            {
                denominacion = "DocumentosSAP",
                error = JsonConvert.SerializeObject(new
                {
                    fecha = DateTime.Now,
                    message = $"Listado de documentos obtenido: {documentos.SelectMany(x => x.AttachmentContent).Count()}",
                })
            });

            await db.SaveChangesAsync();

            #region Subir los documentos al FTP

            List<string> documentosEncontrados = (await ftp.GetListing("", FtpListOption.Recursive))
                .Select(x => x.Name).ToList();

            List<string> documentosSubidos = new();

            foreach (var documento in documentos)
            {
                foreach (var content in documento.AttachmentContent)
                {
                    if (documentosEncontrados.Any(file => file.Contains(content.ArchiveDocumentID))) continue;

                    var factura = (Factura)documento.metadata;
                    var extension = content.FileName.Split('.').Last();
                    var nombreArchivo = $"{factura.FiscalYear}/{factura.FiscalPeriod}/" +
                        $"F{factura.BP}_{(string.IsNullOrEmpty(factura.NCF) ? factura.SupplierInvoice : factura.NCF)}_{content.ArchiveDocumentID}.{extension}";

                    try
                    {
                        using HttpResponseMessage attachmentContent = await sap.attachmentContentController.Get(
                            content.DocumentInfoRecordDocType,
                            content.DocumentInfoRecordDocNumber,
                            content.DocumentInfoRecordDocPart,
                            content.DocumentInfoRecordDocVersion,
                            content.LogicalDocument,
                            content.ArchiveDocumentID,
                            content.BusinessObjectTypeName
                        );
                        using var stream = await attachmentContent.Content.ReadAsStreamAsync();
                        var ftpResponse = await ftp.UploadStream(stream, nombreArchivo, FtpRemoteExists.Skip, true);
                        if (ftpResponse == FtpStatus.Success)
                        {
                            lock (documentosSubidos)
                            {
                                documentosSubidos.Add(nombreArchivo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        db.tblLogError.Add(new tblLogError
                        {
                            denominacion = "DocumentosSAP",
                            error = JsonConvert.SerializeObject(new
                            {
                                title=$"Error al subir el archivo {nombreArchivo}",
                                error=$"Error: {ex.Message}",
                                metadata=JsonConvert.SerializeObject(content)
                            }),
                        });
                    }
                }
            }

            #endregion

            db.tblLogError.Add(new tblLogError
            {
                denominacion = "DocumentosSAP",
                error = JsonConvert.SerializeObject(new
                {
                    fecha = DateTime.Now,
                    message = JsonConvert.SerializeObject(new
                    {
                        msg = $"Documentos subidos: {documentosSubidos.Count}",
                        archivos = JsonConvert.SerializeObject(documentosSubidos)
                    }),
                })
            });

            await db.SaveChangesAsync();
        }

        private async Task PartidasContables()
        {
            db.tblLogError.Add(new tblLogError
            {
                denominacion = "DocumentosSAP",
                error = JsonConvert.SerializeObject(new
                {
                    fecha = DateTime.Now,
                    message = "Iniciando proceso de subida de documentos - PARTIDAS",
                })
            });

            await db.SaveChangesAsync();

            #region Obtener los asientos contables de los últimos 3 meses

            var odata = "$filter=CompanyCode eq 'DO01' and AccountingDocumentType eq 'RE' and (";

            for (int i = 0; i < 3; i++)
            {
                var fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-i);

                odata += $"(FiscalYear eq '{fecha:yyyy}' and FiscalPeriod eq '{fecha:MM}')";
                if (i < 2) odata += " or ";
                else odata += ")";
            }

            odata += "&$select=AccountingDocument,FiscalYear,FiscalPeriod,DocumentReferenceID";

            var partidasContables = await SAPUtils.GetResponseAndSerialize<PartidasContables.Model>(sap.partidasContablesController.Get(odata));
            string partidasJsonString = JsonConvert.SerializeObject(partidasContables.entry.Select(x => x.content.properties).ToArray());
            List<Asiento> listaAsientos = JsonConvert.DeserializeObject<List<Asiento>>(partidasJsonString)
                .GroupBy(x => new { x.AccountingDocument, x.FiscalYear, x.FiscalPeriod, x.NCF })
                .Select(x => x.First())
                .ToList();

            #endregion

            db.tblLogError.Add(new tblLogError
            {
                denominacion = "DocumentosSAP",
                error = JsonConvert.SerializeObject(new
                {
                    fecha = DateTime.Now,
                    message = $"Asientos obtenidos: {listaAsientos.Count}",
                })
            });

            await db.SaveChangesAsync();

            #region Buscar los documentos asociados a los asientos en SAP

            var documentosConcurrentes = new ConcurrentBag<Documento>();

            foreach (var asientoChunk in listaAsientos.Chunk(100))
            {
                var tasksSAP = asientoChunk.Select(async asiento =>
                {
                    try
                    {
                        var response = await sap.allOriginalsController.Get($"DO01{asiento.AccountingDocument}{asiento.FiscalYear}");
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            var asientoSAP = await SAPUtils.DeserializeResponseAsync<AttachmentContentSet>(response);

                            documentosConcurrentes.Add(new Documento
                            {
                                metadata = asiento,
                                AttachmentContent = asientoSAP.AttachmentContent
                            });
                        }

                        response.Dispose();
                    }
                    catch (Exception ex)
                    {
                        db.tblLogError.Add(new tblLogError
                        {
                            denominacion = "DocumentosSAP",
                            error = JsonConvert.SerializeObject(new List<string>
                            {
                                $"Error al procesar el factura DO01{asiento.AccountingDocument}{asiento.FiscalYear}",
                                $"Error: {ex.Message}"
                            }),
                        });
                    }
                    return Task.CompletedTask;
                });

                await Task.WhenAll(tasksSAP);

            }


            // Convertir ConcurrentBag a List para mantener la compatibilidad con el resto del código
            var documentos = documentosConcurrentes.ToList();

            #endregion

            db.tblLogError.Add(new tblLogError
            {
                denominacion = "DocumentosSAP",
                error = JsonConvert.SerializeObject(new
                {
                    fecha = DateTime.Now,
                    message = $"Listado de documentos obtenido: {documentos.SelectMany(x => x.AttachmentContent).Count()}",
                })
            });

            await db.SaveChangesAsync();

            #region Subir los documentos al FTP

            List<string> documentosEncontrados = (await ftp.GetListing("", FtpListOption.Recursive))
                .Select(x => x.Name).ToList();

            //TODO: Añadir proveedores
            var dicCliente = db.tblAdmFacturaVenta.Where(x =>
                documentos.Select(x => ((Asiento)x.metadata).NCF).Contains(x.NCF)
            ).ToDictionary(x => x.NCF, x => x.idAdmClienteNavigation.codigo);

            List<AttachmentContentSetAttachmentContent> documentosSubidos = new();

            foreach (var documento in documentos)
            {
                foreach (var content in documento.AttachmentContent)
                {
                    if (documentosEncontrados.Any(file => file.Contains(content.ArchiveDocumentID))) continue;

                    var asiento = (Asiento)documento.metadata;
                    var extension = content.FileName.Split('.').Last();
                    var nombreArchivo = $"{asiento.FiscalYear}/{asiento.FiscalPeriod}/" +
                        $"P{(dicCliente.TryGetValue(asiento.NCF, out string BP) ? BP : asiento.AccountingDocument)}_{(string.IsNullOrEmpty(asiento.NCF) ? "NCF" : asiento.NCF)}_{content.ArchiveDocumentID}.{extension}";

                    try
                    {
                        using HttpResponseMessage attachmentContent = await sap.attachmentContentController.Get(
                            content.DocumentInfoRecordDocType,
                            content.DocumentInfoRecordDocNumber,
                            content.DocumentInfoRecordDocPart,
                            content.DocumentInfoRecordDocVersion,
                            content.LogicalDocument,
                            content.ArchiveDocumentID,
                            content.BusinessObjectTypeName
                        );
                        using var stream = await attachmentContent.Content.ReadAsStreamAsync();
                        using var objStream = new MemoryStream();
                        await stream.CopyToAsync(objStream);
                        var ftpResponse = await ftp.UploadStream(stream, nombreArchivo, FtpRemoteExists.Skip, true);
                        if (ftpResponse == FtpStatus.Success)
                        {
                            lock (documentosSubidos)
                            {
                                documentosSubidos.Add(content);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (db)
                        {
                            db.tblLogError.Add(new tblLogError
                            {
                                denominacion = "DocumentosSAP",
                                error = JsonConvert.SerializeObject(new List<string>
                                {
                                    $"Error al subir el archivo {nombreArchivo}",
                                    $"Error: {ex.Message}"
                                }),
                            });
                        }
                    }
                }
            }

            #endregion

            db.tblLogError.Add(new tblLogError
            {
                denominacion = "DocumentosSAP",
                error = JsonConvert.SerializeObject(new
                {
                    fecha = DateTime.Now,
                    message = JsonConvert.SerializeObject(new
                    {
                        msg = $"Documentos subidos: {documentosSubidos.Count}",
                        archivos = JsonConvert.SerializeObject(documentosSubidos)
                    }),
                })
            });

            await db.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        private class Documento
        {
            public object metadata { get; set; }
            public AllOriginals.AttachmentContentSetAttachmentContent[] AttachmentContent { get; set; }
        }

        public class Asiento
        {
            public string AccountingDocument { get; set; }
            public string FiscalYear { get; set; }
            public string FiscalPeriod { get; set; }
            [JsonProperty("DocumentReferenceID")]
            public string NCF { get; set; }
        }

        public class Factura
        {
            public string CompanyCode { get; set; }
            public string SupplierInvoice { get; set; }
            public string FiscalYear { get; set; }
            public string FiscalPeriod { get; set; }
            public DateTime? DocumentDate { get; set; }
            [JsonProperty("InvoicingParty")]
            public string BP { get; set; }
            [JsonProperty("SupplierPostingLineItemText")]
            public string NCF { get; set; }
        }
    }
}
