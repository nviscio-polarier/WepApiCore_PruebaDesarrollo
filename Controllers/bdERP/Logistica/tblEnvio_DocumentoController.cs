using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblEnvio_DocumentoController : ODataController
{
    private readonly bdERP db;

    public tblEnvio_DocumentoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int idEnvio)
    {
        return Ok(db.tblEnvio_Documento.Where(x => x.idEnvio == idEnvio));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblEnvio_Documento documento)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        db.tblEnvio_Documento.Add(documento);
        await db.SaveChangesAsync();
        return Created(documento);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int keyidDocumento, [FromODataUri] int keyidEnvio)
    {
        var entity = await db.tblEnvio_Documento.FindAsync(keyidDocumento, keyidEnvio);
        if (entity == null)
        {
            return NotFound();
        }

        db.tblEnvio_Documento.Remove(entity);

        await db.SaveChangesAsync();
        return NoContent();
    }
}
