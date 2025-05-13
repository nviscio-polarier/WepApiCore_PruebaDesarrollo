using Dapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Net;
using WebApiCore.Class;
using WebApiCore.Class.Proyectos.MyPolarier.RRHH;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.RRHH;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;
using WebApiCore.Security;
using static WebApiCore.Controllers.Proyectos.MyPolarier.RRHH.GestionNominasController;

namespace WebApiCore.Controllers;

public class SolicitudFiniquitoController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> hubContext;

    public enum idsTipoSolicitud : short
    {
        Interna = 1,
        Gestoria = 2
    }

    private readonly tblNominaController nc;
    private readonly GestionNominasController gnc;
    private readonly AsientosNominasController anc;

    private readonly NotificacionesRRHH_GestoriaService notificacionesFiniquitoService;

    public SolicitudFiniquitoController(bdERP context, IHubContext<NotificacionesHub> _hubContext)
    {
        db = context;
        hubContext = _hubContext;

        notificacionesFiniquitoService = new(db, false);

        nc = new(context, _hubContext);
        gnc = new(context, _hubContext);
        anc = new(context, _hubContext);
    }

    [EnableQuery]
    [HttpGet("/odata/MyPolarier/RRHH/SolicitudFiniquito/GenerarFiniquitos")]
    [Authorize]
    public async Task<ActionResult> GenerarFiniquitos([FromODataUri] DateTime fechaBaja, [FromODataUri] string idsPersona)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var connection = db.Database.GetDbConnection();

        var fechaDesde = new DateTime(fechaBaja.Year, fechaBaja.Month, 1);
        var fechaHasta = new DateTime(fechaBaja.Year, fechaBaja.Month, DateTime.DaysInMonth(fechaBaja.Year, fechaBaja.Month));

        var results =
            (await connection.QueryAsync<GestionNomina_persona>("EXEC [MyReporting].[EF_gestionNominas_personas] @idUsuario, @fechaDesde, @fechaHasta, 1, @idsPersona, 1, @fechaBaja",
            new { idUsuario, fechaDesde, fechaHasta, idsPersona, fechaBaja })).ToList();

        var resultsVac =
            (await connection.QueryAsync<dynamic>("EXEC [RRHH].[EF_gestionFiniquitos_vacacionesPendientes] @idUsuario, @fechaHasta, @idsPersona",
            new { idUsuario, fechaHasta = fechaBaja, idsPersona })).ToList();

        // FILTRAR PAGAS POR CONTRATO ACTIVO
        // Implementado por duplicación de finiquitos en caso de tener varios contratos en el mes de baja
        var intIdsPersona = idsPersona.Split('|').Select(x => int.Parse(x)).ToList();
        var tblPersonaNTipoContrato = db.tblPersonaNTipoContrato
            .Where(x => intIdsPersona.Contains(x.idPersona) && x.fechaBajaContrato == null)
            .OrderByDescending(x => x.fechaAltaContrato)
            .ToList();

        results = results.Where(x => tblPersonaNTipoContrato.FirstOrDefault(y => y.idPersona == x.idPersona).fechaAltaContrato <= x.fechaInicioNomina).ToList();

        return Ok(results.Select(x => new SolicitudFiniquito
        {
            idPersona = x.idPersona,
            idNomina = x.idNomina,
            nombreCompleto = x.nombre + " " + x.apellidos,
            vacacionesPendientes = resultsVac.FirstOrDefault(y => y.idPersona == x.idPersona)?.vacacionesPendientes ?? 9999,
            impHorasExtra = x.impHorasExtra ?? 0,
            plusNocturnidad = x.plusNocturnidad ?? 0,
            plusActividad = x.plusActividad,
            plusFestivoTrab = x.plusFestivoTrab ?? 0,
            plusProductividad = x.plusProductividad,
            anticipos = x.anticipos ?? 0,
            numAbsentismo = x.numAbsentismo,
            numAbsentismosMesAnterior = x.numAbsentismosMesAnterior
        }));
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        var tblNomina =
            from finiquito in db.tblNomina.Where(x => x.idTipoNomina == (short)idsTipoNomina.PagaFiniquito)
            join nomina in db.tblNomina.Where(x => x.idTipoNomina == (short)idsTipoNomina.PagaMesual)
            on new { finiquito.idPersona, finiquito.fechaBaja } equals new { nomina.idPersona, nomina.fechaBaja } into nf
            from nomina in nf.DefaultIfEmpty()
            where
                finiquito.fechaBaja != null
            select new ModeloFiniquito
            {
                idFiniquito = finiquito.idNomina,
                idNomina = nomina != null ? nomina.idNomina : null,
                idPersona = finiquito.idPersona,
                nombreCompleto = finiquito.nombreCompleto,
                codigoGestoria = finiquito.idPersonaNavigation.codigoGestoria,
                fechaDesde = finiquito.fechaDesde,
                fechaHasta = finiquito.fechaHasta,
                fechaBaja = finiquito.fechaBaja,
                idMotivoBaja = finiquito.idMotivoBaja,
                detalles = finiquito.detalles,
                IBAN = finiquito.IBAN.IsNullOrEmpty() ? null : finiquito.IBAN,
                idEstadoNomina = nomina != null && nomina.idEstadoNomina == (byte)idsEstadoNomina.ImportadoConConflictos
                    ? (byte)idsEstadoNomina.ImportadoConConflictos
                    : nomina != null && nomina.idEstadoNomina == (byte)idsEstadoNomina.ConflictosEnPagoValidado
                        ? (byte)idsEstadoNomina.ConflictosEnPagoValidado
                        : nomina != null && nomina.idEstadoNomina == (byte)idsEstadoNomina.ValidadoGestoria
                            ? (byte)idsEstadoNomina.ValidadoGestoria
                            : finiquito.idEstadoNomina,
                tblDocumentoNNomina_count = finiquito.tblDocumentoNNomina.Count,
                tblEstadoNominaNNomina = finiquito.tblEstadoNominaNNomina,
                vacacionesPendientes = finiquito.tblConceptoNominaNNomina.Any(x => x.idConceptoNomina == (short)idsConceptoNomina.ProporcionalVacacionesFiniquito) ?
                    finiquito.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.ProporcionalVacacionesFiniquito).cantidad ?? 0 : 0,
                absentismos = finiquito.tblConceptoNominaNNomina.Any(x => x.idConceptoNomina == (short)idsConceptoNomina.DtoAbsentismo) ?
                    finiquito.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.DtoAbsentismo).cantidad ?? 0 : 0,
                impHorasExtra = nomina.tblConceptoNominaNNomina.Any(x => x.idConceptoNomina == (short)idsConceptoNomina.HorasExtrasResto) ?
                    nomina.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.HorasExtrasResto).importe ?? 0 : 0,
                anticipos = nomina.tblConceptoNominaNNomina.Any(x => x.idConceptoNomina == (short)idsConceptoNomina.Anticipo) ?
                    nomina.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.Anticipo).importe ?? 0 : 0,
                plusActividad = nomina.tblConceptoNominaNNomina.Any(x => x.idConceptoNomina == (short)idsConceptoNomina.PlusActividad) ?
                    nomina.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.PlusActividad).importe ?? 0 : 0,
                plusFestivoTrab = nomina.tblConceptoNominaNNomina.Any(x => x.idConceptoNomina == (short)idsConceptoNomina.FestivosTrabajados) ?
                    nomina.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.FestivosTrabajados).importe ?? 0 : 0,
                plusNocturnidad = nomina.tblConceptoNominaNNomina.Any(x => x.idConceptoNomina == (short)idsConceptoNomina.PlusNocturnidad) ?
                    nomina.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.PlusNocturnidad).importe ?? 0 : 0,
                plusProductividad = nomina.tblConceptoNominaNNomina.Any(x => x.idConceptoNomina == (short)idsConceptoNomina.PlusProductividad) ?
                    nomina.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.PlusProductividad).importe ?? 0 : 0
            };

        return Ok(tblNomina);
    }

    [HttpPost("odata/MyPolarier/RRHH/SolicitudFiniquito/GetLiquidosPercibir")]
    [Authorize]
    public ActionResult GetLiquidosPercibir([FromBody] List<int> idsFiniquito)
    {
        var finiquitos = db.tblNomina.Where(n => idsFiniquito.Contains(n.idNomina)).ToList();

        var idsPersona = finiquitos.Select(f => f.idPersona).ToList();

        var tblNomina = db.tblNomina.Where(n => idsPersona.Contains(n.idPersona)).ToList();

        var tblHistoricoAsientoNomina = db.tblHistoricoAsientoNomina
            .Where(han => idsPersona.Contains(han.idNominaNavigation.idPersona))
            .Include(han => han.idNominaNavigation)
            .ToList();

        var liquidosPercibir = tblNomina
            .Where(n => finiquitos.Any(f => f.idPersona == n.idPersona && f.fechaBaja == n.fechaBaja))
            .GroupBy(n =>
            {
                string tipoPaga = "";

                if (n.idTipoNomina == (short)idsTipoNomina.PagaAtrasos)
                {
                    tipoPaga = "Paga Atrasos";
                }
                else if (n.fechaBaja != null)
                {
                    tipoPaga = "Paga Finiquito";
                }
                else
                {
                    tipoPaga = "Paga Mensual";
                }

                return new { n.idPersona, n.fechaBaja, tipoPaga };
            })
            .Select(g => new
            {
                g.Key.idPersona,
                g.Key.fechaBaja,
                g.Key.tipoPaga,
                liquidoPercibir = g.Sum(n => n.liquidoPercibir ?? 0)
            })
            .ToList();

        var anterioresLiquidosPercibir = tblHistoricoAsientoNomina
            .Where(han => finiquitos.Any(f => f.idPersona == han.idNominaNavigation.idPersona && f.fechaBaja == han.idNominaNavigation.fechaBaja))
            .GroupBy(han => han.idNomina)
            .Select(g => g.OrderByDescending(han => han.fecha).FirstOrDefault())
            .Where(han => han != null)
            .GroupBy(han => new { han!.idNominaNavigation.idPersona, han!.idNominaNavigation.fechaBaja })
            .Select(g => new
            {
                g.Key.idPersona,
                g.Key.fechaBaja,
                liquidoPercibir = g.Sum(han => han!.liquidoPercibir)
            })
            .ToList();

        var result = (
           from lp in liquidosPercibir
           join alp in anterioresLiquidosPercibir
           on new { lp.idPersona, lp.fechaBaja } equals new { alp.idPersona, alp.fechaBaja } into alpGroup
           from alp in alpGroup.DefaultIfEmpty()
           select new
           {
               lp.idPersona,
               lp.fechaBaja,
               lp.tipoPaga,
               lp.liquidoPercibir,
               anteriorLiquidoPercibir = alp != null ? alp.liquidoPercibir : 0
           }
        );

        return Ok(result);
    }

    [EnableQuery]
    [HttpPost("odata/MyPolarier/RRHH/SolicitudFiniquito/PostMasivo")]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] List<SolicitudFiniquito> tblSolFiniquito)
    {
        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            var tblNomina = db.tblNomina
                .Include(x => x.tblConceptoNominaNNomina)
                .ThenInclude(x => x.idConceptoNominaNavigation)
                .Where(x => tblSolFiniquito.Select(x => x.idNomina).Contains(x.idNomina))
                .ToList();
            var tblPersona = db.tblPersona
                .Include(p => p.tblDatosSalariales)
                .Include(p => p.idEmpresaPolarierNavigation)
                .Include(p => p.idLavanderiaNavigation)
                .Include(p => p.idCategoriaInternaNavigation.idCategoriaConvenioNavigation)
                .Include(p => p.idCentroTrabajoNavigation)
                .Include(p => p.idTipoTrabajoNavigation)
                .Include(p => p.tblPersonaNTipoContrato)
                .Where(p => tblSolFiniquito.Select(x => x.idPersona).Contains(p.idPersona))
                .ToList();

            List<tblNomina> i_nominas = new();
            List<tblNomina> d_nominas = new();
            List<tblNomina> nominasEnviar = new();
            List<tblNomina> nominasGuardar = new();
            foreach (var solFiniquito in tblSolFiniquito)
            {
                var fechaDesde = new DateTime(solFiniquito.fechaBaja.Value.Year, solFiniquito.fechaBaja.Value.Month, 1);
                var fechaHasta = new DateTime(fechaDesde.Year, fechaDesde.Month, DateTime.DaysInMonth(fechaDesde.Year, fechaDesde.Month));

                var objPersona = tblPersona.FirstOrDefault(x => x.idPersona == solFiniquito.idPersona);
                var objContrato = objPersona.tblPersonaNTipoContrato.OrderByDescending(x => x.fechaAltaContrato).FirstOrDefault(x => x.fechaBajaContrato == null);
                if (objContrato != null && objContrato.fechaAltaContrato > fechaDesde) fechaDesde = objContrato.fechaAltaContrato;


                var nomina = tblNomina.FirstOrDefault(x => x.idNomina == solFiniquito.idNomina);
                if (nomina == null)
                {
                    nomina = nc.GetNewNomina(
                        tblPersona.FirstOrDefault(x => x.idPersona == solFiniquito.idPersona),
                        fechaDesde,
                        solFiniquito.fechaBaja.Value,
                        idUsuario
                    );
                }
                else
                {
                    var oldNomina = nomina;
                    nomina = nc.GetNominaBasedOnNomina(nomina.idNomina, idUsuario, nomina.fechaDesde, nomina.fechaHasta, idEstadoNomina: (byte)idsEstadoNomina.EnProceso);
                    if (nomina == null) throw new Exception("Error al copiar la nómina.");

                    foreach (var item in oldNomina.tblConceptoNominaNNomina.Where(x => x.idConceptoNominaNavigation.isConceptoVariable))
                    {
                        nomina.tblConceptoNominaNNomina.Add(new tblConceptoNominaNNomina
                        {
                            idConceptoNomina = item.idConceptoNomina,
                            fecha = item.fecha,
                            cantidad = item.cantidad,
                            precioUnitario = item.precioUnitario,
                            importe = item.importe,
                            fecha_validacion = item.fecha_validacion,
                            idUsuario_validacion = item.idUsuario_validacion,
                            observaciones = item.observaciones
                        });
                    };

                    d_nominas.Add(oldNomina);
                }


                var finiquito =
                     nc.GetNewNomina(
                        tblPersona.FirstOrDefault(x => x.idPersona == solFiniquito.idPersona),
                        fechaDesde,
                        solFiniquito.fechaBaja.Value,
                        idUsuario,
                        (short)idsTipoNomina.PagaFiniquito,
                        solFiniquito.idTipoSolicitud == idsTipoSolicitud.Gestoria ? (byte)idsEstadoNomina.SolicitudFiniquitoCreada : (byte)idsEstadoNomina.SolicitudFiniquitoInterna
                    );

                if (nomina == null || finiquito == null) continue;
                if (solFiniquito.idTipoSolicitud == idsTipoSolicitud.Gestoria) nominasEnviar.Add(nomina);
                else if (solFiniquito.idTipoSolicitud == idsTipoSolicitud.Interna) nominasGuardar.Add(nomina);

                UpdateConceptoNomina(nomina, (short)idsConceptoNomina.PlusActividad, solFiniquito.plusActividad, idUsuario);
                UpdateConceptoNomina(nomina, (short)idsConceptoNomina.Anticipo, solFiniquito.anticipos, idUsuario);
                UpdateConceptoNomina(finiquito, (short)idsConceptoNomina.ProporcionalVacacionesFiniquito, solFiniquito.vacacionesPendientes, idUsuario);
                UpdateConceptoNomina(finiquito, (short)idsConceptoNomina.DtoAbsentismo, solFiniquito.numAbsentismo + solFiniquito.numAbsentismosMesAnterior, idUsuario);

                i_nominas.Add(finiquito);
                if (nomina.idNomina == 0) i_nominas.Add(nomina);
                else
                {
                    nomina.tblEstadoNominaNNomina.Add(new()
                    {
                        idEstadoNomina = solFiniquito.idTipoSolicitud == idsTipoSolicitud.Gestoria ? (byte)idsEstadoNomina.SolicitudFiniquitoCreada : (byte)idsEstadoNomina.SolicitudFiniquitoInterna,
                        fecha = DateTimeOffset.UtcNow,
                        idUsuario_valida = idUsuario,
                        observaciones = "La nómina ha sido convertida en finiquito."
                    });
                }
                nomina.fechaHasta = solFiniquito.fechaBaja.Value;
                nomina.fechaBaja = solFiniquito.fechaBaja;

                // Datos solicitud finiquito
                finiquito.idTipoNomina = (short)idsTipoNomina.PagaFiniquito;
                finiquito.idEstadoNomina = solFiniquito.idTipoSolicitud == idsTipoSolicitud.Gestoria ? (byte)idsEstadoNomina.SolicitudFiniquitoCreada : (byte)idsEstadoNomina.SolicitudFiniquitoInterna;
                finiquito.fechaBaja = solFiniquito.fechaBaja;
                finiquito.idMotivoBaja = solFiniquito.idMotivoBaja;
                finiquito.detalles = solFiniquito.detalles;
                finiquito.tblDocumentoNNomina = solFiniquito.tblDocumentoNNomina;

                if (objContrato != null)
                {
                    objContrato.fechaBajaContrato = solFiniquito.fechaBaja;
                    objContrato.idMotivoBaja = solFiniquito.idMotivoBaja;
                    tblPersonaController.ActualizarEstadoPersona(objPersona);
                }
            }

            db.tblNomina.AddRange(i_nominas);
            nc.DeleteNominas(d_nominas);
            await db.SaveChangesAsync();

            gnc.idUsuario = idUsuario;
            foreach (var nomina in nominasEnviar)
            {
                var fechaDesde = new DateTime(nomina.fechaBaja.Value.Year, nomina.fechaBaja.Value.Month, 1);
                var fechaHasta = new DateTime(fechaDesde.Year, fechaDesde.Month, DateTime.DaysInMonth(fechaDesde.Year, fechaDesde.Month));

                ActionResult EnviarNominaGestoriaResult;

                try
                {
                    EnviarNominaGestoriaResult = await gnc.EnviarNominaGestoria(fechaDesde, fechaHasta, nomina.idNomina, false, false, transaction.GetDbTransaction());
                }
                catch
                {
                    EnviarNominaGestoriaResult = new BadRequestResult();
                }

                if (EnviarNominaGestoriaResult is not OkObjectResult)
                {
                    return StatusCode((int)HttpStatusCode.BadGateway, "errorAPIEnviarFiniquito");
                }
            }

            var fechaDesdeSQL = new DateTime(tblSolFiniquito.Min(x => x.fechaBaja).Value.Year, tblSolFiniquito.Min(x => x.fechaBaja).Value.Month, 1);
            var fechaHastaSQL = new DateTime(fechaDesdeSQL.Year, fechaDesdeSQL.Month, DateTime.DaysInMonth(fechaDesdeSQL.Year, fechaDesdeSQL.Month));

            var connection = db.Database.GetDbConnection();
            var result = (
                await connection.QueryAsync<GestionNomina_persona>(
                    "EXEC [MyReporting].[EF_gestionNominas_personas] @idUsuario, @fechaDesdeSQL, @fechaHastaSQL, 1, @idsPersona, 0",
                    new { idUsuario, fechaDesdeSQL, fechaHastaSQL, idsPersona = string.Join('|', nominasGuardar.Select(x => x.idPersona)) },
                    transaction.GetDbTransaction()
                )
            )
            .Where(x => x.idTipoNomina == (short)idsTipoNomina.PagaMesual);

            foreach (var item in result)
            {
                List<tblConceptoNominaNNomina> lista = new();
                gnc.GuardarConceptosNDB(item, ref lista);
            }

            await db.SaveChangesAsync();

            await hubContext.Clients.Group("GestionFiniquitos").SendAsync("GestionFiniquitos/refresh", idUsuario);

            if (nominasEnviar.Count > 0)
            {
                await notificacionesFiniquitoService.SendAvisos_NuevosFiniquitos(nominasEnviar.Count);
            }

            await transaction.CommitAsync();

            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [EnableQuery]
    [HttpPatch("odata/MyPolarier/RRHH/SolicitudFiniquito(idFiniquito={idFiniquito},idNomina={idNomina})")]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int idFiniquito, [FromODataUri] int? idNomina, [FromBody] JsonPatchDocument<tblNomina> nomina)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var entity = await db.tblNomina.FirstOrDefaultAsync(n => n.idNomina == idFiniquito);

        using var transaction = await db.Database.BeginTransactionAsync();

        if (entity == null) return NotFound();

        var objUsuario = db.tblUsuario.Include(x => x.idPermiso).FirstOrDefault(x => x.idUsuario == idUsuario);
        if (!(objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.GestionFiniquitosGestoria || x.codigo == idsPermiso.GestionFiniquitosRRHH) || objUsuario.idCargo == (short)idsCargo.Desarrollador))
            return StatusCode((int)HttpStatusCode.Forbidden, "sinPermisosModificar");

        if (idNomina == null)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, "finiquitoSinNomina");
        }

        if (
            (nomina.Operations.Count == 1 && nomina.Operations.Any(op => op.path == "/idEstadoNomina")) ||

            ((entity.idEstadoNomina == (byte)idsEstadoNomina.SolicitudFiniquitoCreada ||
            entity.idEstadoNomina == (byte)idsEstadoNomina.SolicitudFiniquitoInterna ||
            entity.idEstadoNomina == (byte)idsEstadoNomina.SolicitudCambioRRHH) &&
            (objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.GestionFiniquitosRRHH) || objUsuario.idCargo == (short)idsCargo.Desarrollador)) ||

            ((entity.idEstadoNomina == (byte)idsEstadoNomina.EnProceso ||
            entity.idEstadoNomina == (byte)idsEstadoNomina.ValidadoGestoria) &&
            (objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.GestionFiniquitosGestoria) || objUsuario.idCargo == (short)idsCargo.Desarrollador) &&
            nomina.Operations.Count <= 2 && !nomina.Operations.Any(op => op.path != "/tblDocumentoNNomina" && op.path != "/idEstadoNomina"))
        )
        {
            var validandoFiniquito = false;
            var enviarFiniquito = false;
            var enviarMailIncidenciaSolucionada = false;

            var operations_idEstadoNomina = nomina.Operations.FirstOrDefault(op => op.path == "/idEstadoNomina");
            if (operations_idEstadoNomina != null)
            {
                var idEstadoNomina = Convert.ToByte(operations_idEstadoNomina.value);

                if (idEstadoNomina == (byte)idsEstadoNomina.ValidadoGestoria)
                {
                    validandoFiniquito = true;
                }
                else
                {
                    if (idEstadoNomina == (byte)idsEstadoNomina.SolicitudFiniquitoCreada)
                    {
                        enviarFiniquito = true;
                    }
                    else if (idEstadoNomina == (byte)idsEstadoNomina.ValidadoRRHH)
                    {
                        enviarMailIncidenciaSolucionada = true;
                    }

                    entity.tblEstadoNominaNNomina.Add(new()
                    {
                        idEstadoNomina = idEstadoNomina,
                        fecha = DateTimeOffset.UtcNow,
                        idUsuario_valida = idUsuario
                    });
                }
            }

            var operations_fechaBaja = nomina.Operations.FirstOrDefault(op => op.path == "/fechaBaja");
            if (operations_fechaBaja != null)
            {
                var fechaBaja = Convert.ToDateTime(operations_fechaBaja.value);

                entity.fechaHasta = fechaBaja;

                var objNomina = db.tblNomina.FirstOrDefault(x => x.idNomina == idNomina);
                if (objNomina != null)
                {
                    objNomina.fechaHasta = fechaBaja;
                    objNomina.fechaBaja = fechaBaja;
                }

                var objPersona = db.tblPersona.Include(x => x.tblPersonaNTipoContrato).FirstOrDefault(x => x.idPersona == entity.idPersona);
                var objContrato = objPersona.tblPersonaNTipoContrato.FirstOrDefault(x => x.fechaBajaContrato == entity.fechaBaja);
                if (objContrato != null)
                {
                    objContrato.fechaBajaContrato = fechaBaja;
                    tblPersonaController.ActualizarEstadoPersona(objPersona);
                }
            }

            var operations_tblDocumentoNNomina = nomina.Operations.FirstOrDefault(op => op.path == "/tblDocumentoNNomina");
            if (operations_tblDocumentoNNomina != null)
            {
                db.tblDocumentoNNomina.RemoveRange(db.tblDocumentoNNomina.Where(x => x.idNomina == entity.idNomina));
            }

            var operations_plusActividad = nomina.Operations.FirstOrDefault(op => op.path == "/plusActividad");
            if (operations_plusActividad != null)
            {
                decimal plusActividad = Convert.ToDecimal(operations_plusActividad.value);

                var concepto = db.tblConceptoNominaNNomina.FirstOrDefault(x => x.idNomina == idNomina && x.idConceptoNomina == (short)idsConceptoNomina.PlusActividad);
                if (concepto != null)
                {
                    concepto.precioUnitario = plusActividad;
                    concepto.importe = plusActividad;
                }
                else
                {
                    db.tblConceptoNominaNNomina.Add(new()
                    {
                        idNomina = (int)idNomina,
                        idConceptoNomina = (short)idsConceptoNomina.PlusActividad,
                        precioUnitario = plusActividad,
                        cantidad = 1,
                        importe = plusActividad,
                        fecha = entity.fechaDesde,
                        idUsuario_validacion = idUsuario,
                        fecha_validacion = DateTimeOffset.Now
                    });
                }

                nomina.Operations.Remove(operations_plusActividad);
                enviarFiniquito = true;
            }

            var operations_plusProductividad = nomina.Operations.FirstOrDefault(op => op.path == "/plusProductividad");
            if (operations_plusProductividad != null)
            {
                decimal plusProductividad = Convert.ToDecimal(operations_plusProductividad.value);

                var concepto = db.tblConceptoNominaNNomina.FirstOrDefault(x => x.idNomina == idNomina && x.idConceptoNomina == (short)idsConceptoNomina.PlusProductividad);
                if (concepto != null)
                {
                    concepto.precioUnitario = plusProductividad;
                    concepto.importe = plusProductividad;
                }
                else
                {
                    db.tblConceptoNominaNNomina.Add(new()
                    {
                        idNomina = (int)idNomina,
                        idConceptoNomina = (short)idsConceptoNomina.PlusProductividad,
                        precioUnitario = plusProductividad,
                        cantidad = 1,
                        importe = plusProductividad
                    });
                }

                nomina.Operations.Remove(operations_plusProductividad);
                enviarFiniquito = true;
            }

            var operations_anticipos = nomina.Operations.FirstOrDefault(op => op.path == "/anticipos");
            if (operations_anticipos != null)
            {
                decimal anticipos = Convert.ToDecimal(operations_anticipos.value);

                var concepto = db.tblConceptoNominaNNomina.FirstOrDefault(x => x.idNomina == idNomina && x.idConceptoNomina == (short)idsConceptoNomina.Anticipo);
                if (concepto != null)
                {
                    concepto.precioUnitario = anticipos;
                    concepto.importe = anticipos;
                }
                else
                {
                    db.tblConceptoNominaNNomina.Add(new()
                    {
                        idNomina = (int)idNomina,
                        idConceptoNomina = (short)idsConceptoNomina.Anticipo,
                        precioUnitario = anticipos,
                        cantidad = 1,
                        importe = anticipos,
                        fecha = entity.fechaDesde,
                        idUsuario_validacion = idUsuario,
                        fecha_validacion = DateTimeOffset.Now
                    });
                }

                nomina.Operations.Remove(operations_anticipos);
                enviarFiniquito = true;
            }

            nomina.ApplyTo(entity);

            entity.idUsuario_modifica = idUsuario;
            entity.fecha_modifica = DateTimeOffset.UtcNow;

            await db.SaveChangesAsync();

            // Si se está validando la nómina, importamos el finiquito
            if (validandoFiniquito)
            {
                gnc.idUsuario = idUsuario;
                var importarNominaResult = await gnc.ImportarNominaGestoria(entity.fechaDesde, entity.fechaHasta, entity.idPersona, transaction.GetDbTransaction());

                if (importarNominaResult is not OkObjectResult)
                {
                    return StatusCode((int)HttpStatusCode.BadGateway, "errorAPIImportarFiniquito");
                }
            }

            if (validandoFiniquito)
            {
                await notificacionesFiniquitoService.SendAvisos_FiniquitoValidado(entity);
            }
            else if (enviarMailIncidenciaSolucionada)
            {
                await notificacionesFiniquitoService.SendAvisos_FiniquitoIncidenciaSolucionada(entity);
            }

            if (operations_fechaBaja != null)
            {
                entity = await db.tblNomina.Include(x => x.tblConceptoNominaNNomina).FirstOrDefaultAsync(n => n.idNomina == idFiniquito);
                var connection = db.Database.GetDbConnection();
                var resultsVac = (
                    await connection.QueryAsync<dynamic>(
                        "EXEC [RRHH].[EF_gestionFiniquitos_vacacionesPendientes] @idUsuario, @fechaHasta, @idsPersona",
                        new { idUsuario, fechaHasta = entity.fechaBaja, idsPersona = entity.idPersona.ToString() },
                        transaction.GetDbTransaction()
                    )
                )
                .FirstOrDefault();

                UpdateConceptoNomina(entity, (short)idsConceptoNomina.ProporcionalVacacionesFiniquito, resultsVac?.vacacionesPendientes > 0 ? resultsVac?.vacacionesPendientes : 0, idUsuario);

                var year = entity.fechaDesde.Year;
                var month = entity.fechaDesde.Month;

                var gestionNomina_persona = (
                    await connection.QueryAsync<GestionNomina_persona>(
                        "EXEC [MyReporting].[EF_gestionNominas_personas] @idUsuario, @fechaDesde, @fechaHasta, 1, @idsPersona, 0",
                        new { idUsuario, fechaDesde = new DateTime(year, month, 1), fechaHasta = new DateTime(year, month, DateTime.DaysInMonth(year, month)), idsPersona = entity.idPersona },
                        transaction.GetDbTransaction()
                    )
                )
                .FirstOrDefault(x => x.idTipoNomina == (short)idsTipoNomina.PagaMesual);
                UpdateConceptoNomina(entity, (short)idsConceptoNomina.DtoAbsentismo, gestionNomina_persona.numAbsentismo + gestionNomina_persona.numAbsentismosMesAnterior, idUsuario);
            }

            await db.SaveChangesAsync();

            if (enviarFiniquito && entity.idEstadoNomina != (byte)idsEstadoNomina.SolicitudFiniquitoInterna)
            {
                gnc.idUsuario = idUsuario;
                var fechaDesde = new DateTime(entity.fechaDesde.Year, entity.fechaDesde.Month, 1);
                var fechaHasta = new DateTime(entity.fechaDesde.Year, entity.fechaDesde.Month, DateTime.DaysInMonth(entity.fechaDesde.Year, entity.fechaDesde.Month));

                ActionResult EnviarNominaGestoriaResult;

                try
                {
                    EnviarNominaGestoriaResult = await gnc.EnviarNominaGestoria(fechaDesde, fechaHasta, (int)idNomina, false, false, transaction.GetDbTransaction());
                }
                catch
                {
                    EnviarNominaGestoriaResult = new BadRequestResult();
                }

                if (EnviarNominaGestoriaResult is not OkObjectResult)
                {
                    return StatusCode((int)HttpStatusCode.BadGateway, "errorAPIEnviarFiniquito");
                }
            }

            await hubContext.Clients.Group("GestionFiniquitos").SendAsync("GestionFiniquitos/refresh", idUsuario);

            await transaction.CommitAsync();

            return Updated(entity);
        }
        else return StatusCode((int)HttpStatusCode.Forbidden, "estadoBloqueadoAPI");
    }

    [EnableQuery]
    [HttpDelete("odata/MyPolarier/RRHH/SolicitudFiniquito(idFiniquito={idFiniquito},idNomina={idNomina})")]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int idFiniquito, [FromODataUri] int? idNomina)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var entity = await db.tblNomina.FirstOrDefaultAsync(n => n.idNomina == idFiniquito);

        if (entity == null) return NotFound();

        var objUsuario = db.tblUsuario.Include(x => x.idPermiso).FirstOrDefault(x => x.idUsuario == idUsuario);
        if (!(objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.GestionFiniquitosGestoria || x.codigo == idsPermiso.GestionFiniquitosRRHH) || objUsuario.idCargo == (short)idsCargo.Desarrollador))
            return StatusCode((int)HttpStatusCode.Forbidden, "sinPermisosModificar");

        if (entity.idEstadoNomina == (byte)idsEstadoNomina.SolicitudFiniquitoCreada || entity.idEstadoNomina == (byte)idsEstadoNomina.SolicitudFiniquitoInterna)
        {
            var objNomina = db.tblNomina.FirstOrDefault(x => x.idNomina == idNomina);
            if (objNomina != null)
            {
                objNomina.fechaHasta = new DateTime(objNomina.fechaDesde.Year, objNomina.fechaDesde.Month, DateTime.DaysInMonth(objNomina.fechaDesde.Year, objNomina.fechaDesde.Month));
                objNomina.fechaBaja = null;
                objNomina.idMotivoBaja = null;
            }

            var objPersona = db.tblPersona.Include(x => x.tblPersonaNTipoContrato).FirstOrDefault(x => x.idPersona == entity.idPersona);
            var objContrato = objPersona.tblPersonaNTipoContrato.FirstOrDefault(pntc => pntc.fechaBajaContrato == entity.fechaBaja);
            if (objContrato != null)
            {
                objContrato.fechaBajaContrato = null;
                objContrato.idMotivoBaja = null;
                tblPersonaController.ActualizarEstadoPersona(objPersona);
            }


            nc.DeleteNominas(new List<tblNomina> { entity });

            await db.SaveChangesAsync();

            await hubContext.Clients.Group("GestionFiniquitos").SendAsync("GestionFiniquitos/refresh", idUsuario);

            return Ok(true);

        }
        else return StatusCode((int)HttpStatusCode.Forbidden, "estadoBloqueadoAPI");
    }

    [EnableQuery]
    [HttpPatch("odata/MyPolarier/RRHH/SolicitudFiniquito/EnviarGestoriaMasivo")]
    [Authorize]
    public async Task<ActionResult> EnviarGestoriaMasivo([FromBody] List<FiniquitoNominaPair> ids)
    {
        using var transaction = await db.Database.BeginTransactionAsync();

        if (ids == null || ids.Any(x => x.idNomina == null)) return BadRequest();

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var tblFiniquito = db.tblNomina
            .Include(x => x.tblConceptoNominaNNomina)
            .Where(x => ids.Select(x => x.idFiniquito).Contains(x.idNomina))
            .ToList();

        if (tblFiniquito.Any(x => x.idEstadoNomina != (byte)idsEstadoNomina.SolicitudFiniquitoInterna))
            return StatusCode((int)HttpStatusCode.Forbidden, "estadoBloqueadoAPI");

        foreach (var finiquito in tblFiniquito)
        {
            finiquito.idEstadoNomina = (byte)idsEstadoNomina.SolicitudFiniquitoCreada;
            finiquito.tblEstadoNominaNNomina.Add(new()
            {
                idEstadoNomina = (byte)idsEstadoNomina.SolicitudFiniquitoCreada,
                fecha = DateTimeOffset.UtcNow,
                idUsuario_valida = idUsuario
            });
        }

        await db.SaveChangesAsync();

        gnc.idUsuario = idUsuario;
        foreach (var finiquito in tblFiniquito)
        {
            var fechaDesde = new DateTime(finiquito.fechaDesde.Year, finiquito.fechaDesde.Month, 1);
            var fechaHasta = new DateTime(finiquito.fechaDesde.Year, finiquito.fechaDesde.Month, DateTime.DaysInMonth(finiquito.fechaDesde.Year, finiquito.fechaDesde.Month));

            var idNomina = ids.First(x => x.idFiniquito == finiquito.idNomina).idNomina;

            ActionResult EnviarNominaGestoriaResult;

            try
            {
                EnviarNominaGestoriaResult = await gnc.EnviarNominaGestoria(fechaDesde, fechaHasta, (int)idNomina, false, false, transaction.GetDbTransaction());
            }
            catch
            {
                EnviarNominaGestoriaResult = new BadRequestResult();
            }

            if (EnviarNominaGestoriaResult is not OkObjectResult)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, "errorAPIEnviarFiniquito");
            }
        }

        await hubContext.Clients.Group("GestionFiniquitos").SendAsync("GestionFiniquitos/refresh", idUsuario);

        await notificacionesFiniquitoService.SendAvisos_NuevosFiniquitos(tblFiniquito.Count);

        await transaction.CommitAsync();

        return Ok(true);
    }

    [EnableQuery]
    [HttpPatch("odata/MyPolarier/RRHH/SolicitudFiniquito/EnviarContabilidadMasivo")]
    [Authorize]
    public async Task<ActionResult> EnviarContabilidadMasivo([FromBody] List<FiniquitoNominaPair> ids)
    {
        if (ids == null) return BadRequest();

        var idsFiniquito = ids.Select(x => x.idFiniquito).ToList();
        var idsNomina = ids.Select(x => x.idNomina).ToList();

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var tblNomina = db.tblNomina
            .Where(x => idsFiniquito.Contains(x.idNomina) || idsNomina.Contains(x.idNomina))
            .ToList();

        var idsPersona = tblNomina.Select(n => n.idPersona).Distinct().ToList();

        var personasFechasBaja = tblNomina.Select(n => new { n.idPersona, n.fechaBaja }).Distinct().ToList();

        var nominasOtrosTipos = db.tblNomina
            .Where(n =>
                idsPersona.Contains(n.idPersona)
                && !idsFiniquito.Contains(n.idNomina)
                && !idsNomina.Contains(n.idNomina)
            )
            .ToList();

        tblNomina.AddRange(
            nominasOtrosTipos.Where(n =>
                personasFechasBaja.Any(pfb => pfb.idPersona == n.idPersona && pfb.fechaBaja == n.fechaBaja)
            )
        );

        if (tblNomina.Any(x => x.idEstadoNomina != (byte)idsEstadoNomina.ValidadoGestoria && x.idEstadoNomina != (byte)idsEstadoNomina.PagoValidado))
            return StatusCode((int)HttpStatusCode.Forbidden, "estadoBloqueadoAPI");

        foreach (var finiquito in tblNomina)
        {
            finiquito.idEstadoNomina = (byte)idsEstadoNomina.PagoValidado;
            finiquito.tblEstadoNominaNNomina.Add(new()
            {
                idEstadoNomina = (byte)idsEstadoNomina.PagoValidado,
                fecha = DateTimeOffset.UtcNow,
                idUsuario_valida = idUsuario
            });
        }

        var idsNominaFinal = tblNomina.Select(n => n.idNomina);

        var ultimoHistoricoAsientoNomina = db.tblHistoricoAsientoNomina
            .Where(han => idsNominaFinal.Contains(han.idNomina))
            .GroupBy(han => han.idNomina)
            .Select(g => g.OrderByDescending(han => han.fecha).FirstOrDefault())
            .ToList();

        anc.GenerarHistoricoAsientoNomina(tblNomina.Select(n => n.idNomina).ToList(), DateTime.Today, idUsuario, null, false);

        await db.SaveChangesAsync();

        await hubContext.Clients.Group("GestionFiniquitos").SendAsync("GestionFiniquitos/refresh", idUsuario);

        await notificacionesFiniquitoService.SendAvisos_FiniquitoValidadoContabilizar(ids, ultimoHistoricoAsientoNomina);

        return Ok(true);
    }

    [EnableQuery]
    [HttpPatch("odata/MyPolarier/RRHH/SolicitudFiniquito/Recalcular")]
    [Authorize]
    public async Task<ActionResult> Recalcular([FromODataUri] int idFiniquito, [FromODataUri] int idNomina)
    {
        using var transaction = await db.Database.BeginTransactionAsync();

        var objNomina = db.tblNomina.Include(x => x.tblConceptoNominaNNomina).FirstOrDefault(x => x.idNomina == idNomina);
        var objFiniquito = db.tblNomina.Include(x => x.tblConceptoNominaNNomina).FirstOrDefault(x => x.idNomina == idFiniquito);
        if (objFiniquito == null || objNomina == null) return NotFound();

        if (objFiniquito.idEstadoNomina != (byte)idsEstadoNomina.SolicitudFiniquitoInterna && objFiniquito.idEstadoNomina != (byte)idsEstadoNomina.SolicitudFiniquitoCreada && objFiniquito.idEstadoNomina != (byte)idsEstadoNomina.SolicitudCambioRRHH)
            return BadRequest("estadoBloqueadoAPI");

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario.Include(x => x.idPermiso).FirstOrDefault(x => x.idUsuario == idUsuario);
        if (!(objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.GestionFiniquitosRRHH) || objUsuario.idCargo == (short)idsCargo.Desarrollador))
            return BadRequest("sinPermisosModificar");

        DateTime fechaDesde = new DateTime(objFiniquito.fechaDesde.Year, objFiniquito.fechaDesde.Month, 1);
        DateTime fechaHasta = new DateTime(objFiniquito.fechaHasta.Year, objFiniquito.fechaHasta.Month, DateTime.DaysInMonth(objFiniquito.fechaHasta.Year, objFiniquito.fechaHasta.Month));

        var connection = db.Database.GetDbConnection();
        var item = (
            await connection.QueryAsync<GestionNomina_persona>(
                "EXEC [MyReporting].[EF_gestionNominas_personas] @idUsuario, @fechaDesde, @fechaHasta, 1, @idsPersona, 0",
                new { idUsuario, fechaDesde, fechaHasta, idsPersona = objFiniquito.idPersona },
                transaction.GetDbTransaction()
            )
        )
        .FirstOrDefault(x => x.idTipoNomina == (short)idsTipoNomina.PagaMesual);

        if (item != null && item.idNomina != null)
        {
            gnc.idUsuario = idUsuario;
            List<tblConceptoNominaNNomina> lista = new();

            gnc.GuardarConceptosNDB(item, ref lista);
        }
        else
        {
            return BadRequest("No se ha encontrado la nómina asociada.");
        }

        var resultsVac = (
            await connection.QueryAsync<dynamic>(
                "EXEC [RRHH].[EF_gestionFiniquitos_vacacionesPendientes] @idUsuario, @fechaHasta, @idsPersona",
                new { idUsuario, fechaDesde, objFiniquito.fechaHasta, idsPersona = objFiniquito.idPersona },
                transaction.GetDbTransaction()
            )
        ).ToList();

        var vacacionesPendientes = resultsVac.FirstOrDefault()?.vacacionesPendientes;

        UpdateConceptoNomina(objFiniquito, (short)idsConceptoNomina.ProporcionalVacacionesFiniquito, vacacionesPendientes < 0 ? 0 : vacacionesPendientes, idUsuario);
        UpdateConceptoNomina(objFiniquito, (short)idsConceptoNomina.DtoAbsentismo, item.numAbsentismo + item.numAbsentismosMesAnterior, idUsuario);

        await db.SaveChangesAsync();

        if (objFiniquito.idEstadoNomina != (byte)idsEstadoNomina.SolicitudFiniquitoInterna)
        {
            gnc.idUsuario = idUsuario;

            ActionResult EnviarNominaGestoriaResult;

            try
            {
                EnviarNominaGestoriaResult = await gnc.EnviarNominaGestoria(fechaDesde, fechaHasta, objNomina.idNomina, false, false, transaction.GetDbTransaction());
            }
            catch
            {
                EnviarNominaGestoriaResult = new BadRequestResult();
            }

            if (EnviarNominaGestoriaResult is not OkObjectResult)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, "errorAPIEnviarNominaGestoria");
            }
        }

        await hubContext.Clients.Group("GestionFiniquitos").SendAsync("GestionFiniquitos/refresh", idUsuario);

        await transaction.CommitAsync();

        return Ok(true);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/RRHH/SolicitudFiniquito/RecalcularVacaciones")]
    [Authorize]
    public async Task<ActionResult> RecalcularVacaciones([FromODataUri] int idPersona, [FromODataUri] DateTime fechaBaja)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var connection = db.Database.GetDbConnection();
        var resultsVac =
            (await connection.QueryAsync<dynamic>("EXEC [RRHH].[EF_gestionFiniquitos_vacacionesPendientes] @idUsuario, @fechaHasta, @idsPersona",
            new { idUsuario, fechaHasta = fechaBaja, idsPersona = idPersona.ToString() })).FirstOrDefault();

        if (resultsVac != null)
        {
            return Ok(resultsVac.vacacionesPendientes);
        }
        else
        {
            return BadRequest("No se ha podido calcular.");
        }
    }

    [EnableQuery]
    [HttpPatch("odata/MyPolarier/RRHH/SolicitudFiniquito/ReimportarFiniquito")]
    [Authorize]
    public async Task<ActionResult> ReimportarFiniquito([FromODataUri] int idFiniquito)
    {
        using var transaction = await db.Database.BeginTransactionAsync();

        var objNomina = db.tblNomina.Include(x => x.tblConceptoNominaNNomina).FirstOrDefault(x => x.idNomina == idFiniquito);
        if (objNomina == null) return NotFound();

        if (objNomina.idEstadoNomina != (byte)idsEstadoNomina.ImportadoConConflictos && objNomina.idEstadoNomina != (byte)idsEstadoNomina.ValidadoGestoria && objNomina.idEstadoNomina != (byte)idsEstadoNomina.PagoValidado)
            return BadRequest("estadoBloqueadoAPI");

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario.Include(x => x.idPermiso).FirstOrDefault(x => x.idUsuario == idUsuario);
        if (!(objUsuario.idPermiso.Any(x => x.codigo == idsPermiso.GestionFiniquitosRRHH || x.codigo == idsPermiso.GestionFiniquitosGestoria) || objUsuario.idCargo == (short)idsCargo.Desarrollador))
            return BadRequest("sinPermisosModificar");

        gnc.idUsuario = idUsuario;
        var importarNominaResult = await gnc.ImportarNominaGestoria(objNomina.fechaDesde, objNomina.fechaHasta, objNomina.idPersona, transaction.GetDbTransaction());

        if (importarNominaResult is not OkObjectResult)
        {
            return StatusCode((int)HttpStatusCode.BadGateway, "errorAPIImportarFiniquito");
        }

        var objEstado = db.tblEstadoNominaNNomina.Where(x => x.idNomina == idFiniquito).OrderByDescending(x => x.fecha).First();
        objEstado.observaciones ??= "Finiquito reimportado.";

        await db.SaveChangesAsync();

        await hubContext.Clients.Group("GestionFiniquitos").SendAsync("GestionFiniquitos/refresh", idUsuario);

        await transaction.CommitAsync();

        return Ok(true);
    }

    [EnableQuery]
    [HttpPatch("odata/MyPolarier/RRHH/SolicitudFiniquito/NotificarIncidencia")]
    [Authorize]
    public async Task<ActionResult> NotificarIncidencia([FromODataUri] int idNomina)
    {

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var objNomina = db.tblNomina
            .Include(x => x.tblEstadoNominaNNomina)
            .FirstOrDefault(x => x.idNomina == idNomina);
        if (objNomina == null) return NotFound();

        //var incidencia= objNomina.tblEstadoNominaNNomina.OrderByDescending(x => x.fecha).FirstOrDefault(x => x.idEstadoNomina == (byte)idsEstadoNomina.SolicitudCambioRRHH);

        await notificacionesFiniquitoService.SendAvisos_FiniquitoIncidenciaCreada(objNomina);

        await hubContext.Clients.Group("GestionFiniquitos").SendAsync("GestionFiniquitos/refresh", idUsuario);

        return Ok();
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/RRHH/SolicitudFiniquito/GenerarXMLBanco")]
    [Authorize]
    public async Task<ActionResult> GenerarXMLBanco([FromBody] List<FiniquitoNominaPair> idsNominas)
    {
        if (Utils.isProduccion())
        {
            return BadRequest("Estas en producción");
        }
        else
        {

            var idsFiniquito = idsNominas.Select(n => n.idFiniquito).ToList();
            var idsNomina = idsNominas.Select(n => n.idNomina).ToList();

            var ultimoHistoricoAsientoNomina = db.tblHistoricoAsientoNomina
                .Where(han => idsFiniquito.Contains(han.idNomina) || idsNomina.Contains(han.idNomina))
                .GroupBy(han => han.idNomina)
                .Select(g => g.OrderByDescending(han => han.fecha).FirstOrDefault())
                .ToList();

            return Ok(await notificacionesFiniquitoService.SendAvisos_FiniquitoValidadoContabilizar(idsNominas, ultimoHistoricoAsientoNomina));
        }
    }

    void UpdateConceptoNomina(tblNomina nomina, short idConceptoNomina, decimal? importe, int idUsuario)
    {
        List<short> idsConceptoNomina_soloCantidad = new()
        {
            (short)idsConceptoNomina.ProporcionalVacacionesFiniquito,
            (short)idsConceptoNomina.DtoAbsentismo
        };

        var concepto = nomina.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina);
        if (concepto == null)
        {
            concepto = new tblConceptoNominaNNomina
            {
                idConceptoNomina = idConceptoNomina,
                observaciones = "Creado por solicitud de finiquito",
                idUsuario_validacion = idUsuario,
                fecha_validacion = DateTimeOffset.UtcNow,
                fecha = nomina.fechaDesde
            };
            nomina.tblConceptoNominaNNomina.Add(concepto);
        }

        var isSoloCantidad = idsConceptoNomina_soloCantidad.Contains(idConceptoNomina);

        concepto.cantidad = isSoloCantidad ? importe : 1;
        concepto.precioUnitario = isSoloCantidad ? 0 : importe;
        concepto.importe = isSoloCantidad ? 0 : importe;
        concepto.idUsuario_validacion = idUsuario;
        concepto.fecha_validacion = DateTimeOffset.UtcNow;
        concepto.fecha = nomina.fechaDesde;
    }

    public class SolicitudFiniquito
    {
        public int idPersona { get; set; }
        public int? idNomina { get; set; }
        public byte idMotivoBaja { get; set; }
        public string nombreCompleto { get; set; }
        public DateTime? fechaBaja { get; set; }
        public string detalles { get; set; }
        public idsTipoSolicitud idTipoSolicitud { get; set; }
        public decimal? vacacionesPendientes { get; set; }
        public decimal? impHorasExtra { get; set; }
        public decimal? plusNocturnidad { get; set; }
        public decimal? plusFestivoTrab { get; set; }
        public decimal? plusProductividad { get; set; }
        public decimal? plusActividad { get; set; }
        public decimal? anticipos { get; set; }
        public decimal? numAbsentismo { get; set; }
        public decimal? numAbsentismosMesAnterior { get; set; }
        public List<tblDocumentoNNomina> tblDocumentoNNomina { get; set; }
    }

    public class ModeloFiniquito
    {
        [Key]
        public int idFiniquito { get; set; }
        public int? idNomina { get; set; }
        public int idPersona { get; set; }
        public string nombreCompleto { get; set; }
        public string codigoGestoria { get; set; }
        public DateTime fechaDesde { get; set; }
        public DateTime fechaHasta { get; set; }
        public DateTime? fechaBaja { get; set; }
        public byte? idMotivoBaja { get; set; }
        public string? detalles { get; set; }
        public string? IBAN { get; set; }
        public byte idEstadoNomina { get; set; }
        public int tblDocumentoNNomina_count { get; set; }
        public ICollection<tblEstadoNominaNNomina> tblEstadoNominaNNomina { get; set; }
        public decimal? vacacionesPendientes { get; set; }
        public decimal? absentismos { get; set; }
        public decimal? impHorasExtra { get; set; }
        public decimal? plusNocturnidad { get; set; }
        public decimal? plusFestivoTrab { get; set; }
        public decimal? plusActividad { get; set; }
        public decimal? plusProductividad { get; set; }
        public decimal? anticipos { get; set; }
    }

    public class FiniquitoNominaPair
    {
        public int idFiniquito { get; set; }
        public int? idNomina { get; set; }
    }

}
