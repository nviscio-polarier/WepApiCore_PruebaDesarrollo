using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiCore.Class.Proyectos.MyPolarier.RRHH;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.Gestoria;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblSolicitudAltaController : ODataController
{

    private NotificacionesRRHH_GestoriaService notificaciones;
    private readonly IHubContext<NotificacionesHub> _hubContext;

    private readonly bdERP db;
    public tblSolicitudAltaController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
        notificaciones = new NotificacionesRRHH_GestoriaService(db, true);
    }

    private enum TipoMail
    {
        AltaCreada,
        AltaValidada,
        AltaSS,
        IncidenciaCreada,
        IncidenciaSolucionada,
        PetAnulacion,
        Anulada,
    }

    [EnableQuery(MaxExpansionDepth = 4)]
    [HttpGet]
    [Authorize]

    public async Task<ActionResult> Get()
    {
        return Ok(db.tblSolicitudAlta);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] List<SolicitudGestoria> solicitudGestoria)
    {
        try
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            foreach (var item in solicitudGestoria)
            {
                var llamamiento = db.tblLlamamiento.First(x => x.idLlamamiento == item.idLlamamiento && x.activo == true);
                var fechaActual = new DateTimeOffset(DateTime.Now);

                var insert = new tblSolicitudAlta
                {
                    fecha_reg = fechaActual,
                    idEstadoSolicitudAlta = (byte)idsEstadoSolicitudAlta.Pendiente,
                    idSolicitudAlta = (int)item.idLlamamiento,
                    isUrgente = (bool)(item.isUrgente == null ? false : item.isUrgente),
                    isLlamamiento = (bool)(item.isLlamamiento == null ? false : item.isLlamamiento),
                    tblDocumentoNSolicitudAlta = item.tblDocumentoNSolicitudAlta
                };

                insert.tblEstadoSolicitudAltaNSolicitudAlta.Add(new tblEstadoSolicitudAltaNSolicitudAlta
                {
                    idEstadoSolicitudAlta = insert.idEstadoSolicitudAlta,
                    idUsuario = idUsuario,
                    fecha = DateTimeOffset.Now,
                });

                db.tblSolicitudAlta.Add(insert);
            }
            await db.SaveChangesAsync();

            await notificaciones.SendAvisos_NuevasAltas(solicitudGestoria.Count);

            return Ok(true);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblSolicitudAlta> solicitudGestoria)
    {
        try
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            var objUsuario = db.tblUsuario.Include(x => x.idPermiso).FirstOrDefault(x => x.idUsuario == idUsuario);
            TipoMail? enviarMail = null;

            var entity = await db.tblSolicitudAlta
                .Include(x => x.tblDocumentoNSolicitudAlta)
                    .ThenInclude(x => x.idDocumentoNavigation)
                .Include(x => x.idSolicitudAltaNavigation)
                    .ThenInclude(x => x.idPersonaNavigation)
                .FirstOrDefaultAsync(x => x.idSolicitudAlta == key);

            var operation_tblDocumentoNSolicitudAlta = solicitudGestoria.Operations.FirstOrDefault(x => x.path == "/tblDocumentoNSolicitudAlta");
            if (operation_tblDocumentoNSolicitudAlta != null)
            {
                var value = JsonConvert.DeserializeObject<List<tblDocumentoNSolicitudAlta>>(operation_tblDocumentoNSolicitudAlta.value.ToString());
                foreach (var item in value)
                {
                    var documento = entity.tblDocumentoNSolicitudAlta.FirstOrDefault(x => x.idDocumento == item.idDocumento && x.idDocumento != 0);
                    if (documento == null)
                    {
                        item.idDocumentoNavigation.fecha = DateTimeOffset.Now;
                        item.idDocumentoNavigation.idUsuario = idUsuario;
                        item.idDocumentoNavigation.fechaModificacion = DateTimeOffset.Now;
                        item.idDocumentoNavigation.idUsuarioModificacion = idUsuario;

                        item.idDocumentoNavigation.idPersona = entity.idSolicitudAltaNavigation.idPersona;
                        item.idDocumentoNavigation.isVisible = false;

                        item.idDocumentoNavigation.idCarpetaDocumentos =
                            item.idDocumentoNavigation.idTipoDocumento == (byte)idsTipoDocumento.Contrato
                                ? (byte)idsCarpetaDocumentos.Contratos
                                : (byte)idsCarpetaDocumentos.General;


                        item.isFromGestoria = objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.SolicitudesAltaGestoria);

                        entity.tblDocumentoNSolicitudAlta.Add(item);
                    }
                    else
                    {
                        documento.idDocumentoNavigation.fechaModificacion = DateTimeOffset.Now;
                        documento.idDocumentoNavigation.idTipoDocumento = item.idDocumentoNavigation.idTipoDocumento;
                        documento.idDocumentoNavigation.denominacion = item.idDocumentoNavigation.denominacion;

                        var notificar = item.idDocumentoNavigation.isVisible != documento.idDocumentoNavigation.isVisible && documento.idDocumentoNavigation.isVisible == true;
                        documento.idDocumentoNavigation.isVisible = item.idDocumentoNavigation.isVisible;
                        documento.idDocumentoNavigation.firmado = item.idDocumentoNavigation.firmado;

                        if (notificar)
                        {
                            _hubContext.Clients.Group("RRHH").SendAsync("RRHH/notificaciones", "tblDocumento");

                            tblUsuario entityUsuario = db.tblUsuario.FirstOrDefault(x => x.idPersona == documento.idDocumentoNavigation.idPersona);
                            if (entityUsuario != null)
                            {
                                NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
                                notificaciones.SendToUser(entityUsuario.idUsuario.ToString(), "notificaciones_RRHH", "tblDocumento");

                                if (documento.idDocumentoNavigation.notificacion == true)
                                {
                                    string type = documento.idDocumentoNavigation.requerido == true ? "tblDocumento_requerido" : documento.idDocumentoNavigation.firmado == true ? "tblDocumento_firmado" : "tblDocumento_nuevo";
                                    await new tblDocumentoController(db, _hubContext).PushNotification(type, entityUsuario.idUsuario, entityUsuario.notificationToken, documento.idDocumentoNavigation.denominacion);
                                }
                            }
                        }
                    }
                }

                db.tblDocumentoNSolicitudAlta.RemoveRange(entity.tblDocumentoNSolicitudAlta.Where(x => value.All(y => y.idDocumento != x.idDocumento)));

                solicitudGestoria.Operations.Remove(operation_tblDocumentoNSolicitudAlta);
            }

            var operation_fechaIni = solicitudGestoria.Operations.FirstOrDefault(x => x.path == "/idSolicitudAltaNavigation/fechaIni");
            if (operation_fechaIni != null)
            {
                var fechaIni = new JsonPatchDocument<tblSolicitudAlta> { ContractResolver = new DefaultContractResolver() };
                fechaIni.Operations.Add(operation_fechaIni);

                fechaIni.ApplyTo(entity);

                solicitudGestoria.Operations.Remove(operation_fechaIni);
            }

            var operation_codigoGestoria = solicitudGestoria.Operations.FirstOrDefault(x => x.path == "/idSolicitudAltaNavigation/idPersonaNavigation/codigoGestoria");
            if (operation_codigoGestoria != null)
            {
                var codigoGestoria = new JsonPatchDocument<tblSolicitudAlta> { ContractResolver = new DefaultContractResolver() };
                codigoGestoria.Operations.Add(operation_codigoGestoria);

                codigoGestoria.ApplyTo(entity);

                solicitudGestoria.Operations.Remove(operation_codigoGestoria);
            }

            var operation_fechaAntiguedad = solicitudGestoria.Operations.FirstOrDefault(x => x.path == "/fechaAntiguedad");
            if (operation_fechaAntiguedad != null)
            {
                var objDatosSalariales = db.tblDatosSalariales.FirstOrDefault(x => x.idPersona == entity.idSolicitudAltaNavigation.idPersona);
                if (objDatosSalariales != null)
                {
                    objDatosSalariales.fechaAntiguedad = DateTime.Parse(operation_fechaAntiguedad.value.ToString());
                    objDatosSalariales.fechaAntiguedad_idUsuario_mod = idUsuario;
                    objDatosSalariales.fechaAntiguedad_fecha_mod = DateTimeOffset.Now;
                }
                else
                {
                    db.tblDatosSalariales.Add(new tblDatosSalariales
                    {
                        idPersona = (int)entity.idSolicitudAltaNavigation.idPersona,
                        fechaAntiguedad = DateTime.Parse(operation_fechaAntiguedad.value.ToString()),
                        fechaAntiguedad_idUsuario_mod = idUsuario,
                        fechaAntiguedad_fecha_mod = DateTimeOffset.Now,
                    });
                }

                solicitudGestoria.Operations.Remove(operation_fechaAntiguedad);
            }

            var operation_idEstadoSolicitudAlta = solicitudGestoria.Operations.FirstOrDefault(x => x.path == "/idEstadoSolicitudAlta");
            if (operation_idEstadoSolicitudAlta != null)
            {
                var value = byte.Parse(operation_idEstadoSolicitudAlta.value.ToString());

                switch ((idsEstadoSolicitudAlta)value)
                {
                    case idsEstadoSolicitudAlta.PetAnulacion:
                        if (!objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.SolicitudesAltaRRHH) && objUsuario.idCargo != (short)idsCargo.Desarrollador)
                            return BadRequest("No tienes permisos para anular solicitudes");
                        break;
                    case idsEstadoSolicitudAlta.Anulada:
                        if (!objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.SolicitudesAltaGestoria) && objUsuario.idCargo != (short)idsCargo.Desarrollador)
                            return BadRequest("No tienes permisos para anular solicitudes");
                        break;
                    case idsEstadoSolicitudAlta.EnProceso:
                        if (!objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.SolicitudesAltaGestoria) && objUsuario.idCargo != (short)idsCargo.Desarrollador)
                            return BadRequest("No tienes permisos para procesar solicitudes");
                        break;
                    case idsEstadoSolicitudAlta.PendienteDocs:
                        if (!objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.SolicitudesAltaGestoria) && objUsuario.idCargo != (short)idsCargo.Desarrollador)
                            return BadRequest("No tienes permisos para validar solicitudes");
                        break;
                    case idsEstadoSolicitudAlta.PendienteRRHH:
                        if (!objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.SolicitudesAltaGestoria) && objUsuario.idCargo != (short)idsCargo.Desarrollador)
                            return BadRequest("No tienes permisos para validar solicitudes");
                        break;
                    case idsEstadoSolicitudAlta.Validado:
                        if (!objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.SolicitudesAltaRRHH) && objUsuario.idCargo != (short)idsCargo.Desarrollador)
                            return BadRequest("No tienes permisos para validar solicitudes");
                        break;
                    default:
                        break;
                }

                if (value == (byte)idsEstadoSolicitudAlta.PendienteDocs)
                {
                    if (
                        entity.idSolicitudAltaNavigation.idPersonaNavigation.codigoGestoria != null &&
                        //Si tiene contrato o es llamamiento
                        entity.tblDocumentoNSolicitudAlta.Any(x => x.isFromGestoria && (x.idDocumentoNavigation.idTipoDocumento == (byte)idsTipoDocumento.Contrato || entity.isLlamamiento)) &&
                        entity.tblDocumentoNSolicitudAlta.Any(x => x.isFromGestoria && x.idDocumentoNavigation.idTipoDocumento == (byte)idsTipoDocumento.Mod145)
                    )
                    {
                        value = (byte)idsEstadoSolicitudAlta.PendienteRRHH;
                    }
                    else
                    {
                        value = (byte)idsEstadoSolicitudAlta.PendienteDocs;
                    }

                    if (!entity.hasAltaSS || operation_fechaIni != null)
                    {
                        var llamamiento = entity.idSolicitudAltaNavigation;

                        // TODO: ELIMINAR CONTEMPLACIÓN DE POSIBLE CONTRATO EXISTENTE AL TERMINAR PROCESO DE INTEGRACIÓN DE LA PANTALLA
                        var contrato = db.tblPersonaNTipoContrato.FirstOrDefault(x => x.idPersona == llamamiento.idPersona && x.fechaBajaContrato == null);
                        if (contrato != null)
                        {
                            db.tblPersonaNTipoContrato.Remove(contrato);
                        }
                        db.tblPersonaNTipoContrato.Add(new tblPersonaNTipoContrato
                        {
                            idPersona = (int)llamamiento.idPersona,
                            idTipoContrato = (byte)llamamiento.idTipoContrato,
                            fechaAltaContrato = llamamiento.fechaIni,
                            numDiasPeriodoPrueba = llamamiento.numDiasPeriodoPrueba,
                        });

                        entity.hasAltaSS = true;
                        entity.isUrgente = false;

                        enviarMail = TipoMail.AltaSS;
                    }
                }

                if (value == (byte)idsEstadoSolicitudAlta.Anulada)
                {
                    var llamamiento = entity.idSolicitudAltaNavigation;
                    llamamiento.activo = false;
                    var contrato = db.tblPersonaNTipoContrato.FirstOrDefault(x => x.idPersona == llamamiento.idPersona && x.fechaAltaContrato == llamamiento.fechaIni);
                    if (contrato != null)
                    {
                        db.tblPersonaNTipoContrato.Remove(contrato);
                    }
                }

                if (value == (byte)idsEstadoSolicitudAlta.Validado)
                {
                    entity.idUsuario_validacion = idUsuario;
                    entity.fecha_validacion = DateTimeOffset.Now;
                }

                if (entity.idEstadoSolicitudAlta != value)
                {
                    entity.idEstadoSolicitudAlta = value;
                    entity.tblEstadoSolicitudAltaNSolicitudAlta.Add(new tblEstadoSolicitudAltaNSolicitudAlta
                    {
                        idEstadoSolicitudAlta = entity.idEstadoSolicitudAlta,
                        idUsuario = idUsuario,
                        fecha = DateTimeOffset.Now,
                    });

                    enviarMail = (idsEstadoSolicitudAlta)value switch
                    {
                        idsEstadoSolicitudAlta.PendienteRRHH => TipoMail.AltaValidada,
                        idsEstadoSolicitudAlta.PendienteDocs => TipoMail.AltaValidada,
                        idsEstadoSolicitudAlta.PetAnulacion => TipoMail.PetAnulacion,
                        idsEstadoSolicitudAlta.Anulada => TipoMail.Anulada,
                        _ => null,
                    };
                }

                solicitudGestoria.Operations.Remove(operation_idEstadoSolicitudAlta);
            }

            // Se modifica el estado el crear una solicitud cambio RRHH / Gestoría
            var operation_tblEstadoSolicitudAltaNSolicitudAlta = solicitudGestoria.Operations.FirstOrDefault(x => x.path == "/tblEstadoSolicitudAltaNSolicitudAlta");
            if (operation_tblEstadoSolicitudAltaNSolicitudAlta != null)
            {
                var value = JsonConvert.DeserializeObject<List<tblEstadoSolicitudAltaNSolicitudAlta>>(operation_tblEstadoSolicitudAltaNSolicitudAlta.value.ToString());

                foreach (var item in value)
                {
                    item.idUsuario = idUsuario;
                    item.fecha = DateTimeOffset.Now;
                    entity.idEstadoSolicitudAlta = item.idEstadoSolicitudAlta;
                    entity.tblEstadoSolicitudAltaNSolicitudAlta.Add(item);

                    switch ((idsEstadoSolicitudAlta)item.idEstadoSolicitudAlta)
                    {
                        case idsEstadoSolicitudAlta.SolicitudCambioRRHH:
                            if (!objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.SolicitudesAltaGestoria) && objUsuario.idCargo != (short)idsCargo.Desarrollador)
                                return BadRequest("No tienes permisos para solicitar cambios de RRHH");
                            break;
                        case idsEstadoSolicitudAlta.SolicitudCambioGestoria:
                            if (!objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.SolicitudesAltaRRHH) && objUsuario.idCargo != (short)idsCargo.Desarrollador)
                                return BadRequest("No tienes permisos para solicitar cambios de Gestoría");
                            break;
                        default:
                            break;
                    }

                    enviarMail = item.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.EnProceso ? TipoMail.IncidenciaSolucionada : TipoMail.IncidenciaCreada;
                }

                solicitudGestoria.Operations.Remove(operation_tblEstadoSolicitudAltaNSolicitudAlta);
            }

            solicitudGestoria.ApplyTo(entity);
            await db.SaveChangesAsync();

            switch (enviarMail)
            {
                case TipoMail.AltaValidada:
                    await notificaciones.SendAvisos_AltaValidada(entity);
                    break;
                case TipoMail.AltaSS:
                    await notificaciones.SendAvisos_AltaSS(entity);
                    break;
                case TipoMail.IncidenciaCreada:
                    await notificaciones.SendAvisos_AltaIncidenciaCreada(entity, entity.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.SolicitudCambioRRHH);
                    break;
                case TipoMail.IncidenciaSolucionada:
                    await notificaciones.SendAvisos_AltaIncidenciaSolucionada(entity, entity.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.SolicitudCambioRRHH);
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return Ok(true);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = db.tblSolicitudAlta
            .Include(x => x.tblDocumentoNSolicitudAlta)
            .Include(x => x.tblEstadoSolicitudAltaNSolicitudAlta)
            .FirstOrDefault(x => x.idSolicitudAlta == key);

        if (entity == null)
            return false;

        db.tblDocumentoNSolicitudAlta.RemoveRange(entity.tblDocumentoNSolicitudAlta);
        db.tblEstadoSolicitudAltaNSolicitudAlta.RemoveRange(entity.tblEstadoSolicitudAltaNSolicitudAlta);

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }

    private class DocumentoNAltaSolicitud
    {
        public int? idDocumentoNSolicitudAlta { get; set; }
        public byte idTipoDocumentoNSolicitudAlta { get; set; }
        public string denominacion { get; set; } = null!;
        public string extension { get; set; } = null!;
        public byte[] documento { get; set; } = null!;
        public DateTimeOffset fecha { get; set; }
        public int idUsuario { get; set; }
        public bool isFromGestoria { get; set; }
    }
}