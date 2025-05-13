using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCierreDatos_FacturacionController : ODataController
{
    private readonly bdERP db;

    public tblCierreDatos_FacturacionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxNodeCount = 3000)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblCierreDatos_Facturacion);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblCierreDatos_Facturacion cierreDatos)
    {
        try
        {
            tblEntidad ent = db.tblEntidad.Find(cierreDatos.idEntidad);

            cierreDatos.idTipoConsumoLenceria = (byte)ent.idTipoConsumoLenceria;
            cierreDatos.idTipoFacturacionCliente = (byte)ent.idTipoFacturacionCliente;
            cierreDatos.objKgEstancia = (decimal)ent.objKgEstancia;
            cierreDatos.costeEstancia = (decimal)ent.costeEstancia;

            db.tblCierreDatos_Facturacion.Add(cierreDatos);
            await db.SaveChangesAsync();

            return Created(cierreDatos);
        }
        catch (Exception ex)
        {
            return BadRequest("Error de BDD");
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int keyidEntidad, [FromODataUri] short keyaño, [FromODataUri] byte keymes, [FromBody] JsonPatchDocument<tblCierreDatos_Facturacion> cierreDatos)
    {
        var entity = await db.tblCierreDatos_Facturacion.FindAsync(keyidEntidad, keyaño, keymes);
        if (entity == null)
        {
            return NotFound();
        }

        cierreDatos.ApplyTo(entity);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int keyidEntidad, [FromODataUri] byte keyaño, [FromODataUri] short keymes)
    {
        var entity = await db.tblCierreDatos_Facturacion.FindAsync(keyidEntidad, keyaño, keymes);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblCierreDatos_Facturacion.Remove(entity);

        await db.SaveChangesAsync();
        return NoContent();
    }
}
