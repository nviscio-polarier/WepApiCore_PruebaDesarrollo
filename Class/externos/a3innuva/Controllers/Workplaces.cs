namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Workplaces">Centros de trabajo</a>
    /// </summary>
    public class Workplaces
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Workplaces/CompaniesWorkplacesCollection_GetCollectionWorkplaces">Relación de centros de trabajo por empresa</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        public async Task<HttpResponseMessage> Get_Workplaces(int companyCode, int pageNumber, int pageSize = 50)
        {
            string endpoint = $"api/companies/{companyCode}/workplaces?pageSize={pageSize}&pageNumber={pageNumber}";

            return await A3innuvaHttpService.Get(endpoint);
        }
    }
}
