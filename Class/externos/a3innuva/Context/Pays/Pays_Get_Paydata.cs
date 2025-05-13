namespace WebApiCore.Class.externos.a3innuva.Context.Pays
{
    public class Pays_Get_Paydata
    {
        public string payID { get; set; }
        public string identifierNumber { get; set; }
        public int workDayTypeID { get; set; }
        public decimal workDayPercentage { get; set; }
        public int ssAffiliationNumberPrefix { get; set; }
        public int ssAffiliationNumber { get; set; }
        public int ssAffiliationNumberSuffix { get; set; }
        public string agreementCode { get; set; }
        public int classificationCode { get; set; }
        public string classificationDescription { get; set; }
        public string regimeTypeID { get; set; }
        public int? tariffGroupID { get; set; }
        public int contractCode { get; set; }
        public int collectionTypeID { get; set; }
        public DateTime seniorityCalculationDate { get; set; }
        public int contributionTypeID { get; set; }
        public int indFormation { get; set; }
        public decimal commonContingenciesMax { get; set; }
        public decimal commonContingenciesMin { get; set; }
        public decimal accidentsUnemploymentMax { get; set; }
        public decimal accidentsUnemploymentMin { get; set; }
        public decimal accidentsTrainingMax { get; set; }
        public decimal accidentsTrainingMin { get; set; }
        public decimal accidentsFogasaMax { get; set; }
        public decimal accidentsFogasaMin { get; set; }
        public DateTime enrolmentDate { get; set; }
        public DateTime extraPayCalculationDate { get; set; }
        public string ssPaymentNumberID { get; set; }
        public int indNoResident { get; set; }
        public string agreementDescription { get; set; }
        public string contractDescription { get; set; }
        public string contributionTypeDescription { get; set; }
        public string ccGroupDescription { get; set; }
        public int subClassificationCode { get; set; }
        public string jobTitleCode { get; set; }
        public string? jobTitle { get; set; }
        public int contributionAgrarianModeID { get; set; }
        public bool indReductionRealDay { get; set; }
        public int agrarianCensusExclusionTypeID { get; set; }
        public DateTime labourPeriodStartDate { get; set; }
        public DateTime seniorityCompanyDate { get; set; }
        public int extensionsNumber { get; set; }
        public bool indIgnoreMinTax { get; set; }
        public string workplaceBankAccountID { get; set; }
        public bool indEmployeeSubClassification { get; set; }
        public int citricRegimeContributionTypeID { get; set; }
        public bool indENotGenSegmCRW { get; set; }
        public bool indEAutManNotGenSegmCRW { get; set; }
        public int citricRegimePermanenceCoefficientTypeID { get; set; }
        public bool payLocked { get; set; }
        public decimal baseAC { get; set; }
        public decimal baseCC { get; set; }
        public decimal prorratedExtraPay { get; set; }
        public decimal totalBaseIRPF { get; set; }
        public decimal totalDeduction { get; set; }
        public decimal totalGross { get; set; }
        public decimal totalRemuneration { get; set; }
        public decimal costBusiness { get; set; }
        public decimal percentageIMS { get; set; }
        public decimal percentageIT { get; set; }
        public bool indPublication { get; set; }
        public bool hasMailTo { get; set; }
    }
}