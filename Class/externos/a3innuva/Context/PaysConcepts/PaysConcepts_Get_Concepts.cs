namespace WebApiCore.Class.externos.a3innuva.Context.PaysConcepts
{
    public class PaysConcepts_Get_Concepts
    {
        public int conceptCode { get; set; }
        public string conceptType { get; set; }
        public string description { get; set; }
        public decimal payConceptAmount { get; set; }
        public decimal payConceptUnits { get; set; }
        public decimal payConceptUnitsAmount { get; set; }
        public bool indMonthlyPayCollection { get; set; }
        public bool indExtraPayCollection { get; set; }
        public int conceptCollectionTypeID { get; set; }
        public string conceptCollectionTypeDesc { get; set; }
    }
}