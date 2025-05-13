using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.Context;
using WebApiCore.Hubs;

namespace WebApiCore.Controllers;

public class tblEventoPersona_EstadoController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblEventoPersona_EstadoController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }


    [AllowAnonymous]
    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblEventoPersona_Estado);
    }
}