using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

[Authorize]
public class tblMantenimientoPrevController : ODataController
{
    private readonly bdERP db;

    public tblMantenimientoPrevController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxExpansionDepth = 3)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblMantenimientoPrev);
    }


    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblMantenimientoPrev mantPrev)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var cadencia = db.tblTareaMantenimientoPrev.FirstOrDefault(r => r.idTareaMantenimientoPrev == mantPrev.idTareaMantenimientoPrev)?.cadencia;
        var idLavanderia = db.tblMaquina.FirstOrDefault(r => r.idMaquina == mantPrev.idMaquina)?.idLavanderia;

        if (idLavanderia == null)
        {
            return BadRequest();
        }
        var currentServerDate = DateTimeOffset.UtcNow;
        var fechaLavanderia = Utils.getFechaLavanderia(db, idLavanderia.Value, currentServerDate);

        var tblRecambioNMovimientoRecambio = mantPrev.idMovimientoRecambioNavigation?.tblRecambioNMovimientoRecambio;

        var movimientoRecambio = new tblMovimientoRecambio
        {
            fecha = fechaLavanderia,
            idTipoMovimientoRecambio = 9,
            idAlmacenOrigen = mantPrev.idMovimientoRecambioNavigation?.idAlmacenOrigen,
            tblRecambioNMovimientoRecambio = tblRecambioNMovimientoRecambio
        };

        List<int> idsPersona = mantPrev.idPersona.Select(x => x.idPersona).ToList();
        var personas = db.tblPersona.Where(x => idsPersona.Contains(x.idPersona)).ToList();

        var objMantPrev = new tblMantenimientoPrev
        {
            idTareaMantenimientoPrev = mantPrev.idTareaMantenimientoPrev,
            idUsuario = idUsuario,
            cadencia = cadencia,
            idMaquina = mantPrev.idMaquina,
            fecha = fechaLavanderia,
            idPersona = personas,
            idMovimientoRecambioNavigation = movimientoRecambio
        };

        db.tblMantenimientoPrev.Add(objMantPrev);
        await db.SaveChangesAsync();
        return Created(objMantPrev);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblMantenimientoPrev> mantPrev)
    {
        var entity = db.tblMantenimientoPrev
            .Include(r => r.idPersona)
            .FirstOrDefault(r => r.idMantenimientoPrev == key);

        if (entity == null)
        {
            return NotFound();
        }

        var operation_idPersona = mantPrev.Operations.FirstOrDefault(x => x.path == "/idPersona");
        if (operation_idPersona != null)
        {
            if (entity != null)
                entity.idPersona.Clear();

            List<int> idsPersona = JsonConvert.DeserializeObject<List<tblPersona>>(operation_idPersona.value.ToString()).Select(x => x.idPersona).ToList();
            entity.idPersona = db.tblPersona.Where(x => idsPersona.Contains(x.idPersona)).ToList();
            mantPrev.Operations.Remove(operation_idPersona);
        }

        mantPrev.ApplyTo(entity);
        await db.SaveChangesAsync();
        return Updated(mantPrev);
    }

}
