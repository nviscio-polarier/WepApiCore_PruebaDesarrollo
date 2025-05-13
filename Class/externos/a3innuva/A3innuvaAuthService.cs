using Newtonsoft.Json;

namespace WebApiCore.Class.externos.a3innuva
{
    public class A3innuvaAuthService
    {
        private static string? client_id;
        private static string? client_secret;

        // URL del servicio de tokens
        private readonly string tokenEndpoint = "https://login.wolterskluwer.eu/auth/core/connect/token";

        public A3innuvaAuthService()
        {
            IConfigurationSection configuation_a3innuva = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build()
                .GetSection("a3innuva");

            client_id = configuation_a3innuva.GetSection("client_id").Value;
            client_secret = configuation_a3innuva.GetSection("client_secret").Value;
        }

        public async Task<TokenResponse?> Refresh_acces_token(string? refresh_token = null)
        {
            // Datos del cuerpo de la solicitud
            var requestData = new Dictionary<string, string>
                {
                    { "client_id", client_id },
                    { "client_secret", client_secret },
                    { "grant_type", "refresh_token" },
                    { "refresh_token", refresh_token ?? A3innuvaAuthUtils.Get_refresh_token() }
                };

            // Crear el contenido del cuerpo de la solicitud
            var content = new FormUrlEncodedContent(requestData);

            using var httpClient = new HttpClient();

            // Realizar la solicitud POST
            var response = await httpClient.PostAsync(tokenEndpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var newAuth = JsonConvert.DeserializeObject<TokenResponse>(responseString);

            if (newAuth != null)
            {
                // Guardar el nuevo token en memoria
                A3innuvaAuthUtils.Set_access_token(newAuth.access_token);
                A3innuvaAuthUtils.Set_refresh_token(newAuth.refresh_token);

                // Refrescar el nuevo token de las llamadas a la API
                A3innuvaHttpService.Refresh_acces_token();

                // Guardar el nuevo token en el archivo authorization.json
                A3innuvaAuthUtils.SaveTokenToFile();
            }

            return newAuth;
        }

        public class TokenResponse
        {
            public string id_token { get; set; }
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }
            public string refresh_token { get; set; }
        }
    }
}
