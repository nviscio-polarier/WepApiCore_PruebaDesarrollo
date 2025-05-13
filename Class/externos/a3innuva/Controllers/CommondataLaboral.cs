namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Common%20data%20-%20Laboral">Tablas comunes - Laboral</a>
    /// </summary>
    public class CommondataLaboral
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Common%20data%20-%20Laboral/DataLists_GetPayTypes">Relación tipos de pago</a>
        /// </summary>
        public async Task<HttpResponseMessage> Get_Paytypes()
        {
            string endpoint = "api/paytypes";

            return await A3innuvaHttpService.Get(endpoint);
        }
    }
}