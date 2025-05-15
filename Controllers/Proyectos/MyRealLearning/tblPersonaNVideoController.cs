//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Formatter;
//using Microsoft.AspNetCore.OData.Query;
//using Microsoft.AspNetCore.OData.Routing.Controllers;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using WebApiCore.Context;
//using WebApiCore.Enums.MyRealLearning;
//using WebApiCore.Hubs;
//using WebApiCore.Services.MyRealBonus;

//namespace WebApiCore.Controllers.Proyectos.MyRealLearning
//{
//    public class tblPersonaNVideoController : ODataController
//    {


//        private readonly bdERP db;
//        private readonly IHubContext<NotificacionesHub> _hubContext;
//        private readonly PersonaVideoService _personaVideoService;


//        public tblPersonaNVideoController(bdERP context, IHubContext<NotificacionesHub> hubContext, PersonaVideoService personaVideoService)
//        {
//            db = context;
//            _hubContext = hubContext;
//            _personaVideoService = personaVideoService;
//        }


//        /// <summary>
//        /// Devuelve la información de los videos que una persona tiene asignados.
//        /// </summary>
//        [EnableQuery]
//        [HttpGet("odata/getVideosPorPersona")]
//        public async Task<ActionResult> getVideosPorPersona([FromODataUri] int idPersona)
//        {
//            try
//            {
//                var videos = await db.tblVideoNPersona
//                    .Where(pv => pv.idPersona == idPersona)
//                    .Include(pv => pv.idVideoNavigation)
//                    .Select(pv => new
//                    {
//                        pv.idVideo,
//                        pv.idPersona,
//                        pv.idPersonaVideo,
//                        pv.idEstado,
//                        pv.comentario,
//                        DenominacionEstado = pv.idEstadoNavigation.denominacion,
//                        pv.idVideoNavigation.titulo,
//                        pv.idVideoNavigation.descripcion,
//                        pv.idVideoNavigation.youtubeId,
//                        pv.idVideoNavigation.duracionSegundos,
//                        pv.idVideoNavigation.fechaSubida,
//                        pv.idVideoNavigation.idCategoria,
//                        pv.fechaEnvio
//                    })
//                    .ToListAsync();

//                return Ok(videos);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al obtener los videos asignados: {ex.Message}");
//            }
//        }


//        /// <summary>
//        /// Asigna videos a personas utilizando una lista de asignaciones.
//        /// Es una lista de objetos, donde cada objeto individual representa una asignación a una o N personas.
//        ///  [
//            ///  {
//            ///    "idVideo": 1,
//            ///    "idsPersonas": [1392],
//            ///    "comentario": "Mirate este video."
//            ///  },
//            ///  {
//            ///    "idVideo": 2,
//            ///    "idsPersonas": [1392],
//            ///    "comentario": ""
//            ///  },
//            ///  {
//            ///    "idVideo": 3,
//            ///    "idsPersonas": [1392, 5747],
//            ///    "comentario": "Hola."
//            ///  }
//            ///]

//        [HttpPost("odata/asignarVideo")]
//        public async Task<List<int>> AsignarVideos(
//        [FromBody] List<AsignacionVideoMultipleDTO> asignacionesDto,
//        [FromQuery] int remitente
//        )

//        {

            

//            var asignaciones = new List<tblVideoNPersona>();
//            var notificaciones = new List<tblNotificacionNPersonaNVideo>();
//            var idsAsignaciones = new List<int>();



//            foreach (var dto in asignacionesDto)
//            {
//                foreach (var idPersona in dto.idsPersonas)
//                {
//                    var asignacion = new tblVideoNPersona
//                    {
//                        idPersona = idPersona,
//                        idVideo = dto.idVideo,
//                        idPersonaRemitente = remitente,
//                        fechaEnvio = DateTime.Now,
//                        idEstado = (int)EstadoVideo.Asignado,
//                        comentario = dto.comentario
//                    };

//                    asignaciones.Add(asignacion);
//                }
//            }

//            // Guardar todas las asignaciones
//            db.tblVideoNPersona.AddRange(asignaciones);
//            await db.SaveChangesAsync();

//            foreach (var asignacion in asignaciones)
//            {
//                await _personaVideoService.logEventoVideo(new EventoVideoDTO
//                {
//                    idPersonaVideo = asignacion.idPersonaVideo,
//                    idTipoEvento = (int)TipoEventoMyRealLearning.VideoEnviado,
//                    idEstadoNuevo = (int)EstadoVideo.Asignado,
//                    actorId = remitente
//                });

//                //    // TODO >:(
//                //    notificaciones.Add(new tblNotificacionNPersonaNVideo
//                //    {
//                //        idNotificacion = TODO
//                //        idPersonaVideo = asignacion.idPersonaVideo
//                //    });

//                //    idsAsignaciones.Add(asignacion.idPersonaVideo);
//            }

//            //db.tblNotificacionNPersonaNVideo.AddRange(notificaciones);
//            await db.SaveChangesAsync();

//            return idsAsignaciones;
//        }


//        /// <summary>
//        /// Registra desde el móvil una sesión de visualización (Cuando el usuario abre, visualiza, cierra el video desde su pantalla).
//        /// Y registra los logs correspondientes.
//        /// </summary>
//        [HttpPost("odata/registrarSesionVisualizacion")]
//        public async Task<IActionResult> logSesionVisualizacion([FromBody] SesionVisualizacionDTO dto)
//        {
//            var asignacion = await db.tblVideoNPersona.FindAsync(dto.idPersonaVideo);
//            if (asignacion == null)
//                return NotFound("Asignación no encontrada.");

//            var fechaARegistrar = DateTime.Now;
//            bool esFinalizado = dto.finalizadoYoutube || dto.finalizadoManual;

//            // Registrar la sesión de visualización 
//            db.tblSesionesVisualizacion.Add(new tblSesionesVisualizacion
//            {
//                idPersonaVideo = dto.idPersonaVideo,
//                fechaInicio = fechaARegistrar,
//                segundosVisualizados = dto.segundosVisualizados,
//                finalizadoYoutube = dto.finalizadoYoutube,
//                finalizadoManual = dto.finalizadoManual
//            });

//            // Si la asignación aún NO está finalizada y esta sesión la finaliza, estadp = finalizado
//            if (asignacion.idEstado != (int)EstadoVideo.Finalizado && esFinalizado)
//            {
//                asignacion.fechaFinalizado = fechaARegistrar;
//                asignacion.idEstado = (int)EstadoVideo.Finalizado;

//                await _personaVideoService.logEventoVideo(new EventoVideoDTO
//                {
//                    idPersonaVideo = dto.idPersonaVideo,
//                    idTipoEvento = dto.finalizadoYoutube
//                        ? (int)TipoEventoMyRealLearning.FinalizadoAutomaticamente
//                        : (int)TipoEventoMyRealLearning.MarcadoComoFinalizado,
//                    idEstadoAnterior = (int)EstadoVideo.Abierto,
//                    idEstadoNuevo = (int)EstadoVideo.Finalizado,
//                    actorId = asignacion.idPersona ?? 0
//                });
//            }

//            // Log de sesión de visualización 
//            await _personaVideoService.logEventoVideo(new EventoVideoDTO
//            {
//                idPersonaVideo = dto.idPersonaVideo,
//                idTipoEvento = (int)TipoEventoMyRealLearning.SesionVisualizacion,
//                idEstadoNuevo = asignacion.idEstado ?? (int)EstadoVideo.Abierto,
//                actorId = asignacion.idPersona ?? 0
//            });

//            await db.SaveChangesAsync();
//            return Ok("Sesión registrada correctamente.");
//        }


//        /// <summary>
//        /// Devuelve una lista de las asignaciones de un video.
//        /// </summary>
//        [EnableQuery]
//        [HttpGet("odata/getAsignacionesPorVideo")]
//        public async Task<IActionResult> getAsignacionesPorVideo([FromODataUri] int idVideo)
//        {
//            try
//            {



//                var asignaciones = await db.tblVideoNPersona
//                    .Where(a => a.idVideo == idVideo)
//                    .Select(a => new
//                    {
//                        a.idPersonaVideo,
//                        a.idPersona,
//                        a.idEstado,
//                        Estado = a.idEstadoNavigation.denominacion,
//                        a.fechaEnvio,
//                        a.fechaAbierto,
//                        a.fechaFinalizado,
//                        PersonaNombre = a.idPersonaNavigation.nombre,
//                        PersonaApellidos = a.idPersonaRemitenteNavigation.apellidos,
//                        Lavanderia = a.idPersonaNavigation.idLavanderiaNavigation.denominacion,
//                        Remitente = a.idPersonaRemitenteNavigation.nombre,
//                        PorcentajeVisto = db.tblSesionesVisualizacion
//                            .Where(s => s.idPersonaVideo == a.idPersonaVideo && s.segundosVisualizados != null)
//                            .Max(s => (double?)s.segundosVisualizados) / (a.idVideoNavigation.duracionSegundos == 0 ? 1 : a.idVideoNavigation.duracionSegundos) * 100.0

//                    })


//                    .ToListAsync();

//                return Ok(asignaciones);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al obtener asignaciones: {ex.Message}");
//            }
//        }


//        /*
         
//    idEstado	denominacion
//        1	Asignado
//        2	Cancelado
//        3	Abierto
//        5	Finalizado
//         */

//        [HttpPut("odata/updateAsignacion")]
//        public async Task<IActionResult> updateAsignacion([FromODataUri] int idPersonaVideo, [FromODataUri] int idEstadoNuevo)
//        {
//            var asignacion = await db.tblVideoNPersona.FindAsync(idPersonaVideo);
//            if (asignacion == null)
//                return NotFound("Asignación no encontrada.");

//            var estadoAnterior = asignacion.idEstado;

//            // Si se abre por primera vez
//            if (idEstadoNuevo == (int)EstadoVideo.Abierto && asignacion.fechaAbierto == null)
//            {
//                asignacion.fechaAbierto = DateTime.Now;
//            }

//            // Si se finaliza
//            if (idEstadoNuevo == (int)EstadoVideo.Finalizado)
//            {
//                asignacion.fechaFinalizado = DateTime.Now;
//            }

//            if (estadoAnterior == (int)EstadoVideo.Finalizado && idEstadoNuevo != (int)EstadoVideo.Finalizado)
//            {
//                return BadRequest("La asignación ya está finalizada. No se puede cambiar a otro estado.");
//            }

//            asignacion.idEstado = idEstadoNuevo;

//            await _personaVideoService.logEventoVideo(new EventoVideoDTO
//            {
//                idPersonaVideo = idPersonaVideo,
//                idTipoEvento = (int)TipoEventoMyRealLearning.EstadoActualizado,
//                idEstadoAnterior = estadoAnterior,
//                idEstadoNuevo = idEstadoNuevo,
//                actorId = asignacion.idPersona ?? 0
//            });

//            await db.SaveChangesAsync();
//            return Ok(new { message = "Estado actualizado correctamente." });
//        }

//        /// <summary>
//        /// Devuelve el historial de eventos en relación a una asignación.
//        /// </summary>
//        [HttpGet("odata/getHistorialAsignacion")]
//        public async Task<ActionResult> getHistorialAsignacion([FromODataUri] int idAsignacion)
//        {
//            try
//            {
//                var historico = await db.tblEventoMyRealLearning
//                    .Where(e => e.idPersonaVideo == idAsignacion)
//                    .OrderBy(e => e.fecha)
//                    .Select(e => new
//                    {
//                        e.fecha,
//                        e.idTipoEventoNavigation.denominacion,
//                        EstadoAnterior = e.idEstadoAnterior != null ? db.tblEstadosVideo.FirstOrDefault(z => z.idEstado == e.idEstadoAnterior).denominacion : null,
//                        EstadoNuevo = e.idEstadoNuevo != null ? db.tblEstadosVideo.FirstOrDefault(z => z.idEstado == e.idEstadoNuevo).denominacion : null,
//                        e.actor,
//                        ActorNombre = db.tblPersona.FirstOrDefault(n => n.idPersona == e.actor).nombre,
//                        e.comentario



//                    })
//                    .ToListAsync();
//                return Ok(historico);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al obtener los videos asignados: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Devuelve las sesiones de visualización asociadas a una asignación.
//        /// </summary>
//        [HttpGet("odata/getSesionesAsignacion")]
//        public async Task<ActionResult> getSesionesAsignacion([FromODataUri] int idAsignacion)
//        {
//            try
//            {
//                var sesiones = await db.tblSesionesVisualizacion
//                    .Where(e => e.idPersonaVideo == idAsignacion)
//                    .OrderBy(e => e.fechaInicio)
//                    .Select(e => new
//                    {
//                        e.idSesion,
//                        e.fechaInicio,
//                        e.segundosVisualizados,
//                        e.finalizadoYoutube

//                    })
//                    .ToListAsync();


//                return Ok(sesiones);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al obtener los videos asignados: {ex.Message}");
//            }
//        }

//        //[HttpGet("odata/getSesionesAsignacion")]
//        //public async Task<ActionResult> getMaximoVistoVideo([FromODataUri] int idAsignacion)
//        //{
//        //    try
//        //    {
//        //        var sesiones = await db.tblSesionesVisualizacion
//        //            .Where(e => e.idPersonaVideo == idAsignacion)
//        //            .OrderBy(e => e.fechaInicio)
//        //            .Select(e => new
//        //            {
//        //                e.idSesion,
//        //                e.fechaInicio,
//        //                e.segundosVisualizados,
//        //                e.finalizadoYoutube

//        //            })
//        //            .ToListAsync();


//        //        return Ok(sesiones).Max();
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return StatusCode(500, $"Error al obtener los videos asignados: {ex.Message}");
//        //    }
//        //}


//        /// <summary>
//        /// Devuelve un histórico de eventos y un agregado de los datos a partir de una asignación.
//            ///        {
//            ///"idPersonaVideo": 1,
//            ///"idEstado": 3,
//            ///"Estado": "Abierto",
//            ///"fechaAbierto": "2025-05-07T10:26:24.06",
//            ///"fechaFinalizado": "2025-05-12T08:25:03.517",
//            ///"DuracionVideoSegundos": 240,
//            ///"MaximoSegundosVisto": 0.0,
//            ///"PorcentajeVisto": 0.0,
//            ///"TotalSesiones": 10,
//            ///"Eventos": [
//            ///    {...  }
//        /// </summary>
//        [HttpGet("odata/getResumenAsignacion")]
//        public async Task<ActionResult> getResumenAsignacion([FromODataUri] int idPersonaVideo)
//        {
//            var asignacion = await db.tblVideoNPersona
//                .Include(a => a.idVideoNavigation)
//                .Include(a => a.idEstadoNavigation)
//                .FirstOrDefaultAsync(a => a.idPersonaVideo == idPersonaVideo);

//            if (asignacion == null)
//                return NotFound("Asignación no encontrada.");

//            var sesiones = await db.tblSesionesVisualizacion
//                .Where(s => s.idPersonaVideo == idPersonaVideo)
//                .OrderBy(s => s.fechaInicio)
//                .ToListAsync();

//            var eventos = await db.tblEventoMyRealLearning
//                .Where(e => e.idPersonaVideo == idPersonaVideo)
//                .OrderBy(e => e.fecha)
//                .Select(e => new
//                {
//                    e.fecha,
//                    TipoEvento = e.idTipoEventoNavigation.denominacion,
//                    EstadoAnterior = e.idEstadoAnterior,
//                    EstadoAnteriorDeno = db.tblEstadosVideo
//                        .Where(z => z.idEstado == e.idEstadoAnterior)
//                        .Select(z => z.denominacion)
//                        .FirstOrDefault(),
//                    EstadoNuevo = e.idEstadoNuevo,
//                    EstadoNuevoDeno = db.tblEstadosVideo
//                        .Where(z => z.idEstado == e.idEstadoNuevo)
//                        .Select(z => z.denominacion)
//                        .FirstOrDefault(),
//                    e.comentario
//                })
//                .ToListAsync();

//            var maxVisualizado = sesiones
//                .Where(s => s.segundosVisualizados.HasValue)
//                .Max(s => s.segundosVisualizados) ?? 0;

//            var porcentajeVisto = asignacion.idVideoNavigation.duracionSegundos > 0
//                ? maxVisualizado / asignacion.idVideoNavigation.duracionSegundos * 100
//                : 0;

//            return Ok(new
//            {
//                asignacion.idPersonaVideo,
//                asignacion.idEstado,
//                Estado = asignacion.idEstadoNavigation.denominacion,
//                asignacion.fechaAbierto,
//                asignacion.fechaFinalizado,
//                DuracionVideoSegundos = asignacion.idVideoNavigation.duracionSegundos,
//                MaximoSegundosVisto = maxVisualizado,
//                PorcentajeVisto = Math.Round(porcentajeVisto, 2),
//                TotalSesiones = sesiones.Count,
//                Eventos = eventos
//            });
//        }



//    }


//    public class AsignacionVideoMultipleDTO
//    {
//        public int idVideo { get; set; }
//        public List<int> idsPersonas { get; set; }
//        public string comentario { get; set; }
//    }

//    public class EventoVideoDTO
//    {
//        public int idPersonaVideo { get; set; }
//        public int idTipoEvento { get; set; }
//        public int idEstadoNuevo { get; set; }
//        public int actorId { get; set; }
//        public int? idEstadoAnterior { get; set; }
//        public string? comentario { get; set; }
//    }

//    public class SesionVisualizacionDTO
//    {
//        public int idPersonaVideo { get; set; }
//        public float? segundosVisualizados { get; set; }
//        public bool finalizadoYoutube { get; set; }
//        public bool finalizadoManual { get; set; }
//    }


//}