using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class AppLogisticaExternaController : ODataController
{
    private readonly bdERP db;
    public AppLogisticaExternaController(bdERP context)
    {
        db = context;
    }


    [HttpGet("odata/AppLogisticaExterna/tblPersona")]
    [Authorize]
    public async Task<ActionResult> tblPersona()
    {

        var usuarioNPersonaTransportista = db.tblUsuario.Where(x =>
                                            x.idPersonaNavigation.activo
                                            && x.idPersonaNavigation.eliminado == false
                                            && x.idPersonaNavigation.idTipoTrabajo.Value == 6)
                                                .OrderBy(x => x.nombre)
                                                .Select(x => new
                                                {
                                                    x.idPersonaNavigation.nombre,
                                                    x.idPersonaNavigation.apellidos,
                                                    idPersona = x.idPersona.Value,
                                                    x.idUsuario
                                                });

        if (usuarioNPersonaTransportista == null || !usuarioNPersonaTransportista.Any())
        {
            return NotFound();
        }

        return Ok(usuarioNPersonaTransportista);
    }


    #region PARTE DE TRANSPORTE

    [HttpPost("odata/AppLogisticaExterna/inicioParteTransporte")]
    [Authorize]
    public async Task<ActionResult> inicioParteTransporte([FromBody] tblParteTransporte parteTransporte)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        //Se cierran los partes en curso del mismo usuario
        var partes_cerrar = db.tblParteTransporte.Where(x => x.idUsuarioResponsable.Equals(idUsuario) && x.idEstado == 1);
        foreach (tblParteTransporte parte in partes_cerrar)
        {
            parte.idEstado = 3;
        }

        parteTransporte.idUsuarioResponsable = idUsuario;
        parteTransporte.idEstado = 1;

        var paradasPersonalizadas = parteTransporte.tblParadaNParteTransporte;

        if (paradasPersonalizadas.Count == 0)
        {
            var ruta = db.tblRutaExpedicion.Where(x => x.idRutaExpedicion.Equals(parteTransporte.idRutaExpedicion))
                  .Select(x => new
                  {
                      denoRutaExpedicion = x.denominacion,
                      paradas = x.tblParadaNRutaExpedicion
                  }).FirstOrDefault();

            //Ruta de expedición
            parteTransporte.denoRutaExpedicion = ruta.denoRutaExpedicion;
            parteTransporte.tblParadaNParteTransporte = ruta.paradas.Select(x => new tblParadaNParteTransporte()
            {
                idLavanderia = x.idLavanderia,
                idEntidad = x.idEntidad,
                orden = x.orden,
                observaciones = x.observaciones,
                isCarga = x.isCarga
            }).ToList();
        }
        else
        {
            parteTransporte.denoRutaExpedicion = "Ruta personalizada";
            parteTransporte.tblParadaNParteTransporte = parteTransporte.tblParadaNParteTransporte.Select(x => new tblParadaNParteTransporte()
            {
                idLavanderia = x.idLavanderia,
                idEntidad = x.idEntidad,
                orden = x.orden,
                observaciones = x.observaciones,
                isCarga = x.isCarga
            }).ToList();
        }

        //Transportistas
        ICollection<tblPersona> transportistas = parteTransporte.idPersonaTransportista;
        parteTransporte.idPersonaTransportista = new List<tblPersona>();

        await db.SaveChangesAsync();

        foreach (tblPersona transportista in transportistas)
        {
            parteTransporte.idPersonaTransportista.Add(db.tblPersona.Where(e => e.idPersona == transportista.idPersona).FirstOrDefault());
        }

        db.tblParteTransporte.Add(parteTransporte);

        await db.SaveChangesAsync();

        return Ok(new
        {
            parteTransporte = datosParteTransporte(parteTransporte.idParteTransporte),
            tblParadaNParteTransporte = datosTblParadaNParteTransporte(parteTransporte.idParteTransporte)
        });
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/checkParteTransportePendiente")]
    [Authorize]
    public async Task<ActionResult> checkParteTransportePendiente()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        //Solo se permitirá reanudar un parte si su última fecha de parada es de hace menos de 3 horas.
        var fechaFiltro = DateTimeOffset.UtcNow.AddHours(-4);
        return Ok(db.tblParteTransporte
            .Where(x =>
                x.idUsuarioResponsable.Equals(idUsuario) &&
                x.idEstado == 1 &&
                x.tblParadaNParteTransporte.Count(p => p.fechaLlegada >= fechaFiltro || p.fechaSalida >= fechaFiltro) > 0
            ).Select(x => new { x.idParteTransporte }).FirstOrDefault());
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/reanudarParteTransporte")]
    [Authorize]
    public async Task<ActionResult> reanudarParteTransporte([FromODataUri] int idParteTransporte)
    {
        return Ok(new
        {
            parteTransporte = datosParteTransporte(idParteTransporte),
            tblParadaNParteTransporte = datosTblParadaNParteTransporte(idParteTransporte)
        });
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/finalizarParteTransporte")]
    [Authorize]
    public async Task<ActionResult> finalizarParteTransporte([FromODataUri] int idParteTransporte, [FromBody] tblParteTransporte parteTransporte)
    {
        tblParteTransporte objParteTransporte = await db.tblParteTransporte.FindAsync(idParteTransporte);
        objParteTransporte.idEstado = 2;
        objParteTransporte.observaciones = parteTransporte.observaciones;
        objParteTransporte.kmsFinalesVehiculo = parteTransporte.kmsFinalesVehiculo;

        var descargaLavanderia = db.tblParadaNParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte) && x.idLavanderia != null && x.isCarga.Equals(false));
        foreach (tblParadaNParteTransporte parada in descargaLavanderia)
        {
            parada.fechaLlegada = DateTimeOffset.UtcNow;
            parada.fechaSalida = DateTimeOffset.UtcNow;
        }

        await db.SaveChangesAsync();

        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/cerrarParteTransporte")]
    [Authorize]
    public async Task<ActionResult> cerrarParteTransporte([FromODataUri] int idParteTransporte)
    {
        //bool eliminable = db.tblParadaNParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte) && x.fechaLlegada != null).Count() == 0;
        //if (eliminable)
        //{
        //    db.tblParadaNParteTransporte.RemoveRange(db.tblParadaNParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte)));
        //    db.tblRevisionVehiculoNParteTransporte.RemoveRange(db.tblRevisionVehiculoNParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte)));
        //    db.tblParteTransporte_Localizacion.RemoveRange(db.tblParteTransporte_Localizacion.Where(x => x.idParteTransporte.Equals(idParteTransporte)));
        //    db.tblParteTransporte.Remove(db.tblParteTransporte.Find(idParteTransporte));
        //    //tblRepartoNParteTransporte 
        //    //tblTransportistaNParteTransporte
        //}
        //else
        //{
        db.tblParteTransporte.Find(idParteTransporte).idEstado = 3;
        //}

        await db.SaveChangesAsync();

        return Ok();
    }

    #endregion

    #region PARADAS

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/cancelarLLegadaParadaNParteTransporte")]
    [Authorize]
    public async Task<ActionResult> cancelarLLegadaParadaNParteTransporte([FromODataUri] int idParadaNParteTransporte)
    {
        tblParadaNParteTransporte objParada = await db.tblParadaNParteTransporte.FindAsync(idParadaNParteTransporte);
        objParada.fechaLlegada = null;
        await db.SaveChangesAsync();
        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/posponerParadaNParteTransporte")]
    [Authorize]
    public async Task<ActionResult> posponerParadaNParteTransporte([FromODataUri] int idParadaNParteTransporte)
    {
        tblParadaNParteTransporte objParada = await db.tblParadaNParteTransporte.FindAsync(idParadaNParteTransporte);
        tblParadaNParteTransporte objParadaSiguiente = db.tblParadaNParteTransporte
            .Where(x => x.idParteTransporte.Equals(objParada.idParteTransporte) && x.orden == objParada.orden + 1).FirstOrDefault();

        if (objParadaSiguiente == null)
        {
            return BadRequest();
        }

        objParada.fechaLlegada = null; //Se limpia la fechaLLegada para que no detecte la parada como activa.
        objParada.fechaPospuesto = DateTimeOffset.UtcNow;
        objParada.orden++;
        objParadaSiguiente.orden--;

        await db.SaveChangesAsync();
        return Ok(datosTblParadaNParteTransporte(objParada.idParteTransporte));
    }

    #endregion

    #region LAVANDERIA

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/llegadaLavanderia")]
    [Authorize]
    public async Task<ActionResult> llegadaLavanderia([FromODataUri] int idParteTransporte, int idLavanderia)
    {
        tblParadaNParteTransporte objParada = db.tblParadaNParteTransporte
            .Where(x => x.idParteTransporte.Equals(idParteTransporte) && x.idLavanderia.Equals(idLavanderia) && x.isCarga.Equals(true)).First();

        objParada.fechaLlegada = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync();

        return Ok(repartosDisponibles(idParteTransporte, idLavanderia));
    }

    [EnableQuery]
    [HttpGet("odata/AppLogisticaExterna/getRepartosDisponibles")]
    [Authorize]
    public async Task<ActionResult> getRepartosDisponibles([FromODataUri] int idParteTransporte, int idLavanderia)
    {
        return Ok(repartosDisponibles(idParteTransporte, idLavanderia));
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/salidaLavanderia")]
    [Authorize]
    public async Task<ActionResult> salidaLavanderia([FromODataUri] int idParteTransporte, int idLavanderia, [FromBody] List<tblRepartoNParteTransporte> repartosNParteTransporte)
    {
        tblParadaNParteTransporte objParada = db.tblParadaNParteTransporte
            .Where(x => x.idParteTransporte.Equals(idParteTransporte) && x.idLavanderia.Equals(idLavanderia) && x.isCarga.Equals(true)).First();

        objParada.fechaSalida = DateTimeOffset.UtcNow;

        tblParteTransporte objParteTransporte = await db.tblParteTransporte.FindAsync(idParteTransporte);
        foreach (tblRepartoNParteTransporte repNparte in repartosNParteTransporte)
        {
            objParteTransporte.tblRepartoNParteTransporte.Add(new tblRepartoNParteTransporte()
            {
                idReparto = repNparte.idReparto,
                numCarros = repNparte.numCarros,
                numCarrosRemontados = repNparte.numCarrosRemontados
            });
        }

        await db.SaveChangesAsync();

        return Ok(datosTblParadaNParteTransporte(idParteTransporte));
    }


    #endregion

    #region ENTIDAD

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/llegadaEntidad")]
    [Authorize]
    public async Task<ActionResult> llegadaEntidad([FromODataUri] int idParteTransporte, int idEntidad)
    {
        tblParadaNParteTransporte objParada = db.tblParadaNParteTransporte
            .Where(x => x.idParteTransporte.Equals(idParteTransporte) && x.idEntidad.Equals(idEntidad)).First();

        if (objParada.fechaLlegada == null)
        {
            objParada.fechaLlegada = DateTimeOffset.UtcNow;
        }

        await db.SaveChangesAsync();

        return Ok(repartosEntidadYPrendasPendientes(idParteTransporte, idEntidad));
    }

    [EnableQuery]
    [HttpGet("odata/AppLogisticaExterna/reanudaEntidad")]
    [Authorize]
    public async Task<ActionResult> reanudaEntidad([FromODataUri] int idParteTransporte, int idEntidad)
    {
        return Ok(repartosEntidadYPrendasPendientes(idParteTransporte, idEntidad));
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/salidaEntidad")]
    [Authorize]
    public async Task<ActionResult> salidaEntidad([FromODataUri] int idParteTransporte, int idEntidad)
    {
        tblParadaNParteTransporte objParada = db.tblParadaNParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte) && x.idEntidad.Equals(idEntidad)).First();

        objParada.fechaSalida = DateTimeOffset.UtcNow;

        var repartos = db.tblParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte))
            .Select(x => x.tblRepartoNParteTransporte.Where(r => r.idRepartoNavigation.idEntidad.Equals(idEntidad) &&
                    //Filtramos aquellos repartos incompletos.
                    r.idRepartoNavigation.numCarros -
                    r.idRepartoNavigation.tblRepartoNParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte)).Sum(p => p.numCarros) -
                    r.idRepartoNavigation.tblRepartoNParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte)).Sum(p => p.numCarrosRemontados) == 0
                    )
                    .Select(x => x.idRepartoNavigation))
            .First();

        //Set estado repartos
        foreach (tblReparto rep in repartos)
        {
            rep.idRepartoEstado = 3;
        }

        await db.SaveChangesAsync();

        return Ok(datosTblParadaNParteTransporte(idParteTransporte));
    }

    #endregion

    #region PAUSAS

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/iniciarPausa")]
    [Authorize]
    public async Task<ActionResult> iniciarPausa([FromBody] tblParadaNParteTransporte paradaNParteTransporte)
    {
        List<tblParadaNParteTransporte> paradasSinTerminar = db.tblParadaNParteTransporte.OrderBy(y => y.orden)
                                   .Where(x => x.idParteTransporte.Equals(paradaNParteTransporte.idParteTransporte) &&
                                   x.fechaLlegada == null && x.fechaSalida == null && x.fechaOmitido == null)
                                   .ToList();

        if (paradasSinTerminar.Count > 0)
        {
            byte siguienteOrden = paradasSinTerminar.First().orden;
            foreach (tblParadaNParteTransporte parada in paradasSinTerminar)
            {
                parada.orden++;
            }

            db.tblParadaNParteTransporte.Add(new tblParadaNParteTransporte()
            {
                idParteTransporte = paradaNParteTransporte.idParteTransporte,
                orden = siguienteOrden,
                fechaLlegada = DateTimeOffset.UtcNow,
                idMotivoPausa = paradaNParteTransporte.idMotivoPausa,
                idIncidencia = paradaNParteTransporte.idIncidencia,
                observaciones = paradaNParteTransporte.observaciones,
            });

            await db.SaveChangesAsync();
        }
        return Ok(datosTblParadaNParteTransporte(paradaNParteTransporte.idParteTransporte));
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/finalizarPausa")]
    [Authorize]
    public async Task<ActionResult> finalizarPausa([FromBody] tblParadaNParteTransporte paradaNParteTransporte)
    {
        tblParadaNParteTransporte objParada = await db.tblParadaNParteTransporte.FindAsync(paradaNParteTransporte.idParadaNParteTransporte);
        objParada.fechaSalida = DateTimeOffset.UtcNow;
        objParada.idMotivoPausa = paradaNParteTransporte.idMotivoPausa;
        objParada.idIncidencia = paradaNParteTransporte.idIncidencia;
        objParada.observaciones = paradaNParteTransporte.observaciones;

        await db.SaveChangesAsync();

        return Ok(datosTblParadaNParteTransporte(paradaNParteTransporte.idParteTransporte));
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/cancelarPausa")]
    [Authorize]
    public async Task<ActionResult> cancelarPausa([FromODataUri] int idParadaNParteTransporte)
    {
        tblParadaNParteTransporte objParada = await db.tblParadaNParteTransporte.FindAsync(idParadaNParteTransporte);
        objParada.fechaCancelado = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync();

        return Ok(datosTblParadaNParteTransporte(objParada.idParteTransporte));
    }

    #endregion

    #region OTROS

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/setLocalizacion")]
    [Authorize]
    public async Task<ActionResult> setLocalizacion([FromBody] List<tblParteTransporte_Localizacion> localizaciones)
    {
        try
        {
            List<tblParteTransporte_Localizacion> localizaciones_add = new List<tblParteTransporte_Localizacion>();
            foreach (tblParteTransporte_Localizacion localizacion in localizaciones)
            {
                localizaciones_add.Add(new tblParteTransporte_Localizacion()
                {
                    idParteTransporte = localizacion.idParteTransporte,
                    fecha = localizacion.fecha,
                    coordenadas = localizacion.coordenadas,
                    idParadaNParteTransporte = localizacion?.idParadaNParteTransporte ?? null,
                    isOffline = localizacion?.isOffline ?? false,
                    heading = localizacion?.heading ?? null,
                    accuracy = localizacion?.accuracy ?? null,
                    speed = localizacion?.speed ?? null
                });
            }

            db.tblParteTransporte_Localizacion.AddRange(localizaciones_add);

            await db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaExterna/setLogError")]
    //[Authorize]
    public async Task<ActionResult> setLogError([FromODataUri] string text)
    {
        try
        {
            db.tblLogError.Add(new tblLogError()
            {
                denominacion = text,
                error = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")
            });

            db.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }

    #endregion

    #region DATOS

    //Inicio y reanudar
    private object datosParteTransporte(int idParteTransporte)
    {
        return db.tblParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte))
            .Select(x => new
            {
                x.idParteTransporte,
                x.idVehiculo,
                x.denoRutaExpedicion,
                x.kmsInicialesVehiculo
            }).FirstOrDefault();
    }

    //Inicio, reanudar, salidas y omitir parada
    private object datosTblParadaNParteTransporte(int idParteTransporte)
    {
        return db.tblParadaNParteTransporte.OrderBy(x => x.orden).Where(x => x.idParteTransporte.Equals(idParteTransporte) && x.fechaCancelado == null)
            .Select(x => new
            {
                x.idParadaNParteTransporte,
                x.idLavanderia,
                x.idEntidad,
                x.fechaLlegada,
                x.fechaSalida,
                x.observaciones,
                x.isCarga,
                x.idIncidencia,
                x.idMotivoPausa,
                x.fechaPospuesto,
                denoParada = x.idEntidad != null ? x.idEntidadNavigation.denominacion : x.idLavanderiaNavigation.denominacion,
                denoCompañia = x.idEntidad != null ? x.idEntidadNavigation.idCompañiaNavigation.denominacion : null,
                coordenadas = x.idEntidad != null ? x.idEntidadNavigation.coordenadas : x.idLavanderiaNavigation.coordenadas,
                activo = x.idEntidad == null || x.idParteTransporteNavigation.tblRepartoNParteTransporte.Where(r => r.idRepartoNavigation.idEntidad.Equals(x.idEntidad)).Count() > 0,
                done = x.fechaSalida != null,
                omitido = x.fechaOmitido != null,
                paradaAnterior = db.tblParadaNParteTransporte.OrderBy(y => y.orden)
                                    .Where(y => y.orden < x.orden && y.idParteTransporte.Equals(idParteTransporte) && y.fechaCancelado == null)
                                    .Select(y => new { done = y.fechaSalida != null, omitido = y.fechaOmitido != null }).LastOrDefault()
            }).ToList()
            .Select(x =>
            new
            {
                x.idParadaNParteTransporte,
                x.idLavanderia,
                x.idEntidad,
                x.fechaLlegada,
                x.fechaSalida,
                x.observaciones,
                x.isCarga,
                x.idIncidencia,
                x.idMotivoPausa,
                x.fechaPospuesto,
                x.denoParada,
                x.denoCompañia,
                x.coordenadas,
                x.activo,
                actual = (x.paradaAnterior == null || x.paradaAnterior.done || x.paradaAnterior.omitido) && !x.done && !x.omitido,
                x.done,
                x.omitido,
                colorEstado = _getColorEstadoParada(x.activo, x.done, ((x.paradaAnterior == null || x.paradaAnterior.done || x.paradaAnterior.omitido) && !x.done && !x.omitido))
            }).ToList();
    }

    private string _getColorEstadoParada(bool activo, bool done, bool actual)
    {
        //Actual por defecto
        string color = "primary";

        if (!actual)
        {
            if (activo) //Con repartos
            {
                if (done) //Ya repartido
                {
                    color = "success";
                }
                else //Por repartir
                {
                    color = "gray";
                }
            }
            else //Sin repartos
            {
                if (done) //Recogida de sucio
                {
                    color = "success";
                }
                else //Opcional recogida sucio
                {
                    color = "lightGray";
                }
            }
        }

        return color;
    }

    //Llegada lavanderia y refresh
    private object repartosDisponibles(int idParteTransporte, int idLavanderia)
    {
        var entidades = db.tblParadaNParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte) && x.idEntidad != null).Select(x => x.idEntidad);

        return db.tblReparto.Where(x => entidades.Contains(x.idEntidad) && x.idRepartoEstado == 1 && x.idLavanderia.Equals(idLavanderia))
            .Select(x => new
            {
                x.idReparto,
                x.codigo,
                x.fecha,
                x.idEntidad,
                numCarros = ((sbyte?)x.numCarros) -
                (
                    x.tblRepartoNParteTransporte.Where(x => x.idParteTransporteNavigation.idEstado != 3).Sum(r => r.numCarros) +
                    x.tblRepartoNParteTransporte.Where(x => x.idParteTransporteNavigation.idEstado != 3).Sum(r => r.numCarrosRemontados)
                ),
                denoEntidad = x.idEntidadNavigation.denominacion,
                denoCompañia = x.idEntidadNavigation.idCompañiaNavigation.denominacion
            })
            .Where(x => x.numCarros > 0).ToList();
    }

    //LLegada entidad
    private object repartosEntidadYPrendasPendientes(int idParteTransporte, int idEntidad)
    {
        var idsRepartos = db.tblParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte)).Select(x => x.tblRepartoNParteTransporte.Select(x => x.idReparto)).First();
        var repartos = db.tblReparto.Where(x => idsRepartos.Contains(x.idReparto) && x.idEntidad.Equals(idEntidad))
             .Select(x => new
             {
                 x.idReparto,
                 x.idPedido,
                 x.codigo,
                 x.fecha,
                 x.idEntidad,
                 numCarros = x.tblRepartoNParteTransporte.Where(r => r.idParteTransporte.Equals(idParteTransporte)).Sum(r => r.numCarros),
                 isCompleto = (((sbyte?)x.numCarros) -
                 (
                       x.tblRepartoNParteTransporte.Sum(r => r.numCarros) + x.tblRepartoNParteTransporte.Sum(r => r.numCarrosRemontados)
                 )) == 0,
                 denoEntidad = x.idEntidadNavigation.denominacion,
                 denoCompañia = x.idEntidadNavigation.idCompañiaNavigation.denominacion,
                 tblPrendaNReparto = x.tblPrendaNReparto.Select(pnr => new
                 {
                     pnr.idPrenda,
                     pnr.cantidad,
                     pnr.idPrendaNavigation.denominacion,
                     pnr.idPrendaNavigation.codigoPrenda,
                     pnr.idPrendaNavigation.udsXBacReparto,
                     colorTapa = pnr.idPrendaNavigation.idColorTapaNavigation.codigoHexadecimal,
                     marcaTapa = pnr.idPrendaNavigation.idMarcaTapaNavigation.marca
                 }).ToList()
             });

        //Cálculo prendas pendientes
        var idsPedido = repartos.Select(x => x.idPedido);

        var pedido = from pnp in db.tblPrendaNPedido
                     where idsPedido.Contains(pnp.idPedido)
                     group pnp by new { pnp.idPrenda } into g
                     select new
                     {
                         idPrenda = g.Key.idPrenda,
                         peticion = g.Sum(pnp => pnp.peticion)
                     };

        var repartido = from pnr in db.tblPrendaNReparto
                        where idsPedido.Contains((int)pnr.idRepartoNavigation.idPedido)
                        group pnr by pnr.idPrenda into g
                        select new
                        {
                            idPrenda = g.Key,
                            repartido = g.Sum(pnr => pnr.cantidad)
                        };


        var prendasPendientes = from pnp in pedido
                                join pre in db.tblPrenda on pnp.idPrenda equals pre.idPrenda
                                join pnr in repartido on pnp.idPrenda equals pnr.idPrenda into rep_
                                from pnr in rep_.DefaultIfEmpty()
                                select new
                                {
                                    pnp.idPrenda,
                                    cantidad = pnp.peticion - (pnr.repartido != null ? pnr.repartido : 0),
                                    pre.denominacion,
                                    pre.codigoPrenda,
                                    pre.udsXBacReparto,
                                    colorTapa = pre.idColorTapaNavigation.codigoHexadecimal,
                                    marcaTapa = pre.idMarcaTapaNavigation.marca
                                };

        prendasPendientes = prendasPendientes.Where(x => x.cantidad > 0);

        return new { repartos, prendasPendientes };
    }

    #endregion
}
