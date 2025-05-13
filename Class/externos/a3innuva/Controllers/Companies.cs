namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Companies">Empresas</a>
    /// </summary>
    public class Companies
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Companies/CompaniesCollection_GetCollectionCompanies">Relación empresas</a>
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public async Task<HttpResponseMessage> Get_Companies(int pageNumber, int pageSize = 50)
        {
            string endpoint = $"api/companies?pageNumber={pageNumber}&pageSize={pageSize}";

            return await A3innuvaHttpService.Get(endpoint);
        }
    }
}
