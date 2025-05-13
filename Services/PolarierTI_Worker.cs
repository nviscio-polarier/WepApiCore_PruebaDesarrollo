using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text;
using WebApiCore.Class;
using WebApiCore.Class.sms;
using WebApiCore.Context;
using WebApiCore.Controllers;
using WebApiCore.Hubs;

namespace WebApiCore.Services
{
    public class PolarierTI_Worker : IHostedService, IDisposable
    {
        private Timer? _timer = null;

        private readonly bdERP db;
        private readonly IHubContext<NotificacionesHub> _hubContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public PolarierTI_Worker(IServiceProvider serviceProvider)
        {
            IServiceScope scope = serviceProvider.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<bdERP>();
            _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificacionesHub>>();
            _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            _clientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(CurrentTask, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void CurrentTask(object? state)
        {
            PolarierTIController controller = new PolarierTIController(db, _hubContext);
            List<dynamic> result = controller.getAppStatus();

            bool sendSMS = false;
            foreach (dynamic item in result.Where(x => x.error == 1))
            {
                try
                {
                    string errorCode = item.errorCode;
                    //Si no se encuentra una notificación con el mismo código y sin solucionar se genera una nueva.
                    if (item.notificacion == null)
                    {
                        sendSMS = true;
                        db.tblNotificaciones_TI.Add(new tblNotificaciones_TI()
                        {
                            idAplicacion = item.idAplicacion,
                            fecha_Inicio = DateTime.UtcNow,
                            codigo = errorCode,
                            descripcion = item.errorText,
                        });
                    }
                }
                catch (Exception ex)
                {

                }
            }

            await db.SaveChangesAsync();

            //SMS             
            if (sendSMS && Utils.isProduccion())
            {
                string url = "https://api.gateway360.com/api/3.0/sms/send";
                string from = "Polarier";
                string text = "Incidencia TI - Revisa App PolarierTI";
                VerificarTelefonoRequest objRequest = new VerificarTelefonoRequest(_configuration);

                List<string> telefonos = db.tblUsuario
                     .Where(x => x.idUsuario == 179 || x.idUsuario == 180)
                     .Select(x => "+" + x.idPersonaNavigation.prefijoTelefonico + x.idPersonaNavigation.telefono).ToList();
                foreach (string tlf in telefonos)
                {
                    string to = tlf;//Telefono

                    VerificarTelefonoRequest.Message message = new VerificarTelefonoRequest.Message()
                    {
                        to = to,
                        from = from,
                        text = text
                    };

                    objRequest.messages.Add(message);
                }

                var content = JsonConvert.SerializeObject(objRequest);

                HttpClient httpClient = _clientFactory.CreateClient();
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, stringContent);

                var responseContent = await response.Content.ReadAsStringAsync();
                //if (responseContent.Contains("error"))
                //    return BadRequest();

            }

            _hubContext.Clients.Group("PolarierTI").SendAsync("signalR_refresh", Newtonsoft.Json.JsonConvert.SerializeObject(controller.getAppStatus()));
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}