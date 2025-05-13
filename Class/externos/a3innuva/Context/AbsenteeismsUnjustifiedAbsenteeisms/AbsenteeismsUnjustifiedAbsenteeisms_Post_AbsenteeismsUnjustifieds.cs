namespace WebApiCore.Class.externos.a3innuva.Context.AbsenteeismsUnjustifiedAbsenteeisms
{
    public class AbsenteeismsUnjustifiedAbsenteeisms_Post_AbsenteeismsUnjustifieds
    {
        public string absenteeismID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool indLessThanOneDay { get; set; }
        public int durationHours { get; set; }
        public int durationMinuts { get; set; }
        public bool indDiscountRestsWeekly { get; set; }
        public bool indDiscountNaturalDays { get; set; }
        public DateTime lastUpdate { get; set; }
    }
}