using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPrendaController : ODataController
{
    private readonly bdERP db;

    public tblPrendaController(bdERP context)
    {
        db = context;
    }


    [EnableQuery(MaxExpansionDepth = 1, MaxNodeCount = 3000)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblPrenda);
    }

    [EnableQuery]
    [HttpDelete]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var entity = await db.tblPrenda.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblPrendaNAlmacen.RemoveRange(db.tblPrendaNAlmacen.Where(x => x.idPrenda == key));
        db.tblPrendaNSubAlmacen.RemoveRange(db.tblPrendaNSubAlmacen.Where(x => x.idPrenda == key));
        db.tblPrendaNEntidad_NuevoPedido.RemoveRange(db.tblPrendaNEntidad_NuevoPedido.Where(x => x.idPrenda == key));
        db.tblPrendaNUsuarioNEntidad.RemoveRange(db.tblPrendaNUsuarioNEntidad.Where(x => x.idPrenda == key));
        db.tblPrendaNTipoHabitacion.RemoveRange(db.tblPrendaNTipoHabitacion.Where(x => x.idPrenda == key));
        db.tblDocumentoPrenda.RemoveRange(db.tblDocumentoPrenda.Where(x => x.idPrenda == key));

        entity.eliminado = true;
        entity.activo = false;

        await db.SaveChangesAsync();

        return NoContent();
    }
}
