using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCategoriaConvenioController : ODataController
{
    private readonly bdERP db;

    public tblCategoriaConvenioController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxExpansionDepth = 4)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] bool todas)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        List<int> categoriasConvNUsuario = db.tblCategoriaInterna.Where(x => x.idUsuario.Select(x => x.idUsuario).Contains(idUsuario)).Select(x => x.idCategoriaConvenio).Distinct().ToList();

        return Ok(db.tblCategoriaConvenio.Where(x => (!todas && (categoriasConvNUsuario.Count() > 0 && (categoriasConvNUsuario.Contains(x.idCategoriaConvenio))
        || categoriasConvNUsuario.Count() == 0)) || todas));
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(db.tblCategoriaConvenio.Where(x => x.idCategoriaConvenio == key));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblCategoriaConvenio categoriaConvenio)
    {
        try
        {
            db.tblCategoriaConvenio.Add(categoriaConvenio);
            await db.SaveChangesAsync();

            return Ok(Created(categoriaConvenio).Entity.idCategoriaConvenio);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblCategoriaConvenio> categoriaConvenio)
    {
        var entity = db.tblCategoriaConvenio.FirstOrDefault(x => x.idCategoriaConvenio.Equals(key));
        if (entity == null)
            return BadRequest();

        categoriaConvenio.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblCategoriaConvenio.FindAsync(key);
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
