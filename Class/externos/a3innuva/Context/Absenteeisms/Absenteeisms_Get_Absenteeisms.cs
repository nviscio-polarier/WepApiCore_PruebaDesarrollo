namespace WebApiCore.Class.externos.a3innuva.Context.Absenteeisms
{
    public class Absenteeisms_Get_Absenteeisms
    {
        public string absenteeismID { get; set; }
        public string typeOfItAbsenteeism { get; set; }
        public string employeeID { get; set; }
        public string employeeCode { get; set; }
        public int companyWorkplaceCode { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string description { get; set; }
        public decimal percentageDayAbsenteeisms { get; set; }
        public string absenteeismModeType { get; set; }
        public int securityProfileID { get; set; }
    }
}