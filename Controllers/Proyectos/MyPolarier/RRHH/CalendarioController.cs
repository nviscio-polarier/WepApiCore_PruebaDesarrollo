using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH;

[Route("odata/RRHH/Calendario")]
public class CalendarioController : ODataController
{
    private readonly bdERP db;

    public readonly byte idCalendario_EstadoSinEvento = 0;

    public CalendarioController(bdERP context)
    {
        db = context;
    }

    [HttpGet("GetCalendarioPersonal")]
    [Authorize]
    public async Task<ActionResult> GetCalendarioPersonal([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int idPersona)
    {
        var result = await Handle_GetCalendarioPersonal(fechaDesde, fechaHasta, new List<int>() { idPersona });

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    public async Task<List<DiaCalendario<EventoCalendarioPersonal>>?> Handle_GetCalendarioPersonal(DateTime fechaDesde, DateTime fechaHasta, List<int> idsPersona)
    {
        var tblPersona = db.tblPersona.Where(p => idsPersona.Contains(p.idPersona)).ToList();

        return await Handle_GetCalendarioPersonal(fechaDesde, fechaHasta, tblPersona);
    }

    public async Task<List<DiaCalendario<EventoCalendarioPersonal>>?> Handle_GetCalendarioPersonal(DateTime fechaDesde, DateTime fechaHasta, List<tblPersona> tblPersona, bool getDiasSinContrato = false)
    {
        DateTime hoy = DateTime.Today;

        var tblCalendario_Estado = await db.tblCalendario_Estado.ToListAsync();

        List<DateTime> dias = GenerarListaDias(fechaDesde, fechaHasta);

        var idsPersona = tblPersona.Select(p => p.idPersona).ToList();

        var tblCalendarioPersonal = await db.tblCalendarioPersonal
            .Where(cp =>
                (cp.idPersonaNavigation.idLavanderia == null || cp.fecha.Date <= hoy.Date)
                && cp.fecha.Date >= fechaDesde.Date
                && cp.fecha.Date <= fechaHasta.Date
                && idsPersona.Contains(cp.idPersona)
            )
            .Select(cp => new
            {
                cp.fecha,
                cp.idCalendario_Estado,
                cp.idPersona,
                idLavanderia = cp.idLavanderia ?? (
                    cp.idJornadaNavigation != null
                    ? cp.idJornadaNavigation.idLavanderia
                    : cp.idCuadrantePersonalNavigation != null
                        ? cp.idCuadrantePersonalNavigation.idLavanderia
                        : cp.idPersonaNavigation.idLavanderia
                ),
                cp.idCuadrantePersonal,
                cp.idJornada,
                idTurno = cp.idJornadaNavigation != null
                    ? cp.idJornadaNavigation.idTurno
                    : cp.idCuadrantePersonalNavigation != null
                        ? cp.idCuadrantePersonalNavigation.idTurno
                        : cp.idPersonaNavigation.idTurno,
                horaEntrada = cp.idJornadaNavigation != null
                    ? cp.idJornadaNavigation.horaIni
                    : cp.idCuadrantePersonalNavigation != null
                        ? cp.idCuadrantePersonalNavigation.horaEntrada
                        : cp.idPersonaNavigation.idTurnoNavigation != null
                            ? cp.idPersonaNavigation.idTurnoNavigation.horaEntrada
                            : null,
                horaSalida = cp.idJornadaNavigation != null
                    ? cp.idJornadaNavigation.horaFin
                    : cp.idCuadrantePersonalNavigation != null
                        ? cp.idCuadrantePersonalNavigation.horaSalida
                        : cp.idPersonaNavigation.idTurnoNavigation != null
                            ? cp.idPersonaNavigation.idTurnoNavigation.horaSalida
                            : null,
                tiempoDescanso = cp.idJornadaNavigation != null
                    ? cp.idJornadaNavigation.tiempoDescanso
                    : cp.idPersonaNavigation != null && cp.idPersonaNavigation.idTurnoNavigation != null
                        ? cp.idPersonaNavigation.idTurnoNavigation.descanso
                        : null,
            })
            .ToListAsync();

        var tblCuadrantePersonal = db.tblCuadrantePersonal
            .Where(cp =>
                cp.fecha.Date > hoy.Date
                && cp.fecha.Date >= fechaDesde.Date
                && cp.fecha.Date <= fechaHasta.Date
                && idsPersona.Contains(cp.idPersona)
            )
            .ToList()
            .Select(cp => new
            {
                cp.fecha,
                cp.idCalendario_Estado,
                cp.idPersona,
                cp.idLavanderia,
                cp.idCuadrantePersonal,
                cp.idTurno,
                cp.horaEntrada,
                cp.horaSalida,
            }).ToList();

        var diasBloqueados = GetEstadoBloqueadoNCalendarioPersonal(fechaDesde, fechaHasta, idsPersona);

        var result = (
                from d in dias

                from p in tblPersona

                join cape in tblCalendarioPersonal
                on new { d.Date, p.idPersona } equals new { cape.fecha.Date, cape.idPersona } into capeGroup
                from cape in capeGroup.DefaultIfEmpty()

                join cupe in tblCuadrantePersonal
                on new { d.Date, p.idPersona } equals new { cupe.fecha.Date, cupe.idPersona } into cupeGroup
                from cupe in cupeGroup.DefaultIfEmpty()

                let idCalendario_Estado = cape?.idCalendario_Estado ?? cupe?.idCalendario_Estado ?? idCalendario_EstadoSinEvento

                join ce in tblCalendario_Estado
                on idCalendario_Estado equals ce.idCalendario_Estado into ceGroup
                from ce in ceGroup.DefaultIfEmpty()

                join db in diasBloqueados
                on new { d.Date, p.idPersona } equals new { db.fecha.Date, db.idPersona }

                where
                    getDiasSinContrato || db.isContratado

                let idLavanderia = cape?.idLavanderia ?? cupe?.idLavanderia ?? p.idLavanderia
                let idCuadrantePersonal = cape?.idCuadrantePersonal ?? cupe?.idCuadrantePersonal
                let idJornada = cape?.idJornada
                let idTurno = cape?.idTurno ?? cupe?.idTurno
                let horaEntrada = cape?.horaEntrada ?? cupe?.horaEntrada
                let horaSalida = cape?.horaSalida ?? cupe?.horaSalida
                let colorHexa = ce?.colorHexa ?? ""

                select new DiaCalendario<EventoCalendarioPersonal>
                {
                    fecha = d,
                    idCalendario_Estado = idCalendario_Estado,
                    colorHexa = colorHexa,
                    isBloqueado = db.isBloqueado,
                    eventos = new()
                    {
                        new()
                        {
                            fecha = d,
                            idCalendario_Estado = idCalendario_Estado,
                            idPersona = p.idPersona,
                            isBloqueado = db.isBloqueado,
                            isContratado = db.isContratado,
                            colorHexa = colorHexa,
                            idLavanderia = idLavanderia,
                            idCuadrantePersonal = idCuadrantePersonal,
                            idJornada = idJornada,
                            idTurno = idTurno,
                            horaEntrada = horaEntrada,
                            horaSalida = horaSalida,
                            tiempoDescanso = cape?.tiempoDescanso
                        }
                    }
                }
            ).ToList();

        return result;
    }

    [HttpGet("GetCalendarioPersonalConLavanderia")]
    [Authorize]
    public async Task<ActionResult> GetCalendarioPersonalConLavanderia([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int idPersona)
    {
        var result = await Handle_GetCalendarioPersonalConLavanderia(fechaDesde, fechaHasta, idPersona);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    public async Task<List<DiaCalendario<dynamic>>?> Handle_GetCalendarioPersonalConLavanderia(DateTime fechaDesde, DateTime fechaHasta, int idPersona, bool rellenarSinEventoConLavanderia = true)
    {
        var persona = db.tblPersona
            .Include(p => p.idCategoriaInternaNavigation)
            .Include(p => p.idTurnoNavigation)
            .FirstOrDefault(p => p.idPersona == idPersona);

        if (persona == null)
        {
            return null;
        }

        var calendarioPersonal = await Handle_GetCalendarioPersonal(fechaDesde, fechaHasta, new List<tblPersona>() { persona });//().SelectMany(cp => cp.eventos);

        var idsLavanderia = calendarioPersonal
            .SelectMany(cp => cp.eventos)
            .Where(cp => cp.idLavanderia != null)
            .Select(cp => (int)cp.idLavanderia)
            .Distinct()
            .ToList();

        var calendarioLavanderia = (await Handle_GetCalendarioLavanderia(fechaDesde, fechaHasta, idsLavanderia)).SelectMany(cl => cl.eventos);

        List<DiaCalendario<dynamic>> result = new();

        foreach (var cp in calendarioPersonal)
        {
            var eventoCalendarioPersonal = cp.eventos.FirstOrDefault();

            var eventoCalendarioLavanderia = calendarioLavanderia
                .FirstOrDefault(cl =>
                    cl.fecha.Date == cp.fecha.Date
                    && cl.idLavanderia == (eventoCalendarioPersonal?.idLavanderia ?? persona.idLavanderia)
                    && cl.idCalendario_Estado != idCalendario_EstadoSinEvento
                );

            DiaCalendario<dynamic> diaCalendario = new()
            {
                fecha = cp.fecha,
                idCalendario_Estado = cp.idCalendario_Estado,
                colorHexa = cp.colorHexa,
                isBloqueado = cp.isBloqueado,
                eventos = new()
            };

            if (rellenarSinEventoConLavanderia && diaCalendario.idCalendario_Estado == idCalendario_EstadoSinEvento && eventoCalendarioLavanderia != null)
            {
                diaCalendario.idCalendario_Estado = eventoCalendarioLavanderia.idCalendario_Estado;
                diaCalendario.colorHexa = eventoCalendarioLavanderia.colorHexa;
            }

            if (eventoCalendarioPersonal != null && diaCalendario.idCalendario_Estado == eventoCalendarioPersonal.idCalendario_Estado)
            {
                diaCalendario.eventos.Add(new
                {
                    eventoCalendarioPersonal.fecha,
                    eventoCalendarioPersonal.idCalendario_Estado,
                    eventoCalendarioPersonal.idPersona,
                    eventoCalendarioPersonal.isBloqueado,
                    eventoCalendarioPersonal.colorHexa,
                    eventoCalendarioPersonal.idLavanderia,
                    eventoCalendarioPersonal.idCuadrantePersonal,
                    eventoCalendarioPersonal.idJornada,
                    eventoCalendarioPersonal.idTurno,
                    eventoCalendarioPersonal.horaEntrada,
                    eventoCalendarioPersonal.horaSalida,
                    eventoCalendarioPersonal.tiempoDescanso,
                });
            }

            if (eventoCalendarioLavanderia != null)
            {
                diaCalendario.eventos.Add(new
                {
                    eventoCalendarioLavanderia.fecha,
                    eventoCalendarioLavanderia.idCalendario_Estado,
                    eventoCalendarioLavanderia.isBloqueado,
                    eventoCalendarioLavanderia.colorHexa,
                    eventoCalendarioLavanderia.idLavanderia,
                    idPersona = (int?)null,
                    idJornada = (int?)null,
                    idCuadrantePersonal = (int?)null,
                    idTurno = (int?)null,
                    horaEntrada = (TimeSpan?)null,
                    horaSalida = (TimeSpan?)null,
                });
            }

            result.Add(diaCalendario);
        }

        return result;
    }

    [HttpGet("GetCalendarioLavanderia")]
    [Authorize]
    public async Task<ActionResult> GetCalendarioLavanderia([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int idLavanderia)
    {
        var result = await Handle_GetCalendarioLavanderia(fechaDesde, fechaHasta, idLavanderia);

        return Ok(result);
    }

    public async Task<List<DiaCalendario<EventoCalendarioLavanderia>>> Handle_GetCalendarioLavanderia(DateTime fechaDesde, DateTime fechaHasta, int idLavanderia)
    {
        return await Handle_GetCalendarioLavanderia(fechaDesde, fechaHasta, new List<int>() { idLavanderia });
    }

    public async Task<List<DiaCalendario<EventoCalendarioLavanderia>>> Handle_GetCalendarioLavanderia(DateTime fechaDesde, DateTime fechaHasta, List<int> idsLavanderia)
    {
        DateTime hoy = DateTime.Today;

        var tblCalendario_Estado = await db.tblCalendario_Estado.ToListAsync();

        List<DateTime> dias = GenerarListaDias(fechaDesde, fechaHasta);

        var tblCalendarioLavanderia = await db.tblCalendarioLavanderia
            .Where(cp =>
                cp.fecha.Date >= fechaDesde.Date
                && cp.fecha.Date <= fechaHasta.Date
                && idsLavanderia.Contains(cp.idLavanderia)
            )
            .ToListAsync();

        var diasBloqueados = GetEstadoBloqueadoNCalendarioLavanderia(fechaDesde, fechaHasta, idsLavanderia);

        var result = (
                from d in dias

                join db in diasBloqueados
                on d.Date equals db.fecha.Date

                join cl in tblCalendarioLavanderia
                on new { db.idLavanderia, d.Date } equals new { cl.idLavanderia, cl.fecha.Date } into clGroup
                from cl in clGroup.DefaultIfEmpty()

                let idCalendario_Estado = cl?.idCalendario_Estado ?? idCalendario_EstadoSinEvento

                join ce in tblCalendario_Estado
                on idCalendario_Estado equals ce.idCalendario_Estado into ceGroup
                from ce in ceGroup.DefaultIfEmpty()

                where
                    db.isBloqueado
                    || cl != null

                let colorHexa = ce?.colorHexa ?? ""

                select new DiaCalendario<EventoCalendarioLavanderia>
                {
                    fecha = d,
                    idCalendario_Estado = idCalendario_Estado,
                    colorHexa = colorHexa,
                    isBloqueado = db.isBloqueado,
                    eventos = new()
                    {
                        new()
                        {
                            fecha = d,
                            idCalendario_Estado = idCalendario_Estado,
                            idLavanderia = db.idLavanderia,
                            isBloqueado = db.isBloqueado,
                            colorHexa = colorHexa,
                        }
                    }
                }
            ).ToList();

        return result;
    }

    [HttpGet("GetCalendarioCentroTrabajo")]
    [Authorize]
    public async Task<ActionResult> GetCalendarioCentroTrabajo([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int idCentroTrabajo)
    {
        var tblCalendario_Estado = await db.tblCalendario_Estado.ToListAsync();

        List<DateTime> dias = GenerarListaDias(fechaDesde, fechaHasta);

        var tblCalendarioCentroTrabajo = await db.tblCalendarioCentroTrabajo
            .Where(cp =>
                cp.fecha.Date >= fechaDesde.Date
                && cp.fecha.Date <= fechaHasta.Date
                && cp.idCentroTrabajo == idCentroTrabajo
            )
            .ToListAsync();

        var result = (
                from d in dias

                join cct in tblCalendarioCentroTrabajo
                on d.Date equals cct.fecha.Date into cctGroup
                from cct in cctGroup.DefaultIfEmpty()

                let idCalendario_Estado = cct?.idCalendario_Estado ?? idCalendario_EstadoSinEvento

                join ce in tblCalendario_Estado
                on idCalendario_Estado equals ce.idCalendario_Estado into ceGroup
                from ce in ceGroup.DefaultIfEmpty()

                where
                    cct?.idCalendario_Estado != null

                let colorHexa = ce?.colorHexa ?? ""

                select new DiaCalendario<EventoCalendarioCentroTrabajo>
                {
                    fecha = d,
                    idCalendario_Estado = idCalendario_Estado,
                    colorHexa = colorHexa,
                    isBloqueado = false,
                    eventos = new()
                    {
                        new()
                        {
                            idCalendario_Estado = idCalendario_Estado,
                            colorHexa = colorHexa,
                            idCentroTrabajo= idCentroTrabajo,
                        }
                    }
                }
            ).ToList();

        return Ok(result);
    }

    [HttpPost("IUD_CalendarioPersonal")]
    [Authorize]
    public async Task<ActionResult> IUD_CalendarioPersonal([FromBody] List<tblCalendarioPersonal> eventosNuevo, int? idUsuario = null)
    {
        idUsuario = idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());

        var hoy = DateTimeOffset.UtcNow;

        var tblPersona = db.tblPersona.Where(p => eventosNuevo.Select(e => e.idPersona).Contains(p.idPersona)).ToList();

        var idsLavanderiaPermitidas = db.tblCuadrantePersonal.Select(cp => cp.idLavanderia).Distinct().ToList();

        // No es posible gestionar eventos de personas que pertenecen a lavanderías con sistemas antigüos
        eventosNuevo = eventosNuevo.Where(en =>
            tblPersona.Any(p => p.idPersona == en.idPersona && (p.idLavanderia == null || idsLavanderiaPermitidas.Contains((int)p.idLavanderia)))
        )
        .ToList();

        var fechas = eventosNuevo.Select(c => c.fecha.Date).Distinct();
        var idsLavanderia = eventosNuevo
            .Where(en => en.idLavanderia != null)
            .Select(en => (int)en.idLavanderia)
            .Distinct()
            .ToList();

        var festivos = db.tblCalendarioLavanderia.Where(cl => fechas.Contains(cl.fecha.Date) && idsLavanderia.Contains(cl.idLavanderia) && cl.idCalendario_Estado == (byte)idsCalendario_Estado.Festivo).ToList();

        foreach (var eventoNuevo in eventosNuevo)
        {
            var persona = tblPersona.FirstOrDefault(p => p.idPersona == eventoNuevo.idPersona);

            if (persona == null)
            {
                return BadRequest("Persona no encotrada");
            }

            var isFestivo = festivos.FirstOrDefault(f => f.fecha.Date == eventoNuevo.fecha.Date && f.idLavanderia == (eventoNuevo.idLavanderia ?? persona.idLavanderia)) != null;

            var new_idCalendario_Estado = GetEstadoByFestivo(isFestivo, eventoNuevo.idCalendario_Estado);

            if (new_idCalendario_Estado == null)
            {
                return BadRequest("idCalendario_Estado no puede ser null.");
            }

            eventoNuevo.idCalendario_Estado = (byte)new_idCalendario_Estado;

            bool isPersonaOficinas = persona.idLavanderia == null;

            eventoNuevo.idUsuario_validacion = idUsuario;
            eventoNuevo.fecha_validacion = hoy;

            // Si la persona es de oficinas o si la fecha de eventoNuevo es menor a hoy, se maneja tblCalendarioPersonal si no tblCuadrantePersonal.
            if (isPersonaOficinas || eventoNuevo.fecha.Date <= hoy.Date)
            {
                var eventoActual = db.tblCalendarioPersonal.FirstOrDefault(cp => cp.fecha.Date == eventoNuevo.fecha.Date && cp.idPersona == eventoNuevo.idPersona);

                if (eventoActual == null && eventoNuevo.idCalendario_Estado == idCalendario_EstadoSinEvento)
                {
                    continue;
                }
                else if (eventoActual == null)
                {
                    db.tblCalendarioPersonal.Add(eventoNuevo);

                    continue;
                }

                if (eventoNuevo.idCalendario_Estado == eventoActual.idCalendario_Estado)
                {
                    continue;
                }

                if (eventoNuevo.idCalendario_Estado == idCalendario_EstadoSinEvento)
                {
                    db.tblCalendarioPersonal.Remove(eventoActual);

                    continue;
                }

                eventoActual.idCalendario_Estado = eventoNuevo.idCalendario_Estado;
                eventoActual.idUsuario_validacion = idUsuario;
                eventoActual.fecha_validacion = hoy;

                continue;
            }

            if (eventoNuevo.idCalendario_Estado == idCalendario_EstadoSinEvento)
            {
                db.tblCuadrantePersonal.RemoveRange(db.tblCuadrantePersonal.Where(cp => cp.idPersona == eventoNuevo.idPersona && cp.fecha.Date == eventoNuevo.fecha.Date));

                continue;
            }

            var cuadranteActual = db.tblCuadrantePersonal.FirstOrDefault(cp => cp.fecha.Date == eventoNuevo.fecha.Date && cp.idPersona == eventoNuevo.idPersona);

            if (cuadranteActual != null)
            {
                cuadranteActual.idCalendario_Estado = eventoNuevo.idCalendario_Estado;

                continue;
            }

            if (isPersonaOficinas)
            {
                continue;
            }

            db.tblCuadrantePersonal.Add(new tblCuadrantePersonal()
            {
                idPersona = eventoNuevo.idPersona,
                fecha = eventoNuevo.fecha,
                idCalendario_Estado = eventoNuevo.idCalendario_Estado,
                horaEntrada = null,
                horaSalida = null,
                idTurno = persona.idTurno,
                idPosicionNAreaLavanderiaNLavanderia = null,
                idLavanderia = (int)persona.idLavanderia,
                isCorregido = false,
                idUsuario_validacion = idUsuario,
                fecha_validacion = DateTimeOffset.UtcNow
            });
        }

        await db.SaveChangesAsync();

        return Ok(true);
    }

    [HttpPost("IUD_CalendarioLavanderia")]
    [Authorize]
    public async Task<ActionResult> IUD_CalendarioLavanderia([FromBody] List<tblCalendarioLavanderia> eventosNuevo)
    {
        foreach (var eventoNuevo in eventosNuevo)
        {
            var eventoActual = db.tblCalendarioLavanderia.FirstOrDefault(cl => cl.fecha.Date == eventoNuevo.fecha.Date && cl.idLavanderia == eventoNuevo.idLavanderia);

            if (eventoActual == null && eventoNuevo.idCalendario_Estado == idCalendario_EstadoSinEvento)
            {
                continue;
            }
            else if (eventoActual == null)
            {
                db.tblCalendarioLavanderia.Add(eventoNuevo);

                // En este caso la persona que tenga día trabajado pasa a festivo trabajado.
                ActulizarCalendarioPersonal(eventoNuevo.idCalendario_Estado, (byte)idsCalendario_Estado.DiaTrabajado, (byte)idsCalendario_Estado.FestivoTrabajado);
                ActulizarCalendarioPersonal(eventoNuevo.idCalendario_Estado, (byte)idsCalendario_Estado.DiaLibre, (byte)idsCalendario_Estado.FestivoDisfrutado);

                continue;
            }

            if (eventoNuevo.idCalendario_Estado == eventoActual.idCalendario_Estado)
            {
                continue;
            }

            // En este caso la persona que tenga festivo trabajado pasa a día trabajado.
            ActulizarCalendarioPersonal(eventoActual.idCalendario_Estado, (byte)idsCalendario_Estado.FestivoTrabajado, (byte)idsCalendario_Estado.DiaTrabajado);
            ActulizarCalendarioPersonal(eventoActual.idCalendario_Estado, (byte)idsCalendario_Estado.FestivoDisfrutado, (byte)idsCalendario_Estado.DiaLibre);

            // En este caso la persona que tenga día trabajado pasa a festivo trabajado.
            ActulizarCalendarioPersonal(eventoNuevo.idCalendario_Estado, (byte)idsCalendario_Estado.DiaTrabajado, (byte)idsCalendario_Estado.FestivoTrabajado);
            ActulizarCalendarioPersonal(eventoNuevo.idCalendario_Estado, (byte)idsCalendario_Estado.DiaLibre, (byte)idsCalendario_Estado.FestivoDisfrutado);

            if (eventoNuevo.idCalendario_Estado == idCalendario_EstadoSinEvento)
            {
                db.tblCalendarioLavanderia.Remove(eventoActual);

                continue;
            }

            db.tblCalendarioLavanderia.Remove(eventoActual);
            db.tblCalendarioLavanderia.Add(eventoNuevo);

            void ActulizarCalendarioPersonal(byte idCalendario_Estado, byte idCalendario_EstadoPrev, byte idCalendario_EstadoNew)
            {
                if (idCalendario_Estado == (byte)idsCalendario_Estado.Festivo)
                {
                    var tblCalendarioPersonal = db.tblCalendarioPersonal
                        .Where(cp => cp.fecha.Date == eventoNuevo.fecha.Date && cp.idLavanderia == eventoNuevo.idLavanderia && cp.idCalendario_Estado == idCalendario_EstadoPrev)
                        .ToList();

                    foreach (var cp in tblCalendarioPersonal)
                    {
                        cp.idCalendario_Estado = idCalendario_EstadoNew;
                    }

                    var tblCuadrantePersonal = db.tblCuadrantePersonal
                        .Where(cp => cp.fecha.Date == eventoNuevo.fecha.Date && cp.idLavanderia == eventoNuevo.idLavanderia && cp.idCalendario_Estado == idCalendario_EstadoPrev)
                        .ToList();

                    foreach (var cp in tblCuadrantePersonal)
                    {
                        cp.idCalendario_Estado = idCalendario_EstadoNew;
                    }
                }
            }
        }

        await db.SaveChangesAsync();

        return Ok(true);
    }

    [HttpPost("IUD_CalendarioCentroTrabajo")]
    [Authorize]
    public async Task<ActionResult> IUD_CalendarioCentroTrabajo([FromBody] List<tblCalendarioCentroTrabajo> eventosNuevo)
    {
        foreach (var eventoNuevo in eventosNuevo)
        {
            var eventoActual = db.tblCalendarioCentroTrabajo.FirstOrDefault(cl => cl.fecha.Date == eventoNuevo.fecha.Date && cl.idCentroTrabajo == eventoNuevo.idCentroTrabajo);

            if (eventoActual == null && eventoNuevo.idCalendario_Estado == idCalendario_EstadoSinEvento)
            {
                continue;
            }
            else if (eventoActual == null)
            {
                db.tblCalendarioCentroTrabajo.Add(eventoNuevo);

                continue;
            }

            if (eventoNuevo.idCalendario_Estado == eventoActual.idCalendario_Estado)
            {
                continue;
            }

            if (eventoNuevo.idCalendario_Estado == idCalendario_EstadoSinEvento)
            {
                db.tblCalendarioCentroTrabajo.Remove(eventoActual);

                continue;
            }

            db.tblCalendarioCentroTrabajo.Remove(eventoActual);
            db.tblCalendarioCentroTrabajo.Add(eventoNuevo);
        }

        await db.SaveChangesAsync();

        return Ok(true);
    }

    public List<EstadoBloqueadoNCalendarioPersonal> GetEstadoBloqueadoNCalendarioPersonal(DateTime fechaDesde, DateTime fechaHasta, List<int> idsPersona)
    {
        var hoy = DateTime.Today;

        var tblPersonaNTipoContrato = db.tblPersonaNTipoContrato
            .Where(pntc =>
                idsPersona.Contains(pntc.idPersona)
                && pntc.fechaAltaContrato.Date <= fechaHasta.Date
                && (pntc.fechaBajaContrato == null || ((DateTime)pntc.fechaBajaContrato).Date >= fechaDesde.Date)
            )
            .ToList();

        var tblNomina = db.tblNomina
            .Where(n =>
                idsPersona.Contains(n.idPersona)
                && n.fechaDesde.Date <= fechaHasta.Date
                && n.fechaHasta.Date >= fechaDesde.Date
                && (
                    n.fechaBaja == null
                    || n.idTipoNomina == (short)idsTipoNomina.PagaFiniquito
                    || n.idMotivoBaja == (byte)idsMotivoBaja.ConversionContrato
                )
            )
            .ToList();

        List<EstadoBloqueadoNCalendarioPersonal> result = new();

        for (DateTime fecha = fechaDesde; fecha <= fechaHasta; fecha = fecha.AddDays(1))
        {
            foreach (var idPersona in idsPersona)
            {
                var isContratado = tblPersonaNTipoContrato.Any(pntc =>
                    pntc.idPersona == idPersona
                    && fecha.Date >= pntc.fechaAltaContrato.Date
                    && (pntc.fechaBajaContrato == null || fecha.Date <= ((DateTime)pntc.fechaBajaContrato).Date)
                );

                result.Add(new EstadoBloqueadoNCalendarioPersonal
                {
                    idPersona = idPersona,
                    fecha = fecha,
                    isContratado = isContratado,
                    isBloqueado =
                    !isContratado
                    || (
                        (
                            fecha.Year != hoy.Year
                            || fecha.Month != hoy.Month
                            || fecha.Date < hoy.Date
                        )
                        && tblNomina.Any(n =>
                            n.idPersona == idPersona
                            && n.fechaDesde.Date <= fecha.Date
                            && n.fechaHasta.Date >= fecha.Date
                            && (
                                (n.fechaBaja != null && n.idEstadoNomina != (byte)idsEstadoNomina.SolicitudFiniquitoInterna)
                                || (n.fechaBaja == null && n.idEstadoNomina != (byte)idsEstadoNomina.EnProceso)
                            )
                        )
                    )
                });
            }
        }

        return result;
    }

    public List<EstadoBloqueadoNCalendarioLavanderia> GetEstadoBloqueadoNCalendarioLavanderia(DateTime fechaDesde, DateTime fechaHasta, List<int> idsLavanderia)
    {
        var tblLavanderia = db.tblLavanderia.Where(l => idsLavanderia.Contains(l.idLavanderia)).ToList();

        var tblNomina = db.tblNomina.Where(n =>
            tblLavanderia.Select(l => l.idAdmElementoPEP).Contains(n.idAdmElementoPEP)
            && n.fechaDesde <= fechaHasta
            && n.fechaHasta >= fechaDesde
            && (
                n.fechaBaja == null
                || n.idTipoNomina == (short)idsTipoNomina.PagaFiniquito
            )
        ).ToList();

        List<EstadoBloqueadoNCalendarioLavanderia> result = new();

        for (DateTime fecha = fechaDesde; fecha <= fechaHasta; fecha = fecha.AddDays(1))
        {
            foreach (var l in tblLavanderia)
            {
                result.Add(new EstadoBloqueadoNCalendarioLavanderia
                {
                    idLavanderia = l.idLavanderia,
                    fecha = fecha,
                    isBloqueado = tblNomina.Any(n =>
                        n.idAdmElementoPEP == l.idAdmElementoPEP
                        && n.fechaDesde <= fecha
                        && n.fechaHasta >= fecha
                        && (
                            (n.fechaBaja != null && n.idEstadoNomina != (byte)idsEstadoNomina.SolicitudFiniquitoInterna)
                            || (n.fechaBaja == null && n.idEstadoNomina != (byte)idsEstadoNomina.EnProceso)
                        )
                    )
                });
            }
        }

        return result;
    }

    public static async Task SincronizarCalendarioPersonal(bdERP db, List<int?> idsLavanderia)
    {
        var hoy = DateTime.Today;

        var idsPersona = db.tblCuadrantePersonal.Select(cp => cp.idPersona).Distinct().ToList();

        DateTime? ultimaFechaValida = db.tblCalendarioPersonal
            .Where(cp =>
                idsPersona.Contains(cp.idPersona)
                && cp.idCuadrantePersonal != null
                && idsLavanderia.Contains(cp.idLavanderia)
            )
            .OrderByDescending(cp => cp.fecha)
            .Select(cp => (DateTime?)cp.fecha)
            .FirstOrDefault();

        var fechaDesde = ultimaFechaValida?.AddDays(1) ?? hoy.Date;
        fechaDesde = fechaDesde.Date > hoy.Date ? hoy.Date : fechaDesde;
        var fechaHasta = hoy.Date;

        var tblCuadrantePersonal = db.tblCuadrantePersonal
            .Where(cp =>
                cp.fecha.Date >= fechaDesde.Date
                && cp.fecha.Date <= fechaHasta.Date
                && idsLavanderia.Contains(cp.idLavanderia)
                && cp.idCalendario_Estado != null
            )
            .ToList();

        var tblCalendarioPersonal = db.tblCalendarioPersonal
            .Where(cp =>
                cp.fecha.Date >= fechaDesde.Date
                && cp.fecha.Date <= fechaHasta.Date
                && tblCuadrantePersonal.Select(cp => cp.idPersona).Contains(cp.idPersona)
            )
            .ToList();

        var listaEventos = (
            from cupe in tblCuadrantePersonal

            join cape in tblCalendarioPersonal
            on new { cupe.idPersona, cupe.fecha.Date } equals new { cape.idPersona, cape.fecha.Date } into capeGroup
            from cape in capeGroup.DefaultIfEmpty()

            where
                cape == null

            select new tblCalendarioPersonal
            {

                fecha = cupe.fecha,
                idPersona = cupe.idPersona,
                idCalendario_Estado = (byte)cupe.idCalendario_Estado,
                idLavanderia = cupe.idLavanderia,
                idUsuario_validacion = null,
                fecha_validacion = hoy,
                idCuadrantePersonal = cupe.idCuadrantePersonal,
            }
        );

        db.tblCalendarioPersonal.AddRange(listaEventos);

        await db.SaveChangesAsync();
    }

    public static List<DateTime> GenerarListaDias(DateTime fechaDesde, DateTime fechaHasta)
    {
        List<DateTime> dias = new();

        while (fechaDesde <= fechaHasta)
        {
            dias.Add(fechaDesde);
            fechaDesde = fechaDesde.AddDays(1);
        }

        return dias;
    }

    public byte GetEstadoByDiaLibre(bool isDiaLibre, bool isFestivo)
    {
        byte? result = null;

        if (isDiaLibre)
        {
            result = GetEstadoByFestivo(isFestivo, (byte)idsCalendario_Estado.DiaLibre);
        }
        else
        {
            result = GetEstadoByFestivo(isFestivo, (byte)idsCalendario_Estado.DiaTrabajado);
        }

        if (result == null)
        {
            throw new Exception("Error GetEstadoByDiaLibre");
        }

        return (byte)result;
    }

    public byte? GetEstadoByFestivo(bool isFestivo, byte? idCalendario_Estado)
    {
        if (idCalendario_Estado == null)
        {
            return null;
        }

        if (isFestivo)
        {
            if (idCalendario_Estado == (byte)idsCalendario_Estado.DiaTrabajado)
            {
                return (byte)idsCalendario_Estado.FestivoTrabajado;
            }

            if (idCalendario_Estado == (byte)idsCalendario_Estado.DiaLibre)
            {
                return (byte)idsCalendario_Estado.FestivoDisfrutado;
            }
        }
        else
        {
            if (idCalendario_Estado == (byte)idsCalendario_Estado.FestivoTrabajado)
            {
                return (byte)idsCalendario_Estado.DiaTrabajado;
            }

            if (idCalendario_Estado == (byte)idsCalendario_Estado.FestivoDisfrutado)
            {
                return (byte)idsCalendario_Estado.DiaLibre;
            }
        }

        return idCalendario_Estado;
    }

    public class EventoCalendarioPersonal
    {
        public DateTime fecha { get; set; }
        public byte idCalendario_Estado { get; set; }
        public int idPersona { get; set; }
        public bool isBloqueado { get; set; }
        public bool isContratado { get; set; }
        public string colorHexa { get; set; }
        public int? idLavanderia { get; set; }
        public int? idCuadrantePersonal { get; set; }
        public int? idJornada { get; set; }
        public int? idTurno { get; set; }
        public TimeSpan? horaEntrada { get; set; }
        public TimeSpan? horaSalida { get; set; }
        public TimeSpan? tiempoDescanso { get; set; }
    }

    public class EventoCalendarioLavanderia
    {
        public DateTime fecha { get; set; }
        public byte idCalendario_Estado { get; set; }
        public int idLavanderia { get; set; }
        public bool isBloqueado { get; set; }
        public string colorHexa { get; set; }
    }

    public class EventoCalendarioCentroTrabajo
    {
        public byte idCalendario_Estado { get; set; }
        public string colorHexa { get; set; }
        public int idCentroTrabajo { get; set; }
    }

    public class DiaCalendario<T>
    {
        public DateTime fecha { get; set; }
        public byte idCalendario_Estado { get; set; }
        public string colorHexa { get; set; }
        public bool isBloqueado { get; set; }
        public List<T> eventos { get; set; }
    }

    public class EstadoBloqueadoNCalendarioPersonal
    {
        public int idPersona { get; set; }
        public DateTime fecha { get; set; }
        public bool isContratado { get; set; }
        public bool isBloqueado { get; set; }
    }

    public class EstadoBloqueadoNCalendarioLavanderia
    {
        public int idLavanderia { get; set; }
        public DateTime fecha { get; set; }
        public bool isBloqueado { get; set; }
    }

    public class EstadoBloqueadoNCalendarioCentroTrabajo
    {
        public int idCentroTrabajo { get; set; }
        public DateTime fecha { get; set; }
        public bool isBloqueado { get; set; }
    }
}
