namespace WebApiCore.Class.externos.a3innuva.Context.EmployeesBankaccounts
{
    public class EmployeesBankaccounts_Get_Bankaccount
    {
        public string entity { get; set; }
        public string agency { get; set; }
        public string digitControl { get; set; }
        public string account { get; set; }
        public string iban { get; set; }
        public string ibanSufix { get; set; }
        public string holder { get; set; }
        public bool isMainAccount { get; set; }
        public decimal distributionAmount { get; set; }
        public decimal distributionPercentage { get; set; }
        public string bic { get; set; }
    }
}