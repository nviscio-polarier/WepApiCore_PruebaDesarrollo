namespace WebApiCore.Class.bdERP.RRHH.tblDocumento
{
    using WebApiCore.Context;
    public class PostMasivoMultDocBody
    {
        public List<int> idsPersona { get; set; }
        public List<tblDocumento> documentos { get; set; }
    }
}
