using WebApiCore.Class;
using WebApiCore.Class.Proyectos.MyPolarier.RRHH;
using WebApiCore.Context;

namespace WebApiCore.Services
{
    public class Notificaciones_Worker : IHostedService, IDisposable
    {
        private Timer? timer = null;

        private readonly bdERP db;
        private readonly IHttpClientFactory clientFactory;

        private readonly int horaDelDia = 8;

        public Notificaciones_Worker(IServiceProvider serviceProvider)
        {
            IServiceScope scope = serviceProvider.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<bdERP>();
            clientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            var runTime = new TimeSpan(horaDelDia, 0, 0);
            var currentTime = DateTime.Now.TimeOfDay;

            // Si runTime aún no ha ocurrido hoy, resta la hora actual de runTime
            // De lo contrario, calcula el tiempo hasta runTime de mañana
            var delay = Utils.isProduccion() ? currentTime < runTime ? runTime - currentTime : TimeSpan.FromDays(1) + runTime - currentTime : TimeSpan.Zero;

            timer = new Timer(DailyTask, null, delay, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void DailyTask(object? state)
        {
            var tasks = new List<Task>();

            Aviso_PlusesNominaService aviso_PlusesNominaService = new(db, clientFactory);
            tasks.Add(aviso_PlusesNominaService.EnviarAviso_PlusesNomina());

            NotificacionesRRHH_GestoriaService notificaciones = new(db, null);
            tasks.Add(notificaciones.SendAvisosDiarios_RRHH());
            tasks.Add(notificaciones.SendAvisosDiarios_Gestoria());

            await Task.WhenAll(tasks);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
