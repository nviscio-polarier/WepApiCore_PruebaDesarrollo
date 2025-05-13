using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using WebApiCore.Class.externos.XMLBanco;
using WebApiCore.Context;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Enums.RRHH;
using static WebApiCore.Controllers.SolicitudFiniquitoController;

namespace WebApiCore.Class.Proyectos.MyPolarier.RRHH
{
    public class NotificacionesRRHH_GestoriaService
    {
        private readonly int DiasLimite_RRHH = 3;
        private readonly int DiasLimite_Gestoria = 3;

        private readonly Context.bdERP db;
        private SmtpClient smtpClient;
        private bool? isAlta;

        public NotificacionesRRHH_GestoriaService(Context.bdERP db, bool? isAlta)
        {
            this.db = db;

            // Generamos el paquete SMTP con la configuracion del servidor y las credenciales de acceso
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            smtpClient = new()
            {
                Host = "outlook.office365.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("mypolarier@polarier.com", "Vog45080"),
            };
            this.isAlta = isAlta;
        }

        #region Diarios

        public async Task<bool> SendAvisosDiarios_RRHH()
        {
            var subject = "Recordatorio solicitudes MyPolarier";

            var finiquitos = db.tblNomina.Where(x => 
                x.idEstadoNomina == (byte)idsEstadoNomina.SolicitudFiniquitoInterna && 
                x.fechaBaja < DateTime.UtcNow.Date
                ).ToList();

            var fechaLimite = DateTime.UtcNow.Date.AddDays(7);

            var incorporacionesPendientes = db.tblLlamamiento
                .Where(x => x.tblSolicitudAlta != null && x.fechaIni <= fechaLimite)
                .ToList();

            if (!finiquitos.Any() && !incorporacionesPendientes.Any())
                return false;

            var emails = GetCorreos_RRHH();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoRRHH_Diario");

                string htmlAltas;
                if (incorporacionesPendientes.Count > 0)
                    htmlAltas = $"Hay <b>{incorporacionesPendientes.Count}</b> incorporaciones pendientes de solicitar en los próximos <b>7 días</b>.";
                else
                    htmlAltas = $"No hay solicitudes de altas pendientes de validación.";

                mail.Body = mail.Body.Replace("@@htmlAltas", htmlAltas);

                string htmlFiniquitos;
                if (finiquitos.Count > 0)
                {
                    htmlFiniquitos = $"Hay {finiquitos.Count} finiquitos creados sin solicitar validación, y su fecha de baja se está aproximando.";
                    htmlFiniquitos += "<br/>";
                    htmlFiniquitos += $"Debes solicitar la validación de los finiquitos antes del <b>{finiquitos.Min(x => x.fechaBaja):dd/MM/yyyy}</b>.";
                }
                else
                    htmlFiniquitos = "No hay finiquitos pendientes de solicitar.";

                mail.Body = mail.Body.Replace("@@htmlFiniquitos", htmlFiniquitos);

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendAvisosDiarios_Gestoria()
        {
            var subject = "Recordatorio solicitudes MyPolarier";

            var finiquitos = db.tblNomina.Where(x => 
                (x.idEstadoNomina == (byte)idsEstadoNomina.EnProceso || 
                x.idEstadoNomina == (byte)idsEstadoNomina.ValidadoRRHH || 
                x.idEstadoNomina == (byte)idsEstadoNomina.SolicitudFiniquitoCreada
                ) && x.fechaBaja < DateTime.UtcNow.Date
            ).ToList();

            var altas = db.tblSolicitudAlta
                .Include(x => x.idSolicitudAltaNavigation)
                .Where(x =>
                x.idEstadoSolicitudAlta != (byte)idsEstadoSolicitudAlta.Validado
            ).ToList();

            if (!finiquitos.Any() && !altas.Any())
                return false;

            var emails = GetCorreos_Gestoria();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoGestoria_Diario");

                var altasPendientes = altas.Where(x => x.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.Pendiente || x.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.EnProceso).Count();
                var altasDocumentacion = altas.Where(x => x.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.PendienteDocs).Count();
                var fechaLimiteAltas = $"{altas.Where(x => x.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.Pendiente || x.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.EnProceso).Min(x => x.idSolicitudAltaNavigation.fechaIni):dd/MM/yyyy}";

                string htmlAltas;
                if (altasPendientes > 0)
                    htmlAltas = $"Hay <b>{altasPendientes}</b> solicitudes de alta pendientes de validación. La fecha de alta más próxima es el <b>{fechaLimiteAltas}</b>.";
                else
                    htmlAltas = $"No hay solicitudes de altas pendientes de validación.";

                htmlAltas += "<br/>";

                if (altasDocumentacion > 0)
                    htmlAltas += $"Hay <b>{altasDocumentacion}</b> solicitudes de alta con documentación pendiente.";
                else
                    htmlAltas += "No hay solicitudes de altas con documentación pendiente.";

                mail.Body = mail.Body.Replace("@@htmlAltas", htmlAltas);

                string htmlFiniquitos;
                if (finiquitos.Count > 0)
                    htmlFiniquitos = $"Hay <b>{finiquitos.Count}</b> finiquitos creados pendientes de validación, y su fecha de baja se está aproximando.";
                else
                    htmlFiniquitos = "No hay finiquitos pendientes de validación.";
                mail.Body = mail.Body.Replace("@@htmlFiniquitos", htmlFiniquitos);

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Finiquitos

        public async Task<bool> SendAvisos_NuevosFiniquitos(int finiquitosCreados)
        {
            var subject = "Nuevos finiquitos en MyPolarier.";

            var finiquitosPendientes = db.tblNomina.Count(x => 
                x.idTipoNomina == (short)idsTipoNomina.PagaFiniquito &&
                (
                    x.idEstadoNomina == (byte)idsEstadoNomina.EnProceso ||
                    x.idEstadoNomina == (byte)idsEstadoNomina.ValidadoRRHH ||
                    x.idEstadoNomina == (byte)idsEstadoNomina.SolicitudFiniquitoCreada
                )
                );

            var emails = GetCorreos_Gestoria();
            var cc = GetCorreos_RRHH();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoGestoria_NuevosFiniquitos", cc);

                mail.Body = mail.Body.Replace("@@finiquitosCreados", finiquitosCreados.ToString());
                mail.Body = mail.Body.Replace("@@finiquitosPendientes", finiquitosPendientes.ToString());

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendAvisos_FiniquitoValidado(tblNomina finiquito)
        {
            var subject = $"Finiquito validado: {finiquito.nombreCompleto}";
            var emails = GetCorreos_RRHH();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoRRHH_FiniquitoValidado");

                mail.Body = mail.Body.Replace("@@nombreCompleto", finiquito.nombreCompleto);

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendAvisos_FiniquitoIncidenciaCreada(tblNomina finiquito)
        {
            var subject = $"Incidencia de finiquito creada";
            var emails = GetCorreos_RRHH();

            var finiquitosPendientes = db.tblNomina.Count(x =>
                x.idTipoNomina == (short)idsTipoNomina.PagaFiniquito &&
                (
                    x.idEstadoNomina == (byte)idsEstadoNomina.EnProceso ||
                    x.idEstadoNomina == (byte)idsEstadoNomina.ValidadoRRHH ||
                    x.idEstadoNomina == (byte)idsEstadoNomina.SolicitudFiniquitoCreada
                )
                );

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoRRHH_FiniquitoIncidenciaCreada");

                mail.Body = mail.Body.Replace("@@nombreCompleto", finiquito.nombreCompleto);
                mail.Body = mail.Body.Replace("@@finiquitosPendientes", finiquitosPendientes.ToString());

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendAvisos_FiniquitoIncidenciaSolucionada(tblNomina finiquito)
        {
            var subject = $"Incidencia de finiquito solucionada";
            var emails = GetCorreos_Gestoria();
            var cc = GetCorreos_RRHH();

            var finiquitosPendientes = db.tblNomina.Count(x =>
                x.idTipoNomina == (short)idsTipoNomina.PagaFiniquito &&
                (
                    x.idEstadoNomina == (byte)idsEstadoNomina.EnProceso ||
                    x.idEstadoNomina == (byte)idsEstadoNomina.ValidadoRRHH ||
                    x.idEstadoNomina == (byte)idsEstadoNomina.SolicitudFiniquitoCreada
                )
                );

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoRRHH_FiniquitoIncidenciaSolucionada", cc);

                mail.Body = mail.Body.Replace("@@nombreCompleto", finiquito.nombreCompleto);
                mail.Body = mail.Body.Replace("@@finiquitosPendientes", finiquitosPendientes.ToString());

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendAvisos_FiniquitoValidadoContabilizar(List<FiniquitoNominaPair> ids, List<tblHistoricoAsientoNomina?> ultimoHistoricoAsientoNomina)
        {
            var subject = $"Finiquitos validados para liquidar";
            var emails = GetCorreos_Contabilidad();

            var finiquitosPendientes = db.tblNomina.Count(x =>
                x.idTipoNomina == (short)idsTipoNomina.PagaFiniquito &&
                x.idEstadoNomina == (byte)idsEstadoNomina.ValidadoGestoria
                );

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoContabilidad_FiniquitoValidadoContabilizar");

                var idsFiniquito = ids.Select(x => x.idFiniquito).ToList();
                var idsNomina = ids.Select(x => x.idNomina).ToList();

                var liquidosPercibirAnteriores = ultimoHistoricoAsientoNomina
                    .Where(h => h != null)
                    .ToDictionary(h => h!.idNomina, h => h.liquidoPercibir);

                // TODO: Filtrar nominas ya validadas
                var tblNomina = db.tblNomina
                    .Include(x => x.idEmpresaPolarierNavigation)
                        .ThenInclude(x => x.idMonedaNavigation)
                    .Include(x => x.idEmpresaPolarierNavigation)
                        .ThenInclude(x => x.idPaisNavigation)
                    .Include(x => x.idAdmCentroCosteNavigation)
                    .Include(x => x.idAdmElementoPEPNavigation)
                    .Include(x => x.idPersonaNavigation)
                    .Where(x => (idsFiniquito.Contains(x.idNomina) || idsNomina.Contains(x.idNomina)))
                    .ToList();

                var idsPersona = tblNomina.Select(n => n.idPersona).Distinct().ToList();

                var personasFechasBaja = tblNomina.Select(n => new { n.idPersona, n.fechaBaja }).Distinct().ToList();

                var nominasOtrosTipos = db.tblNomina
                    .Where(n =>
                        idsPersona.Contains(n.idPersona)
                        && !idsFiniquito.Contains(n.idNomina)
                        && !idsNomina.Contains(n.idNomina)
                    )
                    .ToList();

                tblNomina.AddRange(
                    nominasOtrosTipos.Where(n =>
                        personasFechasBaja.Any(pfb => pfb.idPersona == n.idPersona && pfb.fechaBaja == n.fechaBaja)
                    )
                );

                var XMLs = tblNomina
                    .GroupBy(x =>
                    {

                        var domicilio = $"{x.idPersonaNavigation.calle}, {x.idPersonaNavigation.numDomicilio}";
                        if (!string.IsNullOrEmpty(x.idPersonaNavigation.piso) && !string.IsNullOrEmpty(x.idPersonaNavigation.puerta))
                        {
                            domicilio += $", {x.idPersonaNavigation.piso}, {x.idPersonaNavigation.puerta}";
                        }

                        var tipoPaga = x.idTipoNomina == (short)idsTipoNomina.PagaAtrasos ? "Paga Atrasos" : "Paga habitual";

                        return new
                        {
                            x.idPersona,
                            x.fechaBaja,
                            x.idEmpresaPolarierNavigation,
                            x.idAdmCentroCosteNavigation,
                            x.idAdmElementoPEPNavigation,
                            fecha = $"{new DateTime(x.fechaHasta.Year, x.fechaHasta.Month, DateTime.DaysInMonth(x.fechaHasta.Year, x.fechaHasta.Month)):yyyy-MM}",
                            x.IBAN,
                            x.numDocumentoIdentidad,
                            x.nombreCompleto,
                            domicilio = domicilio,
                            tipoPaga
                        };
                    })
                    .Select(x => new XMLBancoUtils.XMLBanco_Empleado
                    {
                        idEmpresaPolarierNavigation = x.Key.idEmpresaPolarierNavigation,
                        idAdmCentroCosteNavigation = x.Key.idAdmCentroCosteNavigation,
                        idAdmElementoPEPNavigation = x.Key.idAdmElementoPEPNavigation,
                        fecha = x.Key.fecha,
                        liquidoPercibir = Math.Max(0, x.Sum(n =>
                        {
                            var liquidoPercibirAnterior = liquidosPercibirAnteriores.TryGetValue(n.idNomina, out var valor) ? valor : 0;
                            return (n.liquidoPercibir - liquidoPercibirAnterior) ?? 0;
                        })),
                        IBAN = x.Key.IBAN,
                        numDocumentoIdentidad = x.Key.numDocumentoIdentidad,
                        nombreCompleto = x.Key.nombreCompleto,
                        domicilio = x.Key.domicilio
                    })
                    .GroupBy(x => new
                    {
                        x.idEmpresaPolarierNavigation,
                        x.idAdmCentroCosteNavigation,
                        x.idAdmElementoPEPNavigation,
                        x.fecha
                    });

                foreach (var item in XMLs)
                {
                    var nombreArchivo = $"{item.Key.idAdmCentroCosteNavigation?.denominacion ?? item.Key.idAdmElementoPEPNavigation?.denominacion} - {item.Key.fecha}.xml";

                    var xml = XMLBancoUtils.GetXMLBanco(item);
                    MemoryStream xmlStream = XMLBancoUtils.SerializeToStream(xml);

                    Attachment objAttach = new(xmlStream, nombreArchivo, "application/xml");
                    objAttach.TransferEncoding = TransferEncoding.Base64;
                    mail.Attachments.Add(objAttach);

                }


                mail.Body = mail.Body.Replace("@@finiquitosValidados", ids.Count.ToString());
                mail.Body = mail.Body.Replace("@@finiquitosPendientes", finiquitosPendientes.ToString());

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Altas

        public async Task<bool> SendAvisos_NuevasAltas(int altasCreadas)
        {
            var subject = "Nuevas solicitudes de alta en MyPolarier.";

            var altasPendiente = db.tblSolicitudAlta.Count(x => x.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.Pendiente || x.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.EnProceso);
            var altasPendienteDocs = db.tblSolicitudAlta.Count(x => x.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.PendienteDocs);

            var emails = GetCorreos_Gestoria();
            var cc = GetCorreos_RRHH();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoGestoria_NuevasAltas", cc);

                mail.Body = mail.Body.Replace("@@altasCreadas", altasCreadas.ToString());
                mail.Body = mail.Body.Replace("@@altasPendientes", altasPendiente.ToString());

                var htmlPendienteDocs = $"</br>{(altasPendienteDocs > 0 ? $"Hay <b>{altasPendienteDocs}</b> solicitudes de alta con documentación pendiente." : "No hay solicitudes de alta con documentación pendiente.")}";
                mail.Body = mail.Body.Replace("@@altasPendienteDocs", htmlPendienteDocs);

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendAvisos_AltaValidada(tblSolicitudAlta alta)
        {
            var subject = $"Solicitud de alta validada: {alta.idSolicitudAltaNavigation.idPersonaNavigation.nombre} {alta.idSolicitudAltaNavigation.idPersonaNavigation.apellidos}";
            var emails = GetCorreos_RRHH();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoRRHH_AltaValidada");

                mail.Body = mail.Body.Replace("@@nombreCompleto", $"{ alta.idSolicitudAltaNavigation.idPersonaNavigation.nombre} { alta.idSolicitudAltaNavigation.idPersonaNavigation.apellidos}");
                mail.Body = mail.Body.Replace("@@fechaAlta", $"{alta.idSolicitudAltaNavigation.fechaIni:dd/MM/yyyy}");

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        public async Task<bool> SendAvisos_AltaSS(tblSolicitudAlta alta)
        {
            var subject = $"Solicitud de alta validada: {alta.idSolicitudAltaNavigation.idPersonaNavigation.nombre} {alta.idSolicitudAltaNavigation.idPersonaNavigation.apellidos}";
            var emails = GetCorreos_RRHH();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoRRHH_AltaSS");

                mail.Body = mail.Body.Replace("@@nombreCompleto", $"{ alta.idSolicitudAltaNavigation.idPersonaNavigation.nombre} { alta.idSolicitudAltaNavigation.idPersonaNavigation.apellidos}");
                mail.Body = mail.Body.Replace("@@fechaAlta", $"{alta.idSolicitudAltaNavigation.fechaIni:dd/MM/yyyy}");

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendAvisos_AltaIncidenciaCreada(tblSolicitudAlta alta, bool isIncidenciaRRHH)
        {
            var subject = $"Incidencia de solicitud de alta creada";
            var emails = isIncidenciaRRHH ? GetCorreos_RRHH() : GetCorreos_Gestoria();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAviso_AltaIncidenciaCreada");

                // TODO: Cambiar por el nombre al realizar cambio en la base de datos.
                mail.Body = mail.Body.Replace("@@nombreCompleto", alta.idSolicitudAltaNavigation.idPersonaNavigation.nombre + " " + alta.idSolicitudAltaNavigation.idPersonaNavigation.apellidos);

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendAvisos_AltaIncidenciaSolucionada(tblSolicitudAlta alta, bool isIncidenciaRRHH)
        {
            var subject = $"Incidencia de solicitud de alta solucionada";
            var emails = isIncidenciaRRHH ? GetCorreos_Gestoria() : GetCorreos_RRHH();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAviso_AltaIncidenciaSolucionada");

                mail.Body = mail.Body.Replace("@@nombreCompleto", alta.idSolicitudAltaNavigation.idPersonaNavigation.nombre + " " + alta.idSolicitudAltaNavigation.idPersonaNavigation.apellidos);

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendAvisos_AltaPetAnulacion(tblSolicitudAlta alta)
        {
            var subject = $"Petición de anulación de alta creada";
            var emails = GetCorreos_Gestoria();
            var cc = GetCorreos_RRHH();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoGestoria_AltaPetAnulacion", cc);

                // TODO: Cambiar por el nombre al realizar cambio en la base de datos.
                mail.Body = mail.Body.Replace("@@nombreCompleto", alta.idSolicitudAltaNavigation.idPersonaNavigation.nombre + " " + alta.idSolicitudAltaNavigation.idPersonaNavigation.apellidos);

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendAvisos_AltaAnulada(tblSolicitudAlta alta)
        {
            var subject = $"Solicitud de alta anulada";
            var emails = GetCorreos_RRHH();

            try
            {
                var mail = GetMailTemplate(subject, emails, "EmailAvisoRRHH_AltaAnulada");

                mail.Body = mail.Body.Replace("@@nombreCompleto", alta.idSolicitudAltaNavigation.idPersonaNavigation.nombre + " " + alta.idSolicitudAltaNavigation.idPersonaNavigation.apellidos);

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        private MailMessage GetMailTemplate(string subject, List<string> emails, string plantilla, List<string>? cc = null)
        {
            // Generamos el mail a enviar
            MailMessage mail = new()
            {
                From = new MailAddress("mypolarier@polarier.com", "No Reply MyPolarier")
            };

            emails.ForEach(email => mail.To.Add(email));
            cc?.ForEach(email => mail.CC.Add(email));

            GetCorreos_TEST().ForEach(email => mail.CC.Add(email));

            mail.Subject = Utils.isProduccion() ? subject : "TEST - " + subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            mail.Body = string.Empty;

            using (StreamReader reader = new StreamReader(@"./Pages/" + plantilla + ".html"))
            {
                mail.Body = reader.ReadToEnd();
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = "Media/Imagenes";
            string fullPath = Path.Combine(currentDirectory, path, "logoLoveYourLinen.png");

            Attachment objAttach = new Attachment(fullPath);
            mail.Attachments.Add(objAttach);

            mail.Body = mail.Body.Replace("@@img", objAttach.ContentId);

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            return mail;
        }

        private List<string> GetCorreos_Gestoria()
        {
            if (Utils.isProduccion())
            {
                return db.tblPermiso
                    .Include(x => x.idUsuario)
                    .Where(x => 
                        isAlta == null ? 
                            x.codigo == idsPermiso.SolicitudesAltaGestoria || x.codigo == idsPermiso.GestionFiniquitosGestoria
                        : isAlta == true ? 
                            x.codigo == idsPermiso.SolicitudesAltaGestoria
                        :
                        x.codigo == idsPermiso.GestionFiniquitosGestoria
                    )
                    .SelectMany(x => x.idUsuario)
                    .Select(x => x.email ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
            }
            else
            {
                return GetCorreos_TEST();
            }
        }

        private List<string> GetCorreos_RRHH()
        {
            if (Utils.isProduccion())
            {
                return db.tblPermiso
                    .Include(x => x.idUsuario)
                    .Where(x =>
                        isAlta == null ?
                            x.codigo == idsPermiso.SolicitudesAltaRRHH || x.codigo == idsPermiso.GestionFiniquitosRRHH
                        : isAlta == true ?
                            x.codigo == idsPermiso.SolicitudesAltaRRHH
                        :
                        x.codigo == idsPermiso.GestionFiniquitosRRHH
                    )
                    .SelectMany(x => x.idUsuario)
                    .Select(x => x.email ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
            }
            else
            {
                return GetCorreos_TEST();
            }
        }

        private List<string> GetCorreos_Contabilidad()
        {
            if (Utils.isProduccion())
            {
                return db.tblPermiso
                    .Include(x => x.idUsuario)
                    .Where(x => x.codigo == idsPermiso.RecibirCorreoFiniquitosContabilidad)
                    .SelectMany(x => x.idUsuario)
                    .Select(x => x.email ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
            }
            else
            {
                return GetCorreos_TEST();
            }
        }

        private static List<string> GetCorreos_TEST()
        {
            if (Utils.isDevelopment())
                return new List<string> { "acarrascosa@polarier.com" };
            else
                return new List<string> { "acarrascosa@polarier.com", "aramis@polarier.com", "dtorrello@polarier.com", "nalfonso@polarier.com" };
        }
    }
}
