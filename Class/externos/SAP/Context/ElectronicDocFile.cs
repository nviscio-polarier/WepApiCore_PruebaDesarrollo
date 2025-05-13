namespace WebApiCore.Class.externos.SAP.Context
{

    public class ElectronicDocFile
    {
        public Electronicdocfile_Type[] ElectronicDocFile_Type { get; set; }
    }

    public class Electronicdocfile_Type
    {
        public string ElectronicDocCompanyCode { get; set; }
        public string ElectronicDocUUID { get; set; }
        public string ElectronicDocCountry { get; set; }
        public string ElectronicDocSourceType { get; set; }
        public string ElectronicDocSourceKey { get; set; }
        public string ElectronicDocType { get; set; }
        public string ElectronicDocProcessStatus_Text { get; set; }
        public string ElectronicDocLastChangeDate { get; set; }
        public string ElectronicDocLastChangeTime { get; set; }
        public string ElectronicDocCreationDate { get; set; }
        public string ElectronicDocCreationTime { get; set; }
    }

}
