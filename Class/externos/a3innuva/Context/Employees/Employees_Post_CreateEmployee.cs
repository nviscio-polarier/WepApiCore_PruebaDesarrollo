namespace WebApiCore.Class.externos.a3innuva.Context.Employees
{
    public class Employees_Post_CreateEmployee
    {
        public int workplaceCode { get; set; }
        public Identification identification { get; set; }
        public Agreement agreement { get; set; }
        public Contribution contribution { get; set; }
        public Address address { get; set; }

        public class Identification
        {
            public int identificationType { get; set; }
            public string identifierNumber { get; set; }
            public string employeeCode { get; set; }
            public string legalName { get; set; }
            public string secondName1 { get; set; }
            public string secondName2 { get; set; }
            public string sex { get; set; }
            public string ssAffiliationNumberPrefix { get; set; }
            public string ssAffiliationNumber { get; set; }
            public string ssAffiliationNumberSuffix { get; set; }
            public DateTime enrollmentDate { get; set; }
            public string personType { get; set; }
            public string birthRegion { get; set; }
            public string birthCountry { get; set; }
        }

        public class Agreement
        {
            public string companyAgreementCode { get; set; }
            public int classificationCode { get; set; }
            public string jobPositionCode { get; set; }
            public int subClassificationCode { get; set; }
            public string cnoOccupationID { get; set; }
        }

        public class Contribution
        {
            public int contributionTypeID { get; set; }
            public string regime { get; set; }
            public int tariffGroupID { get; set; }
            public string occupationCodeTGSS { get; set; }
            public decimal ims { get; set; }
            public decimal it { get; set; }
            public int paymentFrequency { get; set; }
        }

        public class Address
        {
            public string acronymID { get; set; }
            public string streetName { get; set; }
            public string buildingNumber { get; set; }
            public string stairwell { get; set; }
            public string floor { get; set; }
            public string gate { get; set; }
            public string town { get; set; }
            public string postalCode { get; set; }
            public int countryID { get; set; }
            public string poBox { get; set; }
        }
    }
}