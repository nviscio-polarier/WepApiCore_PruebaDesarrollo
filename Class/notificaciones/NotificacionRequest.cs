namespace WebApiCore.Class.notificaciones
{
    public class NotificacionRequest
    {
        public int idUsuario { get; set; }
        public string notificacionToken { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string topic { get; set; }
    }
}
