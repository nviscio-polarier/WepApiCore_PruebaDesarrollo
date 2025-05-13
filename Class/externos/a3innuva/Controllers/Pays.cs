namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Pays">Pagas</a>
    /// </summary>
    public class Pays
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Pays/PayInfo_GetPaysStartDateEndDateTypeids">Relación de pagas calculadas por empleado</a>
        /// <br/>
        /// Consultar tipos de paga con <see cref="CommondataLaboral.Get_Paytypes">Get_Paytypes</see>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="fechaDesde">Fecha desde para la que búsqueda</param>
        /// <param name="fechaHasta">Fecha hasta para la que búsqueda</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        public async Task<HttpResponseMessage> Get_Pays(int companyCode, string employeeCode, DateTime fechaDesde, DateTime fechaHasta, int pageNumber, int pageSize = 50)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/pays?pageNumber={pageNumber}&pageSize={pageSize}&startDate={fechaDesde.ToString("yyyy-MM-dd")}&endDate={fechaHasta.ToString("yyyy-MM-dd")}";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Pays/PayInfo_GetEmployeePayData">Detalle cabecera paga</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="payId">Identificador de la paga</param>
        public async Task<HttpResponseMessage> Get_Paydata(int companyCode, string employeeCode, string payId)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/pays/{payId}/paydata";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Pays/PayInfo_GetCompanyPayData">Relación de pagas calculadas por empresa y fecha</a>
        /// <br/>
        /// Consultar tipos de paga con <see cref="CommondataLaboral.Get_Paytypes">Get_Paytypes</see>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="year">Año a consultar</param>
        /// <param name="month">Mes a consultar</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        public async Task<HttpResponseMessage> Get_Pays(int companyCode, int year, int month, int pageNumber, int pageSize = 50)
        {
            string endpoint = $"api/companies/{companyCode}/pays/{year}/{month}?pageNumber={pageNumber}&pageSize={pageSize}";

            return await A3innuvaHttpService.Get(endpoint);
        }
    }
}