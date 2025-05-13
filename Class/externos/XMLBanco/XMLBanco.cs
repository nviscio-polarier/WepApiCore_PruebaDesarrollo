using System.ComponentModel;
using System.Xml.Serialization;

namespace WebApiCore.Class.externos.XMLBanco
{

    // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    [XmlRoot(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03", IsNullable = false, ElementName = "Document")]
    public class XMLBanco
    {

        private DocumentCstmrCdtTrfInitn cstmrCdtTrfInitnField;

        [XmlElement("CstmrCdtTrfInitn")]
        public DocumentCstmrCdtTrfInitn CstmrCdtTrfInitn
        {
            get
            {
                return this.cstmrCdtTrfInitnField;
            }
            set
            {
                this.cstmrCdtTrfInitnField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitn
    {

        private DocumentCstmrCdtTrfInitnGrpHdr grpHdrField;

        private DocumentCstmrCdtTrfInitnPmtInf pmtInfField;

        [XmlElement("GrpHdr")]
        public DocumentCstmrCdtTrfInitnGrpHdr GrpHdr
        {
            get
            {
                return this.grpHdrField;
            }
            set
            {
                this.grpHdrField = value;
            }
        }

        [XmlElement("PmtInf")]
        public DocumentCstmrCdtTrfInitnPmtInf PmtInf
        {
            get
            {
                return this.pmtInfField;
            }
            set
            {
                this.pmtInfField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnGrpHdr
    {

        private string msgIdField;

        private string creDtTmField;

        private int nbOfTxsField;

        private DocumentCstmrCdtTrfInitnGrpHdrInitgPty initgPtyField;

        [XmlElement("MsgId")]
        public string MsgId
        {
            get
            {
                return this.msgIdField;
            }
            set
            {
                this.msgIdField = value;
            }
        }

        [XmlElement("CreDtTm")]
        public string CreDtTm
        {
            get
            {
                return this.creDtTmField;
            }
            set
            {
                this.creDtTmField = value;
            }
        }

        [XmlElement("NbOfTxs")]
        public int NbOfTxs
        {
            get
            {
                return this.nbOfTxsField;
            }
            set
            {
                this.nbOfTxsField = value;
            }
        }

        [XmlElement("InitgPty")]
        public DocumentCstmrCdtTrfInitnGrpHdrInitgPty InitgPty
        {
            get
            {
                return this.initgPtyField;
            }
            set
            {
                this.initgPtyField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnGrpHdrInitgPty
    {

        private string nmField;

        private DocumentCstmrCdtTrfInitnGrpHdrInitgPtyID idField;

        [XmlElement("Nm")]
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        [XmlElement("Id")]
        public DocumentCstmrCdtTrfInitnGrpHdrInitgPtyID Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnGrpHdrInitgPtyID
    {

        private DocumentCstmrCdtTrfInitnGrpHdrInitgPtyIDOrgId orgIdField;

        [XmlElement("OrgId")]
        public DocumentCstmrCdtTrfInitnGrpHdrInitgPtyIDOrgId OrgId
        {
            get
            {
                return this.orgIdField;
            }
            set
            {
                this.orgIdField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnGrpHdrInitgPtyIDOrgId
    {

        private DocumentCstmrCdtTrfInitnGrpHdrInitgPtyIDOrgIdOthr othrField;

        [XmlElement("Othr")]
        public DocumentCstmrCdtTrfInitnGrpHdrInitgPtyIDOrgIdOthr Othr
        {
            get
            {
                return this.othrField;
            }
            set
            {
                this.othrField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnGrpHdrInitgPtyIDOrgIdOthr
    {

        private string idField;

        [XmlElement("Id")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInf
    {

        private string pmtInfIdField;

        private string pmtMtdField;

        private bool btchBookgField;

        private int nbOfTxsField;

        private DocumentCstmrCdtTrfInitnPmtInfPmtTpInf pmtTpInfField;

        private string reqdExctnDtField;

        private DocumentCstmrCdtTrfInitnPmtInfDbtr dbtrField;

        private DocumentCstmrCdtTrfInitnPmtInfDbtrAcct dbtrAcctField;

        private DocumentCstmrCdtTrfInitnPmtInfDbtrAgt dbtrAgtField;

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInf[] cdtTrfTxInfField;

        [XmlElement("PmtInfId")]
        public string PmtInfId
        {
            get
            {
                return this.pmtInfIdField;
            }
            set
            {
                this.pmtInfIdField = value;
            }
        }

        [XmlElement("PmtMtd")]
        public string PmtMtd
        {
            get
            {
                return this.pmtMtdField;
            }
            set
            {
                this.pmtMtdField = value;
            }
        }

        [XmlElement("BtchBookg")]
        public bool BtchBookg
        {
            get
            {
                return this.btchBookgField;
            }
            set
            {
                this.btchBookgField = value;
            }
        }

        [XmlElement("NbOfTxs")]
        public int NbOfTxs
        {
            get
            {
                return this.nbOfTxsField;
            }
            set
            {
                this.nbOfTxsField = value;
            }
        }

        [XmlElement("PmtTpInf")]
        public DocumentCstmrCdtTrfInitnPmtInfPmtTpInf PmtTpInf
        {
            get
            {
                return this.pmtTpInfField;
            }
            set
            {
                this.pmtTpInfField = value;
            }
        }

        [XmlElement("ReqdExctnDt")]
        public string ReqdExctnDt
        {
            get
            {
                return this.reqdExctnDtField;
            }
            set
            {
                this.reqdExctnDtField = value;
            }
        }

        [XmlElement("Dbtr")]
        public DocumentCstmrCdtTrfInitnPmtInfDbtr Dbtr
        {
            get
            {
                return this.dbtrField;
            }
            set
            {
                this.dbtrField = value;
            }
        }

        [XmlElement("DbtrAcct")]
        public DocumentCstmrCdtTrfInitnPmtInfDbtrAcct DbtrAcct
        {
            get
            {
                return this.dbtrAcctField;
            }
            set
            {
                this.dbtrAcctField = value;
            }
        }

        [XmlElement("DbtrAgt")]
        public DocumentCstmrCdtTrfInitnPmtInfDbtrAgt DbtrAgt
        {
            get
            {
                return this.dbtrAgtField;
            }
            set
            {
                this.dbtrAgtField = value;
            }
        }

        [XmlElement("CdtTrfTxInf")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInf[] CdtTrfTxInf
        {
            get
            {
                return this.cdtTrfTxInfField;
            }
            set
            {
                this.cdtTrfTxInfField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfPmtTpInf
    {

        private DocumentCstmrCdtTrfInitnPmtInfPmtTpInfSvcLvl svcLvlField;

        private DocumentCstmrCdtTrfInitnPmtInfPmtTpInfCtgyPurp ctgyPurpField;

        [XmlElement("SvcLvl")]
        public DocumentCstmrCdtTrfInitnPmtInfPmtTpInfSvcLvl SvcLvl
        {
            get
            {
                return this.svcLvlField;
            }
            set
            {
                this.svcLvlField = value;
            }
        }

        [XmlElement("CtgyPurp")]
        public DocumentCstmrCdtTrfInitnPmtInfPmtTpInfCtgyPurp CtgyPurp
        {
            get
            {
                return this.ctgyPurpField;
            }
            set
            {
                this.ctgyPurpField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfPmtTpInfSvcLvl
    {

        private string cdField;

        [XmlElement("Cd")]
        public string Cd
        {
            get
            {
                return this.cdField;
            }
            set
            {
                this.cdField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfPmtTpInfCtgyPurp
    {

        private string cdField;

        [XmlElement("Cd")]
        public string Cd
        {
            get
            {
                return this.cdField;
            }
            set
            {
                this.cdField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfDbtr
    {

        private string nmField;

        private DocumentCstmrCdtTrfInitnPmtInfDbtrPstlAdr pstlAdrField;

        private DocumentCstmrCdtTrfInitnPmtInfDbtrID idField;

        [XmlElement("Nm")]
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        [XmlElement("PstlAdr")]
        public DocumentCstmrCdtTrfInitnPmtInfDbtrPstlAdr PstlAdr
        {
            get
            {
                return this.pstlAdrField;
            }
            set
            {
                this.pstlAdrField = value;
            }
        }

        [XmlElement("Id")]
        public DocumentCstmrCdtTrfInitnPmtInfDbtrID Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfDbtrPstlAdr
    {

        private string ctryField;

        [XmlElement("Ctry")]
        public string Ctry
        {
            get
            {
                return this.ctryField;
            }
            set
            {
                this.ctryField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfDbtrID
    {

        private DocumentCstmrCdtTrfInitnPmtInfDbtrIDOrgId orgIdField;

        [XmlElement("OrgId")]
        public DocumentCstmrCdtTrfInitnPmtInfDbtrIDOrgId OrgId
        {
            get
            {
                return this.orgIdField;
            }
            set
            {
                this.orgIdField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfDbtrIDOrgId
    {

        private DocumentCstmrCdtTrfInitnPmtInfDbtrIDOrgIdOthr othrField;

        [XmlElement("Othr")]
        public DocumentCstmrCdtTrfInitnPmtInfDbtrIDOrgIdOthr Othr
        {
            get
            {
                return this.othrField;
            }
            set
            {
                this.othrField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfDbtrIDOrgIdOthr
    {

        private string idField;

        [XmlElement("Id")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfDbtrAcct
    {

        private DocumentCstmrCdtTrfInitnPmtInfDbtrAcctID idField;

        [XmlElement("Id")]
        public DocumentCstmrCdtTrfInitnPmtInfDbtrAcctID Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfDbtrAcctID
    {

        private string iBANField;

        [XmlElement("IBAN")]
        public string IBAN
        {
            get
            {
                return this.iBANField;
            }
            set
            {
                this.iBANField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfDbtrAgt
    {

        private DocumentCstmrCdtTrfInitnPmtInfDbtrAgtFinInstnId finInstnIdField;

        [XmlElement("FinInstnId")]
        public DocumentCstmrCdtTrfInitnPmtInfDbtrAgtFinInstnId FinInstnId
        {
            get
            {
                return this.finInstnIdField;
            }
            set
            {
                this.finInstnIdField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfDbtrAgtFinInstnId
    {

        private string bICField;

        private DocumentCstmrCdtTrfInitnPmtInfDbtrAgtFinInstnIdPstlAdr pstlAdrField;

        [XmlElement("BIC")]
        public string BIC
        {
            get
            {
                return this.bICField;
            }
            set
            {
                this.bICField = value;
            }
        }

        [XmlElement("PstlAdr")]
        public DocumentCstmrCdtTrfInitnPmtInfDbtrAgtFinInstnIdPstlAdr PstlAdr
        {
            get
            {
                return this.pstlAdrField;
            }
            set
            {
                this.pstlAdrField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfDbtrAgtFinInstnIdPstlAdr
    {

        private string ctryField;

        [XmlElement("Ctry")]
        public string Ctry
        {
            get
            {
                return this.ctryField;
            }
            set
            {
                this.ctryField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInf
    {

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfPmtId pmtIdField;

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfAmt amtField;

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAgt cdtrAgtField;

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtr cdtrField;

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAcct cdtrAcctField;

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfRmtInf rmtInfField;

        [XmlElement("PmtId")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfPmtId PmtId
        {
            get
            {
                return this.pmtIdField;
            }
            set
            {
                this.pmtIdField = value;
            }
        }

        [XmlElement("Amt")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfAmt Amt
        {
            get
            {
                return this.amtField;
            }
            set
            {
                this.amtField = value;
            }
        }

        [XmlElement("CdtrAgt")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAgt CdtrAgt
        {
            get
            {
                return this.cdtrAgtField;
            }
            set
            {
                this.cdtrAgtField = value;
            }
        }

        [XmlElement("Cdtr")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtr Cdtr
        {
            get
            {
                return this.cdtrField;
            }
            set
            {
                this.cdtrField = value;
            }
        }

        [XmlElement("CdtrAcct")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAcct CdtrAcct
        {
            get
            {
                return this.cdtrAcctField;
            }
            set
            {
                this.cdtrAcctField = value;
            }
        }

        [XmlElement("RmtInf")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfRmtInf RmtInf
        {
            get
            {
                return this.rmtInfField;
            }
            set
            {
                this.rmtInfField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfPmtId
    {

        private string endToEndIdField;

        [XmlElement("EndToEndId")]
        public string EndToEndId
        {
            get
            {
                return this.endToEndIdField;
            }
            set
            {
                this.endToEndIdField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfAmt
    {

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfAmtInstdAmt instdAmtField;

        [XmlElement("InstdAmt")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfAmtInstdAmt InstdAmt
        {
            get
            {
                return this.instdAmtField;
            }
            set
            {
                this.instdAmtField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfAmtInstdAmt
    {

        private string ccyField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Ccy
        {
            get
            {
                return this.ccyField;
            }
            set
            {
                this.ccyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAgt
    {

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAgtFinInstnId finInstnIdField;

        [XmlElement("FinInstnId")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAgtFinInstnId FinInstnId
        {
            get
            {
                return this.finInstnIdField;
            }
            set
            {
                this.finInstnIdField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAgtFinInstnId
    {

        private string bICField;

        [XmlElement("BIC")]
        public string BIC
        {
            get
            {
                return this.bICField;
            }
            set
            {
                this.bICField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtr
    {

        private string nmField;

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrPstlAdr pstlAdrField;

        [XmlElement("Nm")]
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        [XmlElement("PstlAdr")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrPstlAdr PstlAdr
        {
            get
            {
                return this.pstlAdrField;
            }
            set
            {
                this.pstlAdrField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrPstlAdr
    {

        private string ctryField;

        private string[] adrLineField;

        [XmlElement("Ctry")]
        public string Ctry
        {
            get
            {
                return this.ctryField;
            }
            set
            {
                this.ctryField = value;
            }
        }

        [XmlElement("AdrLine")]
        public string[] AdrLine
        {
            get
            {
                return this.adrLineField;
            }
            set
            {
                this.adrLineField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAcct
    {

        private DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAcctID idField;

        [XmlElement("Id")]
        public DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAcctID Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAcctID
    {

        private string iBANField;

        [XmlElement("IBAN")]
        public string IBAN
        {
            get
            {
                return this.iBANField;
            }
            set
            {
                this.iBANField = value;
            }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfRmtInf
    {

        private string ustrdField;

        [XmlElement("Ustrd")]
        public string Ustrd
        {
            get
            {
                return this.ustrdField;
            }
            set
            {
                this.ustrdField = value;
            }
        }
    }
}
