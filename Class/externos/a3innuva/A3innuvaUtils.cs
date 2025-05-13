using Newtonsoft.Json;
using WebApiCore.Class.externos.a3innuva.Context.Employees;
using WebApiCore.Class.externos.a3innuva.Controllers;
using WebApiCore.Context;
using WebApiCore.Enums.A3innuva;
using WebApiCore.Enums.Assistant;
using WebApiCore.Enums.General;

namespace WebApiCore.Class.externos.a3innuva
{
    public class A3innuvaUtils
    {
        /// <summary>
        /// Revisa si la respuesta está vacía. Se utiliza en llamadas que devuelven más de un objeto
        /// </summary>
        /// <param name="response"></param>
        /// <returns>Booleano que define si la respuesta está vacía o no</returns>
        public async static Task<bool> IsEmptyResponseAsync(HttpResponseMessage response)
        {
            var resultString = await response.Content.ReadAsStringAsync();

            return string.IsNullOrEmpty(resultString) || resultString == "[]" || resultString == "null";
        }

        /// <summary>
        /// Convierte una respuesta en un objeto dinámico
        /// </summary>
        /// <param name="response"></param>
        /// <returns>Objeto dinámico</returns>
        public static async Task<dynamic> DeserializeResponseAsync(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Convierte una respuesta en un objeto del tipo deseado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns>Objeto del tipo deseado</returns>
        public static async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            var result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

            return result == null ? Activator.CreateInstance<T>() : result;
        }

        /// <summary>
        /// Maneja la paginación de las respuestas. Se utiliza en llamadas que devuelven más de un objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="llamadaA3"></param>
        /// <returns>Lista de objetos que devuelve la llamada de A3</returns>
        /// <example>
        /// Se puede utilizar de la siguiente manera:
        /// <code>
        ///     Func<int, Task<HttpResponseMessage>> llamadaGET = async (pageNumber) =>
        ///     await a3innuva.ApartadoA3.Get_Llamada(parametro1, parametro2, parametro3, pageNumber);
        ///     
        ///     List<TipoDevuelto> resultado = await A3innuvaUtils.HandlePagination<TipoDevuelto>(llamadaGET);
        /// </code>
        /// </example>
        public static async Task<List<T>> HandlePagination<T>(Func<int, Task<HttpResponseMessage>> llamadaA3)
        {
            List<T> result = new();

            int? pageNumber = 1;

            while (true)
            {
                var response = await llamadaA3((int)pageNumber);

                if (await IsEmptyResponseAsync(response))
                {
                    break;
                }

                var resultParsed = await DeserializeResponseAsync<List<T>>(response);
                result.AddRange(resultParsed);

                var paginationHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();

                if (paginationHeader != null)
                {
                    var paginationInfo = JsonConvert.DeserializeObject<dynamic>(paginationHeader);

                    if (paginationInfo != null && paginationInfo?.currentPage < paginationInfo?.totalPages)
                    {
                        pageNumber = paginationInfo?.currentPage + 1;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Devuelve un diccionario, por cada empresa sus empleados.
        /// </summary>
        /// <param name="a3innuva"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static async Task<Dictionary<int, List<Employees_Get_Employees>>> GetAllEmployeesAsync(A3innuva a3innuva, string? filter = null)
        {
            var allCompanyCodes = Enum.GetValues(typeof(companiesCodes)).Cast<int>().ToList();

            Dictionary<int, List<Employees_Get_Employees>> result = new();

            foreach (var companyCode in allCompanyCodes)
            {
                Func<int, Task<HttpResponseMessage>> getEmployeeConcepts = async (pageNumber) => await a3innuva.Employees.Get_Employees(companyCode, pageNumber++, 50, filter);

                result.Add(companyCode, await HandlePagination<Employees_Get_Employees>(getEmployeeConcepts));
            }

            return result;
        }

        /// <summary>
        /// Obtiene el companyCode de A3 equivalente a un idEmpresaPolarier de Polarier
        /// </summary>
        /// <param name="idEmpresaPolarier"></param>
        public static int? GetCompanyCode(short idEmpresaPolarier)
        {
            var companyCodeMap = new Dictionary<short, int>
                {
                    { (short)idsEmpresaPolarier.Polarier, (int)companiesCodes.Polarier },
                    { (short)idsEmpresaPolarier.PolIbiza, (int)companiesCodes.PolarierIbiza },
                    { (short)idsEmpresaPolarier.PolAndalucia, (int)companiesCodes.PolarierAndalucia }
                };

            return companyCodeMap.TryGetValue(idEmpresaPolarier, out int companyCode) ? companyCode : null;
        }

        /// <summary>
        /// Obtiene el companyCode de la persona
        /// </summary>
        /// <param name="tblLavanderia"></param>
        /// <param name="persona"></param>
        public static int? GetCompanyCode(List<tblLavanderia> tblLavanderia, tblPersona persona)
        {
            List<int?> idsLavanderiaIbiza = new() { (int)idsLavanderia.PolarierIbiza };
            List<int?> idsLavanderiaAndalucia = new() { (int)idsLavanderia.PolarierAndalucia };
            List<int?> idsLavanderiaResto = tblLavanderia
               .Where(l => l.idPais == (int)idsPais.España && idsLavanderiaIbiza.All(ili => ili != l.idLavanderia) && idsLavanderiaAndalucia.All(ila => ila != l.idLavanderia))
               .Select(l => (int?)l.idLavanderia)
               .ToList();

            List<int?> idsCentroTrabajoCustom = new() { (int)idsCentroTrabajo.OficinaSonCastello };

            int? companyCode = idsLavanderiaResto.Any(il => il == persona.idLavanderia)
                    ? (int)companiesCodes.Polarier
                    : idsLavanderiaIbiza.Any(ili => ili == persona.idLavanderia)
                        ? (int)companiesCodes.PolarierIbiza
                        : idsLavanderiaAndalucia.Any(ili => ili == persona.idLavanderia)
                            ? (int)companiesCodes.PolarierAndalucia
                            : idsCentroTrabajoCustom.Any(ict => ict == persona.idCentroTrabajo)
                                ? (int)companiesCodes.Polarier
                                : null;

            return companyCode;
        }

        public static int GetAcuerdoNC(int companyCode)
        {
            if (companyCode == (int)companiesCodes.PolarierIbiza) return (int)conceptsCodes.AcuerdoNCPolarierIbiza;

            if (companyCode == (int)companiesCodes.PolarierAndalucia) return (int)conceptsCodes.AcuerdoNCPolarierAndalucia;

            // Por defecto es el de Polarier.
            return (int)conceptsCodes.AcuerdoNCPolarier;
        }
    }
}
