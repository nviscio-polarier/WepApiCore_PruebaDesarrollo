using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Xml.Serialization;
using WebApiCore.Context;

namespace WebApiCore.Class.externos.XMLBanco
{
    public class XMLBancoUtils
    {

        static readonly private string IBAN_empresa = "ES9601822368390201512141";
        static readonly private string SWIFT_empresa = "BBVAESMMXXX";

        static public XMLBanco GetXMLBanco(IGrouping<dynamic, XMLBanco_Empleado> item)
        {

            // Códigos '000' requeridos por BBVA
            string CIF = item.Key.idEmpresaPolarierNavigation?.CIF.Replace("-", "");
            string IdCIF = CIF + "000" ?? "";
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            return new XMLBanco
            {
                CstmrCdtTrfInitn = new DocumentCstmrCdtTrfInitn
                {
                    GrpHdr = new DocumentCstmrCdtTrfInitnGrpHdr
                    {
                        MsgId = $"{CIF}{timeStamp}",
                        CreDtTm = $"{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss}",
                        NbOfTxs = item.Count(),
                        InitgPty = new DocumentCstmrCdtTrfInitnGrpHdrInitgPty
                        {
                            Nm = item.Key.idEmpresaPolarierNavigation?.nombreFiscal ?? "",
                            Id = new DocumentCstmrCdtTrfInitnGrpHdrInitgPtyID
                            {
                                OrgId = new DocumentCstmrCdtTrfInitnGrpHdrInitgPtyIDOrgId
                                {
                                    Othr = new DocumentCstmrCdtTrfInitnGrpHdrInitgPtyIDOrgIdOthr
                                    {
                                        Id = IdCIF,
                                    }
                                }
                            }
                        }
                    },
                    PmtInf = new DocumentCstmrCdtTrfInitnPmtInf
                    {
                        PmtInfId = $"{CIF}{timeStamp}0{DateTime.UtcNow:fffffff}",
                        PmtMtd = "TRF", // Método de pago -- Estático?
                        NbOfTxs = item.Count(),
                        PmtTpInf = new DocumentCstmrCdtTrfInitnPmtInfPmtTpInf
                        {
                            SvcLvl = new DocumentCstmrCdtTrfInitnPmtInfPmtTpInfSvcLvl
                            {
                                Cd = "SEPA" // Estático?
                            },
                            CtgyPurp = new DocumentCstmrCdtTrfInitnPmtInfPmtTpInfCtgyPurp
                            {
                                Cd = "SALA" // Estático?
                            }
                        },
                        ReqdExctnDt = $"{DateTime.UtcNow:yyyy-MM-dd}", // Fecha de ejecución
                        Dbtr = new DocumentCstmrCdtTrfInitnPmtInfDbtr
                        {
                            Nm = item.Key.idEmpresaPolarierNavigation?.nombreFiscal ?? "",
                            PstlAdr = new DocumentCstmrCdtTrfInitnPmtInfDbtrPstlAdr
                            {
                                Ctry = item.Key.idEmpresaPolarierNavigation?.idPaisNavigation?.codigo ?? "",
                            },
                            Id = new DocumentCstmrCdtTrfInitnPmtInfDbtrID
                            {
                                OrgId = new DocumentCstmrCdtTrfInitnPmtInfDbtrIDOrgId
                                {
                                    Othr = new DocumentCstmrCdtTrfInitnPmtInfDbtrIDOrgIdOthr
                                    {
                                        Id = IdCIF,
                                    }
                                }
                            }
                        },
                        DbtrAcct = new DocumentCstmrCdtTrfInitnPmtInfDbtrAcct
                        {
                            Id = new DocumentCstmrCdtTrfInitnPmtInfDbtrAcctID
                            {
                                IBAN = IBAN_empresa, // IBAN de la empresa
                            }
                        },
                        DbtrAgt = new DocumentCstmrCdtTrfInitnPmtInfDbtrAgt
                        {
                            FinInstnId = new DocumentCstmrCdtTrfInitnPmtInfDbtrAgtFinInstnId
                            {
                                BIC = SWIFT_empresa, // Banco de la empresa
                                PstlAdr = new DocumentCstmrCdtTrfInitnPmtInfDbtrAgtFinInstnIdPstlAdr
                                {
                                    Ctry = IBAN_empresa.Substring(0,2), // Código pais del IBAN empresa
                                }
                            }
                        },
                        CdtTrfTxInf = item.Select(x =>
                        {

                            var IBAN = x.IBAN?.Trim().Replace(" ", "") ?? "";
                            if (!SwiftDict.TryGetValue(int.Parse(IBAN.Substring(4, 4)), out string BIC))
                                BIC = "";

                            return new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInf
                            {
                                PmtId = new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfPmtId
                                {
                                    EndToEndId = $"{x.numDocumentoIdentidad}{timeStamp}0{DateTime.UtcNow:fffffff}",
                                },
                                Amt = new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfAmt
                                {
                                    InstdAmt = new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfAmtInstdAmt
                                    {
                                        Ccy = item.Key.idEmpresaPolarierNavigation.idMonedaNavigation.codigo, // Moneda de la empresa?
                                        Value = x.liquidoPercibir,
                                    }
                                },
                                CdtrAgt = new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAgt
                                {
                                    FinInstnId = new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAgtFinInstnId
                                    {
                                        BIC = BIC, // Banco del empleado
                                    }
                                },
                                Cdtr = new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtr
                                {
                                    Nm = x.nombreCompleto,
                                    PstlAdr = new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrPstlAdr
                                    {
                                        Ctry = item.Key.idEmpresaPolarierNavigation?.idPaisNavigation?.codigo ?? "",
                                        AdrLine = new string[] { x.domicilio ?? "" }
                                    }
                                },
                                CdtrAcct = new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAcct
                                {
                                    Id = new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfCdtrAcctID
                                    {
                                        IBAN = IBAN,
                                    }
                                },
                                RmtInf = new DocumentCstmrCdtTrfInitnPmtInfCdtTrfTxInfRmtInf
                                {
                                    Ustrd = $"ABONO FINIQUITO {x.fecha}"
                                }
                            };
                        }).ToArray()
                    },
                }
            };
        }

        static public MemoryStream SerializeToStream(XMLBanco xml)
        {
            XmlSerializer serializer = new(typeof(XMLBanco));

            MemoryStream memoryStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memoryStream, new UTF8Encoding(false)); // "false" evita el BOM (Byte Order Mark)
            serializer.Serialize(writer, xml);
            writer.Flush();
            memoryStream.Position = 0; // Reseteamos la posición del stream para que esté listo para leer
            return memoryStream; // Retornamos el MemoryStream
        }

        public class XMLBancoGroupKey
        {
            public tblEmpresasPolarier idEmpresaPolarierNavigation { get; set; }
            public int? idAdmCentroCoste { get; set; }
            public int? idAdmElementoPEP { get; set; }
        }

        static private Dictionary<int, string> SwiftDict = new()
        {
            { 3, "BDEPESM1XXX" },
            { 30, "ESPCESMMXXX" },
            { 31, "ETCHES2GXXX" },
            { 36, "SABNESMMXXX" },
            { 46, "GALEES2GXXX" },
            { 49, "BSCHESMMXXX" },
            { 57, "BVADESMMXXX" },
            { 58, "BNPAESMMXXX" },
            { 59, "MADRESMMXXX" },
            { 61, "BMARES2MXXX" },
            { 65, "BARCESMMXXX" },
            { 73, "OPENESMMXXX" },
            { 75, "POPUESMMXXX" },
            { 78, "BAPUES22XXX" },
            { 81, "BSABESBBXXX" },
            { 83, "RENBESMMXXX" },
            { 86, "NORTESMMXXX" },
            { 94, "BVALESMMXXX" },
            { 122, "CITIES2XXXX" },
            { 125, "BAOFESM1XXX" },
            { 128, "BKBKESMMXXX" },
            { 130, "CGDIESMMXXX" },
            { 133, "MIKBESB1XXX" },
            { 136, "AREBESMMXXX" },
            { 138, "BKOAES22XXX" },
            { 149, "BNPAESMSXXX" },
            { 167, "GEBAESMMXXX" },
            { 182, "BBVAESMMXXX" },
            { 184, "BEDFESM1XXX" },
            { 186, "BFIVESBBXXX" },
            { 188, "ALCLESMMXXX" },
            { 190, "BBPIESMMXXX" },
            { 196, "WELAESMMXXX" },
            { 198, "BCOEESMMXXX" },
            { 200, "PRVBESB1XXX" },
            { 211, "PROAESMMXXX" },
            { 216, "POHIESMMXXX" },
            { 219, "BMCEESMMXXX" },
            { 220, "FIOFESM1XXX" },
            { 224, "SCFBESMMXXX" },
            { 227, "UNOEESM1XXX" },
            { 229, "POPLESMMXXX" },
            { 231, "DSBLESMMXXX" },
            { 232, "INVLESMMXXX" },
            { 233, "POPIESMMXXX" },
            { 234, "CCOCESMMXXX" },
            { 235, "PICHESMMXXX" },
            { 236, "LOYIESMMXXX" },
            { 237, "CSURES2CXXX" },
            { 238, "PSTRESMMXXX" },
            { 239, "EVOBESMMXXX" },
            { 487, "GBMNESMMXXX" },
            { 1459, "PRABESMMXXX" },
            { 1460, "CRESESMMXXX" },
            { 1465, "INGDESMMXXX" },
            { 1474, "CITIESMXXXX" },
            { 1475, "CCSEESM1XXX" },
            { 1490, "SELFESMMXXX" },
            { 1491, "TRIOESMMXXX" },
            { 1524, "UBIBESMMXXX" },
            { 1525, "BCDMESMMXXX" },
            { 1534, "KBLXESMMXXX" },
            { 1544, "BACAESMMXXX" },
            { 2000, "CECAESMMXXX" },
            { 2013, "CESCESBBXXX" },
            { 2031, "CECAESMM031" },
            { 2038, "CAHMESMMXXX" },
            { 2043, "CECAESMM043" },
            { 2045, "CECAESMM045" },
            { 2048, "CECAESMM048" },
            { 2051, "CECAESMM051" },
            { 2056, "CECAESMM056" },
            { 2066, "CECAESMM066" },
            { 2080, "CAGLESMMXXX" },
            { 2085, "CAZRES2ZXXX" },
            { 2086, "CECAESMM086" },
            { 2095, "BASKES2BXXX" },
            { 2096, "CSPAES2LXXX" },
            { 2099, "CECAESMM099" },
            { 2100, "CAIXESBBXXX" },
            { 2103, "UCJAES2MXXX" },
            { 2104, "CSSOES2SXXX" },
            { 2105, "CECAESMM105" },
            { 3001, "BCOEESMM001" },
            { 3005, "BCOEESMM005" },
            { 3007, "BCOEESMM007" },
            { 3008, "BCOEESMM008" },
            { 3009, "BCOEESMM009" },
            { 3016, "BCOEESMM016" },
            { 3017, "BCOEESMM017" },
            { 3018, "BCOEESMM018" },
            { 3020, "BCOEESMM020" },
            { 3023, "BCOEESMM023" },
            { 3025, "CDENESBBXXX" },
            { 3029, "CCRIES2A029" },
            { 3035, "CLPEES2MXXX" },
            { 3045, "CCRIES2A045" },
            { 3058, "CCRIES2AXXX" },
            { 3059, "BCOEESMM059" },
            { 3060, "BCOEESMM060" },
            { 3063, "BCOEESMM063" },
            { 3067, "BCOEESMM067" },
            { 3070, "BCOEESMM070" },
            { 3076, "BCOEESMM076" },
            { 3080, "BCOEESMM080" },
            { 3081, "BCOEESMM081" },
            { 3084, "CVRVES2BXXX" },
            { 3085, "BCOEESMM085" },
            { 3089, "BCOEESMM089" },
            { 3095, "CCRIES2A095" },
            { 3096, "BCOEESMM096" },
            { 3098, "BCOEESMM098" },
            { 3102, "BCOEESMM102" },
            { 3104, "BCOEESMM104" },
            { 3105, "CCRIES2A105" },
            { 3110, "BCOEESMM110" },
            { 3111, "BCOEESMM111" },
            { 3112, "CCRIES2A112" },
            { 3113, "BCOEESMM113" },
            { 3115, "BCOEESMM115" },
            { 3116, "BCOEESMM116" },
            { 3117, "BCOEESMM117" },
            { 3118, "CCRIES2A118" },
            { 3119, "CCRIES2A119" },
            { 3121, "CCRIES2A121" },
            { 3123, "CCRIES2A123" },
            { 3127, "BCOEESMM127" },
            { 3130, "BCOEESMM130" },
            { 3134, "BCOEESMM134" },
            { 3135, "CCRIES2A135" },
            { 3138, "BCOEESMM138" },
            { 3140, "BCOEESMM140" },
            { 3144, "BCOEESMM144" },
            { 3146, "CCCVESM1XXX" },
            { 3150, "BCOEESMM150" },
            { 3152, "CCRIES2A152" },
            { 3157, "CCRIES2A157" },
            { 3159, "BCOEESMM159" },
            { 3160, "CCRIES2A160" },
            { 3162, "BCOEESMM162" },
            { 3165, "CCRIES2A165" },
            { 3166, "BCOEESMM166" },
            { 3174, "BCOEESMM174" },
            { 3179, "CCRIES2A179" },
            { 3183, "CASDESBBXXX" },
            { 3186, "CCRIES2A186" },
            { 3187, "BCOEESMM187" },
            { 3190, "BCOEESMM190" },
            { 3191, "BCOEESMM191" },
            { 9000, "ESPBESMMXXX" },
        };

        public class XMLBanco_Empresa
        {
            public tblEmpresasPolarier idEmpresaPolarierNavigation { get; set; }
            public tblAdmCentroCoste idAdmCentroCosteNavigation { get; set; }
            public tblAdmElementoPEP idAdmElementoPEPNavigation { get; set; }
            public string fecha { get; set; }
        }

        public class XMLBanco_Empleado : XMLBanco_Empresa
        {
            public string IBAN { get; set; }
            public string nombreCompleto { get; set; }
            public string numDocumentoIdentidad { get; set; }
            public string domicilio { get; set; }
            public decimal liquidoPercibir { get; set; }
            public string fecha { get; set; }
        }
    }
}
