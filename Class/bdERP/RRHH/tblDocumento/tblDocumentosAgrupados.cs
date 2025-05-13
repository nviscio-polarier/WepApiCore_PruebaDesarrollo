namespace WebApiCore.Class.bdERP.tblDocumento
{
    using WebApiCore.Context;
    public class tblDocumentosAgrupados
    {
        public List<tblDocumento_> documentos { get; set; }
        public class tblDocumento_ : tblDocumento
        {
            public int? idUsuario { get; set; }
            public string? notificationToken { get; set; }
        }
    }
}
