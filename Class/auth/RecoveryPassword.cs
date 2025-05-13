namespace WebApiCore.Class.auth
{
    public class RecoveryPassword
    {
        public string Email { get; set; }
        public Guid Token { get; set; }
        public string NewPassword { get; set; }
    }
}
