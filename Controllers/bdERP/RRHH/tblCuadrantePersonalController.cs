using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCuadrantePersonalController : ODataController
{
    private readonly bdERP db;

    public tblCuadrantePersonalController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblCuadrantePersonal);
    }

    //[EnableQuery]
    //[HttpPost]
    //[Authorize]
    //public async Task<ActionResult> Post([FromBody] tblCuadrantePersonal cuadrantePersonal)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    db.tblCuadrantePersonal.Add(cuadrantePersonal);
    //    await db.SaveChangesAsync();

    //    return Ok(cuadrantePersonal);
    //}

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblCuadrantePersonal> cuadrantePersonal)
    {
        var entity = db.tblCuadrantePersonal.FirstOrDefault(x => x.idCuadrantePersonal.Equals(key));
        if (entity == null)
        {
            return BadRequest();
        }

        cuadrantePersonal.ApplyTo(entity);

        var operation_idCalendario_Estado = cuadrantePersonal.Operations.FirstOrDefault(x => x.path.Equals("/idCalendario_Estado"));
        if (operation_idCalendario_Estado != null)
        {
            var registroCalendario = db.tblCalendarioPersonal.FirstOrDefault(x => entity.idPersona == x.idPersona && entity.fecha == x.fecha);
            // Estos estados son ignorados por que pertenecen a Lavandería o a Jornada
            var estadosIgnorados = (entity.idCalendario_Estado == 3 || entity.idCalendario_Estado == 4 || entity.idCalendario_Estado == 8 || entity.idCalendario_Estado == 10);
            if (registroCalendario != null && estadosIgnorados)
            {
                db.tblCalendarioPersonal.Remove(registroCalendario);
            }
            else if (registroCalendario != null && !estadosIgnorados)
            {
                registroCalendario.idCalendario_Estado = (byte)entity.idCalendario_Estado;
            }
            else if (registroCalendario == null && !estadosIgnorados)
            {
                db.tblCalendarioPersonal.Add(new tblCalendarioPersonal { idPersona = entity.idPersona, fecha = entity.fecha, idCalendario_Estado = (byte)entity.idCalendario_Estado });
            }
        }

        if (entity.idCalendario_Estado == 8)
        {
            db.tblCuadrantePersonal.Remove(entity);
        }

        await db.SaveChangesAsync();

        return Ok(entity);
    }

}
