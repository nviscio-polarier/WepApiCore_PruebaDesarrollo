using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace WebApiCore.Class.externos.a3innuva
{
    /// <summary>
    /// HttpService para la API de A3innuva
    /// </summary>
    public static class A3innuvaHttpService
    {
        private static readonly bool isProduccion = Utils.isProduccion();
        private static readonly string baseUrl = "https://a3api.wolterskluwer.es/Laboral/";
        private static readonly HttpClient httpClient = new()
        {
            BaseAddress = new Uri(baseUrl)
        };
        private static readonly HttpResponseMessage Unauthorized_error = new(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent("Servicio no disponible para servidores de prueba")
        };

        static A3innuvaHttpService()
        {
            if (isProduccion) // Si estamos en producción, añadimos el Auth Token y Subscription Key a la petición
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", A3innuvaAuthUtils.Get_access_token());
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", A3innuvaAuthUtils.Get_subscription_key());
            }
        }

        /// <summary>
        /// HttpService GET a3innuva. Añade el Auth Token y Subscription Key a la petición.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>Resultado de la llamada como string</returns>
        /// <exception cref="A3innuvaHttpServiceException"></exception>
        public static async Task<HttpResponseMessage> Get(string endpoint)
        {
            if (isProduccion)
            {
                var response = await httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                throw new A3innuvaHttpServiceException(response);
            }

            return Unauthorized_error;
        }

        /// <summary>
        /// HttpService POST a3innuva. Añade el Auth Token y Subscription Key a la petición.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="body"></param>
        /// <returns>Resultado de la llamada como string</returns>
        /// <exception cref="A3innuvaHttpServiceException"></exception>
        public static async Task<HttpResponseMessage> Post(string endpoint, dynamic body)
        {
            if (isProduccion)
            {
                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                throw new A3innuvaHttpServiceException(response);
            }

            return Unauthorized_error;
        }

        /// <summary>
        /// HttpService PUT a3innuva. Añade el Auth Token y Subscription Key a la petición.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>Resultado de la llamada como string</returns>
        /// <exception cref="A3innuvaHttpServiceException"></exception>
        public static async Task<HttpResponseMessage> Put(string endpoint)
        {
            if (isProduccion)
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { }), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                throw new A3innuvaHttpServiceException(response);
            }

            return Unauthorized_error;
        }

        /// <summary>
        /// HttpService PUT a3innuva. Añade el Auth Token y Subscription Key a la petición.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="body"></param>
        /// <returns>Resultado de la llamada como string</returns>
        /// <exception cref="A3innuvaHttpServiceException"></exception>
        public static async Task<HttpResponseMessage> Put(string endpoint, dynamic body)
        {
            if (isProduccion)
            {
                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                throw new A3innuvaHttpServiceException(response);
            }

            return Unauthorized_error;
        }

        /// <summary>
        /// HttpService DELETE a3innuva. Añade el Auth Token y Subscription Key a la petición.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>Resultado de la llamada como string</returns>
        /// <exception cref="A3innuvaHttpServiceException"></exception>
        public static async Task<HttpResponseMessage> Delete(string endpoint)
        {
            if (isProduccion)
            {
                var response = await httpClient.DeleteAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                throw new A3innuvaHttpServiceException(response);
            }

            return Unauthorized_error;
        }

        /// <summary>
        /// Refresca el access_token de la API de A3innuva
        /// </summary>
        public static void Refresh_acces_token()
        {
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", A3innuvaAuthUtils.Get_access_token());
        }
    }
}
