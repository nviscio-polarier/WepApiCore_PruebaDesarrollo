using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblDocumentoPrendaController : ODataController
{
    private readonly bdERP db;

    public tblDocumentoPrendaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]

    public async Task<IQueryable<tblDocumentoPrenda>> Get()
    {
        return db.tblDocumentoPrenda;
    }

    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(db.tblDocumentoPrenda.Where(x => x.idDocumento == key));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblDocumentoPrenda documento)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = db.tblDocumentoPrenda.FirstOrDefault(x => x.idPrenda.Equals(documento.idPrenda));
        if (entity != null)
            db.Remove(entity);

        db.tblDocumentoPrenda.Add(documento);
        await db.SaveChangesAsync();


        return Created(documento);
    }


    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblDocumentoPrenda.FindAsync(key);
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
