namespace WebApiCore.Class.externos.a3innuva.Controllers
{
    /// <summary>
    /// <a href="https://a3developers.wolterskluwer.es/apis/a3laboral-services-api/swagger-ui#">Clase API a3innuva</a>
    /// </summary>
    public class A3innuva
    {
        public readonly Absenteeisms Absenteeisms = new();
        public readonly AbsenteeismsUnjustifiedAbsenteeisms AbsenteeismsUnjustifiedAbsenteeisms = new();
        public readonly Commondata Commondata = new();
        public readonly CommondataLaboral CommondataLaboral = new();
        public readonly Companies Companies = new();
        public readonly Employees Employees = new();
        public readonly EmployeesBankaccounts EmployeesBankaccounts = new();
        public readonly EmployeesConcepts EmployeesConcepts = new();
        public readonly EmployeeContract EmployeeContract = new();
        public readonly EmployeesLaborLife EmployeesLaborLife = new();
        public readonly EmployeesSalary EmployeesSalary = new();
        public readonly Pays Pays = new();
        public readonly PaysConcepts PaysConcepts = new();
        public readonly TemporaryDisabilities TemporaryDisabilities = new();
        public readonly VariableConcepts VariableConcepts = new();
        public readonly Workplaces Workplaces = new();

        /// <summary>
        /// Comprueba el estado de la API de a3innuva
        /// </summary>
        /// <returns>Booleano que define si la API de a3innuva esta disponible</returns>
        public async Task<bool> GetStatus()
        {
            var response = await Commondata.Get_Sextypes();
            return response != null;
        }
    }
}