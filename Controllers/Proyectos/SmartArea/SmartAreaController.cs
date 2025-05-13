using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using WebApiCore.Class;
using WebApiCore.Class.Proyectos.MyPolarier.RRHH;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
[BasicAuth]
public class SmartAreaController : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public SmartAreaController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet("odata/SmartArea/tblPersona")]
    public ActionResult tblPersona([FromODataUri] int idLavanderia)
    {
        int idPais = db.tblLavanderia.Where(x => x.idLavanderia == idLavanderia).Select(x => x.idPais).FirstOrDefault();

        return Ok(db.tblPersona.Where(x =>
            (x.idLavanderiaNavigation.idPais == idPais) &&
            x.activo == true &&
            x.eliminado == false)
            .Select(y => new
            {
                idPersona = y.idPersona,
                nombre = y.nombre,
                apellidos = y.apellidos,
                codigoRFID = y.codigoRFID,
                idFotoPerfil = y.idFotoPerfil,
                idCategoriaInterna = y.idCategoriaInterna
            }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartArea/tblDocumento")]
    public ActionResult tblDocumento([FromODataUri] int? idLavanderia, [FromODataUri] int? idPersona)
    {
        return Ok(db.tblPersona.Where(x =>
            ((idPersona == null && x.idLavanderia.Equals(idLavanderia)) ||
            (idPersona != null && x.idPersona.Equals(idPersona))) &&
            x.activo == true &&
            x.eliminado == false)
            .Select(y => new
            {
                idPersona = y.idPersona,
                idDocumento = y.idFotoPerfilNavigation != null ? y.idFotoPerfilNavigation.idDocumento : -1,
                documento = y.idFotoPerfilNavigation != null ? y.idFotoPerfilNavigation.documento : null
            }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartArea/tblAreaLavanderia")]
    public ActionResult tblAreaLavanderia([FromODataUri] int idLavanderia)
    {
        var areasLavanderia_filtradas = db.tblAreaLavanderia
                        .Where(x =>
                        x.idAreaLavanderia != 3 && //Procesado
                        x.tblAreaLavanderiaNLavanderia
                        .Any(y => y.idLavanderia == idLavanderia))
                        .Select(x => new
                        {
                            denominacion = x.idTraduccionNavigation.es != null ? x.idTraduccionNavigation.es : x.denominacion,
                            x.idAreaLavanderia,
                            tblPosicionNAreaLavanderiaNLavanderia = x.tblPosicionNAreaLavanderiaNLavanderia
                                                    .Where(posicion => posicion.idLavanderia == idLavanderia && posicion.idMaquina != null && posicion.activo == true)
                                                    .Select(posicion => new
                                                    {
                                                        posicion.idMaquina,
                                                        posicion.idMaquinaNavigation.etiqueta,
                                                        posicion.denominacion,
                                                        denoMaquina = posicion.idMaquinaNavigation.denominacion
                                                    })
                                                    .GroupBy(posicion => posicion.idMaquina)
                                                    .Select(grupo => new
                                                    {
                                                        idMaquina = grupo.Key,
                                                        etiqueta = grupo.FirstOrDefault().etiqueta,
                                                        denominacion = grupo.FirstOrDefault().denoMaquina
                                                    })
                        });

        return Ok(areasLavanderia_filtradas);
    }

    [EnableQuery]
    [HttpPost("odata/SmartArea/tblPersonaNAreaNLavanderia")]
    public async Task<ActionResult> tblPersonaNAreaNLavanderia([FromBody] PostPersonaNAreaNLavanderia personaNAreaNLavanderia)
    {
        #region Aplicar offset lavanderia a fecha actual

        var tblLavanderia = db.tblLavanderia
       .Where(x => x.idLavanderia.Equals(personaNAreaNLavanderia.idLavanderia))
       .Select(x => new
       {
           x.idLavanderia,
           x.horarioVerano,
           x.idZonaHorariaNavigation
       }).FirstOrDefault();

        int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);
        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
        #endregion

        bool isSalir = personaNAreaNLavanderia.idAreaLavanderia == null;
        var regActivos = db.tblPersonaNAreaNLavanderia.Where(x => x.idPersona.Equals(personaNAreaNLavanderia.idPersona) && x.fechaFin == null);
        if (regActivos.Count() > 0) //Finalizamos cualquier registro activo
        {
            foreach (tblPersonaNAreaNLavanderia pnanl in regActivos)
            {
                pnanl.fechaFin = offset;
            }
        }

        if (!isSalir)
        {
            db.tblPersonaNAreaNLavanderia.Add(new tblPersonaNAreaNLavanderia()
            {
                idAreaLavanderia = (byte)personaNAreaNLavanderia.idAreaLavanderia,
                idLavanderia = personaNAreaNLavanderia.idLavanderia,
                idPersona = personaNAreaNLavanderia.idPersona,
                fechaIni = offset
            });
        }

        //DESLOGUEO SMART HUB
        List<tblPersonaNMaquina> personaActiva = db.tblPersonaNMaquina.Where(x => x.idPersona.Equals(personaNAreaNLavanderia.idPersona) &&
                                                                                  x.fechaIni < offset &&
                                                                                  x.fechaFin == null).ToList();
        foreach (tblPersonaNMaquina pnm in personaActiva)
        {
            pnm.fechaFin = offset;
        }

        //Creamos un registro en tblPersonaNMaquina si se recibe una posición y máquina
        if (personaNAreaNLavanderia.idMaquina != null)
        {
            db.tblPersonaNMaquina.Add(new tblPersonaNMaquina()
            {
                idPersona = personaNAreaNLavanderia.idPersona,
                idMaquina = (int)personaNAreaNLavanderia.idMaquina,
                fechaIni = offset,
                numPos = personaNAreaNLavanderia.numPos
            });
        }

        await db.SaveChangesAsync();

        //Una vez guardada la modificación. Se finaliza clientes y se envia señal de refresco
        foreach (tblPersonaNMaquina pnm in personaActiva)
        {
            await Utils.FinalizaClienteNMaquinaAsync(db, offset, pnm.idMaquina, pnm.idPersona);
        }

        List<string> srcs = new List<string> { "PersonalActivo" };
        _hubContext.Clients.Group("SmartHUB_" + tblLavanderia.idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);

        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartArea/tblPersonaNAreaNLavanderia_offline")]
    public async Task<ActionResult> tblPersonaNAreaNLavanderia_offline([FromBody] List<PostPersonaNAreaNLavanderia> regPAL_offline)
    {
        try
        {
            var first_fechaRegOffline = regPAL_offline.OrderBy(x => x.fecha).FirstOrDefault().fecha;
            var idsPersonas = regPAL_offline.Select(x => x.idPersona).ToList();

            var eventosPersona = db.tblEventoPersona.Where(x => x.fecha >= first_fechaRegOffline && idsPersonas.Contains(x.idPersona)).ToList();
            var personaNAreaNLavanderia = db.tblPersonaNAreaNLavanderia.Where(x => idsPersonas.Contains(x.idPersona)).ToList();
            var personaNMaquina = db.tblPersonaNMaquina.Where(x => idsPersonas.Contains(x.idPersona)).ToList();
            var idLavNRegPAL = regPAL_offline.Select(x => x.idLavanderia).Distinct().ToList();

            var regs_PAL_insertados = new List<tblPersonaNAreaNLavanderia>();
            var regs_PNM_insertados = new List<tblPersonaNMaquina>();

            foreach (var regOffline in regPAL_offline)
            {
                var regs_PAL_activos_anteriores = personaNAreaNLavanderia
                                                        .Where(x => x.idPersona == regOffline.idPersona
                                                                    && x.fechaIni < regOffline.fecha
                                                                    && (x.fechaFin == null || x.fechaFin > regOffline.fecha)).ToList();
                var regs_PNM_activos_anteriores = personaNMaquina
                                                        .Where(x => x.idPersona == regOffline.idPersona
                                                                    && x.fechaIni < regOffline.fecha
                                                                    && (x.fechaFin == null || x.fechaFin > regOffline.fecha)).ToList();
                var regs_PAL_insertados_activos = regs_PAL_insertados
                                                        .Where(x => x.idPersona == regOffline.idPersona
                                                                    && x.fechaIni < regOffline.fecha
                                                                    && (x.fechaFin == null || x.fechaFin > regOffline.fecha)).ToList();
                var regs_PNM_insertados_activos = regs_PNM_insertados
                                                        .Where(x => x.idPersona == regOffline.idPersona
                                                                    && x.fechaIni < regOffline.fecha
                                                                    && (x.fechaFin == null || x.fechaFin > regOffline.fecha)).ToList();
                var isSalir = regOffline.idAreaLavanderia == null;
                // Si es un registro de salida se finalizan todos los registros activos

                if (isSalir)
                {

                    foreach (var reg in regs_PAL_activos_anteriores)
                    {
                        reg.fechaFin = regOffline.fecha;
                    }

                    foreach (var reg in regs_PAL_insertados_activos)
                    {
                        reg.fechaFin = regOffline.fecha;
                    }

                    foreach (var reg in regs_PNM_activos_anteriores)
                    {
                        reg.fechaFin = regOffline.fecha;
                    }

                    foreach (var reg in regs_PNM_insertados_activos)
                    {
                        reg.fechaFin = regOffline.fecha;
                    }

                    continue;
                }

                var regPAL_anterior = personaNAreaNLavanderia.Where(x => x.idPersona == regOffline.idPersona && x.fechaIni < regOffline.fecha).OrderByDescending(x => x.fechaIni).FirstOrDefault();
                var regPNM_anterior = personaNMaquina.Where(x => x.idPersona == regOffline.idPersona && x.fechaIni < regOffline.fecha).OrderByDescending(x => x.fechaIni).FirstOrDefault();
                var regEP_posterior = eventosPersona.OrderBy(x => x.fecha).FirstOrDefault(x => x.idPersona == regOffline.idPersona && x.fecha >= regOffline.fecha);
                var regPAL_posterior = personaNAreaNLavanderia.OrderBy(x => x.fechaIni).FirstOrDefault(x => x.idPersona == regOffline.idPersona && x.fechaIni >= regOffline.fecha);

                // Selecciona la que no sea null y la que sea menor
                var fechaPosterior = regEP_posterior != null && regPAL_posterior != null
                                        ? (regEP_posterior.fecha < regPAL_posterior.fechaIni
                                            ? regEP_posterior.fecha
                                            : regPAL_posterior.fechaIni)
                                        : (regEP_posterior != null
                                            ? regEP_posterior.fecha
                                            : regPAL_posterior?.fechaIni);

                //Insert  PAL
                regs_PAL_insertados.Add(new tblPersonaNAreaNLavanderia()
                {
                    idAreaLavanderia = (byte)regOffline?.idAreaLavanderia,
                    idLavanderia = regOffline.idLavanderia,
                    idPersona = regOffline.idPersona,
                    fechaIni = (DateTimeOffset)regOffline?.fecha,
                    fechaFin = fechaPosterior,
                    isOffline = true
                });

                //Insert PNM
                if (regOffline.idMaquina != null)
                {
                    regs_PNM_insertados.Add(new tblPersonaNMaquina()
                    {
                        idPersona = regOffline.idPersona,
                        idMaquina = (int)regOffline.idMaquina,
                        fechaIni = (DateTimeOffset)regOffline.fecha,
                        numPos = regOffline.numPos,
                        fechaFin = fechaPosterior,
                        isOffline = true
                    });
                }

                var regOffline_activo_pal = regs_PAL_insertados
                                            .OrderByDescending(x => x.fechaIni)
                                            .FirstOrDefault(x => x.idPersona == regOffline.idPersona && x.fechaFin == null && x.fechaIni < regOffline.fecha);

                if (regOffline_activo_pal != null)
                {
                    regOffline_activo_pal.fechaFin = regOffline.fecha;
                }

                // Actualiza el registro anterior de PAL
                if ((regPAL_anterior.fechaFin > regOffline.fecha || regPAL_anterior.fechaFin == null) && regPAL_anterior.fechaIni < regOffline.fecha)
                {
                    regPAL_anterior.fechaFin = regOffline.fecha;
                }

                var regOffline_activo_pnm = regs_PNM_insertados
                                            .OrderByDescending(x => x.fechaIni)
                                            .FirstOrDefault(x => x.idPersona == regOffline.idPersona && x.fechaFin == null && x.fechaIni < regOffline.fecha);

                if (regOffline_activo_pnm != null)
                {
                    regOffline_activo_pnm.fechaFin = regOffline.fecha;
                }

                // Actualiza el registro anterior de PNM
                if ((regPNM_anterior.fechaFin > regOffline.fecha || regPNM_anterior.fechaFin == null) && regPNM_anterior.fechaIni < regOffline.fecha)
                {
                    regPNM_anterior.fechaFin = regOffline.fecha;
                }

                //Inserto nuevos registros  
                db.tblPersonaNAreaNLavanderia.AddRange(regs_PAL_insertados);
                db.tblPersonaNMaquina.AddRange(regs_PNM_insertados);

            }
            await db.SaveChangesAsync();

            // Una vez guardados los datos, se comprueba si debe finalizar el cliente donde estaba la persona.
            foreach (var item in regPAL_offline)
            {
                var idPersona = item.idPersona;
                var idMaquina = item.idMaquina;
                var fecha = item.fecha;

                if (idMaquina == null)
                {
                    var regPNM_anterior = db.tblPersonaNMaquina.Where(x => x.idPersona == idPersona && x.fechaFin < item.fecha)
                                                                .OrderByDescending(x => x.fechaFin)
                                                                .FirstOrDefault();
                    if (regPNM_anterior.idMaquina != null)
                        idMaquina = regPNM_anterior?.idMaquina;
                }

                // Si la persona y la máquina no son nulas y la fecha es válida, finaliza el cliente en la máquina.
                if (idPersona != null && idMaquina != null && fecha != null)
                    await Utils.FinalizaClienteNMaquinaAsync(db, (DateTimeOffset)fecha, (int)idMaquina, idPersona);
            }

            var idsLavanderias = regPAL_offline.Select(x => x.idLavanderia).Distinct().ToList();

            foreach (var idLav in idsLavanderias)
            {

                List<string> srcs = new List<string> { "PersonalActivo" };
                _hubContext.Clients.Group("SmartHUB_" + idLav).SendAsync("SmartView/signalR_refresh", srcs);

            }

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}