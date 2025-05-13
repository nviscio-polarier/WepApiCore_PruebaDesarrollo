namespace WebApiCore.Class.auth
{
    public class LoginResponse
    {
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string refreshToken { get; set; }
        public string token { get; set; }
        public DateTimeOffset token_expireDate { get; set; }
        public Nullable<DateTimeOffset> refreshToken_expireDate { get; set; }
    }
}
