using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Enums.MyRealData;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH;

public class CuadrantePersonalController : ODataController
{
    private readonly bdERP db;
    private CalendarioController cc;

    public readonly byte idCalendario_EstadoSinEvento = 0;

    public CuadrantePersonalController(bdERP context)
    {
        db = context;
        cc = new(context);
    }

    [HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/GetDataCuadrantePersonal")]
    [Authorize]
    public async Task<ActionResult> GetDataCuadrantePersonal([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int? idTipoTrabajo, [FromODataUri] int idTurno)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        bool enableDatosRRHH = db.tblUsuario.AsNoTracking().FirstOrDefault(x => x.idUsuario == idUsuario)?.enableDatosRRHH ?? false;

        var tblPersona = getTblPersona(idUsuario, idLavanderia, fechaDesde, fechaHasta, enableDatosRRHH, idTurno, idTipoTrabajo)
            .Include(p => p.idCategoriaInternaNavigation)
            .Include(p => p.idTurnoNavigation)
            .ToList();

        var calendarioPersonal = (await cc.Handle_GetCalendarioPersonal(fechaDesde, fechaHasta, tblPersona, true)).SelectMany(cp => cp.eventos);

        var calendarioLavanderia = (await cc.Handle_GetCalendarioLavanderia(fechaDesde, fechaHasta, idLavanderia)).SelectMany(cl => cl.eventos).Where(cl => cl.idCalendario_Estado != idCalendario_EstadoSinEvento);

        var festivos = calendarioLavanderia.Where(cl => cl.idCalendario_Estado == (byte)idsCalendario_Estado.Festivo);

        var tblJornada = getTblJornada(idUsuario, fechaDesde, fechaHasta, enableDatosRRHH, idTipoTrabajo, idTurno).ToList() ?? new List<tblJornada>();

        var tblCuadrantePersonal = db.tblCuadrantePersonal
            .Where(cupe =>
                calendarioPersonal.Select(cape => cape.idCuadrantePersonal).Contains(cupe.idCuadrantePersonal)
                || tblJornada.Select(j => j.idCuadrantePersonal).Contains(cupe.idCuadrantePersonal)
            )
            .ToList();

        var tblDiasLibresPersonal = db.tblDiasLibresPersonal.Where(dlp => tblPersona.Select(p => (int?)p.idPersona).Contains(dlp.idPersona));

        var tblCalendario_Estado = db.tblCalendario_Estado.ToList();

        var tblTurno = db.tblTurno.Where(t => t.idLavanderia == idLavanderia && t.activo == true && !t.eliminado).ToList();

        var hoy = DateTime.Today;

        var llamamientosAsociados = db.tblLlamamiento
            .Where(l =>
                l.activo == true
                && l.idPersona != null
                && l.idLavanderia == idLavanderia
                && l.fechaIni.Date <= fechaHasta.Date
                && l.fechaIni.Date > hoy.Date
                && l.idTurno == idTurno
                && (idTipoTrabajo == null || l.idTipoTrabajo == idTipoTrabajo)
            )
            .ToList();

        var dataCuadrantePersonal = (
            from cape in calendarioPersonal

            join p in tblPersona
            on cape.idPersona equals p.idPersona

            join la in llamamientosAsociados
            on p.idPersona equals la.idPersona into laGroup
            from la in laGroup.DefaultIfEmpty()

            let jornadas = tblJornada
                .Where(j => j.idPersona == p.idPersona && j.fecha.Date == cape.fecha.Date)

            let idCuadrantePersonal = cape.idCuadrantePersonal ?? jornadas.FirstOrDefault(j => j.idCuadrantePersonal != null)?.idCuadrantePersonal

            join cupe in tblCuadrantePersonal
            on idCuadrantePersonal equals cupe.idCuadrantePersonal into cupeGroup
            from cupe in cupeGroup.DefaultIfEmpty()

            join dlp_idDiaSemana in tblDiasLibresPersonal
            on new { idPersona = (int?)cape.idPersona, idDiaSemana = (byte?)cape.fecha.DayOfWeek } equals new { dlp_idDiaSemana.idPersona, idDiaSemana = dlp_idDiaSemana.idDiaSemana == 7 ? 0 : dlp_idDiaSemana.idDiaSemana } into dlp_idDiaSemanaGroup
            from dlp_idDiaSemana in dlp_idDiaSemanaGroup.DefaultIfEmpty()

            join dlp_idDiaMes in tblDiasLibresPersonal
            on new { idPersona = (int?)cape.idPersona, idDiaMes = (int?)cape.fecha.Day } equals new { dlp_idDiaMes.idPersona, dlp_idDiaMes.idDiaMes } into dlp_idDiaMesGroup
            from dlp_idDiaMes in dlp_idDiaMesGroup.DefaultIfEmpty()

            let isEstimacion = cape.idCalendario_Estado == idCalendario_EstadoSinEvento

            join f in festivos
            on new { isEstimacion, cape.fecha.Date } equals new { isEstimacion = true, f.fecha.Date } into fGroup
            from f in fGroup.DefaultIfEmpty()

            let isDiaLibre = dlp_idDiaSemana != null || dlp_idDiaMes != null
            let idCalendario_Estado = isEstimacion
                ? cc.GetEstadoByDiaLibre(isDiaLibre, f != null)
                : cape.idCalendario_Estado
            let idTurnoFinal = isEstimacion ? cupe?.idTurno ?? p.idTurno : cape.idTurno
            let estadoJornada = GetEstadoJornada(tblJornada, cape.fecha, idCalendario_Estado, p.idPersona, null, null, cupe)

            join ce in tblCalendario_Estado
            on idCalendario_Estado equals ce.idCalendario_Estado into ceGroup
            from ce in ceGroup.DefaultIfEmpty()

            join t in tblTurno
            on idTurnoFinal equals t.idTurno

            where
                (t.idTurno == idTurno || t.idTurnoPadre == idTurno)
                && cape.idLavanderia == idLavanderia
                && (idTipoTrabajo == null || p.idTipoTrabajo == idTipoTrabajo)
                && (cape.isContratado || (la != null && cape.fecha.Date >= la.fechaIni.Date))

            select new
            {
                cape.fecha,
                idCalendario_Estado,
                idPersona = (int?)cape.idPersona,
                p.idTipoTrabajo,
                idLlamamiento = (int?)null,
                fechaIni = (DateTime?)null,
                la?.codigoLlamamiento,
                nombreCompleto = $"{p.nombre} {p.apellidos}",
                categoriaInterna = p.idCategoriaInternaNavigation?.denominacion ?? "SIN CATEGORÍA INTERNA",
                isBloqueado = cape.isContratado ? cape.isBloqueado : false,
                colorHexa = ce?.colorHexa ?? "",
                cape.idLavanderia,
                idCuadrantePersonal,
                cupe?.idPosicionNAreaLavanderiaNLavanderia,
                cape.idJornada,
                isJornadaRevisada = jornadas.Any() && jornadas.All(j => j.isRevisado),
                estadoJornada,
                idTurno = (int?)t.idTurno,
                horaEntradaEstimada = cupe?.horaEntrada ?? p.idTurnoNavigation?.horaEntrada,
                horaSalidaEstimada = cupe?.horaSalida ?? p.idTurnoNavigation?.horaSalida,
                horaEntrada = isEstimacion ? cupe?.idTurnoNavigation?.horaEntrada ?? p.idTurnoNavigation?.horaEntrada : cape.horaEntrada,
                horaSalida = isEstimacion ? cupe?.idTurnoNavigation?.horaSalida ?? p.idTurnoNavigation?.horaSalida : cape.horaSalida,
                tiempoDescanso = isEstimacion ? cupe?.idTurnoNavigation?.descanso ?? p.idTurnoNavigation?.descanso : cape.tiempoDescanso,
                isEstimacion
            }
        )
        .ToList();

        var estimacionLlamamientos = GetEstimacionLlamamientos();

        dataCuadrantePersonal.AddRange(estimacionLlamamientos);

        var dataCuadrantePersonalFinal = new
        {
            personas = dataCuadrantePersonal
                .Where(dcp => dcp.idPersona != null)
                .GroupBy(dcp => (int)dcp.idPersona)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(
                        dcp => new DateTimeOffset(dcp.fecha).ToUnixTimeMilliseconds(),
                        dcp => dcp
                    )
                ),

            llamamientos = dataCuadrantePersonal
                .Where(dcp => dcp.idLlamamiento != null)
                .GroupBy(dcp => (int)dcp.idLlamamiento)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(
                        dcp => new DateTimeOffset(dcp.fecha).ToUnixTimeMilliseconds(),
                        dcp => dcp
                    )
                )
        };

        var resumenNFecha = GetResumenNFecha();

        var calendarioLavanderiaFinal = calendarioLavanderia
            .GroupBy(cl => cl.fecha)
            .ToDictionary(
                g => new DateTimeOffset(g.Key).ToUnixTimeMilliseconds(),
                g => g.First()
            );

        var personasCuadrantePersonal = dataCuadrantePersonal
            .GroupBy(dcp => new
            {
                dcp.idPersona,
                dcp.idLlamamiento,
                dcp.nombreCompleto,
                dcp.codigoLlamamiento,
                dcp.idTipoTrabajo,
                dcp.idLavanderia,
                dcp.categoriaInterna
            })
            .Select(g => new
            {
                g.Key.idPersona,
                g.Key.idLlamamiento,
                g.Key.nombreCompleto,
                g.Key.codigoLlamamiento,
                g.Key.idTipoTrabajo,
                g.Key.idLavanderia,
                g.Key.categoriaInterna,
                fechas = g.Select(dcp => new DateTimeOffset(dcp.fecha).ToUnixTimeMilliseconds())
            })
            .OrderBy(p => p.codigoLlamamiento)
            .ThenBy(p => p.nombreCompleto)
            .ToList();

        return Ok(new { dataCuadrantePersonal = dataCuadrantePersonalFinal, resumenNFecha, calendarioLavanderia = calendarioLavanderiaFinal, personasCuadrantePersonal });

        dynamic GetEstimacionLlamamientos()
        {
            var dias = calendarioPersonal.Select(cp => cp.fecha.Date).Distinct().ToList();

            var tblLlamamiento = db.tblLlamamiento
                .Include(l => l.idCategoriaInternaNavigation)
                .Include(l => l.idTurnoNavigation)
                .Include(l => l.tblDiasLibresPersonal_Llamamiento)
                .Where(l =>
                    l.activo == true
                    && l.idPersona == null
                    && l.idLavanderia == idLavanderia
                    && l.fechaIni.Date <= fechaHasta.Date
                    && l.idTurno == idTurno
                    && (idTipoTrabajo == null || l.idTipoTrabajo == idTipoTrabajo)
                )
                .ToList();

            var tblDiasLibresPersonal_Llamamiento = tblLlamamiento.SelectMany(l => l.tblDiasLibresPersonal_Llamamiento);

            var festivosNLavanderia = calendarioLavanderia.Where(cl => cl.idCalendario_Estado == (byte)idsCalendario_Estado.Festivo);

            return (
                from d in dias

                from l in tblLlamamiento

                join dlp_idDiaSemana in tblDiasLibresPersonal_Llamamiento
                on new { l.idLlamamiento, idDiaSemana = (byte?)d.DayOfWeek } equals new { dlp_idDiaSemana.idLlamamiento, idDiaSemana = dlp_idDiaSemana.idDiaSemana == 7 ? 0 : dlp_idDiaSemana.idDiaSemana } into dlp_idDiaSemanaGroup
                from dlp_idDiaSemana in dlp_idDiaSemanaGroup.DefaultIfEmpty()

                join dlp_idDiaMes in tblDiasLibresPersonal_Llamamiento
                on new { l.idLlamamiento, idDiaMes = (int?)d.Day } equals new { dlp_idDiaMes.idLlamamiento, dlp_idDiaMes.idDiaMes } into dlp_idDiaMesGroup
                from dlp_idDiaMes in dlp_idDiaMesGroup.DefaultIfEmpty()

                join fnl in festivosNLavanderia
                on d.Date equals fnl.fecha.Date into fnlGroup
                from fnl in fnlGroup.DefaultIfEmpty()

                let isDiaLibre = dlp_idDiaSemana != null || dlp_idDiaMes != null
                let idCalendario_Estado = cc.GetEstadoByDiaLibre(isDiaLibre, fnl != null)
                let estadoJornada = GetEstadoJornada(new List<tblJornada>(), d, idCalendario_Estado, null, l.idLlamamiento, null, null)

                join ce in tblCalendario_Estado
                on idCalendario_Estado equals ce.idCalendario_Estado into ceGroup
                from ce in ceGroup.DefaultIfEmpty()

                where
                    d.Date >= l.fechaIni.Date

                select new
                {
                    fecha = d.Date,
                    idCalendario_Estado,
                    idPersona = (int?)null,
                    idTipoTrabajo = (byte?)l.idTipoTrabajo,
                    idLlamamiento = (int?)l.idLlamamiento,
                    fechaIni = (DateTime?)l.fechaIni,
                    l.codigoLlamamiento,
                    nombreCompleto = "",
                    categoriaInterna = l.idCategoriaInternaNavigation?.denominacion ?? "SIN CATEGORÍA INTERNA",
                    isBloqueado = false,
                    colorHexa = ce?.colorHexa ?? "",
                    idLavanderia = (int?)l.idLavanderia,
                    idCuadrantePersonal = (int?)null,
                    idPosicionNAreaLavanderiaNLavanderia = (short?)null,
                    idJornada = (int?)null,
                    isJornadaRevisada = false,
                    estadoJornada,
                    idTurno = (int?)l.idTurno,
                    horaEntradaEstimada = (TimeSpan?)l.idTurnoNavigation.horaEntrada,
                    horaSalidaEstimada = (TimeSpan?)l.idTurnoNavigation.horaSalida,
                    horaEntrada = (TimeSpan?)l.idTurnoNavigation.horaEntrada,
                    horaSalida = (TimeSpan?)l.idTurnoNavigation.horaSalida,
                    tiempoDescanso = (TimeSpan?)l.idTurnoNavigation.descanso,
                    isEstimacion = true
                }
            );
        }

        dynamic GetResumenNFecha()
        {
            return dataCuadrantePersonal
                .GroupBy(dcp => dcp.fecha)
                .ToDictionary(
                    g => new DateTimeOffset(g.Key).ToUnixTimeMilliseconds(),
                    g =>
                    {
                        var diasTrabajadosPorProduccion = g
                            .Where(dcp =>
                                dcp.idLlamamiento == null
                                && dcp.idTipoTrabajo == (byte)idsTipoTrabajo.Produccion
                                && (
                                    dcp.idCalendario_Estado == (byte)idsCalendario_Estado.DiaTrabajado
                                    || dcp.idCalendario_Estado == (byte)idsCalendario_Estado.FestivoTrabajado
                                )
                            );

                        var estadoEstructuraOperativa = diasTrabajadosPorProduccion.Any()
                            ? diasTrabajadosPorProduccion.Any(dcp => dcp.isEstimacion)
                                ? new { tipo = "warning", estado = "Hay previsiones sin fijar" }
                                : diasTrabajadosPorProduccion.Any(dcp => dcp.idPosicionNAreaLavanderiaNLavanderia == null)
                                        ? new { tipo = "warning", estado = "Estructura operativa incompleta" }
                                        : new { tipo = "valido", estado = "Estructura operativa completa" }
                            : new { tipo = "omitir", estado = "Sin estructura operativa" };

                        return new
                        {
                            fecha = g.Key,
                            contratado = g.Count(),
                            estadoEstructuraOperativa,
                            estadoNCategoria = g
                                .GroupBy(dcp => dcp.categoriaInterna)
                                .ToDictionary(
                                    g => g.Key,
                                    g => g
                                    .GroupBy(dcp => dcp.idCalendario_Estado)
                                    .ToDictionary(
                                        g => g.Key,
                                        g => g.Count()
                                    )
                                ),
                            total = g
                                .GroupBy(dcp => dcp.idCalendario_Estado)
                                .ToDictionary(
                                    g => g.Key,
                                    g => g.Count()
                                )
                        };
                    }
                );
        }
    }

    [HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/GetDataCalendarioCuadrante")]
    [Authorize]
    public async Task<ActionResult> GetDataCalendarioCuadrante([FromODataUri] int idPersona, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        bool enableDatosRRHH = db.tblUsuario.AsNoTracking().FirstOrDefault(x => x.idUsuario == idUsuario)?.enableDatosRRHH ?? false;

        var persona = db.tblPersona
            .Include(p => p.idCategoriaInternaNavigation)
            .Include(p => p.idTurnoNavigation)
            .FirstOrDefault(p => p.idPersona == idPersona);

        if (persona == null)
        {
            return BadRequest();
        }

        var calendarioPersonalConLavanderia = (await cc.Handle_GetCalendarioPersonalConLavanderia(fechaDesde, fechaHasta, persona.idPersona, false)).SelectMany(cpcl => cpcl.eventos).ToList();

        var festivos = calendarioPersonalConLavanderia.Where(cpcl => cpcl.idCalendario_Estado == (byte)idsCalendario_Estado.Festivo);

        var tblJornada = db.tblJornada
            .Where(j =>
                j.idPersona == idPersona
                && j.fecha.Date >= fechaDesde.Date
                && j.fecha.Date <= fechaHasta.Date
            ).ToList();

        var idsCuadrantePersonal = calendarioPersonalConLavanderia.Select(cpcl => (int?)cpcl.idCuadrantePersonal).ToList();

        var tblCuadrantePersonal = db.tblCuadrantePersonal
            .Where(cupe =>
                idsCuadrantePersonal.Contains(cupe.idCuadrantePersonal)
                || tblJornada.Select(j => j.idCuadrantePersonal).Contains(cupe.idCuadrantePersonal)
            ).ToList();

        var tblDiasLibresPersonal = db.tblDiasLibresPersonal.Where(dlp => dlp.idPersona == persona.idPersona);

        var tblCalendario_Estado = db.tblCalendario_Estado.ToList();

        var hoy = DateTime.Today;

        var dataCuadrantePersonal = (
            from cpcl in calendarioPersonalConLavanderia.Where(cpcl => cpcl.idPersona != null)

            join j in tblJornada
            on cpcl.idJornada equals j.idJornada into jGroup
            from j in jGroup.DefaultIfEmpty()

            join cupe in tblCuadrantePersonal
            on cpcl.idCuadrantePersonal ?? j?.idCuadrantePersonal equals cupe.idCuadrantePersonal into cupeGroup
            from cupe in cupeGroup.DefaultIfEmpty()

            join dlp_idDiaSemana in tblDiasLibresPersonal
            on new { idPersona = (int?)cpcl.idPersona, idDiaSemana = (byte?)cpcl.fecha.DayOfWeek } equals new { dlp_idDiaSemana.idPersona, idDiaSemana = dlp_idDiaSemana.idDiaSemana == 7 ? 0 : dlp_idDiaSemana.idDiaSemana } into dlp_idDiaSemanaGroup
            from dlp_idDiaSemana in dlp_idDiaSemanaGroup.DefaultIfEmpty()

            join dlp_idDiaMes in tblDiasLibresPersonal
            on new { idPersona = (int?)cpcl.idPersona, idDiaMes = (int?)cpcl.fecha.Day } equals new { dlp_idDiaMes.idPersona, dlp_idDiaMes.idDiaMes } into dlp_idDiaMesGroup
            from dlp_idDiaMes in dlp_idDiaMesGroup.DefaultIfEmpty()

            let isEstimacion = (byte)cpcl.idCalendario_Estado == idCalendario_EstadoSinEvento

            join f in festivos
            on new { isEstimacion, fecha = (DateTime)cpcl.fecha.Date } equals new { isEstimacion = true, fecha = (DateTime)f.fecha.Date } into clGroup
            from f in clGroup.DefaultIfEmpty()

            let isDiaLibre = dlp_idDiaSemana != null || dlp_idDiaMes != null
            let idCalendario_Estado = isEstimacion ? cc.GetEstadoByDiaLibre(isDiaLibre, f != null) : cpcl.idCalendario_Estado
            let idTurnoFinal = isEstimacion ? cupe?.idTurno ?? persona.idTurno : cpcl.idTurno

            join ce in tblCalendario_Estado
            on idCalendario_Estado equals ce.idCalendario_Estado into ceGroup
            from ce in ceGroup.DefaultIfEmpty()

            where
                !isEstimacion
                || cpcl.fecha.Date > hoy.Date

            select new
            {
                cpcl.fecha,
                idCalendario_Estado,
                idPersona = (int?)cpcl.idPersona,
                cpcl.isBloqueado,
                colorHexa = ce?.colorHexa ?? "",
                cpcl.idLavanderia,
                cpcl.idCuadrantePersonal,
                cupe?.idPosicionNAreaLavanderiaNLavanderia,
                idTurno = idTurnoFinal,
                horaEntrada = isEstimacion ? cupe?.idTurnoNavigation?.horaEntrada ?? persona.idTurnoNavigation?.horaEntrada : cpcl.horaEntrada,
                horaSalida = isEstimacion ? cupe?.idTurnoNavigation?.horaSalida ?? persona.idTurnoNavigation?.horaSalida : cpcl.horaSalida,
                isEstimacion
            }
        )
        .Cast<dynamic>()
        .ToList();

        dataCuadrantePersonal.AddRange(calendarioPersonalConLavanderia.Where(cpcl => cpcl.idPersona == null));

        return Ok(dataCuadrantePersonal);
    }

    [HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/GetDataJornadaPopup")]
    [Authorize]
    public async Task<ActionResult> GetDataJornadaPopup([FromODataUri] int idPersona, [FromODataUri] DateTime fecha)
    {
        var persona = db.tblPersona.FirstOrDefault(p => p.idPersona == idPersona);

        if (persona == null)
        {
            return BadRequest();
        }

        var calendarioPersonal = (await cc.Handle_GetCalendarioPersonal(fecha, fecha, new List<tblPersona>() { persona })).SelectMany(cp => cp.eventos).FirstOrDefault();

        var tblJornada = db.tblJornada
            .Where(j => j.idPersona == idPersona && j.fecha.Date == fecha.Date)
            .Include(j => j.idCuadrantePersonalNavigation)
                .ThenInclude(cp => cp != null ? cp.idTurnoNavigation : null)
            .Include(j => j.tblBalanceHoras)
            .Include(j => j.tblBalanceHorasExtra)
            .Include(j => j.tblEventoPersona)
            .ToList();

        var cuadrantePersonal = db.tblCuadrantePersonal
            .Include(cp => cp.idTurnoNavigation)
            .FirstOrDefault(cp => calendarioPersonal != null && calendarioPersonal.idCuadrantePersonal == cp.idCuadrantePersonal);

        cuadrantePersonal ??= tblJornada.FirstOrDefault(j => j.idCuadrantePersonal != null)?.idCuadrantePersonalNavigation;

        foreach (var jornada in tblJornada)
        {
            AgregarBalanceHoras(true);
            AgregarBalanceHoras(false);
            AgregarBalanceHorasExtra(true);
            AgregarBalanceHorasExtra(false);

            void AgregarBalanceHoras(bool isInicio)
            {
                if (!jornada.tblBalanceHoras.Any(bh => bh.isInicio == isInicio))
                {
                    jornada.tblBalanceHoras.Add(new()
                    {
                        idBalanceHoras = 0,
                        idPersona = idPersona,
                        idJornada = jornada.idJornada,
                        fecha = fecha,
                        minutos = 0,
                        isInicio = isInicio
                    });
                }
            }

            void AgregarBalanceHorasExtra(bool isInicio)
            {
                if (!jornada.tblBalanceHorasExtra.Any(bh => bh.isInicio == isInicio))
                {
                    jornada.tblBalanceHorasExtra.Add(new()
                    {
                        idBalanceHorasExtra = 0,
                        idPersona = idPersona,
                        idJornada = jornada.idJornada,
                        fecha = fecha,
                        minutos = 0,
                        isInicio = isInicio
                    });
                }
            }
        }

        var jornadas = tblJornada
            .Select(j =>
            {
                var estadoHorario = GetEstadoHorarioJornada(j, cuadrantePersonal);

                return new
                {
                    j.idJornada,
                    j.idPersona,
                    j.idCuadrantePersonal,
                    j.fecha,
                    j.idTurno,
                    j.idTipoTrabajo,
                    j.idLavanderia,
                    j.horaIni,
                    j.horaFin,
                    j.tiempoDescanso,
                    j.horasDiarias,
                    j.idMotivoIncumplimiento_horaFin,
                    j.idMotivoIncumplimiento_horaIni,
                    j.idMotivoIncumplimiento_tiempoDescanso,
                    j.isRegValido,
                    j.isRevisado,
                    j.idUsuario_validacion,
                    j.fecha_validacion,
                    tblBalanceHoras = j.tblBalanceHoras.Select(bh => new
                    {
                        bh.idBalanceHoras,
                        bh.idPersona,
                        bh.idJornada,
                        bh.fecha,
                        bh.minutos,
                        bh.isInicio,
                    }),
                    tblBalanceHorasExtra = j.tblBalanceHorasExtra.Select(bhe => new
                    {
                        bhe.idBalanceHorasExtra,
                        bhe.idPersona,
                        bhe.idJornada,
                        bhe.fecha,
                        bhe.minutos,
                        bhe.isInicio,
                    }),
                    tblEventoPersona = j.tblEventoPersona.Select(ep => new
                    {
                        ep.idPersona,
                        ep.fecha,
                        ep.idEventoPersona_Estado,
                        ep.idLavanderia,
                        ep.isRegManual,
                        ep.isOffline,
                        ep.idJornada,
                        ep.isRevisado,
                    }),
                    estadoHorario,
                    estadoJornada = GetEstadoJornada(tblJornada, fecha, calendarioPersonal?.idCalendario_Estado ?? idCalendario_EstadoSinEvento, idPersona, null, j.idJornada, cuadrantePersonal, estadoHorario)
                };
            });

        var result = new
        {
            cuadrantePersonal = cuadrantePersonal != null
                ? new
                {
                    cuadrantePersonal.idCuadrantePersonal,
                    cuadrantePersonal.idPersona,
                    cuadrantePersonal.fecha,
                    cuadrantePersonal.idCalendario_Estado,
                    cuadrantePersonal.horaEntrada,
                    cuadrantePersonal.horaSalida,
                    cuadrantePersonal.idTurno,
                    cuadrantePersonal.idPosicionNAreaLavanderiaNLavanderia,
                    cuadrantePersonal.idLavanderia
                }
                : null,
            jornadas,
            estadoJornada = GetEstadoJornada(tblJornada, fecha, calendarioPersonal?.idCalendario_Estado ?? idCalendario_EstadoSinEvento, idPersona, null, null, cuadrantePersonal)
        };

        return Ok(result);
    }

    [HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/GetEstimacion")]
    [Authorize]
    public async Task<ActionResult> GetEstimacion([FromODataUri] int idPersona, [FromODataUri] DateTime fecha, [FromODataUri] int idLavanderia, [FromODataUri] byte idTurno)
    {
        var calendarioLavanderia = (await cc.Handle_GetCalendarioLavanderia(fecha, fecha, idLavanderia)).SelectMany(cl => cl.eventos);

        var isFestivo = calendarioLavanderia.FirstOrDefault(cl => cl.fecha.Date == fecha.Date)?.idCalendario_Estado == (byte)idsCalendario_Estado.Festivo;

        var idCalendario_Estado = isFestivo
            ? (byte)idsCalendario_Estado.FestivoTrabajado
            : (byte)idsCalendario_Estado.DiaTrabajado;

        var turno = db.tblTurno.FirstOrDefault(t => t.idTurno == idTurno);

        var result = new
        {
            idCuadrantePersonal = 0,
            idPersona,
            fecha,
            idCalendario_Estado,
            turno?.horaEntrada,
            turno?.horaSalida,
            idTurno,
            idPosicionNAreaLavanderiaNLavanderia = (short?)null,
            idLavanderia
        };

        return Ok(result);
    }

    //[HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/PrevisionCuadrantePersonal")]
    //[Authorize]
    //public async Task<ActionResult> PrevisionCuadrantePersonal([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    //{
    //    var connection = db.Database.GetDbConnection();
    //    var results = await connection.QueryAsync("EXEC [RRHH].[EF_spSelectCuadrantePersonal] @idLavanderia, @fechaDesde, @fechaHasta",
    //        new { idLavanderia, fechaDesde, fechaHasta });

    //    return Ok(results);
    //}

    //[HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/PrevisionCuadrantePersonal_Persona")]
    //[Authorize]
    //public async Task<ActionResult> PrevisionCuadrantePersonal_Persona([FromODataUri] int idPersona, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int? idLavanderia, [FromODataUri] int? idTurno)
    //{
    //    var connection = db.Database.GetDbConnection();
    //    var results = await connection.QueryAsync("EXEC [RRHH].[EF_spSelectCuadrantePersonal] @idLavanderia, @fechaDesde, @fechaHasta, @idPersona, @idTurno",
    //        new { idLavanderia, fechaDesde, fechaHasta, idPersona, idTurno });

    //    return Ok(results);
    //}

    [EnableQuery]
    [HttpPost("odata/MyPolarier/RRHH/CuadrantePersonal/IUD_CuadrantePersonal")]
    [RequestSizeLimit(1_000_000_000)]
    [Authorize]
    public async Task<ActionResult> IUD_CuadrantePersonal([FromBody] List<tblCuadrantePersonal> cuadrantes, int? idUsuario = null)
    {
        idUsuario ??= int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var hoy = DateTimeOffset.UtcNow;

        var idsLavanderia = cuadrantes.Select(c => c.idLavanderia).Distinct();
        var fechasCuadrantes = cuadrantes.Select(c => c.fecha.Date).Distinct();
        var fechasCalendario = fechasCuadrantes.Where(fc => fc.Date <= hoy.Date);
        var idsPersona = cuadrantes.Select(c => c.idPersona);

        var tblCalendarioLavanderia = db.tblCalendarioLavanderia.Where(cl => fechasCuadrantes.Contains(cl.fecha.Date) && idsLavanderia.Contains(cl.idLavanderia) && cl.idCalendario_Estado == (byte)idsCalendario_Estado.Festivo).ToList();
        var tblCuadrantePersonal = db.tblCuadrantePersonal.Where(cp => fechasCuadrantes.Contains(cp.fecha) && idsPersona.Contains(cp.idPersona)).ToList();
        var tblCalendarioPersonal = db.tblCalendarioPersonal.Where(cp => fechasCalendario.Contains(cp.fecha) && idsPersona.Contains(cp.idPersona)).ToList();

        List<tblCuadrantePersonal> cuadreantesFinal = new();

        foreach (var c in cuadrantes)
        {
            var isFestivo = tblCalendarioLavanderia.FirstOrDefault(cl => cl.fecha.Date == c.fecha.Date && cl.idLavanderia == c.idLavanderia) != null;

            var idCalendario_Estado = cc.GetEstadoByFestivo(isFestivo, c.idCalendario_Estado);

            tblCuadrantePersonal cuadranteNuevo = new()
            {
                idCuadrantePersonal = 0,
                idPersona = c.idPersona,
                fecha = c.fecha,
                idCalendario_Estado = idCalendario_Estado,
                horaEntrada = c.horaEntrada,
                horaSalida = c.horaSalida,
                idTurno = c.idTurno,
                idPosicionNAreaLavanderiaNLavanderia = c.idPosicionNAreaLavanderiaNLavanderia,
                idLavanderia = c.idLavanderia,
                idUsuario_validacion = idUsuario,
                fecha_validacion = hoy
            };

            var cuadranteActual = tblCuadrantePersonal.FirstOrDefault(cp => cp.idPersona == c.idPersona && cp.fecha.Date == c.fecha.Date);

            if (cuadranteActual == null)
            {
                cuadranteActual = cuadranteNuevo;
                db.tblCuadrantePersonal.Add(cuadranteNuevo);
            }
            else
            {
                cuadranteActual.idCalendario_Estado = cuadranteNuevo.idCalendario_Estado;
                cuadranteActual.horaEntrada = cuadranteNuevo.horaEntrada;
                cuadranteActual.horaSalida = cuadranteNuevo.horaSalida;
                cuadranteActual.idTurno = cuadranteNuevo.idTurno;
                cuadranteActual.idPosicionNAreaLavanderiaNLavanderia = cuadranteNuevo.idPosicionNAreaLavanderiaNLavanderia;
                cuadranteActual.idLavanderia = cuadranteNuevo.idLavanderia;
                cuadranteActual.idUsuario_validacion = idUsuario;
                cuadranteActual.fecha_validacion = hoy;
            }

            cuadreantesFinal.Add(cuadranteActual);


            if (c.fecha.Date <= hoy.Date)
            {
                if (idCalendario_Estado == null)
                {
                    continue;
                }

                tblCalendarioPersonal eventoNuevo = new()
                {
                    idPersona = c.idPersona,
                    fecha = c.fecha,
                    idCalendario_Estado = (byte)idCalendario_Estado,
                    idLavanderia = c.idLavanderia,
                    idCuadrantePersonalNavigation = cuadranteActual,
                    idUsuario_validacion = idUsuario,
                    fecha_validacion = hoy
                };

                var eventoActual = tblCalendarioPersonal.FirstOrDefault(cp => cp.idPersona == c.idPersona && cp.fecha.Date == c.fecha.Date);

                if (eventoActual == null)
                {
                    db.tblCalendarioPersonal.Add(eventoNuevo);

                    continue;
                }

                eventoActual.idCalendario_Estado = eventoNuevo.idCalendario_Estado;
                eventoActual.idLavanderia = eventoNuevo.idLavanderia;
                eventoActual.idCuadrantePersonalNavigation = eventoNuevo.idCuadrantePersonalNavigation;
                eventoActual.idUsuario_validacion = eventoNuevo.idUsuario_validacion;
                eventoActual.fecha_validacion = eventoNuevo.fecha_validacion;
            }
        }

        await db.SaveChangesAsync();

        return Ok(cuadreantesFinal);
    }

    [EnableQuery]
    [HttpPost("odata/MyPolarier/RRHH/CuadrantePersonal/RecalcularCuadrantePersonal")]
    [RequestSizeLimit(1_000_000_000)]
    [Authorize]
    public async Task<ActionResult> RecalcularCuadrantePersonal([FromBody] List<tblCuadrantePersonal> cuadrantes)
    {
        var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var hoy = DateTimeOffset.UtcNow;

        if (!cuadrantes.Any() || cuadrantes.Any(c => c.fecha.Date <= hoy.Date || c.idCuadrantePersonal == 0))
        {
            return BadRequest();
        }

        var idLavanderia = cuadrantes.First().idLavanderia;
        var idsPersona = cuadrantes.Select(p => (int?)p.idPersona).Distinct();
        var idsTurno = cuadrantes.Select(c => c.idTurno).Distinct();

        var calendarioLavanderia = (await cc.Handle_GetCalendarioLavanderia(cuadrantes.Min(c => c.fecha), cuadrantes.Max(c => c.fecha), idLavanderia)).SelectMany(cl => cl.eventos);

        var tblDiasLibresPersonal = db.tblDiasLibresPersonal.Where(dlp => idsPersona.Contains(dlp.idPersona));
        var tblTurno = db.tblTurno.Where(t => idsTurno.Contains(t.idTurno)).ToList();

        foreach (var c in cuadrantes)
        {
            var isDiaLibre = tblDiasLibresPersonal
                .Any(dlp =>
                    dlp.idPersona == c.idPersona
                    && (
                        (dlp.idDiaSemana == 7 ? 0 : dlp.idDiaSemana) == (byte)c.fecha.DayOfWeek
                        || dlp.idDiaMes == c.fecha.Day
                    )
                );

            var isFestivo = calendarioLavanderia.FirstOrDefault(cl => cl.fecha.Date == c.fecha.Date)?.idCalendario_Estado == (byte)idsCalendario_Estado.Festivo;

            var idCalendario_Estado = cc.GetEstadoByDiaLibre(isDiaLibre, isFestivo);

            var turno = tblTurno.FirstOrDefault(t => t.idTurno == c.idTurno);

            tblCuadrantePersonal cuadrante = new()
            {
                idCuadrantePersonal = c.idCuadrantePersonal,
                idPersona = c.idPersona,
                fecha = c.fecha,
                idCalendario_Estado = idCalendario_Estado,
                horaEntrada = isDiaLibre ? null : turno?.horaEntrada,
                horaSalida = isDiaLibre ? null : turno?.horaSalida,
                idTurno = c.idTurno,
                idPosicionNAreaLavanderiaNLavanderia = c.idPosicionNAreaLavanderiaNLavanderia,
                idLavanderia = c.idLavanderia,
                idUsuario_validacion = idUsuario,
                fecha_validacion = hoy
            };

            db.tblCuadrantePersonal.Update(cuadrante);
        }

        await db.SaveChangesAsync();

        return Ok(true);
    }

    [EnableQuery]
    [HttpPatch("odata/MyPolarier/RRHH/CuadrantePersonal/CambiarPosicion")]
    [Authorize]
    public async Task<ActionResult> CambiarPosicion([FromODataUri] int idCuadrantePersonal, [FromODataUri] short idPosicionNAreaLavanderiaNLavanderia)
    {

        var entity = db.tblCuadrantePersonal.First(x => x.idCuadrantePersonal == idCuadrantePersonal);
        if (entity == null) return BadRequest();

        List<tblCuadrantePersonal> lista = db.tblCuadrantePersonal.Where(x => x.idPosicionNAreaLavanderiaNLavanderia == idPosicionNAreaLavanderiaNLavanderia && x.fecha == entity.fecha && x.idTurno == entity.idTurno).ToList();

        foreach (var item in lista)
        {
            item.idPosicionNAreaLavanderiaNLavanderia = entity.idPosicionNAreaLavanderiaNLavanderia;
        }

        entity.idPosicionNAreaLavanderiaNLavanderia = idPosicionNAreaLavanderiaNLavanderia;
        lista.Add(entity);

        await db.SaveChangesAsync();

        return Ok(lista);
    }

    #region Llamadas get

    [EnableQuery]
    [HttpGet]
    [Route("odata/MyPolarier/RRHH/CuadrantePersonal/GetPersonas")]
    [Authorize]
    public async Task<IQueryable<PersonaCuadrante>> GetPersonas([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int? idTipoTrabajo, [FromODataUri] int idTurno)
    {

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        bool enableDatosRRHH = db.tblUsuario.AsNoTracking().FirstOrDefault(x => x.idUsuario == idUsuario)?.enableDatosRRHH ?? false;

        var tblPersona = getTblPersona(idUsuario, idLavanderia, fechaDesde, fechaHasta, enableDatosRRHH, idTurno, idTipoTrabajo)
                        .Select(x => new PersonaCuadrante
                        {
                            idPersona = x.idPersona,
                            nombreCompleto = x.nombre + " " + x.apellidos,
                            categoriaInterna = x.idCategoriaInternaNavigation.denominacion,
                            idLavanderia = x.idLavanderia,
                            idTurno = x.idTurno,
                            idTipoTrabajo = x.idTipoTrabajo,
                            activo = x.activo
                        });

        return tblPersona;
    }

    //[HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/GetCuadrantes")]
    //[Authorize]
    //public async Task<IQueryable<dynamic>> GetCuadrantes([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int idTurno)
    //{

    //    int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
    //    bool enableDatosRRHH = db.tblUsuario.AsNoTracking().FirstOrDefault(x => x.idUsuario == idUsuario)?.enableDatosRRHH ?? false;

    //    var tblCuadrante = getTblCuadrantePersonal(idUsuario, fechaDesde, fechaHasta, enableDatosRRHH, idTurno).Select(x => new
    //    {
    //        x.idCuadrantePersonal,
    //        x.idPersona,
    //        x.fecha,
    //        x.idCalendario_Estado,
    //        x.horaEntrada,
    //        x.horaSalida,
    //        x.idTurno,
    //        x.idPosicionNAreaLavanderiaNLavanderia,
    //        x.idLavanderia,
    //        x.isCorregido
    //    });

    //    return tblCuadrante;
    //}

    //[HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/GetJornadas")]
    //[Authorize]
    //public async Task<IQueryable<dynamic>> GetJornadas([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int? idTipoTrabajo, [FromODataUri] int idTurno)
    //{

    //    int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
    //    bool enableDatosRRHH = db.tblUsuario.AsNoTracking().FirstOrDefault(x => x.idUsuario == idUsuario)?.enableDatosRRHH ?? false;

    //    var tblJornada = getTblJornada(idUsuario, fechaDesde, fechaHasta, enableDatosRRHH, idTipoTrabajo, idTurno).Select(x => new
    //    {
    //        x.idJornada,
    //        x.idPersona,
    //        x.idCuadrantePersonal,
    //        x.fecha,
    //        x.idTurno,
    //        x.idTipoTrabajo,
    //        x.idLavanderia,
    //        x.horaIni,
    //        x.horaFin,
    //        x.tiempoDescanso,
    //        x.horasDiarias,
    //        x.idMotivoIncumplimiento_horaFin,
    //        x.idMotivoIncumplimiento_horaIni,
    //        x.idMotivoIncumplimiento_tiempoDescanso,
    //        x.isRegValido,
    //        x.isRevisado,
    //        x.idUsuario_validacion,
    //        x.fecha_validacion,
    //        x.tblBalanceHoras,
    //        x.tblBalanceHorasExtra,
    //        x.tblEventoPersona,
    //    });

    //    return tblJornada;
    //}

    //[HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/GetCalendarioPersonal")]
    //[Authorize]
    //public async Task<IQueryable<tblCalendarioPersonal>> GetCalendarioPersonal([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int? idTipoTrabajo, [FromODataUri] int idTurno)
    //{

    //    int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
    //    bool enableDatosRRHH = db.tblUsuario.AsNoTracking().FirstOrDefault(x => x.idUsuario == idUsuario)?.enableDatosRRHH ?? false;

    //    var tblPersona = getTblPersona(idUsuario, idLavanderia, fechaDesde, fechaHasta, enableDatosRRHH, idTurno, idTipoTrabajo).Select(x => x.idPersona).ToList();

    //    var tblCalendarioPersonal = db.tblCalendarioPersonal.Where(evento => tblPersona.Contains(evento.idPersona) && fechaDesde <= evento.fecha && fechaHasta >= evento.fecha);

    //    return tblCalendarioPersonal;
    //}

    //[HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/getDiasLibres")]
    //[Authorize]
    //public async Task<IQueryable<tblDiasLibresPersonal>> getDiasLibres([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int? idTipoTrabajo, [FromODataUri] int idTurno)
    //{

    //    int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
    //    bool enableDatosRRHH = db.tblUsuario.AsNoTracking().FirstOrDefault(x => x.idUsuario == idUsuario)?.enableDatosRRHH ?? false;

    //    var tblPersona = getTblPersona(idUsuario, idLavanderia, fechaDesde, fechaHasta, enableDatosRRHH, idTurno, idTipoTrabajo).Select(x => x.idPersona).ToList();

    //    var tblDiasLibresPersonal = db.tblDiasLibresPersonal.Where(diaLibre => tblPersona.Contains((int)diaLibre.idPersona));

    //    return tblDiasLibresPersonal;
    //}

    //[HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/GetPersonaNTipoContrato")]
    //[Authorize]
    //public async Task<IQueryable<tblPersonaNTipoContrato>> GetPersonaNTipoContrato([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int? idTipoTrabajo, [FromODataUri] int idTurno)
    //{

    //    int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
    //    bool enableDatosRRHH = db.tblUsuario.AsNoTracking().FirstOrDefault(x => x.idUsuario == idUsuario)?.enableDatosRRHH ?? false;

    //    var tblPersona = getTblPersona(idUsuario, idLavanderia, fechaDesde, fechaHasta, enableDatosRRHH, idTurno, idTipoTrabajo).Select(x => x.idPersona).ToList();

    //    var tblPersonaNTipoContrato = db.tblPersonaNTipoContrato.Where(contrato =>
    //        tblPersona.Contains((int)contrato.idPersona) &&
    //        contrato.fechaAltaContrato <= fechaHasta &&
    //        (contrato.fechaBajaContrato == null || contrato.fechaBajaContrato >= fechaDesde)
    //        );

    //    return tblPersonaNTipoContrato;
    //}

    //[HttpGet("odata/MyPolarier/RRHH/CuadrantePersonal/GetEstadoNominaNFecha")]
    //[Authorize]
    //public async Task<List<EstadoBloqueadoNCalendarioPersonal>> GetEstadoNominaNFecha([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int? idTipoTrabajo, [FromODataUri] int idTurno)
    //{
    //    int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
    //    bool enableDatosRRHH = db.tblUsuario.AsNoTracking().FirstOrDefault(x => x.idUsuario == idUsuario)?.enableDatosRRHH ?? false;

    //    var idsPersona = getTblPersona(idUsuario, idLavanderia, fechaDesde, fechaHasta, enableDatosRRHH, idTurno, idTipoTrabajo).Select(x => x.idPersona).ToList();

    //    return cc.GetEstadoBloqueadoNCalendarioPersonal(fechaDesde, fechaHasta, idsPersona);
    //}

    private IQueryable<tblPersona> getTblPersona(int idUsuario, int idLavanderia, DateTime fechaDesde, DateTime fechaHasta, bool enableDatosRRHH, int idTurno, int? idTipoTrabajo)
    {

        var tblCuadrante = getTblCuadrantePersonal(idUsuario, fechaDesde, fechaHasta, enableDatosRRHH, idTurno)
            .Where(x => x.idTurno == idTurno || x.idTurnoNavigation.idTurnoPadre == idTurno)
            .Select(x => x.idPersona).Distinct().ToList();
        var tblJornada = getTblJornada(idUsuario, fechaDesde, fechaHasta, enableDatosRRHH, idTipoTrabajo, idTurno)
            .Where(x => x.idTurno == idTurno || x.idTurnoNavigation.idTurnoPadre == idTurno)
            .Select(x => x.idPersona).Distinct().ToList();

        var cantidadDias = (fechaHasta - fechaDesde).TotalDays + 1;

        var tblPersona = db.tblPersona
            .Where(pers =>
                (pers.idTipoTrabajo == idTipoTrabajo || idTipoTrabajo == null) &&
                (
                    tblCuadrante.Contains(pers.idPersona) ||
                    tblJornada.Contains(pers.idPersona) ||
                    (pers.idLavanderia == idLavanderia &&
                        (pers.idTurno == idTurno ||
                        pers.idTurnoNavigation.idTurno == idTurno) &&
                        pers.tblCuadrantePersonal.Where(cuad => fechaDesde <= cuad.fecha && fechaHasta >= cuad.fecha).Count() < cantidadDias
                    )
                ) &&
                (
                    enableDatosRRHH ||
                    db.tblTipoTrabajoNUsuario.Any(x => x.idUsuario.Equals(idUsuario) && x.idTipoTrabajo == pers.idTipoTrabajo && x.idLavanderia == pers.idLavanderia)
                ) &&
                (
                    pers.tblPersonaNTipoContrato.Any(contrato => contrato.fechaAltaContrato <= fechaHasta && (contrato.fechaBajaContrato == null || contrato.fechaBajaContrato >= fechaDesde))
                    || pers.tblLlamamiento.Any(l => l.activo == true)
                )
            );

        return tblPersona;
    }

    private IQueryable<tblCuadrantePersonal> getTblCuadrantePersonal(int idUsuario, DateTime fechaDesde, DateTime fechaHasta, bool enableDatosRRHH, int idTurno)
    {

        IQueryable<tblCuadrantePersonal> tblCuadrantePersonal;
        var personas = db.tblCuadrantePersonal
            .Where(cuad => (cuad.idTurno == idTurno || cuad.idTurnoNavigation.idTurnoPadre == idTurno || cuad.idPersonaNavigation.idTurno == idTurno) && fechaDesde <= cuad.fecha && fechaHasta >= cuad.fecha)
            .Select(x => x.idPersona).Distinct().ToList();

        if (enableDatosRRHH)
        {
            tblCuadrantePersonal = db.tblCuadrantePersonal.Where(cuad =>
                fechaDesde <= cuad.fecha && fechaHasta >= cuad.fecha &&
                personas.Contains(cuad.idPersona)
            );
        }
        else
        {
            tblCuadrantePersonal = from cuad in db.tblCuadrantePersonal.Where(cuad =>
                                            fechaDesde <= cuad.fecha && fechaHasta >= cuad.fecha &&
                                            personas.Contains(cuad.idPersona)
                                        )
                                   join perm in db.tblTipoTrabajoNUsuario.Where(x => x.idUsuario.Equals(idUsuario)) on
                                       new { idTipoTrabajo = cuad.idPersonaNavigation.idTipoTrabajo ?? (byte)0, cuad.idLavanderia }
                                       equals
                                       new { perm.idTipoTrabajo, perm.idLavanderia }
                                   select cuad;
        }

        return tblCuadrantePersonal;
    }

    private IQueryable<tblJornada> getTblJornada(int idUsuario, DateTime fechaDesde, DateTime fechaHasta, bool enableDatosRRHH, int? idTipoTrabajo, int idTurno)
    {
        IQueryable<tblJornada> tblJornada;
        var personas = db.tblJornada
            .Where(jorn =>
                (jorn.idTurno == idTurno || jorn.idTurnoNavigation.idTurnoPadre == idTurno || jorn.idPersonaNavigation.idTurno == idTurno) &&
                fechaDesde <= jorn.fecha &&
                fechaHasta >= jorn.fecha &&
                (jorn.idTipoTrabajo == idTipoTrabajo || idTipoTrabajo == null)
            )
            .Select(x => x.idPersona).Distinct().ToList();

        if (enableDatosRRHH)
        {
            tblJornada = db.tblJornada
                .Include(x => x.tblBalanceHorasExtra)
                .Include(x => x.tblBalanceHoras)
                .Include(x => x.tblEventoPersona)
                .Where(jorn =>
                    fechaDesde <= jorn.fecha &&
                    fechaHasta >= jorn.fecha &&
                    personas.Contains(jorn.idPersona)
                );
        }
        else
        {
            tblJornada = (from jorn in db.tblJornada.Where(jorn =>
                                fechaDesde <= jorn.fecha &&
                                fechaHasta >= jorn.fecha &&
                                personas.Contains(jorn.idPersona)
                            )
                          join perm in db.tblTipoTrabajoNUsuario.Where(x => x.idUsuario.Equals(idUsuario)) on
                              new { idTipoTrabajo = jorn.idPersonaNavigation.idTipoTrabajo ?? (byte)0, jorn.idLavanderia }
                              equals
                              new { perm.idTipoTrabajo, perm.idLavanderia }
                          select jorn
                          )
                          .Include(x => x.tblBalanceHoras)
                          .Include(x => x.tblBalanceHorasExtra)
                          .Include(x => x.tblEventoPersona);
        }

        return tblJornada;
    }

    #endregion

    //private void SyncCalendarioPersonal(DateTime fecha, int idPersona, byte? idCalendario_Estado, List<tblCalendarioPersonal> tblCalendarioPersonal)
    //{
    //    var eliminarEstados = (idCalendario_Estado == 3 || idCalendario_Estado == 4 || idCalendario_Estado == 8 || idCalendario_Estado == 10 || idCalendario_Estado == null);
    //    var registroCalendario = tblCalendarioPersonal.FirstOrDefault(x => x.fecha == fecha && x.idPersona == idPersona);
    //    if (registroCalendario != null && eliminarEstados)
    //    {
    //        db.tblCalendarioPersonal.Remove(registroCalendario);
    //    }
    //    if (registroCalendario != null && idCalendario_Estado != null)
    //    {
    //        registroCalendario.idCalendario_Estado = (byte)idCalendario_Estado;
    //    }
    //    else if (registroCalendario == null && !eliminarEstados)
    //    {
    //        db.tblCalendarioPersonal.Add(new tblCalendarioPersonal { fecha = fecha, idPersona = idPersona, idCalendario_Estado = (byte)idCalendario_Estado });
    //    }
    //}


    private static dynamic GetEstadoJornada(List<tblJornada> tblJornada, DateTime fecha, byte idCalendario_Estado, int? idPersona, int? idLlamamiento, int? idJornada, tblCuadrantePersonal? cuadrantePersonal, dynamic? estadoHorario = null)
    {
        bool isDiaTrabajado = idCalendario_Estado == (byte)idsCalendario_Estado.DiaTrabajado || idCalendario_Estado == (byte)idsCalendario_Estado.FestivoTrabajado;

        if (idLlamamiento != null)
        {
            return isDiaTrabajado
                ? new { tipo = "valido", estado = "Jornada finalizada" }
                : new { tipo = "valido", estado = "Día sin trabajo válido" };
        }

        var hoy = DateTime.Now;
        var hayCuadrante = cuadrantePersonal != null;

        var jornadas = tblJornada.Where(j => (idJornada == null && j.fecha.Date == fecha.Date && idPersona != null && j.idPersona == idPersona) || j.idJornada == idJornada);

        if (!jornadas.Any())
        {
            if (!isDiaTrabajado)
            {
                if (fecha.Date < hoy.Date && !hayCuadrante)
                {
                    return new { tipo = "error", estado = "Día sin trabajo no validado" };
                }

                return new { tipo = "valido", estado = "Día sin trabajo válido" };
            }

            if (!hayCuadrante)
            {
                return new
                {
                    tipo = "error",
                    estado = "Jornada sin cuadrante asociado",
                    hideRevision = true,
                };
            }

            if (hayCuadrante && cuadrantePersonal!.horaEntrada != null)
            {
                var dateTimeCuadrantePersonal = fecha.Date.Add((TimeSpan)cuadrantePersonal.horaEntrada);

                if (dateTimeCuadrantePersonal > hoy)
                {
                    return new { tipo = "activo", estado = "Aún no ha empezado el cuadrante establecido" };
                }
            }

            return new { tipo = "error", estado = "No se ha registrado ninguna jornada" };
        }

        List<dynamic> estadoJornadas = new();

        foreach (var jornada in jornadas)
        {
            var hayJornada = jornada != null;

            if (!isDiaTrabajado)
            {
                estadoJornadas.Add(new { tipo = "error", estado = "El estado del cuadrante difiere de la realidad" });

                continue;
            }

            if (jornada!.idCuadrantePersonal == null)
            {
                if (jornada.isRevisado)
                {
                    estadoJornadas.Add(new
                    {
                        tipo = "valido",
                        estado = "Jornada válida sin cuadrante asociado",
                        hideRevision = true
                    });

                    continue;
                }

                estadoJornadas.Add(new
                {
                    tipo = "error",
                    estado = "Jornada sin cuadrante asociado",
                    hideRevision = true
                });

                continue;
            }

            if (jornada!.horaIni == null && jornada.horaFin == null && jornada.tiempoDescanso == null)
            {
                if (hayCuadrante && cuadrantePersonal!.horaEntrada != null)
                {
                    var dateTimeCuadrantePersonal = fecha.Date.Add((TimeSpan)cuadrantePersonal.horaEntrada);

                    if (dateTimeCuadrantePersonal > hoy)
                    {
                        estadoJornadas.Add(new { tipo = "activo", estado = "Aún no ha empezado el cuadrante establecido" });

                        continue;
                    }
                }

                estadoJornadas.Add(new { tipo = "error", estado = "No se ha registrado ninguna jornada" });

                continue;
            }

            if (jornada!.horaIni != null || jornada.horaFin != null || jornada.tiempoDescanso != null)
            {
                estadoHorario ??= CuadrantePersonalController.GetEstadoHorarioJornada(jornada, cuadrantePersonal);

                if (estadoHorario.horaIni == "valido" && estadoHorario.horaFin == "valido")
                {
                    if (estadoHorario.tiempoDescanso == "valido")
                    {
                        estadoJornadas.Add(new { tipo = "valido", estado = "Jornada finalizada" });

                        continue;
                    }

                    if (estadoHorario.tiempoDescanso == "descansando")
                    {
                        estadoJornadas.Add(new { tipo = "valido", estado = "Jornada finalizada con incumplimiento horario" });

                        continue;
                    }
                }

                if (estadoHorario.horaIni != "error" && estadoHorario.horaFin == "error" && fecha.Date == hoy.Date)
                {
                    estadoJornadas.Add(new
                    {
                        tipo = "activo",
                        estado = estadoHorario.tiempoDescanso == "descansando" ? "Descansando" : "Trabajando"
                    });
                }

                if (estadoHorario.horaIni == "error" || estadoHorario.horaFin == "error" || estadoHorario.tiempoDescanso == "error")
                {
                    estadoJornadas.Add(new { tipo = "error", estado = "Datos de la jornada incompletos" });

                    continue;
                }

                estadoJornadas.Add(new { tipo = "warning", estado = "La jornada ha infringido el horario del cuadrante" });

                continue;
            }

            if (jornada.idMotivoIncumplimiento_horaIni == null || jornada.idMotivoIncumplimiento_horaFin == null || jornada.idMotivoIncumplimiento_tiempoDescanso == null)
            {
                estadoJornadas.Add(new { tipo = "error", estado = "Datos de la jornada incompletos" });

                continue;
            }

            estadoJornadas.Add(new { tipo = "valido", estado = "Jornada finalizada" });
        }

        return estadoJornadas.FirstOrDefault(ej => ej.estado == "error") ?? estadoJornadas.FirstOrDefault() ?? new { tipo = "valido", estado = "Día sin trabajo válido" };
    }

    private static dynamic GetEstadoHorarioJornada(tblJornada jornada, tblCuadrantePersonal? cuadrantePersonal)
    {
        if (cuadrantePersonal == null)
        {
            return new { horaIni = "error", horaFin = "error", tiempoDescanso = "error" };
        }

        var horaEntrada = cuadrantePersonal!.horaEntrada;
        var horaSalida = cuadrantePersonal.horaSalida;
        var descanso = cuadrantePersonal?.idTurnoNavigation?.descanso;

        if (horaEntrada == null || horaSalida == null || descanso == null)
        {
            return new { horaIni = "valido", horaFin = "valido", tiempoDescanso = "valido" };
        }

        TimeSpan margen = new(0, 30, 0);
        TimeSpan maxHoras = new(10, 0, 0);

        return new { horaIni = GetEstado_horaIni(), horaFin = GetEstado_horaFin(), tiempoDescanso = GetEstado_tiempoDescanso() };

        string GetEstado_horaIni()
        {
            if (jornada.idMotivoIncumplimiento_horaIni != null)
            {
                return "valido";
            }

            if (jornada.horaIni == null)
            {
                return "error";
            }

            var balanceHorasInicio =
                jornada.tblBalanceHorasExtra.FirstOrDefault(bhe => bhe.isInicio && bhe.minutos != 0)?.minutos
                ?? jornada.tblBalanceHoras.FirstOrDefault(bh => bh.isInicio && bh.minutos != 0)?.minutos
                ?? 0;

            horaEntrada += new TimeSpan(0, balanceHorasInicio, 0);
            var diffEntrada = (TimeSpan)horaEntrada - (TimeSpan)jornada.horaIni;

            var condicionMinimoEntrada =
                diffEntrada.TotalMilliseconds < margen.TotalMilliseconds
                && diffEntrada.TotalMilliseconds > -maxHoras.TotalMilliseconds;
            var condicionLimiteEntrada =
                diffEntrada.TotalMilliseconds >= 0
                || diffEntrada.TotalMilliseconds <= -maxHoras.TotalMilliseconds;

            if (condicionMinimoEntrada && condicionLimiteEntrada)
            {
                return "valido";
            }

            if (condicionMinimoEntrada)
            {
                return "tiempoIncumplido";
            }

            if (condicionLimiteEntrada)
            {
                return "tiempoExcedido";
            }

            return "error";
        }

        string GetEstado_horaFin()
        {
            if (jornada.idMotivoIncumplimiento_horaFin != null)
            {
                return "valido";
            }

            if (jornada.horaFin == null)
            {
                return "error";
            }

            var balanceHorasFin =
                jornada.tblBalanceHorasExtra.FirstOrDefault(bhe => !bhe.isInicio && bhe.minutos != 0)?.minutos
                ?? jornada.tblBalanceHoras.FirstOrDefault(bh => !bh.isInicio && bh.minutos != 0)?.minutos
                ?? 0;

            horaSalida += new TimeSpan(0, balanceHorasFin, 0);
            var diffSalida = (TimeSpan)horaSalida - (TimeSpan)jornada.horaFin;

            var condicionMinimoSalida =
                diffSalida.TotalMilliseconds > -margen.TotalMilliseconds
                && diffSalida.TotalMilliseconds < maxHoras.TotalMilliseconds;
            var condicionLimiteSalida =
                diffSalida.TotalMilliseconds <= 0
                || diffSalida.TotalMilliseconds >= maxHoras.TotalMilliseconds;

            if (condicionMinimoSalida && condicionLimiteSalida)
            {
                return "valido";
            }
            else if (condicionMinimoSalida)
            {
                return "tiempoIncumplido";
            }
            else if (condicionLimiteSalida)
            {
                return "tiempoExcedido";
            }

            return "error";
        }

        string GetEstado_tiempoDescanso()
        {
            if (jornada.idMotivoIncumplimiento_tiempoDescanso != null)
            {
                return "valido";
            }

            var ultimoEvento = jornada.tblEventoPersona
                .OrderByDescending(e => e.fecha)
                .FirstOrDefault();

            var diffDescanso = (TimeSpan)descanso - (jornada.tiempoDescanso ?? new TimeSpan(0));

            if (ultimoEvento?.idEventoPersona_Estado == (byte)idsEventoPersona_Estado.InicioDescanso && descanso == diffDescanso)
            {
                return "descansando";
            }

            var condicionMinimoDescanso =
                diffDescanso.TotalMilliseconds > -margen.TotalMilliseconds
                && diffDescanso.TotalMilliseconds < maxHoras.TotalMilliseconds;
            var condicionLimiteDescanso =
                diffDescanso.TotalMilliseconds <= 0
                || diffDescanso.TotalMilliseconds >= maxHoras.TotalMilliseconds;

            if (condicionMinimoDescanso && condicionLimiteDescanso)
            {
                return "valido";
            }
            else if (condicionMinimoDescanso)
            {
                return "tiempoIncumplido";
            }
            else if (condicionLimiteDescanso)
            {
                return "tiempoExcedido";
            }

            return "error";
        }
    }

    public class PersonaCuadrante
    {
        public int? idPersona { get; set; }
        public string? nombreCompleto { get; set; }
        public string? categoriaInterna { get; set; }
        public int? idLavanderia { get; set; }
        public int? idTurno { get; set; }
        public int? idTipoTrabajo { get; set; }
        public bool? activo { get; set; }

    }

    //public class PostCuadrante
    //{
    //    public int? idCuadrantePersonal { get; set; }
    //    public int? idPersona { get; set; }
    //    public DateTime? fecha { get; set; }
    //    public byte? idCalendario_Estado { get; set; }
    //    public TimeSpan? horaEntrada { get; set; }
    //    public TimeSpan? horaSalida { get; set; }
    //    public int? idTurno { get; set; }
    //    public int? idPosicionNAreaLavanderiaNLavanderia { get; set; }
    //    public int? idLavanderia { get; set; }
    //}

    //public class EstadoNominaNFecha
    //{
    //    public int idPersona { get; set; }
    //    public DateTime fecha { get; set; }
    //    public bool isBloqueado { get; set; }
    //}
}