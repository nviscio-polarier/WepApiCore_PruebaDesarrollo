using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class.externos.a3innuva;
using WebApiCore.Class.externos.a3innuva.Context.Employees;
using WebApiCore.Class.externos.a3innuva.Context.EmployeesBankaccounts;
using WebApiCore.Class.externos.a3innuva.Context.Pays;
using WebApiCore.Class.externos.a3innuva.Context.PaysConcepts;
using WebApiCore.Class.externos.a3innuva.Context.Workplaces;
using WebApiCore.Class.externos.a3innuva.Controllers;
using WebApiCore.Context;
using WebApiCore.Enums.A3innuva;
using WebApiCore.Enums.General;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Security;
using static WebApiCore.Class.externos.a3innuva.A3innuvaHttpUtils;

namespace WebApiCore.Controllers;

public class A3innuvaController : ODataController
{
    private readonly bdERP db;
    private readonly A3innuva a3innuva = new();

    private readonly List<int> idsUsuarioCustom = new()
    {
        (int)idsUsuario.NehuenAlfonsoGomez,
        (int)idsUsuario.DavidTorrelloCocera,
        (int)idsUsuario.AlejandroCarrascosaMartorell,
    };

    public A3innuvaController(bdERP context)
    {
        db = context;
    }

    [HttpPost("odata/A3innuva/RefreshAccesTokenA3innuva")]
    [Authorize]
    public async Task<ActionResult> RefreshAccesTokenA3innuva([FromODataUri] string refresh_token)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        if (!idsUsuarioCustom.Contains(idUsuario))
        {
            return Unauthorized();
        }

        try
        {
            A3innuvaAuthService authService = new();
            var newAuth = await authService.Refresh_acces_token(refresh_token);

            return Ok(newAuth);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

    }

    [EnableQuery]
    [HttpGet("odata/A3innuva/GetStatusA3innuva")]
    [Authorize]
    public async Task<ActionResult> GetStatusA3innuva()
    {
        bool status;
        try
        {
            status = await a3innuva.GetStatus();
        }
        catch (A3innuvaHttpServiceException ex)
        {
            var result = await A3innuvaUtils.DeserializeResponseAsync(ex.response);
            return Ok(result);
        }

        return Ok(status);
    }

    [EnableQuery]
    [HttpGet("odata/A3innuva/GetAccessTokenA3innuva")]
    [Authorize]
    public ActionResult GetAccessTokenA3innuva()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        if (idsUsuarioCustom.Contains(idUsuario))
        {
            return Ok(new
            {
                access_token = A3innuvaAuthUtils.Get_access_token(),
                refresh_token = A3innuvaAuthUtils.Get_refresh_token()
            });
        }
        return Unauthorized();
    }

    [EnableQuery]
    [HttpGet("odata/A3innuva/GetPagasMesA3")]
    [Authorize]
    public async Task<ActionResult> GetPagasMesA3([FromODataUri] int idPersona, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        if (!idsUsuarioCustom.Contains(idUsuario))
        {
            return Unauthorized();
        }

        var persona = db.tblPersona.FirstOrDefault(p => p.idPersona == idPersona);

        if (persona == null)
        {
            return BadRequest("Persona no encontrada");
        }

        if (string.IsNullOrEmpty(persona.codigoGestoria))
        {
            return BadRequest("No tiene código gestoría");
        }

        var tblLavanderia = db.tblLavanderia.Where(l => l.idPais == (int)idsPais.España).ToList();

        int? companyCode = A3innuvaUtils.GetCompanyCode(tblLavanderia, persona);

        if (companyCode == null)
        {
            return BadRequest("No se ha podido obtener companyCode");
        }

        List<Pays_Get_Pays> pays = new();

        Func<int, Task<HttpResponseMessage>> getPays = async (pageNumber) => await a3innuva.Pays.Get_Pays((int)companyCode, persona.codigoGestoria, fechaDesde, fechaHasta, pageNumber);

        pays = await A3innuvaUtils.HandlePagination<Pays_Get_Pays>(getPays);

        return Ok(pays);
    }

    [EnableQuery]
    [HttpGet("odata/A3innuva/GetConceptosMesA3")]
    [Authorize]
    public async Task<ActionResult> GetConceptosMesA3([FromODataUri] int idPersona, [FromODataUri] string payId)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        if (!idsUsuarioCustom.Contains(idUsuario))
        {
            return Unauthorized();
        }

        var persona = db.tblPersona.FirstOrDefault(p => p.idPersona == idPersona);

        if (persona == null)
        {
            return BadRequest("Persona no encontrada");
        }

        if (string.IsNullOrEmpty(persona.codigoGestoria))
        {
            return BadRequest("No tiene código gestoría");
        }

        var tblLavanderia = db.tblLavanderia.Where(l => l.idPais == (int)idsPais.España).ToList();

        int? companyCode = A3innuvaUtils.GetCompanyCode(tblLavanderia, persona);

        if (companyCode == null)
        {
            return BadRequest("No se ha podido obtener companyCode");
        }

        List<dynamic> payConcepts = new();

        Func<int, Task<HttpResponseMessage>> getConcepts = async (pageNumber) => await a3innuva.PaysConcepts.Get_Concepts((int)companyCode, persona.codigoGestoria, payId, pageNumber);

        payConcepts.AddRange(await A3innuvaUtils.HandlePagination<PaysConcepts_Get_Concepts>(getConcepts));

        Func<int, Task<HttpResponseMessage>> getCalculatedInternalConcepts = async (pageNumber) => await a3innuva.PaysConcepts.Get_Calculatedinternalconcepts((int)companyCode, persona.codigoGestoria, payId, pageNumber);

        payConcepts.AddRange((await A3innuvaUtils.HandlePagination<PaysConcepts_Get_Calculatedinternalconcepts>(getCalculatedInternalConcepts)).Where(cic => cic.indPayrollSheet));

        return Ok(payConcepts);
    }

    [HttpGet("odata/A3innuva/ActualizacionDatosA3")]
    [Authorize]
    public async Task<ActionResult> ActualizacionDatosA3()
    {
        int idUsuario = int.Parse(HttpContext.Items["idUsuario"].ToString());

        var fechaDesde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var fechaHasta = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day);

        var tblLavanderia = db.tblLavanderia.Where(l => l.idPais == (int)idsPais.España).ToList();

        var connection = db.Database.GetDbConnection();
        var datosSalarialesNPersona = (await connection.QueryAsync("EXEC [MyReporting].[EF_gestionNominas_datosSalariales] @idUsuario, @fechaDesde, @fechaHasta", new { idUsuario, fechaDesde, fechaHasta }))
            .ToList()
            .Select(ds => new
            {
                ds.idPersona,
                ds.idAdmCentroCoste,
                ds.idAdmElementoPEP,
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
                ds.fechaAntiguedad,
                IBAN = ds.IBAN?.Replace(" ", "").ToUpper()
            }).ToList();

        var tblAdmCentroCoste = db.tblAdmCentroCoste.ToList();
        var tblAdmElementoPEP = db.tblAdmElementoPEP.ToList();

        var datosSalarialesNPersona_filtered = datosSalarialesNPersona;

        var response_getWorkplaces_polarier = await a3innuva.Workplaces.Get_Workplaces((int)companiesCodes.Polarier, 1);
        List<Workplaces_Get_Workplaces> workplaces = await A3innuvaUtils.DeserializeResponseAsync<List<Workplaces_Get_Workplaces>>(response_getWorkplaces_polarier);

        var response_getWorkplaces_polarierIbiza = await a3innuva.Workplaces.Get_Workplaces((int)companiesCodes.PolarierIbiza, 1);

        workplaces.AddRange(await A3innuvaUtils.DeserializeResponseAsync<List<Workplaces_Get_Workplaces>>(response_getWorkplaces_polarierIbiza));

        var response_getWorkplaces_polarierAndalucia = await a3innuva.Workplaces.Get_Workplaces((int)companiesCodes.PolarierAndalucia, 1);

        workplaces.AddRange(await A3innuvaUtils.DeserializeResponseAsync<List<Workplaces_Get_Workplaces>>(response_getWorkplaces_polarierAndalucia));

        List<ConflictoDatosSalariales> result = new();

        List<dynamic> personasSinEnviar = new();

        foreach (var dsnp in datosSalarialesNPersona_filtered)
        {
            try
            {
                if (dsnp.companyCode == null || (dsnp.employeeCode == null && dsnp.numDocumentoIdentidad == null))
                {
                    personasSinEnviar.Add(new
                    {
                        dsnp.idPersona,
                        motivo = $"Persona sin companyCode ({dsnp.companyCode}), employeeCode ({dsnp.employeeCode}) o numDocumentoIdentidad ({dsnp.numDocumentoIdentidad})"
                    });

                    continue;
                }

                A3innuvaFilter filter = new()
                {
                    field = dsnp.numDocumentoIdentidad != null ? "identifierNumber" : "employeeCode",
                    type = "eq",
                    value = dsnp.numDocumentoIdentidad ?? dsnp.employeeCode,
                    isString = true
                };

                var response_getEmployees = await a3innuva.Employees.Get_Employees((int)dsnp.companyCode, 1, 50, filter);
                List<Employees_Get_Employees> employees = await A3innuvaUtils.DeserializeResponseAsync<List<Employees_Get_Employees>>(response_getEmployees);

                // Si el empleado ha tenido más de un contrato en A3, se cogerá el último
                var employee = employees.OrderByDescending(e => e.enrolmentDate).FirstOrDefault();

                if (employee == null)
                {
                    personasSinEnviar.Add(new
                    {
                        dsnp.idPersona,
                        motivo = "No se ecntontró el trabajador en A3."
                    });

                    continue;
                }

                #region workplaceCode

                var centroTrabajo = tblAdmCentroCoste.FirstOrDefault(cc => cc.idAdmCentroCoste == dsnp.idAdmCentroCoste);

                var elementoPEP = tblAdmElementoPEP.FirstOrDefault(cc => cc.idAdmElementoPEP == dsnp.idAdmElementoPEP);

                var workplaceCode = dsnp.idAdmCentroCoste != null
                ? centroTrabajo?.workplaceCode_A3
                : elementoPEP?.workplaceCode_A3;

                if (workplaceCode == null)
                {
                    var workplace = workplaces.FirstOrDefault(w => w.workplaceCode == employee.workplaceCode);

                    var denominacion = dsnp.idAdmCentroCoste != null
                        ? centroTrabajo?.denominacion
                        : elementoPEP?.denominacion;

                    personasSinEnviar.Add(new
                    {
                        dsnp.idPersona,
                        motivo = $"{(dsnp.idAdmCentroCoste != null ? "El centro de coste" : "El elemento PEP")} {denominacion} ({(dsnp.idAdmCentroCoste ?? dsnp.idAdmElementoPEP)}) no tiene un workplaceCode_A3 configurado. En A3 es {workplace?.workplaceName} ({workplace?.workplaceCode})"
                    });
                }
                else if (workplaceCode != employee.workplaceCode.ToString())
                {
                    var newWorkplaceCode = new
                    {
                        workplaceCode
                    };

                    await a3innuva.Employees.Put_Identification((int)dsnp.companyCode, dsnp.employeeCode, newWorkplaceCode);
                }

                #endregion

                #region IBAN

                var response_getBankaccounts = await a3innuva.EmployeesBankaccounts.Get_Bankaccounts((int)dsnp.companyCode, dsnp.employeeCode);
                List<EmployeesBankaccounts_Get_Bankaccounts> bankAccountCodes = await A3innuvaUtils.DeserializeResponseAsync<List<EmployeesBankaccounts_Get_Bankaccounts>>(response_getBankaccounts);

                var mainAccountCode = bankAccountCodes.FirstOrDefault(cb => cb.isMainAccount)?.bankAccountCode;

                if (mainAccountCode != null)
                {
                    var response_getBankaccount = await a3innuva.EmployeesBankaccounts.Get_Bankaccount((int)dsnp.companyCode, dsnp.employeeCode, mainAccountCode);
                    EmployeesBankaccounts_Get_Bankaccount mainBankAccount = await A3innuvaUtils.DeserializeResponseAsync<EmployeesBankaccounts_Get_Bankaccount>(response_getBankaccount);

                    if (dsnp.IBAN == null)
                    {
                        if (mainBankAccount.iban == null)
                        {
                            personasSinEnviar.Add(new { dsnp.idPersona, motivo = "Persona sin IBAN. Sin IBAN en la cuenta principal en A3." });
                        }
                        else
                        {
                            personasSinEnviar.Add(new { dsnp.idPersona, motivo = $"Persona sin IBAN. IBAN A3: {mainBankAccount.iban}" });
                        }

                        continue;
                    }

                    if (dsnp.IBAN == mainBankAccount.iban)
                    {
                        continue;
                    }
                    else
                    {
                        await a3innuva.EmployeesBankaccounts.Delete_Bankaccount((int)dsnp.companyCode, dsnp.employeeCode, mainAccountCode);
                    }
                }

                if (dsnp.IBAN == null)
                {
                    personasSinEnviar.Add(new { dsnp.idPersona, motivo = "Persona sin IBAN. Sin cuenta bancaria principal en A3." });
                    continue;
                }

                if (dsnp.IBAN?.Lenght != 24)
                {
                    personasSinEnviar.Add(new { dsnp.idPersona, motivo = $"IBAN inválido: {dsnp.IBAN}." });
                    continue;
                }

                var entity = dsnp.IBAN.Substring(4, 4);
                var agency = dsnp.IBAN.Substring(8, 4);
                var digitControl = dsnp.IBAN.Substring(12, 2);
                var account = dsnp.IBAN.Substring(14, 10);

                int idPersona = dsnp.idPersona;

                var persona = db.tblPersona.FirstOrDefault(p => p.idPersona == idPersona);

                EmployeesBankaccounts_Post_Bankaccounts newBankAccount = new()
                {
                    entity = entity,
                    agency = agency,
                    digitControl = digitControl,
                    account = account,
                    iban = dsnp.IBAN,
                    holder = persona != null ? $"{persona.nombre.ToUpper()} {(persona.apellidos ?? "").ToUpper()}" : "",
                    isMainAccount = true,
                    distributionAmount = 0,
                    distributionPercentage = 0,
                    bic = "",
                };

                await a3innuva.EmployeesBankaccounts.Post_Bankaccounts((int)dsnp.companyCode, dsnp.employeeCode, newBankAccount);

                #endregion
            }
            catch (Exception ex)
            {
                personasSinEnviar.Add(new { dsnp.idPersona, motivo = "ERROR: " + ex.Message });
                continue;
            }
        }

        return Ok(personasSinEnviar);
    }

    private class ConflictoDatosSalariales
    {
        public int idPersona { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string field { get; set; }
        public object? valor { get; set; }
        public object? valorA3 { get; set; }
    }
}
