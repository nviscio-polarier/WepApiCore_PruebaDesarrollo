using Microsoft.AspNetCore.SignalR;
using WebApiCore.Context;
using WebApiCore.Hubs;

namespace WebApiCore.Services
{
    public class EnergyHub_Worker : IHostedService, IDisposable
    {
        private Timer? _timer = null;

        private readonly bdERP db;
        private readonly IHubContext<NotificacionesHub> _hubContext;
        public EnergyHub_Worker(IServiceProvider serviceProvider)
        {
            IServiceScope scope = serviceProvider.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<bdERP>();
            _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificacionesHub>>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            //Se ejecutará cada hora en punto.
            TimeSpan currentTime = DateTime.UtcNow.TimeOfDay;
            TimeSpan nextFullHour = TimeSpan.FromHours(Math.Ceiling(currentTime.TotalHours)) + TimeSpan.FromMinutes(5);
            TimeSpan timeToRun = (nextFullHour - currentTime);

            _timer = new Timer(CurrentTask, null, timeToRun, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private async void CurrentTask(object? state)
        {
            db.tblLogError.Add(new tblLogError()
            {
                denominacion = "EnergyHub_Worker",
                error = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")
            });
            await db.SaveChangesAsync();

            var horaEjecucion = 2;
            var filtroZonaHoraria = horaEjecucion - DateTimeOffset.UtcNow.Hour;
            var fechaAnterior = DateTime.Now.AddDays(-1).Date;

            var controlContadores = db.tblRecursoContador
                .Where(rc =>
                rc.isAutomatico == true && rc.isVirtual == false && rc.activo == true && rc.eliminado == false &&
                ((rc.idLavanderiaNavigation.horarioVerano == true ? 1 : 0) + Convert.ToInt32(rc.idLavanderiaNavigation.idZonaHorariaNavigation.GMT)) == filtroZonaHoraria
                )
                .Select(rc => new
                {
                    idRecursoContador = rc.idRecursoContador,
                    sumaInforme = rc.sumaInforme,
                    controlContadorExistente = db.tblControlContador
                        .FirstOrDefault(cc => cc.idRecursoContador == rc.idRecursoContador && cc.fecha.Date == fechaAnterior),
                    controlContadorAnterior = db.tblControlContador.OrderByDescending(x => x.fecha)
                        .FirstOrDefault(cc => cc.idRecursoContador == rc.idRecursoContador && cc.fecha.Date < fechaAnterior)
                });

            foreach (var cc in controlContadores)
            {
                var ultimoValorAutomatizado = db.tblLecturaContador
                         .Where(lc => lc.idRecursoContador == cc.idRecursoContador)
                         .OrderByDescending(lc => lc.fecha)
                         .Select(x => x.valor)
                         .FirstOrDefault();

                var actual = ultimoValorAutomatizado != null ? ultimoValorAutomatizado : 0;
                var actual_ant = cc.controlContadorAnterior != null ? cc.controlContadorAnterior.actual : 0;

                if (cc.controlContadorExistente == null)
                {
                    db.tblControlContador.Add(new tblControlContador()
                    {
                        idRecursoContador = cc.idRecursoContador,
                        actual = actual,
                        diferencia = actual - actual_ant,
                        fecha = fechaAnterior,
                        sumaInforme = cc.sumaInforme
                    });
                }
                else
                {
                    cc.controlContadorExistente.actual = actual;
                    cc.controlContadorExistente.diferencia = actual - actual_ant;
                }
            }
            db.SaveChanges();
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