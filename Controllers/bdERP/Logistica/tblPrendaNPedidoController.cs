using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPrendaNPedidoController : ODataController
{
    private readonly bdERP db;
    public tblPrendaNPedidoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxExpansionDepth = 5)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get(int? idPedido)
    {
        return Ok(db.tblPrendaNPedido.Where(x => ((idPedido != null && x.idPedido.Equals(idPedido)) || idPedido == null)));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblPrendaNPedido prendaNPedido)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        try
        {
            db.tblPrendaNPedido.Add(prendaNPedido);
            await db.SaveChangesAsync();

            return Created(prendaNPedido);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPrendaNPedido> prendaNpedido)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblPrendaNPedido.FirstOrDefault(x => x.idPrendaNPedido.Equals(key));
        if (entity == null)
            return BadRequest();

        prendaNpedido.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = db.tblPrendaNPedido.Where(x => x.idPrendaNPedido.Equals(key)).FirstOrDefault();
        if (entity == null)
        {
            return false;
        }

        db.Remove(entity);
        await db.SaveChangesAsync();

        return true;
    }
}
