using System.Net;
using System.Text.RegularExpressions;

namespace WebApiCore.Class.externos.a3innuva
{
    public class A3innuvaHttpServiceException : Exception
    {
        public readonly HttpResponseMessage? response;
        public readonly HttpStatusCode? statusCode;

        public A3innuvaHttpServiceException(string mensaje) : base(mensaje)
        {
        }

        public A3innuvaHttpServiceException(HttpResponseMessage response) : base(GetMessage(response).Result)
        {
            this.response = response;
            statusCode = response.StatusCode;
        }

        /// <summary>
        /// Devuelve el mensaje de error de la respuesta
        /// </summary>
        /// <param name="response"></param>
        /// <returns>Mensaje de error de la respuesta</returns>
        private static async Task<string> GetMessage(HttpResponseMessage response)
        {
            var result = await A3innuvaUtils.DeserializeResponseAsync(response);
            return (result?.message ?? (string)response.ReasonPhrase) ?? "null";
        }

        /// <summary>
        /// Se usa con respuestas de error 429 (Too Many Requests). Devuelve el número de segundos que hay que esperar antes de volver a hacer la petición.
        /// </summary>
        /// <returns>Número de segundos que hay que esperar antes de volver a hacer la petición</returns>
        public int Get_SecondsToWait()
        {
            // Utilizar una expresión regular para extraer el número
            Match match = Regex.Match(Message, @"\d+");

            // Verificar si se encontró un número
            if (match.Success)
            {
                return int.Parse(match.Value);
            }
            return 0;
        }
    }
}
