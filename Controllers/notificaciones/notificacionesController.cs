using Microsoft.AspNetCore.Mvc;
using WebApiCore.Class;
using WebApiCore.Class.notificaciones;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.notificaciones
{
    [Authorize]
    public class notificacionesController : ControllerBase
    {
        private readonly bdERP db;
        public notificacionesController(bdERP context)
        {
            db = context;
        }

        [HttpPost]
        [Route("notificaciones/sendNotification")]
        public async Task<IActionResult> sendNotification([FromBody] NotificacionRequest request)
        {
            if (request.notificacionToken == null)
            {
                request.notificacionToken = db.tblUsuario.Find(request.idUsuario).notificationToken;
            }
            await new Utils.Notificaciones(db).SendMessageAsync(request);
            return Ok();
        }

        [HttpPost]
        [Route("notificaciones/sendNotificationMasivo")]
        public async Task<IActionResult> sendNotificationMasivo([FromBody] List<NotificacionRequest> request)
        {
            foreach (var item in request)
            {
                if (item.notificacionToken == null)
                {
                    item.notificacionToken = db.tblUsuario.Find(item.idUsuario).notificationToken;
                }
            }
            await new Utils.Notificaciones(db).SendMessageAsync(request);

            return Ok();
        }

        [HttpPost]
        [Route("notificaciones/notificationACK")]
        public async Task<ActionResult> notificationACK([FromBody] NotificationACK notificationACK)
        {
            if (notificationACK == null)
            {
                return BadRequest();
            }
            if (notificationACK.idNotificacion == null || notificationACK.idNotificacion == "")
            {
                return BadRequest();
            }
            tblNotificacion_Evento? eventoRecord = db.tblNotificacion_Evento.Where(x => x.idNotificacion == Int32.Parse(notificationACK.idNotificacion)).FirstOrDefault();
            if (eventoRecord != null)
            {
                tblNotificacion_Evento evento = new()
                {
                    idNotificacion = eventoRecord.idNotificacion,
                    idNotificacion_Estado = notificationACK.action,
                    idMessage = notificationACK.idMessage
                };
                db.tblNotificacion_Evento.Add(evento); //Esta inserción ejecuta un trigger dentro de sql automaticamente para actualizar la tabla de notificaciones principal
                db.SaveChanges();
                return Ok();
            }
            return BadRequest("Esta notificacion no está registrada en la base de datos.");
        }
    }
}
