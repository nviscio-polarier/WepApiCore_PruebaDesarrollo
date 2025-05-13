using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblMovimientoController : ODataController
{
    private readonly bdERP db;

    public tblMovimientoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxNodeCount = 3000)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int? idLavanderia)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia);

        List<int> idsCompañia = db.tblEntidad.Where(e => idsEntidad.Contains(e.idEntidad)).Select(e => e.idCompañia).ToList();

        var resultado = db.tblMovimiento.Where(m => idsCompañia.Contains(m.idCompañia ?? -1) || idsEntidad.Contains(m.idEntidad ?? -1) );
        //|| m.idGrupoPlantillaPrenda_generica != null

        return Ok(resultado);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromODataUri] int? idLavanderia, [FromBody] tblMovimiento movimiento)
    {
        db.tblMovimiento.Add(movimiento);
        await db.SaveChangesAsync();

        return Created(movimiento);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblMovimiento> movimiento)
    {
        var entity = await db.tblMovimiento.FindAsync(key);
        if (entity == null)
            return NotFound();

        movimiento.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Updated(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var entity = await db.tblMovimiento.FindAsync(key);
        if (entity == null)
            return NotFound();


        db.tblPrendaNMovimiento.RemoveRange(db.tblPrendaNMovimiento.Where(x => x.idMovimiento.Equals(entity.idMovimiento)));
        db.tblMovimiento.Remove(entity);

        await db.SaveChangesAsync();

        return NoContent();
    }
}
