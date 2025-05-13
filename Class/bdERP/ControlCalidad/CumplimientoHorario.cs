namespace WebApiCore.Class.bdERP.ControlCalidad
{
    public class CumplimientoHorario
    {
        public int idCompañia { get; set; }
        public string denoCompa { get; set; }
        public int idEntidad { get; set; }
        public string denoEnti { get; set; }
        public DateTime? fechaLlegada { get; set; }
        public DateTime? fechaSalida { get; set; }
        public TimeSpan? horarioInicioReparto { get; set; }
        public TimeSpan? horarioFinReparto { get; set; }
        public int entregaHorarioCumplido { get; set; }
        public string nombreTransportista { get; set; }
    }
}
