using Microsoft.AspNetCore.SignalR;
using WebApiCore.Context;
using WebApiCore.Security;
using WebApiCore.SignalR;

namespace WebApiCore.Hubs
{
    [Authorize]
    public class NotificacionesHub : Hub
    {
        private readonly bdERP db;
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        protected IHubContext<NotificacionesHub> _context;

        //--------------
        //--- GRUPOS ---
        //--------------
        // RRHH => MyPolarier
        // notificaciones_RRHH => AppRRHH
        // notificaciones_LogisticaInterna_{idLavanderia} => AppLogisticaInterna
        // EnergyHub_idLavanderia => Dashboard Energy Hub
        // LecturaLavadorasHub_idLavanderia => Dashboard Producción Lavadoras
        // "JornadaPersona_" + idLavanderia + fecha => MyPolarier Entrada Salida Personas
        // "SmartHUB_" + idLavanderia=> MyPolarier LayoutLavanderia
        // "AppLaundrydashboard_" + idLavanderia => MyPolarier Dashboard Lavanderia
        // "InventarioRecambios_" + idMovimientoRecambio => MyPolarier Inventario Recambios

        public NotificacionesHub(IHubContext<NotificacionesHub> context, bdERP dbContext)
        {
            db = dbContext;
            _context = context;
        }

        public Task JoinGroup(string group)
        {
            var httpContext = Context.GetHttpContext();
            var idUsuario = httpContext.Items["idUsuario"];

            if (idUsuario != null)
            {
                tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == int.Parse(idUsuario.ToString()) && !x.isEliminado).FirstOrDefault();
                if (objUsuario != null /*&& objUsuario.enableDatosRRHH*/) // Deshabilitar verificación de permiso de RRHH
                {
                    return Groups.AddToGroupAsync(Context.ConnectionId, group);
                }
            }
            return Task.CompletedTask;
        }
        public Task RemoveGroup(string group)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }

        public void SendToUser(string idUsuario, string methodName, string param1)
        {
            foreach (var connectionId in _connections.GetConnections(idUsuario))
            {
                _context.Clients.Client(connectionId).SendAsync(methodName, param1);
            }

        }
        public void SendToUsers(List<string> idsUsuario, string methodName, string param1)
        {
            idsUsuario.ForEach(delegate (string idUsuario)
            {
                if (idUsuario != null)
                {
                    foreach (var connectionId in _connections.GetConnections(idUsuario))
                    {
                        _context.Clients.Client(connectionId).SendAsync(methodName, param1);
                    }
                }
            });
        }

        public void SendToAllUsers(string methodName, string param1)
        {
            _context.Clients.All.SendAsync(methodName, param1);
        }

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var idUsuario = httpContext.Items["idUsuario"];

            if (idUsuario != null)
            {
                _connections.Add(idUsuario.ToString(), Context.ConnectionId);
            }

            return Task.CompletedTask;
        }

        public Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var idUsuario = httpContext.Items["idUsuario"];
            if (idUsuario != null)
            {
                _connections.Remove(idUsuario.ToString(), Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}