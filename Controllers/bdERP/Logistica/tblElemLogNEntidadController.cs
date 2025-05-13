using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblElemLogNPedidoController : ODataController
{
    private readonly bdERP db;

    public tblElemLogNPedidoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblElemLogNPedido);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<bool> Post([FromBody] tblElemLogNPedido tblElemLogNPedido)
    {
        try
        {

            db.tblElemLogNPedido.Add(tblElemLogNPedido);
            await db.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int keyidPedido, [FromODataUri] int keyidTipoElemLog)
    {

        var elemento = db.tblElemLogNPedido.FirstOrDefault(x => x.idPedido == keyidPedido && x.idTipoElemLog == keyidTipoElemLog);

        if (elemento != null)
        {
            db.Remove(elemento);
            await db.SaveChangesAsync();
            return true;
        }
        {

            return false;
        }
    }

}

