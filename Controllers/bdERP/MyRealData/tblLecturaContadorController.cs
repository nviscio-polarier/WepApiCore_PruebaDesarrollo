using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using System.Text;
using System.Web;
using WebApiCore.Class.bdERP.MyRealData;
using WebApiCore.Context;
using WebApiCore.Hubs;

namespace WebApiCore.Controllers;

public class tblLecturaContadorController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblLecturaContadorController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [AllowAnonymous]
    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblLecturaContador);
    }

    [AllowAnonymous]
    [EnableQuery]
    [HttpGet("odata/tblLecturaContador/DatosActuales")]
    public ContentResult DatosActuales()
    {
        //Se obtienen los últimos datos de cada contador
        var data = db.tblLecturaContador.Where(x=>x.idRecursoContadorNavigation.activo == true)
            .GroupBy(x => x.idRecursoContador)
            .Select(x => x.OrderByDescending(y => y.fecha).Select(x => new
            {
                Lavanderia = x.idRecursoContadorNavigation.idLavanderiaNavigation.denominacion,
                Grupo = x.idRecursoContadorNavigation.idGrupoEnergeticoNavigation.denominacion,
                Contador = x.idRecursoContadorNavigation.denominacion,
                Fecha = x.fecha,
                UltimoValor = x.valor
            }).FirstOrDefault()).ToList().OrderBy(x => x.Lavanderia).ThenBy(x => x.Grupo).ThenBy(x => x.Contador);

        // Estilos CSS en línea
        string cssStyles = @"
            /* Estilos para el contenedor de tarjetas */
            .card-container {
              display: flex;
              flex-wrap: wrap;
              justify-content: space-around;
              margin: 20px;
            }

            /* Estilos para cada tarjeta */
            .card {
              background-color: #fff;
              box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
              border-radius: 10px;
              margin: 10px;
              width: 350px;
              text-align: left; /* Alinear texto a la izquierda */
              overflow: hidden;
            }

            /* Estilos para el título h4 */
            .card h4 {
              font-size: 18px;
              margin:0;
              margin-bottom: 10px;
              background-color: #f0f0f0; /* Fondo gris clarito */
              padding: 10px; /* Relleno alrededor del texto */
              border-bottom: 1px solid #ccc; /* Línea inferior */
            }

            /* Estilos para los párrafos de datos */
            .card p {
              font-size: 16px;
              line-height: 1.5;
              padding: 0px 20px;
            }

            /* Negrita para ""Grupo"", ""Contador"", ""Fecha"" y ""Último valor"" */
            .card p strong {
              font-weight: bold; /* Negrita */
            }
        ";

        // Generar el HTML de las tarjetas
        StringBuilder html = new StringBuilder();
        html.Append("<style>");
        html.Append(cssStyles); // Variable que contiene los estilos CSS definidos anteriormente
        html.Append("</style>");
        html.Append("<div class=\"card-container\">");

        foreach (var item in data)
        {
            html.Append("<div class=\"card\">");
            html.Append($"<h4>{HttpUtility.HtmlEncode(item.Lavanderia)}</h4>");
            html.Append($"<p><strong>Grupo</strong>: {HttpUtility.HtmlEncode(item.Grupo)}</p>");
            html.Append($"<p><strong>Contador</strong>: {HttpUtility.HtmlEncode(item.Contador)}</p>");
            html.Append($"<p><strong>Fecha</strong>: {item.Fecha}</p>");
            html.Append($"<p><strong>{HttpUtility.HtmlEncode("Último valor")}</strong>: {item.UltimoValor}</p>");
            html.Append("</div>");
        }

        html.Append("</div>");

        return new ContentResult
        {
            ContentType = "text/html",
            Content = html.ToString()
        };
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] tblLecturaContador lectura)
    {
        tblRecursoContador tblRecursoContador = await db.tblRecursoContador.FindAsync(lectura.idRecursoContador);

        #region Aplicar offset lavanderia a fecha actual
        int idLavanderia = tblRecursoContador.idLavanderia;

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

        lectura.fecha = offset;
        lectura.valor = lectura.valor * tblRecursoContador.valorPulsoEnergyHub;
        db.tblLecturaContador.Add(lectura);

        try
        {
            await db.SaveChangesAsync();

            #region SignalR
            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            _hubContext.Clients.Group("EnergyHub_" + idLavanderia).SendAsync("signalR_refresh");
            #endregion

            return Ok(1);
        }
        catch (Exception ex)
        {
            return Ok(-1);
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> PostMasivo([FromBody] tblLecturaContadorAgrupado lectura)
    {
        int? idRecursoContador = lectura.tblLecturaContador.FirstOrDefault().idRecursoContador;
        tblRecursoContador tblRecursoContador = await db.tblRecursoContador.FindAsync(idRecursoContador);

        #region Aplicar offset lavanderia a fecha actual
        int idLavanderia = tblRecursoContador.idLavanderia;

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

        List<tblLecturaContador> lecturasContador = new List<tblLecturaContador>();
        foreach (var lecturas in lectura.tblLecturaContador)
        {
            lecturasContador.Add(new tblLecturaContador
            {
                fecha = lecturas.fecha,
                valor = lecturas.valor,
                idRecursoContador = lecturas.idRecursoContador
            });
        }

        foreach (tblLecturaContador data in lecturasContador)
        {
            data.fecha = offset;
        }
        db.tblLecturaContador.AddRange(lecturasContador);

        try
        {
            await db.SaveChangesAsync();

            #region SignalR
            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            _hubContext.Clients.Group("EnergyHub_" + idLavanderia).SendAsync("signalR_refresh");
            #endregion

            return Ok(true);
        }
        catch (Exception ex)
        {
            return Ok(false);
        }
    }
}