using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class InventariosController : ODataController
{
    private readonly bdERP db;
    private readonly tblMovimientoRecambioController mrc;
    private readonly AssistantController ac; private readonly IHubContext<NotificacionesHub> hubContext;

    public InventariosController(bdERP context, IHubContext<NotificacionesHub> _hubContext)
    {
        db = context;
        mrc = new(context);
        ac = new(context);
        hubContext = _hubContext;

    }

    [EnableQuery]
    [HttpPost("odata/Assistant/Inventarios/IU_recambioInventario")]
    [Authorize]
    public async Task<ActionResult> IU_recambioInventario([FromODataUri] int idMovimientoRecambio, [FromODataUri] bool isForced, [FromBody] tblRecambioNMovimientoRecambio recambio)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        //1. Obtener datos de movimientoRecambio
        var objMovimientoRecambio = db.tblMovimientoRecambio
            .Include(mr => mr.tblRecambioNMovimientoRecambio)
            .FirstOrDefault(mr => mr.idMovimientoRecambio == idMovimientoRecambio);

        if (objMovimientoRecambio == null)
        {
            return BadRequest("El inventario no existe.");
        }

        //2. Comprobar si existe el recambio en tblRecambioNMovimientoRecambio mediante getRecambiosInventario
        var recambioSel = objMovimientoRecambio.tblRecambioNMovimientoRecambio.FirstOrDefault(rnmr => rnmr.idRecambio == recambio.idRecambio);
        bool recambioRepetido = recambioSel != null;

        //3. Si existe devolver mensaje de aviso.
        if (recambioRepetido && isForced == false)
        {
            return BadRequest("EXISTE");
        }

        //4. Si no existe  
        tblMovimientoRecambioController obj = new(db);

        if (recambioRepetido == false)
        {
            objMovimientoRecambio.tblRecambioNMovimientoRecambio = new List<tblRecambioNMovimientoRecambio>
                                                                  {
                                                                      new ()
                                                                      {
                                                                          idRecambio = recambio.idRecambio,
                                                                          cantidad = recambio.cantidad,
                                                                          ubicacion = recambio.ubicacion,
                                                                          precio = recambio.precio,
                                                                          idUsuario = idUsuario,
                                                                          fecha = recambio.fecha,
                                                                          isApp = recambio.isApp
                                                                      }
                                                                  };


        }//5. Si existe y se fuerza la actualización
        else if (recambioRepetido && isForced == true)
        {
            recambio.idUsuario = idUsuario;
            objMovimientoRecambio.tblRecambioNMovimientoRecambio = new List<tblRecambioNMovimientoRecambio> { recambio };
        }


        var response = await obj.spIUD_tblMovimientoRecambio(objMovimientoRecambio, "patch", false);
        if (!response.resultado)
            return BadRequest("INVALIDO");

        var srcs = recambio.isApp ? "isApp" : "isMyPolarier";
        await hubContext.Clients.Group("InventarioRecambios_" + idMovimientoRecambio).SendAsync("InventarioRecambios/signalR_refresh", srcs);

        return Ok(true);
    }


}
