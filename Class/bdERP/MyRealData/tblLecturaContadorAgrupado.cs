namespace WebApiCore.Class.bdERP.MyRealData
{
    public class tblLecturaContadorAgrupado
    {
        public List<lecturas> tblLecturaContador { get; set; }

        public class lecturas
        {
            public DateTime fecha { get; set; }
            public decimal valor { get; set; }
            public int idRecursoContador { get; set; }
        }
    }
}
