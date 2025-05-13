namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Common%20data">Tablas comunes</a>
    /// </summary>
    public class Commondata
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Common%20data/DataLists_GetSexTypes">Relación tipos de género</a>
        /// </summary>
        public async Task<HttpResponseMessage> Get_Sextypes()
        {
            string endpoint = "api/sextypes";

            return await A3innuvaHttpService.Get(endpoint);
        }
    }
}
