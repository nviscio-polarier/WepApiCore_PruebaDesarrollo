using WebApiCore.Class.externos.a3innuva.Context.Employees;
using static WebApiCore.Class.externos.a3innuva.A3innuvaHttpUtils;

namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees">Empleados</a>
    /// </summary>
    public class Employees
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees/GetEmployees">Relación de empleados</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Get_Employees(int companyCode, int pageNumber, int pageSize = 50, A3innuvaFilter? filter = null)
        {
            string endpoint = $"api/companies/{companyCode}/employees?pageNumber={pageNumber}&pageSize={pageSize}".AddA3innuvaFilter(filter);

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees/GetEmployees">Relación de empleados</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Get_Employees(int companyCode, int pageNumber, int pageSize = 50, string? filter = null)
        {
            string endpoint = $"api/companies/{companyCode}/employees?pageNumber={pageNumber}&pageSize={pageSize}".AddA3innuvaFilters(filter);

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees/CreateEmployee">Crear empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Post_CreateEmployee(int companyCode, Employees_Post_CreateEmployee body)
        {
            string endpoint = $"api/companies/{companyCode}/employees/create-employee?entityMode=Default";

            return await A3innuvaHttpService.Post(endpoint, body);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees/EmployeesIdentification_GetEmployeeIdentification">Detalle datos identificativos del empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        public async Task<HttpResponseMessage> Get_Identification(int companyCode, string employeeCode)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/identification";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees/EmployeesIdentification_PutEmployeeDetails">Actualizar datos identificativos del empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Put_Identification(int companyCode, string employeeCode, dynamic body)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/identification";

            return await A3innuvaHttpService.Put(endpoint, body);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees/HiringDatesEntity_GetEmployeeHiringDatesInformation">Detalle fechas de contratación del empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        public async Task<HttpResponseMessage> Get_Hiringdates(int companyCode, string employeeCode)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/hiringdates";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees/HiringDatesEntity_PutEmployeeHiringDatesInformation">Actualizar fechas de contratación del empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Put_Hiringdates(int companyCode, string employeeCode, Employees_Put_Hiringdates body)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/hiringdates";

            return await A3innuvaHttpService.Put(endpoint, body);
        }
    }
}