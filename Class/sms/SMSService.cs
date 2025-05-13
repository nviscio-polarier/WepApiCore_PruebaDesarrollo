using Newtonsoft.Json;
using System.Text;

namespace WebApiCore.Class.sms
{
    public class SMSService
    {
        private readonly string url = "https://api.gateway360.com/api/3.0/sms/send";
        private readonly string? defaultFrom;
        private readonly string? defaultText;
        private readonly IHttpClientFactory clientFactory;

        private readonly SMSRequest smsRequest = new();

        public SMSService(IHttpClientFactory clientFactory, DefaultCofig defaultCofig)
        {
            this.clientFactory = clientFactory;
            defaultFrom = defaultCofig.from;
            defaultText = defaultCofig.text;
        }

        public SMSService(IHttpClientFactory clientFactory, string from)
        {
            this.clientFactory = clientFactory;
            this.defaultFrom = from;
        }

        /// <summary>
        /// Añade un mensaje SMS
        /// </summary>
        /// <param name="message"></param>
        public void Add(SMSRequest.Message message)
        {
            smsRequest.messages.Add(message);
        }

        /// <summary>
        /// Añade un mensaje SMS
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="text"></param>
        public void Add(string to, string from, string text)
        {
            SMSRequest.Message message = new()
            {
                to = to,
                from = from,
                text = text
            };

            smsRequest.messages.Add(message);
        }

        /// <summary>
        /// Añade un mensaje SMS con el remitente por defecto
        /// </summary>
        /// <param name="to"></param>
        /// <param name="text"></param>
        public void Add(string to, string text)
        {
            SMSRequest.Message message = new()
            {
                to = to,
                from = defaultFrom,
                text = text
            };

            smsRequest.messages.Add(message);
        }

        /// <summary>
        /// Añade un mensaje SMS con el texto y el remitente por defecto
        /// </summary>
        /// <param name="to"></param>
        public void Add(string to)
        {
            SMSRequest.Message message = new()
            {
                to = to,
                from = defaultFrom,
                text = defaultText
            };

            smsRequest.messages.Add(message);
        }

        /// <summary>
        /// Envia los mensajes SMS
        /// </summary>
        /// <returns>Booleano que define si ha ido bien o no el envío</returns>
        public async Task<bool> Send()
        {
            if (!Utils.isProduccion()) return false;

            try
            {
                smsRequest.messages = smsRequest.messages
                    .GroupBy(m => new { m.from, m.to, m.text })
                    .Select(g => g.First())
                    .ToList();

                var content = JsonConvert.SerializeObject(smsRequest);

                using HttpClient httpClient = clientFactory.CreateClient();
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, stringContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public class DefaultCofig
        {
            public string? from { get; set; }
            public string? text { get; set; }
        }
    }
}
