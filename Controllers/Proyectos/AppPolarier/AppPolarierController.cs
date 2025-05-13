using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.RRHH;
using AuthorizeAttribute = WebApiCore.Security.AuthorizeAttribute;

namespace WebApiCore.Controllers;
public class AppPolarierController : ODataController
{
    private readonly bdERP db;
    private CalendarioController cc;

    public AppPolarierController(bdERP context)
    {
        db = context;
        cc = new(context);
    }

    [EnableQuery]
    [HttpGet("odata/AppPolarier/personas")]
    [Authorize]
    public ActionResult Get()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        int? idPersona = db.tblUsuario
       .Where(x => x.idUsuario.Equals(idUsuario) && !x.isEliminado)
       .Select(x => x.idPersona)
       .FirstOrDefault();

        if (idPersona == null)
            return BadRequest();

        var result = db.tblPersona.Where(x => x.idPersona == idPersona).Include(x => x.idLicenciaConducir);

        return Ok(result);
    }

    [EnableQuery]
    [HttpPost("odata/AppPolarier/actualizarNotificationToken")]
    [Authorize]
    public ActionResult actualizarNotificationToken([FromODataUri] string notificationToken)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        db.tblLogError.Add(new tblLogError() { denominacion = "Cambio notificationToken idUsuario: " + idUsuario });
        db.tblUsuario.Find(idUsuario).notificationToken = notificationToken;
        db.SaveChanges();

        return Ok();
    }

    [EnableQuery]
    [HttpGet("odata/AppPolarier/tblCalendarioPersonal")]
    [Authorize]
    public async Task<ActionResult> GetTblCalendarioPersonal()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var hoy = DateTime.Today;

        var usuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario == idUsuario && !x.isEliminado);
        if (usuario == null)
            return BadRequest();

        var persona = db.tblPersona.FirstOrDefault(x => x.idPersona == usuario.idPersona);
        if (persona == null)
            return BadRequest();

        var fechaDesde = new DateTime(hoy.Year - 1, 1, 1);
        var fechaHasta = new DateTime(hoy.Year + 1, 12, 31);

        var calendarioPersonal = await cc.Handle_GetCalendarioPersonal(fechaDesde, fechaHasta, new List<int>() { persona.idPersona });

        if (calendarioPersonal == null)
        {
            return BadRequest();
        }

        var idsTurno = calendarioPersonal
            .SelectMany(cp => cp.eventos.Select(e => e.idTurno))
            .Where(idTurno => idTurno != null)
            .Distinct()
            .ToList();

        var tblTurno = db.tblTurno.Where(t => idsTurno.Contains(t.idTurno)).ToList();

        var result = calendarioPersonal
            .Where(cp => cp.idCalendario_Estado != cc.idCalendario_EstadoSinEvento)
            .Select(cp =>
            {
                var evento = cp.eventos.FirstOrDefault();
                var turno = tblTurno.FirstOrDefault(t => evento != null && t.idTurno == evento.idTurno)?.denominacion;

                return new
                {
                    cp.fecha,
                    cp.idCalendario_Estado,
                    horaEntrada = evento?.horaEntrada != null ? evento.horaEntrada?.ToString(@"hh\:mm") : "",
                    horaSalida = evento?.horaSalida != null ? evento.horaSalida?.ToString(@"hh\:mm") : "",
                    turno = turno ?? "",
                    isCuadrante = cp.fecha > hoy
                };
            });

        return Ok(result);
    }

    [EnableQuery]
    [HttpDelete("odata/AppPolarier/deleteDocumento")]
    [Authorize]
    public async Task<ActionResult> deleteDocumento([FromODataUri] int idDocumento)
    {
        var entity = db.tblDocumento.FirstOrDefault(x => x.idDocumento.Equals(idDocumento));

        if (entity == null)
            return BadRequest();

        try
        {
            var relation_peticionCambios = db.tblPersona_PeticionCambioDatos.Where(
                    x => x.idFotoDocumentoIdentidad_A.Equals(idDocumento) ||
                    x.idFotoDocumentoIdentidad_B.Equals(idDocumento) ||
                    x.idFotoNAF.Equals(idDocumento) ||
                    x.idFotoIBAN.Equals(idDocumento) ||
                    x.idDocumentoLicenciaConducir.Equals(idDocumento)
                );

            var hasRelation_persona = db.tblPersona.Where(
                x => x.idFotoDocumentoIdentidad_A.Equals(idDocumento) ||
                x.idFotoDocumentoIdentidad_B.Equals(idDocumento) ||
                x.idFotoNAF.Equals(idDocumento) ||
                x.idFotoIBAN.Equals(idDocumento) ||
                x.idDocumentoLicenciaConducir.Equals(idDocumento)
            );

            //Elimina relaciones 

            foreach (var item in relation_peticionCambios)
            {
                if (item.idFotoDocumentoIdentidad_A.Equals(idDocumento))
                    item.idFotoDocumentoIdentidad_A = null;
                if (item.idFotoDocumentoIdentidad_B.Equals(idDocumento))
                    item.idFotoDocumentoIdentidad_B = null;
                if (item.idFotoNAF.Equals(idDocumento))
                    item.idFotoNAF = null;
                if (item.idFotoIBAN.Equals(idDocumento))
                    item.idFotoIBAN = null;
                if (item.idDocumentoLicenciaConducir.Equals(idDocumento))
                    item.idDocumentoLicenciaConducir = null;
            }

            foreach (var item in hasRelation_persona)
            {
                if (item.idFotoDocumentoIdentidad_A.Equals(idDocumento))
                    item.idFotoDocumentoIdentidad_A = null;
                if (item.idFotoDocumentoIdentidad_B.Equals(idDocumento))
                    item.idFotoDocumentoIdentidad_B = null;
                if (item.idFotoNAF.Equals(idDocumento))
                    item.idFotoNAF = null;
                if (item.idFotoIBAN.Equals(idDocumento))
                    item.idFotoIBAN = null;
                if (item.idDocumentoLicenciaConducir.Equals(idDocumento))
                    item.idDocumentoLicenciaConducir = null;
            }

            db.Remove(entity);
            await db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception error)
        {
            return BadRequest(error);
        }

    }

    public string formatTimeSpan(string timeSpan)
    {
        if (timeSpan == null || timeSpan.Length == 0)
        {
            return null;
        }
        else
        {
            TimeSpan ts = TimeSpan.Parse(timeSpan);
            var hours = ts.Hours < 10 ? ("0" + ts.Hours) : ts.Hours.ToString();
            var minutes = ts.Minutes < 10 ? ("0" + ts.Minutes) : ts.Minutes.ToString();
            return hours + ":" + minutes;
        }
    }

    [EnableQuery]
    [HttpGet("odata/AppPolarier/getCalendario_Estado")]
    [Authorize]
    public async Task<ActionResult> getCalendario_Estado()
    {
        return Ok(db.tblCalendario_Estado.Where(ce => !ce.isLavanderia));
    }

}
