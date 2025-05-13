namespace WebApiCore.Class.bdERP.RRHH
{
    public class tblCalendarioPersonalAgrupado
    {
        public List<fechas> fechasCalendario { get; set; }

        public class fechas
        {
            public DateTime fechaDesde { get; set; }
            public DateTime fechaHasta { get; set; }
            public int idPersona { get; set; }
            public byte idCalendario_Estado { get; set; }
        }
    }
}
