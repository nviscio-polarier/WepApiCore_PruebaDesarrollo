using System.Net.Http;
using System.Text;

namespace WebApiCore.Class.externos.SAP
{
    public class SAPHttpService
    {

        private static readonly Uri url_CDS = new("https://integration-suite-ivgui6i4.it-cpi023-rt.cfapps.eu20-001.hana.ondemand.com/gw/odata/SAP/");
        private static readonly Uri url_Attachments = new("https://integration-suite-ivgui6i4.it-cpi023-rt.cfapps.eu20-001.hana.ondemand.com/http/prod/attachment/");
        private static readonly Uri url_ElectronicDocFile = new("https://integration-suite-ivgui6i4.it-cpi023-rt.cfapps.eu20-001.hana.ondemand.com/http/test/");

        private static readonly string user;
        private static readonly string password;
        private static readonly string credenciales;

        static SAPHttpService()
        {
            IConfigurationSection configuation_SAP = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build()
               .GetSection("SAP");
            user = configuation_SAP.GetSection("user").Value;
            password = configuation_SAP.GetSection("password").Value;
            credenciales = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{password}"));
        }

        public static async Task<HttpResponseMessage> Get_CDS(string endpoint)
        {
            HttpClient httpClient = new()
            {
                BaseAddress = url_CDS,
                DefaultRequestHeaders = {
                    Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credenciales)
                },
                Timeout = TimeSpan.FromMinutes(10)
            };

            return await httpClient.GetAsync(endpoint);
        }

        public static async Task<HttpResponseMessage> Get_Attachments(string endpoint)
        {
            HttpClient httpClient = new()
            {
                BaseAddress = url_Attachments,
                DefaultRequestHeaders = {
                    Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credenciales)
                },
                //Timeout = TimeSpan.FromMinutes(5)
            };

            return await httpClient.GetAsync(endpoint);
        }
        
        public static async Task<HttpResponseMessage> Get_ElectronicDocFile(string endpoint)
        {
            //if (Utils.isProduccion())
            //{
            HttpClient httpClient = new()
            {
                BaseAddress = url_ElectronicDocFile,
                DefaultRequestHeaders = {
                    Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credenciales)
                },
                //Timeout = TimeSpan.FromMinutes(5)
            };
            HttpResponseMessage response = await httpClient.GetAsync(endpoint); //HttpConnectionResponseContent
                                                                                //Console.WriteLine(response.Content);
            return response;
            //}
        }
    }
}
