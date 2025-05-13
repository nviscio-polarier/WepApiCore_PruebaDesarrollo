namespace WebApiCore.Class.externos.a3innuva.Context.TemporaryDisabilities
{
    public class TemporaryDisabilities_Get_TemporaryDisabilities
    {
        public string temporaryDisabilityID { get; set; }
        public string employeeID { get; set; }
        public string employeeCode { get; set; }
        public int incidentTypeID { get; set; }
        public string description { get; set; }
        public DateTime startdate { get; set; }
        public DateTime? enddate { get; set; }
        public bool indCovid19 { get; set; }
        public bool indHospitalization { get; set; }
        public bool indComplement { get; set; }
        public decimal percentagePartialMaternity { get; set; }
        public int benefitTypeId { get; set; }
        public int contingencyID { get; set; }
        public DateTime lastUpdate { get; set; }
        public bool indAutManEndDate { get; set; }
        public string relapsedIncidentID { get; set; }
        public string parentIncidentID { get; set; }
    }
}