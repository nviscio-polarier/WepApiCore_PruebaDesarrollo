using System.Globalization;
using WebApiCore.Class.externos.a3innuva.Context.EmployeesConcepts;

namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Concepts">Empleados - Conceptos</a>
    /// </summary>
    public class EmployeesConcepts
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Concepts/PayInfo_GetPaysConcepts">Relación conceptos salariales del empleado (ficha)</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        public async Task<HttpResponseMessage> Get_Concepts(int companyCode, string employeeCode, int pageNumber, int pageSize = 50)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/concepts?pageNumber={pageNumber}&pageSize={pageSize}";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Concepts/PayInfo_AddPaysConcept">Crear concepto salarial de empleado (ficha)</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Post_Concepts(int companyCode, string employeeCode, EmployeesConcepts_Post_Concepts body)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/concepts";

            return await A3innuvaHttpService.Post(endpoint, body);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Concepts/PayInfo_RemovePaysConcept">Crear concepto salarial de empleado (ficha)</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="conceptCode">Código concepto</param>
        public async Task<HttpResponseMessage> Delete_Concepts(int companyCode, string employeeCode, string conceptCode)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/concepts?conceptCode={conceptCode}";

            return await A3innuvaHttpService.Delete(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Concepts/PayInfo_GetPaysConceptSByConceptId">Detalle concepto salarial de empleado (ficha)</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="conceptCode">Código concepto</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        public async Task<HttpResponseMessage> Get_Concepts(int companyCode, string employeeCode, string conceptCode, int pageNumber, int pageSize = 50)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/concepts/{conceptCode}?pageNumber={pageNumber}&pageSize={pageSize}";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Employees%20-%20Concepts/PayInfo_SetEmployeeConceptAmount">Crear concepto salarial de empleado (ficha)</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="conceptCode">Código concepto</param>
        /// <param name="amount">Cantidad nueva para el concepto</param>
        public async Task<HttpResponseMessage> Put_Amount(int companyCode, string employeeCode, string conceptCode, decimal amount)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/concepts/{conceptCode}/amount?amount={amount.ToString("0.00", CultureInfo.InvariantCulture)}";

            return await A3innuvaHttpService.Put(endpoint);
        }
    }
}
