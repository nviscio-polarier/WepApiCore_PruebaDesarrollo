using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
[BasicAuth]
public class AppLaundrydashboardController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public AppLaundrydashboardController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [AllowAnonymous]
    [EnableQuery]
    [HttpGet("odata/AppLaundrydashboard/resetLaundryDashboard")]
    public ActionResult ResetLaundryDashboard([FromODataUri] int? idLavanderia)
    {
        if (idLavanderia == null)
            return BadRequest();

        _hubContext.Clients.Group("AppLaundryDashboard_" + idLavanderia).SendAsync("AppLaundryDashboard/signalR_refresh");

        return Ok();
    }

}