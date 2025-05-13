namespace WebApiCore.Class.externos.a3innuva.Context.Pays
{
    public class Pays_Get_Pays_Year_Month
    {
        public DateTime payDate { get; set; }
        public string payID { get; set; }
        public string payType { get; set; }
        public int payTypeID { get; set; }
        public DateTime periodEndDate { get; set; }
        public DateTime periodStartDate { get; set; }
        public DateTime accruedStartDate { get; set; }
        public DateTime accruedEndDate { get; set; }
        public int companyCode { get; set; }
        public string workplaceCode { get; set; }
        public string workplaceName { get; set; }
        public string employeeId { get; set; }
        public string employeeCode { get; set; }
        public string completeName { get; set; }
        public string identifierNumber { get; set; }
        public decimal baseCC { get; set; }
        public decimal baseAC { get; set; }
        public decimal prorratedExtraPay { get; set; }
        public decimal totalBaseIRPF { get; set; }
        public decimal totalDeduction { get; set; }
        public decimal totalGross { get; set; }
        public decimal totalRemuneration { get; set; }
        public decimal costBusiness { get; set; }
        public DateTime systemDate { get; set; }
        public bool payLocked { get; set; }
        public bool hasMailTo { get; set; }
    }
}