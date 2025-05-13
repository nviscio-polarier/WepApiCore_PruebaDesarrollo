using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblRepartoController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblRepartoController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery(MaxExpansionDepth = 0, MaxAnyAllExpressionDepth = 50, MaxNodeCount = 3000)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int? idLavanderia)
    {
        if (idLavanderia == -1)
        {
            idLavanderia = null;
        }
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia);

        return Ok(db.tblReparto.Where(x => idsEntidad.Contains((int)x.idEntidad) &&
        (idLavanderia == null || x.idEntidadNavigation.idLavanderia.Count(l => l.idLavanderia.Equals(idLavanderia)) > 0)));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblReparto reparto, int idUser)
    {
        int idUsuario = idUser != null ? idUser : int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        try
        {
            var resultParameter = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Output,
                Size = 8
            };

            db.Database.ExecuteSqlRaw(string.Format("SET @result = (SELECT Logistica.EF_funCodidoReparto({0}));", reparto.idLavanderia), resultParameter);
            reparto.codigo = (string)resultParameter.Value;
            reparto.idUsuarioPrepara = idUsuario;
            foreach (tblPrendaNReparto pnr in reparto.tblPrendaNReparto)
            {
                if (pnr.rechazo == null)
                {
                    pnr.rechazo = 0;
                }
                if (pnr.retiro == null)
                {
                    pnr.retiro = 0;
                }
            }

            db.tblReparto.Add(reparto);
            await db.SaveChangesAsync();

            db.Database.ExecuteSqlRaw(string.Format("EXEC [Logistica].[EF_spCalculaEstadoPedido] @idPedido = {0}", reparto.idPedido));

            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToAllUsers("notificaciones_LogisticaInterna_" + reparto.idLavanderia, "tblPedido|tblReparto");
            return Created(reparto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblReparto> reparto, int? idUser)
    {
        int idUsuario = (int)(idUser != null ? idUser : int.Parse(this.HttpContext.Items["idUsuario"].ToString()));

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblReparto.FirstOrDefault(x => x.idReparto.Equals(key));
        if (entity == null)
            return BadRequest();

        reparto.ApplyTo(entity);

        await db.SaveChangesAsync();

        //Cálculo en preparación.
        //Si se modifica el idRepartoEstado y queda diferente a 1 (Pendiente) se libera el pedido del idUsuarioPrepara_activo;
        var operation_idRepartoEstado = reparto.Operations.Where(x => x.path.Equals("/idRepartoEstado"));
        if (operation_idRepartoEstado.Count() > 0)
        {
            if (int.Parse(operation_idRepartoEstado.First().value.ToString()) != 1)
            {
                tblPedido objPedido = db.tblPedido.Where(x => x.idPedido.Equals(entity.idPedido)).FirstOrDefault();
                if (objPedido != null)
                {
                    objPedido.idUsuarioPrepara_activo = null;
                    await db.SaveChangesAsync();
                }
            }
        }

        db.Database.ExecuteSqlRaw(string.Format("EXEC [Logistica].[EF_spCalculaEstadoPedido] @idPedido = {0}", entity.idPedido));

        NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
        notificaciones.SendToAllUsers("notificaciones_LogisticaInterna_" + entity.idLavanderia, "tblPedido|tblReparto");

        return Updated(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var reparto = await db.tblReparto.FindAsync(key);

        if (reparto == null)
            return false;

        var repartoNParteTransporte = db.tblRepartoNParteTransporte.Where(x => x.idReparto == reparto.idReparto);
        var prendaNReparto = db.tblPrendaNReparto.Where(x => x.idReparto == key);

        if (repartoNParteTransporte != null)
        {
            foreach (var repTransporte in repartoNParteTransporte)
            {
                db.Remove(repTransporte);
            }
        }

        if (prendaNReparto != null)
        {
            foreach (var prendaNRep in prendaNReparto)
            {
                db.Remove(prendaNRep);
            }
        }

        db.Remove(reparto);

        await db.SaveChangesAsync();

        db.Database.ExecuteSqlRaw(string.Format("EXEC [Logistica].[EF_spCalculaEstadoPedido] @idPedido = {0}", reparto.idPedido));
        NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
        notificaciones.SendToAllUsers("notificaciones_LogisticaInterna_" + reparto.idLavanderia, "tblPedido|tblReparto");

        return true;
    }

    #region FUNCIONES GENERALES

    #endregion
}