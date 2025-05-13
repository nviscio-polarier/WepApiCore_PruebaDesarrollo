using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblConceptoNominaNNominaController : ODataController
{
    private readonly bdERP db;
    private readonly tblNominaController nc;

    public tblConceptoNominaNNominaController(bdERP context, IHubContext<NotificacionesHub> _hubContext)
    {
        db = context;
        nc = new(context, _hubContext);
    }

    [EnableQuery(MaxExpansionDepth = 3)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] bool filtroTipoTrabajoRRHH = true)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var idsPersona = Utils.selectPersonasVisibles(db, idUsuario, filtroTipoTrabajoRRHH);

        return Ok(db.tblConceptoNominaNNomina.Where(x => idsPersona.Contains(x.idNominaNavigation.idPersona)));

    }

    [EnableQuery]
    [HttpPost("odata/tblConceptoNominaNNomina/IU_tblConceptoNominaNNomina")]
    [Authorize]
    public async Task<ActionResult> IU_tblConceptoNominaNNomina([FromBody] CustomConceptoNominaNNomina conceptoNominaNNomina)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        if (!IU_tblConceptoNNomina(idUsuario, conceptoNominaNNomina))
            return BadRequest("La persona esta mal parametrizada.");

        await db.SaveChangesAsync();

        return Ok(true);
    }

    [EnableQuery]
    [HttpPost("odata/tblConceptoNominaNNomina/IU_masivo_tblConceptoNominaNNomina")]
    [Authorize]
    public async Task<ActionResult> IU_masivo_tblConceptoNominaNNomina([FromBody] List<CustomConceptoNominaNNomina> conceptoNominaNNomina)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        foreach (var conceptoGroup in conceptoNominaNNomina.GroupBy(x => x.idConceptoNomina))
        {
            foreach (var concepto in conceptoGroup)
            {
                if (!IU_tblConceptoNNomina(idUsuario, concepto))
                    return BadRequest("La persona esta mal parametrizada.");
            }
            await db.SaveChangesAsync();
        }

        return Ok(true);
    }


    [EnableQuery]
    [HttpDelete("odata/tblConceptoNominaNNomina/{idNomina}/{idConceptoNomina}")]
    [Authorize]
    public async Task<bool> Delete(int idNomina, short idConceptoNomina)
    {
        var entity = await db.tblConceptoNominaNNomina.FindAsync(idNomina, idConceptoNomina);
        if (entity == null)
            return false;

        db.tblConceptoNominaNNomina.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }

    public bool IU_tblConceptoNNomina(int idUsuario, CustomConceptoNominaNNomina conceptoNominaNNomina, int? idNomina = null)
    {
        var objNomina = db.tblNomina
            .Include(n => n.tblConceptoNominaNNomina)
            .FirstOrDefault(x =>
                idNomina == null && conceptoNominaNNomina.idNomina == null
                ? (
                    x.idPersona.Equals(conceptoNominaNNomina.idPersona) &&
                    x.fechaDesde.Date.Equals(conceptoNominaNNomina.fechaInicioNomina.Date) &&
                    x.fechaHasta.Date.Equals(conceptoNominaNNomina.fechaFinNomina.Date) &&
                    x.idTipoNomina == (short)idsTipoNomina.PagaMesual
                )
                : x.idNomina.Equals(idNomina ?? conceptoNominaNNomina.idNomina)
            );

        tblConceptoNominaNNomina objConcepto = new()
        {
            idConceptoNomina = conceptoNominaNNomina.idConceptoNomina,
            cantidad = conceptoNominaNNomina.cantidad,
            precioUnitario = conceptoNominaNNomina.precioUnitario,
            observaciones = conceptoNominaNNomina.observaciones,
            idUsuario_validacion = idUsuario,
            fecha_validacion = DateTimeOffset.UtcNow,
            fecha = conceptoNominaNNomina.fecha ?? conceptoNominaNNomina.fechaInicioNomina.Date,
            importe = conceptoNominaNNomina.importe
        };

        if (objNomina == null) //INSERT
        {
            objNomina = nc.GetNewNomina(conceptoNominaNNomina.idPersona, conceptoNominaNNomina.fechaInicioNomina, conceptoNominaNNomina.fechaFinNomina, idUsuario);

            if (objNomina == null)
            {
                return false;
            }

            objNomina.tblConceptoNominaNNomina.Add(objConcepto);

            db.tblNomina.Add(objNomina);
        }
        else //UPDATE
        {
            var conceptoExistente = objNomina.tblConceptoNominaNNomina.FirstOrDefault(cnnn => cnnn.idConceptoNomina == conceptoNominaNNomina.idConceptoNomina);
            if (conceptoExistente != null)
            {
                db.tblConceptoNominaNNomina.Remove(conceptoExistente);
            }
            objConcepto.idNomina = objNomina.idNomina;
        }


        db.tblConceptoNominaNNomina.Add(objConcepto);

        return true;
    }

    public class CustomConceptoNominaNNomina
    {
        public int? idNomina { get; set; }
        public short idConceptoNomina { get; set; }
        public decimal? cantidad { get; set; }
        public decimal? precioUnitario { get; set; }
        public string? observaciones { get; set; }
        public DateTime? fecha { get; set; }
        public int idPersona { get; set; }
        public DateTime fechaInicioNomina { get; set; }
        public DateTime fechaFinNomina { get; set; }
        public decimal? importe { get; set; }
    }
}
