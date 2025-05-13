namespace WebApiCore.Class.bdERP.ControlCalidad
{
    public class RechazoCliente
    {
        public int idPrenda { get; set; }
        public string fechaString { get; set; }
        public DateTime fecha { get; set; }
        public int idCompañia { get; set; }
        public int idEntidad { get; set; }
        public string denoPrenda { get; set; }
        public string denoCompa { get; set; }
        public string denoEnti { get; set; }
        public string denoCategoriaAbono { get; set; }
        public int? cantidadReparto { get; set; }
        public int? cantidadAbono { get; set; }
        public int? cantAbonoCalidad { get; set; }
        public int? cantAbonoError { get; set; }
        public int? udsEntregadas { get; set; }
        public int? udsCorrectas { get; set; }
        public int? udsAbonadas { get; set; }
    }
}
