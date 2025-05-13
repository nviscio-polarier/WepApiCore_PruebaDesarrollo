using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Enums.General;
using WebApiCore.Hubs;

namespace WebApiCore.Services
{
    public class SmartHub_Worker : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private readonly int horasMaximasPersona = 20;

        private readonly bdERP db;
        private readonly IHubContext<NotificacionesHub> _hubContext;
        public SmartHub_Worker(IServiceProvider serviceProvider)
        {
            IServiceScope scope = serviceProvider.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<bdERP>();
            _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificacionesHub>>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(Tareas, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void Tareas(object? state)
        {
            await CheckEstadoMaquina(state);
            await CheckEstadoPersona(state);
        }

        private async Task CheckEstadoMaquina(object? state)
        {
            var maquinas = db.tblEstadoSmartHubNMaquina.Select(x => new { x.idMaquina, x.idMaquinaNavigation.idLavanderia }).Distinct();
            try
            {
                var maquinasApagadas = (from maquina in maquinas
                                        from ultimoEstado in db.tblEstadoSmartHubNMaquina.Where(x => x.idMaquina.Equals(maquina.idMaquina)).OrderByDescending(x => x.fechaUltimaActualizacion).Take(1)
                                        where ultimoEstado.idEstadoSmartHub != 2 && EF.Functions.DateDiffMinute(ultimoEstado.fechaUltimaActualizacion, DateTimeOffset.UtcNow) > 2
                                        select new { ultimoEstado.idMaquina, maquina.idLavanderia }
                                       ).ToList();

                var idsLavanderias = maquinasApagadas.Select(x => x.idLavanderia).Distinct();

                foreach (var maquina in maquinasApagadas)
                {
                    #region Aplicar offset lavanderia a fecha actual

                    var tblLavanderia = db.tblLavanderia
                   .Where(x => x.idLavanderia.Equals(maquina.idLavanderia))
                   .Select(x => new
                   {
                       x.idLavanderia,
                       x.horarioVerano,
                       x.idZonaHorariaNavigation
                   }).FirstOrDefault();

                    int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

                    DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
                    #endregion

                    db.tblEstadoSmartHubNMaquina.Add(new tblEstadoSmartHubNMaquina()
                    {
                        idMaquina = (int)maquina.idMaquina,
                        idEstadoSmartHub = 2,
                        fechaInicio = offset,
                        fechaUltimaActualizacion = offset
                    });

                    //DESLOGUEO SMART HUB
                    //Personas
                    List<tblPersonaNMaquina> personaActiva = db.tblPersonaNMaquina.Where(x => x.idMaquina.Equals((int)maquina.idMaquina) && x.fechaFin == null).ToList();
                    foreach (tblPersonaNMaquina pnm in personaActiva)
                    {
                        pnm.fechaFin = offset;

                        //DESLOGUEO SMART AREA
                        var regActivos = db.tblPersonaNAreaNLavanderia.Where(x => x.idPersona.Equals(pnm.idPersona) && x.fechaFin == null);
                        if (regActivos.Count() > 0) //Finalizamos cualquier registro activo
                        {
                            foreach (tblPersonaNAreaNLavanderia pnanl in regActivos)
                            {
                                pnanl.fechaFin = offset;
                            }
                        }
                    }

                    //Cliente
                    List<tblClienteNMaquina> listCnm = db.tblClienteNMaquina.Where(x => x.idMaquina.Equals((int)maquina.idMaquina) && x.fechaFin == null).ToList();
                    foreach (tblClienteNMaquina cnm in listCnm)
                    {
                        cnm.fechaFin = offset;
                    }
                }

                if (maquinasApagadas.Count() > 0)
                {
                    await db.SaveChangesAsync();

                    foreach (var idLavanderia in idsLavanderias)
                    {
                        List<string> srcs = new List<string> { "tblEstadoSmartHubNMaquina", "tblClienteNMaquina", "PersonalActivo" };
                        await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
                    }
                }
            }
            catch
            {
            }
        }

        private async Task CheckEstadoPersona(object? state)
        {
            //Se delogean las personas que llevan más de 20 horas logueadas
            var cutoffTime = DateTimeOffset.UtcNow.AddHours(-horasMaximasPersona);

            var pnm = db.tblPersonaNMaquina.Include(x => x.idMaquinaNavigation)
              .Where(x => x.fechaFin == null && x.fechaIni <= cutoffTime);

            var pnanl = db.tblPersonaNAreaNLavanderia
             .Where(x => x.fechaFin == null && x.fechaIni <= cutoffTime);

            foreach (var item in pnm)
            {
                item.fechaFin = DateTimeOffset.UtcNow;
            }
            foreach (var item in pnanl)
            {
                item.fechaFin = DateTimeOffset.UtcNow;
            }

            //SIGNAL R - SMART VIEW
            var idsLavanderias = pnm.Select(x => x.idMaquinaNavigation.idLavanderia).Union(pnanl.Select(x => x.idLavanderia)).ToList();
            foreach (var idLavanderia in idsLavanderias)
            {
                List<string> srcs = new List<string> { "tblEstadoSmartHubNMaquina", "tblClienteNMaquina", "PersonalActivo" };
                await _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
            }

            await db.SaveChangesAsync();
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