using WebApiCore.Class;
using WebApiCore.Class.externos.SAP.Controllers;
using WebApiCore.Class.externos.TimbradoMX;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.SAP;

namespace WebApiCore.Services
{
    public class TimbradoMX_Worker : IHostedService, IDisposable
    {
        private Timer? timer = null;

        private readonly bdERP db;
        private TimbradoMXService TimbradoMX;

        public TimbradoMX_Worker(IServiceProvider serviceProvider)
        {
            IServiceScope scope = serviceProvider.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<bdERP>();
            TimbradoMX = new TimbradoMXService(db);
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            var delay = TimeSpan.Zero;
            var period = TimeSpan.FromMinutes(1);

            timer = new Timer(DailyTask, null, delay, period);

            return Task.CompletedTask;
        }

        private async void DailyTask(object? state)
        {
            await TimbradoMX.checkPendientes();
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
