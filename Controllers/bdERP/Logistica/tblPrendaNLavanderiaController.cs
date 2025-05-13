using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPrendaNLavanderiaController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblPrendaNLavanderiaController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [HttpGet]
    [EnableQuery]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblPrendaNLavanderia);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int keyidLavanderia, [FromODataUri] int keyidPrenda, [FromBody] JsonPatchDocument<tblPrendaNLavanderia> prendaNLavanderia)
    {
        var entity = db.tblPrendaNLavanderia.FirstOrDefault(x => x.idPrenda.Equals(keyidPrenda) && x.idLavanderia.Equals(keyidLavanderia));
        if (entity == null)
        {
            entity = new tblPrendaNLavanderia() { idLavanderia = keyidLavanderia, idPrenda = keyidPrenda };
            db.tblPrendaNLavanderia.Add(entity);
        }
        prendaNLavanderia.ApplyTo(entity);

        await db.SaveChangesAsync();

        NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
        notificaciones.SendToAllUsers("notificaciones_LogisticaInterna_" + keyidLavanderia, "tblPrendaNLavanderia");

        return Ok(entity);
    }
}
