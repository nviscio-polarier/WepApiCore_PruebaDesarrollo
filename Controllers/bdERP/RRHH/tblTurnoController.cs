using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Newtonsoft.Json;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTurnoController : ODataController
{
    private readonly bdERP db;

    public tblTurnoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int? idLavanderia)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var objUsuario = db.tblUsuario
      .Where(x => x.idUsuario.Equals(idUsuario) && !x.isEliminado)
      .Select(x => new
      {
          x.idUsuario,
          x.idLavanderia
      }).FirstOrDefault();

        if (objUsuario == null)
            return BadRequest();


        return Ok(db.tblTurno.Where(x => objUsuario.idLavanderia.Select(l => l.idLavanderia).Contains(x.idLavanderia) &&
        ((idLavanderia != null && idLavanderia == x.idLavanderia) || idLavanderia == null)));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblTurno turno)
    {
        try
        {
            turno.activo = true;
            turno.eliminado = false;

            db.tblTurno.Add(turno);
            await db.SaveChangesAsync();

            return Ok(Created(turno).Entity.idTurno);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblTurno> turno)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();

        if (objUsuario == null)
            return BadRequest();

        List<int> categoriasNUsuario = db.tblCategoriaInterna
                  .Select(x => new tblCategoriaInterna { idCategoriaInterna = x.idCategoriaInterna, idUsuario = x.idUsuario })
                  .Where(x => x.idUsuario.Select(x => x.idUsuario).Contains(idUsuario)).Select(x => x.idCategoriaInterna).ToList();

        var entity = db.tblTurno.FirstOrDefault(x => x.idTurno.Equals(key));
        if (entity == null)
            return BadRequest();

        var operation = turno.Operations.FirstOrDefault(x => x.path.Equals("/tblCategoriaInternaNTurno"));
        if (operation != null)
        {
            List<int> ids_categoriaInterna = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(operation.value));

            db.tblCategoriaInternaNTurno.RemoveRange(db.tblCategoriaInternaNTurno.Where(x => x.idTurno == key && (categoriasNUsuario.Count() > 0 && (categoriasNUsuario.Contains(x.idCategoriaInterna)) || categoriasNUsuario.Count() == 0)
            ));
            await db.SaveChangesAsync();

            turno.Operations.FirstOrDefault(x => x.path.Equals("/tblCategoriaInternaNTurno")).value = null;

            List<tblCategoriaInternaNTurno> tblCategoriaInternaNTurno_list = new List<tblCategoriaInternaNTurno>();
            foreach (int id_categoria in ids_categoriaInterna)
            {
                tblCategoriaInternaNTurno obj = new tblCategoriaInternaNTurno();
                obj.idCategoriaInterna = id_categoria;
                obj.idTurno = key;

                tblCategoriaInternaNTurno_list.Add(obj);
            }
            turno.Operations.FirstOrDefault(x => x.path.Equals("/tblCategoriaInternaNTurno")).value = tblCategoriaInternaNTurno_list;
        }

        turno.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblTurno.FindAsync(key);
        if (entity == null)
            return false;

        var subTurnos = db.tblTurno.Where(x => x.idTurnoPadre == key);

        foreach (var subTurno in subTurnos)
        {
            subTurno.activo = false;
            subTurno.eliminado = true;
        }

        entity.activo = false;
        entity.eliminado = true;

        await db.SaveChangesAsync();
        return true;
    }
}
