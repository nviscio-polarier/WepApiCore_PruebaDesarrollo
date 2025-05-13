using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblEstadoNominaNNominaController : ODataController
{
    private readonly bdERP db;

    public tblEstadoNominaNNominaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public ActionResult Get([FromODataUri] int? idNomina, [FromODataUri] int? idEstadoNomina)
    {
        var tblEstadoNominaNNomina = db.tblEstadoNominaNNomina
            .Where(ennn =>
                (idNomina == null || ennn.idNomina == idNomina)
                && (idEstadoNomina == null || ennn.idEstadoNomina == idEstadoNomina)
            );

        if (idNomina != null || idEstadoNomina != null)
        {
            tblEstadoNominaNNomina = tblEstadoNominaNNomina.OrderByDescending(ennn => ennn.fecha).Take(1);
        }

        return Ok(tblEstadoNominaNNomina);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblEstadoNominaNNomina estadoNominaNNomina)
    {
        int idUsuario = int.Parse(HttpContext.Items["idUsuario"].ToString());

        var nomina = db.tblNomina.FirstOrDefault(x => x.idNomina == estadoNominaNNomina.idNomina);

        if (nomina == null || nomina.idEstadoNomina == estadoNominaNNomina.idEstadoNomina)
        {
            return BadRequest();
        }

        // Si la paga es parte del finiquito de una persona. Se cambiará el estado a todas a la vez.
        if (nomina.fechaBaja != null)
        {
            var finiquitos = db.tblNomina.Where(n => nomina.idNomina != n.idNomina && nomina.idPersona == n.idPersona && nomina.fechaBaja == n.fechaBaja);

            List<tblEstadoNominaNNomina> estadoNominaNNomina_finquitos = new();

            foreach (var f in finiquitos)
            {
                f.idEstadoNomina = estadoNominaNNomina.idEstadoNomina;

                estadoNominaNNomina_finquitos.Add(new()
                {
                    idNomina = f.idNomina,
                    idEstadoNomina = estadoNominaNNomina.idEstadoNomina,
                    fecha = DateTimeOffset.UtcNow,
                    idUsuario_valida = idUsuario,
                    observaciones = estadoNominaNNomina.observaciones
                });
            }

            db.tblEstadoNominaNNomina.AddRange(estadoNominaNNomina_finquitos);
        }

        estadoNominaNNomina.idUsuario_valida = idUsuario;
        estadoNominaNNomina.fecha = DateTimeOffset.UtcNow;

        nomina.idEstadoNomina = estadoNominaNNomina.idEstadoNomina;

        db.tblEstadoNominaNNomina.Add(estadoNominaNNomina);

        await db.SaveChangesAsync();

        return Created(estadoNominaNNomina);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromBody] tblEstadoNominaNNomina estadoNominaNNomina)
    {
        int idUsuario = int.Parse(HttpContext.Items["idUsuario"].ToString());

        var entity = db.tblEstadoNominaNNomina
            .Where(ennn => ennn.idNomina == estadoNominaNNomina.idNomina && ennn.idEstadoNomina == estadoNominaNNomina.idEstadoNomina)
            .OrderByDescending(ennn => ennn.fecha)
            .FirstOrDefault();

        if (entity == null)
        {
            return BadRequest();
        }

        entity.observaciones = estadoNominaNNomina.observaciones;

        await db.SaveChangesAsync();

        return Updated(entity);
    }
}
