using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblUsuario", Schema = "GestionInterna")]
    [Index("usuario", Name = "AK_usuario", IsUnique = true)]
    public partial class tblUsuario
    {
        public tblUsuario()
        {
            tblCalendarioPersonal = new HashSet<tblCalendarioPersonal>();
            tblComentarioNCuentaContable = new HashSet<tblComentarioNCuentaContable>();
            tblCuadrantePersonal = new HashSet<tblCuadrantePersonal>();
            tblDatosSalariales = new HashSet<tblDatosSalariales>();
            tblDocumentoNNomina = new HashSet<tblDocumentoNNomina>();
            tblDocumentoidUsuarioModificacionNavigation = new HashSet<tblDocumento>();
            tblDocumentoidUsuarioNavigation = new HashSet<tblDocumento>();
            tblEncuesta = new HashSet<tblEncuesta>();
            tblEstadoMovimientoRecambioNMovimientoRecambio = new HashSet<tblEstadoMovimientoRecambioNMovimientoRecambio>();
            tblEstadoNominaNNomina = new HashSet<tblEstadoNominaNNomina>();
            tblEstadoSolicitudAltaNSolicitudAlta = new HashSet<tblEstadoSolicitudAltaNSolicitudAlta>();
            tblFormularioNUsuario = new HashSet<tblFormularioNUsuario>();
            tblGestionRetiroidUsuarioNavigation = new HashSet<tblGestionRetiro>();
            tblGestionRetiroidUsuarioValidadorNavigation = new HashSet<tblGestionRetiro>();
            tblGrupoInventario_generico = new HashSet<tblGrupoInventario_generico>();
            tblHistoricoAsientoNomina = new HashSet<tblHistoricoAsientoNomina>();
            tblHistoricoAsientoNomina_MX = new HashSet<tblHistoricoAsientoNomina_MX>();
            tblHistoricoAsientoNomina_RD = new HashSet<tblHistoricoAsientoNomina_RD>();
            tblIncidenciaidUsuarioCreaNavigation = new HashSet<tblIncidencia>();
            tblIncidenciaidUsuarioRevisorNavigation = new HashSet<tblIncidencia>();
            tblJornada = new HashSet<tblJornada>();
            tblLecturaCarro = new HashSet<tblLecturaCarro>();
            tblLog = new HashSet<tblLog>();
            tblLogAcciones = new HashSet<tblLogAcciones>();
            tblLogAcciones_App = new HashSet<tblLogAcciones_App>();
            tblMantenimientoNMaquina = new HashSet<tblMantenimientoNMaquina>();
            tblMantenimientoPrev = new HashSet<tblMantenimientoPrev>();
            tblNomina = new HashSet<tblNomina>();
            tblNotificacion = new HashSet<tblNotificacion>();
            tblNotificaciones_TI = new HashSet<tblNotificaciones_TI>();
            tblParteTrabajo = new HashSet<tblParteTrabajo>();
            tblParteTransporte = new HashSet<tblParteTransporte>();
            tblPedidoidUsuarioCreadorNavigation = new HashSet<tblPedido>();
            tblPedidoidUsuarioPrepara_activoNavigation = new HashSet<tblPedido>();
            tblPersona = new HashSet<tblPersona>();
            tblPrendaNUsuarioNEntidad = new HashSet<tblPrendaNUsuarioNEntidad>();
            tblRecambioNMovimientoRecambio = new HashSet<tblRecambioNMovimientoRecambio>();
            tblRecoveryPassword = new HashSet<tblRecoveryPassword>();
            tblReparto = new HashSet<tblReparto>();
            tblRespuesta = new HashSet<tblRespuesta>();
            tblReunion = new HashSet<tblReunion>();
            tblSolicitudAbono = new HashSet<tblSolicitudAbono>();
            tblSolicitudAlta = new HashSet<tblSolicitudAlta>();
            tblTipoTrabajoNUsuario = new HashSet<tblTipoTrabajoNUsuario>();
            tblToken_Refresh = new HashSet<tblToken_Refresh>();
            tblToken_Refresh_Mobile = new HashSet<tblToken_Refresh_Mobile>();
            idAdmCentroCoste = new HashSet<tblAdmCentroCoste>();
            idAdmElementoPEP = new HashSet<tblAdmElementoPEP>();
            idCategoriaInterna = new HashSet<tblCategoriaInterna>();
            idCentroTrabajo = new HashSet<tblCentroTrabajo>();
            idEmpresaPolarier = new HashSet<tblEmpresasPolarier>();
            idEntidad = new HashSet<tblEntidad>();
            idLavanderia = new HashSet<tblLavanderia>();
            idPermiso = new HashSet<tblPermiso>();
        }

        [Key]
        public int idUsuario { get; set; }
        [StringLength(150)]
        public string? usuario { get; set; }
        [StringLength(50)]
        public string password { get; set; } = null!;
        public bool cambiaPassword { get; set; }
        public string? email { get; set; }
        public short idIdioma { get; set; }
        [StringLength(150)]
        public string? nombre { get; set; }
        public string? detallesCargo { get; set; }
        public short idCargo { get; set; }
        public bool refactura { get; set; }
        public bool importaEntidades { get; set; }
        public int? idFormularioInicio { get; set; }
        public int? idLavanderiaInicio { get; set; }
        public short? idLocalizacion { get; set; }
        public int? idCompañia { get; set; }
        public int? idTipoUsuario { get; set; }
        public DateTimeOffset? fechaCreacion { get; set; }
        public int? idPersona { get; set; }
        public bool isEliminado { get; set; }
        public bool enableFullScreen { get; set; }
        public bool enableDatosRRHH { get; set; }
        public int? subtipoUsuario { get; set; }
        public int? idAplicacionInicial { get; set; }
        public string? notificationToken { get; set; }
        public bool isDepartamentoControl { get; set; }
        public bool enableDatosSalarialesOficina { get; set; }

        [ForeignKey("idAplicacionInicial")]
        [InverseProperty("tblUsuario")]
        public virtual tblAplicacion? idAplicacionInicialNavigation { get; set; }
        [ForeignKey("idCargo")]
        [InverseProperty("tblUsuario")]
        public virtual tblCargo idCargoNavigation { get; set; } = null!;
        [ForeignKey("idCompañia")]
        [InverseProperty("tblUsuario")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idFormularioInicio")]
        [InverseProperty("tblUsuario")]
        public virtual tblFormulario? idFormularioInicioNavigation { get; set; }
        [ForeignKey("idIdioma")]
        [InverseProperty("tblUsuario")]
        public virtual tblIdioma idIdiomaNavigation { get; set; } = null!;
        [ForeignKey("idLavanderiaInicio")]
        [InverseProperty("tblUsuario")]
        public virtual tblLavanderia? idLavanderiaInicioNavigation { get; set; }
        [ForeignKey("idLocalizacion")]
        [InverseProperty("tblUsuario")]
        public virtual tblLocalizacion? idLocalizacionNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblUsuario")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idTipoUsuario")]
        [InverseProperty("tblUsuario")]
        public virtual tblTipoUsuario? idTipoUsuarioNavigation { get; set; }
        [InverseProperty("idUsuario_validacionNavigation")]
        public virtual ICollection<tblCalendarioPersonal> tblCalendarioPersonal { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblComentarioNCuentaContable> tblComentarioNCuentaContable { get; set; }
        [InverseProperty("idUsuario_validacionNavigation")]
        public virtual ICollection<tblCuadrantePersonal> tblCuadrantePersonal { get; set; }
        [InverseProperty("fechaAntiguedad_idUsuario_modNavigation")]
        public virtual ICollection<tblDatosSalariales> tblDatosSalariales { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblDocumentoNNomina> tblDocumentoNNomina { get; set; }
        [InverseProperty("idUsuarioModificacionNavigation")]
        public virtual ICollection<tblDocumento> tblDocumentoidUsuarioModificacionNavigation { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblDocumento> tblDocumentoidUsuarioNavigation { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblEncuesta> tblEncuesta { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblEstadoMovimientoRecambioNMovimientoRecambio> tblEstadoMovimientoRecambioNMovimientoRecambio { get; set; }
        [InverseProperty("idUsuario_validaNavigation")]
        public virtual ICollection<tblEstadoNominaNNomina> tblEstadoNominaNNomina { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblEstadoSolicitudAltaNSolicitudAlta> tblEstadoSolicitudAltaNSolicitudAlta { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblFormularioNUsuario> tblFormularioNUsuario { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblGestionRetiro> tblGestionRetiroidUsuarioNavigation { get; set; }
        [InverseProperty("idUsuarioValidadorNavigation")]
        public virtual ICollection<tblGestionRetiro> tblGestionRetiroidUsuarioValidadorNavigation { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblGrupoInventario_generico> tblGrupoInventario_generico { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina> tblHistoricoAsientoNomina { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina_MX> tblHistoricoAsientoNomina_MX { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina_RD> tblHistoricoAsientoNomina_RD { get; set; }
        [InverseProperty("idUsuarioCreaNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidenciaidUsuarioCreaNavigation { get; set; }
        [InverseProperty("idUsuarioRevisorNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidenciaidUsuarioRevisorNavigation { get; set; }
        [InverseProperty("idUsuario_validacionNavigation")]
        public virtual ICollection<tblJornada> tblJornada { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblLecturaCarro> tblLecturaCarro { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblLog> tblLog { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblLogAcciones> tblLogAcciones { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblLogAcciones_App> tblLogAcciones_App { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblMantenimientoNMaquina> tblMantenimientoNMaquina { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblMantenimientoPrev> tblMantenimientoPrev { get; set; }
        [InverseProperty("idUsuario_modificaNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblNotificacion> tblNotificacion { get; set; }
        [InverseProperty("idUsuarioResponsableNavigation")]
        public virtual ICollection<tblNotificaciones_TI> tblNotificaciones_TI { get; set; }
        [InverseProperty("idUsuarioCreaNavigation")]
        public virtual ICollection<tblParteTrabajo> tblParteTrabajo { get; set; }
        [InverseProperty("idUsuarioResponsableNavigation")]
        public virtual ICollection<tblParteTransporte> tblParteTransporte { get; set; }
        [InverseProperty("idUsuarioCreadorNavigation")]
        public virtual ICollection<tblPedido> tblPedidoidUsuarioCreadorNavigation { get; set; }
        [InverseProperty("idUsuarioPrepara_activoNavigation")]
        public virtual ICollection<tblPedido> tblPedidoidUsuarioPrepara_activoNavigation { get; set; }
        [InverseProperty("idUsuario_validacion_nominaNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblPrendaNUsuarioNEntidad> tblPrendaNUsuarioNEntidad { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblRecambioNMovimientoRecambio> tblRecambioNMovimientoRecambio { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblRecoveryPassword> tblRecoveryPassword { get; set; }
        [InverseProperty("idUsuarioPreparaNavigation")]
        public virtual ICollection<tblReparto> tblReparto { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblRespuesta> tblRespuesta { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblReunion> tblReunion { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblSolicitudAbono> tblSolicitudAbono { get; set; }
        [InverseProperty("idUsuario_validacionNavigation")]
        public virtual ICollection<tblSolicitudAlta> tblSolicitudAlta { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblTipoTrabajoNUsuario> tblTipoTrabajoNUsuario { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblToken_Refresh> tblToken_Refresh { get; set; }
        [InverseProperty("idUsuarioNavigation")]
        public virtual ICollection<tblToken_Refresh_Mobile> tblToken_Refresh_Mobile { get; set; }

        [ForeignKey("idUsuario")]
        [InverseProperty("idUsuario")]
        public virtual ICollection<tblAdmCentroCoste> idAdmCentroCoste { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("idUsuario")]
        public virtual ICollection<tblAdmElementoPEP> idAdmElementoPEP { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("idUsuario")]
        public virtual ICollection<tblCategoriaInterna> idCategoriaInterna { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("idUsuario")]
        public virtual ICollection<tblCentroTrabajo> idCentroTrabajo { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("idUsuario")]
        public virtual ICollection<tblEmpresasPolarier> idEmpresaPolarier { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("idUsuario")]
        public virtual ICollection<tblEntidad> idEntidad { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("idUsuario")]
        public virtual ICollection<tblLavanderia> idLavanderia { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("idUsuario")]
        public virtual ICollection<tblPermiso> idPermiso { get; set; }
    }
}
