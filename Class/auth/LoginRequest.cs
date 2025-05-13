namespace WebApiCore.Class.auth
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string notificationToken { get; set; }
    }
}
