namespace WebApiCore.Class.sms
{
    public class SMSRequest
    {
        public SMSRequest()
        {
            api_key = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build()
            .GetSection("SMS:VERIFICACION_TLF_API_KEY")
            .Value;

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
