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

[AllowAnonymous]
public class tblLecturaLavadorasController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblLecturaLavadorasController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblLecturaLavadoras);
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get([FromODataUri] int idMaquina, [FromODataUri] DateTime fecha)
    {
        return Ok(db.tblLecturaLavadoras.Find(idMaquina, fecha));
    }

    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] tblLecturaLavadoras lecturaLavadoras)
    {
        try
        {
            tblEntidad objEnt = db.tblEntidad.Where(x => x.idEntidad == lecturaLavadoras.idEntidad).FirstOrDefault();
            if (objEnt == null)
            {
                lecturaLavadoras.idEntidad = null;
                lecturaLavadoras.lecturaValida = false;
            }

            tblLecturaLavadoras encontrado = db.tblLecturaLavadoras.Where(x => x.idMaquina == lecturaLavadoras.idMaquina && x.fecha == lecturaLavadoras.fecha).FirstOrDefault();
            if (encontrado == null)
            {
                db.tblLecturaLavadoras.Add(lecturaLavadoras);
                await db.SaveChangesAsync();

                #region SignalR
                int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == lecturaLavadoras.idMaquina).FirstOrDefault().idLavanderia;

                NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
                _hubContext.Clients.Group("LecturaLavadorasHub_" + idLavanderia).SendAsync("signalR_refresh");
                #endregion
            }

            return Ok(1);
        }
        catch
        {
            return Ok(-1);
        }
    }
}