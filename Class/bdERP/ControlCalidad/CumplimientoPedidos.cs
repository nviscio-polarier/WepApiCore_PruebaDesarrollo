namespace WebApiCore.Class.bdERP.ControlCalidad
{
    public class CumplimientoPedidos
    {
        public int idPrenda { get; set; }
        public string fechaString { get; set; }
        public DateTime fecha { get; set; }
        public int idCompañia { get; set; }
        public int idEntidad { get; set; }
        public string denoPrenda { get; set; }
        public string denoCompa { get; set; }
        public string denoEnti { get; set; }
        public int idPedido { get; set; }
        public int? cantidadReparto { get; set; }
        public int? cantidadPedido { get; set; }
        public int? udsPedidas { get; set; }
        public int? udsEntregadas { get; set; }
        public int? udsPendientes { get; set; }

    }
}
