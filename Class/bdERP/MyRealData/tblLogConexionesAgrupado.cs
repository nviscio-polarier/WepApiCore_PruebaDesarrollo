namespace WebApiCore.Class.bdERP.MyRealData
{
    public class tblLogConexionesAgrupado
    {
        public List<conexiones> tblLogConexiones { get; set; }

        public class conexiones
        {
            public string tipoConexion { get; set; }
            public DateTimeOffset? fecha { get; set; }
            public string codigoRespuesta { get; set; }
            public int idEnergyHub { get; set; }
        }
    }
}
