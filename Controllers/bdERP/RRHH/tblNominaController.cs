using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblNominaController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> hubContext;
    private readonly string CNAE = "9601";

    public tblNominaController(bdERP context, IHubContext<NotificacionesHub> _hubContext)
    {
        db = context;
        hubContext = _hubContext;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario
                            .Include(x => x.idLavanderia)
                            .Include(x => x.idCentroTrabajo)
                            .Include(x => x.idPermiso)
                            .FirstOrDefault(x => x.idUsuario == idUsuario);

        if (objUsuario == null)
            return BadRequest();

        var lavanderias = objUsuario.idLavanderia.Select(x => x.idLavanderia);
        var centros = objUsuario.idCentroTrabajo.Select(x => x.idCentroTrabajo);
        var permisos = objUsuario.idPermiso.Select(x => x.codigo);

        var tblNomina = db.tblNomina.Where(x =>
            objUsuario.idCargo == 1 || // CARGO: DESARROLLADOR (TI)
            x.idPersonaNavigation.idUsuario_validacion_nomina == idUsuario || // Si el usuario es validador de la persona
            (
                (objUsuario.enableDatosRRHH ||
                    permisos.Contains(idsPermiso.GestionFiniquitosGestoria)
                ) && // Si es RRHH y tiene permiso a la lavandería/centro de la persona
                (
                    (x.idPersonaNavigation.idLavanderia != null && lavanderias.Contains((int)x.idPersonaNavigation.idLavanderia)) ||
                    (x.idPersonaNavigation.idCentroTrabajo != null && centros.Contains((int)x.idPersonaNavigation.idCentroTrabajo))
                )

            )
        );

        return Ok(tblNomina);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblNomina> tblNomina)
    {
        var entity = await db.tblNomina.FirstOrDefaultAsync(n => n.idNomina == key);

        if (entity == null)
        {
            return NotFound();
        }

        var operations_idEstadoNomina = tblNomina.Operations.FirstOrDefault(op => op.path == "/idEstadoNomina");
        if (operations_idEstadoNomina != null)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            var idEstadoNomina = Convert.ToByte(operations_idEstadoNomina.value);

            entity.tblEstadoNominaNNomina.Add(new()
            {
                idEstadoNomina = idEstadoNomina,
                fecha = DateTimeOffset.UtcNow,
                idUsuario_valida = idUsuario
            });
        }

        var operations_tblDocumentoNNomina = tblNomina.Operations.FirstOrDefault(op => op.path == "/tblDocumentoNNomina");
        if (operations_tblDocumentoNNomina != null)
        {
            db.tblDocumentoNNomina.RemoveRange(db.tblDocumentoNNomina.Where(x => x.idNomina == entity.idNomina));
        }


        tblNomina.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Updated(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var entity = await db.tblNomina.FirstOrDefaultAsync(n => n.idNomina == key);

        if (entity == null)
        {
            return NotFound();
        }

        DeleteNominas(new List<tblNomina> { entity });

        await db.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Obtiene una nueva nomina con los datos de la persona
    /// </summary>
    /// <param name="idPersona"></param>
    /// <param name="fechaInicioNomina"></param>
    /// <param name="fechaFinNomina"></param>
    /// <param name="idUsuario"></param>
    /// <param name="idTipoNomina"></param>
    /// <param name="idEstadoNomina"></param>
    /// <returns>Nómina con los datos de la persona. En caso de que falten datos en la persona devuelve null</returns>
    public tblNomina? GetNewNomina(int idPersona, DateTime fechaInicioNomina, DateTime fechaFinNomina, int idUsuario, short idTipoNomina = (short)idsTipoNomina.PagaMesual, byte idEstadoNomina = (byte)idsEstadoNomina.EnProceso)
    {
        var objPersona = db.tblPersona
                .Include(p => p.tblDatosSalariales)
                .Include(p => p.idEmpresaPolarierNavigation)
                .Include(p => p.idLavanderiaNavigation)
                .Include(p => p.idCategoriaInternaNavigation.idCategoriaConvenioNavigation)
                .Include(p => p.idCentroTrabajoNavigation)
                .Include(p => p.idTipoTrabajoNavigation)
                .FirstOrDefault(p => p.idPersona == idPersona);

        return GetNewNomina(objPersona, fechaInicioNomina, fechaFinNomina, idUsuario, idTipoNomina, idEstadoNomina);
    }

    public tblNomina? GetNewNomina(tblPersona objPersona, DateTime fechaInicioNomina, DateTime fechaFinNomina, int idUsuario, short idTipoNomina = (short)idsTipoNomina.PagaMesual, byte idEstadoNomina = (byte)idsEstadoNomina.EnProceso)
    {
        if (
            objPersona == null
            || objPersona.tblDatosSalariales.fechaAntiguedad == null
            || objPersona.tblDatosSalariales.numPagas == null
            || objPersona.tblDatosSalariales.percSegSocial == null
            || objPersona.tblDatosSalariales.isTrienio == null
            || objPersona.numDocumentoIdentidad == null
            || objPersona.idCategoriaInternaNavigation == null
            || objPersona.tblDatosSalariales == null
            || objPersona.idEmpresaPolarierNavigation == null
            || objPersona.idCategoriaInternaNavigation == null
            || objPersona.idCategoriaInternaNavigation.idCategoriaConvenioNavigation == null
        )
        {
            return null;
        }

        tblEstadoNominaNNomina objEstadoNominaNNomina = new()
        {
            idEstadoNomina = idEstadoNomina,
            fecha = DateTimeOffset.UtcNow,
            idUsuario_valida = idUsuario
        };

        var personaNTipoContrato = db.tblPersonaNTipoContrato
            .Include(pntc => pntc.idTipoContratoNavigation)
            .Where(pntc =>
                pntc.idPersona == objPersona.idPersona
                && (
                    (
                        fechaInicioNomina <= pntc.fechaAltaContrato
                        && fechaFinNomina >= pntc.fechaAltaContrato
                    )
                    || (
                        fechaInicioNomina >= pntc.fechaAltaContrato
                        && (pntc.fechaBajaContrato == null || fechaInicioNomina <= pntc.fechaBajaContrato)
                    )
                )
            )
            .OrderByDescending(pntc => pntc.fechaAltaContrato)
            .FirstOrDefault();

        var hasFechaBajaContrato = personaNTipoContrato?.fechaBajaContrato != null && fechaInicioNomina.Date <= ((DateTime)personaNTipoContrato?.fechaBajaContrato).Date && fechaFinNomina.Date >= ((DateTime)personaNTipoContrato?.fechaBajaContrato).Date;

        tblNomina nomina = new()
        {
            idEstadoNomina = idEstadoNomina,
            idPersona = objPersona.idPersona,
            fechaDesde = fechaInicioNomina,
            fechaHasta = fechaFinNomina,
            fechaEmision = DateTimeOffset.UtcNow,
            fechaAntiguedad = (DateTime)objPersona.tblDatosSalariales.fechaAntiguedad,
            fechaBaja = personaNTipoContrato?.fechaBajaContrato != null && hasFechaBajaContrato
                    ? personaNTipoContrato?.fechaBajaContrato
                    : null,
            idMotivoBaja = personaNTipoContrato?.idMotivoBaja != null && hasFechaBajaContrato
                    ? personaNTipoContrato?.idMotivoBaja
                    : null,
            idTipoNomina = idTipoNomina,
            tipoPaga = db.tblTipoNomina.FirstOrDefault(tn => tn.idTipoNomina == idTipoNomina)?.denominacion ?? null,
            numSegSocial_empresa = objPersona?.idEmpresaPolarierNavigation?.numInscripcionSegSocial ?? "",
            numSegSocial_persona = objPersona.NAF ?? "",
            NIF_empresa = objPersona?.idEmpresaPolarierNavigation?.CIF ?? "",
            idEmpresaPolarier = objPersona.idEmpresaPolarier,
            empresa = objPersona?.idEmpresaPolarierNavigation?.denominacion ?? "",
            nombreCompleto = $"{objPersona.apellidos}, {objPersona.nombre}",
            domicilio = objPersona?.idLavanderiaNavigation?.direccion ?? "",
            poblacion = objPersona?.idLavanderiaNavigation?.poblacion?.ToUpper() ?? "",
            numDocumentoIdentidad = objPersona.numDocumentoIdentidad.ToUpper(),
            IBAN = objPersona.IBAN?.ToUpper() ?? "",
            denoCategoria = objPersona?.idCategoriaInternaNavigation?.idCategoriaConvenioNavigation?.denominacion ?? "",
            numPagas = (short)objPersona.tblDatosSalariales.numPagas,
            percSegSocial = (decimal)objPersona.tblDatosSalariales.percSegSocial,
            isTrienio = (bool)objPersona.tblDatosSalariales.isTrienio,
            CNAE = CNAE,
            seccion = "",
            NRO = objPersona.codigoGestoria,
            idTipoTrabajo = objPersona.idTipoTrabajo,
            idTipoContrato = personaNTipoContrato?.idTipoContrato,
            idAdmCentroCoste = objPersona.idAdmCentroCoste,
            idAdmElementoPEP = objPersona.idAdmElementoPEP,
            idAdmCuentaContable_Salario = objPersona?.idAdmCuentaContable_Salario,
            idAdmCuentaContable_SSEmpresa = objPersona?.idAdmCuentaContable_SSEmpresa,
            tblEstadoNominaNNomina = new List<tblEstadoNominaNNomina> { objEstadoNominaNNomina }
        };

        return nomina;
    }

    /// <summary>
    /// Obtiene una nueva nomina con los datos de otra nómina
    /// </summary>
    /// <param name="idNomina"></param>
    /// <param name="idUsuario"></param>
    /// <param name="idTipoNomina"></param>
    /// <param name="idEstadoNomina"></param>
    /// <returns>Nómina con los datos de otra nómina. En caso de que no exista la nómina devuelve null</returns>
    public tblNomina? GetNominaBasedOnNomina(int idNomina, int idUsuario, DateTime fechaDesde, DateTime fechaHasta, short? idTipoNomina = null, byte? idEstadoNomina = null)
    {
        var nomina = db.tblNomina.FirstOrDefault(n => n.idNomina == idNomina);

        if (nomina == null)
        {
            return null;
        }

        tblNomina newNomina = new()
        {
            idEstadoNomina = idEstadoNomina ?? nomina.idEstadoNomina,
            idPersona = nomina.idPersona,
            fechaDesde = fechaDesde,
            fechaHasta = fechaHasta,
            fechaEmision = DateTimeOffset.UtcNow,
            fechaBaja = nomina.fechaBaja,
            fechaAntiguedad = nomina.fechaAntiguedad,
            idTipoNomina = idTipoNomina ?? nomina.idTipoNomina,
            numSegSocial_empresa = nomina.numSegSocial_empresa,
            numSegSocial_persona = nomina.numSegSocial_persona,
            NIF_empresa = nomina.NIF_empresa,
            idEmpresaPolarier = nomina.idEmpresaPolarier,
            empresa = nomina.empresa,
            nombreCompleto = nomina.nombreCompleto,
            domicilio = nomina.domicilio,
            poblacion = nomina.poblacion,
            numDocumentoIdentidad = nomina.numDocumentoIdentidad,
            IBAN = nomina.IBAN,
            denoCategoria = nomina.denoCategoria,
            numPagas = nomina.numPagas,
            percSegSocial = nomina.percSegSocial,
            isTrienio = nomina.isTrienio,
            CNAE = nomina.CNAE,
            seccion = nomina.seccion,
            NRO = nomina.NRO,
            idTipoTrabajo = nomina.idTipoTrabajo,
            idTipoContrato = nomina.idTipoContrato,
            idAdmCentroCoste = nomina.idAdmCentroCoste,
            idAdmElementoPEP = nomina.idAdmElementoPEP,
            idAdmCuentaContable_Salario = nomina.idAdmCuentaContable_Salario,
            idAdmCuentaContable_SSEmpresa = nomina.idAdmCuentaContable_SSEmpresa,
            idMotivoBaja = nomina.idMotivoBaja,
            isRetenida = nomina.isRetenida,
            tblEstadoNominaNNomina = new List<tblEstadoNominaNNomina> {
                new () {
                    idEstadoNomina = idEstadoNomina ?? nomina.idEstadoNomina,
                    fecha = DateTimeOffset.UtcNow,
                    idUsuario_valida = idUsuario
                }
            }
        };

        return newNomina;
    }

    /// <summary>
    /// Elimina todos los registros relacionados con una nómina
    /// </summary>
    /// <param name="nominas"></param>
    public void DeleteNominas(List<tblNomina> nominas)
    {
        var idsNomina = nominas.Select(n => n.idNomina).ToList();

        db.tblEstadoNominaNNomina.RemoveRange(db.tblEstadoNominaNNomina.Where(ennn => idsNomina.Contains(ennn.idNomina)));

        db.tblConceptoNominaNNomina.RemoveRange(db.tblConceptoNominaNNomina.Where(cnnn => idsNomina.Contains(cnnn.idNomina)));

        db.tblConceptoNominaNNomina_Gestoria.RemoveRange(db.tblConceptoNominaNNomina_Gestoria.Where(cnnn => idsNomina.Contains(cnnn.idNomina)));

        db.tblHistoricoAsientoNomina.RemoveRange(db.tblHistoricoAsientoNomina.Where(han => idsNomina.Contains(han.idNomina)));

        db.tblDocumentoNNomina.RemoveRange(db.tblDocumentoNNomina.Where(han => idsNomina.Contains(han.idNomina)));

        db.tblDetalleNNomina.RemoveRange(db.tblDetalleNNomina.Where(dnn => idsNomina.Contains(dnn.idNomina)));

        db.tblNomina.RemoveRange(nominas);
    }
}
