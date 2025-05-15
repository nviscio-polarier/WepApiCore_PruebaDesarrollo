//using System.Text.RegularExpressions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Formatter;
//using Microsoft.AspNetCore.OData.Query;
//using Microsoft.AspNetCore.OData.Routing.Controllers;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using WebApiCore.Context;
//using WebApiCore.Hubs;
//using WebApiCore.Services.MyRealBonus;

//namespace WebApiCore.Controllers.Proyectos.MyRealLearning
//{
//    public class tblVideoController : ODataController
//    {


//        private readonly bdERP db;
//        private readonly IHubContext<NotificacionesHub> _hubContext;
//        private readonly PersonaVideoService _personaVideoService;
//        public tblVideoController(bdERP context, IHubContext<NotificacionesHub> hubContext, PersonaVideoService personaVideoService)
//        {
//            db = context;
//            _hubContext = hubContext;
//            _personaVideoService = personaVideoService;
//        }


//        // GET //
//        /// <summary>
//        /// Devuelve los videos con toda su información.
//        /// </summary>
//        [EnableQuery]
//        [HttpGet("odata/getVideos")]
//        public async Task<ActionResult> getVideos()
//        {
//            try
//            {
//                var videos = await db.tblVideo
//                    .Select(v => new
//                    {
//                        v.idVideo,
//                        v.titulo,
//                        v.descripcion,
//                        v.duracionSegundos,
//                        v.fechaSubida,
//                        v.youtubeId,
//                        v.url,
//                        v.idCategoria,
//                        CategoriaDenominacion = v.idCategoriaNavigation.denominacion,
//                        Etiquetas = v.idEtiqueta.Select(e => new
//                        {
//                            e.idEtiqueta,
//                            e.denominacion
//                        }).ToList()
//                    })
//                    .ToListAsync();

//                return Ok(videos);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al obtener los videos: {ex.Message}");
//            }
//        }


//        /// <summary>
//        /// Devuelve la información de los videos filtrados por categoría.
//        /// </summary>
//        [EnableQuery]
//        [HttpGet("odata/getVideosPorCategoria")]
//        public async Task<ActionResult> getVideosPorCategoria([FromODataUri] int idCategoria)
//        {
//            try
//            {
//                var videos = await db.tblVideo
//                    .Where(v => v.idCategoria == idCategoria)
//                    .Select(v => new
//                    {
//                        v.idVideo,
//                        v.titulo,
//                        v.descripcion,
//                        v.duracionSegundos,
//                        v.fechaSubida,
//                        v.youtubeId,
//                        v.url,
//                        CategoriaDenominacion = v.idCategoriaNavigation.denominacion,
//                        Etiquetas = v.idEtiqueta.Select(e => new
//                        {
//                            e.idEtiqueta,
//                            e.denominacion
//                        }).ToList()
//                    })
//                    .ToListAsync();

//                return Ok(videos);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al obtener los videos: {ex.Message}");
//            }

//        }
//        /// <summary>
//        /// Devuelve todas las categorías que existen.
//        /// </summary>
//        [EnableQuery]
//        [HttpGet("odata/getCategorias")]
//        public async Task<ActionResult> getCateogiras()
//        {
//            try
//            {
//                var categorias =
//                    await db.tblCategoriasVideos
//                    .Select(c => new
//                    {
//                        c.idCategoria,
//                        c.denominacion
//                    })
//                    .ToListAsync();

//                return Ok(categorias);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al obtener los videos: {ex.Message}");
//            }

//        }


//        // GET //
//        /// <summary>
//        /// Devuelve todas las etiquetas.
//        /// </summary>
//        [EnableQuery]
//        [HttpGet("odata/getEtiquetas")]
//        public async Task<ActionResult> getEtiquetas()
//        {
//            try
//            {
//                var etiquetas = await db.tblEtiquetas
//                    .Select(e => new
//                    {
//                        e.idEtiqueta,
//                        e.denominacion
//                    })
//                    .ToListAsync();

//                return Ok(etiquetas);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al obtener las etiquetas: {ex.Message}");
//            }
//        }

//        // GET //
//        /// <summary>
//        /// Devuelve las etiquetas de un video.
//        /// </summary>
//        [EnableQuery]
//        [HttpGet("odata/getVideosPorEtiqueta")]
//        public async Task<ActionResult> getVideosPorEtiqueta([FromODataUri] int idEtiqueta)
//        {
//            try
//            {
//                var videos = await db.tblVideo
//                    .Where(v => v.idEtiqueta.Any(e => e.idEtiqueta == idEtiqueta))
//                    .Select(v => new
//                    {
//                        v.idVideo,
//                        v.titulo,
//                        v.youtubeId,
//                        v.descripcion,
//                        v.url,
//                        v.duracionSegundos,
//                        v.fechaSubida,
//                        v.idCategoria,
//                        CategoriaDenominacion = v.idCategoriaNavigation.denominacion
//                    })
//                    .ToListAsync();

//                return Ok(videos);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al obtener los videos: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Devuelve la información de un video concreto.
//        /// </summary>
//        [HttpGet("odata/getVideoPorId")]
//        public async Task<ActionResult> getVideoPorId([FromODataUri] int idVideo)
//        {
//            var video = await db.tblVideo
//                .Where(v => v.idVideo == idVideo)
//                .Select(v => new
//                {
//                    v.idVideo,
//                    v.titulo,
//                    v.descripcion,
//                    v.youtubeId,
//                    v.url,
//                    v.duracionSegundos,
//                    v.fechaSubida,
//                    v.idCategoria,
//                    CategoriaDenominacion = v.idCategoriaNavigation.denominacion,
//                    Etiquetas = v.idEtiqueta.Select(e => e.denominacion).ToList()
//                })
//                .FirstOrDefaultAsync();

//            if (video == null)
//                return NotFound("Video no encontrado");

//            return Ok(video);
//        }

//        /// <summary>
//        /// Devuelve las etiquetas de un video.
//        /// </summary>
//        [EnableQuery]
//        [HttpGet("odata/getEtiquetasVideo")]
//        public async Task<ActionResult> getEtiquetasVideo([FromODataUri] int idVideo)
//        {
//            try
//            {
//                var etiquetas = await db.tblVideo
//                    .Where(v => v.idVideo == idVideo)
//                    .SelectMany(v => v.idEtiqueta.Select(e => new
//                    {
//                        e.idEtiqueta,
//                        e.denominacion
//                    }))
//                    .ToListAsync();

//                return Ok(etiquetas);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al obtener las etiquetas del video: {ex.Message}");
//            }
//        }



//        // CUD Etiquetas

//        /// <summary>
//        /// Crea una etiqueta.
//        /// </summary>
//        [HttpPost("odata/crearEtiqueta")]
//        public async Task<IActionResult> addEtiqueta([FromBody] tblEtiquetas etiqueta)
//        {
//            db.tblEtiquetas.Add(etiqueta);
//            await db.SaveChangesAsync();
//            return Ok(etiqueta);
//        }

//        /// <summary>
//        /// Actualiza una etiqueta.
//        /// </summary>
//        [HttpPut("odata/actualizarEtiqueta")]
//        public async Task<IActionResult> actualizarEtiqueta([FromODataUri] int idEtiqueta, [FromBody] tblEtiquetas tblEtiquetas)
//        {
//            var etiquetaActualizar = await db.tblEtiquetas.FindAsync(idEtiqueta);
//            if (etiquetaActualizar == null) return NotFound();
//            etiquetaActualizar.denominacion = tblEtiquetas.denominacion;
//            await db.SaveChangesAsync();
//            return Ok(etiquetaActualizar.denominacion);
//        }

//        /// <summary>
//        /// Borra una etiqueta.
//        /// </summary>
//        [HttpDelete("odata/eliminarEtiqueta")]
//        public async Task<IActionResult> eliminarEtiqueta([FromODataUri] int idEtiqueta)
//        {
//            var etiqueta = await db.tblEtiquetas
//               .Include(e => e.idVideo)
//               .FirstOrDefaultAsync(e => e.idEtiqueta == idEtiqueta);
//            if (etiqueta == null)
//                return NotFound($"Etiqueta con ID {idEtiqueta} no encontrada.");

//            // Verificar si tiene videos asociados si tiene no se borra

//            if (etiqueta.idVideo.Any())
//                return BadRequest("No se puede eliminar la etiqueta porque está asociada a uno o más videos.");

//            db.tblEtiquetas.Remove(etiqueta);
//            await db.SaveChangesAsync();

//            return Ok("Borrada.");
//        }

//        // CUD Categorias

//        /// <summary>
//        /// Crea una categoira.
//        /// </summary>
//        [HttpPost("odata/addCategoria")]
//        public async Task<IActionResult> addCategoria([FromBody] tblCategoriasVideos cat)
//        {
//            db.tblCategoriasVideos.Add(cat);
//            await db.SaveChangesAsync();
//            return Ok(cat);
//        }
//        /// <summary>
//        /// Actualiza una categoria.
//        /// </summary>
//        [HttpPut("odata/actualizarCategoria")]

//        /// <summary>
//        /// Elimina una categoría.
//        /// </summary>
//        public async Task<IActionResult> actualizarCategoria([FromODataUri] int idCategoria, [FromBody] tblCategoriasVideos tblCategoriasVideos)
//        {
//            var categoriaActualizar = await db.tblCategoriasVideos.FindAsync(idCategoria);
//            if (categoriaActualizar == null) return NotFound();
//            categoriaActualizar.denominacion = tblCategoriasVideos.denominacion;
//            await db.SaveChangesAsync();
//            return Ok(categoriaActualizar.denominacion);
//        }
//        [HttpDelete("odata/eliminarCategoria")]
//        public async Task<IActionResult> eliminarCategoria([FromODataUri] int idCategoria)
//        {
//            var categoria = await db.tblCategoriasVideos
//                .FirstOrDefaultAsync(c => c.idCategoria == idCategoria);

//            if (categoria == null)
//                return NotFound("Categoría no encontrada.");

//            //No se borra si e está en uso

//            bool tieneVideos = await db.tblVideo.AnyAsync(v => v.idCategoria == idCategoria);
//            if (tieneVideos)
//                return BadRequest("No se puede eliminar la categoría porque tiene videos asociados.");

//            db.tblCategoriasVideos.Remove(categoria);
//            await db.SaveChangesAsync();

//            return Ok("Borrada.");
//        }



//        /// <summary>
//        /// Crea un video. La duración del video y el id de youtube son datos derivados que se calculan desde MyPolarier.
//        /// Se puede probar desde postman usando una duracion, la id aun así se calcula sola también.
//        /// Para un ejemplo completo revisar postman.
//        /// </summary>
//        [HttpPost("odata/crearVideo")]

//        public async Task<IActionResult> addVideo([FromBody] VideoConEtiquetasDTO dto)
//        {

//            if (string.IsNullOrEmpty(dto.titulo) ||
//                string.IsNullOrEmpty(dto.url) ||
//                !dto.idCategoria.HasValue ||
//                dto.idsEtiquetas == null || !dto.idsEtiquetas.Any())
//            {
//                return BadRequest("Faltan campos obligatorios para crear el video.");
//            }

//            if (!await db.tblCategoriasVideos.AnyAsync(c => c.idCategoria == dto.idCategoria.Value))
//                return BadRequest("La categoría proporcionada no existe.");

//            var etiquetas = await db.tblEtiquetas
//                .Where(e => dto.idsEtiquetas.Contains(e.idEtiqueta))
//                .ToListAsync();

//            if (etiquetas.Count != dto.idsEtiquetas.Count)
//                return BadRequest("Una o más etiquetas no existen.");

//            var video = new tblVideo
//            {
//                titulo = dto.titulo,
//                descripcion = dto.descripcion,
//                url = dto.url,
//                youtubeId = _personaVideoService.getYoutubeID(dto.url),
//                duracionSegundos = dto.duracionSegundos.Value,
//                fechaSubida = dto.fechaSubida ?? DateTime.Now,
//                idCategoria = dto.idCategoria.Value,
//                idEtiqueta = etiquetas
//            };

//            db.tblVideo.Add(video);
//            await db.SaveChangesAsync();

//            return Ok(new
//            {
//                message = "Video insertado con exito.",
//                idVideo = video.idVideo,
//                youtubeId = video.youtubeId,
//                duracion = video.duracionSegundos
//            });
//        }

//        /// <summary>
//        /// Actualiza la información de un video, si la URL cambia se recalculan la duración del video y del ID (Falta por probar).
//        /// </summary>
//        [HttpPut("odata/actualizarVideo")]
//        public async Task<IActionResult> actualizarVideo([FromBody] VideoConEtiquetasDTO dto)
//        {
//            var video = await db.tblVideo
//                .Include(v => v.idEtiqueta)
//                .FirstOrDefaultAsync(v => v.idVideo == dto.idVideo);

//            if (video == null) return NotFound();

//            if (dto.titulo != null) video.titulo = dto.titulo;
//            if (dto.descripcion != null) video.descripcion = dto.descripcion;

//            // Si la URL ha cambiado, vuelve a checkear la duracion.

//            if (dto.fechaSubida.HasValue) video.fechaSubida = dto.fechaSubida.Value;
//            if (dto.idCategoria.HasValue) video.idCategoria = dto.idCategoria.Value;

//            if (dto.idsEtiquetas != null)
//            {
//                var nuevas = await db.tblEtiquetas
//                    .Where(e => dto.idsEtiquetas.Contains(e.idEtiqueta))
//                    .ToListAsync();

//                video.idEtiqueta.Clear();
//                foreach (var et in nuevas)
//                    video.idEtiqueta.Add(et);
//            }

//            await db.SaveChangesAsync();
//            return Ok(new
//            {
//                message = "Video actualizado correctamente.",
//                idVideo = video.idVideo
//            });
//        }



//        // DTO's
//        public class VideoConEtiquetasDTO
//        {
//            public int idVideo { get; set; }

//            public string? titulo { get; set; }
//            public string? descripcion { get; set; }
//            public string? url { get; set; }
//            public string? youtubeId { get; set; }
//            public int? duracionSegundos { get; set; }
//            public DateTime? fechaSubida { get; set; }
//            public int? idCategoria { get; set; }

//            public List<int>? idsEtiquetas { get; set; }
//        }




//    }
//}

