namespace WebApiCore.Class.externos.a3innuva.Context.EmployeesSalary
{
    public class EmployeesSalary_Get_Salaryadjustments
    {
        public decimal amount { get; set; }
        public string liquidationType { get; set; }
        public AgreedSalaryExcess excess { get; set; }
        public AgreedSalaryExtraPays extraPayments { get; set; }
        public AgreedSalaryExcludedConcepts excludedConcepts { get; set; }
        public AgreedSalaryTaxationQuote taxationQuote { get; set; }
        public Indicators indicators { get; set; }
    }
}