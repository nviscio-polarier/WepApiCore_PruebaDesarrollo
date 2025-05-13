using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
[BasicAuth]
public class SmartHUBController_v2 : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public SmartHUBController_v2(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [HttpGet("odata/SmartHUB_v2/spSelectNodosLavanderia")]
    public async Task<ActionResult> spSelectNodosLavanderia([FromODataUri] int idLavanderia)
    {
        var connection = db.Database.GetDbConnection();
        var results = await connection.QueryAsync("EXEC [MyRealData].[EF_spSelectNodosLavanderia] @idLavanderia", new { idLavanderia = idLavanderia });
        return Ok(results.ToList());
    }

    [HttpGet("odata/SmartHUB_v2/spSelectReportSmartHub")]
    public async Task<ActionResult> spSelectReportSmartHub([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] string group, [FromODataUri] int? idMaquina, [FromODataUri] int? idCompañia, [FromODataUri] int? idEntidad, [FromODataUri] int? idFamilia, [FromODataUri] int? idTipoPrenda)
    {
        var connection = db.Database.GetDbConnection();
        var results = await connection.QueryAsync("EXEC [MyRealData].[EF_spSelectReportSmartHub] @idLavanderia, @fechaIni, @fechaFin, @group, @idMaquina, @idCompañia, @idEntidad, @idFamilia, @idTipoPrenda",
            new
            {
                idLavanderia = idLavanderia,
                fechaIni = fechaIni,
                fechaFin = fechaFin,
                group = group,
                idMaquina = idMaquina,
                idCompañia = idCompañia,
                idEntidad = idEntidad,
                idFamilia = idFamilia,
                idTipoPrenda = idTipoPrenda
            });
        return Ok(results.ToList());
    }

    [HttpGet("odata/SmartHUB_v2/spSelectPersonaNAreaSmartHub")]
    public async Task<ActionResult> spSelectPersonaNAreaSmartHub([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] string group, [FromODataUri] int? idPersona)
    {
        var connection = db.Database.GetDbConnection();
        var results = await connection.QueryAsync("EXEC [MyRealData].[EF_spSelectPersonaNAreaSmartHub] @idLavanderia, @fechaIni, @fechaFin, @group, @idPersona",
            new
            {
                idLavanderia = idLavanderia,
                fechaIni = fechaIni,
                fechaFin = fechaFin,
                group = group,
                idPersona = idPersona
            });
        return Ok(results.ToList());
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/tblPersona")]
    public ActionResult tblPersona([FromODataUri] int idMaquina)
    {
        int idPais = db.tblMaquina.Where(x => x.idMaquina == idMaquina).Select(x => x.idLavanderiaNavigation.idPais).FirstOrDefault();

        List<int?> categoriaEncargado = new List<int?>() { 6, 7, 12, 77 };

        return Ok(db.tblPersona.Where(x =>
            (x.idLavanderiaNavigation.idPais == idPais) &&
            x.activo == true &&
            x.eliminado == false)
            .Select(y => new
            {
                y.idPersona,
                y.nombre,
                y.apellidos,
                y.codigoRFID,
                y.idCategoria,
                y.idCategoriaInterna,
                y.numDocumentoIdentidad,
                isEncargado = y.idCategoria == 12 || categoriaEncargado.Contains(y.idCategoriaInterna)
            }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/tblMaquina")]
    public async Task<ActionResult> tblMaquina([FromODataUri] int idMaquina)
    {
        return Ok(db.tblMaquina.Where(x => x.idMaquina.Equals(idMaquina)).Select(x => new
        {
            denominacion = x.denominacion,
            codigoTipoMaquina = x.idTipoMaquinaNCategoriaMaquinaNavigation.idTipoMaquinaNavigation.codigo
        }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/tblEntidad")]
    public async Task<ActionResult> tblEntidad([FromODataUri] int idMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == idMaquina).FirstOrDefault().idLavanderia;

        return Ok(db.tblEntidad.Where(x =>
            x.idLavanderia.Where(l => l.idLavanderia == idLavanderia).Count() > 0 &&
            x.activo == true &&
            x.eliminado == false)
            .Select(y => new
            {
                idEntidad = y.idEntidad,
                idCompañia = y.idCompañia,
                denominacion = y.denominacion
            }).OrderBy(x => x.denominacion));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/tblCompañia")]
    public async Task<ActionResult> tblCompañia([FromODataUri] int idMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == idMaquina).FirstOrDefault().idLavanderia;

        return Ok(db.tblCompañia.Where(x =>
              x.tblEntidad.Where(y => y.idLavanderia.Where(l => l.idLavanderia == idLavanderia).Count() > 0).Count() > 0 &&
              x.activo == true &&
              x.eliminado == false)
              .Select(y => new
              {
                  idCompañia = y.idCompañia,
                  denominacion = y.denominacion
              })
              .OrderBy(x => x.denominacion));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/tblTipoPrenda")]
    public async Task<ActionResult> tblTipoPrenda(int idMaquina)
    {
        return Ok(db.tblTipoPrenda.Where(x => x.tblPrendasHora.Where(y => y.idMaquina.Equals(idMaquina)).Count() > 0).Select(x => new { x.idTipoPrenda, x.idFamilia, x.denominacion }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/tblPersonaNMaquina")]
    public async Task<ActionResult> tblPersonaNMaquina(int idMaquina)
    {
        return Ok(db.tblPersonaNMaquina.Where(x => x.idMaquina.Equals(idMaquina) && x.fechaFin == null && x.numPos <= 5).Select(x => new { x.idPersona, x.numPos, x.idPersonaNMaquina }));
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/tblClienteNMaquina_Inicio")]
    public async Task<ActionResult> tblClienteNMaquina_Inicio([FromBody] iuClienteNMaquina clienteNMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == clienteNMaquina.idMaquina).FirstOrDefault().idLavanderia;
        int gmt = await ObtenerGMTLav(idLavanderia);
        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));

        //En caso de que haya otro cliente activo por la misma máquina se cierra
        List<tblClienteNMaquina> clientesActivos = db.tblClienteNMaquina.Where(x => x.idMaquina.Equals(clienteNMaquina.idMaquina) && x.fechaFin == null).ToList();
        foreach (tblClienteNMaquina cli in clientesActivos)
        {
            cli.fechaFin = offset;
        }

        db.tblClienteNMaquina.Add(new tblClienteNMaquina()
        {
            idCompañia = clienteNMaquina.idCompañia,
            idEntidad = clienteNMaquina.idEntidad,
            idMaquina = clienteNMaquina.idMaquina,
            idFamilia = clienteNMaquina.idFamilia,
            idTipoPrenda = clienteNMaquina.idTipoPrenda,
            fechaIni = offset
        });


        await db.SaveChangesAsync();


        List<string> srcs = new List<string> { "tblClienteNMaquina" };
        await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);

        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/tblClienteNMaquina_update_Fin")]
    public async Task<ActionResult> tblClienteNMaquina_update_Fin([FromODataUri] int idClienteNMaquina)
    {
        var clienteNMaquina = db.tblClienteNMaquina.Where(x => x.idClienteNMaquina == idClienteNMaquina).FirstOrDefault();
        int idLavanderia = db.tblMaquina.FirstOrDefault(x => x.idMaquina == clienteNMaquina.idMaquina).idLavanderia;
        int gmt = await ObtenerGMTLav(idLavanderia);
        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));

        if (clienteNMaquina != null)
        {
            clienteNMaquina.fechaFin = offset;
            await db.SaveChangesAsync();
            List<string> srcs = new List<string> { "tblClienteNMaquina" };
            await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
        }

        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/tblClienteNMaquina_Fin")]
    public async Task<ActionResult> tblClienteNMaquina_Fin([FromBody] iuClienteNMaquina clienteNMaquina)
    {
        int idLavanderia = db.tblMaquina.FirstOrDefault(x => x.idMaquina == clienteNMaquina.idMaquina).idLavanderia;
        int gmt = await ObtenerGMTLav(idLavanderia);
        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));

        tblClienteNMaquina objClienteNMaquina = db.tblClienteNMaquina.Where(x =>
                                                                        x.idCompañia == clienteNMaquina.idCompañia &&
                                                                        x.idEntidad == clienteNMaquina.idEntidad &&
                                                                        x.idMaquina == clienteNMaquina.idMaquina &&
                                                                        x.idFamilia == clienteNMaquina.idFamilia &&
                                                                        x.idTipoPrenda == clienteNMaquina.idTipoPrenda &&
                                                                        x.fechaFin == null)
                                                                    .OrderByDescending(s => s.fechaIni)
                                                                    .FirstOrDefault();

        if (objClienteNMaquina != null)
        {
            objClienteNMaquina.fechaFin = offset;
            await db.SaveChangesAsync();
            List<string> srcs = new List<string> { "tblClienteNMaquina" };
            await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
        }

        var idClienteNMaquina = objClienteNMaquina != null ? objClienteNMaquina?.idClienteNMaquina : null;

        return Ok(idClienteNMaquina);
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/insert_tblClienteNMaquina_offline")]
    public async Task<ActionResult> insert_tblClienteNMaquina_offline([FromBody] List<iuClienteNMaquina> clienteNMaquinas)
    {
        var tblClienteNMaquina = db.tblClienteNMaquina.ToList();
        int idMaquina = clienteNMaquinas.FirstOrDefault(x => x.idMaquina != null).idMaquina;
        int idLavanderia = db.tblMaquina.FirstOrDefault(x => x.idMaquina == idMaquina).idLavanderia;
        int gmt = await ObtenerGMTLav(idLavanderia);
        DateTimeOffset? offset = null;

        //Eliminar registro de clienteNMaquina que ya se encuentren en tblClienteNmaquina

        var clienteNMaquinas_filtered = clienteNMaquinas.Where(x => !tblClienteNMaquina.Any(y =>
                                                                        y.idMaquina == x.idMaquina &&
                                                                        y.idCompañia == x.idCompañia &&
                                                                        y.idEntidad == x.idEntidad &&
                                                                        y.idFamilia == x.idFamilia &&
                                                                        y.idTipoPrenda == x.idTipoPrenda &&
                                                                        y.fechaIni == x.fechaIni &&
                                                                        y.fechaFin == x.fechaFin)).ToList();

        if (clienteNMaquinas.Count != clienteNMaquinas_filtered.Count)
        {
            db.tblLogError.Add(new tblLogError
            {
                denominacion = "insert_tblClienteNMaquina_offline_" + idLavanderia,
                error = JsonSerializer.Serialize(clienteNMaquinas)
            });
        }

        foreach (var cliente in clienteNMaquinas_filtered)
        {

            if (cliente.idClienteNMaquina != null)
            {
                offset = cliente.fechaFin?.ToOffset(TimeSpan.FromHours(gmt));
                var clienteFound = tblClienteNMaquina.FirstOrDefault(x => x.idClienteNMaquina == cliente.idClienteNMaquina);

                if (clienteFound != null)
                {
                    clienteFound.fechaFin = offset;
                    clienteFound.isOffline = true;
                }

            }
            else
            {
                var inicioOffset = cliente.fechaIni?.ToOffset(TimeSpan.FromHours(gmt));
                var finOffset = cliente.fechaFin?.ToOffset(TimeSpan.FromHours(gmt));

                var cNMaquina = new tblClienteNMaquina
                {
                    fechaIni = (DateTimeOffset)inicioOffset,
                    fechaFin = finOffset,
                    idCompañia = cliente.idCompañia,
                    isOffline = true,
                    idEntidad = cliente.idEntidad,
                    idFamilia = cliente.idFamilia,
                    idMaquina = cliente.idMaquina,
                    idTipoPrenda = cliente.idTipoPrenda,
                };

                db.tblClienteNMaquina.Add(cNMaquina);
            }
        }

        await db.SaveChangesAsync();
        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/reactivaClienteNMaquina")]
    public async Task<ActionResult> ReactivaClienteNMaquina([FromODataUri] int? idClienteNMaquina)
    {
        var clienteNMaquina = db.tblClienteNMaquina.FirstOrDefault(x => x.idClienteNMaquina == idClienteNMaquina);

        if (clienteNMaquina == null)
        {
            return Ok("No se encontró el cliente");
        }
        else if (clienteNMaquina.fechaFin != null)
        {
            var maquina = db.tblMaquina.FirstOrDefault(x => x.idMaquina == clienteNMaquina.idMaquina);
            var idLavanderia = maquina?.idLavanderia;

            clienteNMaquina.fechaFin = null;
            var fechaActual = DateTimeOffset.UtcNow;
            var fechaString = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
            var logObj = new tblLogError
            {
                denominacion = "reactivaCliente_" + idClienteNMaquina,
                error = fechaString
            };

            db.tblLogError.Add(logObj);

            await db.SaveChangesAsync();

            List<string> srcs = new List<string> { "tblClienteNMaquina" };
            await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
        }
        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/reactivaPersonaNMaquina")]
    public async Task<ActionResult> ReactivaPersonaNMaquina([FromBody] List<int> idsPersonaNMaquina)
    {
        var hayCambio = false;
        var tblPersonaNMaquina = db.tblPersonaNMaquina.Where(x => idsPersonaNMaquina.Contains(x.idPersonaNMaquina)).ToList();
        var idsPersonaNPersonaNMaquina = tblPersonaNMaquina.Select(x => x.idPersona).Distinct().ToList();
        var maquinas = tblPersonaNMaquina.Select(x => x.idMaquina).Distinct().ToList();
        var lavsMaquina = db.tblMaquina.Where(x => maquinas.Contains(x.idMaquina)).Select(x => x.idLavanderia).Distinct().ToList();

        tblPersonaNMaquina = db.tblPersonaNMaquina.Where(x => idsPersonaNPersonaNMaquina.Contains(x.idPersona)).ToList();

        var tblPersonaNAreaLavanderia = db.tblPersonaNAreaNLavanderia.Where(x => idsPersonaNPersonaNMaquina.Contains(x.idPersona) && lavsMaquina.Contains(x.idLavanderia)).ToList();
        var tblEventoPersona = db.tblEventoPersona.Where(x => idsPersonaNPersonaNMaquina.Contains(x.idPersona) && lavsMaquina.Contains(x.idLavanderia)).ToList();

        var fechaActual = DateTimeOffset.UtcNow;
        var fechaString = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");

        foreach (var idPersonaNMaquina in idsPersonaNMaquina)
        {
            var personaNMaquina = tblPersonaNMaquina.FirstOrDefault(x => x.idPersonaNMaquina == idPersonaNMaquina);

            if (personaNMaquina != null)
            {
                if (personaNMaquina.fechaFin != null)
                {
                    var idPersona_PersonaNMaquina = personaNMaquina.idPersona;
                    var fechaIniPersonaNMaquina = personaNMaquina.fechaIni;
                    var fechaFinPersonaNMaquina = personaNMaquina.fechaFin;
                    var lastsRegistros_area = tblPersonaNAreaLavanderia.Where(x => x.idPersona == idPersona_PersonaNMaquina &&
                                                                        (x.fechaFin >= fechaIniPersonaNMaquina || x.fechaIni >= fechaIniPersonaNMaquina));
                    var hayPersonaNArea_posterior = lastsRegistros_area.Any(x => x.fechaIni > fechaIniPersonaNMaquina);
                    var hayEventoPersona_posterior = tblEventoPersona.Any(x => x.idPersona == idPersona_PersonaNMaquina && x.fecha >= fechaIniPersonaNMaquina);
                    var hayPersonaNMaquina_posterior = tblPersonaNMaquina.Any(x => x.idPersona == idPersona_PersonaNMaquina && x.fechaIni > fechaIniPersonaNMaquina);
                    var hayActividad_posterior = hayPersonaNArea_posterior || hayEventoPersona_posterior || hayPersonaNMaquina_posterior;
                    var finAreaMatch = lastsRegistros_area.FirstOrDefault(x => x.idAreaLavanderia == 3 && x.fechaFin == fechaFinPersonaNMaquina);

                    if (finAreaMatch != null && !hayActividad_posterior)
                    {
                        finAreaMatch.fechaFin = null; // Reactiva personaNArea
                        personaNMaquina.fechaFin = null; // Reactiva personaNMaquina

                        var logObj = new tblLogError
                        {
                            denominacion = "reactivaPersona_" + idPersonaNMaquina,
                            error = fechaString
                        };

                        db.tblLogError.Add(logObj);
                        hayCambio = true;
                    }
                }
            }
        }

        if (hayCambio == true)
        {
            await db.SaveChangesAsync();

            foreach (var idLavanderia in lavsMaquina)
            {
                // Enviar señal para actualizar en el cliente
                List<string> srcs = new List<string> { "PersonalActivo" };
                await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
            }
        }
        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/tblPersonaNMaquina_Inicio")]
    public async Task<ActionResult> tblPersonaNMaquina_Inicio([FromBody] iuPersonaNMaquina personaNMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == personaNMaquina.idMaquina).FirstOrDefault().idLavanderia;
        int gmt = await ObtenerGMTLav(idLavanderia);
        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));

        List<tblPersonaNMaquina> personaActiva = db.tblPersonaNMaquina.Where(x =>
                                                                       (x.idPersona.Equals(personaNMaquina.idPersona)
                                                                       || (x.idMaquina.Equals(personaNMaquina.idMaquina)
                                                                       && x.numPos.Equals(personaNMaquina.numPos))) &&
                                                                        x.fechaFin == null)
                                                                        .ToList();

        foreach (tblPersonaNMaquina pnm in personaActiva)
        {
            pnm.fechaFin = offset;

            bool sinPersonaNMaquina = !db.tblPersonaNMaquina.Any(x => x.idMaquina == pnm.idMaquina && x.idPersona != pnm.idPersona && x.fechaFin == null);

            if (sinPersonaNMaquina)
            {
                // FIN CLIENTE
                List<tblClienteNMaquina> listCnm = db.tblClienteNMaquina
                    .Where(x => x.idMaquina == pnm.idMaquina && x.fechaFin == null)
                    .ToList();

                foreach (tblClienteNMaquina cnm in listCnm)
                {
                    cnm.fechaFin = offset;
                }
            }

            // FIN SMART AREA
            var regActivos_ = db.tblPersonaNAreaNLavanderia.Where(x => x.idPersona == pnm.idPersona && x.fechaFin == null);

            foreach (tblPersonaNAreaNLavanderia pnanl in regActivos_)
            {
                pnanl.fechaFin = offset;
            }
        }

        // Agregar una nueva persona y máquina
        db.tblPersonaNMaquina.Add(new tblPersonaNMaquina()
        {
            idPersona = personaNMaquina.idPersona,
            idMaquina = personaNMaquina.idMaquina,
            numPos = personaNMaquina.numPos,
            fechaIni = offset
        });

        // Finalizar registros activos en el área de Lavandería y comenzar en el área de Procesado
        var regActivos = db.tblPersonaNAreaNLavanderia.Where(x => x.idPersona == personaNMaquina.idPersona && x.fechaFin == null);

        if (regActivos.Any())
        {
            foreach (var pnanl in regActivos)
            {
                pnanl.fechaFin = offset;
            }
        }

        // Agregar un nuevo registro en el área de Procesado
        db.tblPersonaNAreaNLavanderia.Add(new tblPersonaNAreaNLavanderia()
        {
            idAreaLavanderia = 3, // Procesado
            idLavanderia = idLavanderia,
            idPersona = personaNMaquina.idPersona,
            fechaIni = offset
        });

        await db.SaveChangesAsync();

        // Enviar señal para actualizar en el cliente
        List<string> srcs = new List<string> { "PersonalActivo" };
        await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);


        // Devolver personas y máquinas activas
        return Ok(db.tblPersonaNMaquina
            .Where(x => x.idMaquina == personaNMaquina.idMaquina && x.fechaFin == null && x.numPos <= 5)
            .Select(x => new { x.idPersona, x.numPos, x.idPersonaNMaquina }));

    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/tblPersonaNMaquina_Fin")] //used
    public async Task<ActionResult> tblPersonaNMaquina_Fin([FromBody] iuPersonaNMaquina personaNMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == personaNMaquina.idMaquina).FirstOrDefault().idLavanderia;

        #region Aplicar offset lavanderia a fecha actual

        var tblLavanderia = db.tblLavanderia
       .Where(x => x.idLavanderia.Equals(idLavanderia))
       .Select(x => new
       {
           x.idLavanderia,
           x.horarioVerano,
           x.idZonaHorariaNavigation
       }).FirstOrDefault();

        int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
        #endregion

        tblPersonaNMaquina objPersonaNMaquina = db.tblPersonaNMaquina.Where(x =>
                  x.idPersona == personaNMaquina.idPersona &&
                  x.idMaquina == personaNMaquina.idMaquina &&
                  x.fechaFin == null).OrderByDescending(s => s.idPersonaNMaquina).FirstOrDefault();

        if (objPersonaNMaquina != null)
        {
            objPersonaNMaquina.fechaFin = offset;

            //DESLOGUEO SMART AREA
            var regActivos = db.tblPersonaNAreaNLavanderia.Where(x => x.idPersona.Equals(objPersonaNMaquina.idPersona) && x.fechaFin == null);
            if (regActivos.Count() > 0) //Finalizamos cualquier registro activo
            {
                foreach (tblPersonaNAreaNLavanderia pnanl in regActivos)
                {
                    pnanl.fechaFin = offset;
                }
            }

            await db.SaveChangesAsync();
            List<string> srcs = new List<string> { "PersonalActivo" };
            await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
        }

        return Ok(db.tblPersonaNMaquina.Where(x => x.idMaquina.Equals(personaNMaquina.idMaquina) && x.fechaFin == null && x.numPos <= 5).Select(x => new { x.idPersona, x.numPos }));
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/tblPersonaNMaquina_Fin_v2")] //used
    public async Task<ActionResult> tblPersonaNMaquina_Fin_v2([FromBody] iuPersonaNMaquina personaNMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == personaNMaquina.idMaquina).FirstOrDefault().idLavanderia;
        int gmt = await ObtenerGMTLav(idLavanderia);
        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));

        tblPersonaNMaquina objPersonaNMaquina = db.tblPersonaNMaquina.FirstOrDefault(x => x.idPersonaNMaquina == personaNMaquina.idPersonaNMaquina);

        if (objPersonaNMaquina != null)
        {
            objPersonaNMaquina.fechaFin = offset;
        }


        //DESLOGUEO SMART AREA
        var regActivos = db.tblPersonaNAreaNLavanderia.Where(x => x.idPersona.Equals(objPersonaNMaquina.idPersona) && x.fechaFin == null);
        if (regActivos.Count() > 0) //Finalizamos cualquier registro activo
        {
            foreach (tblPersonaNAreaNLavanderia pnanl in regActivos)
            {
                pnanl.fechaFin = offset;
            }
        }

        List<string> srcs = new List<string> { "PersonalActivo" };
        await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);

        await db.SaveChangesAsync();

        return Ok(db.tblPersonaNMaquina.Where(x => x.idMaquina.Equals(personaNMaquina.idMaquina) && x.fechaFin == null && x.numPos <= 5).Select(x => new { x.idPersona, x.numPos, x.idPersonaNMaquina }));
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/insert_tblPersonaNMaquina_offline")]
    public async Task<ActionResult> insert_tblPersonaNMaquina_offline([FromBody] List<iuPersonaNMaquina> personaNMaquina)
    {
        var tblPersonaNMaquina = db.tblPersonaNMaquina.ToList();
        var tblPersonaNArea = db.tblPersonaNAreaNLavanderia.ToList();

        // Elimina los registros que ya se encuentren en tblPersonaNMaquina

        var personaNMaquina_filtered = personaNMaquina.Where(x => !tblPersonaNMaquina.Any(y =>
                                                                    y.idPersona == x.idPersona &&
                                                                    y.idMaquina == x.idMaquina &&
                                                                    y.fechaIni == x.fechaIni &&
                                                                    y.fechaFin == x.fechaFin &&
                                                                    y.numPos == x.numPos)).ToList();

        int idMaquina = personaNMaquina.FirstOrDefault(x => x.idMaquina != null).idMaquina;
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == idMaquina).FirstOrDefault().idLavanderia;
        int gmt = await ObtenerGMTLav(idLavanderia);

        DateTimeOffset? offset = null;

        if (personaNMaquina.Count != personaNMaquina_filtered.Count)
        {
            db.tblLogError.Add(new tblLogError
            {
                denominacion = "insert_tblPersonaNMaquina_offline_" + idLavanderia,
                error = JsonSerializer.Serialize(personaNMaquina)
            });
        }

        foreach (var persona in personaNMaquina_filtered)
        {
            if (persona.idPersonaNMaquina != null)
            {
                var personaFound = tblPersonaNMaquina.FirstOrDefault(x => x.idPersonaNMaquina == persona.idPersonaNMaquina);
                offset = persona.fechaFin?.ToOffset(TimeSpan.FromHours(gmt));

                if (personaFound != null)
                {
                    personaFound.isOffline = true;
                    personaFound.fechaFin = offset;
                }

                //DESLOGUEO SMART AREA
                var regActivos = tblPersonaNArea.Where(x => x.idPersona.Equals(personaFound.idPersona) && x.fechaFin == null);
                if (regActivos.Count() > 0) //Finalizamos cualquier registro activo
                {
                    foreach (tblPersonaNAreaNLavanderia pnanl in regActivos)
                    {
                        pnanl.fechaFin = offset;
                    }
                }
            }
            else
            {
                var inicioOffSet = persona.fechaIni?.ToOffset(TimeSpan.FromHours(gmt));
                var finOffset = persona.fechaFin?.ToOffset(TimeSpan.FromHours(gmt));

                var new_pm = new tblPersonaNMaquina
                {
                    fechaFin = finOffset,
                    fechaIni = inicioOffSet,
                    idMaquina = persona.idMaquina,
                    idPersona = persona.idPersona,
                    isOffline = true,
                    numPos = persona.numPos
                };
                db.tblPersonaNMaquina.Add(new_pm);

                //DESLOGUEO SMART AREA
                var regActivos = tblPersonaNArea.Where(x => x.idPersona.Equals(persona.idPersona) && x.fechaFin == null);

                if (regActivos.Count() > 0) //Finalizamos cualquier registro activo
                {
                    foreach (tblPersonaNAreaNLavanderia pnanl in regActivos)
                    {
                        pnanl.fechaFin = finOffset;
                    }
                }
                else
                {
                    //Añado registro
                    var new_PAL = new tblPersonaNAreaNLavanderia
                    {
                        fechaFin = finOffset,
                        fechaIni = (DateTimeOffset)inicioOffSet,
                        idAreaLavanderia = 3,
                        idLavanderia = idLavanderia,
                        idPersona = persona.idPersona,
                    };
                    db.tblPersonaNAreaNLavanderia.Add(new_PAL);
                }
            }
        }

        await db.SaveChangesAsync();

        //Aviso lavanderia
        List<string> srcs = new List<string> { "PersonalActivo" };
        await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);

        var personasActivas = tblPersonaNMaquina.Where(x => x.idMaquina == idMaquina && x.fechaFin == null).Select(x => new
        {
            x.numPos,
            x.idPersona,
            x.idPersonaNMaquina
        });

        return Ok(personasActivas);
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/tblPrendaNMaquina_Insert")]
    public async Task<ActionResult> tblPrendaNMaquina_Insert([FromBody] iPrendaNMaquina prendaNMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == prendaNMaquina.idMaquina).FirstOrDefault().idLavanderia;

        #region Aplicar offset lavanderia a fecha actual

        var tblLavanderia = db.tblLavanderia
       .Where(x => x.idLavanderia.Equals(idLavanderia))
       .Select(x => new
       {
           x.idLavanderia,
           x.horarioVerano,
           x.idZonaHorariaNavigation
       }).FirstOrDefault();

        int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
        #endregion

        db.tblPrendaNMaquina.Add(new tblPrendaNMaquina()
        {
            idMaquina = prendaNMaquina.idMaquina,
            idFamilia = prendaNMaquina.idFamilia,
            idTipoPrenda = prendaNMaquina.idTipoPrenda,
            numVia = prendaNMaquina.numVia,
            fecha = offset
        });


        await db.SaveChangesAsync();

        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v2/tblPrendaNMaquina_Insert_v2")] //used
    public async Task<ActionResult> tblPrendaNMaquina_Insert_v2([FromBody] List<iPrendaNMaquina> prendasNMaquina)
    {
        int idMaquina = prendasNMaquina.FirstOrDefault(x => x.idMaquina != null).idMaquina;
        int idLavanderia = db.tblMaquina.FirstOrDefault(x => x.idMaquina == idMaquina).idLavanderia;
        int gmt = await ObtenerGMTLav(idLavanderia);
        DateTimeOffset? offset = null;

        // Elimina los registros que ya se encuentren en tblPrendaNMaquina

        var prendasNMaquina_filtered = prendasNMaquina.Where(x => !db.tblPrendaNMaquina.Any(y =>
                                                                            y.idMaquina == x.idMaquina &&
                                                                            y.fecha == x.fecha &&
                                                                            y.numVia == x.numVia &&
                                                                            y.idFamilia == x.idFamilia &&
                                                                            y.idTipoPrenda == x.idTipoPrenda
                                                                        )).ToList();

        if (prendasNMaquina.Count != prendasNMaquina_filtered.Count) // Si hay registros duplicados
        {

            var prendasMaquina_json = JsonSerializer.Serialize(prendasNMaquina_filtered);

            db.tblLogError.Add(new tblLogError
            {
                denominacion = "prendas_offline_idLavanderia_" + idLavanderia,
                error = prendasMaquina_json
            });
        }

        foreach (var prenda in prendasNMaquina_filtered)
        {

            if (prenda.fecha != null)
            {
                offset = prenda.fecha?.ToOffset(TimeSpan.FromHours(gmt));
            }
            else
            {
                offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
            }

            //Sacar fecha actual con offset de la lavandería
            var fechaOffset = prenda.fecha?.ToOffset(TimeSpan.FromHours(gmt));

            var fecha = prenda.fecha == null ? offset : fechaOffset;
            var isOffline = prenda.fecha != null ? true : false;

            db.tblPrendaNMaquina.Add(new tblPrendaNMaquina()
            {
                idFamilia = prenda.idFamilia,
                idTipoPrenda = prenda.idTipoPrenda,
                idMaquina = prenda.idMaquina,
                numVia = prenda.numVia,
                fecha = (DateTimeOffset)fecha,
                isOffline = isOffline
            });
        }

        await db.SaveChangesAsync();
        return Ok();
    }

    //======================================================================================================== NUEVO SMARTHUB

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/tblPrendasHora")]
    public async Task<ActionResult> tblPrendasHora(int idMaquina, int idFamilia, int? idTipoPrenda)
    {
        return Ok(db.tblPrendasHora.Where(x => x.idMaquina == idMaquina && x.idFamilia == idFamilia && x.idTipoPrenda == idTipoPrenda)
            .Select(x => new
            {
                x.idFamilia,
                x.idTipoPrenda,
                x.numVias,
                x.prendasHora
            }).FirstOrDefault());
    }


    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/tblPrendasHora_v2")] // uso en smarthub >= v2.0.10 
    public async Task<ActionResult> tblPrendasHora_v2(int idMaquina)
    {
        return Ok(db.tblPrendasHora.Where(x => x.idMaquina == idMaquina)
            .Select(x => new
            {
                x.idFamilia,
                x.idTipoPrenda,
                x.numVias,
                x.prendasHora
            }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/get_maquinas_lavanderia")]
    public async Task<ActionResult> get_maquinas_lavanderia([FromODataUri] int idLavanderia)
    {

        var maquinas = db.tblMaquina
            .Where(x => x.idLavanderia == idLavanderia && x.activo == true && x.eliminado == false &&
            (x.idTipoMaquinaNCategoriaMaquinaNavigation.idTipoMaquinaNavigation.codigo == "01" ||
            x.idTipoMaquinaNCategoriaMaquinaNavigation.idTipoMaquinaNavigation.codigo == "34"))
            .Select(x => new
            {
                x.denominacion,
                x.etiqueta,
                x.idMaquina,
                x.numPosicion,
                x.numSerie,
                x.idTipoMaquinaNCategoriaMaquinaNavigation.idTipoMaquinaNavigation.idTipoMaquina,
                tipoMaquina = x.idTipoMaquinaNCategoriaMaquinaNavigation.idTipoMaquinaNavigation.denominacion,
                x.idTipoMaquinaNCategoriaMaquinaNavigation.idTipoMaquinaNavigation.codigo,
                totalPosiciones = x.tblPosicionNAreaLavanderiaNLavanderia.Where(pall => pall.activo == true && pall.idAreaLavanderia == 3).Count()
            }).OrderBy(x => x.denominacion);

        return Ok(maquinas);
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/get_clienteNMaquina")]
    public async Task<ActionResult> get_clienteNMaquina([FromODataUri] int idMaquina)
    {
        var clienteNMaquina = db.tblClienteNMaquina.Where(x => x.idMaquina == idMaquina && x.fechaFin == null).FirstOrDefault();

        var result = new
        {
            clienteNMaquina.fechaFin,
            clienteNMaquina.fechaIni,
            clienteNMaquina.idClienteNMaquina,
            clienteNMaquina.idCompañia,
            clienteNMaquina.idEntidad,
            clienteNMaquina.idFamilia,
            clienteNMaquina.idMaquina,
            clienteNMaquina.idTipoPrenda
        };

        return Ok(result);
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v2/tblFamilia")]
    public async Task<ActionResult> tblFamilia(int idMaquina)
    {
        var result = db.tblPrendasHora
           .Where(ph => ph.idMaquina == idMaquina)
           .GroupBy(ph => ph.idFamilia)
           .Select(group => new
           {
               idFamilia = group.Key,
               group.First().idFamiliaNavigation.denominacion,
               tblTipoPrenda = group.Where(x => x.idTipoPrendaNavigation != null)
                                    .OrderBy(ph => ph.idTipoPrendaNavigation.codigo)
                                    .Select(ph => new { ph.idTipoPrendaNavigation.idTipoPrenda, ph.idTipoPrendaNavigation.denominacion })

           });

        return Ok(result);
    }

    private async Task<int> ObtenerGMTLav(int idLavanderia)
    {
        var tblLavanderia = db.tblLavanderia
       .Where(x => x.idLavanderia.Equals(idLavanderia))
       .Select(x => new
       {
           x.idLavanderia,
           x.horarioVerano,
           x.idZonaHorariaNavigation
       }).FirstOrDefault();

        int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

        return gmt;
    }

    public class iuClienteNMaquina
    {
        public int? idClienteNMaquina { get; set; }
        public int idMaquina { get; set; }
        public DateTimeOffset? fechaIni { get; set; }
        public DateTimeOffset? fechaFin { get; set; }
        public int? idCompañia { get; set; }
        public int? idEntidad { get; set; }
        public byte idFamilia { get; set; }
        public short? idTipoPrenda { get; set; }
        public bool isOffline { get; set; }
    }

    public class iuPersonaNMaquina
    {
        public int? idPersonaNMaquina { get; set; }
        public int idPersona { get; set; }
        public int idMaquina { get; set; }
        public DateTimeOffset? fechaIni { get; set; }
        public DateTimeOffset? fechaFin { get; set; }
        public int? numPos { get; set; }
        public bool isOffline { get; set; }
    }

    public class iPrendaNMaquina
    {
        public byte idFamilia { get; set; }
        public short? idTipoPrenda { get; set; }
        public int idMaquina { get; set; }
        public int numVia { get; set; }
        public DateTimeOffset? fecha { get; set; }
    }
}