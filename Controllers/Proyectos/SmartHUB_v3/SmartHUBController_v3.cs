using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;
using WebApiCore.Services.MyRealBonus;



namespace WebApiCore.Controllers;
[BasicAuth]
public class SmartHUBController_v3 : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;

    private readonly CalculoTokenService _calculoTokens;
    public SmartHUBController_v3(bdERP context, IHubContext<NotificacionesHub> hubContext, CalculoTokenService calculoTokens)
    {
        db = context;
        _hubContext = hubContext;
        _calculoTokens = calculoTokens;

    }



    [EnableQuery]
    [HttpGet("odata/SmartHUB_v3/tblCompañia")]
    public async Task<ActionResult> tblCompañia([FromODataUri] int idMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == idMaquina).FirstOrDefault().idLavanderia;

        List<int?> idsCompañia_excluir = new List<int?>() { 201, 227, 212 };

        return Ok(db.tblCompañia.Where(x =>
              x.tblEntidad.Where(y => y.idLavanderia.Where(l => l.idLavanderia == idLavanderia).Count() > 0).Count() > 0 &&
              idsCompañia_excluir.Contains(x.idCompañia) == false &&
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
    [HttpGet("odata/SmartHUB_v3/tblEntidad")]
    public async Task<ActionResult> tblEntidad([FromODataUri] int idMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == idMaquina).FirstOrDefault().idLavanderia;
        List<int?> idsEntidad_excluir = new List<int?>() { 579, 626, 678 };

        return Ok(db.tblEntidad.Where(x =>
            x.idLavanderia.Where(l => l.idLavanderia == idLavanderia).Count() > 0 &&
            idsEntidad_excluir.Contains(x.idEntidad) == false &&
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
    [HttpGet("odata/SmartHUB_v3/tblFamilia")]
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

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v3/tblGrupoPlantillaPrenda_generica")]
    public async Task<ActionResult> tblGrupoPlantillaPrenda_generica(int idMaquina)
    {
        var idLavanderia = db.tblMaquina
             .Where(x => x.idMaquina == idMaquina)
             .Select(x => x.idLavanderia)
             .FirstOrDefault();

        var idsPlantillasNPrenda = Utils.selectPrendasGenericasVisibles(db, idLavanderia);

        var grupoPlantillas = db.tblPlantillaPrenda_generica
            .Where(x => idsPlantillasNPrenda.Contains(x.idPlantillaPrenda_generica))
            .Select(x => new
            {
                x.idGrupoPlantillaPrenda_genericaNavigation.denominacion,
                x.idGrupoPlantillaPrenda_generica
            })
            .Distinct()
            .ToList();

        return Ok(grupoPlantillas);
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v3/tblPrendasHora")]
    public async Task<ActionResult> tblPrendasHora(int idMaquina)
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
    [HttpGet("odata/SmartHUB_v3/tblPersona")]
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
    [HttpGet("odata/SmartHUB_v3/tblPersonaNMaquina")]
    public async Task<ActionResult> tblPersonaNMaquina(int idMaquina)
    {
        return Ok(db.tblPersonaNMaquina.Where(x => x.idMaquina.Equals(idMaquina) && x.fechaFin == null && x.numPos <= 5).Select(x => new { x.idPersona, x.numPos, x.idPersonaNMaquina }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB_v3/get_maquinas_lavanderia")]
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
    [HttpGet("odata/SmartHUB_v3/get_clienteNMaquina")]
    public async Task<ActionResult> get_clienteNMaquina([FromODataUri] int idMaquina)
    {
        var clienteNMaquina = db.tblClienteNMaquina.Where(x => x.idMaquina == idMaquina && x.fechaFin == null).FirstOrDefault();

        if (clienteNMaquina == null)
        {
            return Ok("No se encontró el cliente");
        }

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
    [HttpPost("odata/SmartHUB_v3/insert_clienteNMaquina")]
    public async Task<ActionResult> insert_clienteNMaquina([FromBody] List<iuClienteNMaquina> clienteNMaquina)
    {
        #region Preparación de datos
        var idMaquinaCliente = clienteNMaquina.FirstOrDefault(x => x.idMaquina != null).idMaquina;
        var tblClienteNMaquina = db.tblClienteNMaquina.Where(x => x.idMaquina == idMaquinaCliente).ToList();
        // Filtro los registros que ya existen en la base de datos
        var clienteNMaquinaFiltrado = clienteNMaquina.Where(x => !tblClienteNMaquina.Any(y => x.idMaquina == x.idMaquina
                                                                                                && y.idCompañia == x.idCompañia
                                                                                                && y.idEntidad == x.idEntidad
                                                                                                && y.idFamilia == x.idFamilia
                                                                                                && y.idTipoPrenda == x.idTipoPrenda
                                                                                                && y.fechaIni == x.fecha))
                                                                                      .OrderBy(x => x.fecha ?? null)
                                                                                      .ToList();

        var clienteNMaquina_mod = new List<tblClienteNMaquina>();
        int? idLastClienteNMaquinaCerrado = null;

        #endregion

        foreach (var CM in clienteNMaquinaFiltrado)
        {
            #region Obtiene registros activos
            var idMaquina = CM.idMaquina;
            var idLavanderiaMaquina = db.tblMaquina.Where(x => x.idMaquina.Equals(idMaquina)).Select(x => x.idLavanderia).FirstOrDefault();

            var gmt = ObtenerGMTLav(idLavanderiaMaquina);
            var fechaOffSet = CM.fecha != null ? CM.fecha : Class.Utils.aplicarGMT(DateTimeOffset.Now, gmt);

            var clientesActivos = tblClienteNMaquina
                                        .Where(x => x.idMaquina == CM.idMaquina &&
                                                    x.fechaIni < fechaOffSet &&
                                                    (x.fechaFin == null || x.fechaFin > fechaOffSet))
                                        .ToList();

            var clienteActivo_mod = clienteNMaquina_mod.Where(x => x.idMaquina == CM.idMaquina &&
                                                                  x.fechaIni < fechaOffSet &&
                                                                 (x.fechaFin == null || x.fechaFin > fechaOffSet)
                                                        ).ToList();
            #endregion

            // Si es un INICIO
            if (CM.isInicio == true)
            {
                // Activo = fechaInicio < fechaRegistro && (fechaFin == null || fechaFin > fechaRegistro)
                #region Cierra clientes activos
                // Si hay clientes activos, se les asigna la fecha de fin
                if (clientesActivos.Count > 0)
                {
                    var clientesActivosAntiguos = clientesActivos.Where(x => x.fechaIni < fechaOffSet).ToList();
                    foreach (var clienteActivo in clientesActivosAntiguos)
                    {
                        clienteActivo.fechaFin = fechaOffSet;
                    }

                    var fechaActual = DateTimeOffset.UtcNow;
                    var fechaString = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
                    var logObj = new tblLogError
                    {
                        denominacion = "cierreClienteActivo_Inicio_" + CM.idClienteNMaquina + "_fechaAsignada_" + fechaOffSet,
                        error = fechaString
                    };

                    db.tblLogError.Add(logObj);
                }
                // Si hay clientes activos en la lista de modificaciones, se les asigna la fecha de fin
                if (clienteActivo_mod.Count() > 0)
                {
                    var clienteModActivoAntiguos = clienteActivo_mod.Where(x => x.fechaIni < fechaOffSet).ToList();
                    foreach (var clienteActivo in clienteModActivoAntiguos)
                    {
                        clienteActivo.fechaFin = fechaOffSet;
                    }
                }



                #endregion

                #region Añade registro
                // Se crea el nuevo clienteNMaquina 
                var clienteNMaquinaNuevo = new tblClienteNMaquina
                {
                    idMaquina = CM.idMaquina,
                    fechaIni = (DateTimeOffset)fechaOffSet,
                    idCompañia = CM.idCompañia,
                    idEntidad = CM.idEntidad,
                    idGrupoPlantillaPrenda_generica = CM.idGrupoPlantillaPrenda_generica,
                    idFamilia = CM.idFamilia,
                    idTipoPrenda = CM.idTipoPrenda,
                    isOffline = CM.fecha != null ? true : false
                };
                #endregion

                #region Añade fechaFin posterior
                // Se busca el cliente posterior 
                var fecha_clientePosteriorDB = tblClienteNMaquina.Where(x => x.fechaIni > fechaOffSet).OrderBy(x => x.fechaIni).FirstOrDefault()?.fechaIni;
                var fecha_clientePosteriorOffline = clienteNMaquinaFiltrado.Where(x => x.fecha > fechaOffSet).OrderBy(x => x.fecha).FirstOrDefault()?.fecha;

                // Se obtiene la fecha de fin del cliente posterior
                var fecha_clientePosterior = new List<DateTimeOffset?>
                                                            {
                                                            fecha_clientePosteriorDB,
                                                            fecha_clientePosteriorOffline
                                                            }.Where(x => x != null)
                                                            ?.Min() ?? null;

                // Si existe un cliente posterior, se le asigna la fecha de fin al nuevo cliente
                if (fecha_clientePosterior != null)
                {
                    clienteNMaquinaNuevo.fechaFin = fecha_clientePosterior;
                }

                // Revisa personas activas en la maquina. Si no hay personas activas, se asigna la fecha de fin al cliente
                var personasActivas = db.tblPersonaNMaquina.Where(x => x.idMaquina == CM.idMaquina && x.fechaFin == null && x.numPos != null).ToList();

                if (personasActivas.Any() == false && clienteNMaquinaNuevo.fechaFin.HasValue == false)
                {
                    var lastFechaFinPersonaActiva = db.tblPersonaNMaquina.Where(x => x.idMaquina == CM.idMaquina && x.fechaFin != null)
                                                                         .OrderByDescending(x => x.fechaFin)
                                                                         .FirstOrDefault()?.fechaFin;
                    if (lastFechaFinPersonaActiva != null)
                        clienteNMaquinaNuevo.fechaFin = lastFechaFinPersonaActiva;
                }

                clienteNMaquina_mod.Add(clienteNMaquinaNuevo);
                db.tblClienteNMaquina.Add(clienteNMaquinaNuevo);
                #endregion

                #region finaliza mantenimientos activos
                var mantenimientosActivos = db.tblMantenimientoNMaquina
                    .Where(x => x.idMaquina == CM.idMaquina && x.fechaFin == null && x.fechaIni < fechaOffSet)
                    .ToList();

                foreach (var mantenimientoActivo in mantenimientosActivos)
                {
                    mantenimientoActivo.fechaFin = fechaOffSet;
                }

                #endregion
            }
            // Si es un FIN
            else
            {
                #region Cierra clientes activos
                // Si hay clientes activos, se les asigna la fecha de fin
                if (clientesActivos.Count > 0)
                {
                    var clienteActivoAntiguo = clientesActivos.Where(x => x.fechaIni < fechaOffSet).ToList();
                    foreach (var clienteActivo in clienteActivoAntiguo)
                    {
                        clienteActivo.fechaFin = fechaOffSet;
                    }
                    var fechaActual = DateTimeOffset.UtcNow;
                    var fechaString = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
                    var logObj = new tblLogError
                    {
                        denominacion = "cierreClienteActivo_Fin_" + CM.idClienteNMaquina + "_fechaAsignada_" + fechaOffSet,
                        error = fechaString
                    };

                    db.tblLogError.Add(logObj);
                }

                if (clienteActivo_mod.Count() > 0)
                {
                    var clienteModActivoAntiguo = clienteActivo_mod.Where(x => x.fechaIni < fechaOffSet).ToList();
                    foreach (var clienteActivo in clienteModActivoAntiguo)
                    {
                        clienteActivo.fechaFin = fechaOffSet;
                    }
                }

                var isOnline = CM.fecha == null ? true : false;

                if (isOnline && clienteNMaquina.Count == 1)
                {
                    idLastClienteNMaquinaCerrado = clientesActivos.Where(x => x.fechaIni < fechaOffSet)
                                                                    .OrderByDescending(x => x.fechaFin)
                                                                    .FirstOrDefault()?.idClienteNMaquina;
                }

                var sinRegistroActivo = clientesActivos.Count == 0 && clienteActivo_mod.Count() == 0;
                var isRegistroUnico = clienteNMaquina.Count == 1;

                if (isOnline && isRegistroUnico && sinRegistroActivo)
                { // Si es un fin, online y no hay registros activos, se guarda en el offline del cliente
                    return BadRequest();
                }

                #endregion
            }

            db.AddRange(clienteNMaquina_mod);
        }
        var idLavanderia = clienteNMaquinaFiltrado.Select(x => db.tblMaquina.Where(y => y.idMaquina == x.idMaquina).Select(y => y.idLavanderia).FirstOrDefault()).Distinct().ToList();

        await db.SaveChangesAsync();

        List<string> srcs = new List<string> { "tblClienteNMaquina" };
        await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);

        if (idLastClienteNMaquinaCerrado != null)
        {
            return Ok(idLastClienteNMaquinaCerrado);
        }
        else
        {
            return Ok();
        }
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v3/reactivaClienteNMaquina")]
    public async Task<ActionResult> ReactivaClienteNMaquina([FromODataUri] int? idClienteNMaquina)
    {
        var clienteNMaquina = db.tblClienteNMaquina.FirstOrDefault(x => x.idClienteNMaquina == idClienteNMaquina);

        if (clienteNMaquina == null)
        {
            return Ok("No se encontró el cliente");
        }
        else if (clienteNMaquina.fechaFin != null)
        {
            var hayPersonasActivas = db.tblPersonaNMaquina.Any(x => x.idMaquina == clienteNMaquina.idMaquina && x.fechaFin == null && x.numPos != null);
            var hayClientePosterior = db.tblClienteNMaquina.Any(x => x.idMaquina == clienteNMaquina.idMaquina && x.fechaIni > clienteNMaquina.fechaIni);

            // Se reactiva solo si hay personas activas en la maquina y no hay cliente iniciado posterior. 
            if (hayPersonasActivas && !hayClientePosterior)
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
        }
        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v3/tblClienteNMaquina_update_Fin")]
    public async Task<ActionResult> tblClienteNMaquina_update_Fin([FromODataUri] int idClienteNMaquina)
    {
        var clienteNMaquina = db.tblClienteNMaquina.Where(x => x.idClienteNMaquina == idClienteNMaquina).FirstOrDefault();
        int idLavanderia = db.tblMaquina.FirstOrDefault(x => x.idMaquina == clienteNMaquina.idMaquina).idLavanderia;
        int gmt = ObtenerGMTLav(idLavanderia);
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
    [HttpPost("odata/SmartHUB_v3/insert_prendaNMaquina")]
    public async Task<ActionResult> insert_prendaNMaquina([FromBody] List<iPrendaNMaquina> prendasNMaquina)
    {
        int idMaquina = prendasNMaquina.FirstOrDefault(x => x.idMaquina != null).idMaquina;
        int idLavanderia = db.tblMaquina.FirstOrDefault(x => x.idMaquina == idMaquina).idLavanderia;
        int gmt = ObtenerGMTLav(idLavanderia);
        DateTimeOffset? offset = null;

        // Elimina los registros que ya se encuentren en tblPrendaNMaquina
        var prendasNMaquina_filtered = prendasNMaquina.Where(x => !db.tblPrendaNMaquina.Any(y =>
                                                                            y.idMaquina == x.idMaquina &&
                                                                            y.fecha == x.fecha &&
                                                                            y.numVia == x.numVia &&
                                                                            y.idFamilia == x.idFamilia &&
                                                                            y.idTipoPrenda == x.idTipoPrenda
                                                                        )).ToList();

        if (prendasNMaquina.Count != prendasNMaquina_filtered.Count) // Si hay registros duplicados se guardan en tblLogError
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
                offset = prenda.fecha;
            }
            else
            {
                offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
            }

            var isOffline = prenda.fecha != null ? true : false;

            db.tblPrendaNMaquina.Add(new tblPrendaNMaquina()
            {
                idFamilia = prenda.idFamilia,
                idTipoPrenda = prenda.idTipoPrenda,
                idMaquina = prenda.idMaquina,
                numVia = prenda.numVia,
                fecha = (DateTimeOffset)offset,
                isOffline = isOffline
            });
        }

        await db.SaveChangesAsync();
        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v3/insert_personaNMaquina")]
    public async Task<ActionResult> insert_personaNMaquina([FromBody] List<iuPersonaNMaquina> personaNMaquina)
    {
        #region Preparación de datos

        var idsPersona = personaNMaquina.Select(x => x.idPersona).Distinct().ToList();
        var firstFechaReg = personaNMaquina.OrderBy(x => x.fecha).FirstOrDefault(x => x.fecha != null)?.fecha ?? DateTimeOffset.Now;

        // Datos 
        var tblPersonaNMaquina = db.tblPersonaNMaquina.Where(x => idsPersona.Contains(x.idPersona)).ToList();
        var tblPersonaNArea = db.tblPersonaNAreaNLavanderia.Where(x => idsPersona.Contains(x.idPersona)).ToList();
        var tblEventoPersona = db.tblEventoPersona.Where(x => idsPersona.Contains(x.idPersona) && x.fecha >= firstFechaReg).ToList();

        var tblPersonaNMaquina_mod = new List<tblPersonaNMaquina>();
        var tblPersonaNArea_mod = new List<tblPersonaNAreaNLavanderia>();

        #endregion

        #region Elimina los registros que ya se encuentren en tblPersonaNMaquina

        var personaNMaquina_filtered = personaNMaquina.Where(x => x.idPersona != null &&
                                                                !tblPersonaNMaquina.Any(y =>
                                                                    y.idPersona == x.idPersona &&
                                                                    y.idMaquina == x.idMaquina &&
                                                                    y.fechaIni == x.fecha &&
                                                                    y.numPos == x.numPos))
                                                        .OrderBy(x => x.fecha ?? null)
                                                        .ToList();

        int idMaquina = personaNMaquina.FirstOrDefault(x => x.idMaquina != null).idMaquina;
        int idLavanderia = db.tblMaquina.FirstOrDefault(x => x.idMaquina == idMaquina).idLavanderia;
        int gmt = ObtenerGMTLav(idLavanderia);

        #endregion

        foreach (var persMaq in personaNMaquina_filtered)
        {
            var fechaOffSet = persMaq.fecha != null ? persMaq.fecha : Class.Utils.aplicarGMT(DateTimeOffset.Now, gmt);
            var isOnline = persMaq.fecha == null ? true : false;
            var registroUnico = personaNMaquina.Count == 1;
            var isFin = persMaq.isInicio == false;

            {
                if (persMaq.isInicio == false)
                {

                    #region Obtiene registros activos
                    var regPersonaNMaquinaActivo = tblPersonaNMaquina.Where(x =>
                                                                    x.idPersona == persMaq.idPersona &&
                                                                    x.fechaIni < fechaOffSet &&
                                                                    (x.fechaFin == null || x.fechaFin > fechaOffSet))
                                                                  .ToList();

                    var regPersonaNMaquinaActivo_mod = tblPersonaNMaquina_mod.Where(x =>
                                                                                x.idPersona == persMaq.idPersona &&
                                                                                (x.fechaFin == null || x.fechaFin > fechaOffSet) &&
                                                                                x.fechaIni < fechaOffSet)
                                                                              .ToList();

                    var regPersonaNAreaActivo = tblPersonaNArea.Where(x =>
                                                                    x.idAreaLavanderia == 3 &&
                                                                    x.idPersona == persMaq.idPersona &&
                                                                    (x.fechaFin == null || x.fechaFin > fechaOffSet) &&
                                                                    x.fechaIni < fechaOffSet)
                                                                .ToList();

                    var regPersonaNAreaActivo_mod = tblPersonaNArea_mod.Where(x =>
                                                                            x.idPersona == persMaq.idPersona &&
                                                                            x.idAreaLavanderia == 3 &&
                                                                            (x.fechaFin == null || x.fechaFin > fechaOffSet) &&
                                                                            x.fechaIni < fechaOffSet)
                                                                        .ToList();
                    #endregion

                    #region Cierre de registros activos

                    var hayCierrePNM = false;

                    if (regPersonaNMaquinaActivo.Count > 0)
                    {
                        foreach (var pma in regPersonaNMaquinaActivo)
                        {
                            pma.fechaFin = fechaOffSet;
                            //Inicio cálculo tokens

                            int idPersona = pma.idPersona;
                            int idMaq = pma.idMaquina;
                            DateTimeOffset? fechaIni = pma.fechaIni;
                            DateTimeOffset? fechaFin = pma.fechaFin;
                            double tiempoTurnoSegundos = _calculoTokens.obtenerTiempoTurnoEnSegundos(fechaIni, fechaFin);

                            await _calculoTokens.calcularTokensParaPersona(
                                 idPersona,
                                 idMaq,
                                 fechaIni,
                                 fechaFin,
                                 tiempoTurnoSegundos);

                            // Fin cálculo tokens
                        }
                        hayCierrePNM = true;

                        var fechaActual = DateTimeOffset.UtcNow;
                        var fechaString = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
                        var logObj = new tblLogError
                        {
                            denominacion = "cierrePersonaNMaquina_fin_" + persMaq.idPersona + "_fechaAsignada_" + fechaOffSet,
                            error = fechaString
                        };

                        db.tblLogError.Add(logObj);
                    }

                    if (regPersonaNMaquinaActivo_mod.Count > 0)
                    {
                        foreach (var pma in regPersonaNMaquinaActivo_mod)
                        {
                            pma.fechaFin = fechaOffSet;
                            //Inicio cálculo tokens

                            int idPersona = pma.idPersona;
                            int idMaq = pma.idMaquina;
                            DateTimeOffset? fechaIni = pma.fechaIni;
                            DateTimeOffset? fechaFin = pma.fechaFin;
                            double tiempoTurnoSegundos = _calculoTokens.obtenerTiempoTurnoEnSegundos(fechaIni, fechaFin);

                            await _calculoTokens.calcularTokensParaPersona(
                                 idPersona,
                                 idMaq,
                                 fechaIni,
                                 fechaFin,
                                 tiempoTurnoSegundos);

                            // Fin cálculo tokens
                        }
                        hayCierrePNM = true;
                    }

                    if (regPersonaNAreaActivo.Count > 0 && hayCierrePNM == true)
                    {
                        foreach (var pam in regPersonaNAreaActivo)
                        {
                            pam.fechaFin = fechaOffSet;
                        }

                        var fechaActual = DateTimeOffset.UtcNow;
                        var fechaString = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
                        var logObj = new tblLogError
                        {
                            denominacion = "cierrePersonaNArea_fin_" + persMaq.idPersona + "_fechaAsignada_" + fechaOffSet,
                            error = fechaString
                        };

                        db.tblLogError.Add(logObj);
                    }

                    if (regPersonaNAreaActivo_mod.Count > 0 && hayCierrePNM == true)
                    {
                        foreach (var pam in regPersonaNAreaActivo_mod)
                        {
                            pam.fechaFin = fechaOffSet;
                        }
                    }

                    var sinRegActivo = regPersonaNMaquinaActivo.Count == 0 &&
                                        regPersonaNMaquinaActivo_mod.Count == 0 &&
                                        regPersonaNAreaActivo.Count == 0 &&
                                        regPersonaNAreaActivo_mod.Count == 0;


                    if (isOnline && registroUnico && sinRegActivo && isFin)
                    {
                        // Si no hay registros y es online, la llamada falla para guardarse en el offline del cliente
                        return BadRequest();
                    }

                    #endregion

                }
                else
                {
                    #region Obtiene registro anterior
                    var RegPosterior = new List<DateTimeOffset?> { };
                    var regPAL_anterior = tblPersonaNArea.Where(x => x.idPersona == persMaq.idPersona && x.fechaIni < fechaOffSet)
                                                        .OrderByDescending(x => x.fechaIni)
                                                        .FirstOrDefault();

                    var regPAL_mod_anterior = tblPersonaNArea_mod.Where(x => x.idPersona == persMaq.idPersona && x.fechaIni < fechaOffSet)
                                                            .OrderByDescending(x => x.fechaIni)
                                                            .FirstOrDefault();

                    var regPNM_anterior = tblPersonaNMaquina.Where(x => x.idPersona == persMaq.idPersona && x.fechaIni < fechaOffSet)
                                                            .OrderByDescending(x => x.fechaIni)
                                                            .FirstOrDefault();

                    var regPNM_mod_anterior = tblPersonaNMaquina_mod.Where(x => x.idPersona == persMaq.idPersona && x.fechaIni < fechaOffSet)
                                                                .OrderByDescending(x => x.fechaIni)
                                                                .FirstOrDefault();
                    #endregion

                    #region Creación de nuevos registros
                    var personaNMaquina_nuevoReg = new tblPersonaNMaquina
                    {
                        idPersona = (int)persMaq.idPersona,
                        idMaquina = persMaq.idMaquina,
                        fechaIni = fechaOffSet,
                        numPos = persMaq.numPos,
                        isOffline = persMaq.fecha != null ? true : false
                    };

                    var personaNArea_nuevoReg = new tblPersonaNAreaNLavanderia
                    {
                        idPersona = (int)persMaq.idPersona,
                        idAreaLavanderia = 3,
                        idLavanderia = idLavanderia,
                        fechaIni = (DateTimeOffset)fechaOffSet,
                        isOffline = persMaq.fecha != null ? true : false
                    };
                    #endregion

                    #region Obtiene registros posteriores
                    var regPersonaNMaquinaPosterior_local = personaNMaquina_filtered.Where(x => x.idPersona == persMaq.idPersona && x.fecha > fechaOffSet)
                                                                                    .OrderBy(x => x.fecha)
                                                                                    .FirstOrDefault();

                    var regPersonaNMaquinaPosterior_db = tblPersonaNMaquina.Where(x => x.idPersona == persMaq.idPersona && x.fechaIni > fechaOffSet)
                                                                            .OrderBy(x => x.fechaIni)
                                                                            .FirstOrDefault();

                    var regPersonaNAreaPosterior = tblPersonaNArea.Where(x => x.idPersona == persMaq.idPersona && x.fechaIni > fechaOffSet)
                                                                   .OrderBy(x => x.fechaIni)
                                                                   .FirstOrDefault();

                    var regEventoPersonaPosterior = tblEventoPersona.Where(x => x.idPersona == persMaq.idPersona && x.fecha > fechaOffSet)
                                                                    .OrderBy(x => x.fecha)
                                                                    .FirstOrDefault();
                    #endregion

                    #region Añade Registros Posterior

                    if (regPersonaNMaquinaPosterior_local != null && regPersonaNMaquinaPosterior_local.fecha != null)
                    {
                        RegPosterior.Add(regPersonaNMaquinaPosterior_local.fecha.Value);
                    }

                    if (regPersonaNMaquinaPosterior_db != null && regPersonaNMaquinaPosterior_db.fechaIni != null)
                    {
                        RegPosterior.Add(regPersonaNMaquinaPosterior_db.fechaIni.Value);
                    }

                    if (regPersonaNAreaPosterior != null && regPersonaNAreaPosterior.fechaIni != null)
                    {
                        RegPosterior.Add(regPersonaNAreaPosterior.fechaIni);
                    }

                    if (regEventoPersonaPosterior != null && regEventoPersonaPosterior.fecha != null)
                    {
                        RegPosterior.Add(regEventoPersonaPosterior.fecha);
                    }

                    var RegPosteriorMin = RegPosterior.Count > 0 ? RegPosterior.Min() : null;


                    if (RegPosteriorMin != null)
                    {
                        personaNMaquina_nuevoReg.fechaFin = RegPosteriorMin;
                        personaNArea_nuevoReg.fechaFin = RegPosteriorMin;
                    }

                    #endregion
                    

                    #region Actualiza registro anterior

                    if (regPAL_anterior != null && regPAL_anterior.fechaFin == null || regPAL_anterior?.fechaFin > fechaOffSet)
                    {
                        regPAL_anterior.fechaFin = fechaOffSet;
                        var fechaActual = DateTimeOffset.UtcNow;
                        var fechaString = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
                        var logObj = new tblLogError
                        {
                            denominacion = "cierraPAL_anterior" + persMaq.idPersona + "_fechaAsignada_" + fechaOffSet,
                            error = fechaString
                        };

                        db.tblLogError.Add(logObj);
                    }

                    if (regPAL_mod_anterior != null && regPAL_mod_anterior.fechaFin == null || regPAL_mod_anterior?.fechaFin > fechaOffSet)
                    {
                        regPAL_mod_anterior.fechaFin = fechaOffSet;
                    }

                    if (regPNM_anterior != null && regPNM_anterior.fechaFin == null || regPNM_anterior?.fechaFin > fechaOffSet)
                    {
                        regPNM_anterior.fechaFin = fechaOffSet;
                        var fechaActual = DateTimeOffset.UtcNow;
                        var fechaString = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
                        var logObj = new tblLogError
                        {
                            denominacion = "cierraPNM_anterior" + persMaq.idPersona + "_fechaAsignada_" + fechaOffSet,
                            error = fechaString
                        };

                        db.tblLogError.Add(logObj);
                    }

                    if (regPNM_mod_anterior != null && regPNM_mod_anterior.fechaFin == null || regPNM_mod_anterior?.fechaFin > fechaOffSet)
                    {
                        regPNM_mod_anterior.fechaFin = fechaOffSet;
                    }
                    #endregion

                    #region Finaliza mantenimientos activos

                    var mantenimientosActivos = db.tblMantenimientoNMaquina
                        .Where(x => x.idMaquina == persMaq.idMaquina && x.fechaFin == null && x.fechaIni < fechaOffSet)
                        .ToList();

                    foreach (var mantenimientoActivo in mantenimientosActivos)
                    {
                        mantenimientoActivo.fechaFin = fechaOffSet;
                    }

                    #endregion

                    tblPersonaNMaquina_mod.Add(personaNMaquina_nuevoReg);
                    tblPersonaNArea_mod.Add(personaNArea_nuevoReg);
                }
            }

            db.AddRange(tblPersonaNMaquina_mod);
            db.AddRange(tblPersonaNArea_mod);
        }

        await db.SaveChangesAsync();




            var fechaActualLavanderia = Class.Utils.aplicarGMT(DateTimeOffset.Now, gmt);
        foreach (var persMaq in personaNMaquina)
        {
            var fechaOffSet = persMaq.fecha ?? fechaActualLavanderia;

            if (persMaq.isInicio == true)
            {
                var persMaqAnterior = db.tblPersonaNMaquina
                                        .Where(x => x.idPersona == persMaq.idPersona && x.fechaFin < fechaOffSet)
                                        .OrderByDescending(x => x.fechaIni)
                                        .FirstOrDefault();

                if (persMaqAnterior != null)
                    await Utils.FinalizaClienteNMaquinaAsync(db, (DateTimeOffset)fechaOffSet, persMaqAnterior.idMaquina, (int)persMaq.idPersona);
            }
            
            else
            {
                await Utils.FinalizaClienteNMaquinaAsync(db, (DateTimeOffset)fechaOffSet, persMaq.idMaquina, (int)persMaq.idPersona);
            }

            
        }

    

        // Enviar señal para actualizar en el cliente
        List<string> srcs = new List<string> { "PersonalActivo" };
        await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);

        return Ok(db.tblPersonaNMaquina
                             .Where(x =>
                                 x.idMaquina == idMaquina &&
                                 x.fechaFin == null && x.numPos <= 5)
                             .Select(x => new { x.idPersona, x.numPos, x.idPersonaNMaquina }));
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB_v3/reactivaPersonaNMaquina")]
        public async Task<ActionResult> ReactivaPersonaNMaquina([FromBody] List<int> idsPersonaNMaquina)
    {

        if (idsPersonaNMaquina.Count() == 0)
        {
            return Ok();
        }

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
    [HttpPost("odata/SmartHUB_v3/insert_mantenimientoNMaquina")]
    public async Task<ActionResult> insert_mantenimientoNMaquina([FromBody] List<iuMantenimientoNMaquina> mantenimientoNMaquina)
    {
        var idMaquina = mantenimientoNMaquina.FirstOrDefault(x => x.idMaquina != null).idMaquina;

        var personasActivasNMaquina = db.tblPersonaNMaquina.Where(x => x.idMaquina == idMaquina && x.fechaFin == null).ToList();
        var clientesActivosNMaquina = db.tblClienteNMaquina.Where(x => x.idMaquina == idMaquina && x.fechaFin == null).ToList();
        var mantenimientosActivos = db.tblMantenimientoNMaquina.Where(x => x.idMaquina == idMaquina && x.fechaFin == null).ToList();

        var tblMantenimientoNMaquina_mod = new List<tblMantenimientoNMaquina>();
        var personasActivas_mod = new List<tblPersonaNMaquina>();
        var clientesActivos_mod = new List<tblClienteNMaquina>();

        try
        {
            foreach (var mantenimiento in mantenimientoNMaquina)
            {

                var idUsuario = db.tblUsuario.FirstOrDefault(x => x.idPersona == mantenimiento.idPersona)?.idUsuario;
                var isInicio = mantenimiento.isInicio;
                var isOffline = mantenimiento.fecha != null ? true : false;
                idMaquina = mantenimiento.idMaquina;
                var idLavanderia = db.tblMaquina.FirstOrDefault(x => x.idMaquina == idMaquina).idLavanderia;
                var fechaLavanderia = Class.Utils.aplicarGMT(DateTimeOffset.Now, ObtenerGMTLav(idLavanderia));
                var fechaMantenimiento = mantenimiento.fecha ?? fechaLavanderia;

                if (isInicio)
                {
                    var objMantenimiento = new tblMantenimientoNMaquina
                    {
                        idMaquina = mantenimiento.idMaquina,
                        fechaIni = fechaMantenimiento,
                        idTipoMantenimientoNMaquina = 5,
                        idUsuario = (int)idUsuario,
                        isOffline = isOffline
                    };

                    db.tblMantenimientoNMaquina.Add(objMantenimiento);
                    tblMantenimientoNMaquina_mod.Add(objMantenimiento);
                }
                else
                {
                    var mantenimientoActivo = db.tblMantenimientoNMaquina
                                                .Where(x => x.idMaquina == mantenimiento.idMaquina && x.fechaFin == null).ToList();
                    var mantenimientoActivo_mod = tblMantenimientoNMaquina_mod
                                                    .Where(x => x.idMaquina == mantenimiento.idMaquina && x.fechaFin == null).ToList();

                    foreach (var ma in mantenimientoActivo)
                    {
                        ma.fechaFin = fechaMantenimiento;
                    }

                    foreach (var ma in mantenimientoActivo_mod)
                    {
                        ma.fechaFin = fechaMantenimiento;
                    }

                    db.tblMantenimientoNMaquina.UpdateRange(mantenimientoActivo);
                }

                var personasActivasNMaquina_filtered = personasActivasNMaquina.Where(x => x.fechaIni < fechaMantenimiento).ToList();
                var clientesActivosNMaquina_filtered = clientesActivosNMaquina.Where(x => x.fechaIni < fechaMantenimiento).ToList();
                var mantenimientosActivos_filtered = mantenimientosActivos.Where(x => x.fechaIni < fechaMantenimiento).ToList();

                if (personasActivasNMaquina_filtered.Count > 0)
                {
                    foreach (var personaActiva in personasActivasNMaquina_filtered)
                    {
                        if (personaActiva.fechaFin == null)
                        {
                            personaActiva.fechaFin = fechaMantenimiento;
                            personasActivas_mod.Add(personaActiva);
                        }
                    }
                }

                if (clientesActivosNMaquina_filtered.Count > 0)
                {
                    foreach (var clienteActivo in clientesActivosNMaquina_filtered)
                    {
                        if (clienteActivo.fechaFin == null)
                        {
                            clienteActivo.fechaFin = fechaMantenimiento;
                            clientesActivos_mod.Add(clienteActivo);
                        }
                    }
                }

                if (mantenimientosActivos_filtered.Count > 0)
                {
                    foreach (var mantenimientoActivo in mantenimientosActivos_filtered)
                    {
                        if (mantenimientoActivo.fechaFin == null)
                            mantenimientoActivo.fechaFin = fechaMantenimiento;
                    }
                }
            }

            db.AddRange(tblMantenimientoNMaquina_mod);
            db.UpdateRange(personasActivas_mod);
            db.UpdateRange(clientesActivos_mod);
            db.UpdateRange(mantenimientosActivos);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private int ObtenerGMTLav(int idLavanderia)
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

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class iuClienteNMaquina
    {
        public int? idClienteNMaquina { get; set; }
        public int idMaquina { get; set; }
        public DateTimeOffset? fecha { get; set; }
        public int? idCompañia { get; set; }
        public int? idEntidad { get; set; }
        public byte? idGrupoPlantillaPrenda_generica { get; set; }
        public byte idFamilia { get; set; }
        public short? idTipoPrenda { get; set; }
        public bool? isOffline { get; set; }
        public bool? isInicio { get; set; }
    }

    public class iuPersonaNMaquina
    {
        public int? idPersonaNMaquina { get; set; }
        public int? idPersona { get; set; }
        public int idMaquina { get; set; }
        public DateTimeOffset? fecha { get; set; }
        public int? numPos { get; set; }
        public bool? isOffline { get; set; }
        public bool? isInicio { get; set; }

    }

    public class iuMantenimientoNMaquina
    {
        public int idMaquina { get; set; }
        public DateTimeOffset? fecha { get; set; }
        public int? idTipoMantenimiento { get; set; }
        public int? idPersona { get; set; }
        public bool isInicio { get; set; }
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
