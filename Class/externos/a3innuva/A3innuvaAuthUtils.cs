using Newtonsoft.Json;

namespace WebApiCore.Class.externos.a3innuva
{
    public class A3innuvaAuthUtils
    {
        private static string? access_token;
        private static string? refresh_token;
        private static readonly string? subscription_key;

        private static string pathAuthorizationJson = Path.Combine("ExternalServices", "a3innuva", "authorization.json");

        static A3innuvaAuthUtils()
        {
            LoadTokenFromFile();

            IConfigurationSection configuation_a3innuva = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build()
                .GetSection("a3innuva");

            subscription_key = configuation_a3innuva.GetSection("subscription_key").Value;
        }

        private static void LoadTokenFromFile()
        {
            try
            {
                string json = File.ReadAllText(pathAuthorizationJson);

                var tokenData = JsonConvert.DeserializeAnonymousType(json, new
                {
                    access_token = "",
                    refresh_token = "",
                });

                if (tokenData == null)
                    return;

                access_token = tokenData.access_token;
                refresh_token = tokenData.refresh_token;
            }
            catch (Exception ex)
            {
                // TODO: Manejar excepción
            }
        }

        public static void SaveTokenToFile()
        {
            string jsonString = JsonConvert.SerializeObject(new
            {
                access_token,
                refresh_token
            });

            // Guardar el token en el archivo authorization.json
            File.WriteAllText(pathAuthorizationJson, jsonString);
        }

        public static string? Get_access_token()
        {
            return access_token;
        }

        public static void Set_access_token(string token)
        {
            access_token = token;
        }

        public static string? Get_refresh_token()
        {
            return refresh_token;
        }

        public static void Set_refresh_token(string token)
        {
            refresh_token = token;
        }

        public static string? Get_subscription_key()
        {
            return subscription_key;
        }
    }
}
