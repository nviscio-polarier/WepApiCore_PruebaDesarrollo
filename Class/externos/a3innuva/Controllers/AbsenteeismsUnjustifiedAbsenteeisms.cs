using WebApiCore.Class.externos.a3innuva.Context.AbsenteeismsUnjustifiedAbsenteeisms;

namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Absenteeisms%20-%20Unjustified%20absenteeisms">Absentismos - Absentismos injustificados</a>
    /// </summary>
    public class AbsenteeismsUnjustifiedAbsenteeisms
    {
        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Absenteeisms%20-%20Unjustified%20absenteeisms/UnjustifiedAbsenteeismEntity_DeleteMethodDetail">Eliminar absentismo injustificado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="unjustifiedAbsenteeismID"></param>
        public async Task<HttpResponseMessage> Delete_AbsenteeismsUnjustifieds(int companyCode, string employeeCode, string unjustifiedAbsenteeismID)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/absenteeisms/unjustifieds/{unjustifiedAbsenteeismID}";

            return await A3innuvaHttpService.Delete(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Absenteeisms%20-%20Unjustified%20absenteeisms/UnjustifiedAbsenteeismCollection_GetEmployeeUnjustifiedAbsenses">Relación de absentismos injustificados</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        public async Task<HttpResponseMessage> Get_AbsenteeismsUnjustifieds(int companyCode, string employeeCode, int pageNumber, int pageSize = 50, string? filter = null)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/absenteeisms/unjustifieds?pageNumber={pageNumber}&pageSize={pageSize}".AddA3innuvaFilters(filter);

            return await A3innuvaHttpService.Get(endpoint);
        }

        /// <summary>
        /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#/Absenteeisms%20-%20Unjustified%20absenteeisms/UnjustifiedAbsenteeismEntity_PostUnjustifiedAbsenteeism">Crear absentismo injustificado</a>
        /// </summary>
        /// <param name="companyCode">Código empresa</param>
        /// <param name="employeeCode">Código empleado</param>
        /// <param name="body">Cuerpo de la llamada</param>
        public async Task<HttpResponseMessage> Post_AbsenteeismsUnjustifieds(int companyCode, string employeeCode, AbsenteeismsUnjustifiedAbsenteeisms_Post_AbsenteeismsUnjustifieds body)
        {
            string endpoint = $"api/companies/{companyCode}/employees/{employeeCode}/absenteeisms/unjustifieds";

            return await A3innuvaHttpService.Post(endpoint, body);
        }
    }
}