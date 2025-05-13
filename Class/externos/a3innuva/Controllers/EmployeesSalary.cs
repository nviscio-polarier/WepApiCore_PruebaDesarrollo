using WebApiCore.Class.externos.a3innuva.Context.EmployeesSalary;

namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Salary">Empleados - Salario</a>
    /// </summary>
    public class EmployeesSalary
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Salary/EmployeeAgreedSalary_GetEmployeeSalaryAdjustments">Detalle líquido mensual pactado de un empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        public async Task<HttpResponseMessage> Get_Salaryadjustments(int companyCode, string employeeCode)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/salaryadjustments";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Salary/EmployeeAgreedSalary_SetEmployeeSalaryAdjustments">Actualizar líquido mensual pactado de un empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Put_Salaryadjustments(int companyCode, string employeeCode, EmployeesSalary_Put_Salaryadjustments body)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/salaryadjustments";

            return await A3innuvaHttpService.Put(endpoint, body);
        }
    }
}
