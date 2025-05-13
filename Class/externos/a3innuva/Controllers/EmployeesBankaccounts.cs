using WebApiCore.Class.externos.a3innuva.Context.EmployeesBankaccounts;

namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Bank%20accounts">Empleados - Bancos</a>
    /// </summary>
    public class EmployeesBankaccounts
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Bank%20accounts/EmployeesBankAccount_GetBankAccount">Detalle de la cuenta bancaria principal del empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="bankAccountCode"></param>
        public async Task<HttpResponseMessage> Get_Bankaccount(int companyCode, string employeeCode, string bankAccountCode)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/bankaccount/{bankAccountCode}";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Bank%20accounts/EmployeesBankAccount_DeleteBankAccount">	Eliminar cuenta bancaria del empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// /// <param name="bankaccountcode">ID de la cuenta bancaria</param>
        public async Task<HttpResponseMessage> Delete_Bankaccount(int companyCode, string employeeCode, string bankaccountcode)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/bankaccount/{bankaccountcode}";

            return await A3innuvaHttpService.Delete(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Bank%20accounts/EmployeesBankAccount_PostBankAccounts">Crear cuenta bancaria para el empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Post_Bankaccounts(int companyCode, string employeeCode, EmployeesBankaccounts_Post_Bankaccounts body)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/bankaccounts";

            return await A3innuvaHttpService.Post(endpoint, body);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Bank%20accounts/EmployeesBankAccount_GetBankAccounts">Relación de las cuentas bancarias de los empleados</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        public async Task<HttpResponseMessage> Get_Bankaccounts(int companyCode, string employeeCode)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/bankaccounts";

            return await A3innuvaHttpService.Get(endpoint);
        }
    }
}
