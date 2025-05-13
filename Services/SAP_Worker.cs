using WebApiCore.Class.externos.SAP.Controllers;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.SAP;

namespace WebApiCore.Services
{
    public class SAP_Worker : IHostedService, IDisposable
    {
        private Timer? timer = null;

        private readonly bdERP db;
        private readonly IHttpClientFactory clientFactory;
        private readonly SAPWrap sap = new();

        private readonly int horaDelDia = 2;

        public SAP_Worker(IServiceProvider serviceProvider)
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
            var delay = currentTime < runTime ? runTime - currentTime : TimeSpan.FromDays(1) + runTime - currentTime;

            timer = new Timer(DailyTask, null, delay, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void DailyTask(object? state)
        {
            await updateTablasSAP();
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

        public async Task updateTablasSAP()
        {
            SAPInfoDumpController dumper = new(db);
            try
            {
                await dumper.update_CentrosBeneficio();
                await dumper.update_CentrosCoste();
                await dumper.update_ElementosPEP();
                await dumper.update_Clientes();
                await dumper.update_Proveedores();
                await dumper.update_FormasPago();
                await dumper.update_GrupoArticulos();
                await dumper.update_CuentaContable();
                await dumper.update_CondicionPago();
            }
            catch (Exception e)
            {
                string a = e.Message;
            }
        }

    }
}
