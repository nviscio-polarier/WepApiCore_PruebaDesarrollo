using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class PolarierTIController : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public PolarierTIController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [Authorize]
    [EnableQuery]
    [HttpGet("odata/PolarierTI/checkAppStatus")]
    public async Task<ActionResult> checkAppStatus()
    {
        return Ok(getAppStatus());
    }

    public List<object> getAppStatus()
    {
        var currentTime = DateTime.Now;
        var thresholdTime = currentTime.AddMinutes(-125); // Considering 120 minutes + 5 minutes

        var locker_status_ = db.tblLogError
            .Where(e => e.denominacion.Contains("locker"))
            .OrderByDescending(e => e.idError)
            .Take(1)
            .FirstOrDefault();

        string locker_errorCode = "LOC_NS";
        var locker_status = new
        {
            idAplicacion = 18,
            denominacion = "Locker",
            ultimaLLamada = DateTime.Parse(locker_status_.error).AddHours(2),
            error = DateTime.Parse(locker_status_.error) > thresholdTime ? 0 : 1,
            errorCode = locker_errorCode,
            errorText = "SIN SEÑAL",
            notificacion = db.tblNotificaciones_TI
                            .Where(x => x.codigo == locker_errorCode && x.fecha_Solucion == null)
                            .Select(x => new
                            {
                                x.idNotificacion_TI,
                                x.fecha_Inicio,
                                x.fecha_Responsable,
                                x.idUsuarioResponsable,
                                nombreUsuarioResponsable = x.idUsuarioResponsableNavigation.nombre
                            })
                            .FirstOrDefault()
        };

        return new List<object>() { locker_status };
    }

    [Authorize]
    [EnableQuery]
    [HttpPost("odata/PolarierTI/setResponsable")]
    public async Task<ActionResult> setResponsable([FromODataUri] int idNotificacion_TI, [FromODataUri] int idUsuario)
    {
        tblNotificaciones_TI notificacion = db.tblNotificaciones_TI.Find(idNotificacion_TI);
        notificacion.idUsuarioResponsable = idUsuario;
        notificacion.fecha_Responsable = DateTime.UtcNow;
        await db.SaveChangesAsync();

        _hubContext.Clients.Group("PolarierTI").SendAsync("signalR_refresh", JsonConvert.SerializeObject(getAppStatus()));

        return Ok();
    }

    [Authorize]
    [EnableQuery]
    [HttpPost("odata/PolarierTI/setSolucion")]
    public async Task<ActionResult> setSolucion([FromODataUri] int idNotificacion_TI)
    {
        db.tblNotificaciones_TI.Find(idNotificacion_TI).fecha_Solucion = DateTime.UtcNow;
        await db.SaveChangesAsync();

        _hubContext.Clients.Group("PolarierTI").SendAsync("signalR_refresh", JsonConvert.SerializeObject(getAppStatus()));

        return Ok();
    }
}
