using WebApiCore.Class;
using WebApiCore.Class.externos.a3innuva;
using WebApiCore.Context;
using WebApiCore.Enums.GestionInterna;

namespace WebApiCore.Services.a3innuva
{
    public class A3innuvaAuth_Worker : IHostedService, IDisposable
    {
        private Timer? _timer = null;

        private readonly bdERP db;

        public A3innuvaAuth_Worker(IServiceProvider serviceProvider)
        {
            IServiceScope scope = serviceProvider.CreateScope();

            db = scope.ServiceProvider.GetRequiredService<bdERP>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            // Se ejecutará cada 45 minutos.
            TimeSpan interval = TimeSpan.FromMinutes(45);

            _timer = new Timer(RefreshTokenTask, null, TimeSpan.Zero, interval);

            return Task.CompletedTask;
        }

        private async void RefreshTokenTask(object? state)
        {
            // Servicio de autenticación de a3innuva
            A3innuvaAuthService authService = new();

            if (A3innuvaAuthUtils.Get_refresh_token() != null)
            {
                try
                {
                    await authService.Refresh_acces_token();
                }
                catch (Exception ex)
                {
                    List<int> idsUsuarioCustom = new()
                        {
                            (int)idsUsuario.NehuenAlfonsoGomez,
                            (int)idsUsuario.DavidTorrelloCocera,
                        };

                    CorreoService correoService = new();

                    List<string> emails = db.tblUsuario.Where(u => idsUsuarioCustom.Contains(u.idUsuario) && u.email != null).Select(u => u.email).ToList();

                    var subject = "Error al refrescar el token de A3";

                    var body = ex.Message;

                    correoService.Add(emails, subject, body);

                    try
                    {
                        correoService.Send();
                    }
                    catch
                    {
                    }
                }
            }
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
