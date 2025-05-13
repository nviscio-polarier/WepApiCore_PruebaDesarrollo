using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class;
using WebApiCore.Class.notificaciones;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblComunicadoController : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblComunicadoController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblComunicado>> Get()
    {
        return db.tblComunicado;
    }

    [EnableQuery]
    [HttpGet("odata/tblComunicado/get_todos")]
    [Authorize]
    public async Task<ActionResult> get_todos()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        int? idPersonaNUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario == idUsuario)?.idPersona;

        if (idPersonaNUsuario == null)
            return BadRequest();

        var now = DateTimeOffset.Now;
        var tblComunicado = db.tblComunicado
                               .Include(x => x.tblComunicadoNPersona)
                               .Where(x => (x.fecha < now && (now < x.fechaFin || x.fechaFin == null))
                                           && (x.tblComunicadoNPersona.Count() == 0
                                               || x.tblComunicadoNPersona.Where(x => x.idPersona == idPersonaNUsuario && x.fechaDismiss == null).Count() > 0
                                               ))
                               .ToList();

        var comunicado = tblComunicado.Select(x => new
        {
            x.idComunicado,
            x.fecha,
            x.fechaFin,
            x.titulo,
            x.contenido,
            x.tipo,
            x.icon,
            x.isImportante,
            x.isDismiss,
            x.URL
        });

        var comunicadosImportantes = comunicado.Where(x => x.isImportante == true).ToList();
        var comunicadosNormales = comunicado.Where(x => x.isImportante == false).ToList();

        return Ok(new { comunicadosImportantes, comunicadosNormales });
    }


    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblComunicado comunicado)
    {

        var fechaComunicado = comunicado.fecha;
        var fechaFinComunicado = comunicado.fechaFin;

        var gmt = ObtenerGMTLav(14);
        DateTimeOffset fechaComunicadoOffSet = Class.Utils.aplicarGMT(fechaComunicado, gmt);
        DateTimeOffset? fechaFinComunicadoOffSet = fechaFinComunicado == null ? null : Class.Utils.aplicarGMT((DateTimeOffset)fechaFinComunicado, gmt);

        db.tblComunicado.Add(comunicado);
        await db.SaveChangesAsync();

        var hayComunicadoPersonal = comunicado.tblComunicadoNPersona.Count() > 0;

        if (hayComunicadoPersonal) //Envia notificacion a los usuarios seleccionados
        {
            var idsComunicadoNPersona = new List<int?>();

            foreach (var item in comunicado.tblComunicadoNPersona)
            {
                idsComunicadoNPersona.Add(item.idPersona);
            }

            await PushNotification(idsComunicadoNPersona, "tblComunicado_nuevo", "Tienes un nuevo comunicado de Polarier.");
        }
        else //Envia notificacion a todos los usuarios
        {
            await PushNotification(null, "tblComunicado_nuevo", "Tienes un nuevo comunicado de Polarier.");
        }

        _hubContext.Clients.Group("RRHH").SendAsync("RRHH/comunicados", "tblComunicado");

        NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
        notificaciones.SendToAllUsers("notificaciones_RRHH", "tblComunicado");

        return Created(comunicado);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblComunicado> comunicado)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var operations_tblComunicadoNPersona = comunicado.Operations.FirstOrDefault(x => x.path.Equals("/tblComunicadoNPersona"));
        if (operations_tblComunicadoNPersona != null)
        {
            db.tblComunicadoNPersona.RemoveRange(
                db.tblComunicadoNPersona.Where(x => x.idComunicado.Equals(key))
                );
        }

        var entity = db.tblComunicado.Where(x => x.idComunicado.Equals(key)).FirstOrDefault();

        comunicado.ApplyTo(entity);

        await db.SaveChangesAsync();

        _hubContext.Clients.Group("RRHH").SendAsync("RRHH/comunicados", "tblComunicado");

        NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
        notificaciones.SendToAllUsers("notificaciones_RRHH", "tblComunicado");

        List<tblComunicado> result = new List<tblComunicado>();
        result.Add(entity);
        return Ok(result);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = db.tblComunicado.Where(x => x.idComunicado.Equals(key)).FirstOrDefault();
        var comunicadoPersonal = db.tblComunicadoNPersona.Where(x => x.idComunicado.Equals(key)).ToList();
        if (entity == null)
            return false;

        if (comunicadoPersonal.Count() > 0)
        {
            db.tblComunicadoNPersona.RemoveRange(comunicadoPersonal);
        }

        db.tblComunicado.Remove(entity);

        await db.SaveChangesAsync();

        _hubContext.Clients.Group("RRHH").SendAsync("RRHH/comunicados", "tblComunicado");

        NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
        notificaciones.SendToAllUsers("notificaciones_RRHH", "tblComunicado");

        return true;
    }

    private int ObtenerGMTLav(int idLavanderia)
    {
        var tblLavanderia = db.tblLavanderia
       .Where(x => x.idLavanderia.Equals(idLavanderia))
       .Select(x => new
       {
           x.idLavanderia,
           x.horarioVerano,
           x.idZonaHorariaNavigation
       }).FirstOrDefault();

        int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

        return gmt;
    }

    public async Task<bool> PushNotification(List<int?> idPersonaList, string type, string denoComunicado)
    {
        string title = "";
        string body = denoComunicado ?? "Haz click para acceder";

        if (type == "tblComunicado_nuevo")
        {
            title = "Nuevo comunicado";
        }

        var requestList = new List<NotificacionRequest>();

        if (idPersonaList != null)
        {
            var tblUsuarioFiltered = db.tblUsuario.Where(x => idPersonaList.Contains(x.idPersona)).ToList();
            foreach (var idPersona in idPersonaList)
            {
                var notificationToken = tblUsuarioFiltered.FirstOrDefault(x => x.idPersona == idPersona)?.notificationToken;
                var idUsuario = tblUsuarioFiltered.FirstOrDefault(x => x.idPersona == idPersona)?.idUsuario;

                if (notificationToken == null || idUsuario == null)
                    continue;

                var request = new NotificacionRequest()
                {
                    idUsuario = (int)idUsuario,
                    notificacionToken = notificationToken,
                    title = title,
                    body = body
                };
                requestList.Add(request);
            }
        }
        else
        {

            var usuariosNotificacion = db.tblUsuario.Where(x =>
                                                          x.notificationToken != null &&
                                                          x.idPersona != null &&
                                                          x.idPersonaNavigation.activo == true &&
                                                          x.idPersonaNavigation.eliminado == false &&
                                                         (x.idLocalizacion == 1 || x.idLocalizacion == 2))
                                                  .ToList();

            foreach (var item in usuariosNotificacion)
            {
                var notificationToken = item.notificationToken;
                var idUsuario = item.idUsuario;

                if (notificationToken == null || idUsuario == null)
                    continue;

                var request = new NotificacionRequest()
                {
                    idUsuario = (int)idUsuario,
                    notificacionToken = notificationToken,
                    title = title,
                    body = body
                };
                requestList.Add(request);
            }
        }

        await new Utils.Notificaciones(db).SendMessageAsync(requestList);
        return true;
    }
}
