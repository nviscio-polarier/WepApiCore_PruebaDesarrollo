using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPrendaEjecutivoController : ODataController
{
    private readonly bdERP db;

    public tblPrendaEjecutivoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get(int? idLavanderia)
    {
        return Ok(db.tblPrendaEjecutivo.Where(pe => (pe.idLavanderia == idLavanderia || idLavanderia == null) && pe.eliminado == false));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromODataUri] int idLavanderia, [FromBody] tblPrendaEjecutivo prendaEjecutivo)
    {
        var resultParameter = new SqlParameter
        {
            ParameterName = "@result",
            SqlDbType = SqlDbType.NVarChar,
            Direction = ParameterDirection.Output,
            Size = 8
        };
        db.Database.ExecuteSqlRaw("SET @result = (SELECT MyValet.EF_funCodigoPrendaEjecutivo(" + idLavanderia + "));", resultParameter).ToString();

        prendaEjecutivo.codigoPrendaEjecutivo = (string)resultParameter.Value;
        prendaEjecutivo.idLavanderia = idLavanderia;
        prendaEjecutivo.eliminado = false;

        db.tblPrendaEjecutivo.Add(prendaEjecutivo);
        await db.SaveChangesAsync();

        return Created(prendaEjecutivo);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, Delta<tblPrendaEjecutivo> prendaEjecutivo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var entity = await db.tblPrendaEjecutivo.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        prendaEjecutivo.Patch(entity);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest("Error de BDD");
        }
        return Updated(entity);
    }


    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblPrendaEjecutivo.FindAsync(key);
        if (entity == null)
            return false;

        entity.eliminado = true;

        await db.SaveChangesAsync();
        return true;
    }
}
