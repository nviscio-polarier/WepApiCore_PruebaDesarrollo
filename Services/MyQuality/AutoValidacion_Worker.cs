using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Controllers;

namespace WebApiCore.Services.MyQuality;

public class AutoValidacion_Worker : IHostedService, IDisposable
{
    private Timer? timer = null;

    private readonly bdERP db;
    private readonly int horaDelDia = 2;

    public AutoValidacion_Worker(IServiceProvider serviceProvider)
    {
        IServiceScope scope = serviceProvider.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<bdERP>();
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
        await updateAutoValidacion();
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

    public async Task updateAutoValidacion()
    {
        tblGestionRetiroController gestionRetiroController = new(db);
        DateTimeOffset today = DateTimeOffset.UtcNow.Date;
        DateTimeOffset twoDaysAgo = today.AddDays(-2); // es -2 en lugar de -3

        var retirosAValidar = db.tblGestionRetiro.Where(x => x.isValidado == false && x.fechaReg < twoDaysAgo).Include(x => x.tblPrendaNGestionRetiro);

        foreach (var retiro in retirosAValidar)
        {
            retiro.isValidado = true;
            retiro.fechaValidacion = today;
            retiro.idUsuarioValidador = 2398; // idUsuario de JAUME, por poner un idUsuario validador
            gestionRetiroController.CrearMovimiento(retiro);
        }
        await db.SaveChangesAsync();
    }
}
