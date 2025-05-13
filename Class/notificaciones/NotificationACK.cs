namespace WebApiCore.Class.notificaciones
{
    public class NotificationACK
    {
        public string idNotificacion { get; set; }
        public string idMessage { get; set; }
        //public string notificationToken { get; set; }
        public int action { get; set; }
    }
}
