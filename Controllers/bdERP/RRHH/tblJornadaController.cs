using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblJornadaController : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblJornadaController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int? idLavanderia)
    {
        return Ok(db.tblJornada.Where(x => x.idLavanderia.Equals(idLavanderia)));
    }
}