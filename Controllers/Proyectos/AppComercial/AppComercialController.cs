using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using AuthorizeAttribute = WebApiCore.Security.AuthorizeAttribute;

namespace WebApiCore.Controllers;

public class AppComercialController : ODataController
{
    private readonly bdERP db;

    public AppComercialController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost("odata/AppComercial/incidencia")]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblIncidencia incidencia)
    {
        try
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            incidencia.fechaRegistro = DateTime.Now; //UTC

            var resultParameter = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Output,
                Size = 8
            };

            db.Database.ExecuteSqlRaw("SET @result = (SELECT Incidencias.EF_funCodigoIncidencia_AppComercial());", resultParameter).ToString();
            incidencia.codigo = (string)resultParameter.Value;
            incidencia.denominacion = "";
            incidencia.estadoMaquinaInicial = incidencia.estadoMaquina;
            incidencia.idUsuarioCrea = idUsuario;

            db.tblIncidencia.Add(incidencia);
            await db.SaveChangesAsync();

            return Ok(incidencia.idIncidencia);
        }
        catch
        {
            return BadRequest("Error de BDD");
        }
    }

    [EnableQuery]
    [HttpGet("odata/AppComercial/getEncuestaPlantilla")]
    [Authorize]
    public async Task<ActionResult> getEncuestaPlantilla([FromODataUri] int? idEntidad, [FromODataUri] int? idEncuesta)
    {
        var encuestaPlantilla = db.tblEncuestaPlantilla.OrderByDescending(x => x.idEncuestaPlantilla).FirstOrDefault(x => x.idTipoEncuesta == 2);
        var entidad = db.tblEntidad.Include(x => x.idTipoAlmacenajeLimpioNavigation).FirstOrDefault(x => x.idEntidad == idEntidad);

        if (idEncuesta != null)
        {
            encuestaPlantilla = db.tblEncuesta.Include(x => x.idEncuestaPlantillaNavigation).Where(x => x.idEncuesta == idEncuesta).FirstOrDefault().idEncuestaPlantillaNavigation;
        }

        if (encuestaPlantilla == null)
        {
            return Ok(false);
        }

        var preguntaEncuesta = (from enc in db.tblEncuestaPlantilla
                                where enc.idEncuestaPlantilla == encuestaPlantilla.idEncuestaPlantilla
                                join preg in db.tblPregunta.Include(x => x.idTipoPreguntaNavigation).Include(x => x.idOpcion) on enc.idEncuestaPlantilla equals preg.idEncuestaPlantilla
                                select preg).ToList();


        var pregunta = (from preg in preguntaEncuesta
                        orderby preg.orden
                        select new
                        {
                            preg.idPregunta,
                            tipoPregunta = preg.idTipoPreguntaNavigation.denominacion,
                            preg.descripcion,
                            preg.pregunta,
                            preg.minRango,
                            preg.maxRango,
                            preg.minRangoText,
                            preg.maxRangoText,
                            preg.orden,
                            preg.isOpcional,
                            opciones = from opc in preg.idOpcion
                                       select new
                                       {
                                           opc.idOpcion,
                                           opc.descripcion,
                                           opc.icon
                                       }
                        }).ToList();

        pregunta = pregunta.Select(p =>
        {
            var preguntaModificada = p.pregunta;
            var EsPreguntaAlmacenaje = p.pregunta.Contains("{0}") && p.pregunta.Contains("criterio de almacenaje");

            if (EsPreguntaAlmacenaje)
            {
                var tipoAlmacenaje = entidad?.idTipoAlmacenajeLimpioNavigation != null ? entidad.idTipoAlmacenajeLimpioNavigation.denominacion : "(No definido)";
                preguntaModificada = string.Format(p.pregunta, tipoAlmacenaje);
            }

            return new
            {
                p.idPregunta,
                p.tipoPregunta,
                p.descripcion,
                pregunta = preguntaModificada,
                p.minRango,
                p.maxRango,
                p.minRangoText,
                p.maxRangoText,
                p.orden,
                p.isOpcional,
                p.opciones
            };
        }).ToList();

        var preguntaOpcionPregunta = (from preg in preguntaEncuesta
                                      join pregOpcPreg in db.tblPreguntaNOpcionNPregunta on preg.idPregunta equals pregOpcPreg.idPregunta
                                      select new
                                      {
                                          pregOpcPreg.idPregunta,
                                          pregOpcPreg.idOpcion,
                                          pregOpcPreg.idPreguntaAnidada
                                      }).ToList();

        return Ok(new
        {
            encuestaPlantilla.idEncuestaPlantilla,
            encuestaPlantilla.denominacion,
            tblPregunta = pregunta,
            tblPreguntaNOpcionNPregunta = preguntaOpcionPregunta,
        });
    }

    [EnableQuery]
    [HttpPost("odata/AppComercial/IU_Encuesta")]
    [Authorize]
    public async Task<ActionResult> IU_Encuesta([FromODataUri] int idEntidad, [FromODataUri] int? idEncuesta, [FromBody] List<tblRespuesta> respuestas)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var idsPreguntaNRespuesta = respuestas.Select(x => x.idPregunta).ToList();
        var entidad = db.tblEntidad.Include(x => x.idTipoAlmacenajeLimpioNavigation).FirstOrDefault(x => x.idEntidad == idEntidad);

        // Obtener las preguntas de la plantilla de encuesta
        var preguntaEncuesta = db.tblPregunta.Where(x => idsPreguntaNRespuesta.Contains(x.idPregunta))
            .Include(x => x.idTipoPreguntaNavigation)
            .Include(x => x.idEncuestaPlantillaNavigation)
            .Include(x => x.idOpcion).ToList();

        var idEncuestaPlantilla = preguntaEncuesta.FirstOrDefault().idEncuestaPlantilla;
        var idTipoEncuesta = preguntaEncuesta.FirstOrDefault().idEncuestaPlantillaNavigation.idTipoEncuesta;

        // Personalizar las preguntas
        var pregunta = (from preg in preguntaEncuesta
                        orderby preg.orden
                        select new
                        {
                            preg.idPregunta,
                            tipoPregunta = preg.idTipoPreguntaNavigation.denominacion,
                            preg.descripcion,
                            preg.pregunta,
                            preg.minRango,
                            preg.maxRango,
                            preg.minRangoText,
                            preg.maxRangoText,
                            preg.orden,
                            preg.isOpcional,
                            opciones = from opc in preg.idOpcion
                                       select new
                                       {
                                           opc.idOpcion,
                                           opc.descripcion,
                                           opc.icon
                                       }
                        }).ToList();

        pregunta = pregunta.Select(p =>
        {
            var preguntaModificada = p.pregunta;
            var EsPreguntaAlmacenaje = p.pregunta.Contains("{0}") && p.pregunta.Contains("criterio de almacenaje");
            if (EsPreguntaAlmacenaje)
            {
                var tipoAlmacenaje = entidad?.idTipoAlmacenajeLimpioNavigation != null ? entidad.idTipoAlmacenajeLimpioNavigation.denominacion : "(No definido)";
                preguntaModificada = string.Format(p.pregunta, tipoAlmacenaje);
            }

            return new
            {
                p.idPregunta,
                p.tipoPregunta,
                p.descripcion,
                pregunta = preguntaModificada,
                p.minRango,
                p.maxRango,
                p.minRangoText,
                p.maxRangoText,
                p.orden,
                p.isOpcional,
                p.opciones
            };
        }).ToList();


        List<tblRespuesta> respuesta = new List<tblRespuesta>();

        respuesta.AddRange(from resp in respuestas
                           join preg in pregunta on resp.idPregunta equals preg.idPregunta
                           join opc in db.tblOpcion on resp.idOpcion equals opc.idOpcion into _opc
                           from opc in _opc.DefaultIfEmpty()
                           select new tblRespuesta
                           {
                               descripcionPregunta = preg.descripcion,
                               idPregunta = resp.idPregunta,
                               idOpcion = resp.idOpcion,
                               pregunta = preg.pregunta,
                               descripcionOpcion = opc != null ? opc.descripcion : null,
                               valor = resp.valor,
                               texto = resp.texto,
                               minRango = preg.minRango,
                               maxRango = preg.maxRango,
                               idEntidad = idEntidad,
                           });


        var encuestaSelected = idEncuesta != null ? db.tblEncuesta.FirstOrDefault(x => x.idEncuesta == idEncuesta) : null;
        tblEncuesta encuesta = new tblEncuesta();

        if (encuestaSelected == null) //Si no hay encuesta la genero.
        {
            encuesta = new tblEncuesta()
            {
                idEntidad = idEntidad,
                idEncuestaPlantilla = idEncuestaPlantilla,
                idUsuario = idUsuario,
                fechaContesta = DateTimeOffset.UtcNow,
                idTipoEncuesta = 2,
                tblRespuesta = respuesta
            };
            db.tblEncuesta.Add(encuesta);

        }
        else // Si ya existe la encuesta la actualizo.
        {
            var respuestasOld = db.tblRespuesta.Where(x => x.idEncuesta == idEncuesta).ToList();
            db.tblRespuesta.RemoveRange(respuestasOld);
            encuestaSelected.tblRespuesta = respuesta;
        }

        await db.SaveChangesAsync();

        return Ok();
    }

    [EnableQuery]
    [HttpGet("odata/AppComercial/getDatosReunion")]
    [Authorize]
    public async Task<ActionResult> getDatosReunion([FromODataUri] int idReunion)
    {
        var reunion = db.tblReunion
            .Include(x => x.idTipoReunionNavigation)
            .Include(x => x.idFormatoReunionNavigation)
            .Include(x => x.tblParticipantesNReunion)
            .FirstOrDefault(x => x.idReunion == idReunion);

        var reunionData = new
        {
            reunion.idReunion,
            reunion.idTipoReunion,
            denoTipoReunion = reunion.idTipoReunionNavigation.denominacion,
            reunion.idFormatoReunion,
            denoFormatoReunion = reunion.idFormatoReunionNavigation.denominacion,
            reunion.idEntidad,
            reunion.idCompañia,
            reunion.fecha,
            reunion.observaciones,
            tblParticipantesNReunion = reunion.tblParticipantesNReunion.Select(x => new { x.nombre })
        };
        return Ok(reunionData);
    }
}

