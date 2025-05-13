namespace WebApiCore.Class.externos.a3innuva.Context.Pays
{
    public class Pays_Get_Pays
    {
        public DateTime payDate { get; set; }
        public string payId { get; set; }
        public string payType { get; set; }
        public int payTypeId { get; set; }
        public DateTime periodEndDate { get; set; }
        public DateTime periodStartDate { get; set; }
        public DateTime accruedStartDate { get; set; }
        public DateTime accruedEndDate { get; set; }
        public bool indCheque { get; set; }
        public decimal totalDeductions { get; set; }
        public DateTime systemDate { get; set; }
        public bool payLocked { get; set; }
    }
}