using System.Net;
using System.Net.Mail;

namespace WebApiCore.Class
{
    public class CorreoService
    {
        private readonly SmtpClient smtpClient = new();
        private readonly List<MailMessage> mails = new();

        private readonly string defaultFrom = "mypolarier@polarier.com";

        public CorreoService()
        {
            smtpClient.Host = "outlook.office365.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(defaultFrom, "Vog45080");
        }

        public CorreoService(string host, int port, bool enableSsl, NetworkCredential credentials)
        {
            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.EnableSsl = enableSsl;
            smtpClient.Credentials = credentials;
        }

        /// <summary>
        /// Añade un correo a la lista de correos a enviar
        /// </summary>
        /// <param name="mail"></param>
        public void Add(MailMessage mail)
        {
            mails.Add(mail);
        }

        /// <summary>
        /// Añade un correo a la lista de correos a enviar. Necesita una lista de emails, un asunto y un cuerpo
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void Add(string email, string subject, string body)
        {
            MailMessage mail = GetDefaultMail(subject, body);

            mail.To.Add(email);

            Add(mail);
        }

        /// <summary>
        /// Añade un correo a la lista de correos a enviar. Necesita una lista de emails, un asunto y un cuerpo
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void Add(List<string> emails, string subject, string body)
        {

            MailMessage mail = GetDefaultMail(subject, body);

            foreach (string email in emails)
            {
                mail.To.Add(email);
            }

            Add(mail);
        }

        /// <summary>
        /// Envía los correos de la lista
        /// </summary>
        public bool Send()
        {
            if (!Utils.isProduccion()) return false;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            foreach (var mail in mails)
            {
                smtpClient.Send(mail);
            }

            return true;
        }

        /// <summary>
        /// Devuelve un correo con los datos por defecto
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns>Correo con los datos por defecto</returns>
        public MailMessage GetDefaultMail(string subject, string body)
        {
            return new MailMessage()
            {
                From = new MailAddress(defaultFrom, "No Reply MyPolarier"),
                Subject = subject,
                SubjectEncoding = System.Text.Encoding.UTF8,
                Body = body,
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true,
                Priority = MailPriority.High
            };
        }
    }
}