using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblRecursoContadorController : ODataController
{
    private readonly bdERP db;

    public tblRecursoContadorController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxNodeCount = 3000)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int? idRecursoContador)
    {
        if (idRecursoContador != null)
        {
            return Ok(db.tblRecursoContador.Where(x => x.idRecursoContador == idRecursoContador));
        }

        return Ok(db.tblRecursoContador);
    }


    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblRecursoContador recursoContador)
    {
        db.tblRecursoContador.Add(recursoContador);
        await db.SaveChangesAsync();

        return Created(recursoContador);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblRecursoContador> recursoContador)
    {
        var entity = db.tblRecursoContador.Where(x => x.idRecursoContador.Equals(key)).FirstOrDefault();


        #region Guardar tblRecursoVirtual_Calculo

        // Observa si hay cambios en tblRecursoVirtual_Calculo
        var operation_tblRecursoVirtual_Calculo = recursoContador.Operations.FirstOrDefault(x => x.path.Equals("/tblRecursoVirtual_CalculoidRecursoVirtualNavigation"));
        if (operation_tblRecursoVirtual_Calculo != null) // Si hay cambios en tblRecursoVirtual_Calculo
        {
            db.tblRecursoVirtual_Calculo.RemoveRange(db.tblRecursoVirtual_Calculo.Where(x => x.idRecursoVirtual.Equals(key))); // Elimina los antiguos registros de tblRecursoVirtual_Calculo
        }

        #endregion

        recursoContador.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(recursoContador);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblRecursoContador.FindAsync(key);
        if (entity == null)
            return false;

        entity.eliminado = true;
        entity.activo = false;

        await db.SaveChangesAsync();
        return true;
    }
}

