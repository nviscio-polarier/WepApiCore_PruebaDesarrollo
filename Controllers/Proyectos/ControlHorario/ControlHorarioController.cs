using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Hubs;

namespace WebApiCore.Controllers;
[AllowAnonymous]
public class ControlHorarioController : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public ControlHorarioController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet("odata/ControlHorario/v3/tblPersona")]
    public ActionResult getTblPersonaV3([FromODataUri] int idLavanderia)
    {
        int idPais = db.tblLavanderia
            .Where(x => x.idLavanderia == idLavanderia)
            .Select(x => x.idPais)
            .FirstOrDefault();

        List<tblEventoPersona> tblEventoPersonas_inserts = new();
        var horarioLavanderia = db.tblLavanderia.Select(x => new
        { x.idZonaHorariaNavigation, x.horarioVerano, x.idLavanderia }).FirstOrDefault(x => x.idLavanderia == idLavanderia);
        int GMT = (horarioLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(horarioLavanderia.idZonaHorariaNavigation.GMT);

        var fechaActual = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        var fechaLavActual = Class.Utils.aplicarGMT(DateTimeOffset.Now, GMT);
        var tsActual = new TimeSpan(fechaLavActual.Hour, fechaLavActual.Minute, 0);

        List<int?> categoriaEncargado = new List<int?>() { 6, 7, 12, 77 };


        var result = db.tblPersona
            .Where(x => (
                x.idLavanderiaNavigation.idPais == idPais || // Misma lavanderia del pais
                x.tblUsuario.FirstOrDefault().idLavanderia.Where(l => l.idPais.Equals(idPais)).Count() > 0  // Usuarios lavandería país
            )
            && x.activo == true
            && x.eliminado == false
            && (x.idTurno != null
            ))
            .Select(x => new
            {
                x.idPersona,
                x.nombre,
                x.apellidos,
                x.codigoRFID,
                x.numDocumentoIdentidad,
                x.idLavanderia,
                x.idFotoPerfil,
                isEncargado = x.idCategoria == 12 || categoriaEncargado.Contains(x.idCategoriaInterna),
                docPendienteFirma = x.tblDocumento.Where(x => x.firmado == false).Any(),
                tblCuadrantePersonal = x.tblCuadrantePersonal
                    .Select(x => new
                    {
                        x.horaEntrada,
                        x.horaSalida,
                        tiempoDescanso = x.idTurnoNavigation.descanso,
                        x.fecha
                    })
                    .FirstOrDefault(cp => cp.fecha.Date == fechaActual && tsActual < cp.horaSalida) // Cuadrante misma fecha que no haya terminado. 
            });

        return Ok(result);
    }


    [EnableQuery]
    [HttpGet("odata/ControlHorario/GetFotos")]
    public ActionResult GetFotos([FromODataUri] int idLavanderia, [FromODataUri] DateTime fecha)
    {
        int idPais = db.tblLavanderia.Where(x => x.idLavanderia == idLavanderia).Select(x => x.idPais).FirstOrDefault();

        DateTime fecha_ = fecha.ToUniversalTime();
        DateTime fechaNocturna = fecha_.Hour < 12 ? fecha_.AddDays(-1) : fecha_;
        return Ok(db.tblPersona
       .Where(x => (x.idLavanderiaNavigation.idPais == idPais || x.tblUsuario.FirstOrDefault().idLavanderia.Where(l => l.idPais.Equals(idPais)).Count() > 0) &&
                   x.activo == true && x.eliminado == false
        ).Select(x => new
        {
            x.idPersona,
            foto = x.idFotoPerfilNavigation.documento
        }));
    }


    [EnableQuery]
    [HttpGet("odata/ControlHorario/getLastEventoPersona")]
    public async Task<IActionResult> GetLastEventoPersona([FromODataUri] int idPersona)
    {
        var persona = await db.tblPersona.Where(x => x.idPersona == idPersona).Select(x => new { x.idLavanderia }).FirstOrDefaultAsync();

        if (persona == null)
        {
            return NotFound($"No se encontró una persona con id {idPersona}");
        }

        var horarioLavanderia = await db.tblLavanderia
            .Where(x => x.idLavanderia == persona.idLavanderia)
            .Select(x => new
            {
                GMT = x.idZonaHorariaNavigation.GMT,
                x.horarioVerano
            })
            .FirstOrDefaultAsync();

        if (horarioLavanderia == null)
        {
            return NotFound($"No se encontró una lavandería para la persona con id {idPersona}");
        }

        int gmt = ((bool)horarioLavanderia.horarioVerano ? 1 : 0) + Convert.ToInt32(horarioLavanderia.GMT);
        var horaActual = Class.Utils.aplicarGMT(DateTimeOffset.UtcNow, gmt);
        var horaMargenTiempo = horaActual.AddHours(-10);

        // Se obtiene el último evento de la persona en las últimas 10 horas
        var evento = db.tblEventoPersona
                                .Where(x => x.idPersona == idPersona && x.fecha >= horaMargenTiempo)
                                .Select(x => new { x.fecha, x.idEventoPersona_Estado, x.idPersona })
                                .OrderByDescending(x => x.fecha)
                                .FirstOrDefault();

        return Ok(evento);
    }

    [EnableQuery]
    [HttpGet("odata/ControlHorario/getLastEventoPersona_list")]
    public async Task<IActionResult> getLastEventoPersona_list([FromODataUri] int idLavanderia)
    {
        int TIEMPO_MARGEN = 10;
        var idPais = db.tblLavanderia.Where(x => x.idLavanderia == idLavanderia).Select(x => x.idPais).FirstOrDefault();
        var personasPais = db.tblPersona.Where(x => x.idPais == idPais || x.idLavanderia == idLavanderia
                                                && x.activo == true && x.eliminado == false)
                                        .Select(x => x.idPersona).ToList();

        var horarioLavanderia = await db.tblLavanderia
            .Where(x => x.idLavanderia == idLavanderia).Select(x => new { x.idZonaHorariaNavigation.GMT, x.horarioVerano }).FirstOrDefaultAsync();


        int gmt = ((bool)horarioLavanderia.horarioVerano ? 1 : 0) + Convert.ToInt32(horarioLavanderia.GMT);
        var horaActual = Class.Utils.aplicarGMT(DateTimeOffset.UtcNow, gmt);
        var horaMargenTiempo = horaActual.AddHours(-TIEMPO_MARGEN);


        var eventosPersonaFiltrados = db.tblEventoPersona.Where(x => personasPais.Contains(x.idPersona) && x.fecha >= horaMargenTiempo).ToList();

        var lastEventoPersonas_list = personasPais
                                      .Select(item => eventosPersonaFiltrados
                                          .Where(x => x.idPersona == item && x.fecha >= horaMargenTiempo)
                                          .OrderByDescending(x => x.fecha)
                                          .Select(x => new
                                          {
                                              fecha = x.fecha,
                                              idEventoPersona_Estado = x.idEventoPersona_Estado,
                                              idPersona = x.idPersona
                                          })
                                          .FirstOrDefault())
                                      .Where(result => result != null)
                                      .ToList();

        return Ok(lastEventoPersonas_list);
    }


    #region Entrada / Salida personal

    #endregion

    public class Registro
    {
        public int idPersona { get; set; }
        public DateTimeOffset fecha { get; set; }
        public string columna { get; set; }
        public bool isRegManual { get; set; }
        public bool isOffline { get; set; }
    }
}
