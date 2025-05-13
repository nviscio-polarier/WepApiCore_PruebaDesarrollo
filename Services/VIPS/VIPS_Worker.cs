using WebApiCore.Class;
using WebApiCore.Class.Proyectos.MyPolarier.Administracion;
using WebApiCore.Context;

namespace WebApiCore.Services.VIPS
{
    public class VIPS_Worker : IHostedService, IDisposable
    {
        private Timer? timer = null;

        private readonly bdERP db;
        private readonly IConfiguration configuration;

        private readonly int horaDelDia = 2;

        public VIPS_Worker(IServiceProvider serviceProvider)
        {
            IServiceScope scope = serviceProvider.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<bdERP>();
            configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            var runTime = new TimeSpan(horaDelDia, 0, 0);
            var currentTime = DateTime.Now.TimeOfDay;

            // Si runTime aún no ha ocurrido hoy, resta la hora actual de runTime
            // De lo contrario, calcula el tiempo hasta runTime de mañana
            TimeSpan delay;
            if (Utils.isProduccion())
                delay = currentTime < runTime ? runTime - currentTime : TimeSpan.FromDays(1) + runTime - currentTime;
            else
                delay = TimeSpan.Zero;

            timer = new Timer(DailyTask, null, delay, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void DailyTask(object? state)
        {
            await updatePersonasVIPS();
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

        public async Task updatePersonasVIPS()
        {
            var VIPS = new VIPSService(db, configuration);
            await VIPS.UpdatePersonasVIPS();
        }
    }
}
