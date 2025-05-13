using Newtonsoft.Json;
using System.Xml.Serialization;

namespace WebApiCore.Class.externos.SAP
{
    public class SAPUtils
    {
        /// <summary>
        /// Revisa si la respuesta está vacía. Se utiliza en llamadas que devuelven más de un objeto
        /// </summary>
        /// <param name="response"></param>
        /// <returns>Booleano que define si la respuesta está vacía o no</returns>
        public async static Task<bool> IsEmptyResponseAsync(HttpResponseMessage response)
        {
            var resultString = await response.Content.ReadAsStringAsync();

            return string.IsNullOrEmpty(resultString) || resultString == "[]" || resultString == "null";
        }

        ///// <summary>
        ///// Convierte una respuesta en un objeto dinámico
        ///// </summary>
        ///// <param name="response"></param>
        ///// <returns>Objeto dinámico</returns>
        //public static async Task<dynamic> DeserializeResponseAsync(HttpResponseMessage response)
        //{
        //    return JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        //}

        /// <summary>
        /// Convierte una respuesta en un objeto del tipo deseado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns>Objeto del tipo deseado</returns>
        public static async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            XmlSerializer serializer = new(typeof(T));
            T returnObject = (T)serializer.Deserialize(response.Content.ReadAsStream());
            return returnObject;
        }

        /// <summary>
        /// Permite recuperar un objeto del tipo deseado a partir de la llamada correspondiente
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTask"></param>
        /// <returns></returns>
        public static async Task<T> GetResponseAndSerialize<T>(Task<HttpResponseMessage> responseTask)
        {
            HttpResponseMessage response = await responseTask;
            string xmlString = await response.Content.ReadAsStringAsync();
            XmlSerializer serializer = new(typeof(T));
            using (StringReader stringReader = new(xmlString))
            {
                T model = (T)serializer.Deserialize(stringReader);
                return model;
            }
        }

        public static void TryDeserializeResponse<T>(HttpResponseMessage responseTask, out T model)
        {
            try
            {
                var response = DeserializeResponseAsync<T>(responseTask);
                    response.Wait();
                model = response.Result;
            } 
            catch (Exception e)
            {
                model = default;
            }
        }

        //public static T DeserializeXMLFileToObject<T>(string XmlFilename)
        //{
        //    T returnObject = default(T);
        //    if (string.IsNullOrEmpty(XmlFilename)) return default(T);

        //    try
        //    {
        //        StreamReader xmlStream = new StreamReader(XmlFilename);
        //        XmlSerializer serializer = new XmlSerializer(typeof(T));
        //        returnObject = (T)serializer.Deserialize(xmlStream);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionLogger.WriteExceptionToConsole(ex, DateTime.Now);
        //    }
        //    return returnObject;
        //}

        /// <summary>
        /// Maneja la paginación de las respuestas. Se utiliza en llamadas que devuelven más de un objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="llamadaSAP"></param>
        /// <returns>Lista de objetos que devuelve la llamada de SAP</returns>
        /// <example>
        /// Se puede utilizar de la siguiente manera:
        /// <code>
        ///     Func<int, Task<HttpResponseMessage>> llamadaGET = async (pageNumber) =>
        ///     await SAPinnuva.ApartadoSAP.Get_Llamada(parametro1, parametro2, parametro3, pageNumber);
        ///     
        ///     List<TipoDevuelto> resultado = await SAPUtils.HandlePagination<TipoDevuelto>(llamadaGET);
        /// </code>
        /// </example>
        public static async Task<List<T>> HandlePagination<T>(Func<int, Task<HttpResponseMessage>> llamadaSAP)
        {
            List<T> result = new();

            int? pageNumber = 1;

            while (true)
            {
                var response = await llamadaSAP((int)pageNumber);

                if (await IsEmptyResponseAsync(response))
                {
                    break;
                }

                var resultParsed = await DeserializeResponseAsync<List<T>>(response);
                result.AddRange(resultParsed);

                var paginationHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();

                if (paginationHeader != null)
                {
                    var paginationInfo = JsonConvert.DeserializeObject<dynamic>(paginationHeader);

                    if (paginationInfo != null && paginationInfo?.currentPage < paginationInfo?.totalPages)
                    {
                        pageNumber = paginationInfo?.currentPage + 1;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return result;
        }
    }
}
