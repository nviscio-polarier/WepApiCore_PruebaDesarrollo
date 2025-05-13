using WebApiCore.Class.externos.a3innuva.Context.EmployeeContract;

namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Contracts">Empleados - Contratos</a>
    /// </summary>
    public class EmployeeContract
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Contracts/EmployeeContracts_PostEmployeeContract">Actualizar el contrato de un empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Post_Contract(int companyCode, string employeeCode, EmployeeContract_Post_Contract body)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/contract";

            return await A3innuvaHttpService.Post(endpoint, body);
        }
    }
}
