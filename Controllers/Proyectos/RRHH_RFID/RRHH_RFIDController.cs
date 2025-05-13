using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.Context;
using WebApiCore.Hubs;

namespace WebApiCore.Controllers;
[AllowAnonymous]
public class RRHH_RFIDController : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public RRHH_RFIDController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet("odata/RRHH_RFID/tblLavanderia")]

    public ActionResult Get()
    {
        return Ok(db.tblLavanderia
       .Where(x => x.idPais.Equals(1))
        .Select(x => new
        {
            x.idLavanderia,
            x.denominacion
        }).OrderBy(x => x.denominacion));
    }

    [EnableQuery]
    [HttpGet("odata/RRHH_RFID/tblPersona")]

    public ActionResult Get([FromODataUri] int idLavanderia)
    {
        return Ok(db.tblPersona
       .Where(x => x.eliminado == false &&
       (x.idLavanderia == idLavanderia || (x.idCentroTrabajo.Equals(2) && x.tblUsuario.Count > 0 && x.tblUsuario.FirstOrDefault().idLavanderia.Where(l => l.idLavanderia.Equals(idLavanderia)).Count() > 0)))
        .Select(x => new
        {
            x.idPersona,
            x.nombre,
            x.apellidos,
            x.codigoRFID,
            foto = x.idFotoPerfilNavigation.documento
        }).OrderBy(x => x.nombre + x.apellidos));
    }

    [EnableQuery]
    [HttpPost("odata/RRHH_RFID/setCodigoRFID")]
    public async Task<ActionResult> setCodigoRFID([FromBody] EnvioDatos envioDatos)
    {
        tblPersona persMismoCodigo = db.tblPersona.Where(x => x.codigoRFID.Equals(envioDatos.codigoRFID)).FirstOrDefault();

        try
        {
            if (persMismoCodigo == null)
            {
                tblPersona persMod = db.tblPersona.Find(envioDatos.idPersona);
                persMod.codigoRFID_fecha = DateTime.Now;
                persMod.codigoRFID = envioDatos.codigoRFID;

                await db.SaveChangesAsync();
                return Ok();
            }
        }
        catch
        {

        }
        return BadRequest();

    }

    public class EnvioDatos
    {
        public int idPersona { get; set; }
        public DateTimeOffset fecha { get; set; }
        public string codigoRFID { get; set; }
    }
}
