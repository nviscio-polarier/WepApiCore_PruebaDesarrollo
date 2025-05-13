using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.Context;
using WebApiCore.Hubs;

namespace WebApiCore.Controllers;

public class tblClienteNMaquinaController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblClienteNMaquinaController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblClienteNMaquina);
    }
}