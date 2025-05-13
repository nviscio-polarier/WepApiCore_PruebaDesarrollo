using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;
public class tblInventario_DocumentoController : ODataController
{
    private readonly bdERP db;
    public tblInventario_DocumentoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int? idInventario)
    {
        return Ok(db.tblInventario_Documento.Where(invDoc => idInventario == null || invDoc.idInventario == idInventario));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblInventario_Documento tblInventario_Documento)
    {
        db.tblInventario_Documento.Add(tblInventario_Documento);

        await db.SaveChangesAsync();

        return Created(tblInventario_Documento);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblInventario_Documento.FindAsync(key);
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}