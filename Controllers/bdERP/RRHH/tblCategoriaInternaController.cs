using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCategoriaInternaController : ODataController
{
    private readonly bdERP db;

    public tblCategoriaInternaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] bool todas)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();

        if (objUsuario == null)
            return BadRequest();

        List<int> categoriasNUsuario = db.tblCategoriaInterna
                            .Select(x => new tblCategoriaInterna { idCategoriaInterna = x.idCategoriaInterna, idUsuario = x.idUsuario })
                            .Where(x => x.idUsuario.Select(x => x.idUsuario).Contains(idUsuario)).Select(x => x.idCategoriaInterna).ToList();

        if (objUsuario.enableDatosRRHH)
        {
            return Ok(db.tblCategoriaInterna.Where(x => (!todas && (categoriasNUsuario.Count() > 0 && (categoriasNUsuario.Contains(x.idCategoriaInterna)) || categoriasNUsuario.Count() == 0) || todas)));
        }
        else
        {
            return Ok(db.tblCategoriaInterna
               .Where(x => (!todas && (categoriasNUsuario.Count() > 0 && (categoriasNUsuario.Contains(x.idCategoriaInterna)) || categoriasNUsuario.Count() == 0) || todas))
               .Select(x => new tblCategoriaInterna
               {
                   idCategoriaInterna = x.idCategoriaInterna,
                   idCategoriaConvenio = x.idCategoriaConvenio,
                   denominacion = x.denominacion,
                   idCategoriaConvenioNavigation = x.idCategoriaConvenioNavigation,
                   tblCategoriaInternaNTurno = x.tblCategoriaInternaNTurno
               }).ToList());
        }
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        if (objUsuario.enableDatosRRHH)
        {
            return Ok(db.tblCategoriaInterna.Where(x => x.idCategoriaInterna == key));
        }
        else
        {
            return Ok(db.tblCategoriaInterna.Where(x => x.idCategoriaInterna == key).Select(x => new { x.idCategoriaInterna, x.idCategoriaConvenio, x.denominacion }));
        }
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblCategoriaInterna categoriaInterna)
    {
        try
        {
            db.tblCategoriaInterna.Add(categoriaInterna);
            await db.SaveChangesAsync();

            return Ok(Created(categoriaInterna).Entity.idCategoriaInterna);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblCategoriaInterna> categoriaInterna)
    {
        var entity = db.tblCategoriaInterna.FirstOrDefault(x => x.idCategoriaInterna.Equals(key));
        if (entity == null)
            return BadRequest();

        categoriaInterna.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var entity = await db.tblCategoriaInterna.FindAsync(key);
        if (entity == null)
            return BadRequest();

        try
        {
            db.Remove(entity);
            await db.SaveChangesAsync();
            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}
