using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.RRHH;
using WebApiCore.Hubs;

namespace WebApiCore.Services
{
    public class Personal_Worker : IHostedService, IDisposable
    {
        private Timer? _timer = null;

        private readonly bdERP db;
        private readonly IHubContext<NotificacionesHub> _hubContext;
        private List<int?> idsLavanderia;

        public Personal_Worker(IServiceProvider serviceProvider)
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
            var horaEjecucion = 2;
            var filtroZonaHoraria = horaEjecucion - DateTimeOffset.UtcNow.Hour;

            idsLavanderia = db.tblLavanderia
                .Where(x => ((x.horarioVerano == true ? 1 : 0) + Convert.ToInt32(x.idZonaHorariaNavigation.GMT)) == filtroZonaHoraria)
                .Select(x => (int?)x.idLavanderia).ToList();

            ActualizarEstadoPersonas();
            ActualizarLlamamientosActivosAsync();

            await CalendarioController.SincronizarCalendarioPersonal(db, idsLavanderia);
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

        #region Actualizar estados de las personas

        private async void ActualizarEstadoPersonas()
        {
            //Select de todas las personas junto a sus contratos
            //Se busca si tienen contratos válidos
            //Se actualiza solo si cambia su estado de activo
            var personasConContratos = db.tblPersona.Where(x => idsLavanderia.Contains(x.idLavanderia) && x.eliminado == false).Include(x => x.tblPersonaNTipoContrato);
            bool nuevoEstado = false;
            foreach (var persona in personasConContratos)
            {
                if (persona.tblPersonaNTipoContrato.Count > 0)
                {
                    foreach (var contrato in persona.tblPersonaNTipoContrato)
                    {
                        if (isContractValid(contrato.fechaAltaContrato, contrato.fechaBajaContrato))
                        {
                            nuevoEstado = true;
                            break;
                        }
                    }
                }

                if (persona.activo != nuevoEstado)
                {
                    persona.activo = nuevoEstado;
                }
                nuevoEstado = false;
            }
            db.SaveChanges();
        }

        private bool isContractValid(DateTime fechaAltaContrato, DateTime? fechaBajaContrato)
        {
            DateTime currentDay = DateTime.Now;
            if (fechaAltaContrato <= currentDay && (fechaBajaContrato == null || (fechaBajaContrato != null && currentDay < fechaBajaContrato.Value.AddDays(1))))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Actualizar llamamientos activos

        public async void ActualizarLlamamientosActivosAsync()
        {
            foreach (var x in db.tblLlamamiento.Where(x => idsLavanderia.Contains(x.idLavanderia) && x.activo == true && x.idPersona != null && x.fechaIni.Date <= DateTime.Now.Date))
            {
                x.activo = false;
            }

            db.SaveChanges();
        }

        #endregion
    }
}