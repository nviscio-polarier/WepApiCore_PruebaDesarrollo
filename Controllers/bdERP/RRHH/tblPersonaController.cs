using Dapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using WebApiCore.Class;
using WebApiCore.Class.bdERP.tblDocumento;
using WebApiCore.Class.externos.a3innuva;
using WebApiCore.Class.externos.a3innuva.Context.EmployeesBankaccounts;
using WebApiCore.Class.externos.a3innuva.Context.EmployeesLaborLife;
using WebApiCore.Class.externos.a3innuva.Context.EmployeesSalary;
using WebApiCore.Class.externos.a3innuva.Controllers;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.RRHH;
using WebApiCore.Enums.A3innuva;
using WebApiCore.Enums.General;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;
using AuthorizeAttribute = WebApiCore.Security.AuthorizeAttribute;

namespace WebApiCore.Controllers;
public class tblPersonaController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    private readonly tblNominaController nc;
    private readonly GestionNominasController gn;
    private readonly A3innuva a3innuva = new();

    readonly Dictionary<string, int> map_datoSalarial_conceptCode = new()
        {
            {"plusResponsabilidad", (int)conceptsCodes.PlusResponsabilidad},
            {"plusPeligrosidad", (int)conceptsCodes.PlusPeligrosidad},
            {"incentivo", (int)conceptsCodes.Incentivos},
            {"acuerdoNC", (int)conceptsCodes.AcuerdoNCPolarier},
            {"salarioEspecie", (int)conceptsCodes.SalarioEspecie},
        };

    readonly List<byte> idsEstadoSolicitudAlta_bloqueados = new()
        {
            (byte)idsEstadoSolicitudAlta.EnProceso,
            (byte)idsEstadoSolicitudAlta.PendienteDocs,
            (byte)idsEstadoSolicitudAlta.PendienteRRHH,
            (byte)idsEstadoSolicitudAlta.SolicitudCambioGestoria,
            (byte)idsEstadoSolicitudAlta.PetAnulacion
        };

    public tblPersonaController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
        nc = new(context, hubContext);
        gn = new(context, hubContext);
    }

    [EnableQuery(MaxAnyAllExpressionDepth = 50, MaxNodeCount = 3000)]
    [HttpGet]
    [Authorize]
    public ActionResult Get([FromODataUri] bool filtroTipoTrabajoRRHH = true)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var idsPersona = Utils.selectPersonasVisibles(db, idUsuario, filtroTipoTrabajoRRHH);
        return Ok(db.tblPersona.Where(x => idsPersona.Contains(x.idPersona)));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblPersona persona)
    {

        db.tblPersona.Add(persona);

        var datosSalariales = db.tblDatosSalariales.FirstOrDefault(x => x.idPersona == persona.idPersona);
        var categoriaInterna = db.tblCategoriaInterna.FirstOrDefault(x => x.idCategoriaInterna == persona.idCategoriaInterna);

        if (datosSalariales != null && categoriaInterna != null)
        {
            datosSalariales.salarioBase = categoriaInterna.salarioBase;
            datosSalariales.plusAsistencia = categoriaInterna.plusAsistencia;
            datosSalariales.plusResponsabilidad = categoriaInterna.plusResponsabilidad;
            datosSalariales.plusPeligrosidad = categoriaInterna.plusPeligrosidad;
            datosSalariales.incentivo = categoriaInterna.incentivo;
            datosSalariales.impHoraExtra = categoriaInterna.impHoraExtra;
            datosSalariales.percSegSocial = categoriaInterna.percSegSocial;
            datosSalariales.plusProductividad = categoriaInterna.plusProductividad;
        }

        persona.idAdmCuentaContable_Salario = persona.idCentroTrabajo != null ?
            db.tblCuentaContableNCentroTrabajo.FirstOrDefault(x => x.idCentroTrabajo == persona.idCentroTrabajo)?.idAdmCuentaContable_Salario :
            db.tblCuentaContableNTipoTrabajo.FirstOrDefault(x => x.idTipoTrabajo == persona.idTipoTrabajo)?.idAdmCuentaContable_Salario;
        persona.idAdmCuentaContable_SSEmpresa = persona.idCentroTrabajo != null ?
            db.tblCuentaContableNCentroTrabajo.FirstOrDefault(x => x.idCentroTrabajo == persona.idCentroTrabajo)?.idAdmCuentaContable_SSEmpresa :
            db.tblCuentaContableNTipoTrabajo.FirstOrDefault(x => x.idTipoTrabajo == persona.idTipoTrabajo)?.idAdmCuentaContable_SSEmpresa;

        await db.SaveChangesAsync();
        return Created(persona);
    }

    // Se usa en workplace RRHH MyPolarier
    [EnableQuery]
    [HttpPost("odata/tblPersona/PostPersonaUsuario")]
    [Authorize]
    public async Task<ActionResult> PostPersonaUsuario([FromQuery] string? emailEmpresa, [FromQuery] short idLocalizacion, [FromBody] tblPersona persona, [FromQuery] int? idLlamamiento)
    {

        #region ASIGNACIÓN DIFERENTES TIPOS DE USUARIO
        string tipoUsuario = "RRHH";
        int[] idsCategoriaAppServicioTecnico = { 20, 24, 26, 27, 34, 35, 38, 39 };
        int[] idsCategoriaAppLogisticaInterna = { 5, 9, 10 };
        int[] idsCategoriaAppLogisticaExterna = { 1, 2, 15, 33, 36, 37, 58 };

        if (idsCategoriaAppServicioTecnico.Contains(persona.idCategoriaInterna ?? -1)) tipoUsuario = "tecnico";
        else if (idsCategoriaAppLogisticaInterna.Contains(persona.idCategoriaInterna ?? -1)) tipoUsuario = "logistico";
        else if (idsCategoriaAppLogisticaExterna.Contains(persona.idCategoriaInterna ?? -1)) tipoUsuario = "transportista";

        var configUsuario = new Dictionary<string, dynamic>()
        {
            { "tecnico", new tblUsuario {
                subtipoUsuario = 1,
                idCargo = 6,
                tblFormularioNUsuario = new List<tblFormularioNUsuario>(){
                    new tblFormularioNUsuario() { idFormulario = 4 }
                }
            } },
            { "transportista", new tblUsuario {
                subtipoUsuario = 1,
                idCargo = 3 ,
                tblFormularioNUsuario = new List<tblFormularioNUsuario>(){
                    new tblFormularioNUsuario() { idFormulario = 47 }
                }
            } },
            { "logistico", new tblUsuario {
                subtipoUsuario = 1,
                idCargo = 3,
                tblFormularioNUsuario = new List<tblFormularioNUsuario>(){
                    new tblFormularioNUsuario() { idFormulario = 4 }
                }
            } },
            { "RRHH", new tblUsuario {
                subtipoUsuario = 3,
                idCargo = 12 ,
                tblFormularioNUsuario = null
            } }
        };

        if (tipoUsuario == "transportista")
        {
            persona.idCategoria = 3;
        }
        #endregion

        persona.activo = (persona.tblPersonaNTipoContrato.FirstOrDefault()?.fechaAltaContrato <= DateTime.UtcNow);
        persona.eliminado = false;

        persona.idAdmCuentaContable_Salario = persona.idCentroTrabajo != null ?
            db.tblCuentaContableNCentroTrabajo.FirstOrDefault(x => x.idCentroTrabajo == persona.idCentroTrabajo)?.idAdmCuentaContable_Salario :
            db.tblCuentaContableNTipoTrabajo.FirstOrDefault(x => x.idTipoTrabajo == persona.idTipoTrabajo)?.idAdmCuentaContable_Salario;
        persona.idAdmCuentaContable_SSEmpresa = persona.idCentroTrabajo != null ?
            db.tblCuentaContableNCentroTrabajo.FirstOrDefault(x => x.idCentroTrabajo == persona.idCentroTrabajo)?.idAdmCuentaContable_SSEmpresa :
            db.tblCuentaContableNTipoTrabajo.FirstOrDefault(x => x.idTipoTrabajo == persona.idTipoTrabajo)?.idAdmCuentaContable_SSEmpresa;

        if (idLlamamiento != null)
        {
            var llamamiento = db.tblLlamamiento.FirstOrDefault(x => x.idLlamamiento == idLlamamiento);
            var contrato = persona.tblPersonaNTipoContrato.FirstOrDefault();
            llamamiento.isNuevaAlta = true;
            llamamiento.numDiasPeriodoPrueba = contrato.numDiasPeriodoPrueba;
            persona.tblLlamamiento.Add(llamamiento);
        }
        else if (persona.idCentroTrabajo != null)
        {
            var contrato = persona.tblPersonaNTipoContrato.FirstOrDefault();
            persona.tblLlamamiento.Add(new tblLlamamiento
            {
                idCentroTrabajo = persona.idCentroTrabajo,
                idCategoriaInterna = (int)persona.idCategoriaInterna,
                fechaIni = contrato.fechaAltaContrato,
                idTipoContrato = (short)contrato.idTipoContrato,
                numDiasPeriodoPrueba = contrato.numDiasPeriodoPrueba,
                activo = true,
                idFormatoDiasLibres = (byte)persona.idFormatoDiasLibres,
                tblDiasLibresPersonal_Llamamiento = persona.tblDiasLibresPersonal.Select(x => new tblDiasLibresPersonal_Llamamiento
                {
                    numDia = x.numDia,
                    idDiaMes = x.idDiaMes,
                    idDiaSemana = x.idDiaSemana,
                }).ToList(),
                codigoLlamamiento = tblLlamamientoController.GetCodigoLlamamiento(db, persona.idLavanderia, persona.idCentroTrabajo),
                isNuevaAlta = true,
            });
        }

        persona.tblPersonaNTipoContrato.Clear();

        List<tblLavanderia> tblLavanderiaNUsuario = new();
        List<tblFormularioNUsuario> tblFormularioNUsuario = new();
        if (persona.tblUsuario.Count > 0)
        {
            var objUsuario = persona.tblUsuario.First();
            var ids = objUsuario.idLavanderia.Select(x => x.idLavanderia);
            tblLavanderiaNUsuario = db.tblLavanderia.Where(x => ids.Contains(x.idLavanderia)).ToList();

            tblFormularioNUsuario = objUsuario.tblFormularioNUsuario.AsList();

            persona.tblUsuario.Clear();
        }

        db.tblPersona.Add(persona);
        await db.SaveChangesAsync();

        emailEmpresa = emailEmpresa == "null" ? null : emailEmpresa;

        var entity = emailEmpresa != null ? db.tblUsuario.Where(y => y.email == emailEmpresa && !y.isEliminado).FirstOrDefault() : null;
        bool usuario_tienePersona = entity != null && entity.idPersona != null;

        if (entity == null) // No hay usuario previo
        {
            tblUsuario objUsuario = persona.tblUsuario.Count > 0 ? persona.tblUsuario.First() : new();

            objUsuario.nombre = persona.nombre;
            objUsuario.idIdioma = 1; // Español
            objUsuario.idTipoUsuario = 2; // Interno
            objUsuario.subtipoUsuario = configUsuario[tipoUsuario].subtipoUsuario;
            objUsuario.usuario = emailEmpresa != null ? emailEmpresa : persona.email;
            objUsuario.email = emailEmpresa != null ? emailEmpresa : persona.email;
            objUsuario.idLocalizacion = idLocalizacion;
            objUsuario.password = "";
            objUsuario.cambiaPassword = false;
            objUsuario.idCargo = configUsuario[tipoUsuario].idCargo;
            objUsuario.refactura = false;
            objUsuario.importaEntidades = false;
            objUsuario.isEliminado = false;
            objUsuario.enableFullScreen = false;
            objUsuario.enableDatosRRHH = false;
            objUsuario.idPersona = persona.idPersona;
            objUsuario.idLavanderia = tblLavanderiaNUsuario;
            objUsuario.tblFormularioNUsuario = tblFormularioNUsuario;

            if (persona.idCentroTrabajo != null)
                objUsuario.idCentroTrabajo = db.tblCentroTrabajo.Where(x => x.idCentroTrabajo == persona.idCentroTrabajo).ToList();

            if (persona.idLavanderia != null && objUsuario.idLavanderia.FirstOrDefault(x => x.idLavanderia == persona.idLavanderia) == null)
                objUsuario.idLavanderia.Add(db.tblLavanderia.Find(persona.idLavanderia));

            if (tblFormularioNUsuario.Count > 0)
                objUsuario.tblFormularioNUsuario.AsList().AddRange(configUsuario[tipoUsuario].tblFormularioNUsuario);

            db.tblUsuario.Add(objUsuario);
            await db.SaveChangesAsync();
        }
        else if (!usuario_tienePersona)
        {
            entity.idPersona = persona.idPersona;
            entity.nombre = persona.nombre;
            entity.idLocalizacion = idLocalizacion;
            entity.isEliminado = false;
            await db.SaveChangesAsync();
        }

        return Ok(Notificar_emailBienvenida(persona.email, persona, idLocalizacion));
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]

    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPersona> persona)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblPersona
            .Include(x => x.tblPersonaNTipoContrato)
            .Include(x => x.tblDatosSalariales)
            .Include(x => x.idLicenciaConducir)
            .Where(x => x.idPersona.Equals(key))
            .FirstOrDefault();
        var objUsuarioPersona = db.tblUsuario
            .Include(x => x.idLavanderia)
            .Include(x => x.tblFormularioNUsuario)
            .FirstOrDefault(x => x.idPersona.Equals(key));

        if (db.tblLlamamiento.Any(x => 
            x.idPersona == key && 
            x.activo == true && 
            x.tblSolicitudAlta != null && 
            idsEstadoSolicitudAlta_bloqueados.Contains(x.tblSolicitudAlta.idEstadoSolicitudAlta)))
        {
            return Ok("solicitudAltaPendiente");
        }

        #region ASIGNACIÓN DIFERENTES TIPOS DE USUARIO
        //string tipoUsuario = "RRHH";
        //int[] idsCategoriaAppServicioTecnico = { 34, 35, 38, 39 };
        //int[] idsCategoriaAppLogisticaInterna = { 5, 9, 10 };
        //int[] idsCategoriaAppLogisticaExterna = { 1, 2, 15, 33, 36, 37 };

        //var idCategoriaInternaString = persona.Operations.FirstOrDefault(x => x.path.Equals("/idCategoriaInterna"));
        //int? idCategoriaInterna = idCategoriaInternaString != null && idCategoriaInternaString.value != null
        //                          ? int.Parse(idCategoriaInternaString.value.ToString())
        //                          : null;
        //if (idCategoriaInternaString != null)
        //{
        //    if (idsCategoriaAppServicioTecnico.Contains(idCategoriaInterna ?? -1)) tipoUsuario = "tecnico";
        //    else if (idsCategoriaAppLogisticaInterna.Contains(idCategoriaInterna ?? -1)) tipoUsuario = "logistico";
        //    else if (idsCategoriaAppLogisticaExterna.Contains(idCategoriaInterna ?? -1)) tipoUsuario = "transportista";

        //    var configUsuario = new Dictionary<string, dynamic>()
        //    {
        //        { "tecnico", new tblUsuario {
        //            subtipoUsuario = 1,
        //            idCargo = 6,
        //            tblFormularioNUsuario = new List<tblFormularioNUsuario>(){
        //                new tblFormularioNUsuario() { idFormulario = 4 }
        //            }
        //        } },
        //        { "transportista", new tblUsuario {
        //            subtipoUsuario = 1,
        //            idCargo = 3 ,
        //            tblFormularioNUsuario = new List<tblFormularioNUsuario>(){
        //                new tblFormularioNUsuario() { idFormulario = 47 }
        //            }
        //        } },
        //        { "logistico", new tblUsuario {
        //            subtipoUsuario = 1,
        //            idCargo = 3,
        //            tblFormularioNUsuario = new List<tblFormularioNUsuario>(){
        //                new tblFormularioNUsuario() { idFormulario = 4 }
        //            }
        //        } },
        //        { "RRHH", new tblUsuario {
        //            subtipoUsuario = 3,
        //            idCargo = 12 ,
        //            tblFormularioNUsuario = null

        //        } }
        //    };

        //    if (tipoUsuario == "transportista")
        //    {
        //        var idCategoriaOperation = persona.Operations.FirstOrDefault(x => x.path.Equals("/idCategoria"));
        //        if (idCategoriaOperation != null)
        //        {
        //            idCategoriaOperation.value = 3;
        //        }
        //        else
        //        {
        //            persona.Replace(p => p.idCategoria, (short)3);
        //        }
        //    }

        //    if(objUsuarioPersona != null)
        //    {
        //        objUsuarioPersona.subtipoUsuario = configUsuario[tipoUsuario].subtipoUsuario;
        //        objUsuarioPersona.idCargo = configUsuario[tipoUsuario].idCargo;

        //            if (objUsuarioPersona.tblFormularioNUsuario == null)
        //            {
        //                objUsuarioPersona.tblFormularioNUsuario = new List<tblFormularioNUsuario>();
        //            }

        //            foreach (tblFormularioNUsuario formularioNUsuario in configUsuario[tipoUsuario].tblFormularioNUsuario)
        //            {
        //            var findForm = db.tblFormularioNUsuario.Where(x => x.idUsuario == objUsuarioPersona.idUsuario && x.idFormulario == formularioNUsuario.idFormulario);
        //                if (findForm.Count() == 0)
        //                {
        //                    objUsuarioPersona.tblFormularioNUsuario.Add(formularioNUsuario);
        //                }
        //            }
        //    }
        //}

        #endregion

        List<int?> idsFotosAntiguas = new List<int?>();
        if (entity.idFotoDocumentoIdentidad_A != null) idsFotosAntiguas.Add((int)entity.idFotoDocumentoIdentidad_A);
        if (entity.idFotoDocumentoIdentidad_B != null) idsFotosAntiguas.Add((int)entity.idFotoDocumentoIdentidad_B);
        if (entity.idFotoPerfil != null) idsFotosAntiguas.Add((int)entity.idFotoPerfil);
        if (entity.idFotoIBAN != null) idsFotosAntiguas.Add((int)entity.idFotoIBAN);
        if (entity.idFotoNAF != null) idsFotosAntiguas.Add((int)entity.idFotoNAF);

        var operation_idLicenciaConducir = persona.Operations.FirstOrDefault(x => x.path.Equals("/idLicenciaConducir"));
        if (operation_idLicenciaConducir != null)
        {
            if (entity != null)
            {
                entity.idLicenciaConducir.Clear();
            }
            List<byte> idsLicenciaConducir = JsonConvert.DeserializeObject<List<tblLicenciaConducir>>(operation_idLicenciaConducir.value.ToString()).Select(x => x.idLicenciaConducir).ToList();
            entity.idLicenciaConducir = db.tblLicenciaConducir.Where(x => idsLicenciaConducir.Contains(x.idLicenciaConducir)).ToList();
            persona.Operations.Remove(operation_idLicenciaConducir);
        }


        var operation_tblPersonaNTipoContrato = persona.Operations.FirstOrDefault(x => x.path.Equals("/tblPersonaNTipoContrato"));
        if (operation_tblPersonaNTipoContrato != null)
        {
            var tblPersonaNTipoContrato = db.tblPersonaNTipoContrato.Where(x => x.idPersona.Equals(key)).ToList();

            var newPersonaNTipoContrato = JsonConvert.DeserializeObject<List<tblPersonaNTipoContrato>>(operation_tblPersonaNTipoContrato.value.ToString());

            if (tblPersonaNTipoContrato.All(x =>
                newPersonaNTipoContrato.Any(y => y.fechaAltaContrato == x.fechaAltaContrato && y.fechaBajaContrato == x.fechaBajaContrato) ||
                (!newPersonaNTipoContrato.Any(y => y.fechaAltaContrato == x.fechaAltaContrato) && x.fechaBajaContrato == null)
            ))
            {
                db.tblPersonaNTipoContrato.RemoveRange(tblPersonaNTipoContrato);
            }
            else
            {
                persona.Operations.Remove(operation_tblPersonaNTipoContrato);
            }

        }

        var operations_tblDiasLibresPersonal = persona.Operations.FirstOrDefault(x => x.path.Equals("/tblDiasLibresPersonal"));
        if (operations_tblDiasLibresPersonal != null)
        {
            db.tblDiasLibresPersonal.RemoveRange(
                db.tblDiasLibresPersonal.Where(x => x.idPersona.Equals(key))
                );
        }

        var operations_idLavanderia = persona.Operations.FirstOrDefault(x => x.path.Equals("/idLavanderia"));
        if (operations_idLavanderia != null)
        {
            // Eliminar cuadrantes pendientes al cambiar de lavanderia a una persona
            db.tblCuadrantePersonal.RemoveRange(
                db.tblCuadrantePersonal.Where(x =>
                x.idPersona == key &&
                x.idLavanderia == Convert.ToInt32(operations_idLavanderia.value) &&
                x.idCalendario_Estado == null &&
                x.fecha > DateTime.Now.Date
                ));
        }

        var operations_tblDatosSalariales_fechaAntiguedad = persona.Operations.FirstOrDefault(x => x.path.Equals("/tblDatosSalariales/fechaAntiguedad"));
        if (operations_tblDatosSalariales_fechaAntiguedad != null)
        {
            var fecha = Convert.ToDateTime(operations_tblDatosSalariales_fechaAntiguedad.value);
            if ($"{fecha:dd/MM/yyyy}" != $"{entity.tblDatosSalariales.fechaAntiguedad:dd/MM/yyyy}")
            {
                entity.tblDatosSalariales.fechaAntiguedad_idUsuario_mod = idUsuario;
                entity.tblDatosSalariales.fechaAntiguedad_fecha_mod = DateTime.Now;
            }
        }

        var operations_tblDatosSalariales = persona.Operations.FirstOrDefault(x => x.path.Equals("/tblDatosSalariales"));
        if (operations_tblDatosSalariales != null)
        {
            var idCategoriaInterna = persona.Operations.FirstOrDefault(x => x.path.Equals("/idCategoriaInterna"));  //
            var isRemoveDatosSalariales_db = false;
            if (objUsuario.enableDatosRRHH)
            {
                isRemoveDatosSalariales_db = true;
            }
            else if (idCategoriaInterna != null) //else if (idCategoriaInternaString != null)
            {
                //En caso de no tener habilitado enableDatosRRHH se van a calcular los datos salariales dependiendo de la categoriaInterna
                var idCategoriaInternaSel = int.Parse(idCategoriaInterna.value.ToString());
                //var idCategoriaInternaSel = int.Parse(idCategoriaInternaString.value.ToString());
                var catInterna = db.tblCategoriaInterna.Where(x => x.idCategoriaInterna.Equals(idCategoriaInternaSel)).FirstOrDefault();

                persona.Operations.FirstOrDefault(x => x.path.Equals("/tblDatosSalariales")).value = catInterna;
                isRemoveDatosSalariales_db = true;
            }

            if (isRemoveDatosSalariales_db) db.tblDatosSalariales.RemoveRange(db.tblDatosSalariales.Where(x => x.idPersona.Equals(key)));
        }

        var operations_isFormPedidoAllowed = persona.Operations.FirstOrDefault(x => x.path.Equals("/isFormPedidoAllowed"));
        if (operations_isFormPedidoAllowed != null)
        {
            var formulario = objUsuarioPersona.tblFormularioNUsuario.FirstOrDefault(x => x.idFormulario == 30);
            if (formulario != null && !((bool)operations_isFormPedidoAllowed.value))
                objUsuarioPersona.tblFormularioNUsuario.Remove(formulario);
            else if (formulario == null && ((bool)operations_isFormPedidoAllowed.value))
                objUsuarioPersona.tblFormularioNUsuario.Add(new tblFormularioNUsuario { idFormulario = 30, idUsuario = objUsuarioPersona.idUsuario });

            persona.Operations.Remove(operations_isFormPedidoAllowed);
        }

        var operations_tblLavanderiaNUsuario = persona.Operations.FirstOrDefault(x => x.path.Equals("/tblLavanderiaNUsuario"));
        if (operations_tblLavanderiaNUsuario != null)
        {
            var tblLavanderiaNUsuario = ((JArray)operations_tblLavanderiaNUsuario.value).Select(x => (int)x["idLavanderia"]).ToList();

            objUsuarioPersona.idLavanderia.Clear();
            objUsuarioPersona.idLavanderia = db.tblLavanderia.Where(x => tblLavanderiaNUsuario.Contains(x.idLavanderia)).ToList();

            persona.Operations.Remove(operations_tblLavanderiaNUsuario);
        }

        var operations_idCategoriaInterna = persona.Operations.FirstOrDefault(x => x.path.Equals("/idCategoriaInterna"));
        if (operations_idCategoriaInterna != null)
        {
            List<int> idsAccesoLavanderias = new() {
                1, //^ TRANSPORTISTA TIPO B
                2, //^ TRANSPORTISTA TIPO C
                5, //^ OPERARIO ALMACÉN
                9, //^ ENCARGADO ALMACÉN
                10, //^ SUPERVISOR ALMACÉN
                15, //^ ENCARGADO TRANSPORTE
                20, //^ TECNICO DE MANTENIMIENTO
                24, //^ JEFE DE MANTENIMIENTO
                26, //^ TECNICO DE MANTENIMIENTO
                27, //^ JEFE DE MANTENIMIENTO
                33, //^ SUPERVISOR TRANSPORTE
                34, //^ AUXILIAR MANTENIMIENTO
                35, //^ TECNICO MANTENIMIENTO
                36, //^ OPERARIO TRANSPORTE
                37, //^ ENCARGADO TRANSPORTE (RODI)
                38, //^ OPERARIO MANTENIMIENTO
                39 //^ JEFE EQUIPO MANTENIMIENTO
            };

            var categoriaInterna = db.tblCategoriaInterna.Include(x => x.idCategoriaConvenioNavigation).FirstOrDefault(x => x.idCategoriaInterna == Convert.ToInt16(operations_idCategoriaInterna.value));

            if (!idsAccesoLavanderias.Contains(categoriaInterna.idCategoriaInterna) && !categoriaInterna.idCategoriaConvenioNavigation.isOficina)
            {
                objUsuarioPersona.idLavanderia.Clear();

                int? idLavanderia = operations_idLavanderia != null ? Convert.ToInt32(operations_idLavanderia.value) : entity.idLavanderia;
                if (idLavanderia != null)
                    objUsuarioPersona.idLavanderia.Add(db.tblLavanderia.FirstOrDefault(x => x.idLavanderia == idLavanderia));
            }
        }

        var operations_tblAlmacenRecambiosNPersona = persona.Operations.FirstOrDefault(x => x.path.Equals("/tblAlmacenRecambiosNPersona"));
        if (operations_tblAlmacenRecambiosNPersona != null)
        {
            db.tblAlmacenRecambiosNPersona.RemoveRange(db.tblAlmacenRecambiosNPersona.Where(x => x.idPersona.Equals(key)));
        }

        var operations_idLocalizacion = persona.Operations.FirstOrDefault(x => x.path.Equals("/idLocalizacion"));
        if (operations_idLocalizacion != null)
        {
            short idLocalizacion = Convert.ToInt16(operations_idLocalizacion.value);
            objUsuarioPersona.idLocalizacion = idLocalizacion;
            persona.Operations.Remove(operations_idLocalizacion);
        }

        var operations_idLlamamiento = persona.Operations.FirstOrDefault(x => x.path.Equals("/idLlamamiento"));
        if (operations_idLlamamiento != null)
        {
            int idLlamamiento = Convert.ToInt32(operations_idLlamamiento.value);
            if (idLlamamiento > 0)
            {
                var objLlamamiento = db.tblLlamamiento.FirstOrDefault(x => x.idLlamamiento == idLlamamiento);
                if (objLlamamiento != null)
                    objLlamamiento.idPersona = entity.idPersona;
            }
            else
            {
                var objLlamamiento = db.tblLlamamiento.Where(x => x.idPersona == entity.idPersona).OrderByDescending(x => x.fechaIni).FirstOrDefault();
                if (objLlamamiento != null)
                {
                    objLlamamiento.idPersona = null;

                    var objContrato = db.tblPersonaNTipoContrato.FirstOrDefault(x => x.idPersona == entity.idPersona && x.fechaAltaContrato == objLlamamiento.fechaIni);
                    if (objContrato != null)
                        db.tblPersonaNTipoContrato.Remove(objContrato);
                }
            }
            persona.Operations.Remove(operations_idLlamamiento);
        }
        else
        {
            var objLlamamiento = db.tblLlamamiento
                .Include(x => x.tblDiasLibresPersonal_Llamamiento)
                .FirstOrDefault(x => x.idPersona == entity.idPersona && x.activo == true);
            if (objLlamamiento != null)
            {
                if (operations_idLavanderia != null)
                {
                    int idLavanderia = Convert.ToInt32(operations_idLavanderia.value);
                    objLlamamiento.idLavanderia = idLavanderia;

                    objLlamamiento.codigoLlamamiento = tblLlamamientoController.GetCodigoLlamamiento(db, idLavanderia, null);
                }

                var operations_idTipoTrabajo = persona.Operations.FirstOrDefault(x => x.path.Equals("/idTipoTrabajo"));
                if (operations_idTipoTrabajo != null)
                {
                    byte idTipoTrabajo = Convert.ToByte(operations_idTipoTrabajo.value);
                    objLlamamiento.idTipoTrabajo = idTipoTrabajo;
                }

                var operations_idTurno = persona.Operations.FirstOrDefault(x => x.path.Equals("/idTurno"));
                if (operations_idTurno != null)
                {
                    int idTurno = Convert.ToInt32(operations_idTurno.value);
                    objLlamamiento.idTurno = idTurno;
                }

                if (operation_tblPersonaNTipoContrato != null)
                {
                    var tblPersonaNTipoContrato = operation_tblPersonaNTipoContrato.value as IEnumerable<dynamic>;
                    var contrato = tblPersonaNTipoContrato.OrderByDescending(x => x.fechaAltaContrato).FirstOrDefault();
                    if (contrato != null)
                    {
                        objLlamamiento.fechaIni = contrato.fechaAltaContrato;
                        objLlamamiento.idTipoContrato = contrato.idTipoContrato;
                    }
                }

                if (operations_idCategoriaInterna != null)
                {
                    int idCategoriaInterna = Convert.ToInt32(operations_idCategoriaInterna.value);
                    objLlamamiento.idCategoriaInterna = idCategoriaInterna;
                }

                var operations_idFormatoDiasLibres = persona.Operations.FirstOrDefault(x => x.path.Equals("/idFormatoDiasLibres"));
                if (operations_idFormatoDiasLibres != null)
                {
                    byte idFormatoDiasLibres = Convert.ToByte(operations_idFormatoDiasLibres.value);
                    objLlamamiento.idFormatoDiasLibres = idFormatoDiasLibres;
                }

                if (operations_tblDiasLibresPersonal != null)
                {
                    var tblDiasLibresPersonal = operations_tblDiasLibresPersonal.value as IEnumerable<dynamic>;
                    db.tblDiasLibresPersonal_Llamamiento.RemoveRange(objLlamamiento.tblDiasLibresPersonal_Llamamiento);

                    foreach (var item in tblDiasLibresPersonal)
                        objLlamamiento.tblDiasLibresPersonal_Llamamiento.Add(new tblDiasLibresPersonal_Llamamiento
                        {
                            idLlamamiento = objLlamamiento.idLlamamiento,
                            idDiaSemana = item.idDiaSemana,
                            idDiaMes = item.idDiaMes,
                            numDia = item.numDia,
                        });
                }
            }
        }

        var idLocalizacion_persona = entity.tblUsuario.FirstOrDefault()?.idLocalizacion;

        if (Utils.isProduccion() && entity.activo && (idLocalizacion_persona == (short)idsLocalizacion.España_PeninsulaBaleares || idLocalizacion_persona == (short)idsLocalizacion.España_IslasCanarias))
        {
            var tblLavanderia = db.tblLavanderia.Where(l => l.idPais == (int)idsPais.España).ToList();
            var companyCode = A3innuvaUtils.GetCompanyCode(tblLavanderia, new tblPersona { idLavanderia = entity.idLavanderia, idCentroTrabajo = entity.idCentroTrabajo });

            if (companyCode == null)
            {
                return Ok("errorPersonaSinLavanderiaCentroTrabajoValido_A3");
            }

            #region Cambio IBAN en A3

            var cambioIBAN = persona.Operations.FirstOrDefault(x => x.path == "/IBAN");

            if (cambioIBAN != null && (string?)cambioIBAN.value != null && entity.IBAN != (string?)cambioIBAN.value && entity.codigoGestoria != null) //Cuando cambia de IBAN
            {
                var error = await ActualizarIBANA3(entity, (int)companyCode, (string)cambioIBAN.value);

                if (error != null)
                {
                    return Ok(error);
                }
            }

            #endregion

            #region Cambio datos salariales

            var cambioSalarioBrutoMensual = persona.Operations.FirstOrDefault(o => o.path == "/tblDatosSalariales/salarioBrutoMensual");
            var newSalarioBrutoMensual = cambioSalarioBrutoMensual != null
                ? Convert.ToDecimal(cambioSalarioBrutoMensual.value)
                : entity.tblDatosSalariales.salarioBrutoMensual ?? 0;

            if (newSalarioBrutoMensual != entity.tblDatosSalariales.salarioBrutoMensual && entity.codigoGestoria != null)
            {
                if (newSalarioBrutoMensual < 0)
                {
                    return Ok("errorSalarioBrutoMensualNegativo");
                }

                var error = await ActualizarSalarioBrutoMensualA3(entity, (int)companyCode, newSalarioBrutoMensual);

                if (error != null)
                {
                    return Ok(error);
                }
            }

            List<string> cambiosDatosSalarialesContemplados = new()
            {
                "/tblDatosSalariales/plusResponsabilidad",
                "/tblDatosSalariales/plusPeligrosidad",
                "/tblDatosSalariales/incentivo",
                "/tblDatosSalariales/acuerdoNC",
                "/tblDatosSalariales/salarioEspecie",
            };

            List<(string datoSalarialField, decimal value)> cambiosDatosSalariales = persona.Operations
                .Where(o => cambiosDatosSalarialesContemplados.Contains(o.path))
                .Select(o => (o.path.Replace("/tblDatosSalariales/", ""), Convert.ToDecimal(o.value)))
                .ToList();

            if (cambiosDatosSalariales.Any() && entity.codigoGestoria != null)
            {
                if (cambiosDatosSalariales.Any(cds => cds.value < 0))
                {
                    return Ok("errorConceptoSalarialNegativo");
                }

                var error = await ActualizarDatosSalarialesA3(entity, (int)companyCode, cambiosDatosSalariales);

                if (error != null)
                {
                    return Ok(error);
                }
            }

            #endregion

            #region Cambio código de gestoría

            var cambio_codigoGestoria = persona.Operations.FirstOrDefault(x => x.path == "/codigoGestoria");
            if (cambio_codigoGestoria != null && (string?)cambio_codigoGestoria.value != null && entity.codigoGestoria != (string?)cambio_codigoGestoria.value) //Cuando cambia de codigoGestoria
            {
                entity.codigoGestoria = (string)cambio_codigoGestoria.value;
                if (entity.IBAN != null && entity.IBAN.Length > 0)
                {
                    var error = await ActualizarIBANA3(entity, (int)companyCode, (string)entity.IBAN);

                    if (error != null)
                    {
                        return Ok(error);
                    }
                }

                cambiosDatosSalariales = cambiosDatosSalarialesContemplados.Select(cdsc =>
                {
                    string datoSalarialField = cdsc.Replace("/tblDatosSalariales/", "");
                    var propertyInfo = entity.tblDatosSalariales.GetType().GetProperty(datoSalarialField);
                    decimal value = propertyInfo != null && propertyInfo.GetValue(entity.tblDatosSalariales) != null
                                    ? Convert.ToDecimal(propertyInfo.GetValue(entity.tblDatosSalariales))
                                    : 0;

                    return (datoSalarialField, value);
                }).ToList();

                if (cambiosDatosSalariales.Any())
                {
                    if (cambiosDatosSalariales.Any(cds => cds.value < 0))
                    {
                        return Ok("errorActualizarConceptoSalarialA3");
                    }

                    var error = await ActualizarDatosSalarialesA3(entity, (int)companyCode, cambiosDatosSalariales);

                    if (error != null)
                    {
                        return Ok(error);
                    }
                }
            }

            #endregion

            #region Cambio workplace en A3

            //var cambioCentroCoste = persona.Operations.FirstOrDefault(x => x.path == "/idAdmCentroCoste");
            //var cambioElementoPEP = persona.Operations.FirstOrDefault(x => x.path == "/idAdmElementoPEP");


            //int? newIdAdmCentroCoste = cambioCentroCoste != null
            //    ? cambioCentroCoste?.value != null
            //        ? Convert.ToInt32(cambioCentroCoste.value)
            //        : null
            //    : entity.idAdmCentroCoste;

            //int? newIdAdmElementoPEP = cambioElementoPEP != null
            //    ? cambioElementoPEP?.value != null
            //        ? Convert.ToInt32(cambioElementoPEP.value)
            //        : null
            //    : entity.idAdmElementoPEP;

            //if (entity.idAdmCentroCoste != newIdAdmCentroCoste || entity.idAdmElementoPEP != newIdAdmElementoPEP)
            //{
            //    var error = await ActualizarWorkplaceA3(entity, (int)companyCode, newIdAdmCentroCoste, newIdAdmElementoPEP);

            //    if (error != null)
            //    {
            //        return Ok(error);
            //    }
            //}

            #endregion
        }

        var operations_IBAN = persona.Operations.FirstOrDefault(x => x.path.Equals("/IBAN"));
        if (operations_IBAN != null)
        {
            var hoy = DateTime.Today;

            var nominasMesNCursoYAnterior = db.tblNomina
                .Where(n =>
                    n.idPersona == entity.idPersona
                    && n.fechaDesde.Year == hoy.Year
                    && (n.fechaDesde.Month == hoy.Month - 1 || n.fechaDesde.Month == hoy.Month)
                    && (n.IBAN == "" || (entity.IBAN != null && n.IBAN.ToLower() == entity.IBAN.ToLower()))
                );

            foreach (var nmnya in nominasMesNCursoYAnterior)
            {
                nmnya.IBAN = operations_IBAN.value?.ToString() ?? "";
            }
        }

        persona.ApplyTo(entity);

        #region Eliminar nóminas sin contrato en estado en proceso

        var tblNomina = db.tblNomina
            .Where(x => x.idPersona == key && x.idEstadoNomina == (byte)idsEstadoNomina.EnProceso)
            .ToList();

        var meses = new List<(DateTime, DateTime)>();

        if (entity.tblPersonaNTipoContrato.Count == 0)
        {
            nc.DeleteNominas(tblNomina);
        }
        else
        {
            DateTime? fechaAltaContratoIndefinido = null;

            foreach (var pntc in entity.tblPersonaNTipoContrato)
            {
                if (pntc.fechaBajaContrato == null)
                {
                    fechaAltaContratoIndefinido = pntc.fechaAltaContrato;

                    continue;
                }

                var fechaActual = pntc.fechaAltaContrato;
                while (fechaActual <= pntc.fechaBajaContrato)
                {
                    var fechaDesde = new DateTime(fechaActual.Year, fechaActual.Month, 1);
                    var fechaHasta = new DateTime(fechaActual.Year, fechaActual.Month, DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month));

                    meses.Add((fechaDesde, fechaHasta));

                    fechaActual = fechaDesde.AddMonths(1);
                }
            }

            meses = meses.Distinct().ToList();

            nc.DeleteNominas(
                tblNomina.Where(n =>
                    (fechaAltaContratoIndefinido == null || n.fechaHasta.Date < ((DateTime)fechaAltaContratoIndefinido).Date)
                    && !meses.Any(m => m.Item1.Date <= n.fechaDesde.Date && n.fechaHasta.Date <= m.Item2.Date)
                ).ToList()
            );
        }

        #endregion

        ActualizarEstadoPersona(entity);

        await db.SaveChangesAsync();

        if (operation_tblPersonaNTipoContrato != null)
        {

            var tblContrato = db.tblPersonaNTipoContrato
                .Where(x => x.idPersona == key);
            var tblCuadrante = db.tblCuadrantePersonal
                .Include(x => x.tblJornada)
                .Where(x => x.idPersona == key);
            var tblCalendario = db.tblCalendarioPersonal
                .Where(x => x.idPersona == key);

            db.tblCuadrantePersonal.RemoveRange(
                tblCuadrante.Where(x =>
                    !x.tblJornada.Any() &&
                    !tblContrato.Where(c =>
                        c.fechaAltaContrato <= x.fecha
                        && (c.fechaBajaContrato == null || c.fechaBajaContrato >= x.fecha)
                    ).Any()
                )
            );

            db.tblCalendarioPersonal.RemoveRange(
                tblCalendario.Where(x =>
                    !tblContrato.Where(c =>
                        c.fechaAltaContrato <= x.fecha
                        && (c.fechaBajaContrato == null || c.fechaBajaContrato >= x.fecha)
                    ).Any()
                )
            );
        }

        var docs_sinRelacion = idsFotosAntiguas.Where(x =>
            db.tblPersona_PeticionCambioDatos.All(y =>
                 y.idFotoPerfil != x.Value &&
                 y.idFotoDocumentoIdentidad_A != x.Value &&
                 y.idFotoDocumentoIdentidad_B != x.Value &&
                 y.idFotoIBAN != x.Value &&
                 y.idFotoNAF != x.Value
            ) &&
             db.tblPersona.All(y =>
                 y.idFotoPerfil != x.Value &&
                 y.idFotoDocumentoIdentidad_A != x.Value &&
                 y.idFotoDocumentoIdentidad_B != x.Value &&
                 y.idFotoIBAN != x.Value &&
                 y.idFotoNAF != x.Value
            ));

        var documentosBorrar = db.tblDocumento.Where(x => docs_sinRelacion.Contains(x.idDocumento));
        db.tblDocumento.RemoveRange(documentosBorrar);
        await db.SaveChangesAsync();

        tblUsuario entityUsuario = db.tblUsuario.FirstOrDefault(x => x.idPersona == entity.idPersona);
        if (entityUsuario != null)
        {
            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToUser(entityUsuario.idUsuario.ToString(), "notificaciones_RRHH", "tblPersona");
        }

        List<tblPersona> result = new List<tblPersona>();
        result.Add(entity);

        return Ok(objUsuario.enableDatosRRHH ? result : true);
    }

    static public void ActualizarEstadoPersona(tblPersona persona)
    {
        DateTime currentDay = DateTime.Now;
        tblPersonaNTipoContrato ultimoContrato = persona.tblPersonaNTipoContrato.OrderByDescending(x => x.fechaAltaContrato).FirstOrDefault();
        if (ultimoContrato != null)
        {
            persona.activo = (ultimoContrato.fechaAltaContrato <= currentDay &&
                (ultimoContrato.fechaBajaContrato == null || (ultimoContrato.fechaBajaContrato != null && currentDay < ultimoContrato.fechaBajaContrato.Value.AddDays(1))));
        }
        else
        {
            persona.activo = false;
        }
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = db.tblPersona.Where(x => x.idPersona.Equals(key)).FirstOrDefault();
        if (entity == null)
            return false;

        entity.eliminado = true;
        entity.activo = false;
        entity.codigoGestoria = null;

        await db.SaveChangesAsync();

        return true;
    }

    [EnableQuery]
    [HttpGet("odata/isPersonaTecnologia")]
    [Authorize]
    public async Task<IActionResult> isPersonaTecnologia()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var isTecnologia = await db.tblUsuario.AnyAsync(x => x.idUsuario == idUsuario && x.idCargo == 1);
        return Ok(isTecnologia);
    }

    [HttpPost]
    public async Task<ActionResult> fn_isNumDocIdentidadPersonaExists([FromODataUri] string numDocumentoIdentidad, [FromODataUri] int? idPersona)
    {
        var entity = db.tblPersona.Where(x => x.numDocumentoIdentidad == numDocumentoIdentidad && ((idPersona != null && x.idPersona != idPersona) || idPersona == null));
        return Ok(entity.Count() > 0);
    }

    [HttpPost]
    public async Task<ActionResult> fn_isEmailExists([FromODataUri] string email)
    {
        var entity = db.tblPersona.Where(y => y.email == email);
        return Ok(entity.Count() > 0);
    }


    [HttpPost]
    public async Task<ActionResult> fn_CheckCodigoGestoria([FromODataUri] int? idPersona, string codigoGestoria, int? idEmpresaPolarier)
    {
        tblPersona personaCodigo = db.tblPersona.Where(x => x.codigoGestoria == codigoGestoria &&
           (idPersona == null || x.idPersona != idPersona) && (idEmpresaPolarier == null || x.idEmpresaPolarier == idEmpresaPolarier)
        ).FirstOrDefault();

        return Ok(personaCodigo == null ? -1 : personaCodigo.idPersona);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> SendMail_DatosPersonales([FromBody] SendMail_DatosPersonales objSend)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        if (!objUsuario.enableDatosRRHH)
            return BadRequest();

        List<string> correosTotales = new List<string>();
        foreach (var lavCentro in objSend.lavCentros)
        {
            var tblPersonas = db.tblPersona.Where(x => lavCentro.idsPersonas.Contains(x.idPersona));
            var emails = db.tblCorreoAltaGestoriaNCentroLav.Where(x => (lavCentro.idLavanderia == x.idLavanderia && x.idLavanderia != null) || (lavCentro.idCentroTrabajo == x.idCentroTrabajo && x.idCentroTrabajo != null)).Select(x => x.correo);
            correosTotales.AddRange(emails);

            var personas = tblPersonas
                .Select(x => new
                {
                    x.nombre,
                    x.apellidos,
                    x.email,
                    x.telefono,
                    x.nacionalidad,
                    denoLavanderia = x.idLavanderiaNavigation != null ? x.idLavanderiaNavigation.denominacion : null,
                    denoCentroTrabajo = x.idCentroTrabajoNavigation != null ? x.idCentroTrabajoNavigation.denominacion : null,
                    ultimoContrato = x.tblPersonaNTipoContrato != null ? x.tblPersonaNTipoContrato.OrderByDescending(x => x.fechaAltaContrato).FirstOrDefault() : null,
                    denoCategoriaConvenio = x.idCategoriaInterna != null ? x.idCategoriaInternaNavigation.idCategoriaConvenioNavigation.denominacion : null,
                    denoCategoriaInterna = x.idCategoriaInterna != null ? x.idCategoriaInternaNavigation.denominacion : null,
                    denoTurno = x.idTurnoNavigation != null ? x.idTurnoNavigation.denominacion : null,
                    divisaLavanderia = x.idLavanderiaNavigation != null ? x.idLavanderiaNavigation.idMonedaNavigation.simbolo : null,
                    salarioBase = x.tblDatosSalariales != null ? x.tblDatosSalariales.salarioBase : null,
                    plusAsistencia = x.tblDatosSalariales != null ? x.tblDatosSalariales.plusAsistencia : null,
                    numPagas = x.tblDatosSalariales != null ? x.tblDatosSalariales.numPagas : null,
                    documentoIdentidad = x.idTipoDocumentoIdentidadNavigation != null ? x.idTipoDocumentoIdentidadNavigation.denominacion : "",
                    x.numDocumentoIdentidad,
                    x.calle,
                    x.numDomicilio,
                    x.piso,
                    x.puerta,
                    x.codigoPostal,
                    x.localidad,
                    comunidadAutonoma = x.idComunidadAutonomaNavigation != null ? x.idComunidadAutonomaNavigation.denominacion : "",
                    pais = x.idPaisNavigation != null ? x.idPaisNavigation.denominacion : "",
                    x.NAF,
                    x.IBAN,
                    foto_perfil = x.idFotoPerfilNavigation != null ? "base64:" + Convert.ToBase64String(x.idFotoPerfilNavigation.documento) : "",
                    foto_documentoIdentidad_A = x.idFotoDocumentoIdentidad_ANavigation != null ? "base64:" + Convert.ToBase64String(x.idFotoDocumentoIdentidad_ANavigation.documento) : "",
                    foto_documentoIdentidad_B = x.idFotoDocumentoIdentidad_BNavigation != null ? "base64:" + Convert.ToBase64String(x.idFotoDocumentoIdentidad_BNavigation.documento) : "",
                    foto_naf = x.idFotoNAFNavigation != null ? "base64:" + Convert.ToBase64String(x.idFotoNAFNavigation.documento) : "",
                    foto_iban = x.idFotoIBANNavigation != null ? "base64:" + Convert.ToBase64String(x.idFotoIBANNavigation.documento) : "",
                    x.codigoGestoria,
                    empresa = x.idEmpresaPolarier != null ? x.idEmpresaPolarierNavigation.denominacion : null
                });

            #region INIT MESSAGE BODY

            int idAsuntoMail = objSend.lavCentros[0].idAsuntoMailAltasGestorias;

            var primeraPersona = personas.FirstOrDefault();
            string centro = (primeraPersona.denoLavanderia != null ? primeraPersona.denoLavanderia : primeraPersona.denoCentroTrabajo);
            string todayFecha = String.Format("{0:d-M-yyyy}", DateTime.Now);

            string subjectLlamamiento = String.Format("{0}_LLAMAMIENTO", centro);
            if (personas.Count() == 1)
            {
                string fecha = (primeraPersona.ultimoContrato != null ? primeraPersona.ultimoContrato.fechaAltaContrato.ToString("dd-MM-yy") : todayFecha);
                subjectLlamamiento += "_" + fecha + "_" + primeraPersona.nombre + " " + primeraPersona.apellidos;
            }
            else
            {
                subjectLlamamiento += "_" + todayFecha;
            }

            var asuntosMail = new Dictionary<int, dynamic>()
            {
                { 1, new {subject= "Nuevas altas de personal" } },
                { 2, new {subject= subjectLlamamiento} }
            };

            string messageBody = "<font> Ruego procedáis a dar de alta a: </font><br><br>";

            string colorPolarier = "#FFC000";
            string colorPolarier_claro = "#FFF2CC";
            string colorAmarillo = "#FFFF99";
            string colorAmarillo_claro = "#FFFFCC";
            string colorGris = "#D9D9D9";
            string colorGris_claro = "#DDDDDD";
            string colorVerde = "#92D050";
            string colorBlanco = "#ffffff";

            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
            string htmlTableEnd = "</table>";
            string htmlHeaderRowStart = "<tr style =\" color:#555555;\">";
            string htmlHeaderRowEnd = "</tr>";
            string htmlTrStart = "<tr style =\"color:#555555; \">";
            string htmlTrEnd = "</tr>";
            string htmlTdEnd = "</td>";
            int numPersonas_codGestoria = personas.Where(x => x.codigoGestoria != null).Count();

            string thStart(string color)
            {
                return "<th style=\" background-color: " + color + "; border-style:solid; border-width:thin;  padding: 5px 5px; border-color:#555555; font-weight: 400; color:#333333; font-size:12px; \">";
            }

            string thEnd = "</th>";

            string tdStart(string color)
            {
                return "<td style =\" background-color: " + color + ";border-color:#555555; border-style:solid; border-width:thin; padding: 5px; ; font-size:12px; \">";
            }

            messageBody += htmlTableStart;
            messageBody += htmlHeaderRowStart;
            if (numPersonas_codGestoria > 0) { messageBody += thStart(colorVerde) + "EMPRESA" + thEnd; }
            if (numPersonas_codGestoria > 0) { messageBody += thStart(colorVerde) + "CÓD. GESTORÍA" + thEnd; }
            messageBody += thStart(colorPolarier) + "CENTRO" + thEnd;
            messageBody += thStart(colorPolarier) + "TIPO CONTRATO " + thEnd;
            messageBody += thStart(colorPolarier) + "CATEGORÍA CONVENIO " + thEnd;
            messageBody += thStart(colorPolarier) + "CATEGORÍA INTERNA" + thEnd;
            messageBody += thStart(colorPolarier) + "APELLIDOS " + thEnd;
            messageBody += thStart(colorPolarier) + "NOMBRES " + thEnd;
            messageBody += thStart(colorAmarillo) + "FECHA DE ALTA " + thEnd;
            messageBody += thStart(colorAmarillo) + "PERÍODO PRUEBA " + thEnd;
            messageBody += thStart(colorAmarillo) + "TURNO " + thEnd;
            messageBody += thStart(colorGris_claro) + "S. BASE " + thEnd;
            messageBody += thStart(colorGris_claro) + "P. ASIST. " + thEnd;
            messageBody += thStart(colorGris_claro) + "P. EXT " + thEnd;
            messageBody += htmlHeaderRowEnd;
            #endregion

            List<tblTipoContrato> tipoContrato = db.tblTipoContrato.ToList();
            List<Attachment> docs = new List<Attachment>();
            foreach (var persona in personas)
            {
                #region MESSAGE BODY
                messageBody = messageBody + htmlTrStart;
                if (numPersonas_codGestoria > 0)
                {
                    messageBody = messageBody + tdStart(colorBlanco) + (persona.empresa != null ? persona.empresa : "-") + htmlTdEnd;
                    messageBody = messageBody + tdStart(colorBlanco) + (persona.codigoGestoria != null ? persona.codigoGestoria : "-") + htmlTdEnd;
                }
                messageBody = messageBody + tdStart(colorBlanco) + (persona.denoLavanderia != null ? persona.denoLavanderia : persona.denoCentroTrabajo) + htmlTdEnd;
                messageBody = messageBody + tdStart(colorAmarillo_claro) + (persona.ultimoContrato != null ? (persona.ultimoContrato.idTipoContrato != null ? tipoContrato.Find(x => x.idTipoContrato == persona.ultimoContrato.idTipoContrato).denominacion.ToUpper() : "-") : "-") + htmlTdEnd;
                messageBody = messageBody + tdStart(colorPolarier_claro) + (persona.denoCategoriaConvenio != null ? persona.denoCategoriaConvenio.ToUpper() : null) + htmlTdEnd;
                messageBody = messageBody + tdStart(colorPolarier_claro) + (persona.denoCategoriaInterna != null ? persona.denoCategoriaInterna.ToUpper() : null) + htmlTdEnd;
                messageBody = messageBody + tdStart(colorPolarier_claro) + persona.apellidos + htmlTdEnd;
                messageBody = messageBody + tdStart(colorPolarier_claro) + persona.nombre + htmlTdEnd;
                messageBody = messageBody + tdStart(colorPolarier_claro) + (persona.ultimoContrato != null ? persona.ultimoContrato.fechaAltaContrato.ToString("dd-MM-yy") : "-") + htmlTdEnd;
                messageBody = messageBody + tdStart(colorPolarier_claro) + (persona.ultimoContrato != null ? (persona.ultimoContrato.numDiasPeriodoPrueba != null ? persona.ultimoContrato.numDiasPeriodoPrueba + " Días" : "-") : "-") + htmlTdEnd;
                messageBody = messageBody + tdStart(colorBlanco) + (persona.denoTurno != null ? persona.denoTurno : "-") + htmlTdEnd;
                messageBody = messageBody + tdStart(colorBlanco) + (persona.salarioBase != null ? (persona.salarioBase + " " + (persona.denoLavanderia != null ? persona.divisaLavanderia : "€")) : "-") + htmlTdEnd;
                messageBody = messageBody + tdStart(colorBlanco) + (persona.plusAsistencia != null ? (persona.plusAsistencia + " " + (persona.denoLavanderia != null ? persona.divisaLavanderia : "€")) : "-") + htmlTdEnd;
                messageBody = messageBody + tdStart(colorGris) + (persona.numPagas != null ? persona.numPagas : "-") + htmlTdEnd;
                messageBody = messageBody + htmlTrEnd;
                #endregion

                #region ADJUNTAR DOCS
                Document document = new Document();
                document.Info.Title = (persona.nombre ?? "") + " " + (persona.apellidos ?? "");
                document.Info.Subject = asuntosMail[idAsuntoMail].subject;
                document.Info.Author = "Polarier";
                DefineStyles(document);

                Section section = document.AddSection();
                // Put a logo in the header
                string currentDirectory = Directory.GetCurrentDirectory();
                string path = "Media/Imagenes";
                string fullPath = Path.Combine(currentDirectory, path, "logoPolarier.jpg");

                MigraDoc.DocumentObjectModel.Shapes.Image imageLogo = section.Headers.Primary.AddImage(fullPath);
                imageLogo.Width = "5.5cm";
                imageLogo.LockAspectRatio = true;
                imageLogo.RelativeVertical = RelativeVertical.Line;
                imageLogo.RelativeHorizontal = RelativeHorizontal.Margin;
                imageLogo.Top = ShapePosition.Top;
                imageLogo.Left = ShapePosition.Right;
                imageLogo.WrapFormat.Style = WrapStyle.Through;

                section.AddParagraph().Format.SpaceAfter = "1cm";

                TextFrame textFrame_perfil = section.AddTextFrame();
                textFrame_perfil.Width = "15cm";
                textFrame_perfil.RelativeVertical = RelativeVertical.Line;
                textFrame_perfil.RelativeHorizontal = RelativeHorizontal.Margin;
                textFrame_perfil.Top = ShapePosition.Top;
                textFrame_perfil.Left = ShapePosition.Left;
                textFrame_perfil.WrapFormat.Style = WrapStyle.Through;

                addDataParagraph(textFrame_perfil, "Nombre", persona.nombre ?? "");
                addDataParagraph(textFrame_perfil, "Apellidos", persona.apellidos ?? "");
                addDataParagraph(textFrame_perfil, "E-mail", persona.email ?? "");
                addDataParagraph(textFrame_perfil, "Teléfono", persona.telefono ?? "");

                if (persona.foto_perfil == "")
                {
                    TextFrame textFrame_imgNoDisponible = section.AddTextFrame();
                    textFrame_imgNoDisponible.Width = "5cm";
                    textFrame_imgNoDisponible.RelativeVertical = RelativeVertical.Line;
                    textFrame_imgNoDisponible.RelativeHorizontal = RelativeHorizontal.Margin;
                    textFrame_imgNoDisponible.Top = ShapePosition.Top;
                    textFrame_imgNoDisponible.Left = ShapePosition.Right;
                    addImagenNoDisponibleParagraph(textFrame_imgNoDisponible, 70);
                }
                else
                {
                    MigraDoc.DocumentObjectModel.Shapes.Image imagePerfil = section.AddImage(persona.foto_perfil);
                    imagePerfil.Width = "5cm";
                    imagePerfil.LockAspectRatio = true;
                    imagePerfil.RelativeVertical = RelativeVertical.Line;
                    imagePerfil.RelativeHorizontal = RelativeHorizontal.Margin;
                    imagePerfil.Top = ShapePosition.Top;
                    imagePerfil.Left = ShapePosition.Right;
                }

                section.AddParagraph().Format.SpaceAfter = "2cm";

                addDataParagraph(section, "Nacionalidad", persona.nacionalidad ?? "");
                addDataParagraph(section, "Documento de identidad", (persona.documentoIdentidad != "" ? persona.documentoIdentidad + ": " : "") + (persona.numDocumentoIdentidad ?? ""));
                addDataParagraph(section, "Calle", persona.calle ?? "");
                addDataParagraph(section, "Número de domicilio", persona.numDomicilio ?? "");
                addDataParagraph(section, "Piso", persona.piso ?? "");
                addDataParagraph(section, "Puerta", persona.puerta ?? "");
                addDataParagraph(section, "Codigo postal", persona.codigoPostal ?? "");
                addDataParagraph(section, "Localidad", persona.localidad ?? "");
                addDataParagraph(section, "Comunidad autonoma", persona.comunidadAutonoma ?? "");
                addDataParagraph(section, "País", persona.pais ?? "");
                addDataParagraph(section, "NAF", persona.NAF ?? "");
                addDataParagraph(section, "IBAN", persona.IBAN ?? "");

                Section section2 = document.AddSection();
                section2.AddParagraph().Format.SpaceAfter = "1cm";

                if (persona.foto_documentoIdentidad_A != "")
                {
                    addFoto(section2, "Documento identidad (anverso)", persona.foto_documentoIdentidad_A);
                }
                else
                {
                    addDataParagraph(section2, "Documento identidad (anverso)", "");
                    addImagenNoDisponibleParagraph(section2, 50);
                }

                if (persona.foto_documentoIdentidad_B != "")
                {
                    addFoto(section2, "Documento identidad (reverso)", persona.foto_documentoIdentidad_B);
                }
                else
                {
                    addDataParagraph(section2, "Documento identidad (reverso)", "");
                    addImagenNoDisponibleParagraph(section2, 50);
                }

                Section section3 = document.AddSection();
                section3.AddParagraph().Format.SpaceAfter = "1cm";

                if (persona.foto_naf != "")
                {
                    addFoto(section3, "NAF", persona.foto_naf);
                }
                else
                {
                    addDataParagraph(section3, "NAF", "");
                    addImagenNoDisponibleParagraph(section3, 50);
                }

                if (persona.foto_iban != "")
                {
                    addFoto(section3, "IBAN", persona.foto_iban);
                }
                else
                {
                    addDataParagraph(section3, "IBAN", "");
                    addImagenNoDisponibleParagraph(section3, 50);
                }

                MemoryStream ms = new MemoryStream();
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
                pdfRenderer.Document = document;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(ms);

                ContentType type = new ContentType(MediaTypeNames.Application.Pdf);
                Attachment documento = new Attachment(ms, type);
                documento.ContentDisposition.FileName = (persona.nombre ?? "") + " " + (persona.apellidos ?? "") + ".pdf";

                docs.Add(documento);
                #endregion
            }

            #region END MESSAGE BODY
            messageBody = messageBody + htmlTableEnd;
            messageBody += "<br><br> <font> Saludos.</font>";
            #endregion

            int numDocsMail = 10;
            decimal numPages = Math.Ceiling((decimal)docs.Count / numDocsMail);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            SmtpClient smtpClient = new SmtpClient
            {
                Host = "outlook.office365.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("mypolarier@polarier.com", "Vog45080")
            };

            //idsLavanderia
            //idsCentroTrabajo

            for (int i = 1; i <= numPages; i++)
            {
                // Generamos el mail a enviar
                var mail = new MailMessage();
                mail.From = new MailAddress("mypolarier@polarier.com", "No Reply MyPolarier");

                if (Utils.isProduccion())
                {
                    foreach (var email in emails)
                    {
                        mail.To.Add(email);
                    }
                }
                else
                {
                    mail.To.Add("acarrascosa@polarier.com");
                    correosTotales.Clear();
                    correosTotales.AddRange(mail.To.Select(x => x.Address).ToList());
                }

                mail.Subject = asuntosMail[idAsuntoMail].subject + " " + (numPages > 1 ? (i + "/" + numPages) : "");
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                mail.Body = messageBody;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                int getRange_index = (i - 1) * numDocsMail;
                int getRange_count = Math.Min(numDocsMail, docs.Count - getRange_index);

                foreach (Attachment doc in docs.GetRange(getRange_index, getRange_count))
                {
                    mail.Attachments.Add(doc);
                }

                //Enviamos el mail
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        return Ok(correosTotales.Distinct());
    }

    //[HttpPost]
    //[Authorize]
    //public async Task<ActionResult> fn_SendMail_emailBienvenida_masivo()
    //{
    //    // Por idPersona o por idLavanderia/idCentroTrabajo (Comentado)
    //    //[FromODataUri] int? idLavanderia, [FromODataUri] int? idCentroTrabajo

    //    //var objLavanderia = db.tblLavanderia
    //    // .Where(x => x.idLavanderia.Equals(idLavanderia))
    //    // .Select(x => new
    //    // {
    //    //     x.idLavanderia,
    //    //     x.idZonaHoraria
    //    // }).FirstOrDefault();

    //    //var idZonaHoraria = idLavanderia != null ? (db.tblLavanderia.Where(x => x.idLavanderia == idLavanderia).FirstOrDefault().idZonaHoraria) : null;
    //    //var entityLocalizacion = idZonaHoraria != null ? (db.tblLocalizacion.Where(x => x.idZonaHoraria == idZonaHoraria).FirstOrDefault()) : null;
    //    //short? idLocalizacionLav = idCentroTrabajo == null && idLavanderia != null ? (entityLocalizacion != null ? entityLocalizacion.idLocalizacion : null) : 1;

    //    //var idPersonas = new[] { 3120, 2646, 449, 5746, 420, 228, 3603, 1423, 917, 930, 2072, 3007, 3657, 2126, 271, 5478, 2094, 2133, 2068, 2122, 5888, 1413, 2069, 2067, 2097, 2048, 270, 2135, 2106, 335, 5353, 5715, 672, 2066, 5632 };
    //    var idPersonas = new[] { 5632 }; // NICO

    //    List<tblPersona> tblPersona = db.tblPersona.Where(x => x.activo && !x.eliminado && idPersonas.Contains(x.idPersona)).ToList(); //&& (x.idPersona == 5632)
    //    foreach (tblPersona persona in tblPersona)
    //    {
    //        //short? idLocalizacion_persona = persona.tblUsuario.FirstOrDefault() != null ? persona.tblUsuario.FirstOrDefault().idLocalizacion : null;
    //        Notificar_emailBienvenida(persona.email, persona, 1);
    //    }

    //    return Ok(tblPersona);
    //}

    void DefineStyles(Document document)
    {
        // Get the predefined style Normal.
        Style style = document.Styles["Normal"];
        // Because all styles are derived from Normal, the next line changes the 
        // font of the whole document. Or, more exactly, it changes the font of
        // all styles and paragraphs that do not redefine the font.
        style.Font.Name = "Verdana";

        style = document.Styles[StyleNames.Header];
        style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

        style = document.Styles[StyleNames.Footer];
        style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

        // Create a new style called Table based on style Normal
        style = document.Styles.AddStyle("Table", "Normal");
        style.Font.Name = "Verdana";
        style.Font.Name = "Times New Roman";
        style.Font.Size = 9;

        // Create a new style called Reference based on style Normal
        style = document.Styles.AddStyle("Reference", "Normal");
        style.ParagraphFormat.SpaceBefore = "5mm";
        style.ParagraphFormat.SpaceAfter = "5mm";
        style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
    }

    void addDataParagraph(dynamic container, string field, string value)
    {
        Paragraph paragraph = container.AddParagraph();
        paragraph.Style = "Reference";
        paragraph.AddFormattedText(field, TextFormat.Bold);
        paragraph.AddTab();
        paragraph.AddText(value);
    }

    void addImagenNoDisponibleParagraph(dynamic container, int space)
    {
        Paragraph paragraph = container.AddParagraph();
        paragraph.Style = "Reference";
        paragraph.AddSpace(space);
        paragraph.AddFormattedText("IMAGEN NO DISPONIBLE");
    }

    void addFoto(Section section, string field, string value)
    {
        Paragraph paragraph = section.AddParagraph();
        paragraph.Style = "Reference";
        paragraph.AddFormattedText(field, TextFormat.Bold);

        MigraDoc.DocumentObjectModel.Shapes.Image imagePerfil = section.AddImage(value);
        imagePerfil.Width = "12cm";
        imagePerfil.LockAspectRatio = true;
        imagePerfil.Top = ShapePosition.Top;
        imagePerfil.Left = ShapePosition.Left;

        section.AddParagraph().Format.SpaceAfter = "1.5cm";
    }

    async Task<string?> ActualizarIBANA3(tblPersona persona, int companyCode, string IBAN)
    {
        if (persona.codigoGestoria == null)
        {
            return "errorActualizarIBANA3";
        }

        try
        {
            IBAN = IBAN.Replace(" ", "").ToUpper();

            var response_getBankaccounts = await a3innuva.EmployeesBankaccounts.Get_Bankaccounts((int)companyCode, persona.codigoGestoria);
            List<EmployeesBankaccounts_Get_Bankaccounts> bankAccountCodes = await A3innuvaUtils.DeserializeResponseAsync<List<EmployeesBankaccounts_Get_Bankaccounts>>(response_getBankaccounts);

            var mainAccountCode = bankAccountCodes.FirstOrDefault(cb => cb.isMainAccount)?.bankAccountCode;

            if (mainAccountCode != null)
            {
                var response_getBankaccount = await a3innuva.EmployeesBankaccounts.Get_Bankaccount((int)companyCode, persona.codigoGestoria, mainAccountCode);
                EmployeesBankaccounts_Get_Bankaccount mainBankAccount = await A3innuvaUtils.DeserializeResponseAsync<EmployeesBankaccounts_Get_Bankaccount>(response_getBankaccount);

                if (IBAN == mainBankAccount.iban)
                {
                    return null;
                }
                else
                {
                    await a3innuva.EmployeesBankaccounts.Delete_Bankaccount((int)companyCode, persona.codigoGestoria, mainAccountCode);
                }
            }

            if (IBAN.Length != 24)
            {
                return "errorActualizarIBANA3";
            }

            var entityIABN = IBAN.Substring(4, 4);
            var agency = IBAN.Substring(8, 4);
            var digitControl = IBAN.Substring(12, 2);
            var account = IBAN.Substring(14, 10);

            int idPersona = persona.idPersona;

            EmployeesBankaccounts_Post_Bankaccounts newBankAccount = new()
            {
                entity = entityIABN,
                agency = agency,
                digitControl = digitControl,
                account = account,
                iban = IBAN,
                holder = $"{persona.nombre.ToUpper()} {(persona.apellidos ?? "").ToUpper()}",
                isMainAccount = true,
                distributionAmount = 0,
                distributionPercentage = 0,
                bic = "",
            };

            await a3innuva.EmployeesBankaccounts.Post_Bankaccounts((int)companyCode, persona.codigoGestoria, newBankAccount);
        }
        catch
        {
            return "errorActualizarIBANA3";
        }

        return null;
    }

    async Task<string?> ActualizarSalarioBrutoMensualA3(tblPersona persona, int companyCode, decimal value)
    {
        if (persona.codigoGestoria == null)
        {
            return "errorPersonaSinCodigoGestoria_A3";
        }

        try
        {
            await ManejarVidaLaboralA3(persona, companyCode);

            HttpResponseMessage response_getSalaryadjustments;

            try
            {
                response_getSalaryadjustments = await a3innuva.EmployeesSalary.Get_Salaryadjustments(companyCode, persona.codigoGestoria);
            }
            catch // La llamada devulve BadRequest para las personas que no tienen salario bruto pactado aunque companyCode y employeeCode sean validos.
            {
                return null;
            }

            var salaryadjustments = await A3innuvaUtils.DeserializeResponseAsync<EmployeesSalary_Get_Salaryadjustments>(response_getSalaryadjustments);

            if (salaryadjustments.amount == value)
            {
                return null;
            }

            EmployeesSalary_Put_Salaryadjustments newSalaryadjustments = new()
            {
                amount = value,
                liquidationType = salaryadjustments.liquidationType,
                excess = salaryadjustments.excess,
                extraPayments = salaryadjustments.extraPayments,
                excludedConcepts = salaryadjustments.excludedConcepts,
                taxationQuote = salaryadjustments.taxationQuote,
                indicators = salaryadjustments.indicators,
            };

            newSalaryadjustments.indicators.partialTime = false;

            await a3innuva.EmployeesSalary.Put_Salaryadjustments(companyCode, persona.codigoGestoria, newSalaryadjustments);
        }
        catch
        {
            return "errorActualizarSalarioBrutoMensualA3";
        }

        return null;
    }

    async Task<string?> ActualizarDatosSalarialesA3(tblPersona persona, int companyCode, List<(string datoSalarialField, decimal value)> cambiosDatosSalariales)
    {

        if (persona.codigoGestoria == null)
        {
            return "errorPersonaSinCodigoGestoria_A3";
        }

        try
        {
            await ManejarVidaLaboralA3(persona, companyCode);

            var employeeConcepts = await gn.GetAllEmployeeConcepts(companyCode, persona.codigoGestoria);

            foreach (var (datoSalarialField, value) in cambiosDatosSalariales)
            {
                map_datoSalarial_conceptCode.TryGetValue(datoSalarialField, out int conceptCode);

                if (conceptCode == (int)conceptsCodes.AcuerdoNCPolarier)
                {
                    conceptCode = A3innuvaUtils.GetAcuerdoNC(companyCode);
                }

                if (employeeConcepts.Any(ec => ec.conceptCode == conceptCode && ec.amount != value))
                {
                    await a3innuva.EmployeesConcepts.Put_Amount(companyCode, persona.codigoGestoria, conceptCode.ToString(), value);
                }
                else if (!employeeConcepts.Any(ec => ec.conceptCode == conceptCode) && value != 0)
                {
                    return "errorActualizarConceptoSalarialA3_sinConceptoNA3";
                }
            }
        }
        catch
        {
            return "errorActualizarConceptoSalarialA3";
        }

        return null;
    }

    async Task<string?> ActualizarWorkplaceA3(tblPersona persona, int companyCode, int? newIdAdmCentroCoste, int? newIdAdmElementoPEP)
    {
        string? workplaceCode = null;

        if (persona.codigoGestoria == null)
        {
            return "errorPersonaSinCodigoGestoria_A3";
        }

        try
        {
            if (newIdAdmCentroCoste == null && newIdAdmElementoPEP == null)
            {
                return "errorActualizarCentroTrabajoA3";
            }

            if (newIdAdmCentroCoste != null)
            {
                var centroTrabajo = db.tblAdmCentroCoste.FirstOrDefault(cc => cc.idAdmCentroCoste == newIdAdmCentroCoste);

                if (centroTrabajo == null)
                {
                    return "errorActualizarCentroTrabajoA3";
                }

                workplaceCode = centroTrabajo.workplaceCode_A3;

            }

            if (newIdAdmElementoPEP != null)
            {
                var elementoPEP = db.tblAdmElementoPEP.FirstOrDefault(cc => cc.idAdmElementoPEP == newIdAdmElementoPEP);

                if (elementoPEP == null)
                {
                    return "errorActualizarCentroTrabajoA3";
                }

                workplaceCode = elementoPEP.workplaceCode_A3;
            }

            if (workplaceCode == null)
            {
                return "errorActualizarCentroTrabajoA3";
            }

            var newWorkplaceCode = new
            {
                workplaceCode
            };

            await a3innuva.Employees.Put_Identification((int)companyCode, persona.codigoGestoria, newWorkplaceCode);
        }
        catch
        {
            return "errorActualizarCentroTrabajoA3";
        }

        return null;
    }

    async Task ManejarVidaLaboralA3(tblPersona persona, int companyCode)
    {
        var hoy = DateTime.Today;
        var fechaFinVigencia = new DateTime(hoy.Year, hoy.Month, 1).AddDays(-1);

        var response_getLaborLife = await a3innuva.EmployeesLaborLife.Get_LabourLife(companyCode, persona.codigoGestoria);
        var laborLife = await A3innuvaUtils.DeserializeResponseAsync<List<EmployeesLaborLife_Get_LabourLife>>(response_getLaborLife);

        if (laborLife.Any(ll => ll.date.Date > fechaFinVigencia.Date))
        {
            return;
        }
        else if (!laborLife.Any(ll => ll.date.Date == fechaFinVigencia.Date))
        {
            var motive = "Cambio de conceptos salariales desde MyPolarier";

            EmployeesLaborLife_Post_CreateLaborlife newLaborLife = new()
            {
                periodStartDate = fechaFinVigencia,
                motive = motive,
                isIRPFChange = false,
                isPeriodChange = true,
                motiveAll = motive,
                indCreta = false
            };

            await a3innuva.EmployeesLaborLife.Post_CreateLaborlife(companyCode, persona.codigoGestoria, newLaborLife);
        }
    }

    [EnableQuery]
    [HttpGet]
    private bool Notificar_emailBienvenida(string email, tblPersona persona, short idLocalizacion)
    {
        try
        {
            string urlAndroid = "https://play.google.com/store/apps/details?id=com.Polarier.Polarier";
            string urlIos = "https://apps.apple.com/es/app/polarier/id1622789099";

            string urlVideoPrimerosPasos = "https://polarier-my.sharepoint.com/:v:/p/aferreira/EayVklOGvopLggOHShUPIyEBNsy6wsyRGKMSK7dJZDevug?e=r0uWer";
            string urlVideoFuncionalidades = "https://polarier-my.sharepoint.com/:v:/p/aferreira/EeMc5KG13NVHnLwLHH5J9NQBUap8_2vzwYZZb2Zc1bAr_w?e=u25SoW";

            if (idLocalizacion == 1 || idLocalizacion == 2) //ESPAÑA
            {
                urlIos = "https://apps.apple.com/es/app/polarier/id1622789099";
            }
            else if (idLocalizacion == 3) // REPÚBLICA DOMINICANA
            {
                urlIos = "https://apps.apple.com/do/app/polarier/id1622789099";
            }
            else if (idLocalizacion == 4) // MÉXICO
            {
                urlIos = "https://apps.apple.com/mx/app/polarier/id1622789099";
            }

            // Generamos el paquete SMTP con la configuracion del servidor y las credenciales de acceso
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "outlook.office365.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential("mypolarier@polarier.com", "Vog45080");

            // Generamos el mail a enviar
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("mypolarier@polarier.com", "No Reply MyPolarier");

            if (Utils.isProduccion())
            {
                mail.To.Add(email);
            }
            else
            {
                mail.To.Add("nseco@polarier.com");
            }

            mail.Subject = "Hola, " + persona.nombre + ". Bienvenido/a a Polarier.";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            mail.Body = string.Empty;

            using (StreamReader reader = new StreamReader(@"./Pages/EmailBienvenida.html"))
            {
                mail.Body = reader.ReadToEnd();
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = "Media/Imagenes";
            string fullPath = Path.Combine(currentDirectory, path, "logoLoveYourLinen.png");

            Attachment objAttach = new Attachment(fullPath);
            mail.Attachments.Add(objAttach);

            mail.Body = mail.Body.Replace("@@nombrePersona", persona.nombre);
            mail.Body = mail.Body.Replace("@@urlAndroid", urlAndroid);
            mail.Body = mail.Body.Replace("@@urlIos", urlIos);

            mail.Body = mail.Body.Replace("@@urlVideoPrimerosPasos", urlVideoPrimerosPasos);
            mail.Body = mail.Body.Replace("@@urlVideoFuncionalidades", urlVideoFuncionalidades);

            mail.Body = mail.Body.Replace("@@img", objAttach.ContentId);


            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            //Enviamos el mail
            smtpClient.Send(mail);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
