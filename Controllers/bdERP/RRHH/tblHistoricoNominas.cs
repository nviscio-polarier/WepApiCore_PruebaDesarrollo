using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblHistoricoNominasController : ODataController
{
    private readonly bdERP db;

    public tblHistoricoNominasController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPatch("odata/tblHistoricoNominas/IU")]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int idPersona, [FromODataUri] DateTime fechaAltaContrato, [FromODataUri] DateTime fechaNomina, [FromBody] JsonPatchDocument<tblHistoricoNominas> historicoNominas)
    {
        var entity = db.tblHistoricoNominas.FirstOrDefault(x => x.idPersona.Equals(idPersona) && x.fechaAltaContrato.Date.Equals(fechaAltaContrato.Date) && x.fechaNomina.Date.Equals(fechaNomina.Date));
        if (entity == null) //INSERT
            entity = db.tblHistoricoNominas.Add(new tblHistoricoNominas()
            {
                idPersona = idPersona,
                fechaAltaContrato = fechaAltaContrato,
                fechaNomina = fechaNomina,
                isCerrado = false
            }).Entity;

        historicoNominas.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }
}
