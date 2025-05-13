using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblIncidencia_DocumentoController : ODataController
{
    private readonly bdERP db;

    public tblIncidencia_DocumentoController(bdERP context)
    {
        db = context;
    }


    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblIncidencia_Documento);
    }

    [EnableQuery]
    [HttpPost("odata/tblIncidencia_Documento/PostMasivo")]
    public async Task<ActionResult> PostMasivo([FromBody] List<tblIncidencia_Documento> incDoc)
    {
        db.tblIncidencia_Documento.AddRange(incDoc);
        await db.SaveChangesAsync();

        return Ok(true);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int keyidDocumento, [FromODataUri] int keyidIncidencia)
    {
        var entity = db.tblIncidencia_Documento.Where(x => x.idDocumento.Equals(keyidDocumento) && x.idIncidencia.Equals(keyidIncidencia)).FirstOrDefault();
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();

        return true;
    }
}
