namespace WebApiCore.Class.bdERP.tblDocumento
{
    public class SendMail_DatosPersonales
    {
        public List<personasNCentroNLav> lavCentros { get; set; }

        public class personasNCentroNLav
        {
            public int idAsuntoMailAltasGestorias { get; set; }
            public int? idLavanderia { get; set; }
            public int? idCentroTrabajo { get; set; }
            public int[] idsPersonas { get; set; }
        }
    }
}
