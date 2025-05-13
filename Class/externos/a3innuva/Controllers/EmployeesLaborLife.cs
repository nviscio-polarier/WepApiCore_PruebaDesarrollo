using WebApiCore.Class.externos.a3innuva.Context.EmployeesLaborLife;

namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Labor%20life">Empleados - Vida laboral</a>
    /// </summary>
    public class EmployeesLaborLife
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Labor%20life/CreateLaborLife">Crear vida laboral para un empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Post_CreateLaborlife(int companyCode, string employeeCode, EmployeesLaborLife_Post_CreateLaborlife body)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/create-laborlife";

            return await A3innuvaHttpService.Post(endpoint, body);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Labor%20life/EmployeesLaboral_GetEmployeeLaborLife">Relación vidas laborales de un empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        public async Task<HttpResponseMessage> Get_LabourLife(int companyCode, string employeeCode)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/labourLife";

            return await A3innuvaHttpService.Get(endpoint);
        }
    }
}