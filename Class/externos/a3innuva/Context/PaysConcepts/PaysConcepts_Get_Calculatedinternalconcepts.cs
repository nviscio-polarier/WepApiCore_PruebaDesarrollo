namespace WebApiCore.Class.externos.a3innuva.Context.PaysConcepts
{
    public class PaysConcepts_Get_Calculatedinternalconcepts
    {
        public bool indPayrollSheet { get; set; }
        public bool indDeduction { get; set; }
        public bool indForcedConcept { get; set; }
        public bool indPaysIRPFTax { get; set; }
        public bool indQuotesCC { get; set; }
        public bool indQuotesPC { get; set; }
        public int internalConceptID { get; set; }
        public string longDescription { get; set; }
        public string shortDescription { get; set; }
        public decimal calculatedAmount { get; set; }
        public decimal calculatedPercentage { get; set; }
        public decimal calculatedUnits { get; set; }
        public int craID { get; set; }
        public int? compensationDeductionID { get; set; }
    }
}