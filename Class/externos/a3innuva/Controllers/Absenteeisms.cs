namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Absenteeisms">Absentismos</a>
    /// </summary>
    public class Absenteeisms
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Absenteeisms/EmployeeAbsenteeims_GetEmployeeAbsenteeismsForYear">Relación de absentismo por empleado y año</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="year"></param>
        public async Task<HttpResponseMessage> Get_Absenteeisms(int companyCode, string employeeCode, int year)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/absenteeisms/{year}";

            return await A3innuvaHttpService.Get(endpoint);
        }
    }
}