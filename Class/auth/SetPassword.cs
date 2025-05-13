namespace WebApiCore.Class.auth
{
    public class SetPassword
    {
        public Guid PasswordToken { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string notificationToken { get; set; }
    }
}
