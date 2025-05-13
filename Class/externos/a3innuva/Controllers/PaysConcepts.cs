namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Pays%20-%20Concepts">Pagas - Conceptos salariales</a>
    /// </summary>
    public class PaysConcepts
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Pays%20-%20Concepts/PayInfo_GetPaysConcept">Relación de conceptos salariales calculados en una paga</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="payId">Identificador de la paga</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        public async Task<HttpResponseMessage> Get_Concepts(int companyCode, string employeeCode, string payId, int pageNumber, int pageSize = 50)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/pays/{payId}/concepts?pageNumber={pageNumber}&pageSize={pageSize}";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Pays%20-%20Concepts/PayInfo_GetCalculatedInternalConcepts">	Relación de conceptos internos calculados por paga</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="payId">Identificador de la paga</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        public async Task<HttpResponseMessage> Get_Calculatedinternalconcepts(int companyCode, string employeeCode, string payId, int pageNumber, int pageSize = 50)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/pays/{payId}/calculatedinternalconcepts?pageNumber={pageNumber}&pageSize={pageSize}";

            return await A3innuvaHttpService.Get(endpoint);
        }
    }
}