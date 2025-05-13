using WebApiCore.Class.externos.a3innuva.Context.VariableConcepts;

namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Variable%20concepts">Conceptos variables</a>
    /// </summary>
    public class VariableConcepts
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Variable%20concepts/EmployeesVariableConcepts_GetVariableConceptInDateRange">Relación conceptos variables por empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        public async Task<HttpResponseMessage> Get_Variableconcepts(int companyCode, string employeeCode, DateTime fechaDesde, DateTime fechaHasta, int pageNumber, int pageSize = 50)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/variableconcepts/{fechaDesde.ToString("yyyy-MM-dd")}/{fechaHasta.ToString("yyyy-MM-dd")}?pageSize={pageSize}&pageNumber={pageNumber}";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Variable%20concepts/EmployeesVariableConcepts_GetVariableConcept">Detalle de un concepto de variable</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="variableConceptCode">Código del concepto</param>
        /// <param name="fechaDesde">Fecha de inicio del concepto</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        public async Task<HttpResponseMessage> Get_Variableconcepts(int companyCode, string employeeCode, string variableConceptCode, DateTime fechaDesde, int pageNumber, int pageSize = 50)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/variableconcepts/{variableConceptCode}?pageSize={pageSize}&pageNumber={pageNumber}&startDate={fechaDesde.ToString("yyyy-MM-dd")}";

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Variable%20concepts/EmployeesVariableConcepts_PostVariableConcept">Crear concepto de variable para un empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Post_Variableconcepts(int companyCode, string employeeCode, VariableConcepts_Post_Variableconcepts body)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/variableconcepts";

            return await A3innuvaHttpService.Post(endpoint, body);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Variable%20concepts/EmployeesVariableConcepts_DeleteVariableConcept">Eliminar concepto variable para un empleado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// /// <param name="variableConceptCode">ID concepto variable</param>
        public async Task<HttpResponseMessage> Delete_Variableconcepts(int companyCode, string employeeCode, string variableConceptCode)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/variableconcepts/{variableConceptCode}";

            return await A3innuvaHttpService.Delete(endpoint);
        }
    }
}
