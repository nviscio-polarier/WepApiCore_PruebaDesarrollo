using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPrendaNRepartoController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblPrendaNRepartoController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery(MaxExpansionDepth = 0, MaxNodeCount = 1000)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int idReparto)
    {
        return Ok(db.tblPrendaNReparto.Where(x => x.idReparto.Equals(idReparto)));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblPrendaNReparto prendaNReparto)
    {
        try
        {
            if (prendaNReparto.rechazo == null)
            {
                prendaNReparto.rechazo = 0;
            }
            if (prendaNReparto.retiro == null)
            {
                prendaNReparto.retiro = 0;
            }

            db.tblPrendaNReparto.Add(prendaNReparto);
            await db.SaveChangesAsync();

            int idPedido = (int)db.tblReparto.Find(prendaNReparto.idReparto).idPedido;
            db.Database.ExecuteSqlRaw(string.Format("EXEC [Logistica].[EF_spCalculaEstadoPedido] @idPedido = {0}", idPedido));

            //Prenda N Lavandería
            tblReparto objReparto = db.tblReparto.Find(prendaNReparto.idReparto);
            var pnl = db.tblPrendaNLavanderia.FirstOrDefault(x => x.idPrenda.Equals(prendaNReparto.idPrenda) && x.idLavanderia.Equals(objReparto.idLavanderia));
            if (pnl != null)
            {
                pnl.stock -= prendaNReparto.cantidad;
                pnl.stock = pnl.stock < 0 ? 0 : pnl.stock;

                await db.SaveChangesAsync();

                NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
                notificaciones.SendToAllUsers("notificaciones_LogisticaInterna_" + objReparto.idLavanderia, "tblPrendaNLavanderia");
            }

            return Created(prendaNReparto);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int keyidPrenda, [FromODataUri] int keyidReparto, [FromBody] JsonPatchDocument<tblPrendaNReparto> prendaNReparto, int? idUser)
    {
        int idUsuario = (int)(idUser != null ? idUser : int.Parse(this.HttpContext.Items["idUsuario"].ToString()));
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblPrendaNReparto.FirstOrDefault(x => x.idPrenda.Equals(keyidPrenda) && x.idReparto.Equals(keyidReparto));
        if (entity == null)
            return BadRequest();

        var cantidad_old = entity.cantidad;

        int idPedido = (int)db.tblReparto.Find(entity.idReparto).idPedido;

        prendaNReparto.ApplyTo(entity);

        await db.SaveChangesAsync();

        db.Database.ExecuteSqlRaw(string.Format("EXEC [Logistica].[EF_spCalculaEstadoPedido] @idPedido = {0}", idPedido));

        //Prenda N Lavandería
        tblReparto objReparto = db.tblReparto.Find(entity.idReparto);
        var pnl = db.tblPrendaNLavanderia.FirstOrDefault(x => x.idPrenda.Equals(keyidPrenda) && x.idLavanderia.Equals(objReparto.idLavanderia));
        if (pnl != null)
        {
            pnl.stock += cantidad_old - entity.cantidad;
            pnl.stock = pnl.stock < 0 ? 0 : pnl.stock;

            await db.SaveChangesAsync();

            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToAllUsers("notificaciones_LogisticaInterna_" + objReparto.idLavanderia, "tblPrendaNLavanderia");
        }

        return Ok(entity);
    }


    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int keyidPrenda, [FromODataUri] int keyidReparto)
    {
        var entity = db.tblPrendaNReparto.FirstOrDefault(x => x.idPrenda.Equals(keyidPrenda) && x.idReparto.Equals(keyidReparto));
        if (entity == null)
        {
            return false;
        }

        tblReparto objReparto = db.tblReparto.Find(entity.idReparto);
        int idPedido = (int)objReparto.idPedido;

        db.Remove(entity);
        await db.SaveChangesAsync();

        //Eliminamos repartos vacios
        var numPrendasNReparto = db.tblPrendaNReparto.Count(x => x.idReparto.Equals(keyidReparto));
        if (numPrendasNReparto == 0)
        {
            db.tblReparto.Remove(objReparto);
            await db.SaveChangesAsync();
        }


        db.Database.ExecuteSqlRaw(string.Format("EXEC [Logistica].[EF_spCalculaEstadoPedido] @idPedido = {0}", idPedido));

        //Prenda N Lavandería
        var pnl = db.tblPrendaNLavanderia.FirstOrDefault(x => x.idPrenda.Equals(keyidPrenda) && x.idLavanderia.Equals(objReparto.idLavanderia));
        if (pnl != null)
        {
            pnl.stock += entity.cantidad;

            await db.SaveChangesAsync();

            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToAllUsers("notificaciones_LogisticaInterna_" + objReparto.idLavanderia, "tblPrendaNLavanderia");
        }

        return true;
    }
}
