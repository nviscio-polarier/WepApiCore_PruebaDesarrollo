using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using AuthorizeAttribute = WebApiCore.Security.AuthorizeAttribute;

namespace WebApiCore.Controllers;
public class EncuestasController : ODataController
{
    private readonly bdERP db;
    public EncuestasController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost("odata/Encuestas/iniciarEncuesta")]
    [Authorize]
    public async Task<ActionResult> iniciarEncuesta([FromODataUri] int idEncuestaPlantilla, [FromBody] List<int> idsUsuario)
    {
        db.tblCampañaEncuesta.Add(new tblCampañaEncuesta()
        {
            idEncuestaPlantilla = idEncuestaPlantilla,
            fechaIni = DateTimeOffset.UtcNow,
            tblEncuesta = idsUsuario.Select(x => new tblEncuesta()
            {
                idUsuario = x
            }).ToList()
        });

        await db.SaveChangesAsync();

        return Ok(true);
    }

    [EnableQuery]
    [HttpGet("odata/Encuestas/checkEncuestaActiva")]
    [Authorize]
    public async Task<ActionResult> checkEncuestaActiva()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var encuestaActiva = db.tblEncuesta
            .Where(x => x.idUsuario.Equals(idUsuario) &&
            x.fechaContesta == null &&
            x.idCampañaEncuesta != null &&
            x.idCampañaEncuestaNavigation.fechaFin == null || DateTimeOffset.UtcNow <= x.idCampañaEncuestaNavigation.fechaFin)
            .OrderBy(x => x.idCampañaEncuestaNavigation.fechaIni)
            .Select(x => new
            {
                x.idEncuesta,
                x.idCampañaEncuestaNavigation.idEncuestaPlantilla,
                denoEncuesta = x.idCampañaEncuestaNavigation.idEncuestaPlantillaNavigation.denominacion,
                isObligatorio = x.idCampañaEncuestaNavigation.fechaLimite != null && DateTimeOffset.UtcNow >= x.idCampañaEncuestaNavigation.fechaLimite,
                x.idCampañaEncuestaNavigation.fechaLimite
            })
            .FirstOrDefault();

        if (encuestaActiva == null)
        {
            return Ok(false);
        }

        var preguntaEncuesta = from enc in db.tblEncuestaPlantilla
                               where enc.idEncuestaPlantilla == encuestaActiva.idEncuestaPlantilla
                               join preg in db.tblPregunta on enc.idEncuestaPlantilla equals preg.idEncuestaPlantilla
                               select preg;

        var pregunta = from preg in preguntaEncuesta
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
                       };

        var preguntaOpcionPregunta = from preg in preguntaEncuesta
                                     join pregOpcPreg in db.tblPreguntaNOpcionNPregunta on preg.idPregunta equals pregOpcPreg.idPregunta
                                     select new
                                     {
                                         pregOpcPreg.idPregunta,
                                         pregOpcPreg.idOpcion,
                                         pregOpcPreg.idPreguntaAnidada
                                     };

        return Ok(new
        {
            encuestaActiva.idEncuesta,
            encuestaActiva.denoEncuesta,
            encuestaActiva.isObligatorio,
            encuestaActiva.fechaLimite,
            tblPregunta = pregunta,
            tblPreguntaNOpcionNPregunta = preguntaOpcionPregunta,
        });
    }

    [EnableQuery]
    [HttpGet("odata/Encuestas/checkEncuestaActiva_v2")]
    [Authorize]
    public async Task<ActionResult> checkEncuestaActiva_v2()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var encuestaActiva = db.tblEncuesta
           .Where(x => x.idUsuario.Equals(idUsuario) &&
            x.fechaContesta == null &&
            x.idCampañaEncuesta != null &&
            x.idCampañaEncuestaNavigation.fechaFin == null || DateTimeOffset.UtcNow <= x.idCampañaEncuestaNavigation.fechaFin)
            .OrderBy(x => x.idCampañaEncuestaNavigation.fechaIni)
            .Select(x => new
            {
                x.idEncuesta,
                x.idCampañaEncuestaNavigation.idEncuestaPlantilla,
                denoEncuesta = x.idCampañaEncuestaNavigation.idEncuestaPlantillaNavigation.denominacion,
                isObligatorio = x.idCampañaEncuestaNavigation.fechaLimite != null && DateTimeOffset.UtcNow >= x.idCampañaEncuestaNavigation.fechaLimite,
                x.idCampañaEncuestaNavigation.fechaLimite
            })
            .FirstOrDefault();

        if (encuestaActiva == null)
        {
            return Ok(false);
        }


        var preguntaEncuesta = from enc in db.tblEncuestaPlantilla
                               where enc.idEncuestaPlantilla == encuestaActiva.idEncuestaPlantilla
                               join preg in db.tblPregunta on enc.idEncuestaPlantilla equals preg.idEncuestaPlantilla
                               select preg;

        var preguntaSinGrupo = (from preg in preguntaEncuesta
                                where preg.idGrupoPregunta == null
                                orderby preg.orden
                                select new
                                {
                                    preg.idPregunta,
                                    tipoPregunta = preg.idTipoPreguntaNavigation.denominacion,
                                    preg.descripcion,
                                    preg.pregunta,
                                    preg.idGrupoPregunta,
                                    preg.minRango,
                                    preg.maxRango,
                                    preg.minRangoText,
                                    preg.maxRangoText,
                                    preg.orden,
                                    preg.isOpcional,
                                    opciones = from opc in preg.idOpcion select new { opc.idOpcion, opc.descripcion, opc.icon }
                                });

        var grupoPreguntas = (from preg in preguntaEncuesta
                              where preg.idGrupoPregunta != null
                              orderby preg.orden
                              group preg by preg.idGrupoPregunta into grupo
                              select new
                              {
                                  idGrupoPregunta = grupo.Key,
                                  denominacion = grupo.Select(x => x.idGrupoPreguntaNavigation.denominacion).FirstOrDefault(),
                                  orden = grupo.Select(x => x.orden).FirstOrDefault(),
                                  preguntas = grupo.Select(preg => new
                                  {
                                      preg.idPregunta,
                                      tipoPregunta = preg.idTipoPreguntaNavigation.denominacion,
                                      preg.descripcion,
                                      preg.pregunta,
                                      preg.idGrupoPregunta,
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
                                  })
                              });


        var pregunta = new List<object>();

        pregunta.AddRange(grupoPreguntas);
        pregunta.AddRange(preguntaSinGrupo);

        pregunta = pregunta.OrderBy(x => x.GetType().GetProperty("orden").GetValue(x, null)).ToList();

        var preguntaOpcionPregunta = from preg in preguntaEncuesta
                                     join pregOpcPreg in db.tblPreguntaNOpcionNPregunta on preg.idPregunta equals pregOpcPreg.idPregunta
                                     select new
                                     {
                                         pregOpcPreg.idPregunta,
                                         pregOpcPreg.idOpcion,
                                         pregOpcPreg.idPreguntaAnidada
                                     };


        return Ok(new
        {
            encuestaActiva.idEncuesta,
            encuestaActiva.denoEncuesta,
            encuestaActiva.isObligatorio,
            encuestaActiva.fechaLimite,
            tblPregunta = pregunta,
            tblPreguntaNOpcionNPregunta = preguntaOpcionPregunta,
        });
    }

    [EnableQuery]
    [HttpPost("odata/Encuestas/setEncuesta")]
    [Authorize]
    public async Task<ActionResult> setEncuesta([FromODataUri] int idEncuesta, [FromBody] List<tblRespuesta> respuestas)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var encuestaPendiente = db.tblEncuesta.Where(x => x.idEncuesta.Equals(idEncuesta) && x.idUsuario.Equals(idUsuario) && x.fechaContesta == null).Count() > 0;
        if (encuestaPendiente)
        {
            #region Aplicar offset lavanderia a fecha actual

            DateTimeOffset offset = DateTimeOffset.UtcNow;

            var idLav = (from usu in db.tblUsuario
                         where usu.idUsuario.Equals(idUsuario)
                         select new
                         {
                             idLav_usu_lavanderia = usu.idLavanderia.Count > 0 ? (int?)usu.idLavanderia.First().idLavanderia : null,
                             idLav_usu_entidad = usu.idEntidad.Count > 0 ? usu.idEntidad.First().idLavanderia.Count > 0 ? (int?)usu.idEntidad.First().idLavanderia.First().idLavanderia : null : null,
                             idLav_usu_persona = usu.idPersonaNavigation != null ? (int?)usu.idPersonaNavigation.idLavanderiaNavigation.idLavanderia : null
                         }).First();

            int? idLavanderia = (idLav.idLav_usu_entidad != null ? idLav.idLav_usu_entidad :
                                idLav.idLav_usu_lavanderia != null ? idLav.idLav_usu_lavanderia :
                                idLav.idLav_usu_persona);

            if (idLavanderia != null)
            {
                int gmt = db.tblLavanderia
               .Where(x => x.idLavanderia.Equals(idLavanderia))
               .Select(x => (x.horarioVerano == true ? 1 : 0) + Convert.ToInt32(x.idZonaHorariaNavigation.GMT)).FirstOrDefault();

                offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
            }

            #endregion

            db.tblRespuesta.AddRange(from resp in respuestas
                                     join preg in db.tblPregunta on resp.idPregunta equals preg.idPregunta
                                     join opc in db.tblOpcion on resp.idOpcion equals opc.idOpcion into _opc
                                     from opc in _opc.DefaultIfEmpty()
                                     select new tblRespuesta
                                     {
                                         idEncuesta = idEncuesta,
                                         descripcionPregunta = preg.descripcion,
                                         idPregunta = resp.idPregunta,
                                         idOpcion = resp.idOpcion,
                                         pregunta = preg.pregunta,
                                         descripcionOpcion = opc != null ? opc.descripcion : null,
                                         idUsuario = idUsuario,
                                         valor = resp.valor,
                                         texto = resp.texto,
                                         minRango = preg.minRango,
                                         maxRango = preg.maxRango
                                     }
                        );

            var objEncuestaNUsuario = db.tblEncuesta.Where(x => x.idUsuario.Equals(idUsuario) && x.idEncuesta.Equals(idEncuesta)).FirstOrDefault();
            if (objEncuestaNUsuario != null)
            {
                objEncuestaNUsuario.fechaContesta = offset;
            }

            try
            {
                await db.SaveChangesAsync();

                //Check si la encuesta está terminada por todos los usuarios
                int numUsuariosPendientes = db.tblEncuesta.Count(x => x.idEncuesta.Equals(idEncuesta) && x.fechaContesta == null);
                if (numUsuariosPendientes == 0)
                {
                    db.tblEncuesta.Find(idEncuesta).fechaContesta = DateTimeOffset.UtcNow;
                    await db.SaveChangesAsync();
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        return BadRequest();
    }

    [EnableQuery]
    [HttpGet("odata/Encuestas/getDatosEncuestas")]
    [Authorize]
    public async Task<ActionResult> getDatosEncuestas([FromODataUri] int idCampañaEncuesta, [FromODataUri] int? idLavanderia, [FromODataUri] string group, [FromODataUri] int idPregunta, [FromODataUri] int? idCompañia)
    {
        var connection = db.Database.GetDbConnection();
        IEnumerable<dynamic> results;

        if (group == "puntuacion")
        {
            results = await connection.QueryAsync("EXEC [ControlCalidad].[EF_spSelectInfEncuestas_puntuacion] @idCampañaEncuesta, @idLavanderia, @idCompañia",
               new { idCampañaEncuesta = idCampañaEncuesta, idLavanderia = idLavanderia, idCompañia = idCompañia });
        }
        else if (group == "seleccion")
        {
            results = await connection.QueryAsync("EXEC [ControlCalidad].[EF_spSelectInfEncuestas_seleccion] @idCampañaEncuesta, @idLavanderia, @idCompañia",
                     new { idCampañaEncuesta = idCampañaEncuesta, idLavanderia = idLavanderia, idCompañia = idCompañia });
        }
        else if (group == "opinion")
        {
            results = await connection.QueryAsync("EXEC [ControlCalidad].[EF_spSelectInfEncuestas_opinion] @idCampañaEncuesta, @idPregunta, @idLavanderia, @idCompañia",
                    new { idCampañaEncuesta = idCampañaEncuesta, idPregunta = idPregunta, idLavanderia = idLavanderia, idCompañia = idCompañia });
        }
        else
        {
            return BadRequest("Invalid group or question ID");
        }

        return Ok(results.ToList());
    }

    [EnableQuery]
    [HttpGet("odata/Encuestas/getDatosEncuestas_resumen")]
    [Authorize]
    public async Task<ActionResult> getDatosEncuestas_resumen([FromODataUri] int idCampañaEncuesta, [FromODataUri] int? idLavanderia, [FromODataUri] int? idCompañia)
    {


        var connection = db.Database.GetDbConnection();
        IEnumerable<dynamic> results;


        results = await connection.QueryAsync("EXEC [ControlCalidad].[EF_spSelectInfEncuestas_resumen] @idCampañaEncuesta, @idLavanderia, @idCompañia",
               new { idCampañaEncuesta = idCampañaEncuesta, idLavanderia = idLavanderia, idCompañia = idCompañia });

        return Ok(results.ToList());

    }

    [EnableQuery]
    [HttpGet("odata/Encuestas/getDatosEncuestas_usuario")]
    [Authorize]
    public async Task<ActionResult> getDatosEncuestas_usuario([FromODataUri] int idCampañaEncuesta, [FromODataUri] int idUsuario)
    {
        var encuestaSel = db.tblCampañaEncuesta.FirstOrDefault(x => x.idCampañaEncuesta == idCampañaEncuesta);
        if (encuestaSel == null)
        {
            return Ok(false);
        }

        var denoUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario == idUsuario).nombre;

        var datosRespuesta = from res in db.tblRespuesta
                             where res.idCampañaEncuesta == idCampañaEncuesta && res.idUsuario == idUsuario
                             join pre in db.tblPregunta on res.idPregunta equals pre.idPregunta
                             orderby pre.orden
                             select new
                             {
                                 res.idRespuesta,
                                 res.idPregunta,
                                 res.idOpcion,
                                 res.descripcionPregunta,
                                 res.descripcionOpcion,
                                 res.texto,
                                 res.maxRango,
                                 res.minRango,
                                 res.valor,
                                 pre.pregunta,
                                 pre.idTipoPregunta
                             };

        return Ok(new
        {
            datosRespuesta,
            denoUsuario
        });
    }

    [EnableQuery]
    [HttpGet("odata/Encuestas/getDatosEncuestas_infRespuestas")]
    [Authorize]
    public async Task<ActionResult> getDatosEncuestas_respuestas_puntuacion([FromODataUri] int idCampañaEncuesta, [FromODataUri] int idPregunta, [FromODataUri] int? idLavanderia, [FromODataUri] int? idCompañia)
    {

        var connection = db.Database.GetDbConnection();
        IEnumerable<dynamic> results;


        results = await connection.QueryAsync("EXEC [ControlCalidad].[EF_spSelectInfEncuestas_infRespuestas] @idCampañaEncuesta,@idPregunta, @idLavanderia, @idCompañia",
               new { idCampañaEncuesta = idCampañaEncuesta, idPregunta = idPregunta, idLavanderia = idLavanderia, idCompañia = idCompañia });

        return Ok(results.ToList());
    }

    [EnableQuery]
    [HttpGet("odata/Encuestas/getDatosEncuestas_listaUsuarios")]
    [Authorize]
    public async Task<ActionResult> getDatosEncuestas_listaUsuarios([FromODataUri] int idCampañaEncuesta, [FromODataUri] int? idLavanderia, [FromODataUri] int? idCompañia, [FromODataUri] int? isEncuestasCompletadas)
    {
        var connection = db.Database.GetDbConnection();
        IEnumerable<dynamic> results;

        results = await connection.QueryAsync("EXEC [ControlCalidad].[EF_spSelectInfEncuestas_usuarios] @idCampañaEncuesta, @idLavanderia , @idCompañia, @isEncuestasCompletadas",
               new { idCampañaEncuesta = idCampañaEncuesta, idLavanderia = idLavanderia, idCompañia = idCompañia, isEncuestasCompletadas = isEncuestasCompletadas });

        return Ok(results.ToList());
    }

    [EnableQuery]
    [HttpGet("odata/Encuestas/getLavanderiaEncuesta")]
    [Authorize]
    public async Task<ActionResult> getLavanderiaEncuesta([FromODataUri] int idCampañaEncuesta)
    {
        var connection = db.Database.GetDbConnection();
        IEnumerable<dynamic> results;
        results = await connection.QueryAsync("EXEC [ControlCalidad].[EF_spSelectLavanderiasNEncuesta] @idCampañaEncuesta",
               new { idCampañaEncuesta = idCampañaEncuesta });
        return Ok(results.ToList());
    }

    [EnableQuery]
    [HttpGet("odata/Encuestas/getCompañiasEncuesta")]
    [Authorize]
    public async Task<ActionResult> getCompañiasEncuesta([FromODataUri] int idCampañaEncuesta)
    {
        var connection = db.Database.GetDbConnection();
        IEnumerable<dynamic> results;
        results = await connection.QueryAsync("EXEC [ControlCalidad].[EF_spSelectCompañiaNEncuesta] @idCampañaEncuesta",
               new { idCampañaEncuesta = idCampañaEncuesta });

        return Ok(results.ToList());
    }


    [EnableQuery]
    [HttpGet("odata/Encuestas/getRespuestas")]
    [Authorize]
    public async Task<ActionResult> getRespuestas([FromODataUri] int idEncuesta)
    {

        var respuestasNEncuesta = db.tblRespuesta.Where(x => x.idEncuesta == idEncuesta);

        if (!respuestasNEncuesta.Any()) return BadRequest("No se encontró encuesta");

        var result = respuestasNEncuesta.Select(x => new
        {
            idOpcion = x.idOpcion != null ? x.idOpcion : null,
            texto = x.texto != null ? x.texto : null,
            valor = x.valor != null ? x.valor : null,
            x.idPregunta
        });

        return Ok(result);
    }
}

public class objEncuestaNEntidad
{
    public int idEncuestaPlantilla { get; set; }
    public int idEntidad { get; set; }
    public List<tblRespuesta> respuestas { get; set; }
    public DateTimeOffset fechaContesta { get; set; }
    public int idTipoEncuesta { get; set; }

}

