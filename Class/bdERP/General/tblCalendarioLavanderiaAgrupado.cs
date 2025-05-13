namespace WebApiCore.Class.bdERP.General
{
    public class tblCalendarioLavanderiaAgrupado
    {
        public List<fechas> fechasCalendario { get; set; }

        public class fechas
        {
            public DateTime fechaDesde { get; set; }
            public DateTime fechaHasta { get; set; }
            public int idLavanderia { get; set; }
            public byte idCalendario_Estado { get; set; }
        }
    }
}
