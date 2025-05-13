using System.Text.RegularExpressions;
using System.Xml;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.MyRealLearning;
using WebApiCore.Hubs;

namespace WebApiCore.Services.MyRealBonus
{
    public class PersonaVideoService
    {

        private readonly bdERP db;
        private readonly IHubContext<NotificacionesHub> _hubContext;
        // Para el screto de la api
        private readonly IConfiguration _config;

        public PersonaVideoService(bdERP context, IHubContext<NotificacionesHub> hubContext, IConfiguration config)
        {

            {
                db = context;
                _hubContext = hubContext;
                _config = config;
            }
        }

        public string getYoutubeID(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            // Expresiones regulares para ambos formatos
            var patrones = new[]
            {
            @"(?:https?:\/\/)?(?:www\.)?youtube\.com\/watch\?v=([^&]+)",
            @"(?:https?:\/\/)?youtu\.be\/([^?&]+)"
        };

            foreach (var patron in patrones)
            {
                var match = Regex.Match(url, patron);
                if (match.Success && match.Groups.Count > 1)
                    return match.Groups[1].Value;
            }

            return null;
        }


        // Ya no lo usamos
        public async Task<int?> getDuracionVideoDesdeYoutube(string enlace)

        {

            // Recoge desde el secreto inyectado la key
            var key = _config["YouTube:ApiKey"];

            var videoID = getYoutubeID(enlace);

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = key,
                ApplicationName = "MyRealLearning"
            });

            // Request a la api de yt, cualquier duda preguntad o :
            // https://developers.google.com/youtube/v3/docs/videos/list?hl=es-419

            var request = youtubeService.Videos.List("contentDetails");
            request.Id = videoID;

            var response = await request.ExecuteAsync();
            var video = response.Items.FirstOrDefault();
            if (video == null) return null;

            // Duración en formato ISO 8601 (PT1H2M3S)
            var duracionISODeYoutube = video.ContentDetails.Duration;
            var timeSpan = XmlConvert.ToTimeSpan(duracionISODeYoutube);
            return (int)timeSpan.TotalSeconds;
        }


        public async Task logEventoVideo(
            EventoVideoDTO eventoVideoDTO)
        {
            var evento = new tblEventoMyRealLearning
            {
                idPersonaVideo = eventoVideoDTO.idPersonaVideo,
                idTipoEvento = eventoVideoDTO.idTipoEvento,
                fecha = DateTime.Now,
                idEstadoAnterior = eventoVideoDTO.idEstadoAnterior,
                idEstadoNuevo = eventoVideoDTO.idEstadoNuevo,
                actor = eventoVideoDTO.actorId,
                comentario = eventoVideoDTO.comentario
            };

            db.tblEventoMyRealLearning.Add(evento);
            await db.SaveChangesAsync();
        }


    }



}


