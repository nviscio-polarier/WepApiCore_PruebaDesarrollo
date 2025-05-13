using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class;
using WebApiCore.Class.Proyectos.MyPolarier.RRHH;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH
{
    public class LlamamientoController : ODataController
    {
        private readonly bdERP db;
        public LlamamientoController(bdERP context)
        {
            db = context;
        }

        [HttpGet("odata/MyPolarier/RRHH/tblLlamamiento")]
        [Authorize]
        [EnableQuery]
        public async Task<ActionResult> Get()
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            var objUsuario = db.tblUsuario.Include(x => x.tblTipoTrabajoNUsuario).Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
            if (objUsuario == null)
                return BadRequest();

            var tblTipoTrabajoNUsuario = db.tblTipoTrabajoNUsuario.Where(x => x.idUsuario.Equals(idUsuario));

            var idsLlamamiento = db.tblLlamamiento.Where(llam => 
                (
                    (llam.idLavanderiaNavigation != null && llam.idLavanderiaNavigation.idUsuario.Select(y => y.idUsuario).Contains(idUsuario)) ||
                    (llam.idCentroTrabajoNavigation != null && llam.idCentroTrabajoNavigation.idUsuario.Select(y => y.idUsuario).Contains(idUsuario))
                ) &&
                (
                    objUsuario.enableDatosRRHH ||
                    (
                        tblTipoTrabajoNUsuario.Count() == 0 || (
                            tblTipoTrabajoNUsuario.Count() > 0 &&
                            tblTipoTrabajoNUsuario.FirstOrDefault(x => x.idLavanderia == llam.idLavanderia && x.idTipoTrabajo == llam.idTipoTrabajo) != null
                        )
                    )
                )
            ).Select(x => x.idLlamamiento);

            return Ok(db.tblLlamamiento.Where(x => idsLlamamiento.Contains(x.idLlamamiento) && x.activo == true).Select(x => new
            {
                x.idLlamamiento,
                x.idLavanderia,
                x.idCentroTrabajo,
                x.fechaIni,
                x.idTipoTrabajo,
                x.idCategoriaInterna,
                x.idTurno,
                x.idFormatoDiasLibres,
                x.activo,
                x.codigoLlamamiento,
                x.idPersona,
                x.idTipoContrato,
                x.numDiasPeriodoPrueba,
                x.isNuevaAlta,
                tblDiasLibresPersonal_Llamamiento = x.tblDiasLibresPersonal_Llamamiento.Select(x => new
                {
                    x.idDiaSemana,
                    x.idDiaMes,
                    x.numDia
                }),
                idPersonaNavigation = x.idPersonaNavigation != null ? new
                {
                    x.idPersonaNavigation.nombre,
                    x.idPersonaNavigation.apellidos,
                }:null,
                isAltaSolicitada = x.tblSolicitudAlta != null,
                isValidadoRRHH = x.tblSolicitudAlta != null ? x.tblSolicitudAlta.idEstadoSolicitudAlta == (byte)idsEstadoSolicitudAlta.Validado : false,
            }));
        }

        [HttpGet]
        [Authorize]
        [EnableQuery]
        public ActionResult GetPersonasDiscontinuas()
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            var idsPersona = Utils.selectPersonasVisibles(db, idUsuario, false);

            var tblPersona = db.tblPersona.Where(pers =>
                idsPersona.Contains(pers.idPersona) &&
                pers.eliminado == false && pers.activo == false &&
                pers.tblUsuario.Where(x => x.idLocalizacion == 1 || x.idLocalizacion == 2).Count() > 0 && // Solo españa, quitar cuando PersonalGeneral se utilice en mas paises
                (
                    pers.tblPersonaNTipoContrato.Count() == 0 || // No tiene contratos
                    (
                        pers.tblPersonaNTipoContrato.Count() > 0 &&
                        pers.tblPersonaNTipoContrato.OrderByDescending(x => x.fechaAltaContrato).First().fechaBajaContrato.Value.Year >= (DateTime.UtcNow.Year - 1) ||
                        pers.tblPersonaNTipoContrato.OrderByDescending(x => x.fechaAltaContrato).First().fechaAltaContrato > DateTime.UtcNow
                    )
                )
            ).Select(x => new PersonaLlamamiento
            {
                idPersona = x.idPersona,
                idLavanderia = x.idLavanderia,
                nombre = x.nombre,
                apellidos = x.apellidos,
                telefono = x.telefono,
                idCategoriaInterna = x.idCategoriaInterna,
                idTipoTrabajo = x.idTipoTrabajo,
                tblDatosSalariales = x.tblDatosSalariales,
                tblPersonaNTipoContrato = x.tblPersonaNTipoContrato.OrderByDescending(x => x.fechaAltaContrato).FirstOrDefault()
            });

            return Ok(tblPersona);
        }

    }
}
