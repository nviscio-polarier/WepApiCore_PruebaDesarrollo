namespace WebApiCore.Enums.MyRealBonus
{
    
        public enum TipoOperacionToken
        {
            INSERT,                 // Inserción general de tokens (por rendimiento)
            SOLICITUD_CANJE,        // Cuando un usuario solicita un canje
            APROBAR_CANJE,          // Cuando un canje es aprobado
            RECHAZAR_CANJE,         // Cuando un canje es rechazado
            ERROR_SOLICITUD_CANJE,  // Error al registrar la solicitud
            ERROR_APROBAR_CANJE,    // Error al aprobar el canje
            ERROR_RECHAZAR_CANJE,   // Error al rechazar el canje
            UMBRAL_INSUFICIENTE     // Cuando el rendimiento fue insuficiente para generar tokens
        }

    
}
