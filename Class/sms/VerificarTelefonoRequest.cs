namespace WebApiCore.Class.sms
{
    public class VerificarTelefonoRequest
    {
        public VerificarTelefonoRequest(IConfiguration configuration)
        {
            api_key = configuration.GetSection("SMS:VERIFICACION_TLF_API_KEY").Value;
            messages = new List<Message>();
        }

        public string api_key { get; set; }
        public List<Message> messages { get; set; }

        public class Message
        {
            public string from { get; set; }
            public string to { get; set; }
            public string text { get; set; }
        }
    }
}