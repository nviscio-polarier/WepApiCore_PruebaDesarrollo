using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Text.RegularExpressions;
using WebApiCore.Class;
using WebApiCore.Class.externos.a3innuva;
using WebApiCore.Class.externos.a3innuva.Context.Absenteeisms;
using WebApiCore.Class.externos.a3innuva.Context.AbsenteeismsUnjustifiedAbsenteeisms;
using WebApiCore.Class.externos.a3innuva.Context.Employees;
using WebApiCore.Class.externos.a3innuva.Context.EmployeesBankaccounts;
using WebApiCore.Class.externos.a3innuva.Context.EmployeesConcepts;
using WebApiCore.Class.externos.a3innuva.Context.EmployeesSalary;
using WebApiCore.Class.externos.a3innuva.Context.Pays;
using WebApiCore.Class.externos.a3innuva.Context.PaysConcepts;
using WebApiCore.Class.externos.a3innuva.Context.TemporaryDisabilities;
using WebApiCore.Class.externos.a3innuva.Context.VariableConcepts;
using WebApiCore.Class.externos.a3innuva.Controllers;
using WebApiCore.Class.Proyectos.MyPolarier.RRHH;
using WebApiCore.Context;
using WebApiCore.Enums.A3innuva;
using WebApiCore.Enums.General;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;
using WebApiCore.Security;
using static WebApiCore.Class.externos.a3innuva.A3innuvaHttpUtils;
using static WebApiCore.Controllers.tblConceptoNominaNNominaController;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH;
public class GestionNominasController : ODataController
{
    private readonly bdERP db;
    private readonly tblConceptoNominaNNominaController cnnnc;
    private readonly tblNominaController nc;
    private readonly A3innuva a3innuva = new();
    private short lastIdConceptoNomina;

    public int? idUsuario { get; set; }

    readonly Dictionary<string, short> map_idConceptoNomina = new()
        {
            { "salarioBase", 1 },
            { "plusActividad", 2 },
            { "plusAntiguedad", 3 },
            { "plusAsistencia", 4 },
            { "plusAsistenciaMesAnterior", 33 },
            { "plusFestivoTrab", 5 },
            { "incentivo", 6 },
            { "plusAbsorbible", 8 },
            { "plusProductividad", 9 },
            { "plusNocturnidad", 10 },
            { "segAccidenteConvenio", 11 },
            { "plusPeligrosidad", 12 },
            { "plusResponsabilidad", 13 },
            { "impHorasExtra", 14 },
            //{ "embargo", 15 },
            { "anticipos", 16 },
            { "pagaExtra1", 25 },
            { "pagaExtra2", 26 },
            { "acuerdoNC", 38 },
            { "salarioEspecie", 35 },
        };

    readonly List<InternalConceptImport> internalConcepts = new()
        {
            new() {
                internalConceptID = 703,
                field = "embargo",
                isPercentage = false
            },
            new() {
                internalConceptID = 790,
                field = "absentismo",
                isPercentage = false
            },
            new() {
                internalConceptID = 800,
                field = "segSocialEmpresa",
                isPercentage = false
            },
            new() {
                internalConceptID = 801,
                field = "liquidoPercibir",
                isPercentage = false
            },
            new() {
                internalConceptID = 803,
                field = "percBaseIncapacidadTemporal",
                isPercentage = true
            },
            new() {
                internalConceptID = 803,
                field = "importeBaseIncapacidadTemporal",
                isPercentage = false
            },
            new() {
                internalConceptID = 804,
                field = "percAT_EP",
                isPercentage = true
            },
            new() {
                internalConceptID = 804,
                field = "importeAT_EP",
                isPercentage = false
            },
            new() {
                internalConceptID = 805,
                field = "percDesempleo",
                isPercentage = true
            },
            new() {
                internalConceptID = 805,
                field = "importeDesempleo",
                isPercentage = false
            },
            new() {
                internalConceptID = 806,
                field = "percFormacionProfesional",
                isPercentage = true
            },
            new() {
                internalConceptID = 806,
                field = "importeFormacionProfesional",
                isPercentage = false
            },
            new() {
                internalConceptID = 807,
                field = "percFondoGarantiaSalarial",
                isPercentage = true
            },
            new() {
                internalConceptID = 807,
                field = "importeFondoGarantiaSalarial",
                isPercentage = false
            },
            new() {
                internalConceptID = 809,
                field = "percSegSocialHorasExtra",
                isPercentage = true
            },
            new() {
                internalConceptID = 809,
                field = "importeSegSocialHorasExtra",
                isPercentage = false
            },
            new() {
                internalConceptID = 837,
                field = "totalTC1",
                isPercentage = false
            },
            new() {
                internalConceptID = 989,
                field = "tributacionEspeciesEmpresa",
                isPercentage = false
            },
            new() {
                internalConceptID = -1, // TODO: No se ha encontrado el concepto en a3innuva todavía
                field = "indemnizacion",
                isPercentage = false
            },
        };

    public GestionNominasController(bdERP context, IHubContext<NotificacionesHub> _hubContext)
    {
        db = context;
        cnnnc = new(context, _hubContext);
        nc = new(context, _hubContext);
    }

    [HttpGet("odata/MyPolarier/RRHH/GestionNominas/GetHistorialNominas")]
    [Authorize]
    public ActionResult GetHistorialNominas([FromODataUri] int idPersona)
    {
        short idConceptoNomina_CotFormacionProfesional = 17;
        short idConceptoNomina_TributacionIRPF = 18;
        short idConceptoNomina_ConceptoNEspecie = 18;
        short idConceptoNomina_CotDesempleo = 20;
        short idConceptoNomina_CotCC = 21;
        short idConceptoNomina_PagaExtra1 = 25;
        short idConceptoNomina_PagaExtra2 = 26;

        var tblNomina = db.tblNomina.Where(n => n.idPersona == idPersona).Include(n => n.tblConceptoNominaNNomina).ToList();

        var historialNominas =
            tblNomina
            .Select(n => new HistoricoNomina
            {
                idNomina = n.idNomina,
                fechaCobro = n.fechaHasta,
                tipoPaga = n.tipoPaga,
                salarioBruto = n.salarioBruto,
                pagaExtra1 = GetConceptoNominaNNomina(n.tblConceptoNominaNNomina, idConceptoNomina_PagaExtra1),
                pagaExtra2 = GetConceptoNominaNNomina(n.tblConceptoNominaNNomina, idConceptoNomina_PagaExtra2),
                tributacionIRPF = GetConceptoNominaNNomina(n.tblConceptoNominaNNomina, idConceptoNomina_TributacionIRPF),
                segSocialTrabajador =
                    GetConceptoNominaNNomina(n.tblConceptoNominaNNomina, idConceptoNomina_CotFormacionProfesional)
                    + GetConceptoNominaNNomina(n.tblConceptoNominaNNomina, idConceptoNomina_CotDesempleo)
                    + GetConceptoNominaNNomina(n.tblConceptoNominaNNomina, idConceptoNomina_CotCC),
                liquidoPercibir = n.liquidoPercibir,
                anticipo = n.anticipo,
                embargo = n.embargo,
                absentismo = n.absentismo,
                indemnizacion = n.indemnizacion,
                conceptoNEspecie = GetConceptoNominaNNomina(n.tblConceptoNominaNNomina, idConceptoNomina_ConceptoNEspecie),
                segSocialEmpresa = n.segSocialEmpresa,
                totalTC1 = n.totalTC1,
                baseCC = n.baseCC,
                costeEmpresa = n.costeEmpresa,
            });

        return Ok(historialNominas);
    }

    [HttpGet("odata/MyPolarier/RRHH/GestionNominas/Personas")]
    [Authorize]
    public async Task<ActionResult> Personas([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());
        var connection = db.Database.GetDbConnection();

        var results = await connection.QueryAsync("EXEC [MyReporting].[EF_gestionNominas_personas] @idUsuario, @fechaDesde, @fechaHasta",
            new { idUsuario, fechaDesde, fechaHasta });

        return Ok(results.ToList());
    }

    [HttpPost("odata/MyPolarier/RRHH/GestionNominas/AddConceptoNNomina")]
    [Authorize]
    public async Task<ActionResult> AddConceptoNNomina([FromBody] CustomConceptoNomina concepto)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());

        short idConceptoNomina;
        map_idConceptoNomina.TryGetValue(concepto.field, out idConceptoNomina);

        CustomConceptoNominaNNomina newConcepto = new()
        {
            idPersona = concepto.idPersona,
            fechaInicioNomina = concepto.fechaInicioNomina,
            fechaFinNomina = concepto.fechaFinNomina,
            idConceptoNomina = idConceptoNomina,
            cantidad = null,
            precioUnitario = null,
            importe = concepto.importe,
            observaciones = concepto.observaciones,
        };

        if (!cnnnc.IU_tblConceptoNNomina(idUsuario, newConcepto))
        {
            return BadRequest();
        }

        await db.SaveChangesAsync();

        return Ok(true);
    }

    [HttpPost("odata/MyPolarier/RRHH/GestionNominas/EnviarNominaGestoria")]
    [Authorize]
    public async Task<ActionResult> EnviarNominaGestoria([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int idNomina, bool cambiarEstado = true, bool isPagaMensual = true)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());

        string idPersona = db.tblNomina.FirstOrDefault(n => n.idNomina == idNomina)?.idPersona.ToString() ?? "NULL";

        var connection = db.Database.GetDbConnection();
        var gestionNomina_persona = (await connection.QueryAsync<GestionNomina_persona>("EXEC [MyReporting].[EF_gestionNominas_personas] @idUsuario, @fechaDesde, @fechaHasta, @forzarCalculado, @idListPersona, @isPagaMensual", new { idUsuario, fechaDesde, fechaHasta, forzarCalculado = false, idListPersona = idPersona, isPagaMensual }))
            .FirstOrDefault(gnp => gnp.idNomina == idNomina);

        using var transaction = await db.Database.BeginTransactionAsync();
        await transaction.CreateSavepointAsync($"Savepoint_{idNomina}");

        if (gestionNomina_persona == null)
        {
            return BadRequest();
        }

        var rangoFechasDatos = GetRangoFechasDatos(fechaDesde, fechaHasta);

        try
        {
            List<tblConceptoNominaNNomina> tblConceptoNominaNNomina = new();

            if (!GuardarConceptosNDB(gestionNomina_persona, ref tblConceptoNominaNNomina))
            {
                await transaction.RollbackToSavepointAsync($"Savepoint_{idNomina}");
            }

            await HandleSalarioBrutoMensual(idNomina, gestionNomina_persona, tblConceptoNominaNNomina);

            await EnviarConceptos(idNomina, gestionNomina_persona, tblConceptoNominaNNomina, cambiarEstado);

            await HandleAbsentismos(idNomina, rangoFechasDatos, fechaDesde, fechaHasta, gestionNomina_persona);

            if (gestionNomina_persona.idTipoNomina == (short)idsTipoNomina.PagaMesual)
            {
                UpdateHistoricoSalarioBase(gestionNomina_persona);
            }
        }
        catch
        {
            await transaction.RollbackToSavepointAsync($"Savepoint_{idNomina}");
            return BadRequest();
        }

        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        return Ok(true);
    }

    [HttpPost("odata/MyPolarier/RRHH/GestionNominas/EnviarNominasGestoria")]
    [Authorize]
    public async Task<ActionResult> EnviarNominasGestoria([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromBody] List<int> idsPersona)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());
        List<byte> idsEstadoNominaCustom = new()
        {
            (byte)idsEstadoNomina.EnProceso,
            (byte)idsEstadoNomina.ValidadoRRHH,
        };

        var idListPersona = String.Join('|', idsPersona);

        var connection = db.Database.GetDbConnection();
        var gestionNominas_personas = (await connection.QueryAsync<GestionNomina_persona>("EXEC [MyReporting].[EF_gestionNominas_personas] @idUsuario, @fechaDesde, @fechaHasta, @forzarCalculado, @idListPersona", new { idUsuario, fechaDesde, fechaHasta, forzarCalculado = false, idListPersona }))
            .Where(gnp => gnp.idEstadoNomina <= (byte)idsEstadoNomina.ValidadoRRHH)
            .ToList();

        var gestionNominas_personas_toSend = gestionNominas_personas
            .Where(gnp =>
                gnp.idNomina != null
                && idsEstadoNominaCustom.Contains(gnp.idEstadoNomina)
                && gnp.enableSend
            )
            .ToList();

        var rangoFechasDatos = GetRangoFechasDatos(fechaDesde, fechaHasta);

        var idsNomina = gestionNominas_personas_toSend.Select(gnp => (int)gnp.idNomina).ToList();

        using var transaction = await db.Database.BeginTransactionAsync();
        foreach (var idNomina in idsNomina)
        {
            // Crear punto de control
            await transaction.CreateSavepointAsync($"Savepoint_{idNomina}");

            try
            {
                var gestionNomina_persona = gestionNominas_personas_toSend.FirstOrDefault(gnp => gnp.idNomina == idNomina);

                List<tblConceptoNominaNNomina> tblConceptoNominaNNomina = new();

                if (!GuardarConceptosNDB(gestionNomina_persona, ref tblConceptoNominaNNomina))
                {
                    // Deshacer cambios y continuar con la siguiente nómina
                    await transaction.RollbackToSavepointAsync($"Savepoint_{idNomina}");
                    continue;
                }

                await HandleSalarioBrutoMensual(idNomina, gestionNomina_persona, tblConceptoNominaNNomina);

                await EnviarConceptos(idNomina, gestionNomina_persona, tblConceptoNominaNNomina);

                await HandleAbsentismos(idNomina, rangoFechasDatos, fechaDesde, fechaHasta, gestionNomina_persona);

                if (gestionNomina_persona.idTipoNomina == (short)idsTipoNomina.PagaMesual)
                {
                    UpdateHistoricoSalarioBase(gestionNomina_persona);
                }
            }
            catch
            {
                // Deshacer cambios y continuar con la siguiente nómina
                await transaction.RollbackToSavepointAsync($"Savepoint_{idNomina}");
                continue;
            }
        }

        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        return Ok(true);
    }

    [HttpGet("odata/MyPolarier/RRHH/GestionNominas/CompararNominaGestoria")]
    [Authorize]
    public async Task<ActionResult> CompararNominaGestoria([FromODataUri] int idPersona, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        try
        {
            var result = await GetConceptoNominaComparacionNPersona(idPersona, fechaDesde, fechaHasta);

            return Ok(result);
        }
        catch (A3innuvaHttpServiceException ex)
        {
            if (ex.statusCode == HttpStatusCode.TooManyRequests)
            {
                return Ok(ex.Get_SecondsToWait());
            }
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("odata/MyPolarier/RRHH/GestionNominas/GetHistoricoPagosPersona")]
    [Authorize]
    public ActionResult GetHistoricoPagosPersona([FromODataUri] int idPersona, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        var historico = db.tblHistoricoAsientoNomina
            .Where(han =>
                han.idNominaNavigation.idPersona == idPersona &&
                han.fechaDesde >= fechaDesde &&
                han.fechaHasta <= fechaHasta &&
                han.idEstadoHistoricoAsientoNomina == (byte)idsEstadoHistoricoAsientoNomina.Pagado)
            .OrderBy(han => han.fecha)
            .GroupBy(han => new { han.fecha })
            .Select(g => new
            {
                g.Key.fecha,
                fechaContabilizado = g.FirstOrDefault() != null ? g.FirstOrDefault()!.fechaContabilizado : null,
                salarioBruto = g.Sum(han => han.salarioBruto),
                liquidoPercibir = g.Sum(han => han.liquidoPercibir)
            })
            .ToList();

        var result = historico
            .Select((item, index) => new
            {
                item.fecha,
                item.fechaContabilizado,
                item.salarioBruto,
                item.liquidoPercibir,
                pagado = index == 0 ? item.liquidoPercibir : item.liquidoPercibir - historico[index - 1].liquidoPercibir
            });

        return Ok(result);
    }

    [HttpPost("odata/MyPolarier/RRHH/GestionNominas/ImportarNominaGestoria")]
    [Authorize]
    public async Task<ActionResult> ImportarNominaGestoria([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int idPersona)
    {
        var persona = db.tblPersona.FirstOrDefault(p => p.idPersona == idPersona);

        if (persona == null)
        {
            return BadRequest("Persona no existente");
        }

        var tblLavanderia = db.tblLavanderia.Where(l => l.idPais == (int)idsPais.España).ToList();

        var pays = await GetAllPays(fechaDesde.Year, fechaDesde.Month, A3innuvaUtils.GetCompanyCode(tblLavanderia, persona));

        var rangoFechasDatos = GetRangoFechasDatos(fechaDesde, fechaHasta);

        lastIdConceptoNomina = db.tblConceptoNomina.Max(cn => cn.idConceptoNomina);

        using var transaction = await db.Database.BeginTransactionAsync();

        // Crear punto de control
        await transaction.CreateSavepointAsync($"Savepoint_{idPersona}");

        try
        {
            var conflictoNominaContrato = await HandleImportarDatosNominaGestoria(idPersona, fechaDesde, fechaHasta, pays, rangoFechasDatos);

            await db.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(conflictoNominaContrato);
        }
        catch (Exception ex)
        {
            await transaction.RollbackToSavepointAsync($"Savepoint_{idPersona}");

            return BadRequest(ex.Message);
        }
    }

    [HttpPost("odata/MyPolarier/RRHH/GestionNominas/ImportarNominasGestoria")]
    [Authorize]
    public async Task<ActionResult> ImportarNominasGestoria([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromBody] List<int> idsPersona)
    {
        var pays = await GetAllPays(fechaDesde.Year, fechaDesde.Month);

        var rangoFechasDatos = GetRangoFechasDatos(fechaDesde, fechaHasta);

        lastIdConceptoNomina = db.tblConceptoNomina.Max(cn => cn.idConceptoNomina);
        bool conflictoNominaContrato = false;

        using var transaction = await db.Database.BeginTransactionAsync();
        foreach (var idPersona in idsPersona)
        {
            // Crear punto de control
            await transaction.CreateSavepointAsync($"Savepoint_{idPersona}");

            try
            {
                var result = await HandleImportarDatosNominaGestoria(idPersona, fechaDesde, fechaHasta, pays, rangoFechasDatos);

                conflictoNominaContrato = result || conflictoNominaContrato;
            }
            catch
            {
                // Deshacer cambios y continuar con la siguiente persona
                await transaction.RollbackToSavepointAsync($"Savepoint_{idPersona}");
                continue;
            }
        }

        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        return Ok(conflictoNominaContrato);
    }

    [HttpPost("odata/MyPolarier/RRHH/GestionNominas/ImportarMesCompleto_NominasGestoria")]
    [Authorize]
    public async Task<ActionResult> ImportarMesCompleto_NominasGestoria([FromODataUri] short idEmpresaPolarier, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int pageNumber, [FromODataUri] int pageSize = 10)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());
        lastIdConceptoNomina = db.tblConceptoNomina.Max(cn => cn.idConceptoNomina);

        var idsPersona = await db.tblPersonaNTipoContrato
            .Where(pntc =>
                (
                    (pntc.idPersonaNavigation.idAdmCentroCosteNavigation != null && pntc.idPersonaNavigation.idAdmCentroCosteNavigation.idEmpresaPolarier == idEmpresaPolarier)
                    || (pntc.idPersonaNavigation.idAdmElementoPEPNavigation != null && pntc.idPersonaNavigation.idAdmElementoPEPNavigation.idEmpresaPolarier == idEmpresaPolarier)
                )
                && pntc.fechaAltaContrato.Date <= fechaHasta.Date
                && (pntc.fechaBajaContrato == null || ((DateTime)pntc.fechaBajaContrato).Date >= fechaDesde.Date)
            )
            .Select(pntc => pntc.idPersona)
            .Distinct()
            .OrderBy(idPersona => idPersona)
            .ToListAsync();

        List<byte> idsEstadoNominaFiniquitos = new()
        {
            (byte)idsEstadoNomina.SolicitudFiniquitoCreada,
            (byte)idsEstadoNomina.SolicitudFiniquitoInterna
        };

        var idsPersonaFiniquitosSinEnviar = await db.tblNomina
            .Where(n =>
                idsPersona.Contains(n.idPersona)
                && n.fechaDesde.Date >= fechaDesde.Date
                && n.fechaHasta.Date <= fechaHasta.Date
                && n.idTipoNomina == (short)idsTipoNomina.PagaFiniquito
                && idsEstadoNominaFiniquitos.Contains(n.idEstadoNomina)
            )
            .Select(pntc => pntc.idPersona)
            .Distinct()
            .ToListAsync();

        idsPersona = idsPersona.Except(idsPersonaFiniquitosSinEnviar).ToList();

        if (pageNumber == 1)
        {
            await ComprobarPagasA3_SinImportacionMyPolarier();
        }

        var totalPages = (int)Math.Ceiling((decimal)idsPersona.Count < pageSize ? pageSize : (decimal)idsPersona.Count / pageSize);

        if (totalPages == 0)
        {
            return Ok(new { currentPage = pageNumber, totalPages });
        }

        if (pageNumber <= 0 || pageNumber > totalPages)
        {
            return BadRequest();
        }

        var idsPersona_filtered = idsPersona.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        var importarNominasGestoriaResult = await ImportarNominasGestoria(fechaDesde, fechaHasta, idsPersona_filtered);

        if (importarNominasGestoriaResult is OkObjectResult okObjectResult)
        {
            bool conflictoNominaContrato = (bool)okObjectResult.Value;
            return Ok(new { currentPage = pageNumber, totalPages, conflictoNominaContrato });
        }
        else
        {
            return BadRequest();
        }

        async Task ComprobarPagasA3_SinImportacionMyPolarier()
        {
            try
            {
                var pays = await GetAllPays(fechaDesde.Year, fechaDesde.Month);

                var idsPersonaTodasEmpresas = db.tblPersonaNTipoContrato
                    .Where(pntc =>
                        pntc.fechaAltaContrato.Date <= fechaHasta.Date
                        && (pntc.fechaBajaContrato == null || ((DateTime)pntc.fechaBajaContrato).Date >= fechaDesde.Date)
                    )
                    .Select(pntc => pntc.idPersona)
                    .Distinct()
                    .OrderBy(idPersona => idPersona)
                    .ToList();

                var employeeCodes = pays.Select(p => p.employeeCode).Distinct().ToList();

                var tblPersona = db.tblPersona.Where(p => idsPersonaTodasEmpresas.Contains(p.idPersona)).ToList();

                var employeeCodesNoContemplados = employeeCodes.Where(employeeCode => !tblPersona.Any(p => p.codigoGestoria == employeeCode)).ToList();

                if (employeeCodesNoContemplados.Count > 0)
                {
                    var personasNoContempladas = db.tblPersona.Where(p => employeeCodesNoContemplados.Contains(p.codigoGestoria)).ToList();

                    employeeCodesNoContemplados = employeeCodesNoContemplados.Except(personasNoContempladas.Select(p => p.codigoGestoria)).ToList();

                    SendAviso_Personas_ConPagaA3_SinImportacionMyPolarier(employeeCodesNoContemplados, personasNoContempladas);
                }
            }
            catch (Exception ex)
            {
                SendAviso_FalloAlRevisar_Personas_ConPagaA3_SinImportacionMyPolarier(ex);
            }
        }
    }

    [HttpGet("odata/MyPolarier/RRHH/GestionNominas/GetResumenConflictosDatosSalariales")]
    [Authorize]
    public async Task<ActionResult> GetResumenConflictosDatosSalariales([FromODataUri] int pageNumber, [FromODataUri] int pageSize = 20)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());
        var usuario = db.tblUsuario
            .Include(u => u.idLavanderia)
            .Include(u => u.idCentroTrabajo)
            .FirstOrDefault(u => u.idUsuario == idUsuario);

        if (usuario == null)
        {
            return BadRequest();
        }

        Dictionary<string, short> map_idConceptoNomina_datosSalariales = new()
        {
            { "salarioBase", (short)idsConceptoNomina.SalarioBase },
            { "plusAntiguedad", (short)idsConceptoNomina.PlusAntiguedad },
            { "plusAsistencia", (short)idsConceptoNomina.PlusAsistencia },
            { "incentivo", (short)idsConceptoNomina.Incentivos },
            { "plusPeligrosidad", (short)idsConceptoNomina.PlusPeligrosidad },
            { "plusResponsabilidad", (short)idsConceptoNomina.PlusResponsabilidad },
            { "acuerdoNC", (short)idsConceptoNomina.AcuerdoNC },
        };

        Dictionary<string, string> map_conceptosCustom = new()
        {
            { "codigoGestoria", "Código gestoría" },
            { "numDocumentoIdentidad", "DNI/NIE" },
            { "NAF", "NAF" },
            { "fechaAntiguedad", "Fecha antigüedad" },
            { "IBAN", "IBAN" },
            { "salarioBrutoMensual", "Salario mensual pactado" },
            { "personasActivasInactivas", "Personas ACTIVAS / NO ACTIVAS" },
        };

        var fechaDesde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var fechaHasta = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day);

        var tblLavanderia = db.tblLavanderia.Where(l => l.idPais == (int)idsPais.España).ToList();

        var connection = db.Database.GetDbConnection();
        var datosSalarialesNPersona = (await connection.QueryAsync("EXEC [MyReporting].[EF_gestionNominas_datosSalariales] @idUsuario, @fechaDesde, @fechaHasta", new { idUsuario, fechaDesde, fechaHasta }))
            .ToList()
            .Select(ds => new
            {
                idPersona = (int)ds.idPersona,
                numDocumentoIdentidad = ds.numDocumentoIdentidad?.Trim()?.ToUpper(),
                NAF = ds.NAF?.Trim(),
                ds.nombre,
                ds.apellidos,
                employeeCode = ds.codigoGestoria,
                companyCode = A3innuvaUtils.GetCompanyCode(tblLavanderia, new tblPersona { idLavanderia = ds.idLavanderia, idCentroTrabajo = ds.idCentroTrabajo }),
                ds.salarioBase,
                ds.plusAntiguedad,
                ds.plusAsistencia,
                ds.plusResponsabilidad,
                ds.plusPeligrosidad,
                ds.incentivo,
                ds.acuerdoNC,
                ds.salarioBrutoMensual,
                ds.fechaAntiguedad,
                IBAN = ds.IBAN?.Replace(" ", "").ToUpper()
            }).ToList();

        var totalPages = (int)Math.Round((decimal)datosSalarialesNPersona.Count / pageSize);

        if (pageNumber <= 0 || pageNumber > totalPages)
        {
            return BadRequest();
        }

        var datosSalarialesNPersona_filtered = datosSalarialesNPersona.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        List<ResumenConflictoDatosSalariales> result = new();

        foreach (var cv in map_idConceptoNomina_datosSalariales)
        {
            var conceptField = cv.Key;
            var idConceptoNomina = cv.Value;

            var concepto = db.tblConceptoNomina.FirstOrDefault(cn => cn.idConceptoNomina == idConceptoNomina);

            result.Add(new ResumenConflictoDatosSalariales
            {
                field = conceptField,
                denominacion = concepto.denominacion,
                conflictos = new()
            });
        }

        foreach (var cv in map_conceptosCustom)
        {
            var conceptField = cv.Key;
            var denominacion = cv.Value;

            result.Add(new ResumenConflictoDatosSalariales
            {
                field = conceptField,
                denominacion = denominacion,
                conflictos = new()
            });
        }

        if (pageNumber == 1)
        {
            await Handle_FirstPage();
        }

        foreach (var dsnp in datosSalarialesNPersona_filtered)
        {
            try
            {
                if (await Handle_identificacion() == "continue")
                {
                    continue;
                }

                await Handle_concepts();

                await Handle_fechaAntiguedad();

                await Handle_IBAN();

                await Handle_salarioBrutoMensual();

                async Task<string?> Handle_identificacion()
                {
                    if (dsnp.companyCode == null)
                    {
                        return "continue";
                    }

                    if (dsnp.numDocumentoIdentidad == null && dsnp.employeeCode == null)
                    {
                        Handle_AddConflictosIdentificacion(dsnp, null);
                        return "continue";
                    }
                    else
                    {
                        async Task<List<Employees_Get_Employees>?> GetEmployeesAsync(int companyCode, string field, string value)
                        {
                            A3innuvaFilter filter = new()
                            {
                                field = field,
                                type = "eq",
                                value = value,
                                isString = true
                            };

                            var response_getEmployees = await a3innuva.Employees.Get_Employees(companyCode, 1, 50, filter);
                            return await A3innuvaUtils.DeserializeResponseAsync<List<Employees_Get_Employees>>(response_getEmployees);
                        }

                        if (dsnp.numDocumentoIdentidad != null)
                        {
                            var employees = await GetEmployeesAsync((int)dsnp.companyCode, "identifierNumber", dsnp.numDocumentoIdentidad);

                            if (employees == null && dsnp.companyCode != null)
                            {
                                employees = await GetEmployeesAsync((int)dsnp.companyCode, "employeeCode", dsnp.employeeCode);
                            }

                            var employee = employees?.OrderByDescending(e => e.enrolmentDate).FirstOrDefault();
                            Handle_AddConflictosIdentificacion(dsnp, employee);
                        }
                        else if (dsnp.employeeCode != null)
                        {
                            var employees = await GetEmployeesAsync((int)dsnp.companyCode, "employeeCode", dsnp.employeeCode);

                            var employee = employees?.OrderByDescending(e => e.enrolmentDate).FirstOrDefault();
                            Handle_AddConflictosIdentificacion(dsnp, employee);
                        }
                    }

                    // Si no tiene códigoGestoria, no se puede comparar el resto de datos salariales
                    if (dsnp.employeeCode == null)
                    {
                        Handle_AddConflicto(dsnp.nombre, dsnp.apellidos, "fechaAntiguedad", dsnp.fechaAntiguedad.Date, null);
                        Handle_AddConflicto(dsnp.nombre, dsnp.apellidos, "IBAN", formatIBAN(dsnp.IBAN), null);

                        return "continue";
                    }

                    return null;
                }

                async Task Handle_concepts()
                {
                    List<EmployeesConcepts_Get_Concepts> conceptosA3 = await GetAllEmployeeConcepts((int)dsnp.companyCode, dsnp.employeeCode);

                    foreach (var cv in map_idConceptoNomina_datosSalariales)
                    {
                        var conceptField = cv.Key;
                        var idConceptoNomina = cv.Value;

                        var property = dsnp.GetType().GetProperty(conceptField);

                        if (property != null)
                        {
                            var concepto = db.tblConceptoNomina.FirstOrDefault(cn => cn.idConceptoNomina == idConceptoNomina);

                            if (concepto == null)
                            {
                                throw new Exception($"Error Handle_concepts: El idConceptoNomina {idConceptoNomina} no se encuentra en la BBDD.");
                            }

                            var conceptCode = concepto.codigo;

                            if (conceptCode == ((int)conceptsCodes.AcuerdoNCPolarier).ToString())
                            {
                                conceptCode = A3innuvaUtils.GetAcuerdoNC((int)dsnp.companyCode).ToString();
                            }

                            var valor = (decimal?)property.GetValue(dsnp) ?? 0;
                            var valorA3 = conceptosA3?.FirstOrDefault(c => c.conceptCode.ToString() == conceptCode)?.amount ?? 0;

                            Handle_AddConflicto(dsnp.nombre, dsnp.apellidos, conceptField, valor, valorA3);
                        }
                    }
                }

                async Task Handle_fechaAntiguedad()
                {
                    var response_getHiringdates = await a3innuva.Employees.Get_Hiringdates((int)dsnp.companyCode, dsnp.employeeCode);
                    Employees_Get_Hiringdates fechasA3 = await A3innuvaUtils.DeserializeResponseAsync<Employees_Get_Hiringdates>(response_getHiringdates);

                    Handle_AddConflicto(dsnp.nombre, dsnp.apellidos, "fechaAntiguedad", dsnp.fechaAntiguedad.Date, fechasA3.seniorityCompanyDate.Date);
                }

                async Task Handle_IBAN()
                {
                    var response_getBankaccounts = await a3innuva.EmployeesBankaccounts.Get_Bankaccounts((int)dsnp.companyCode, dsnp.employeeCode);
                    List<EmployeesBankaccounts_Get_Bankaccounts> bankAccountCodes = await A3innuvaUtils.DeserializeResponseAsync<List<EmployeesBankaccounts_Get_Bankaccounts>>(response_getBankaccounts);

                    var mainAccountCode = bankAccountCodes.FirstOrDefault(cb => cb.isMainAccount);

                    if (mainAccountCode != null)
                    {
                        var response_getBankaccount = await a3innuva.EmployeesBankaccounts.Get_Bankaccount((int)dsnp.companyCode, dsnp.employeeCode, mainAccountCode.bankAccountCode);
                        EmployeesBankaccounts_Get_Bankaccount mainBankAccount = await A3innuvaUtils.DeserializeResponseAsync<EmployeesBankaccounts_Get_Bankaccount>(response_getBankaccount);

                        Handle_AddConflicto(dsnp.nombre, dsnp.apellidos, "IBAN", formatIBAN(dsnp.IBAN), formatIBAN(mainBankAccount.iban));
                    }
                    else
                    {
                        Handle_AddConflicto(dsnp.nombre, dsnp.apellidos, "IBAN", formatIBAN(dsnp.IBAN), null);
                    }
                }

                async Task Handle_salarioBrutoMensual()
                {

                    HttpResponseMessage response_getSalaryadjustments;

                    try
                    {
                        response_getSalaryadjustments = await a3innuva.EmployeesSalary.Get_Salaryadjustments((int)dsnp.companyCode, dsnp.employeeCode);
                    }
                    catch // La llamada devulve BadRequest para las personas que no tienen salario bruto pactado aunque companyCode y employeeCode sean validos.
                    {
                        return;
                    }

                    var salaryadjustments = await A3innuvaUtils.DeserializeResponseAsync<EmployeesSalary_Get_Salaryadjustments>(response_getSalaryadjustments);

                    Handle_AddConflicto(dsnp.nombre, dsnp.apellidos, "salarioBrutoMensual", dsnp.salarioBrutoMensual, salaryadjustments.amount);
                }
            }
            catch
            {
                continue;
            }

            string? formatIBAN(string iban)
            {
                if (iban == null)
                {
                    return null;
                }

                return string.Join(" ", Enumerable.Range(0, iban.Length / 4).Select(i => iban.Substring(i * 4, 4)));
            }

            void Handle_AddConflictosIdentificacion(dynamic dsnp, Employees_Get_Employees? employee)
            {
                Handle_AddConflicto(dsnp.nombre, dsnp.apellidos, "codigoGestoria", dsnp.employeeCode, employee?.employeeCode);
                Handle_AddConflicto(dsnp.nombre, dsnp.apellidos, "numDocumentoIdentidad", dsnp.numDocumentoIdentidad, employee?.identifierNumber);
                Handle_AddConflicto(dsnp.nombre, dsnp.apellidos, "NAF", dsnp.NAF, employee != null ? Regex.Replace(employee.ssNAF, "[^0-9]", "") : null);
            }
        }

        return Ok(new { currentPage = pageNumber, totalPages, result });

        void Handle_AddConflicto(string nombre, string apellidos, string field, dynamic? valor, dynamic? valorA3)
        {
            if (valor != valorA3)
            {
                var conceptoResumen = result.FirstOrDefault(r => r.field == field);

                conceptoResumen?.conflictos.Add(new ConflictoDatosSalariales()
                {
                    nombre = nombre,
                    apellidos = apellidos,
                    valor = valor,
                    valorA3 = valorA3
                });
            }
        }

        async Task Handle_FirstPage()
        {
            // En A3, en lugar de devolver null para las fechas, devuelve 01/01/0001
            var dateLikeNull = new DateTime(1, 1, 1);

            List<int> idsPersonaVisible = new();

            var idsPersonaGestionaPluses = db.tblPersona
                .Join(
                    db.tblTipoTrabajoNUsuario.Where(t => t.idUsuario == idUsuario && t.gestionaPlusesNomina),
                    p => new { p.idLavanderia, p.idTipoTrabajo },
                    t => new { idLavanderia = (int?)t.idLavanderia, idTipoTrabajo = (byte?)t.idTipoTrabajo },
                    (p, t) => p.idPersona
                )
                .Distinct()
                .ToList();

            idsPersonaVisible.AddRange(idsPersonaGestionaPluses);

            var idsPersonaValidacionNomina = db.tblPersona
                .Where(p => p.idUsuario_validacion_nomina == idUsuario && !idsPersonaVisible.Contains(p.idPersona))
                .Select(p => p.idPersona)
                .Distinct()
                .ToList();

            idsPersonaVisible.AddRange(idsPersonaValidacionNomina);

            if (usuario.enableDatosRRHH)
            {
                var idsLavanderiaConAcceso = usuario.idLavanderia.Select(l => (int?)l.idLavanderia).ToList();

                var idsPersonaLavanderia = db.tblPersona
                    .Where(p => !idsPersonaVisible.Contains(p.idPersona) && idsLavanderiaConAcceso.Contains(p.idLavanderia))
                    .Select(p => p.idPersona)
                    .Distinct()
                    .ToList();

                idsPersonaVisible.AddRange(idsPersonaLavanderia);

                if (usuario.enableDatosSalarialesOficina)
                {
                    var idsCentroTrabajoConAcceso = usuario.idCentroTrabajo.Where(ct => ct.idPais == (int)idsPais.España).Select(ct => (int?)ct.idCentroTrabajo).ToList();

                    var idsPersonaCentroTrabajo = db.tblPersona
                        .Where(p => !idsPersonaVisible.Contains(p.idPersona) && idsCentroTrabajoConAcceso.Contains(p.idCentroTrabajo))
                        .Select(p => p.idPersona)
                        .Distinct()
                        .ToList();

                    idsPersonaVisible.AddRange(idsPersonaCentroTrabajo);
                }
            }

            var hoy = DateTime.Today;
            var primerDiaMesAnterior = new DateTime(hoy.Year, hoy.Month - 1, hoy.Day);

            var filter = $"personTypeID eq 0 and (dropDate eq null or dropDate ge {primerDiaMesAnterior.ToString("yyyy-MM-dd")})";

            var employeesNCompany = await A3innuvaUtils.GetAllEmployeesAsync(a3innuva, filter);

            var tblPersona = db.tblPersona
                .Where(p =>
                    p.tblUsuario.FirstOrDefault() != null
                    && (
                        p.tblUsuario.FirstOrDefault()!.idLocalizacion == (short)idsLocalizacion.España_PeninsulaBaleares
                        || p.tblUsuario.FirstOrDefault()!.idLocalizacion == (short)idsLocalizacion.España_IslasCanarias
                    )
                )
                .ToList()
                .Where(p =>
                    !p.numDocumentoIdentidad.IsNullOrEmpty()
                    || !p.codigoGestoria.IsNullOrEmpty()
                )
                .ToList();

            var idsPersona = tblPersona.Select(p => p.idPersona);

            var tblPersonaNTipoContrato = db.tblPersonaNTipoContrato
                .Where(pntc =>
                    idsPersona.Contains(pntc.idPersona)
                    && (pntc.fechaBajaContrato == null || ((DateTime)pntc.fechaBajaContrato).Date >= primerDiaMesAnterior.Date)
                )
                .GroupBy(pntc => pntc.idPersona)
                .Select(g => g.OrderByDescending(p => p.fechaAltaContrato).First())
                .ToList();

            var tblLavanderia = db.tblLavanderia.Where(l => l.idPais == (int)idsPais.España).ToList();

            foreach (var enc in employeesNCompany)
            {
                var tblPersona_filtered = tblPersona.Where(p => enc.Key == A3innuvaUtils.GetCompanyCode(tblLavanderia, p));

                var personasSoloA3 = (
                    from e in enc.Value

                    join p_codigoGestoria in tblPersona_filtered
                    on new { codigoGestoria = e.employeeCode, condicion = false } equals new { codigoGestoria = p_codigoGestoria.codigoGestoria?.Replace(" ", "")?.ToLower(), condicion = p_codigoGestoria.codigoGestoria.IsNullOrEmpty() } into p_codigoGestoriaGroup
                    from p_codigoGestoria in p_codigoGestoriaGroup.DefaultIfEmpty()

                    join p_numDocumentoIdentidad in tblPersona_filtered
                    on new { numDocumentoIdentidad = e.identifierNumber, condicion = false } equals new { numDocumentoIdentidad = p_numDocumentoIdentidad.numDocumentoIdentidad?.Replace(" ", "")?.ToLower(), condicion = p_numDocumentoIdentidad.numDocumentoIdentidad.IsNullOrEmpty() } into p_numDocumentoIdentidadGroup
                    from p_numDocumentoIdentidad in p_codigoGestoriaGroup.DefaultIfEmpty()

                    let p = p_codigoGestoria ?? p_numDocumentoIdentidad

                    where
                        p == null

                    select e
                )
                .ToList();

                var personasSoloMyPolarier = (
                    from p in tblPersona_filtered

                    join pntc in tblPersonaNTipoContrato
                    on p.idPersona equals pntc.idPersona

                    join e_employeeCode in enc.Value
                    on new { employeeCode = p.codigoGestoria, condicion = p.codigoGestoria.IsNullOrEmpty() } equals new { e_employeeCode.employeeCode, condicion = false } into e_employeeCodeGroup
                    from e_employeeCode in e_employeeCodeGroup.DefaultIfEmpty()

                    join e_identifierNumber in enc.Value
                    on p.numDocumentoIdentidad equals e_identifierNumber.identifierNumber into e_identifierNumberGroup
                    from e_identifierNumber in e_identifierNumberGroup.DefaultIfEmpty()

                    let e = e_employeeCode ?? e_identifierNumber

                    where
                        e == null

                    select new
                    {
                        p,
                        pntc
                    }
                )
                .Where(psmp =>
                    (usuario.enableDatosRRHH && usuario.enableDatosSalarialesOficina)
                    || idsPersonaVisible.Contains(psmp.p.idPersona)
                )
                .ToList();

                var personasConflictos = (
                        from p in tblPersona_filtered

                        join pntc in tblPersonaNTipoContrato
                        on p.idPersona equals pntc.idPersona

                        join e_employeeCode in enc.Value
                        on new { employeeCode = p.codigoGestoria, condicion = p.codigoGestoria.IsNullOrEmpty() } equals new { e_employeeCode.employeeCode, condicion = false } into e_employeeCodeGroup
                        from e_employeeCode in e_employeeCodeGroup.DefaultIfEmpty()

                        join e_identifierNumber in enc.Value
                        on p.numDocumentoIdentidad equals e_identifierNumber.identifierNumber into e_identifierNumberGroup
                        from e_identifierNumber in e_identifierNumberGroup.DefaultIfEmpty()

                        let e = e_employeeCode ?? e_identifierNumber
                        let e_dropDate = e?.dropDate.Date == dateLikeNull.Date ? (DateTime?)null : e?.dropDate.Date

                        where
                            e != null
                            && (
                                (e_dropDate?.Date != pntc.fechaBajaContrato?.Date)
                                || (e.enrolmentDate.Date != pntc.fechaAltaContrato.Date)
                            )
                            && (
                                (e_dropDate?.Year == hoy.Year && e_dropDate?.Month == hoy.Month)
                                || (e.enrolmentDate.Year == hoy.Year && e.enrolmentDate.Month == hoy.Month)
                                || (pntc.fechaBajaContrato?.Year == hoy.Year && pntc.fechaBajaContrato?.Month == hoy.Month)
                                || (pntc.fechaAltaContrato.Year == hoy.Year && pntc.fechaAltaContrato.Month == hoy.Month)
                            )

                        select new
                        {
                            p,
                            pntc,
                            e,
                        }
                    )
                    .Where(pc =>
                        (usuario.enableDatosRRHH && usuario.enableDatosSalarialesOficina)
                        || idsPersonaVisible.Contains(pc.p.idPersona)
                    )
                    .ToList();

                foreach (var psA3 in personasSoloA3)
                {
                    string[] parts = psA3.completeName.Split(',');
                    string nombre = parts.Length == 2 ? parts[1].Trim() : psA3.completeName;
                    string apellidos = parts.Length == 2 ? parts[0].Trim() : "";

                    var valorA3 = psA3.dropDate.Date == dateLikeNull.Date ? $"ACTIVO ({psA3.enrolmentDate.ToString("dd/MM/yyyy")})" : $"NO ACTIVO ({psA3.dropDate.ToString("dd/MM/yyyy")})";

                    Handle_AddConflicto(nombre, apellidos, "personasActivasInactivas", "NO ACTIVO (no encontrado)", valorA3);

                }

                foreach (var pcmp in personasSoloMyPolarier)
                {
                    var valor = pcmp.pntc.fechaBajaContrato == null ? $"ACTIVO ({pcmp.pntc.fechaAltaContrato.ToString("dd/MM/yyyy")})" : $"NO ACTIVO ({pcmp.pntc.fechaBajaContrato?.ToString("dd/MM/yyyy")})";

                    Handle_AddConflicto(pcmp.p.nombre, pcmp.p.apellidos, "personasActivasInactivas", valor, "NO ACTIVO (no encontrado)");
                }

                foreach (var pc in personasConflictos)
                {
                    var valor = pc.pntc.fechaBajaContrato == null ? $"ACTIVO ({pc.pntc.fechaAltaContrato.ToString("dd/MM/yyyy")})" : $"NO ACTIVO ({pc.pntc.fechaBajaContrato?.ToString("dd/MM/yyyy")})";

                    var valorA3 = pc.e.dropDate.Date == dateLikeNull.Date ? $"ACTIVO ({pc.e.enrolmentDate.ToString("dd/MM/yyyy")})" : $"NO ACTIVO ({pc.e.dropDate.ToString("dd/MM/yyyy")})";

                    Handle_AddConflicto(pc.p.nombre, pc.p.apellidos, "personasActivasInactivas", valor, valorA3);
                }
            }
        }
    }

    [HttpPost("odata/MyPolarier/RRHH/GestionNominas/AceptarNominaGestoria")]
    [Authorize]
    public async Task<ActionResult> AceptarNominaGestoria([FromODataUri] int idNomina, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());

        var nomina = db.tblNomina.FirstOrDefault(n => n.idNomina == idNomina);

        if (nomina == null)
        {
            return BadRequest();
        }

        var tblConceptoNominaNNomina_Gestoria = db.tblConceptoNominaNNomina_Gestoria.Where(cnnn => cnnn.idNomina == idNomina);

        var tblConceptoNominaNNomina = db.tblConceptoNominaNNomina
            .Where(cnnn => cnnn.idNomina == idNomina && tblConceptoNominaNNomina_Gestoria.Select(cnnng => cnnng.idConceptoNomina).Contains(cnnn.idConceptoNomina));

        db.tblConceptoNominaNNomina.RemoveRange(tblConceptoNominaNNomina);

        db.tblConceptoNominaNNomina.AddRange(tblConceptoNominaNNomina_Gestoria.Select(cnnng => new tblConceptoNominaNNomina
        {
            idNomina = cnnng.idNomina,
            idConceptoNomina = cnnng.idConceptoNomina,
            cantidad = cnnng.cantidad,
            precioUnitario = cnnng.precioUnitario,
            observaciones = cnnng.observaciones,
            idUsuario_validacion = cnnng.idUsuario_validacion,
            fecha_validacion = cnnng.fecha_validacion,
            fecha = cnnng.fecha,
            importe = cnnng.importe
        }));

        db.tblConceptoNominaNNomina_Gestoria.RemoveRange(tblConceptoNominaNNomina_Gestoria);

        nomina.idEstadoNomina = (byte)idsEstadoNomina.ValidadoGestoria;

        nomina.tblEstadoNominaNNomina.Add(new tblEstadoNominaNNomina
        {
            idEstadoNomina = (byte)idsEstadoNomina.ValidadoGestoria,
            fecha = DateTimeOffset.UtcNow,
            idUsuario_valida = idUsuario
        });

        await db.SaveChangesAsync();

        var comparacionNominaResult = await CompararNominaGestoria(nomina.idPersona, fechaDesde, fechaHasta);

        if (comparacionNominaResult is OkObjectResult okObjectResult)
        {
            return Ok(okObjectResult.Value);
        }
        else
        {
            return comparacionNominaResult;
        }
    }

    [HttpPost("odata/MyPolarier/RRHH/GestionNominas/AceptarNominasGestoria")]
    [Authorize]
    public async Task<ActionResult> AceptarNominasGestoria([FromODataUri] int idPersona, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());

        var tblNomina = await db.tblNomina.Where(n =>
            n.idPersona == idPersona
            && n.fechaDesde.Date >= fechaDesde.Date
            && n.fechaHasta.Date <= fechaHasta.Date
            && n.idTipoNomina == (byte)idsTipoNomina.PagaMesual
        ).ToListAsync();

        var tblPersonaNTipoContrato = GetTblPersonaNTipoContrato(idPersona, fechaDesde, fechaHasta);

        var nominasConContrato = (
            from n in tblNomina
            join pntc in tblPersonaNTipoContrato on new { fechaDesde = n.fechaDesde.Date, fechaHasta = n.fechaHasta.Date } equals new { fechaDesde = pntc.fechaAltaContrato.Date, fechaHasta = ((DateTime)pntc.fechaBajaContrato).Date }
            select n
        ).ToList();

        nc.DeleteNominas(nominasConContrato);

        var nominasSinValidar = tblNomina.Where(n => n.idEstadoNomina != (byte)idsEstadoNomina.ValidadoGestoria && !nominasConContrato.Select(n => n.idNomina).Contains(n.idNomina));

        for (int i = 0; i < nominasSinValidar.Count(); i++)
        {
            var nf = nominasSinValidar.ElementAt(i);

            nf.idEstadoNomina = (byte)idsEstadoNomina.ValidadoGestoria;

            nf.tblEstadoNominaNNomina.Add(new tblEstadoNominaNNomina
            {
                idEstadoNomina = (byte)idsEstadoNomina.ValidadoGestoria,
                fecha = DateTimeOffset.UtcNow,
                idUsuario_valida = idUsuario
            });
        }

        await db.SaveChangesAsync();

        var comparacionNominaResult = await CompararNominaGestoria(idPersona, fechaDesde, fechaHasta);

        if (comparacionNominaResult is OkObjectResult okObjectResult)
        {
            return Ok(okObjectResult.Value);
        }
        else
        {
            return comparacionNominaResult;
        }
    }

    public async Task<ActionResult> EnviarNominaGestoria(DateTime fechaDesde, DateTime fechaHasta, int idNomina, bool cambiarEstado, bool isPagaMensual, DbTransaction transaction)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());

        string idPersona = db.tblNomina.FirstOrDefault(n => n.idNomina == idNomina)?.idPersona.ToString() ?? "NULL";

        var connection = db.Database.GetDbConnection();
        var gestionNomina_persona = (
            await connection.QueryAsync<GestionNomina_persona>(
                "EXEC [MyReporting].[EF_gestionNominas_personas] @idUsuario, @fechaDesde, @fechaHasta, @forzarCalculado, @idListPersona, @isPagaMensual",
                new { idUsuario, fechaDesde, fechaHasta, forzarCalculado = false, idListPersona = idPersona, isPagaMensual },
                transaction
                )
            )
            .FirstOrDefault(gnp => gnp.idNomina == idNomina);

        if (gestionNomina_persona == null)
        {
            return BadRequest();
        }

        var rangoFechasDatos = GetRangoFechasDatos(fechaDesde, fechaHasta);

        try
        {
            List<tblConceptoNominaNNomina> tblConceptoNominaNNomina = new();

            if (!GuardarConceptosNDB(gestionNomina_persona, ref tblConceptoNominaNNomina))
            {
                throw new Exception("Error al guardar conceptos");
            }

            await HandleSalarioBrutoMensual(idNomina, gestionNomina_persona, tblConceptoNominaNNomina);

            await EnviarConceptos(idNomina, gestionNomina_persona, tblConceptoNominaNNomina, cambiarEstado);

            await HandleAbsentismos(idNomina, rangoFechasDatos, fechaDesde, fechaHasta, gestionNomina_persona);

            if (gestionNomina_persona.idTipoNomina == (short)idsTipoNomina.PagaMesual)
            {
                UpdateHistoricoSalarioBase(gestionNomina_persona);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        await db.SaveChangesAsync();

        return Ok(true);
    }

    public async Task<ActionResult> ImportarNominaGestoria(DateTime fechaDesde, DateTime fechaHasta, int idPersona, DbTransaction transaction)
    {
        var persona = db.tblPersona.FirstOrDefault(p => p.idPersona == idPersona);

        if (persona == null)
        {
            return BadRequest("Persona no existente");
        }

        var tblLavanderia = db.tblLavanderia.Where(l => l.idPais == (int)idsPais.España).ToList();

        var pays = await GetAllPays(fechaDesde.Year, fechaDesde.Month, A3innuvaUtils.GetCompanyCode(tblLavanderia, persona));

        var rangoFechasDatos = GetRangoFechasDatos(fechaDesde, fechaHasta);

        lastIdConceptoNomina = db.tblConceptoNomina.Max(cn => cn.idConceptoNomina);

        try
        {
            var conflictoNominaContrato = await HandleImportarDatosNominaGestoria(idPersona, fechaDesde, fechaHasta, pays, rangoFechasDatos);

            await db.SaveChangesAsync();

            return Ok(conflictoNominaContrato);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Devuelve el importe de un concepto de nómina
    /// </summary>
    /// <param name="tblConceptoNominaNNomina"></param>
    /// <param name="idConceptoNomina"></param>
    /// <returns>Importe de un concepto de nómina</returns>
    private static decimal? GetConceptoNominaNNomina(ICollection<tblConceptoNominaNNomina> tblConceptoNominaNNomina, short idConceptoNomina)
    {
        return tblConceptoNominaNNomina.FirstOrDefault(cnnn => cnnn.idConceptoNomina == idConceptoNomina)?.importe;
    }

    /// <summary>
    /// Guarda los conceptos de nómina en la base de datos
    /// </summary>
    /// <param name="gestionNominas_personas"></param>
    /// <returns>Booleano que define si ha ido bien o no</returns>
    public bool GuardarConceptosNDB(GestionNomina_persona gestionNomina_persona, ref List<tblConceptoNominaNNomina> tblConceptoNominaNNomina)
    {
        int idUsuario = this.idUsuario ?? int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        Dictionary<string, string> map_fieldCantidad_isCantidadRequired = new()
        {
            { "impHorasExtra", "horasExtra" },
            { "plusFestivoTrab", "festivosTrab" },
            { "plusNocturnidad", "horasNocturnas" },
        };

        foreach (var cv in map_idConceptoNomina)
        {
            var conceptField = cv.Key;
            var idConcepto = cv.Value;

            var concepto = db.tblConceptoNominaNNomina
                .Where(cnnn => cnnn.idNomina == gestionNomina_persona.idNomina && cnnn.idConceptoNomina == idConcepto)
                .FirstOrDefault();

            var property = gestionNomina_persona.GetType().GetProperty(conceptField);

            if (property != null)
            {
                var cantidad_concepto = (decimal?)property.GetValue(gestionNomina_persona);

                short idConceptoNomina;
                map_idConceptoNomina.TryGetValue(conceptField, out idConceptoNomina);

                decimal? cantidad = null;
                decimal? precioUnitario = null;
                var observaciones = concepto?.observaciones ?? "Generado automáticamente desde EnviarNominaGestoria";

                if (map_fieldCantidad_isCantidadRequired.ContainsKey(conceptField))
                {
                    var field_isCantidadRequired = map_fieldCantidad_isCantidadRequired[conceptField];
                    var property_isCantidadRequired = gestionNomina_persona.GetType().GetProperty(field_isCantidadRequired);
                    var cantidad_concepto_isCantidadRequired = (decimal?)property_isCantidadRequired.GetValue(gestionNomina_persona);

                    cantidad = cantidad_concepto_isCantidadRequired ?? 0;
                    precioUnitario = cantidad_concepto_isCantidadRequired > 0 ? (cantidad_concepto / cantidad_concepto_isCantidadRequired) : 0;
                }

                if (cantidad_concepto != null && (concepto == null || concepto.importe != cantidad_concepto))
                {
                    CustomConceptoNominaNNomina newConcepto = new()
                    {
                        idPersona = gestionNomina_persona.idPersona,
                        fechaInicioNomina = gestionNomina_persona.fechaInicioNomina,
                        fechaFinNomina = gestionNomina_persona.fechaFinNomina,
                        idConceptoNomina = idConceptoNomina,
                        cantidad = cantidad,
                        precioUnitario = precioUnitario,
                        importe = cantidad_concepto,
                        observaciones = observaciones,
                    };

                    if (!cnnnc.IU_tblConceptoNNomina(idUsuario, newConcepto, gestionNomina_persona.idNomina))
                    {
                        return false;
                    }
                }

                tblConceptoNominaNNomina.Add(new()
                {
                    idNomina = (int)gestionNomina_persona.idNomina,
                    idConceptoNomina = idConceptoNomina,
                    cantidad = cantidad,
                    precioUnitario = precioUnitario,
                    observaciones = observaciones,
                    idUsuario_validacion = idUsuario,
                    fecha_validacion = DateTimeOffset.UtcNow,
                    fecha = gestionNomina_persona.fechaInicioNomina,
                    importe = cantidad_concepto,
                    idNominaNavigation = db.tblNomina.FirstOrDefault(n => n.idNomina == gestionNomina_persona.idNomina),
                    idConceptoNominaNavigation = db.tblConceptoNomina.FirstOrDefault(cn => cn.idConceptoNomina == idConceptoNomina)
                });
            }
        }

        return true;
    }

    private List<dynamic> GetRangoFechasDatos(DateTime fechaDesde, DateTime fechaHasta)
    {
        var fechasInicioDatosLavanderia = db.tblCalendarioLavanderia
            .Where(cl => cl.idCalendario_Estado == (byte)idsCalendario_Estado.CierreNomina && cl.fecha < fechaDesde)
            .GroupBy(cl => cl.idLavanderia)
            .Select(g => new
            {
                idLavanderia = g.Key,
                fecha = (((fechaDesde.Year - g.Max(cl => cl.fecha).Year) * 12) + fechaDesde.Month - g.Max(cl => cl.fecha).Month) <= 1 ? g.Max(cl => cl.fecha).AddDays(1) : fechaDesde
            })
            .ToList();

        var fechasFinDatosLavanderia = db.tblCalendarioLavanderia
            .Where(cl => cl.idCalendario_Estado == (byte)idsCalendario_Estado.CierreNomina && cl.fecha >= fechaDesde && cl.fecha <= fechaHasta)
            .GroupBy(cl => cl.idLavanderia)
            .Select(g => new
            {
                idLavanderia = g.Key,
                fecha = (fechaHasta.Year == g.Max(cl => cl.fecha).Year && fechaHasta.Month == g.Max(cl => cl.fecha).Month) ? g.Max(cl => cl.fecha) : fechaHasta
            })
            .ToList();

        var rangoFechasCentroTrabajo = db.tblCentroTrabajo
            .Select(ct => new
            {
                idLavanderia = (int?)null,
                ct.idCentroTrabajo,
                fechaInicioDatos = fechaDesde,
                fechaFinDatos = fechaHasta
            })
            .Cast<dynamic>()
            .ToList();

        var rangoFechasDatos = fechasInicioDatosLavanderia
            .GroupJoin(
                fechasFinDatosLavanderia,
                inicio => inicio.idLavanderia,
                fin => fin.idLavanderia,
                (inicio, fin) => new { inicio, fin }
            )
            .SelectMany(
                x => x.fin.DefaultIfEmpty(),
                (inicio, fin) => new
                {
                    inicio.inicio.idLavanderia,
                    idCentroTrabajo = (int?)null,
                    fechaInicioDatos = inicio.inicio.fecha,
                    fechaFinDatos = fin != null ? (DateTime?)fin.fecha : null
                }
            )
            .Cast<dynamic>()
            .ToList();

        rangoFechasDatos.AddRange(rangoFechasCentroTrabajo);

        return rangoFechasDatos;
    }

    private async Task HandleAbsentismos(int idNomina, List<dynamic> rangoFechasDatos, DateTime fechaDesde, DateTime fechaHasta, GestionNomina_persona gestionNomina_persona)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());

        var employeeNNomina = GetEmployeeNNomina(idNomina);

        if (employeeNNomina?.companyCode == null)
        {
            throw new A3innuvaException("Persona sin companyCode");
        }

        if (employeeNNomina?.employeeCode == null)
        {
            throw new A3innuvaException("Persona sin codigoGestoria");
        }

        var nomina = db.tblNomina.Include(n => n.idPersonaNavigation).FirstOrDefault(n => n.idNomina == idNomina);

        if (nomina == null)
        {
            throw new Exception($"No se ha encontrado la nómina ({idNomina})");
        }

        var rangoFechasSel = rangoFechasDatos
            .FirstOrDefault(rfd =>
                rfd.idLavanderia == nomina.idPersonaNavigation.idLavanderia
                || rfd.idCentroTrabajo == nomina.idPersonaNavigation.idCentroTrabajo
            );

        var fechaBaja = db.tblNomina.FirstOrDefault(n => n.idNomina == idNomina)?.fechaBaja;

        if (rangoFechasSel == null || (fechaBaja == null && rangoFechasSel?.fechaFinDatos == null))
        {
            throw new Exception($"HandleAbsentismos: No se ha encontrado el rango de fechas de datos para lavanderia ({nomina.idPersonaNavigation.idLavanderia}) y centro trabajo ({nomina.idPersonaNavigation.idCentroTrabajo})");
        }

        // Si es un finiquito la fecha de fin de datos es la misma que la de su fecha de baja
        var fechaFinDatos = fechaBaja ?? rangoFechasSel!.fechaFinDatos;

        var filter = $"startDate le {fechaFinDatos.ToString("yyyy-MM-dd")} and endDate ge {rangoFechasSel!.fechaInicioDatos.ToString("yyyy-MM-dd")}";
        List<AbsenteeismsUnjustifiedAbsenteeisms_Get_AbsenteeismsUnjustifieds> absenteeismsUnjustifieds = await GetAllAbsenteeismsUnjustifieds(employeeNNomina, filter);

        var getAbsenteeisms = await a3innuva.Absenteeisms.Get_Absenteeisms((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, fechaDesde.Year);
        var absenteeisms = await A3innuvaUtils.DeserializeResponseAsync<List<Absenteeisms_Get_Absenteeisms>>(getAbsenteeisms);

        // Filtrar absentismos injustificados ya que se van a pisar
        absenteeisms.RemoveAll(a => absenteeismsUnjustifieds.Any(au => au.absenteeismID == a.absenteeismID));

        var getTemporaryDisabilities = await a3innuva.TemporaryDisabilities.Get_TemporaryDisabilities((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, fechaDesde.Year);
        var temporaryDisabilities = await A3innuvaUtils.DeserializeResponseAsync<List<TemporaryDisabilities_Get_TemporaryDisabilities>>(getTemporaryDisabilities);

        var absentismos = db.tblCalendarioPersonal
            .Where(cp =>
                cp.idPersona == nomina.idPersona
                && cp.idCalendario_Estado == (byte)idsCalendario_Estado.Absentismo
            )
            .ToList()
            .Where(cp =>
                cp.fecha.Date >= rangoFechasSel.fechaInicioDatos.Date
                && cp.fecha.Date <= fechaFinDatos.Date
                && !absenteeisms.Any(a => a.startDate.Date <= cp.fecha.Date && (a.endDate == null || ((DateTime)a.endDate).Date >= cp.fecha.Date))
                && !temporaryDisabilities.Any(td => td.startdate.Date <= cp.fecha.Date && (td.enddate == null || ((DateTime)td.enddate).Date >= cp.fecha.Date))
            )
            .Select(cp => cp.fecha.Date)
            .OrderBy(date => date)
            .ToList();

        var rangosConsecutivos = GetRangosConsecutivos();

        await PisarAbsentismosInjustificados();

        await HandlePlusAsistenciaMesActual();

        List<(DateTime inicio, DateTime fin)> GetRangosConsecutivos()
        {
            var result = new List<(DateTime inicio, DateTime fin)>();
            if (absentismos.Any())
            {
                var inicioRango = absentismos.First();
                var finRango = absentismos.First();

                // Saltamos el primero porque ya está en inicioRango y finRango
                foreach (var fecha in absentismos.Skip(1))
                {
                    if (fecha == finRango.AddDays(1))
                    {
                        finRango = fecha;
                    }
                    else
                    {
                        result.Add((inicioRango, finRango));
                        inicioRango = fecha;
                        finRango = fecha;
                    }
                }

                result.Add((inicioRango, finRango));
            }
            else
            {
                return result;
            }

            for (int i = 0; i < result.Count; i++)
            {
                var (inicio, fin) = result[i];

                if (inicio.Date >= fechaDesde.Date)
                {
                    continue;
                }

                if (fin >= fechaDesde)
                {
                    var solapa = absenteeisms.Any(a => a.startDate.Date <= fin.Date && (a.endDate == null || ((DateTime)a.endDate).Date >= fechaDesde.Date))
                        || temporaryDisabilities.Any(td => td.startdate.Date <= fin.Date && (td.enddate == null || ((DateTime)td.enddate).Date >= fechaDesde.Date));

                    if (!solapa)
                    {
                        result.Add((fechaDesde, fin));
                    }

                    fin = new DateTime(inicio.Year, inicio.Month, DateTime.DaysInMonth(inicio.Year, inicio.Month));
                }

                var diasRango = (fin - inicio).Days;

                var nuevoInicio = fechaDesde;
                var nuevoFin = nuevoInicio.AddDays(diasRango);

                while (nuevoFin.Date <= fechaFinDatos.Date)
                {
                    bool solapa = result.Any(r => r.inicio.Date <= nuevoFin.Date && r.fin.Date >= nuevoInicio.Date)
                        || absenteeisms.Any(a => a.startDate.Date <= nuevoFin.Date && (a.endDate == null || ((DateTime)a.endDate).Date >= nuevoInicio.Date))
                        || temporaryDisabilities.Any(td => td.startdate.Date <= nuevoFin.Date && (td.enddate == null || ((DateTime)td.enddate).Date >= nuevoInicio.Date));

                    if (!solapa)
                    {
                        result[i] = (nuevoInicio, nuevoFin);

                        break;
                    }

                    nuevoInicio = nuevoInicio.AddDays(1);
                    nuevoFin = nuevoInicio.AddDays(diasRango);
                }
            }

            return result;
        }

        async Task PisarAbsentismosInjustificados()
        {
            foreach (var au in absenteeismsUnjustifieds)
            {
                await a3innuva.AbsenteeismsUnjustifiedAbsenteeisms.Delete_AbsenteeismsUnjustifieds((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, au.absenteeismID);
            }

            var bodys = rangosConsecutivos.Select(r => new AbsenteeismsUnjustifiedAbsenteeisms_Post_AbsenteeismsUnjustifieds
            {
                startDate = r.inicio.Date,
                endDate = r.fin.Date,
                indLessThanOneDay = false,
                durationHours = 0,
                durationMinuts = 0,
                indDiscountRestsWeekly = false,
                indDiscountNaturalDays = true,
                lastUpdate = DateTime.Now,
            });

            foreach (var body in bodys)
            {
                await a3innuva.AbsenteeismsUnjustifiedAbsenteeisms.Post_AbsenteeismsUnjustifieds((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, body);
            }
        }

        async Task HandlePlusAsistenciaMesActual()
        {
            var absentismosMesActual = db.tblCalendarioPersonal
                .Where(cp =>
                    cp.idPersona == nomina.idPersona
                    && cp.idCalendario_Estado == (byte)idsCalendario_Estado.Absentismo
                    && cp.fecha.Date >= fechaDesde.Date
                )
                .ToList()
                .Where(cp =>
                    cp.fecha.Date <= fechaFinDatos.Date
                    && !absenteeisms.Any(a => a.startDate <= cp.fecha && (a.endDate == null || ((DateTime)a.endDate) >= cp.fecha))
                    && !temporaryDisabilities.Any(td => td.startdate <= cp.fecha && (td.enddate == null || ((DateTime)td.enddate) >= cp.fecha))
                );

            var cv = db.tblConceptoNomina.FirstOrDefault(cn => cn.idConceptoNomina == (short)idsConceptoNomina.PlusAsistencia);

            List<VariableConcepts_Get_Variableconcepts> variableConceptsNPersona = await GetAllVariableConcepts(employeeNNomina);

            var plusAsistenciaA3 = variableConceptsNPersona.FirstOrDefault(vc => vc.conceptCode == cv.codigo);

            var plusAsistencia = nomina.tblConceptoNominaNNomina.FirstOrDefault(cnnn => cnnn.idConceptoNomina == (short)idsConceptoNomina.PlusAsistencia);

            if (!absentismosMesActual.Any())
            {
                if (plusAsistencia != null)
                {
                    var importe = (db.tblDatosSalariales.FirstOrDefault(ds => ds.idPersona == nomina.idPersona)?.plusAsistencia ?? 0) * gestionNomina_persona.percMes;

                    plusAsistencia.importe = importe;
                    plusAsistencia.precioUnitario = importe;
                }

                if (plusAsistenciaA3 == null)
                {
                    return;
                }

                await a3innuva.VariableConcepts.Delete_Variableconcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, plusAsistenciaA3.variableConceptID);

                return;
            }

            if (plusAsistencia != null)
            {
                plusAsistencia.importe = 0;
                plusAsistencia.precioUnitario = 0;
            }

            VariableConcepts_Get_Variableconcepts plusAsistenciaDB = new()
            {
                conceptCode = cv.codigo,
                startDate = employeeNNomina.fechaDesde,
                endDate = employeeNNomina.fechaHasta,
                collectionDate = employeeNNomina.fechaDesde,
                payTypeId = 1,
                amount = 0,
                units = 1,
                prorateInPaySections = false,
                enrolmentDropProrate = false
            };

            await HandleEnviarVariableConcept(cv, plusAsistenciaDB, plusAsistenciaA3, employeeNNomina);
        }
    }

    /// <summary>
    /// Obtiene todos los absentismos injustificados (de a3innuva)
    /// </summary>
    /// <param name="employeeNNomina"></param>
    /// <param name="filter"></param>
    /// <returns>Lista de absentismos injustificados</returns>
    private async Task<List<AbsenteeismsUnjustifiedAbsenteeisms_Get_AbsenteeismsUnjustifieds>> GetAllAbsenteeismsUnjustifieds(EmployeeNNomina employeeNNomina, string? filter = null)
    {
        Func<int, Task<HttpResponseMessage>> getAbsenteeismsUnjustifieds = async (pageNumber) =>
        await a3innuva.AbsenteeismsUnjustifiedAbsenteeisms.Get_AbsenteeismsUnjustifieds((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, pageNumber++, 50, filter);

        return await A3innuvaUtils.HandlePagination<AbsenteeismsUnjustifiedAbsenteeisms_Get_AbsenteeismsUnjustifieds>(getAbsenteeismsUnjustifieds);
    }

    /// <summary>
    /// Maneja la actualización del salario bruto mensual
    /// </summary>
    /// <param name="idNomina"></param>
    /// <param name="gestionNomina_persona"></param>
    private async Task HandleSalarioBrutoMensual(int idNomina, GestionNomina_persona gestionNomina_persona, List<tblConceptoNominaNNomina> tblConceptoNominaNNomina)
    {
        List<string> map_conceptosDatosSalariales = new()
        {
            "salarioBase",
            "plusAsistencia",
            "incentivo",
            "plusPeligrosidad",
            "plusResponsabilidad",
            "acuerdoNC",
            "salarioEspecie",
        };

        List<string> map_conceptosCalculados = new()
        {
            "pagaExtra1",
            "pagaExtra2",
            "segAccidenteConvenio",
        };

        List<string> map_conceptosTotales = new()
        {
            "totalPlusAntiguedad",
            "totalPlusProductividad",
        };

        List<string> map_conceptosDevengo = new()
        {
            "plusAntiguedad",
            "plusAsistencia",
            "plusProductividad",
        };

        List<short> idsConceptoNomina_conceptosExtra = new()
        {
            (short)idsConceptoNomina.PlusActividad,
            (short)idsConceptoNomina.FestivosTrabajados,
            (short)idsConceptoNomina.PlusNocturnidad,
            (short)idsConceptoNomina.HorasExtrasResto,
        };

        if (!db.tblDatosSalariales.Any(ds => ds.idPersona == gestionNomina_persona.idPersona && ds.salarioBrutoMensual != null))
        {
            return;
        }

        var employeeNNomina = GetEmployeeNNomina(idNomina);

        if (employeeNNomina?.companyCode == null)
        {
            throw new A3innuvaException("Persona sin companyCode");
        }

        if (employeeNNomina?.employeeCode == null)
        {
            throw new A3innuvaException("Persona sin codigoGestoria");
        }

        decimal totalDevengadoPactado = 0;
        decimal totalDevengado = 0;

        if (gestionNomina_persona == null || (gestionNomina_persona?.isMesCompletoBaja ?? false))
        {
            return;
        }

        var idPersona = gestionNomina_persona!.idPersona;
        var percMes = gestionNomina_persona.percMes ?? 0;
        var brutoMensual = db.tblDatosSalariales.FirstOrDefault(ds => ds.idPersona == idPersona)?.salarioBrutoMensual ?? 0;

        var datosSalariales_persona = db.tblDatosSalariales.FirstOrDefault(ds => ds.idPersona == idPersona);
        if (datosSalariales_persona == null)
        {
            return;
        }

        foreach (var cds in map_conceptosDatosSalariales)
        {
            var property = datosSalariales_persona.GetType().GetProperty(cds);
            var cantidad_concepto = (property != null ? (decimal?)property.GetValue(datosSalariales_persona) * percMes : null) ?? 0;

            totalDevengadoPactado += cantidad_concepto;

            if (!map_conceptosDevengo.Contains(cds))
            {
                totalDevengado += cantidad_concepto;
            }
        }

        foreach (var cds in map_conceptosCalculados)
        {
            var property = gestionNomina_persona.GetType().GetProperty(cds);
            var cantidad_concepto = (property != null ? (decimal?)property.GetValue(gestionNomina_persona) : null) ?? 0;

            totalDevengadoPactado += cantidad_concepto;
            totalDevengado += cantidad_concepto;
        }

        foreach (var ct in map_conceptosTotales)
        {
            var property = gestionNomina_persona.GetType().GetProperty(ct);
            var cantidad_concepto = (property != null ? (decimal?)property.GetValue(gestionNomina_persona) * percMes : null) ?? 0;

            totalDevengadoPactado += cantidad_concepto;
        }

        foreach (var cd in map_conceptosDevengo)
        {
            var property = gestionNomina_persona.GetType().GetProperty(cd);
            var cantidad_concepto = (property != null ? (decimal?)property.GetValue(gestionNomina_persona) : null) ?? 0;

            totalDevengado += cantidad_concepto;
        }

        totalDevengado += (
                tblConceptoNominaNNomina
                    .Where(cnnn =>
                        cnnn.idNomina == idNomina
                        && idsConceptoNomina_conceptosExtra.Contains(cnnn.idConceptoNomina)
                    )
                    .ToList()
                    .Sum(cnnn => cnnn.importe) ?? 0
            );

        VariableConcepts_Post_Variableconcepts plusAbsorbible = new()
        {
            conceptCode = ((int)conceptsCodes.PlusAbsorbible).ToString(),
            startDate = employeeNNomina.fechaDesde,
            endDate = employeeNNomina.fechaHasta,
            collectionDate = employeeNNomina.fechaDesde,
            payTypeId = 1,
            amount = brutoMensual * percMes + (totalDevengado - totalDevengadoPactado),
            units = 1,
            prorateInPaySections = false,
            enrolmentDropProrate = false
        };

        var response_plusAbsorbibleA3 = await a3innuva.VariableConcepts.Get_Variableconcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, ((int)conceptsCodes.PlusAbsorbible).ToString(), gestionNomina_persona.fechaInicioNomina, 1);

        var plusAbsorbibleA3 = (await A3innuvaUtils.IsEmptyResponseAsync(response_plusAbsorbibleA3))
            ? null
            : (await A3innuvaUtils.DeserializeResponseAsync<List<VariableConcepts_Get_Variableconcepts>>(response_plusAbsorbibleA3)).FirstOrDefault();

        if (plusAbsorbibleA3 == null && plusAbsorbible.amount > 0 && plusAbsorbible.amount != brutoMensual)
        {
            await a3innuva.VariableConcepts.Post_Variableconcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, plusAbsorbible);
            return;
        }

        if (plusAbsorbibleA3 != null && plusAbsorbible.amount != plusAbsorbibleA3.amount)
        {
            await a3innuva.VariableConcepts.Delete_Variableconcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, plusAbsorbibleA3.variableConceptID);
            if (plusAbsorbible.amount > 0 && plusAbsorbible.amount != brutoMensual)
            {
                await a3innuva.VariableConcepts.Post_Variableconcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, plusAbsorbible);
            }
        }
    }

    /// <summary>
    /// Envía los conceptos de la nómina a a3innuva
    /// </summary>
    /// <param name="idNomina"></param>
    /// <param name="gestionNomina_persona"></param>
    /// <param name="tblConceptoNominaNNomina"></param>
    private async Task EnviarConceptos(int idNomina, GestionNomina_persona gestionNomina_persona, List<tblConceptoNominaNNomina> tblConceptoNominaNNomina, bool cambiarEstado = true)
    {
        Dictionary<string, short> map_idConceptoNomina_conceptosTotales = new()
        {
            { "totalPlusAntiguedad", 3 },
            { "totalPlusAsistencia", 4 },
        };
        List<short> idsConceptoNomina_conceptosTotales = new(map_idConceptoNomina_conceptosTotales.Values);

        List<tblConceptoNomina> conceptosVariables = db.tblConceptoNomina.Where(cn => cn.isConceptoVariable && cn.isModificable).ToList();
        List<tblConceptoNomina> conceptosFijos = db.tblConceptoNomina.Where(cn => !cn.isConceptoVariable && cn.isModificable || idsConceptoNomina_conceptosTotales.Contains(cn.idConceptoNomina)).ToList();

        var employeeNNomina = GetEmployeeNNomina(idNomina);

        if (employeeNNomina?.companyCode == null)
        {
            throw new A3innuvaException("Persona sin companyCode");
        }

        if (employeeNNomina?.employeeCode == null)
        {
            throw new A3innuvaException("Persona sin codigoGestoria");
        }

        List<EmployeesConcepts_Get_Concepts> conceptosFijosNPersona = new();

        if (gestionNomina_persona == null || (gestionNomina_persona?.isMesCompletoBaja ?? false))
        {
            // Si la persona está todo el mes de baja solo se envian anticipos

            await HandleVariableConcepts(idNomina, conceptosVariables.Where(cv => cv.idConceptoNomina == (short)idsConceptoNomina.Anticipo).ToList(), employeeNNomina, tblConceptoNominaNNomina);

            CambiarEstadoNomina(idNomina);

            return;
        }

        foreach (var ct in map_idConceptoNomina_conceptosTotales)
        {
            var conceptField = ct.Key;
            var idConcepto = ct.Value;

            var concepto = db.tblConceptoNomina.Where(cnnn => cnnn.idConceptoNomina == idConcepto).FirstOrDefault();

            if (gestionNomina_persona == null || concepto == null)
            {
                continue;
            }

            var property = gestionNomina_persona.GetType().GetProperty(conceptField);
            var cantidad_concepto = property != null ? (decimal?)property.GetValue(gestionNomina_persona) : null;

            if (cantidad_concepto != null)
            {
                conceptosFijosNPersona.Add(new EmployeesConcepts_Get_Concepts
                {
                    conceptCode = int.Parse(concepto.codigo),
                    amount = (decimal)cantidad_concepto,
                });
            }
        }

        await HandleEmployeeConcepts(idNomina, conceptosFijos, employeeNNomina, conceptosFijosNPersona, tblConceptoNominaNNomina);

        await HandleVariableConcepts(idNomina, conceptosVariables, employeeNNomina, tblConceptoNominaNNomina);

        CambiarEstadoNomina(idNomina);

        void CambiarEstadoNomina(int idNomina)
        {
            if (!cambiarEstado) return;
            var nomina = db.tblNomina.FirstOrDefault(n => n.idNomina == idNomina);
            if (nomina != null)
            {
                nomina.idEstadoNomina = (byte)idsEstadoNomina.ValidadoRRHH;

                int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());

                db.tblEstadoNominaNNomina.Add(new tblEstadoNominaNNomina
                {
                    idNomina = nomina.idNomina,
                    idEstadoNomina = nomina.idEstadoNomina,
                    fecha = DateTimeOffset.UtcNow,
                    idUsuario_valida = idUsuario
                });
            }
        }
    }

    /// <summary>
    /// Actualixa el histórico de salario base
    /// </summary>
    /// <param name="gestionNomina_persona"></param>
    private void UpdateHistoricoSalarioBase(GestionNomina_persona gestionNomina_persona)
    {
        //IU registro en el histórico de salario_Base
        var objDs = db.tblDatosSalariales.FirstOrDefault(x => x.idPersona.Equals(gestionNomina_persona.idPersona));
        if (objDs != null && objDs.salarioBase != null)
        {
            var objHist = db.tblDatosSalariales_historico_salarioBase.FirstOrDefault(x => x.idPersona.Equals(gestionNomina_persona.idPersona) && x.fecha.Year.Equals(gestionNomina_persona.fechaInicioNomina.Year) && x.fecha.Month.Equals(gestionNomina_persona.fechaInicioNomina.Month));
            if (objHist == null) //Insert
            {
                db.tblDatosSalariales_historico_salarioBase.Add(new tblDatosSalariales_historico_salarioBase()
                {
                    idPersona = gestionNomina_persona.idPersona,
                    fecha = new DateTime(gestionNomina_persona.fechaInicioNomina.Year, gestionNomina_persona.fechaInicioNomina.Month, 1),
                    salarioBase = (decimal)objDs.salarioBase
                });
            }
            else
            {
                objHist.salarioBase = (decimal)objDs.salarioBase;
            }
        }
    }

    /// <summary>
    /// Obtiene la información de un empleado y su nómina
    /// </summary>
    /// <param name="idNomina"></param>
    /// <returns>Empleado con fechaDesde y fechaHasta de su nómina</returns>
    private EmployeeNNomina? GetEmployeeNNomina(int idNomina)
    {
        var tblNomina = db.tblNomina
                .Include(n => n.idPersonaNavigation)
                .Where(n => n.idNomina == idNomina)
                .ToList();

        var tblLavanderia = db.tblLavanderia.Where(l => l.idPais == (int)idsPais.España).ToList();

        var employeeNNomina = tblNomina
                .Select(n => new EmployeeNNomina()
                {
                    idPersona = n.idPersona,
                    fechaDesde = n.fechaDesde,
                    fechaHasta = n.fechaHasta,
                    employeeCode = n.idPersonaNavigation.codigoGestoria,
                    companyCode = A3innuvaUtils.GetCompanyCode(tblLavanderia, new tblPersona { idLavanderia = n.idPersonaNavigation.idLavanderia, idCentroTrabajo = n.idPersonaNavigation.idCentroTrabajo })
                }).FirstOrDefault();

        return employeeNNomina;
    }

    /// <summary>
    /// Obtiene la información de un empleado por idPersona, que todavía no tiene nómina
    /// </summary>
    /// <param name="idPersona"></param>
    /// <param name="fechaDesde"></param>
    /// <param name="fechaHasta"></param>
    /// <returns>Empleado con fechaDesde y fechaHasta para su próxima nómina</returns>
    public EmployeeNNomina? GetEmployeeNNominaSinNomina(int idPersona, DateTime fechaDesde, DateTime fechaHasta)
    {
        var tblPersona = db.tblPersona
                .Where(p => p.idPersona == idPersona)
                .ToList();

        var tblLavanderia = db.tblLavanderia.Where(l => l.idPais == (int)idsPais.España).ToList();

        var employeeNNomina = tblPersona
                .Select(p => new EmployeeNNomina()
                {
                    idPersona = p.idPersona,
                    fechaDesde = fechaDesde,
                    fechaHasta = fechaHasta,
                    employeeCode = p.codigoGestoria,
                    companyCode = A3innuvaUtils.GetCompanyCode(tblLavanderia, p)
                }).FirstOrDefault();

        return employeeNNomina;
    }

    /// <summary>
    /// Obtiene todos los conceptos fijos (de a3innuva) de un empleado en un periodo de tiempo
    /// </summary>
    /// <param name="companyCode"></param>
    /// <param name="employeeCode"></param>
    /// <returns>Lista de conceptos fijos</returns>
    public async Task<List<EmployeesConcepts_Get_Concepts>> GetAllEmployeeConcepts(int companyCode, string employeeCode)
    {
        Func<int, Task<HttpResponseMessage>> getEmployeeConcepts = async (pageNumber) => await a3innuva.EmployeesConcepts.Get_Concepts(companyCode, employeeCode, pageNumber++);

        return await A3innuvaUtils.HandlePagination<EmployeesConcepts_Get_Concepts>(getEmployeeConcepts);
    }

    /// <summary>
    /// Obtiene todos los conceptos variables (de a3innuva) de un empleado en un periodo de tiempo
    /// </summary>
    /// <param name="employeeNContrato"></param>
    /// <returns>Lista de conceptos variables</returns>
    private async Task<List<VariableConcepts_Get_Variableconcepts>> GetAllVariableConcepts(EmployeeNNomina employeeNContrato)
    {
        List<VariableConcepts_Get_Variableconcepts> variableConcepts = new();

        bool hasMore = true;
        int pageNumber = 1;

        while (hasMore)
        {
            var response = await a3innuva.VariableConcepts.Get_Variableconcepts((int)employeeNContrato.companyCode, employeeNContrato.employeeCode, employeeNContrato.fechaDesde, employeeNContrato.fechaHasta, pageNumber++); // El ++ suma uno después de obtener el resultado

            var resultString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(resultString) || resultString == "[]" || resultString == "null")
            {
                hasMore = false;
                break;
            }

            var resultParsed = await A3innuvaUtils.DeserializeResponseAsync<List<VariableConcepts_Get_Variableconcepts>>(response);
            variableConcepts.AddRange(resultParsed);

            if (resultParsed.Count < 50)
                break;
        }

        return variableConcepts;
    }

    /// <summary>
    /// Maneja las diferencias entre los conceptos de la persona de la base de datos y los de a3innuva
    /// </summary>
    /// <param name="idNomina"></param>
    /// <param name="conceptosFijos"></param>
    /// <param name="employeeNNomina"></param>
    /// <param name="conceptosFijosNPersona"></param>
    /// <param name="tblConceptoNominaNNomina"></param>
    private async Task HandleEmployeeConcepts(int idNomina, List<tblConceptoNomina> conceptosFijos, EmployeeNNomina employeeNNomina, List<EmployeesConcepts_Get_Concepts> conceptosFijosNPersona, List<tblConceptoNominaNNomina> tblConceptoNominaNNomina)
    {
        conceptosFijosNPersona.AddRange(
                tblConceptoNominaNNomina
                    .Where(cnn =>
                        !conceptosFijosNPersona.Select(cjnp => cjnp.conceptCode.ToString()).Contains(cnn.idConceptoNominaNavigation.codigo)
                        && cnn.idNomina == idNomina
                        && !cnn.idConceptoNominaNavigation.isConceptoVariable
                        && cnn.idConceptoNominaNavigation.isModificable
                    )
                    .Select(cnn => new EmployeesConcepts_Get_Concepts
                    {
                        conceptCode = int.Parse(cnn.idConceptoNominaNavigation.codigo),
                        amount = cnn.importe ?? 0
                    })
                    .ToList()
            );

        List<EmployeesConcepts_Get_Concepts> employeeConceptsNPersona = await GetAllEmployeeConcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode);

        foreach (var cv in conceptosFijos)
        {
            var conceptoFijoNPersona = conceptosFijosNPersona.FirstOrDefault(cvn => cvn.conceptCode.ToString() == cv.codigo);
            var employeeConceptNPersona = employeeConceptsNPersona.FirstOrDefault(vc => vc.conceptCode.ToString() == cv.codigo);

            if (
                conceptoFijoNPersona != null
                && employeeConceptNPersona != null
                && conceptoFijoNPersona.amount != employeeConceptNPersona.amount
                && conceptoFijoNPersona.amount > 0
            )
            {
                await a3innuva.EmployeesConcepts.Put_Amount((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, cv.codigo, conceptoFijoNPersona.amount);
            }
        }
    }

    /// <summary>
    /// Maneja las diferencias entre los conceptos variables de la base de datos y los de a3innuva
    /// </summary>
    /// <param name="idNomina"></param>
    /// <param name="conceptosVariables"></param>
    /// <param name="employeeNNomina"></param>
    private async Task HandleVariableConcepts(int idNomina, List<tblConceptoNomina> conceptosVariables, EmployeeNNomina employeeNNomina, List<tblConceptoNominaNNomina> tblConceptoNominaNNomina)
    {
        List<VariableConcepts_Get_Variableconcepts> conceptosVariablesNPersona = tblConceptoNominaNNomina
                .Where(cnn =>
                    cnn.idNomina == idNomina
                    && cnn.idConceptoNominaNavigation.isConceptoVariable
                    && cnn.idConceptoNominaNavigation.isModificable
                )
                .Select(cnn => new VariableConcepts_Get_Variableconcepts
                {
                    conceptCode = cnn.idConceptoNominaNavigation.codigo,
                    startDate = cnn.idNominaNavigation.fechaDesde,
                    endDate = cnn.idNominaNavigation.fechaHasta,
                    amount = cnn.importe ?? 0,
                    units = cnn.cantidad ?? 1,
                })
                .ToList();

        List<VariableConcepts_Get_Variableconcepts> variableConceptsNPersona = await GetAllVariableConcepts(employeeNNomina);

        foreach (var cv in conceptosVariables)
        {
            var conceptoVariableNPersona = conceptosVariablesNPersona.FirstOrDefault(cvn => cvn.conceptCode == cv.codigo);
            var variableConceptNPersona = variableConceptsNPersona.FirstOrDefault(vc => vc.conceptCode == cv.codigo);

            await HandleEnviarVariableConcept(cv, conceptoVariableNPersona, variableConceptNPersona, employeeNNomina);
        }
    }

    private async Task HandleEnviarVariableConcept(tblConceptoNomina cv, VariableConcepts_Get_Variableconcepts conceptoVariableNDB, VariableConcepts_Get_Variableconcepts conceptoVariableNA3, EmployeeNNomina employeeNNomina)
    {
        if (
            conceptoVariableNDB != null
            && (cv.idConceptoNomina == map_idConceptoNomina["plusAsistencia"] ? conceptoVariableNDB.amount == 0 : conceptoVariableNDB.amount > 0)
        ) // Si existe el concepto variable en la base de datos y es mayor que cero, se crea o actualiza en a3innuva
        {
            VariableConcepts_Post_Variableconcepts newVariableConcept = new()
            {
                conceptCode = conceptoVariableNDB.conceptCode,
                startDate = employeeNNomina.fechaDesde,
                endDate = employeeNNomina.fechaHasta,
                collectionDate = employeeNNomina.fechaDesde,
                payTypeId = 1,
                amount = conceptoVariableNDB.amount,
                units = cv.idConceptoNomina == map_idConceptoNomina["plusAsistencia"] ? 0 : conceptoVariableNDB.units,
                prorateInPaySections = false,
                enrolmentDropProrate = false
            };

            if (conceptoVariableNA3 != null) // Si existe el concepto variable en la base de datos y en a3innuva, se actualiza
            {
                if (conceptoVariableNDB.amount != conceptoVariableNA3.amount || conceptoVariableNDB.units != conceptoVariableNA3.units)
                {
                    await a3innuva.VariableConcepts.Delete_Variableconcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, conceptoVariableNA3.variableConceptID);
                    await a3innuva.VariableConcepts.Post_Variableconcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, newVariableConcept);
                }
            }
            else // Si existe el concepto variable en la base de datos pero no en a3innuva, se crea
            {
                await a3innuva.VariableConcepts.Post_Variableconcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, newVariableConcept);
            }
        }
        else if (conceptoVariableNA3 != null) // Si no existe el concepto variable en la base de datos pero sí en a3innuva, se elimina
        {
            await a3innuva.VariableConcepts.Delete_Variableconcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, conceptoVariableNA3.variableConceptID);
        }
        // Si no existe el concepto variable en ambas listas, no se hace nada
    }

    /// <summary>
    /// Obtiene todos los conceptos de la nómina calculada de a3innuva y de la base de datos de las nóminas seleccionadas
    /// </summary>
    /// <param name="idPersona"></param>
    /// <returns>Conceptos de la nómina calculada de a3innuva y de la base de datos</returns>
    private async Task<object> GetConceptoNominaComparacionNPersona(int idPersona, DateTime fechaDesde, DateTime fechaHasta)
    {
        var tblNomina_prev = await db.tblNomina.Include(n => n.tblConceptoNominaNNomina_Gestoria).Where(n => n.idPersona == idPersona && n.fechaDesde.Date >= fechaDesde.Date && n.fechaHasta.Date <= fechaHasta.Date).ToListAsync();

        var numPagasMensuales = tblNomina_prev.Where(n => n.idTipoNomina == (byte)idsTipoNomina.PagaMesual).Count();

        // Si la persona solo tiene una paga mensual, no se necesita comprobar los contratos
        List<tblPersonaNTipoContrato> tblPersonaNTipoContrato = numPagasMensuales == 1 ? new() : GetTblPersonaNTipoContrato(idPersona, fechaDesde, fechaHasta);

        var tblNomina = (
            from n in tblNomina_prev
            join pntc in tblPersonaNTipoContrato on new { fechaDesde = n.fechaDesde.Date, fechaHasta = n.fechaHasta.Date } equals new { fechaDesde = pntc.fechaAltaContrato.Date, fechaHasta = ((DateTime)pntc.fechaBajaContrato).Date } into pntcGroup
            from pntc in pntcGroup.DefaultIfEmpty()
            select new
            {
                n.idNomina,
                n.idPersona,
                n.idEstadoNomina,
                n.idEstadoHistoricoAsientoNomina,
                n.fechaDesde,
                n.fechaHasta,
                n.tipoPaga,
                isContratoAsociado = numPagasMensuales == 1 || (n.idTipoNomina == (byte)idsTipoNomina.PagaMesual && pntc != null)
            }
        ).ToList();

        var idsNomina = tblNomina.Select(n => n.idNomina).ToList();

        var tblConceptoNominaNNomina = await db.tblConceptoNominaNNomina
            .Where(cnnn => idsNomina.Contains(cnnn.idNomina))
            .Select(cnnn => new
            {
                cnnn.idNomina,
                cnnn.idConceptoNomina,
                cnnn.importe
            })
            .ToListAsync();

        var tblConceptoNominaNNomina_Gestoria = await db.tblConceptoNominaNNomina_Gestoria
            .Where(cnnn => idsNomina.Contains(cnnn.idNomina))
            .Select(cnnn => new
            {
                cnnn.idNomina,
                cnnn.idConceptoNomina,
                cnnn.importe
            })
            .ToListAsync();

        var conceptosNominaPersona = (
                from n in tblNomina
                join cn in db.tblConceptoNomina on 1 equals 1
                join cnnn in tblConceptoNominaNNomina on new { n.idNomina, cn.idConceptoNomina } equals new { cnnn.idNomina, cnnn.idConceptoNomina } into cnnnGroup
                from cnnn in cnnnGroup.DefaultIfEmpty()
                join cnnng in tblConceptoNominaNNomina_Gestoria on new { n.idNomina, cn.idConceptoNomina } equals new { cnnng.idNomina, cnnng.idConceptoNomina } into cnnngGroup
                from cnnng in cnnngGroup.DefaultIfEmpty()
                where
                    cnnn?.idConceptoNomina != null
                select new
                {
                    n.idNomina,
                    n.idEstadoNomina,
                    n.idEstadoHistoricoAsientoNomina,
                    n.fechaDesde,
                    n.fechaHasta,
                    n.tipoPaga,
                    n.isContratoAsociado,
                    cn.idConceptoNomina,
                    cn.denominacion,
                    importe = cnnn?.importe ?? 0,
                    importeA3 = cnnng?.importe ?? cnnn?.importe ?? 0,
                    cn.isConceptoVariable,
                    cn.isDevengo
                }
            );


        var conceptoNominaComparacionNPersona = conceptosNominaPersona
             .GroupBy(cp => new { cp.idNomina, cp.idEstadoNomina, cp.idEstadoHistoricoAsientoNomina, cp.fechaDesde, cp.fechaHasta, cp.tipoPaga, cp.isContratoAsociado })
             .Select(group => new
             {
                 idPersona,
                 group.Key.idNomina,
                 group.Key.idEstadoNomina,
                 group.Key.idEstadoHistoricoAsientoNomina,
                 group.Key.fechaDesde,
                 group.Key.fechaHasta,
                 group.Key.tipoPaga,
                 group.Key.isContratoAsociado,
                 totalDevengado = group.Where(cp => cp.isDevengo).Sum(cp => cp.importe),
                 totalDeducido = group.Where(cp => !cp.isDevengo).Sum(cp => cp.importe),
                 conceptosNominaNNomina = group.Select(cp => new
                 {
                     cp.idConceptoNomina,
                     cp.denominacion,
                     cp.importeA3,
                     cp.importe,
                     cp.isConceptoVariable,
                     cp.isDevengo
                 }).ToList()
             })
             .ToList();

        return conceptoNominaComparacionNPersona;
    }

    /// <summary>
    /// Obtiene todos los conceptos de la nómina calculada (de a3innuva) de un empleado en un periodo de tiempo
    /// </summary>
    /// <param name="employeeNNomina"></param>
    /// <param name="payId"></param>
    /// <returns>Lista de conceptos de la nómina</returns>
    public async Task<List<PaysConcepts_Get_Concepts>> GetAllPayConcepts(EmployeeNNomina employeeNNomina, string payId)
    {
        List<PaysConcepts_Get_Concepts> payConcepts = new();

        Func<int, Task<HttpResponseMessage>> getConcepts = async (pageNumber) => await a3innuva.PaysConcepts.Get_Concepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, payId, pageNumber);

        payConcepts = await A3innuvaUtils.HandlePagination<PaysConcepts_Get_Concepts>(getConcepts);

        return payConcepts;
    }

    /// <summary>
    /// Obtiene la nómina de A3 de un empleado en un periodo de tiempo. Elimina nóminas temporales
    /// </summary>
    /// <param name="employeeNNomina"></param>
    /// <returns>Nómina de A3</returns>
    public async Task<List<Pays_Get_Pays>> GetAllPaysNEmployeeNNomina(EmployeeNNomina employeeNNomina)
    {
        List<int> idsTipoNominaCustom = new() {
            (int)idsTipoNomina.TemporalPagaMensual,
            (int)idsTipoNomina.TemporalPagaFiniquito,
            (int)idsTipoNomina.TemporalPagaVacacionesFiniquito
        };

        List<Pays_Get_Pays> pays = new();

        Func<int, Task<HttpResponseMessage>> getConcepts = async (pageNumber) => await a3innuva.Pays.Get_Pays((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, employeeNNomina.fechaDesde, employeeNNomina.fechaHasta, pageNumber);

        pays = (await A3innuvaUtils.HandlePagination<Pays_Get_Pays>(getConcepts)).Where(p => !idsTipoNominaCustom.Contains(p.payTypeId)).ToList();

        return pays;
    }

    /// <summary>
    /// Obtiene todas las nómina de A3 de un mes en concreto. Elimina nóminas temporales.
    /// Se puede filtrar por empresa
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="companyCode"></param>
    /// <returns>Nóminas de A3</returns>
    public async Task<List<Pays_Get_Pays_Year_Month>> GetAllPays(int year, int month, int? companyCode = null)
    {
        List<int> idsTipoNominaCustom = new() {
            (int)idsTipoNomina.TemporalPagaMensual,
            (int)idsTipoNomina.TemporalPagaFiniquito,
            (int)idsTipoNomina.TemporalPagaVacacionesFiniquito
        };

        List<companiesCodes> companiesCodesCustom = new((companiesCodes[])Enum.GetValues(typeof(companiesCodes)));

        List<Pays_Get_Pays_Year_Month> pays = new();

        if (companyCode != null)
        {
            return await GetPaysForCompany((int)companyCode);
        }

        foreach (var ccc in companiesCodesCustom)
        {
            pays.AddRange(await GetPaysForCompany((int)ccc));
        }

        async Task<List<Pays_Get_Pays_Year_Month>> GetPaysForCompany(int companyCode)
        {
            Func<int, Task<HttpResponseMessage>> getConcepts = async (pageNumber) => await a3innuva.Pays.Get_Pays(companyCode, year, month, pageNumber);

            var result = (await A3innuvaUtils.HandlePagination<Pays_Get_Pays_Year_Month>(getConcepts)).Where(p => !idsTipoNominaCustom.Contains(p.payTypeID)).ToList();

            foreach (var r in result)
            {
                r.companyCode = companyCode;
            }

            return result;
        }

        return pays.Where(p => p.payDate.Month == month).ToList();
    }

    /// <summary>
    /// Maneja la importación de una nómina de a3innuva a la base de datos
    /// </summary>
    /// <param name="idPersona"></param>
    /// <param name="fechaDesde"></param>
    /// <param name="fechaHasta"></param>
    private async Task<bool> HandleImportarDatosNominaGestoria(int idPersona, DateTime fechaDesde, DateTime fechaHasta, List<Pays_Get_Pays_Year_Month> allPays, List<dynamic> rangoFechasDatos)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());
        var employeeNNomina = GetEmployeeNNominaSinNomina(idPersona, fechaDesde.Date, fechaHasta.Date);

        // Se usa para devolver si hay conflictos donde haya nóminas con y sin contrato asociado.
        List<dynamic> nominasContratos = new();

        // Se almacenan las nóminas que todavía no existen en la base de datos. Más adelante en HandleFiniquitos se añadirán a la base de datos
        List<tblNomina> newNominas = new();

        List<dynamic>? nominasBase = new();

        if (employeeNNomina == null)
        {
            return false;
        }

        if (employeeNNomina?.companyCode == null)
        {
            throw new A3innuvaException("Persona sin companyCode");
        }

        if (employeeNNomina?.employeeCode == null)
        {
            throw new A3innuvaException("Persona sin codigoGestoria");
        }

        // Persona que tiene una fechaBajaContrato dentro del mes en curso
        var isPersonaFiniquitada = db.tblPersonaNTipoContrato.Any(pntc =>
            pntc.idPersona == idPersona
            && pntc.fechaBajaContrato != null
            && pntc.fechaBajaContrato >= fechaDesde
            && pntc.fechaBajaContrato <= fechaHasta
        );

        // Aquí se filtran las pagas para obtener solo las de la persona en curso
        // Además en caso de que la persona no este finiquitada en el mes en curso (!isPersonaFiniquitada) no puede tener algunas pagas (ver abajo)
        var pays = allPays.Where(ap =>
            ap.employeeCode == employeeNNomina.employeeCode
            && ap.companyCode == employeeNNomina.companyCode
            && (
                isPersonaFiniquitada
                || (
                    ap.payTypeID != (int)idsTipoNomina.PagaMensualAtrasos
                    && ap.payTypeID != (int)idsTipoNomina.PagaMensualAtrasosDiferencias
                )
            )
        ).ToList();

        if (pays.Count == 0)
        {
            return false;
        }

        var tblNomina = await db.tblNomina.Where(n => n.idPersona == idPersona && n.fechaDesde.Date >= fechaDesde.Date && n.fechaHasta.Date <= fechaHasta.Date).ToListAsync();

        var tblPersonaNTipoContrato = GetTblPersonaNTipoContrato(idPersona, fechaDesde, fechaHasta);

        // Si solo hay una paga del tipo (x) y un solo contrato, se cambia la fecha de la paga de tipo (x) a la fecha del contrato, para que coincidan siempre
        ActualizarFechaPagas((int)idsTipoNomina.PagaMesual);
        ActualizarFechaPagas((int)idsTipoNomina.PagaFiniquito);
        ActualizarFechaPagas((int)idsTipoNomina.PagaAtrasos);

        // Las personas que tienen solo una paga finiquito, deben tener una paga mensual. Posteriormente en HandleFiniquitos se cambiará la paga mensual a finiquito
        // Este caso contempla que en A3 haya una paga mensual con conceptos y una paga finiquito por valor de 0. En cuyo caso debemos tomar como base una paga mensual de nuevo.
        // De lo contrario, tendriamos una paga mensual de A3 con conceptos y una paga finiquito con los mismos conceptos de MyPolarier.
        if (tblNomina.Count == 1 && tblNomina.First().idTipoNomina == (short)idsTipoNomina.PagaFiniquito)
        {
            tblNomina.First().idTipoNomina = (short)idsTipoNomina.PagaMesual;
        }

        var idNominaBase = tblNomina.FirstOrDefault(n => n.idTipoNomina == (short)idsTipoNomina.PagaMesual)?.idNomina;

        idNominaBase ??= tblNomina.FirstOrDefault()?.idNomina;

        // Se eliminan todas las nóminas que no se hayan generado por Polarier, ya que, serán reimportadas
        var nominasAEliminar = (
            from n in tblNomina
            join pntc in tblPersonaNTipoContrato
            on new { fechaDesde = n.fechaDesde.Date, fechaHasta = n.fechaHasta.Date } equals new { fechaDesde = pntc.fechaAltaContrato.Date, fechaHasta = ((DateTime)pntc.fechaBajaContrato).Date } into pntcGroup
            from pntc in pntcGroup.DefaultIfEmpty()
            where
                (pntc == null || n.idTipoNomina != (short)idsTipoNomina.PagaMesual) &&
                (n.fechaBaja == null) // Añadido para que no se eliminen los finiquitos que se han generado por Polarier
            select n.idNomina
        ).ToList();

        tblNomina.RemoveAll(n => nominasAEliminar.Contains(n.idNomina));

        // Se añade a nominasContratos las nóminas que se han generado por Polarier y que no serán modificadas
        var nominasConContratoSinPay = (
            from pntc in tblPersonaNTipoContrato
            join n in tblNomina
            on new { fechaDesde = pntc.fechaAltaContrato.Date, fechaHasta = ((DateTime)pntc.fechaBajaContrato).Date } equals new { fechaDesde = n.fechaDesde.Date, fechaHasta = n.fechaHasta.Date }
            join p in pays
            on new { fechaDesde = pntc.fechaAltaContrato.Date, fechaHasta = ((DateTime)pntc.fechaBajaContrato).Date } equals new { fechaDesde = p.periodStartDate.Date, fechaHasta = p.periodEndDate.Date } into pGroup
            from p in pGroup.DefaultIfEmpty()
            where
                p == null
            select n.idNomina
        );

        nominasContratos.AddRange(nominasConContratoSinPay.Select(idNomina => new
        {
            idNomina,
            isContratoAsociado = true
        }));

        var paysFiniquitos = pays.Where(p =>
            p.payTypeID == (int)idsTipoNomina.PagaFiniquito
            || p.payTypeID == (int)idsTipoNomina.PagaVacacionesFiniquito
        );

        // Coger todos los conceptos calculados internos
        var calculatedInternalConcepts_taskList = paysFiniquitos.Select(pay => GetAllCalculatedInternalConcepts(employeeNNomina, pay.payID));
        var response_calculatedInternalConcepts_list = await Task.WhenAll(calculatedInternalConcepts_taskList.ToArray());
        // Sumar los conceptos calculados internos
        var calculatedInternalConceptsFiniquitos = response_calculatedInternalConcepts_list
            .SelectMany(x => x)
            .GroupBy(x => x.internalConceptID)
            .Select(x => new PaysConcepts_Get_Calculatedinternalconcepts
            {
                internalConceptID = x.Key,
                calculatedAmount = x.Sum(y => y.calculatedAmount),
                calculatedPercentage = x.FirstOrDefault()?.calculatedPercentage ?? 0,
                longDescription = x.FirstOrDefault()?.longDescription ?? "",
                indPayrollSheet = x.Any(x => x.indPayrollSheet),
                indDeduction = x.Any(x => x.indDeduction),
            })
            .ToList();

        // Coger todos los conceptos de la nómina
        var payConcepts_taskList = paysFiniquitos.Select(pay => GetAllPayConcepts(employeeNNomina, pay.payID));
        var response_payConcepts_list = await Task.WhenAll(payConcepts_taskList.ToArray());
        // Agrupar por concepto

        // TODO: Revisar cantidad y precios de los conceptos agrupados
        var payConceptsFiniquitos = response_payConcepts_list
            .SelectMany(x => x)
            .GroupBy(x => new { x.conceptCode, x.description })
            .Select(x => new PaysConcepts_Get_Concepts
            {
                conceptCode = x.Key.conceptCode,
                description = x.Key.description,
                payConceptAmount = x.Sum(y => y.payConceptAmount),
                payConceptUnits = x.Sum(y => y.payConceptUnits),
                payConceptUnitsAmount = x.Sum(y => y.payConceptAmount) / (x.Sum(y => y.payConceptUnits) <= 0 ? 1 : x.Sum(y => y.payConceptUnits)),
            })
            .ToList();

        var paysContempladas = pays.Where(p =>
            p.payTypeID == (int)idsTipoNomina.PagaMesual
            || p.payTypeID == (int)idsTipoNomina.PagaFiniquito
            || p.payTypeID == (int)idsTipoNomina.PagaAtrasos
        );

        foreach (var pay in paysContempladas)
        {
            bool isFiniquito = pay.payTypeID == (int)idsTipoNomina.PagaFiniquito;
            bool isNominaEnviada = true;

            var contrato = tblPersonaNTipoContrato.FirstOrDefault(pntc =>
                pntc.fechaAltaContrato.Date == pay.periodStartDate.Date
                && ((DateTime)pntc.fechaBajaContrato).Date == pay.periodEndDate.Date
            );

            var nomina = contrato != null
                 ? tblNomina.FirstOrDefault(n =>
                     n.idPersona == idPersona
                     && n.fechaDesde.Date == contrato.fechaAltaContrato.Date
                     && n.fechaHasta.Date == ((DateTime)contrato.fechaBajaContrato).Date
                     && n.idTipoNomina == (short)pay.payTypeID
                 )
                 : null;

            if (nomina != null)
            {
                nominasContratos.Add(new
                {
                    nomina.idNomina,
                    isContratoAsociado = true
                });
            }

            if (nomina == null)
            {
                isNominaEnviada = false;

                var pntc = tblPersonaNTipoContrato.First();

                nomina = idNominaBase != null
                  ? nc.GetNominaBasedOnNomina((int)idNominaBase, idUsuario, pay.periodStartDate.Date, pay.periodEndDate.Date, (short)pay.payTypeID, (byte)idsEstadoNomina.ValidadoGestoria)
                  : nc.GetNewNomina(idPersona, pntc.fechaAltaContrato.Date, ((DateTime)pntc.fechaBajaContrato).Date, idUsuario, (short)pay.payTypeID, (byte)idsEstadoNomina.ValidadoGestoria);

                if (nomina == null) // Si nomina es null la persona no esta bien parametrizada
                {
                    throw new Exception("La persona no esta bien parametrizada");
                }

                nominasContratos.Add(new
                {
                    nomina.idNomina,
                    isContratoAsociado = false
                });
            }

            var response_getPayData = await a3innuva.Pays.Get_Paydata((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, pay.payID);
            var payDataResult = await A3innuvaUtils.DeserializeResponseAsync<List<Pays_Get_Paydata>>(response_getPayData);

            Pays_Get_Paydata payData;

            if (payDataResult.Count == 0)
            {
                payData = new()
                {
                    payID = pay.payID,
                    tariffGroupID = null,
                    jobTitle = null,
                    baseAC = 0,
                    baseCC = 0,
                    prorratedExtraPay = 0,
                    totalBaseIRPF = 0,
                    totalGross = 0,
                    totalRemuneration = 0,
                    costBusiness = 0,
                };
            }
            else
            {
                payData = payDataResult.First();
            }

            var calculatedInternalConcepts = isFiniquito ? calculatedInternalConceptsFiniquitos : await GetAllCalculatedInternalConcepts(employeeNNomina, pay.payID);

            var payConcepts = isFiniquito ? payConceptsFiniquitos : await GetAllPayConcepts(employeeNNomina, pay.payID);

            var finiquitos = paysFiniquitos.Where(pf => pf.payTypeID == (int)idsTipoNomina.PagaFiniquito);

            nomina.tarifa = payData.tariffGroupID.ToString();
            nomina.puestoTrabajo = payData.jobTitle; nomina.baseAC = isFiniquito ? finiquitos.Sum(f => f.baseAC) : pay.baseAC;
            nomina.baseCC = isFiniquito ? finiquitos.Sum(f => f.baseCC) : pay.baseCC;
            nomina.ppext = isFiniquito ? finiquitos.Sum(f => f.prorratedExtraPay) : pay.prorratedExtraPay;
            nomina.totalBaseIRPF = isFiniquito ? finiquitos.Sum(f => f.totalBaseIRPF) : pay.totalBaseIRPF;
            nomina.totalRemuneration = isFiniquito ? finiquitos.Sum(f => f.totalRemuneration) : pay.totalRemuneration;
            nomina.salarioBruto = isFiniquito ? finiquitos.Sum(f => f.totalGross) : pay.totalGross;
            nomina.costeEmpresa = isFiniquito ? paysFiniquitos.Sum(pf => pf.costBusiness) : pay.costBusiness;
            nomina.baseSegSocialHorasExtra = payConcepts.FirstOrDefault(pc => pc.conceptCode == (int)conceptsCodes.HorasExtra)?.payConceptAmount;
            nomina.embargo = payConcepts.FirstOrDefault(pc => pc.conceptCode == (int)conceptsCodes.EmbargoSalarial)?.payConceptAmount;
            nomina.anticipo = payConcepts.FirstOrDefault(pc => pc.conceptCode == (int)conceptsCodes.Anticipo)?.payConceptAmount
                ?? calculatedInternalConcepts.FirstOrDefault(cic => cic.internalConceptID == (int)conceptsCodes.Anticipo)?.calculatedAmount;
            nomina.descuentoPreaviso = payConcepts.FirstOrDefault(pc => pc.conceptCode == (int)conceptsCodes.DescuentoPreaviso)?.payConceptAmount;
            nomina.tipoPaga = pay.payType;

            ApplyCalculatedConcepts(ref nomina, calculatedInternalConcepts, payConcepts.Select(pc => pc.conceptCode).ToList());

            HandleImportarConceptosA3(ref nomina, payConcepts, calculatedInternalConcepts, isNominaEnviada, !isNominaEnviada);

            if (!isNominaEnviada)
            {
                newNominas.Add(nomina);
            }
        }

        tblNomina.AddRange(newNominas);

        await ImportarDetalleNNomina();

        db.tblNomina.AddRange(tblNomina.Where(n => n.idNomina == 0));

        nc.DeleteNominas(db.tblNomina.Where(n => nominasAEliminar.Contains(n.idNomina)).ToList());

        void ActualizarFechaPagas(int idTipoNomina)
        {
            if (pays.Where(p => p.payTypeID == idTipoNomina).Count() == 1 && tblPersonaNTipoContrato.Count == 1)
            {
                var pay = pays.First(p => p.payTypeID == idTipoNomina);
                var pntc = tblPersonaNTipoContrato.First();

                pay.periodStartDate = pntc.fechaAltaContrato;
                pay.periodEndDate = (DateTime)pntc.fechaBajaContrato;
            }
        }

        async Task ImportarDetalleNNomina()
        {
            foreach (var n in tblNomina)
            {
                n.tblDetalleNNomina.Clear();

                if (n.idNomina == 0) return;

                db.tblDetalleNNomina.RemoveRange(db.tblDetalleNNomina.Where(dnn => dnn.idNomina == n.idNomina));
            }

            var persona = db.tblPersona.FirstOrDefault(p => p.idPersona == idPersona);

            var nomina = tblNomina.FirstOrDefault(n => n.idTipoNomina == (short)idsTipoNomina.PagaMesual) ?? tblNomina.FirstOrDefault();

            if (nomina == null) return;

            var rangoFechasSel = rangoFechasDatos
                .FirstOrDefault(rfd =>
                    rfd.idLavanderia == persona?.idLavanderia
                    || rfd.idCentroTrabajo == persona?.idCentroTrabajo
                );

            if (rangoFechasSel == null || (nomina.fechaBaja == null && rangoFechasSel!.fechaFinDatos == null)) return;

            var fechaFinDatos = nomina.fechaBaja ?? rangoFechasSel!.fechaFinDatos;

            var tblCalendarioPersonal = db.tblCalendarioPersonal
                .Where(cp =>
                    cp.idPersona == idPersona
                )
                .ToList()
                .Where(cp =>
                    cp.fecha.Date >= rangoFechasSel!.fechaInicioDatos.Date
                    && cp.fecha.Date <= fechaFinDatos.Date
                )
                .ToList();

            var hayBajasIT = tblCalendarioPersonal.Any(cp =>
                cp.idCalendario_Estado == (byte)idsCalendario_Estado.Baja
                || cp.idCalendario_Estado == (byte)idsCalendario_Estado.BajaAccidenteLaboral
            );

            if (hayBajasIT)
            {
                var response_getTemporaryDisabilities = await a3innuva.TemporaryDisabilities.Get_TemporaryDisabilities((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, nomina.fechaDesde.Year);
                var temporaryDisabilitiesResult = await A3innuvaUtils.DeserializeResponseAsync<List<TemporaryDisabilities_Get_TemporaryDisabilities>>(response_getTemporaryDisabilities);

                var temporaryDisabilities_mesActual = temporaryDisabilitiesResult.Where(td => td.startdate.Date <= nomina.fechaHasta.Date && (td.enddate == null || ((DateTime)td.enddate).Date >= nomina.fechaDesde.Date));

                foreach (var td in temporaryDisabilities_mesActual)
                {
                    var startDate = td.startdate.Date < nomina.fechaDesde.Date ? nomina.fechaDesde : td.startdate;
                    var endDate = td.enddate == null || ((DateTime)td.enddate).Date > nomina.fechaHasta.Date ? nomina.fechaHasta : td.enddate;

                    nomina.tblDetalleNNomina.Add(new()
                    {
                        idNomina = nomina.idNomina,
                        detalle = $"{td.description} {startDate:dd/MM} a {endDate:dd/MM}"
                    });
                }
            }

            var hayAbsentismos = tblCalendarioPersonal.Any(cp => cp.idCalendario_Estado == (byte)idsCalendario_Estado.Absentismo);
            if (hayAbsentismos)
            {
                var response_getAbsenteeisms = await a3innuva.Absenteeisms.Get_Absenteeisms((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, nomina.fechaDesde.Year);
                var absenteeismsResult = await A3innuvaUtils.DeserializeResponseAsync<List<Absenteeisms_Get_Absenteeisms>>(response_getAbsenteeisms);

                foreach (var a in absenteeismsResult)
                {
                    var startDate = a.startDate.Date < nomina.fechaDesde.Date ? nomina.fechaDesde : a.startDate;
                    var endDate = a.endDate == null || ((DateTime)a.endDate).Date > nomina.fechaHasta.Date ? nomina.fechaHasta : a.endDate;

                    nomina.tblDetalleNNomina.Add(new()
                    {
                        idNomina = nomina.idNomina,
                        detalle = $"{a.description} {startDate:dd/MM} a {endDate:dd/MM}"
                    });
                }
            }
        }

        return nominasContratos.GroupBy(nc => nc.isContratoAsociado).Count() > 1;
    }

    /// <summary>
    /// Maneja la importación de los conceptos de una nómina de a3innuva a la base de datos
    /// </summary>
    /// <param name="nomina"></param>
    /// <param name="payConcepts"></param>
    /// <param name="calculatedInternalConcepts"></param>
    /// <param name="isNominaEnviada"></param>
    /// <param name="pisarDiferencias"></param>
    public void HandleImportarConceptosA3(ref tblNomina nomina, List<PaysConcepts_Get_Concepts> payConcepts, List<PaysConcepts_Get_Calculatedinternalconcepts> calculatedInternalConcepts, bool isNominaEnviada, bool pisarDiferencias)
    {
        int idUsuario = this.idUsuario ?? int.Parse(HttpContext.Items["idUsuario"].ToString());
        var idNomina = nomina.idNomina;
        var fechaDesde = nomina.fechaDesde;

        HandlePlusTransporte(nomina.idPersona, nomina.fechaDesde, nomina.fechaHasta);
        HandleAcuerdoNC(nomina.idPersona, nomina.fechaDesde, nomina.fechaHasta);
        HandleCotizacionFormacionProfesional();
        HandleCotizacionDesempleo();

        var calculatedInternalConcepts_inPayrollSheet = calculatedInternalConcepts.Where(cic => cic.indPayrollSheet).ToList();
        var tblConceptoNominaNNomina = db.tblConceptoNominaNNomina.Where(cnnn => cnnn.idNomina == idNomina);

        // Se eliminan todos los conflictos ya que se van a sobreescribir
        db.tblConceptoNominaNNomina_Gestoria.RemoveRange(db.tblConceptoNominaNNomina_Gestoria.Where(cnnn => cnnn.idNomina == idNomina));

        var fullJoinConceptCodes = payConcepts.Select(pc => pc.conceptCode.ToString()).ToList();
        fullJoinConceptCodes.AddRange(calculatedInternalConcepts_inPayrollSheet.Select(cic_ips => cic_ips.internalConceptID.ToString()));
        fullJoinConceptCodes.AddRange(tblConceptoNominaNNomina.Select(cnnn => cnnn.idConceptoNominaNavigation.codigo));

        fullJoinConceptCodes = fullJoinConceptCodes.Distinct().ToList();

        int contadorConflictos = 0;

        // Gestión de conceptos de nómina
        foreach (var cc in fullJoinConceptCodes)
        {
            var pc = payConcepts.FirstOrDefault(pc => pc.conceptCode.ToString() == cc);
            var cic_ips = calculatedInternalConcepts_inPayrollSheet.FirstOrDefault(cic_ips => cic_ips.internalConceptID.ToString() == cc);
            var cnnn = tblConceptoNominaNNomina.FirstOrDefault(cnnn => cnnn.idConceptoNominaNavigation.codigo == cc);

            var conceptoNomina = db.tblConceptoNomina.FirstOrDefault(cn => cn.codigo == cc);

            if (conceptoNomina == null) // Si no existe el concepto en la base de datos, se crea
            {
                lastIdConceptoNomina++;

                if (pc != null)
                {
                    db.tblConceptoNomina.Add(new tblConceptoNomina()
                    {
                        idConceptoNomina = lastIdConceptoNomina,
                        codigo = pc.conceptCode.ToString(),
                        denominacion = pc.description,
                        isInternoCalculado = false,
                        isDevengo = true,
                        isConceptoVariable = false,
                        isModificable = false,
                    });
                }
                else if (cic_ips != null)
                {
                    db.tblConceptoNomina.Add(new tblConceptoNomina()
                    {
                        idConceptoNomina = lastIdConceptoNomina,
                        codigo = cic_ips.internalConceptID.ToString(),
                        denominacion = cic_ips.longDescription,
                        isInternoCalculado = true,
                        isDevengo = !cic_ips.indDeduction,
                        isConceptoVariable = false,
                        isModificable = false,
                    });
                }

            }

            var idConceptoNomina = conceptoNomina?.idConceptoNomina ?? lastIdConceptoNomina;

            if (pisarDiferencias) // Si el tipo de la nómina no es Paga Mensual o se ha autogenerado al importar, se sobreescriben los conceptos
            {
                if (pc == null && cic_ips == null) // Si no existe el concepto en la gestoría, se elimina en la nómina
                {
                    nomina.tblConceptoNominaNNomina.Remove(cnnn);

                    continue;
                }

                if (cnnn == null && pc != null) // Si no existe el concepto en la nómina, se añade a la nómina junto con un conflicto
                {
                    if (pc.payConceptAmount != 0)
                    {
                        var newConceptoNominaNNomina = GetConceptoNominaNNomina(idConceptoNomina, pc.payConceptUnits, pc.payConceptUnitsAmount, pc.payConceptAmount);

                        if (isNominaEnviada)
                        {
                            db.tblConceptoNominaNNomina.Add(newConceptoNominaNNomina);
                        }
                        else
                        {
                            nomina.tblConceptoNominaNNomina.Add(newConceptoNominaNNomina);
                        }
                    }

                    continue;
                }

                if (cnnn == null && cic_ips != null) // Si no existe el concepto interno calculado en la nómina
                {
                    if (cic_ips.calculatedAmount != 0)
                    {
                        var newConceptoNominaNNomina = GetConceptoNominaNNomina(idConceptoNomina, 1, cic_ips.calculatedAmount, cic_ips.calculatedAmount);

                        if (isNominaEnviada)
                        {
                            db.tblConceptoNominaNNomina.Add(newConceptoNominaNNomina);
                        }
                        else
                        {
                            nomina.tblConceptoNominaNNomina.Add(newConceptoNominaNNomina);
                        }
                    }

                    continue;
                }

                // Si existe el concepto interno calculado en la nómina, se actualiza
                if (cic_ips != null)
                {
                    cnnn.cantidad = 1;
                    cnnn.precioUnitario = cic_ips.calculatedAmount;
                    cnnn.idUsuario_validacion = idUsuario;
                    cnnn.fecha_validacion = DateTimeOffset.UtcNow;
                    cnnn.importe = cic_ips.calculatedAmount;

                    continue;
                }

                cnnn.precioUnitario = pc.payConceptUnitsAmount;
                cnnn.cantidad = pc.payConceptUnits;
                cnnn.importe = pc.payConceptAmount;
            }
            else
            {
                if (pc == null && cic_ips == null) // Si no existe el concepto en la gestoría, se crea un conflicto a 0
                {
                    if (conceptoNomina?.isDevengo == true)
                    {
                        if (cnnn.importe != 0) // Si el importe es distinto de 0, se añade un conflicto
                        {
                            var newConceptoNominaNNomina_Gestoria = GetConceptoNominaNNomina_Gestoria(idConceptoNomina, 0, 0, 0);

                            if (isNominaEnviada)
                            {
                                db.tblConceptoNominaNNomina_Gestoria.Add(newConceptoNominaNNomina_Gestoria);
                            }
                            else
                            {
                                nomina.tblConceptoNominaNNomina_Gestoria.Add(newConceptoNominaNNomina_Gestoria);
                            }
                        }
                        else
                        {
                            if (isNominaEnviada)
                            {
                                db.tblConceptoNominaNNomina.Remove(cnnn);
                            }
                            else
                            {
                                nomina.tblConceptoNominaNNomina.Remove(cnnn);
                            }
                        }
                    }
                    else
                    {
                        if (isNominaEnviada)
                        {
                            db.tblConceptoNominaNNomina.Remove(cnnn);
                        }
                        else
                        {
                            nomina.tblConceptoNominaNNomina.Remove(cnnn);
                        }
                    }


                    continue;
                }

                if (cnnn == null && pc != null) // Si no existe el concepto en la nómina, se añade a la nómina junto con un conflicto
                {
                    if (pc.payConceptAmount != 0)
                    {
                        var newConceptoNominaNNomina_Gestoria = GetConceptoNominaNNomina_Gestoria(idConceptoNomina, pc.payConceptUnits, pc.payConceptUnitsAmount, pc.payConceptAmount);

                        if (isNominaEnviada)
                        {
                            db.tblConceptoNominaNNomina_Gestoria.Add(newConceptoNominaNNomina_Gestoria);
                        }
                        else
                        {
                            nomina.tblConceptoNominaNNomina_Gestoria.Add(newConceptoNominaNNomina_Gestoria);
                        }

                        var newConceptoNominaNNomina = GetConceptoNominaNNomina(idConceptoNomina, 0, 0, 0);

                        if (isNominaEnviada)
                        {
                            db.tblConceptoNominaNNomina.Add(newConceptoNominaNNomina);
                        }
                        else
                        {
                            nomina.tblConceptoNominaNNomina.Add(newConceptoNominaNNomina);
                        }
                    }

                    continue;
                }

                if (cnnn == null && cic_ips != null) // Si no existe el concepto interno calculado en la nómina
                {
                    if (cic_ips.calculatedAmount != 0)
                    {
                        if (cic_ips.indDeduction) // Si el concepto interno calculado es una deducción se añade a la nómina
                        {
                            var newConceptoNominaNNomina = GetConceptoNominaNNomina(idConceptoNomina, 1, cic_ips.calculatedAmount, cic_ips.calculatedAmount);

                            if (isNominaEnviada)
                            {
                                db.tblConceptoNominaNNomina.Add(newConceptoNominaNNomina);
                            }
                            else
                            {
                                nomina.tblConceptoNominaNNomina.Add(newConceptoNominaNNomina);
                            }
                        }
                        else // se añade el concepto interno calculado a la nómina junto con un conflicto
                        {
                            var newConceptoNominaNNomina_Gestoria = GetConceptoNominaNNomina_Gestoria(idConceptoNomina, 1, cic_ips.calculatedAmount, cic_ips.calculatedAmount);

                            if (isNominaEnviada)
                            {
                                db.tblConceptoNominaNNomina_Gestoria.Add(newConceptoNominaNNomina_Gestoria);
                            }
                            else
                            {
                                nomina.tblConceptoNominaNNomina_Gestoria.Add(newConceptoNominaNNomina_Gestoria);
                            }

                            var newConceptoNominaNNomina = GetConceptoNominaNNomina(idConceptoNomina, 0, 0, 0);

                            if (isNominaEnviada)
                            {
                                db.tblConceptoNominaNNomina.Add(newConceptoNominaNNomina);
                            }
                            else
                            {
                                nomina.tblConceptoNominaNNomina.Add(newConceptoNominaNNomina);
                            }
                        }
                    }

                    continue;
                }

                // Si existe el concepto interno calculado en la nómina, se actualiza
                if (cic_ips != null)
                {
                    if (cic_ips.indDeduction || cnnn.importe == cic_ips.calculatedAmount) // Si el concepto interno calculado es una deducción o los importes coinciden se importan los valores de gestoría
                    {
                        cnnn.cantidad = 1;
                        cnnn.precioUnitario = cic_ips.calculatedAmount;
                        cnnn.idUsuario_validacion = idUsuario;
                        cnnn.fecha_validacion = DateTimeOffset.UtcNow;

                        if (cic_ips.indDeduction)
                        {
                            cnnn.importe = cic_ips.calculatedAmount;
                        }
                    }
                    else
                    {
                        var newConceptoNominaNNomina_Gestoria = GetConceptoNominaNNomina_Gestoria(idConceptoNomina, 1, cic_ips.calculatedAmount, cic_ips.calculatedAmount);

                        if (isNominaEnviada)
                        {
                            db.tblConceptoNominaNNomina_Gestoria.Add(newConceptoNominaNNomina_Gestoria);
                        }
                        else
                        {
                            nomina.tblConceptoNominaNNomina_Gestoria.Add(newConceptoNominaNNomina_Gestoria);
                        }
                    }

                    continue;
                }

                // Si existe el concepto en la nómina, se actualiza
                if (cnnn.importe != pc.payConceptAmount)
                {
                    var newConceptoNominaNNomina_Gestoria = GetConceptoNominaNNomina_Gestoria(idConceptoNomina, pc.payConceptUnits, pc.payConceptUnitsAmount, pc.payConceptAmount);

                    if (isNominaEnviada)
                    {
                        db.tblConceptoNominaNNomina_Gestoria.Add(newConceptoNominaNNomina_Gestoria);
                    }
                    else
                    {
                        nomina.tblConceptoNominaNNomina_Gestoria.Add(newConceptoNominaNNomina_Gestoria);
                    }
                }
                else
                {
                    cnnn.precioUnitario = pc.payConceptUnitsAmount;
                    cnnn.cantidad = pc.payConceptUnits;
                }
            }
        }

        if (contadorConflictos > 0)
        {
            if (nomina.idEstadoNomina == (byte)idsEstadoNomina.PagoValidado)
            {
                SendAviso_ConflictoNNominaValidadaParaPago(nomina);
            }

            var idEstadoNomina = nomina.fechaBaja != null && nomina.idEstadoHistoricoAsientoNomina != null ? (byte)idsEstadoNomina.ConflictosEnPagoValidado : (byte)idsEstadoNomina.ImportadoConConflictos;

            nomina.idEstadoNomina = idEstadoNomina;

            nomina.tblEstadoNominaNNomina.Add(new tblEstadoNominaNNomina
            {
                idNomina = idNomina,
                idEstadoNomina = idEstadoNomina,
                fecha = DateTimeOffset.UtcNow,
                idUsuario_valida = idUsuario
            });
        }
        else if (!pisarDiferencias)
        {
            if (nomina.fechaBaja == null || nomina.idEstadoNomina < (byte)idsEstadoNomina.ValidadoGestoria)
            {
                nomina.idEstadoNomina = (byte)idsEstadoNomina.ValidadoGestoria;

                nomina.tblEstadoNominaNNomina.Add(new tblEstadoNominaNNomina
                {
                    idNomina = idNomina,
                    idEstadoNomina = (byte)idsEstadoNomina.ValidadoGestoria,
                    fecha = DateTimeOffset.UtcNow,
                    idUsuario_valida = idUsuario
                });
            }
            else
            {
                nomina.tblEstadoNominaNNomina.Add(new tblEstadoNominaNNomina
                {
                    idNomina = idNomina,
                    idEstadoNomina = nomina.idEstadoNomina,
                    fecha = DateTimeOffset.UtcNow,
                    idUsuario_valida = idUsuario
                });
            }
        }

        // Evita conflictos de código de concepto
        void HandlePlusTransporte(int idPersona, DateTime fechaDesde, DateTime fechaHasta)
        {
            var employeeNNomina = GetEmployeeNNominaSinNomina(idPersona, fechaDesde, fechaHasta);

            if (payConcepts.Any(pc => pc.conceptCode == (int)conceptsCodes.AcuerdoNCPolarier) && employeeNNomina.companyCode == (int)companiesCodes.PolarierAndalucia)
            {
                payConcepts.FirstOrDefault(pc => pc.conceptCode == (int)conceptsCodes.AcuerdoNCPolarier).conceptCode = (int)conceptsCodes.PlusTransportePolarierAndalucia;
            }
        }

        // Evita que se dupliquen los conceptos de acuerdo de no conpetencia
        void HandleAcuerdoNC(int idPersona, DateTime fechaDesde, DateTime fechaHasta)
        {
            var employeeNNomina = GetEmployeeNNominaSinNomina(idPersona, fechaDesde, fechaHasta);

            if (payConcepts.Any(pc => pc.conceptCode == (int)conceptsCodes.AcuerdoNCPolarier) && employeeNNomina.companyCode == (int)companiesCodes.PolarierIbiza)
            {
                payConcepts.RemoveAll(pc => pc.conceptCode == (int)conceptsCodes.AcuerdoNCPolarier);
            }
            else if (payConcepts.Any(pc => pc.conceptCode == (int)conceptsCodes.AcuerdoNCPolarierIbiza))
            {
                if (employeeNNomina.companyCode == (int)companiesCodes.Polarier || employeeNNomina.companyCode == (int)companiesCodes.PolarierAndalucia)
                {
                    payConcepts.RemoveAll(pc => pc.conceptCode == (int)conceptsCodes.AcuerdoNCPolarierIbiza);
                }
                else if (employeeNNomina.companyCode == (int)companiesCodes.PolarierIbiza)
                {
                    payConcepts.FirstOrDefault(pc => pc.conceptCode == (int)conceptsCodes.AcuerdoNCPolarierIbiza).conceptCode = (int)conceptsCodes.AcuerdoNCPolarier;
                }
            }
            else if (payConcepts.Any(pc => pc.conceptCode == (int)conceptsCodes.AcuerdoNCPolarierAndalucia))
            {
                if (employeeNNomina.companyCode == (int)companiesCodes.Polarier || employeeNNomina.companyCode == (int)companiesCodes.PolarierIbiza)
                {
                    payConcepts.RemoveAll(pc => pc.conceptCode == (int)conceptsCodes.AcuerdoNCPolarierAndalucia);
                }
                else if (employeeNNomina.companyCode == (int)companiesCodes.PolarierAndalucia)
                {
                    payConcepts.FirstOrDefault(pc => pc.conceptCode == (int)conceptsCodes.AcuerdoNCPolarierAndalucia).conceptCode = (int)conceptsCodes.AcuerdoNCPolarier;
                }
            }
        }

        // Evita que se dupliquen los conceptos de cotización de formación profesional
        void HandleCotizacionFormacionProfesional()
        {
            if (calculatedInternalConcepts.Any(cic => cic.internalConceptID == (int)conceptsCodes.CotizacionFormacionProfesionalFP))
            {
                calculatedInternalConcepts.FirstOrDefault(cic => cic.internalConceptID == (int)conceptsCodes.CotizacionFormacionProfesionalFP).internalConceptID = (int)conceptsCodes.CotizacionFormacionProfesional;
            }
        }

        // Evita que se dupliquen los conceptos de cotización de desempleo
        void HandleCotizacionDesempleo()
        {
            if (calculatedInternalConcepts.Any(cic => cic.internalConceptID == (int)conceptsCodes.CotizacionDesempleoFP))
            {
                calculatedInternalConcepts.FirstOrDefault(cic => cic.internalConceptID == (int)conceptsCodes.CotizacionDesempleoFP).internalConceptID = (int)conceptsCodes.CotizacionDesempleo;
            }
        }

        tblConceptoNominaNNomina GetConceptoNominaNNomina(short idConceptoNomina, decimal cantidad, decimal precioUnitario, decimal importe)
        {
            tblConceptoNominaNNomina newConceptoNominaNNomina = new()
            {
                idNomina = idNomina,
                idConceptoNomina = idConceptoNomina,
                cantidad = cantidad,
                precioUnitario = precioUnitario,
                importe = importe,
                observaciones = "Generado automáticamente desde ImportarNominaGestoria",
                idUsuario_validacion = idUsuario,
                fecha_validacion = DateTimeOffset.UtcNow,
                fecha = fechaDesde,
                idConceptoNominaNavigation = db.tblConceptoNomina.FirstOrDefault(cn => cn.idConceptoNomina == idConceptoNomina)
            };

            return newConceptoNominaNNomina;
        }

        tblConceptoNominaNNomina_Gestoria GetConceptoNominaNNomina_Gestoria(short idConceptoNomina, decimal cantidad, decimal precioUnitario, decimal importe)
        {
            ++contadorConflictos;

            tblConceptoNominaNNomina_Gestoria newConceptoNominaNNomina_Gestoria = new()
            {
                idNomina = idNomina,
                idConceptoNomina = idConceptoNomina,
                cantidad = cantidad,
                precioUnitario = precioUnitario,
                importe = importe,
                observaciones = "Generado automáticamente desde ImportarNominaGestoria",
                idUsuario_validacion = idUsuario,
                fecha_validacion = DateTimeOffset.UtcNow,
                fecha = fechaDesde,
                idConceptoNominaNavigation = db.tblConceptoNomina.FirstOrDefault(cn => cn.idConceptoNomina == idConceptoNomina)
            };

            return newConceptoNominaNNomina_Gestoria;
        }
    }

    /// <summary>
    /// Obtiene el porcentaje de un concepto interno calculado
    /// </summary>
    /// <param name="nomina"></param>
    /// <param name="calculatedInternalConcepts"></param>
    public void ApplyCalculatedConcepts(ref tblNomina nomina, List<PaysConcepts_Get_Calculatedinternalconcepts> calculatedInternalConcepts, List<int> conceptCodes)
    {
        // Es posible que A3 devuelva conceptos tanto en la llamada de concepts como en la de calculatedInternalConcepts y se debben aplicar
        List<int> conceptsCodesCustom = new()
        {
            (int)conceptsCodes.EmbargoSalarial
        };

        var conceptCodesToFilter = conceptCodes.Where(cc => conceptsCodesCustom.Contains(cc)).ToList();

        var internalConceptsFinal = internalConcepts.Where(ic => !conceptCodesToFilter.Contains(ic.internalConceptID));

        foreach (var ic in internalConceptsFinal.Where(ic => calculatedInternalConcepts.Any(cic => cic.internalConceptID == ic.internalConceptID)))
        {
            var calculatedInternalConcept = calculatedInternalConcepts.FirstOrDefault(cic => cic.internalConceptID == ic.internalConceptID);

            var value = ic.isPercentage
                ? (calculatedInternalConcept?.calculatedPercentage / 100)
                : calculatedInternalConcept?.calculatedAmount;

            nomina.GetType().GetProperty(ic.field)?.SetValue(nomina, value);
        }

        foreach (var ic in internalConceptsFinal.Where(ic => !calculatedInternalConcepts.Any(cic => cic.internalConceptID == ic.internalConceptID)))
        {
            nomina.GetType().GetProperty(ic.field)?.SetValue(nomina, null);
        }
    }

    /// <summary>
    /// Obtiene todos los conceptos internos calculados de una nómina de A3
    /// </summary>
    /// <param name="employeeNNomina"></param>
    /// <param name="payId"></param>
    /// <returns>Lista de conceptos internos calculados</returns>
    public async Task<List<PaysConcepts_Get_Calculatedinternalconcepts>> GetAllCalculatedInternalConcepts(EmployeeNNomina employeeNNomina, string payId)
    {
        List<PaysConcepts_Get_Calculatedinternalconcepts> calculatedInternalConcepts = new();

        Func<int, Task<HttpResponseMessage>> getConcepts = async (pageNumber) => await a3innuva.PaysConcepts.Get_Calculatedinternalconcepts((int)employeeNNomina.companyCode, employeeNNomina.employeeCode, payId, pageNumber);

        calculatedInternalConcepts = await A3innuvaUtils.HandlePagination<PaysConcepts_Get_Calculatedinternalconcepts>(getConcepts);

        return calculatedInternalConcepts;
    }

    /// <summary>
    /// Obtiene los contratos de una persona en un rango de fechas
    /// </summary>
    /// <param name="idPersona"></param>
    /// <param name="fechaDesde"></param>
    /// <param name="fechaHasta"></param>
    private List<tblPersonaNTipoContrato> GetTblPersonaNTipoContrato(int idPersona, DateTime fechaDesde, DateTime fechaHasta)
    {
        var tblPersonaNTipoContrato = db.tblPersonaNTipoContrato
            .Where(pntc =>
                pntc.idPersona == idPersona
                && pntc.fechaAltaContrato.Date <= fechaHasta.Date
                && (pntc.fechaBajaContrato == null || ((DateTime)pntc.fechaBajaContrato).Date >= fechaDesde.Date)
            )
            .Select(pntc => new tblPersonaNTipoContrato
            {
                idTipoContrato = pntc.idTipoContrato,
                fechaAltaContrato = pntc.fechaAltaContrato.Date < fechaDesde.Date ? fechaDesde.Date : pntc.fechaAltaContrato.Date,
                fechaBajaContrato = pntc.fechaBajaContrato == null || ((DateTime)pntc.fechaBajaContrato).Date > fechaHasta.Date ? fechaHasta.Date : ((DateTime)pntc.fechaBajaContrato).Date
            })
            .ToList();

        return tblPersonaNTipoContrato;
    }

    public void SendAviso_ConflictoNNominaValidadaParaPago(tblNomina nomina)
    {
        if (Utils.isProduccion())
        {
            return;
        }

        List<int> idsUsuarioCustom = new()
        {
            (int)idsUsuario.NehuenAlfonsoGomez,
            (int)idsUsuario.DavidTorrelloCocera,
            (int)idsUsuario.AlejandroCarrascosaMartorell,
        };

        CorreoService correoService = new();

        List<string> emails = db.tblUsuario.Where(u => idsUsuarioCustom.Contains(u.idUsuario) && u.email != null).Select(u => u.email).ToList();

        var subject = "Cambio de estado en una nómina, de Pago validado a Importado con conflictos";

        var body = $"La paga de {nomina.nombreCompleto} entre {nomina.fechaDesde} y {nomina.fechaHasta} ha cambiado del estado (Pago validado) a (Importado con conflictos).";

        correoService.Add(emails, subject, body);

        try
        {
            correoService.Send();
        }
        catch
        {
        }
    }

    public void SendAviso_Personas_ConPagaA3_SinImportacionMyPolarier(List<string> employeeCodes, List<tblPersona> tblPersona)
    {
        if (Utils.isProduccion())
        {
            return;
        }

        List<int> idsUsuarioCustom = new()
        {
            (int)idsUsuario.NehuenAlfonsoGomez,
            (int)idsUsuario.DavidTorrelloCocera,
            (int)idsUsuario.AlejandroCarrascosaMartorell,
        };

        CorreoService correoService = new();

        List<string> emails = db.tblUsuario.Where(u => idsUsuarioCustom.Contains(u.idUsuario) && u.email != null).Select(u => u.email).ToList();

        var nombresCompletos = string.Join(",\n", tblPersona.Select(p => $"{p.nombre} {p.apellidos}"));

        var listEmployeeCodes = string.Join(",\n", employeeCodes);

        var subject = "Personas con nómina de A3 sin importación en MyPolarier";

        var body = $"Se han detectado {tblPersona.Count + employeeCodes.Count} persona(s) con nómina en A3 que no se está(n) importando en MyPolarier.\n\n" +
                   $"Personas con nómina en A3 ({tblPersona.Count}):\n{nombresCompletos}\n\n" +
                   $"Códigos gestoría sin asociar ({employeeCodes.Count}):\n{listEmployeeCodes}";

        correoService.Add(emails, subject, body);

        try
        {
            correoService.Send();
        }
        catch
        {
        }
    }

    public void SendAviso_FalloAlRevisar_Personas_ConPagaA3_SinImportacionMyPolarier(Exception ex)
    {
        if (Utils.isProduccion())
        {
            return;
        }

        List<int> idsUsuarioCustom = new()
        {
            (int)idsUsuario.NehuenAlfonsoGomez,
            (int)idsUsuario.DavidTorrelloCocera,
            (int)idsUsuario.AlejandroCarrascosaMartorell,
        };

        CorreoService correoService = new();

        List<string> emails = db.tblUsuario.Where(u => idsUsuarioCustom.Contains(u.idUsuario) && u.email != null).Select(u => u.email).ToList();

        var subject = "Error al comprobar personas con nómina de A3 sin importación en MyPolarier";

        var body = $"Ha ocurrido un error al comprobar las nóminas de A3 sin importación en MyPolarier:\n\n{ex.Message}";

        correoService.Add(emails, subject, body);

        try
        {
            correoService.Send();
        }
        catch
        {
        }
    }

    public class GestionNomina_persona
    {
        public bool? isCerrado { get; set; }
        public int? idNomina { get; set; }
        public byte idEstadoNomina { get; set; }
        public short idTipoNomina { get; set; }
        public string tipoNomina { get; set; }
        public DateTime fechaInicioNomina { get; set; }
        public DateTime fechaFinNomina { get; set; }
        public bool isMesCompletoBaja { get; set; }
        public bool enableSend { get; set; }
        public int idPersona { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public int idLavanderia { get; set; }
        public string lavanderia { get; set; }
        public int idTipoTrabajo { get; set; }
        public string tipoTrabajo { get; set; }
        public int? idTipoContrato { get; set; }
        public string tipoContrato { get; set; }
        public int? diasContrato { get; set; }
        public DateTime? fechaAltaContrato { get; set; }
        public int? idCategoriaInterna { get; set; }
        public string categoriaInterna { get; set; }
        public string categoriaConvenio { get; set; }
        public decimal? salarioBase { get; set; }
        public int? numPagas { get; set; }
        public decimal? incentivo { get; set; }
        public decimal? plusAntiguedad { get; set; }
        public decimal? totalPlusAntiguedad { get; set; }
        public bool? isTrienio { get; set; }
        public decimal? salarioBruto { get; set; }
        public int? añosAntiguedad { get; set; }
        public int? numAbsentismo { get; set; }
        public int? numAbsentismosMesAnterior { get; set; }
        public int? faltas { get; set; }
        public int? numAusenciasPuesto { get; set; }
        public decimal? plusAsistencia { get; set; }
        public decimal? totalPlusAsistencia { get; set; }
        public decimal? plusAsistenciaMesAnterior { get; set; }
        public decimal? festivosTrab { get; set; }
        public decimal? plusFestivoTrab { get; set; }
        public bool? baja { get; set; }
        public bool? ceseTemporal { get; set; }
        public bool? bajaLaboral { get; set; }
        public decimal? percMes { get; set; }
        public decimal? pagaExtra1 { get; set; }
        public decimal? pagaExtra2 { get; set; }
        public decimal? horasExtra { get; set; }
        public decimal? horasExtraCalculadas { get; set; }
        public decimal? impHorasExtra { get; set; }
        public string obsHorasExtra { get; set; }
        public decimal? horasNocturnas { get; set; }
        public decimal? plusNocturnidad { get; set; }
        public decimal? plusPeligrosidad { get; set; }
        public decimal? plusResponsabilidad { get; set; }
        public decimal plusActividad { get; set; }
        public string obsPlusActividad { get; set; }
        public decimal plusProductividad { get; set; }
        public decimal totalPlusProductividad { get; set; }
        public string obsPlusProductividad { get; set; }
        public decimal? plusAbsorbible { get; set; }
        public string obsPlusAbsorbible { get; set; }
        public decimal? anticipos { get; set; }
        public string obsAnticipos { get; set; }
        public bool? isEmbargo { get; set; }
        public decimal? acuerdoNC { get; set; }
        public decimal? salarioEspecie { get; set; }
        public decimal segAccidenteConvenio { get; set; }
        public bool isContratoAsociado { get; set; }
        public bool hayConflictos { get; set; }
        public string errorText_salarioBase { get; set; }
        public string errorText_incentivo { get; set; }
        public string errorText_plusAntiguedad { get; set; }
        public string errorText_plusAsistencia { get; set; }
        public string errorText_plusFestivoTrab { get; set; }
        public string errorText_ppext { get; set; }
        public string errorText_impHorasExtra { get; set; }
        public string errorText_plusNocturnidad { get; set; }
        public string errorText_plusPeligrosidad { get; set; }
        public string errorText_plusResponsabilidad { get; set; }
        public string errorText_eventos_diasSinEventos { get; set; }
        public string errorText_eventos_diasMuchosEventos { get; set; }
        public string codMoneda { get; set; }
    }

    public class EmployeeNNomina
    {
        public int idPersona { get; set; }
        public int? companyCode { get; set; }
        public string? employeeCode { get; set; }
        public DateTime fechaDesde { get; set; }
        public DateTime fechaHasta { get; set; }
    }

    private class InternalConceptImport
    {
        public int internalConceptID { get; set; }
        public string field { get; set; }
        public bool isPercentage { get; set; }
    }

    public class HistoricoNomina
    {
        public int idNomina { get; set; }
        public DateTime fechaCobro { get; set; }
        public string? tipoPaga { get; set; }
        public decimal? salarioBruto { get; set; }
        public decimal? pagaExtra1 { get; set; }
        public decimal? pagaExtra2 { get; set; }
        public decimal? tributacionIRPF { get; set; }
        public decimal? segSocialTrabajador { get; set; }
        public decimal? liquidoPercibir { get; set; }
        public decimal? anticipo { get; set; }
        public decimal? embargo { get; set; }
        public decimal? absentismo { get; set; }
        public decimal? indemnizacion { get; set; }
        public decimal? conceptoNEspecie { get; set; }
        public decimal? tributacionEspeciesEmpresa { get; set; }
        public decimal? segSocialEmpresa { get; set; }
        public decimal? totalTC1 { get; set; }
        public decimal? baseCC { get; set; }
        public decimal? costeEmpresa { get; set; }
    }

    private class ResumenConflictoDatosSalariales
    {
        public string field { get; set; }
        public string denominacion { get; set; }
        public List<ConflictoDatosSalariales> conflictos { get; set; }
    }

    private class ConflictoDatosSalariales
    {
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public object? valor { get; set; }
        public object? valorA3 { get; set; }
    }
}
