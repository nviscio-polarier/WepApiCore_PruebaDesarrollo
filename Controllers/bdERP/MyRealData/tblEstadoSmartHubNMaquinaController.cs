using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Hubs;

namespace WebApiCore.Controllers;

public class tblEstadoSmartHubNMaquinaController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblEstadoSmartHubNMaquinaController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }


    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get([FromODataUri] int idLavanderia)
    {
        var result = db.tblMaquina.Where(x => x.idLavanderia.Equals(idLavanderia) && x.tblEstadoSmartHubNMaquina.Count > 0)
        .Select(x => x.tblEstadoSmartHubNMaquina.OrderByDescending(c => c.fechaUltimaActualizacion).First());

        return Ok(result);
    }

    [EnableQuery]
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Post([FromBody] tblEstadoSmartHubNMaquina estadoSmartHubNMaquina)
    {
        try
        {
            int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == estadoSmartHubNMaquina.idMaquina).FirstOrDefault().idLavanderia;

            tblEstadoSmartHubNMaquina entity = db.tblEstadoSmartHubNMaquina
                .Where(x => x.idMaquina.Equals(estadoSmartHubNMaquina.idMaquina))
                .OrderByDescending(x => x.fechaInicio)
                .FirstOrDefault();

            if (entity == null || !entity.idEstadoSmartHub.Equals(estadoSmartHubNMaquina.idEstadoSmartHub))
            {
                estadoSmartHubNMaquina.fechaInicio = DateTimeOffset.UtcNow;
                estadoSmartHubNMaquina.fechaUltimaActualizacion = DateTimeOffset.UtcNow;
                db.tblEstadoSmartHubNMaquina.Add(estadoSmartHubNMaquina);
            }
            else
            {
                entity.fechaUltimaActualizacion = DateTimeOffset.UtcNow;
            }

            await db.SaveChangesAsync();

            if (entity == null || !entity.idEstadoSmartHub.Equals(estadoSmartHubNMaquina.idEstadoSmartHub))
            {
                List<string> srcs = new List<string> { "tblEstadoSmartHubNMaquina" };
                _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}