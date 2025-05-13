namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Temporary%20Disabilities%20(IT)">Absentismos</a>
    /// </summary>
    public class TemporaryDisabilities
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Temporary%20Disabilities%20(IT)/EmployeesTemporaryDiabilities_GetEmployeeTemporaryDisabilitiesForYear">Relación ITs por empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="year"></param>
        public async Task<HttpResponseMessage> Get_TemporaryDisabilities(int companyCode, string employeeCode, int year)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/temporaryDisabilities/{year}";

            return await A3innuvaHttpService.Get(endpoint);
        }
    }
}