using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class TraspasosController : ODataController
{
    private readonly bdERP db;
    public TraspasosController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost("odata/MyPolarier/Assistant/Traspasos/SetIsValidado")]
    [Authorize]
    public async Task<ActionResult> SetIsValidado([FromODataUri] int idMovimientoRecambio)
    {
        var entity = db.tblMovimientoRecambio.FirstOrDefault(mr => mr.idMovimientoRecambio == idMovimientoRecambio);

        if (entity == null)
            return NotFound();

        if (entity.isValidado == null) // No es un trasvase o es un trasvase que no pertenece a la familia de POLARIER GENERAL
            return BadRequest();

        entity.isValidado = !entity.isValidado;

        await db.SaveChangesAsync();

        return Ok(true);
    }
}
