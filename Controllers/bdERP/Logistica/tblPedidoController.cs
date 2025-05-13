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

public class tblPedidoController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblPedidoController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery(MaxNodeCount = 3000)]
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

        return Ok(db.tblPedido.Where(x => idsEntidad.Contains(x.idEntidad) &&
        (idLavanderia == null || x.idEntidadNavigation.idLavanderia.Count(l => l.idLavanderia.Equals(idLavanderia)) > 0))); //INNECESARIO?
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblPedido pedido)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        try
        {
            var resultParameter = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Output,
                Size = 8
            };

            db.Database.ExecuteSqlRaw("SET @result = (SELECT Logistica.EF_funCodigoPedido());", resultParameter);
            string codigo = (string)resultParameter.Value;

            pedido.idUsuarioCreador = idUsuario;
            pedido.codigo = codigo;
            pedido.fechaRegistro = DateTime.Now;
            db.tblPedido.Add(pedido);
            await db.SaveChangesAsync();

            List<int> idsLavanderia = db.tblEntidad.Where(x => x.idEntidad.Equals(pedido.idEntidad)).Select(x => x.idLavanderia).First().Select(x => x.idLavanderia).ToList();
            foreach (int idLav in idsLavanderia)
            {
                NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
                notificaciones.SendToAllUsers("notificaciones_LogisticaInterna_" + idLav, "tblPedido");
            }

            return Created(pedido);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPedido> pedido)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblPedido.FirstOrDefault(x => x.idPedido.Equals(key));
        if (entity == null)
            return BadRequest();

        pedido.ApplyTo(entity);

        await db.SaveChangesAsync();

        List<int> idsLavanderia = db.tblEntidad.Where(x => x.idEntidad.Equals(entity.idEntidad)).Select(x => x.idLavanderia).First().Select(x => x.idLavanderia).ToList();

        foreach (int idLav in idsLavanderia)
        {
            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToAllUsers("notificaciones_LogisticaInterna_" + idLav, "tblPedido");
        }

        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var pedido = await db.tblPedido.FindAsync(key);
        if (pedido == null)
        {
            return NotFound();
        }

        var prendas = db.tblPrendaNPedido.Where(x => x.idPedido.Equals(key)).ToList();
        if (prendas.Count > 0)
        {
            db.tblPrendaNPedido.RemoveRange(prendas);
            await db.SaveChangesAsync();
        }

        db.tblPedido.Remove(pedido);

        await db.SaveChangesAsync();
        return Ok(true);
    }
}

//Eliminamos los registros que no se encuentran en la operación.
//var operation = pedido.Operations.FirstOrDefault(x => x.path.Equals("/tblPrendaNPedido"));
//if (operation != null)
//{
//    List<int> idsPrenda_finales = JsonConvert.DeserializeObject<List<tblPrendaNPedido>>(JsonConvert.SerializeObject(operation.value))
//        .Select(x=>x.idPrenda).ToList();

//    db.tblPrendaNPedido.RemoveRange(db.tblPrendaNPedido.Where(x => x.idPedido.Equals(key) && !idsPrenda_finales.Contains(x.idPrenda)));
//}