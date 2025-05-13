using System.Net.Mail;
using WebApiCore.Class.sms;
using WebApiCore.Enums.RRHH;

namespace WebApiCore.Class.Proyectos.MyPolarier.RRHH
{
    public class Aviso_PlusesNominaService
    {
        private readonly string from = "Polarier";
        private readonly string text = "Recordatorio:\nRecuerda que debes rellenar los pluses de este mes.";

        private readonly Context.bdERP db;
        private readonly IHttpClientFactory clientFactory;

        private readonly int idConceptoNomina_PlusActividad = 2;
        private readonly int numDias_Correo = 5;
        private readonly int numDias_Correo_SMS = 2;
        private readonly int idLavanderia_SonCastello = 14;
        private readonly int idCentroTrabajo_OficinaSonCastello = 1;

        public Aviso_PlusesNominaService(Context.bdERP db, IHttpClientFactory clientFactory)
        {
            this.db = db;
            this.clientFactory = clientFactory;
        }

        private MailMessage Notificar_pluses(string diasRestantes, string fechaLimite)
        {
            // Creación de un nuevo mensaje de correo electrónico
            MailMessage mail = new MailMessage();

            // Se inicializa con una cadena vacía y luego se lee el contenido de un archivo HTML

            mail.Body = string.Empty;
            using (StreamReader reader = new StreamReader(@"./Pages/EmailAvisoPlus.html"))
            {
                mail.Body = reader.ReadToEnd();
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = "Media/Imagenes";
            string fullPath = Path.Combine(currentDirectory, path, "logoLoveYourLinen.png");

            Attachment objAttach = new Attachment(fullPath);
            mail.Attachments.Add(objAttach);

            mail.Body = mail.Body.Replace("@@diasRestantes ", diasRestantes);
            mail.Body = mail.Body.Replace("@@fechaLimite", fechaLimite);
            mail.Body = mail.Body.Replace("@@img", objAttach.ContentId);
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            return mail;
        }

        /// <summary>
        /// Envia un aviso por SMS a los encargados que todavía tienen que asignar pluses a los empleados
        /// </summary>
        public async Task EnviarAviso_PlusesNomina()
        {
            if (!Utils.isProduccion()) return; // Los SMS solo se envían en producción

            DateTime fechaDesde = new(DateTime.Now.Year, DateTime.Now.Month, 1);
            var fechaHasta = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day);

            var contactoResponsables = (from ttnu in db.tblTipoTrabajoNUsuario
                                        join p in db.tblPersona on new { ttnu.idLavanderia, ttnu.idTipoTrabajo } equals new { idLavanderia = (int)p.idLavanderia, idTipoTrabajo = (byte)p.idTipoTrabajo }
                                        join pntc in db.tblPersonaNTipoContrato on p.idPersona equals pntc.idPersona
                                        join n in db.tblNomina on p.idPersona equals n.idPersona into nominaGroup
                                        from nomina in nominaGroup.DefaultIfEmpty()
                                        join u in db.tblUsuario on ttnu.idUsuario equals u.idUsuario
                                        join p_u in db.tblPersona on u.idPersona equals p_u.idPersona
                                        join cl in db.tblCalendarioLavanderia on p.idLavanderia equals cl.idLavanderia
                                        where ttnu.gestionaPlusesNomina
                                           && pntc.fechaAltaContrato <= fechaHasta
                                           && (pntc.fechaBajaContrato == null || pntc.fechaBajaContrato >= fechaDesde)
                                           && (
                                              nomina == null
                                              || nomina.fechaDesde >= fechaDesde && nomina.fechaHasta <= fechaHasta
                                              || pntc.fechaBajaContrato == null || pntc.fechaBajaContrato >= nomina.fechaHasta
                                           )
                                           && (
                                              nomina == null
                                              || !nomina.tblConceptoNominaNNomina.Any(cn => cn.idConceptoNomina == idConceptoNomina_PlusActividad)
                                           )
                                           && cl.fecha >= fechaDesde && cl.fecha <= fechaHasta
                                           && cl.idCalendario_Estado == (byte)idsCalendario_Estado.CierreNomina
                                        select new
                                        {
                                            telefono = $"+{p_u.prefijoTelefonico}{p_u.telefonoEmpresa ?? p_u.telefono}",
                                            u.email,
                                            diasHastaCierreNominas = cl.fecha.Subtract(DateTime.Now.Date).Days,
                                            cl.fecha
                                        }
                          )
                          .Distinct()
                          .ToList()
                          .Where(cr => cr.diasHastaCierreNominas == numDias_Correo || cr.diasHastaCierreNominas == numDias_Correo_SMS)
                          .Select(cr => new
                          {
                              telefono = cr.diasHastaCierreNominas == numDias_Correo_SMS ? cr.telefono : null,
                              cr.email,
                              cr.diasHastaCierreNominas,
                              fechaCierreNomina = cr.fecha
                          })
                          .ToList();

            contactoResponsables.AddRange(
                    (
                        from p in db.tblPersona
                        join pntc in db.tblPersonaNTipoContrato on p.idPersona equals pntc.idPersona
                        join n in db.tblNomina on p.idPersona equals n.idPersona into nominaGroup
                        from nomina in nominaGroup.DefaultIfEmpty()
                        join u in db.tblUsuario on p.idUsuario_validacion_nomina equals u.idUsuario
                        join p_u in db.tblPersona on u.idPersona equals p_u.idPersona
                        from cl in db.tblCalendarioLavanderia
                            .Where(cl => cl.idLavanderia == idLavanderia_SonCastello && cl.idCalendario_Estado == (byte)idsCalendario_Estado.CierreNomina
                                && cl.fecha >= fechaDesde && cl.fecha <= fechaHasta)
                            .DefaultIfEmpty()
                        where
                            p_u.idCentroTrabajo == idCentroTrabajo_OficinaSonCastello
                            && pntc.fechaAltaContrato <= fechaHasta
                            && (pntc.fechaBajaContrato == null || pntc.fechaBajaContrato >= fechaDesde)
                            && (
                                nomina == null
                                || nomina.fechaDesde >= fechaDesde && nomina.fechaHasta <= fechaHasta
                                || pntc.fechaBajaContrato == null || pntc.fechaBajaContrato >= nomina.fechaHasta
                            )
                            && (
                                nomina == null
                                || !nomina.tblConceptoNominaNNomina.Any(cn => cn.idConceptoNomina == idConceptoNomina_PlusActividad)
                            )
                        select new
                        {
                            telefono = $"+{p_u.prefijoTelefonico}{p_u.telefonoEmpresa ?? p_u.telefono}",
                            u.email,
                            diasHastaCierreNominas = cl.fecha.Subtract(DateTime.Now.Date).Days,
                            fechaCierreNomina = cl.fecha
                        }
                    )
                    .Distinct()
                    .ToList()
                    .Where(cr => cr.diasHastaCierreNominas == numDias_Correo || cr.diasHastaCierreNominas == numDias_Correo_SMS)
                );

            CorreoService correoService = new();

            SMSService.DefaultCofig defaultCofig = new()
            {
                from = from,
                text = text
            };

            SMSService smsService = new(clientFactory, defaultCofig);

            foreach (var cr in contactoResponsables)
            {
                var email = cr.email;
                var diasRestantes = cr.diasHastaCierreNominas;
                var diaCierre = cr.fechaCierreNomina.Day;
                var msgDiaCierre = " " + diaCierre + " a las 23:59h";
                var mail = Notificar_pluses(diasRestantes.ToString(), msgDiaCierre);

                mail.Subject = "¡Quedan solo " + diasRestantes + " días!";
                mail.To.Add(email);
                mail.From = new MailAddress("mypolarier@polarier.com", "No Reply MyPolarier");

                correoService.Add(mail);

                smsService.Add(cr.telefono);
            }

            await smsService.Send();
            correoService.Send();
        }
    }
}
