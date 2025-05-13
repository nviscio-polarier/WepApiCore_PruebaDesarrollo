namespace WebApiCore.Enums.MyRealBonus
{
    public static class MensajesErrorTokens
    {
        public const string NingunaPrendaProcesada = "No se encontraron prendas procesadas.";
        public const string MaquinaNoEsPlegadora = "La máquina no es una plegadora.";
        public const string ErrorGeneral = "Ha ocurrido un error inesperado durante el cálculo de tokens.";
        public const string UmbralInsuficiente = "El rendimiento ha sido positivo pero no lo suficiente como para llegar a 1 token.";
    }
}
