namespace WebApiCore.Enums.RRHH
{
    public enum TipoNominaMX : short
    {
        NominaQ1 = 1,
        NominaQ2 = 2,
        Aguinaldo = 3,
        FiniquitoQ1 = 4,
        FiniquitoQ2 = 5,
        PTUQ1 = 6,
        PTUQ2 = 7,
    }

    public class TipoPagaMXUtils
    {
        public static List<TipoNominaMX> PagasQ1 = new() { TipoNominaMX.NominaQ1, TipoNominaMX.FiniquitoQ1, TipoNominaMX.PTUQ1 };
        public static List<TipoNominaMX> PagasQ2 = new() { TipoNominaMX.NominaQ2, TipoNominaMX.FiniquitoQ2, TipoNominaMX.PTUQ2 };

        public static bool isPagaQ1(TipoNominaMX tipoNomina)
        {
            return PagasQ1.Contains(tipoNomina);
        }

        public static bool isPagaQ2(TipoNominaMX tipoNomina)
        {
            return PagasQ2.Contains(tipoNomina);
        }
    }
}
