using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;

namespace WebApiCore.Controllers.Proyectos.MyRealData;
[AllowAnonymous]
public class EventosController : ODataController
{
    private readonly bdERP db;
    private int HORAS_MARGEN_NOCTURNO = 14;
    private int MINUTOS_AJUSTE = 30;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public EventosController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpPost("odata/tblEventoPersona/InsertEventosPersona")]
    public async Task<IActionResult> InsertEventosPersona([FromODataUri] int idLavanderia, [FromBody] List<EventoPersona> eventosPersona)
    {
        List<tblEventoPersona> tblEventoPersonas_inserts = new();
        var horarioLavanderia = db.tblLavanderia.Select(x => new
        { x.idZonaHorariaNavigation, x.horarioVerano, x.idLavanderia }).FirstOrDefault(x => x.idLavanderia == idLavanderia);
        int GMT = (horarioLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(horarioLavanderia.idZonaHorariaNavigation.GMT);

        if (eventosPersona == null)
        {
            return BadRequest();
        }

        // Elimino los registros de eventosPersona que ya se encuentran en DB

        var eventosPersona_filtered = eventosPersona.Where(x => !db.tblEventoPersona.Any(y => y.idPersona == x.idPersona && y.fecha == x.fecha)).ToList();

        if (eventosPersona_filtered.Count == 0)
        {
            return Ok();
        }

        if (eventosPersona_filtered.Count != eventosPersona.Count)
        {
            var eventosPersonajson = JsonSerializer.Serialize(eventosPersona);

            db.tblLogError.Add(new tblLogError
            {
                denominacion = "InsertEventosPersona_idLavanderia_" + idLavanderia,
                error = eventosPersonajson
            });
            db.SaveChanges();
        }

        foreach (var evento in eventosPersona_filtered)
        {
            if (evento.fecha == null)
            {
                evento.fecha = DateTimeOffset.UtcNow;
            }
            else
            {
                evento.isOffline = true;
            }

            tblEventoPersonas_inserts.Add(new tblEventoPersona
            {
                fecha = Class.Utils.aplicarGMT(evento.fecha.Value, GMT),
                idPersona = evento.idPersona,
                idEventoPersona_Estado = evento.idEventoPersona_Estado,
                idLavanderia = idLavanderia,
                isRegManual = evento.isRegManual,
                isOffline = evento.isOffline ?? false
            });
        }

        db.tblEventoPersona.AddRange(tblEventoPersonas_inserts);

        await db.SaveChangesAsync();

        var idsEventosPersona = eventosPersona_filtered.Select(x => x.idPersona).Distinct().ToList();
        var fechaMinimaEvento = tblEventoPersonas_inserts.Min(x => x.fecha);
        var fechaMinimaEvento_margenNocturno = fechaMinimaEvento.AddHours(-HORAS_MARGEN_NOCTURNO);
        var tblEventoPersona_filtered = db.tblEventoPersona.Where(x => idsEventosPersona.Contains(x.idPersona) && x.fecha >= fechaMinimaEvento_margenNocturno).ToList();
        List<PersonaDia> personaDia = new List<PersonaDia>();

        foreach (var ep in eventosPersona_filtered)
        {
            // Si el evento es fechaIni el día es el del evento
            if (ep.idEventoPersona_Estado == 1)
            {
                personaDia.Add(new PersonaDia() { idPersona = ep.idPersona, fecha = ep.fecha.Value.Date });
            }
            else
            {
                //Miro 14h atrás en busca de evento inicio o fin
                var fechaMargenNocturno = ep.fecha.Value.AddHours(-HORAS_MARGEN_NOCTURNO);

                var eventoInicioFin = tblEventoPersona_filtered.OrderBy(y => y.fecha).FirstOrDefault(x => x.idPersona == ep.idPersona &&
                                                                                        (x.idEventoPersona_Estado == 1 || x.idEventoPersona_Estado == 4) &&
                                                                                        x.fecha >= fechaMargenNocturno &&
                                                                                        x.fecha <= ep.fecha);

                if (eventoInicioFin != null && eventoInicioFin.idEventoPersona_Estado == 1) //Se ha encontrado un evento de fecha de inicio
                {
                    if (!personaDia.Any(x => x.idPersona == ep.idPersona && x.fecha == eventoInicioFin.fecha.Date))
                    {
                        personaDia.Add(new PersonaDia() { idPersona = ep.idPersona, fecha = eventoInicioFin.fecha.Date });
                    }
                }
                else //No se ha encontrado evento o el evento que se ha encontrado es fin. Se utilizará la fecha del evento actual.
                {
                    if (!personaDia.Any(x => x.idPersona == ep.idPersona && x.fecha == ep.fecha.Value.Date))
                    {
                        personaDia.Add(new PersonaDia() { idPersona = ep.idPersona, fecha = ep.fecha.Value.Date });
                    }
                }
            }
        }

        return await CalculaJornada(personaDia);
    }

    public async Task<IActionResult> CalculaJornada(List<PersonaDia> personaDia)
    {
        try
        {
            List<int> idsPersona = personaDia.Select(x => x.idPersona).Distinct().ToList();
            List<DateTime> dias = personaDia.Select(x => x.fecha).Distinct().ToList();

            //Seleccionamos las jornadas por persona / día
            List<tblJornada> jornadas_pd = db.tblJornada
                                                .Where(j => idsPersona.Contains(j.idPersona) && dias.Contains(j.fecha)).ToList()
                                                .Where(j => personaDia.Any(x => x.idPersona == j.idPersona && x.fecha == j.fecha.Date)).ToList();

            List<tblJornada> jornadas_pd_sinRevisar = jornadas_pd.Where(j => j.isRevisado == false).ToList();
            List<tblJornada> jornadas_pd_revisadas = jornadas_pd.Where(j => j.isRevisado == true).ToList();

            //Desasociamos todos los eventos de las jornadas que vamos a eliminar.
            var eventos_jornada_sinRevisar = db.tblEventoPersona
                                                .Where(ep =>
                                                ep.isRevisado == false &&
                                                idsPersona.Contains(ep.idPersona) && dias.Contains(ep.fecha.Date)).ToList()
                                                .Where(ep =>
                                                    jornadas_pd_sinRevisar.Any(j => j.idPersona == ep.idPersona && j.fecha == ep.fecha.Date) ||
                                                    jornadas_pd_sinRevisar.Any(j => j.idPersona == ep.idPersona && j.fecha == ep.fecha.Date && j.idJornada == ep.idJornada));

            //Obtener las persona / día no revisadas

            List<PersonaDia> personaDia_revisada = jornadas_pd_revisadas
                                                              .Select(x => new PersonaDia() { idPersona = x.idPersona, fecha = x.fecha.Date })
                                                              .ToList();

            // Crear una nueva lista de objetos PersonaDia que no han sido revisados
            List<PersonaDia> personaDia_sinRevisar = personaDia.Except(personaDia_revisada, new PersonaDiaComparer()).ToList();

            //Obtenemos todos los eventos de personaDia sinRevisar.
            List<tblEventoPersona> eventos = db.tblEventoPersona
                                            .Where(ep =>
                                            ep.isRevisado == false &&
                                            idsPersona.Contains(ep.idPersona) && dias.Contains(ep.fecha.Date)).ToList()
                                            .Where(ep => personaDia_sinRevisar.Any(pd => pd.idPersona.Equals(ep.idPersona) && pd.fecha.Equals(ep.fecha.Date)))
                                            .Where(ep => ep.idJornada == null || ep.idJornadaNavigation?.isRevisado == false)
                                            .ToList();

            var personaLavanderia = db.tblPersona
                                        .Where(p => idsPersona.Contains(p.idPersona)).Select(p => new { p.idPersona, p.idLavanderia, p.idTurno, p.idTipoTrabajo, p.horasDiarias })
                                        .ToList();

            var tblCuadrantePersonal = db.tblCuadrantePersonal
                                            .Where(cp => idsPersona.Contains(cp.idPersona) && dias.Contains(cp.fecha.Date))
                                            .Include(cp => cp.idTurnoNavigation)
                                            .ToList();

            foreach (PersonaDia pd in personaDia_sinRevisar)
            {
                var eventosPD_sinRevisar = eventos
                                            .Where(e =>
                                            e.isRevisado == false &&
                                            e.idPersona.Equals(pd.idPersona) && e.fecha.Date.Equals(pd.fecha))
                                            .ToList();
                var idsJornada_eventosPD_sinRevisar = eventosPD_sinRevisar
                                                        .Where(x => x.isRevisado == false && x.idJornada != null)
                                                        .Select(e => e.idJornada)
                                                        .Distinct()
                                                        .ToList();
                var jornadas_sameDay = jornadas_pd_sinRevisar
                                            .Where(j => idsJornada_eventosPD_sinRevisar.Contains(j.idJornada) && j.fecha.Date == pd.fecha)
                                            .ToList();
                var idsJornadas_sameDay = jornadas_sameDay
                                            .Select(j => j.idJornada).ToList();
                var eventosPD_sinRevisar_sameDay = eventosPD_sinRevisar
                                                        .Where(e =>
                                                        e.isRevisado == false &&
                                                        e.idJornada.HasValue && idsJornadas_sameDay.Contains(e.idJornada.Value))
                                                        .ToList();

                foreach (var evento in eventosPD_sinRevisar_sameDay)
                {
                    evento.idJornada = null;
                }

                var idsJornada = jornadas_sameDay.Select(jsd => jsd.idJornada).ToList();

                var tblCalendarioPersonal = db.tblCalendarioPersonal.Where(cp => cp.idJornada != null && idsJornada.Contains((int)cp.idJornada));

                foreach (var cp in tblCalendarioPersonal)
                {
                    cp.idJornada = null;
                }

                ////Eliminamos las jornadas por persona / día que no están revisadas
                db.tblJornada.RemoveRange(jornadas_sameDay);

                await db.SaveChangesAsync();

                int? idLavanderia = null;
                int? idTurno = null;
                byte? idTipoTrabajo = null;
                TimeSpan? horasDiarias = null;

                var objPersonaDia = personaLavanderia.FirstOrDefault(p => p.idPersona.Equals(pd.idPersona));
                if (objPersonaDia != null)
                {
                    idLavanderia = objPersonaDia.idLavanderia;
                    idTurno = objPersonaDia.idTurno;
                    idTipoTrabajo = objPersonaDia.idTipoTrabajo;
                    horasDiarias = objPersonaDia.horasDiarias;
                }

                //Si la persona no tiene lavandería asignada se omite.
                if (idLavanderia == null) { continue; }

                //Obtenemos los eventos por persona y día y lo ordenamos por fecha.
                List<tblEventoPersona> eventos_pd = eventos
                                                    .Where(e =>
                                                    e.isRevisado == false &&
                                                    e.idPersona.Equals(pd.idPersona) && e.fecha.Date.Equals(pd.fecha.Date))
                                                    .OrderBy(e => e.fecha)
                                                    .ToList();

                var fechaMargenNocturno_posterior = eventos_pd?.LastOrDefault()?.fecha.AddHours(HORAS_MARGEN_NOCTURNO);
                eventos_pd = db.tblEventoPersona.Where(e =>
                                                    e.isRevisado == false &&
                                                    e.idPersona.Equals(pd.idPersona) &&
                                                    pd.fecha <= e.fecha && e.fecha <= fechaMargenNocturno_posterior
                                                     && e.idJornada == null).ToList();

                var cuadrantes = tblCuadrantePersonal
                                            .Where(c =>
                                                c.idPersona.Equals(pd.idPersona)
                                                && c.fecha.Equals(pd.fecha)
                                                && (c.idCalendario_Estado == 3 || c.idCalendario_Estado == 4)
                                                ).ToList()
                                                .Select((cuadrante, i) => (cuadrante, i));

                //Agrupo los eventos para crear jornadas
                List<List<tblEventoPersona>> eventosAgrupados = new List<List<tblEventoPersona>>();

                tblEventoPersona? prev_evento = null;
                foreach (tblEventoPersona evento in eventos_pd)
                {
                    bool nuevoGrupo = prev_evento == null || evento.idEventoPersona_Estado == 1 || prev_evento.idEventoPersona_Estado == 4;

                    if (nuevoGrupo)
                    {
                        List<tblEventoPersona> tblEventoPersona = new List<tblEventoPersona>() { evento };
                        eventosAgrupados.Add(tblEventoPersona);
                    }
                    else
                    {
                        eventosAgrupados.Last().Add(evento);
                    }

                    prev_evento = evento;
                }

                List<tblJornada> newJornadas = new();

                foreach (var (grupo_eventoPersona, i) in eventosAgrupados.Select((grupo_eventoPersona, i) => (grupo_eventoPersona, i)))
                {
                    TimeSpan? tsHoraInicio = null;
                    TimeSpan? tsHoraFin = null;
                    TimeSpan? tsTiempoDescanso = TimeSpan.Zero;

                    //Recorrremos los eventos y calculamos los tiempos de la jornada
                    foreach (var (evento, e) in grupo_eventoPersona.Select((evento, e) => (evento, e)))
                    {
                        tblEventoPersona? evento_prev = e > 0 ? grupo_eventoPersona[e - 1] : null;
                        tblEventoPersona? evento_next = e < grupo_eventoPersona.Count - 1 ? grupo_eventoPersona[e + 1] : null;

                        if (evento.idEventoPersona_Estado.Equals(1))
                        {
                            tsHoraInicio = new TimeSpan(evento.fecha.Hour, evento.fecha.Minute, 0);
                        }
                        else if (tsTiempoDescanso != null && evento.idEventoPersona_Estado.Equals(2))
                        {
                            if (evento_next == null || !evento_next.idEventoPersona_Estado.Equals(3))
                            {
                                tsTiempoDescanso = null;
                            }
                        }
                        else if (tsTiempoDescanso != null && evento.idEventoPersona_Estado.Equals(3))
                        {
                            if (evento_prev != null && evento_prev.idEventoPersona_Estado.Equals(2))
                            {
                                TimeSpan? tsRangoDescanso = new TimeSpan(evento.fecha.Hour, evento.fecha.Minute, 0) - new TimeSpan(evento_prev.fecha.Hour, evento_prev.fecha.Minute, 0);
                                tsTiempoDescanso = tsTiempoDescanso + tsRangoDescanso;
                            }
                            else
                            {
                                tsTiempoDescanso = null;
                            }
                        }
                        else if (evento.idEventoPersona_Estado.Equals(4))
                        {
                            tsHoraFin = new TimeSpan(evento.fecha.Hour, evento.fecha.Minute, 0);
                        }
                    }

                    bool isRegValido = false;
                    int? idCuadrantePersonal = null;
                    tblCuadrantePersonal objCuadrante = cuadrantes.FirstOrDefault(c => c.i.Equals(i)).cuadrante;

                    if (objCuadrante != null)
                    {
                        idLavanderia = objCuadrante.idLavanderia;
                        idCuadrantePersonal = objCuadrante?.idCuadrantePersonal ?? null;

                        //Ajustamos los tiempos en base al cuadrante.
                        TimeSpan? tsHoraInicio_cuadrante = objCuadrante?.horaEntrada ?? null;
                        TimeSpan? tsHoraFin_cuadrante = objCuadrante?.horaSalida ?? null;
                        TimeSpan? tsTiempoDescanso_cuadrante = objCuadrante?.idTurnoNavigation?.descanso ?? null;

                        TimeSpan tsMargenAjuste = new TimeSpan(0, MINUTOS_AJUSTE, 0);

                        if (tsHoraInicio_cuadrante - tsMargenAjuste < tsHoraInicio && tsHoraInicio < tsHoraInicio_cuadrante)
                        {
                            tsHoraInicio = tsHoraInicio_cuadrante;
                        }

                        if (tsHoraFin_cuadrante < tsHoraFin && tsHoraFin < tsHoraFin_cuadrante + tsMargenAjuste)
                        {
                            tsHoraFin = tsHoraFin_cuadrante;
                        }

                        if (TimeSpan.Zero < tsTiempoDescanso && tsTiempoDescanso < tsTiempoDescanso_cuadrante)
                        {
                            tsTiempoDescanso = tsTiempoDescanso_cuadrante;
                        }

                        //Se determina si la jornada va a ser válida cuando
                        //1. La jornada tenga un cuadrante asociado.
                        //2. La jornada tenga todos los tiempos rellenados.
                        //3. El tiempo de la jornada (ajustado) sea igual al tiempo del cuadrante.
                        if (tsHoraInicio != null && tsHoraFin != null && tsTiempoDescanso != null &&
                            tsHoraInicio_cuadrante != null && tsHoraFin_cuadrante != null && tsTiempoDescanso_cuadrante != null)
                        {
                            TimeSpan tiempoJornada = (TimeSpan)tsHoraFin - (TimeSpan)tsHoraInicio - (TimeSpan)tsTiempoDescanso;
                            TimeSpan tiempoJornada_cuadrante = (TimeSpan)tsHoraFin_cuadrante - (TimeSpan)tsHoraInicio_cuadrante - (TimeSpan)tsTiempoDescanso_cuadrante;

                            if (tiempoJornada == tiempoJornada_cuadrante)
                            {
                                isRegValido = true;
                            }
                        }
                    }

                    tblJornada jornada_insert = new tblJornada
                    {
                        fecha = pd.fecha,
                        idPersona = pd.idPersona,
                        idLavanderia = (int)idLavanderia,
                        idCuadrantePersonal = idCuadrantePersonal,
                        idTurno = (int)(objCuadrante != null ? objCuadrante.idTurno : idTurno),
                        horaIni = tsHoraInicio,
                        horaFin = tsHoraFin,
                        tiempoDescanso = tsTiempoDescanso,
                        isRegValido = isRegValido,
                        isRevisado = false,
                        idTipoTrabajo = (byte)idTipoTrabajo,
                        horasDiarias = (TimeSpan)horasDiarias,
                        tblEventoPersona = grupo_eventoPersona,
                        tblCalendarioPersonal = new List<tblCalendarioPersonal>()
                    };

                    var eventoLavanderia = db.tblCalendarioLavanderia.FirstOrDefault(cl => cl.fecha.Date == pd.fecha.Date && cl.idLavanderia == (int)idLavanderia);

                    var idCalendario_EstadoFinal = eventoLavanderia != null && eventoLavanderia.idCalendario_Estado == (byte)idsCalendario_Estado.Festivo
                        ? (byte)idsCalendario_Estado.FestivoTrabajado
                        : (byte)idsCalendario_Estado.DiaTrabajado;

                    var eventoPersona = db.tblCalendarioPersonal.FirstOrDefault(cp => cp.fecha.Date == pd.fecha.Date && cp.idPersona == pd.idPersona);

                    if (eventoPersona == null)
                    {
                        jornada_insert.tblCalendarioPersonal.Add(new tblCalendarioPersonal
                        {
                            idPersona = pd.idPersona,
                            idCalendario_Estado = idCalendario_EstadoFinal,
                            fecha = pd.fecha
                        });
                    }
                    else if (!newJornadas.Any(nj => nj.idPersona == pd.idPersona && nj.fecha.Date == pd.fecha.Date && nj.tblCalendarioPersonal.Any()))
                    {
                        eventoPersona.idCalendario_Estado = idCalendario_EstadoFinal;
                        eventoPersona.idLavanderia = jornada_insert.idLavanderia;
                        eventoPersona.idUsuario_validacion = null;
                        eventoPersona.fecha_validacion = DateTimeOffset.UtcNow;

                        if (!jornada_insert.tblCalendarioPersonal.Contains(eventoPersona))
                        {
                            jornada_insert.tblCalendarioPersonal.Add(eventoPersona);
                        }
                    }

                    newJornadas.Add(jornada_insert);
                }

                db.tblJornada.AddRange(newJornadas);
            }

            await db.SaveChangesAsync();

            bool refreshSmartHub = false;

            var listaPersonasActivas = db.tblPersonaNMaquina.Where(x => personaDia.Select(pd => pd.idPersona).Contains(x.idPersona) &&
                                                                        x.fechaFin == null).ToList();

            //////DESLOGUEO SMART HUB
            foreach (var pnm in listaPersonasActivas)
            {
                var fechaEvento = eventos
                    .Where(x => x.idPersona == pnm.idPersona && pnm.fechaIni < x.fecha)
                    .OrderByDescending(e => e.fecha)
                    .FirstOrDefault()?.fecha;

                if (fechaEvento == null)
                {
                    continue;
                }

                refreshSmartHub = true;
                // Establece la fecha de finalización de la sesión 
                pnm.fechaFin = fechaEvento;

                //Cuando una persona ficha, se comprueba la maquina en la que estaba activo, si no hay personas trabajando se finaliza el cliente.
                await Utils.FinalizaClienteNMaquinaAsync(db, fechaEvento.Value, pnm.idMaquina, pnm.idPersona);
            }

            ////////DESLOGUEO SMART AREA 
            var personaNArea_activa = db.tblPersonaNAreaNLavanderia.Where(x => personaDia.Select(pd => pd.idPersona).Contains(x.idPersona) && x.fechaFin == null).ToList();

            if (personaNArea_activa.Any())
            {
                foreach (var paa in personaNArea_activa)
                {
                    var fechaEvento = eventos
                        .Where(x => x.idPersona == paa.idPersona && paa.fechaIni < x.fecha)
                        .OrderByDescending(e => e.fecha)
                        .FirstOrDefault()?.fecha;

                    if (fechaEvento == null)
                    {
                        continue;
                    }

                    paa.fechaFin = fechaEvento;
                    refreshSmartHub = true;
                }
            }

            await db.SaveChangesAsync();
            ////////SIGNAL-R SMART 

            if (refreshSmartHub)
            {
                var idLavanderia = eventos.FirstOrDefault(x => x.idLavanderia != null).idLavanderia;
                List<string> srcs = new List<string> { "PersonalActivo", "tblClienteNMaquina", "tblEstadoSmartHubNMaquina" };
                _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
            }

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

    }

    public partial class EventoPersona
    {
        [Key]
        public int idPersona { get; set; }
        [Key]
        public DateTimeOffset? fecha { get; set; }
        public byte idEventoPersona_Estado { get; set; }
        public int? idLavanderia { get; set; }
        public bool isRegManual { get; set; }
        public bool? isOffline { get; set; }
        public int? idJornada { get; set; }
    }

    public class PersonaDia
    {
        public int idPersona { get; set; }
        public DateTime fecha { get; set; }
    }


    public class PersonaDiaComparer : IEqualityComparer<PersonaDia>
    {
        // Este método determina si dos objetos PersonaDia son iguales basándose en su fecha y idPersona
        public bool Equals(PersonaDia x, PersonaDia y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.fecha == y.fecha && x.idPersona == y.idPersona;
        }

        // Este método devuelve un valor hash único para cada objeto PersonaDia basado en su fecha y idPersona
        public int GetHashCode(PersonaDia obj)
        {
            if (obj == null)
                return 0;

            int hashPropiedad1 = obj.fecha == null ? 0 : obj.fecha.GetHashCode();
            int hashPropiedad2 = obj.idPersona == null ? 0 : obj.idPersona.GetHashCode();

            return hashPropiedad1 ^ hashPropiedad2;
        }
    }
}
