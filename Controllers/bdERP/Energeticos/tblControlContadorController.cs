using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblControlContadorController : ODataController
{
    private readonly bdERP db;

    public tblControlContadorController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPatch("odata/tblControlContador/{idRecursoContador}")]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int idRecursoContador, [FromODataUri] DateTime fecha, [FromBody] JsonPatchDocument<tblControlContador> recursoContador)
    {
        var entity = db.tblControlContador.Where(x => x.idRecursoContador.Equals(idRecursoContador) && x.fecha == fecha.Date).FirstOrDefault();

        decimal? diferenciaAnt = null;
        if (entity == null)
        {
            tblRecursoContador rc = db.tblRecursoContador.Find(idRecursoContador);
            entity = db.tblControlContador.Add(new tblControlContador()
            {
                idRecursoContador = idRecursoContador,
                fecha = fecha.Date,
                actual = 0,
                sumaInforme = rc.sumaInforme
            }).Entity;
        }
        else
        {
            diferenciaAnt = entity.diferencia;
        }

        recursoContador.ApplyTo(entity);
        await db.SaveChangesAsync();

        //Recalculamos la diferenncia del día siguiente por si hubiera cambiado el valor actual.
        if (diferenciaAnt != null)
        {
            var entity_diaSiguiente = db.tblControlContador.Where(x => x.idRecursoContador.Equals(idRecursoContador) && x.fecha > fecha.Date).OrderBy(x => x.fecha).FirstOrDefault();
            decimal? diff = diferenciaAnt - entity.diferencia;
            entity_diaSiguiente.diferencia = entity_diaSiguiente.diferencia + diff;
            await db.SaveChangesAsync();
        }

        return Ok(entity);
    }
}

