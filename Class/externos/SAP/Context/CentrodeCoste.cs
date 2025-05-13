namespace WebApiCore.Class.externos.SAP.Context
{
    public class CentrodeCoste
    {

        // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        [System.Xml.Serialization.XmlRoot(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false, ElementName = "feed")]
        public partial class Model
        {

            private string idField;

            private feedTitle titleField;

            private DateTime updatedField;

            private feedAuthor authorField;

            private feedLink linkField;

            private feedEntry[] entryField;

            private string baseField;

            /// <remarks/>
            public string id
            {
                get
                {
                    return idField;
                }
                set
                {
                    idField = value;
                }
            }

            /// <remarks/>
            public feedTitle title
            {
                get
                {
                    return titleField;
                }
                set
                {
                    titleField = value;
                }
            }

            /// <remarks/>
            public DateTime updated
            {
                get
                {
                    return updatedField;
                }
                set
                {
                    updatedField = value;
                }
            }

            /// <remarks/>
            public feedAuthor author
            {
                get
                {
                    return authorField;
                }
                set
                {
                    authorField = value;
                }
            }

            /// <remarks/>
            public feedLink link
            {
                get
                {
                    return linkField;
                }
                set
                {
                    linkField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement("entry")]
            public feedEntry[] entry
            {
                get
                {
                    return entryField;
                }
                set
                {
                    entryField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
            public string @base
            {
                get
                {
                    return baseField;
                }
                set
                {
                    baseField = value;
                }
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public partial class feedTitle
        {

            private string typeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string type
            {
                get
                {
                    return typeField;
                }
                set
                {
                    typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlText()]
            public string Value
            {
                get
                {
                    return valueField;
                }
                set
                {
                    valueField = value;
                }
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public partial class feedAuthor
        {

            private object nameField;

            /// <remarks/>
            public object name
            {
                get
                {
                    return nameField;
                }
                set
                {
                    nameField = value;
                }
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public partial class feedLink
        {

            private string hrefField;

            private string relField;

            private string titleField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string href
            {
                get
                {
                    return hrefField;
                }
                set
                {
                    hrefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string rel
            {
                get
                {
                    return relField;
                }
                set
                {
                    relField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string title
            {
                get
                {
                    return titleField;
                }
                set
                {
                    titleField = value;
                }
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public partial class feedEntry
        {

            private string idField;

            private feedEntryTitle titleField;

            private DateTime updatedField;

            private feedEntryCategory categoryField;

            private feedEntryLink linkField;

            private feedEntryContent contentField;

            /// <remarks/>
            public string id
            {
                get
                {
                    return idField;
                }
                set
                {
                    idField = value;
                }
            }

            /// <remarks/>
            public feedEntryTitle title
            {
                get
                {
                    return titleField;
                }
                set
                {
                    titleField = value;
                }
            }

            /// <remarks/>
            public DateTime updated
            {
                get
                {
                    return updatedField;
                }
                set
                {
                    updatedField = value;
                }
            }

            /// <remarks/>
            public feedEntryCategory category
            {
                get
                {
                    return categoryField;
                }
                set
                {
                    categoryField = value;
                }
            }

            /// <remarks/>
            public feedEntryLink link
            {
                get
                {
                    return linkField;
                }
                set
                {
                    linkField = value;
                }
            }

            /// <remarks/>
            public feedEntryContent content
            {
                get
                {
                    return contentField;
                }
                set
                {
                    contentField = value;
                }
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public partial class feedEntryTitle
        {

            private string typeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string type
            {
                get
                {
                    return typeField;
                }
                set
                {
                    typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlText()]
            public string Value
            {
                get
                {
                    return valueField;
                }
                set
                {
                    valueField = value;
                }
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public partial class feedEntryCategory
        {

            private string termField;

            private string schemeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string term
            {
                get
                {
                    return termField;
                }
                set
                {
                    termField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string scheme
            {
                get
                {
                    return schemeField;
                }
                set
                {
                    schemeField = value;
                }
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public partial class feedEntryLink
        {

            private string hrefField;

            private string relField;

            private string titleField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string href
            {
                get
                {
                    return hrefField;
                }
                set
                {
                    hrefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string rel
            {
                get
                {
                    return relField;
                }
                set
                {
                    relField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string title
            {
                get
                {
                    return titleField;
                }
                set
                {
                    titleField = value;
                }
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public partial class feedEntryContent
        {

            private properties propertiesField;

            private string typeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
            public properties properties
            {
                get
                {
                    return propertiesField;
                }
                set
                {
                    propertiesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttribute()]
            public string type
            {
                get
                {
                    return typeField;
                }
                set
                {
                    typeField = value;
                }
            }
        }

        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        [System.Xml.Serialization.XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", IsNullable = false)]
        public partial class properties
        {

            private string controllingAreaField;

            private string costCenterField;

            private string costCenterNameField;

            private string costCenterDescriptionField;

            private DateTime validityStartDateField;

            private DateTime validityEndDateField;

            private string companyCodeField;

            private string costCenterCategoryField;

            private string costCtrResponsiblePersonNameField;

            private string costCtrResponsibleUserField;

            private string profitCenterField;

            private string profitCenterNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public string ControllingArea
            {
                get
                {
                    return controllingAreaField;
                }
                set
                {
                    controllingAreaField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public string CostCenter
            {
                get
                {
                    return costCenterField;
                }
                set
                {
                    costCenterField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public string CostCenterName
            {
                get
                {
                    return costCenterNameField;
                }
                set
                {
                    costCenterNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public string CostCenterDescription
            {
                get
                {
                    return costCenterDescriptionField;
                }
                set
                {
                    costCenterDescriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public DateTime ValidityStartDate
            {
                get
                {
                    return validityStartDateField;
                }
                set
                {
                    validityStartDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public DateTime ValidityEndDate
            {
                get
                {
                    return validityEndDateField;
                }
                set
                {
                    validityEndDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public string CompanyCode
            {
                get
                {
                    return companyCodeField;
                }
                set
                {
                    companyCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public string CostCenterCategory
            {
                get
                {
                    return costCenterCategoryField;
                }
                set
                {
                    costCenterCategoryField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public string CostCtrResponsiblePersonName
            {
                get
                {
                    return costCtrResponsiblePersonNameField;
                }
                set
                {
                    costCtrResponsiblePersonNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public string CostCtrResponsibleUser
            {
                get
                {
                    return costCtrResponsibleUserField;
                }
                set
                {
                    costCtrResponsibleUserField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public string ProfitCenter
            {
                get
                {
                    return profitCenterField;
                }
                set
                {
                    profitCenterField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
            public string ProfitCenterName
            {
                get
                {
                    return profitCenterNameField;
                }
                set
                {
                    profitCenterNameField = value;
                }
            }
        }

    }
}