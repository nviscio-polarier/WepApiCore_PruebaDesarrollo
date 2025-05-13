using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersona", Schema = "RRHH")]
    [Index("idLavanderia", "idCentroTrabajo", Name = "IX_tblPersona_idLavanderia_idCentroTrabajo")]
    [Index("idUsuario_validacion_nomina", Name = "IX_tblPersona_idUsuario_validacion_nomina")]
    [Index("idCentroTrabajo", Name = "tblPersona_idCentroTrabajo")]
    public partial class tblPersona
    {
        public tblPersona()
        {
            tblAlmacenNInventario = new HashSet<tblAlmacenNInventario>();
            tblAlmacenRecambiosNPersona = new HashSet<tblAlmacenRecambiosNPersona>();
            tblBalanceHoras = new HashSet<tblBalanceHoras>();
            tblBalanceHorasExtra = new HashSet<tblBalanceHorasExtra>();
            tblCalendarioPersonal = new HashSet<tblCalendarioPersonal>();
            tblComunicadoNPersona = new HashSet<tblComunicadoNPersona>();
            tblControlAcceso = new HashSet<tblControlAcceso>();
            tblCuadrantePersonal = new HashSet<tblCuadrantePersonal>();
            tblDiasCuadrante = new HashSet<tblDiasCuadrante>();
            tblDiasLibresPersonal = new HashSet<tblDiasLibresPersonal>();
            tblDocumento = new HashSet<tblDocumento>();
            tblEventoPersona = new HashSet<tblEventoPersona>();
            tblHistoricoNominas = new HashSet<tblHistoricoNominas>();
            tblIncidenciaidUsuarioAfectaNavigation = new HashSet<tblIncidencia>();
            tblIncidenciaidUsuarioResponsableNavigation = new HashSet<tblIncidencia>();
            tblJornada = new HashSet<tblJornada>();
            tblJornadaPersona = new HashSet<tblJornadaPersona>();
            tblLibreMensual = new HashSet<tblLibreMensual>();
            tblLibreSemanal = new HashSet<tblLibreSemanal>();
            tblLlamamiento = new HashSet<tblLlamamiento>();
            tblLogsToken = new HashSet<tblLogsToken>();
            tblNomina = new HashSet<tblNomina>();
            tblNomina_MX = new HashSet<tblNomina_MX>();
            tblNomina_RD = new HashSet<tblNomina_RD>();
            tblParteTransporte = new HashSet<tblParteTransporte>();
            tblPedidosExtra = new HashSet<tblPedidosExtra>();
            tblPersonaCoste = new HashSet<tblPersonaCoste>();
            tblPersonaNAreaNLavanderia = new HashSet<tblPersonaNAreaNLavanderia>();
            tblPersonaNMaquina = new HashSet<tblPersonaNMaquina>();
            tblPersonaNTipoContrato = new HashSet<tblPersonaNTipoContrato>();
            tblPersonaTokens = new HashSet<tblPersonaTokens>();
            tblPersona_PeticionCambioDatos = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersonasNParte = new HashSet<tblPersonasNParte>();
            tblPersonasNParte1 = new HashSet<tblPersonasNParte1>();
            tblRepartoOffice = new HashSet<tblRepartoOffice>();
            tblRevision = new HashSet<tblRevision>();
            tblSalidaRepartoidConductorNavigation = new HashSet<tblSalidaReparto>();
            tblSalidaRepartoidEstibador1Navigation = new HashSet<tblSalidaReparto>();
            tblSalidaRepartoidEstibador2Navigation = new HashSet<tblSalidaReparto>();
            tblTaquilla_movimiento = new HashSet<tblTaquilla_movimiento>();
            tblTareaPersonaDia = new HashSet<tblTareaPersonaDia>();
            tblUsuario = new HashSet<tblUsuario>();
            tblVideoNPersonaidPersonaNavigation = new HashSet<tblVideoNPersona>();
            tblVideoNPersonaidPersonaRemitenteNavigation = new HashSet<tblVideoNPersona>();
            idLicenciaConducir = new HashSet<tblLicenciaConducir>();
            idMantenimientoPrev = new HashSet<tblMantenimientoPrev>();
            idParteTransporte = new HashSet<tblParteTransporte>();
        }

        [Key]
        public int idPersona { get; set; }
        public int? idLavanderia { get; set; }
        public int? idCentroTrabajo { get; set; }
        public string nombre { get; set; } = null!;
        public string? apellidos { get; set; }
        public string? email { get; set; }
        public short? prefijoTelefonico { get; set; }
        public string? telefono { get; set; }
        public string? calle { get; set; }
        public string? numDomicilio { get; set; }
        public string? piso { get; set; }
        public string? puerta { get; set; }
        public string? codigoPostal { get; set; }
        public string? localidad { get; set; }
        public short? idComunidadAutonoma { get; set; }
        public int? idPais { get; set; }
        public int? idFotoPerfil { get; set; }
        [StringLength(2)]
        public string? nacionalidad { get; set; }
        public byte? idTipoDocumentoIdentidad { get; set; }
        public string? numDocumentoIdentidad { get; set; }
        [Column(TypeName = "date")]
        public DateTime? caducidadDocumentoIdentidad { get; set; }
        public int? idFotoDocumentoIdentidad_A { get; set; }
        public int? idFotoDocumentoIdentidad_B { get; set; }
        public string? NAF { get; set; }
        public int? idFotoNAF { get; set; }
        public string? IBAN { get; set; }
        public int? idFotoIBAN { get; set; }
        public byte? idGenero { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaNacimiento { get; set; }
        public byte? idEstadoCivil { get; set; }
        public string? numHijos { get; set; }
        public byte? idNivelEstudios { get; set; }
        public byte? idDiscapacidad { get; set; }
        public byte? idTallaAlfa_Camiseta { get; set; }
        public string? tallaPantalon { get; set; }
        public short? idCategoria { get; set; }
        public int? idTurno { get; set; }
        public byte? idTipoTrabajo { get; set; }
        public int? idTipoContrato { get; set; }
        public int? tipoLibre { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horasDiarias { get; set; }
        public string? observaciones { get; set; }
        public string? codigoRFID { get; set; }
        public bool activo { get; set; }
        public bool eliminado { get; set; }
        public bool? avisoLegalAceptado { get; set; }
        public string? numSegSocial { get; set; }
        public string? poblacion { get; set; }
        public string? direccion { get; set; }
        public string? codigoGestoria { get; set; }
        public string? observacionesLaborales { get; set; }
        public bool isCodigoGestoriaValidado { get; set; }
        [Column(TypeName = "date")]
        public DateTime? codigoRFID_fecha { get; set; }
        public int? idCategoriaInterna { get; set; }
        [Required]
        public bool? isIBANSolicitado { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public byte? idFormatoDiasLibres { get; set; }
        public int? idUsuario_validacion_nomina { get; set; }
        public string? nombre_tutor { get; set; }
        public string? apellidos_tutor { get; set; }
        public byte? idTipoDocumentoIdentidad_tutor { get; set; }
        public string? numDocumentoIdentidad_tutor { get; set; }
        public int? idFotoDemandanteEmpleo { get; set; }
        public int? idDocCertificadoDiscapacidad { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public short? prefijoTelefonicoEmpresa { get; set; }
        public string? telefonoEmpresa { get; set; }
        public int? id_VIPS { get; set; }
        public int? idAdmCuentaContable_Salario { get; set; }
        public int? idAdmCuentaContable_SSEmpresa { get; set; }
        public bool? vehiculoPropio { get; set; }
        public int? idDocumentoLicenciaConducir { get; set; }
        public bool? bonificacionDiscapacidad { get; set; }
        public int? id_MX { get; set; }
        public string? idPersona_webfleet { get; set; }
        public bool isCuentasTrabajoBloqueadas { get; set; }
        public bool isCentroCostePEPBloqueadas { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblPersona")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_SSEmpresa")]
        [InverseProperty("tblPersonaidAdmCuentaContable_SSEmpresaNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SSEmpresaNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_Salario")]
        [InverseProperty("tblPersonaidAdmCuentaContable_SalarioNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SalarioNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblPersona")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idCategoriaInterna")]
        [InverseProperty("tblPersona")]
        public virtual tblCategoriaInterna? idCategoriaInternaNavigation { get; set; }
        [ForeignKey("idCategoria")]
        [InverseProperty("tblPersona")]
        public virtual tblCategoria? idCategoriaNavigation { get; set; }
        [ForeignKey("idCentroTrabajo")]
        [InverseProperty("tblPersona")]
        public virtual tblCentroTrabajo? idCentroTrabajoNavigation { get; set; }
        [ForeignKey("idComunidadAutonoma")]
        [InverseProperty("tblPersona")]
        public virtual tblComunidadAutonoma? idComunidadAutonomaNavigation { get; set; }
        [ForeignKey("idDiscapacidad")]
        [InverseProperty("tblPersona")]
        public virtual tblDiscapacidad? idDiscapacidadNavigation { get; set; }
        [ForeignKey("idDocCertificadoDiscapacidad")]
        [InverseProperty("tblPersonaidDocCertificadoDiscapacidadNavigation")]
        public virtual tblDocumento? idDocCertificadoDiscapacidadNavigation { get; set; }
        [ForeignKey("idDocumentoLicenciaConducir")]
        [InverseProperty("tblPersonaidDocumentoLicenciaConducirNavigation")]
        public virtual tblDocumento? idDocumentoLicenciaConducirNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblPersona")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [ForeignKey("idEstadoCivil")]
        [InverseProperty("tblPersona")]
        public virtual tblEstadoCivil? idEstadoCivilNavigation { get; set; }
        [ForeignKey("idFormatoDiasLibres")]
        [InverseProperty("tblPersona")]
        public virtual tblFormatoDiasLibres? idFormatoDiasLibresNavigation { get; set; }
        [ForeignKey("idFotoDemandanteEmpleo")]
        [InverseProperty("tblPersonaidFotoDemandanteEmpleoNavigation")]
        public virtual tblDocumento? idFotoDemandanteEmpleoNavigation { get; set; }
        [ForeignKey("idFotoDocumentoIdentidad_A")]
        [InverseProperty("tblPersonaidFotoDocumentoIdentidad_ANavigation")]
        public virtual tblDocumento? idFotoDocumentoIdentidad_ANavigation { get; set; }
        [ForeignKey("idFotoDocumentoIdentidad_B")]
        [InverseProperty("tblPersonaidFotoDocumentoIdentidad_BNavigation")]
        public virtual tblDocumento? idFotoDocumentoIdentidad_BNavigation { get; set; }
        [ForeignKey("idFotoIBAN")]
        [InverseProperty("tblPersonaidFotoIBANNavigation")]
        public virtual tblDocumento? idFotoIBANNavigation { get; set; }
        [ForeignKey("idFotoNAF")]
        [InverseProperty("tblPersonaidFotoNAFNavigation")]
        public virtual tblDocumento? idFotoNAFNavigation { get; set; }
        [ForeignKey("idFotoPerfil")]
        [InverseProperty("tblPersonaidFotoPerfilNavigation")]
        public virtual tblDocumento? idFotoPerfilNavigation { get; set; }
        [ForeignKey("idGenero")]
        [InverseProperty("tblPersona")]
        public virtual tblGenero? idGeneroNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPersona")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idNivelEstudios")]
        [InverseProperty("tblPersona")]
        public virtual tblNivelEstudios? idNivelEstudiosNavigation { get; set; }
        [ForeignKey("idPais")]
        [InverseProperty("tblPersona")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [ForeignKey("idTallaAlfa_Camiseta")]
        [InverseProperty("tblPersona")]
        public virtual tblTallaAlfa? idTallaAlfa_CamisetaNavigation { get; set; }
        [ForeignKey("idTipoDocumentoIdentidad")]
        [InverseProperty("tblPersonaidTipoDocumentoIdentidadNavigation")]
        public virtual tblTipoDocumentoIdentidad? idTipoDocumentoIdentidadNavigation { get; set; }
        [ForeignKey("idTipoDocumentoIdentidad_tutor")]
        [InverseProperty("tblPersonaidTipoDocumentoIdentidad_tutorNavigation")]
        public virtual tblTipoDocumentoIdentidad? idTipoDocumentoIdentidad_tutorNavigation { get; set; }
        [ForeignKey("idTipoTrabajo")]
        [InverseProperty("tblPersona")]
        public virtual tblTipoTrabajo? idTipoTrabajoNavigation { get; set; }
        [ForeignKey("idTurno")]
        [InverseProperty("tblPersona")]
        public virtual tblTurno? idTurnoNavigation { get; set; }
        [ForeignKey("idUsuario_validacion_nomina")]
        [InverseProperty("tblPersona")]
        public virtual tblUsuario? idUsuario_validacion_nominaNavigation { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual tblDatosSalariales tblDatosSalariales { get; set; } = null!;
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblAlmacenNInventario> tblAlmacenNInventario { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblAlmacenRecambiosNPersona> tblAlmacenRecambiosNPersona { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblBalanceHoras> tblBalanceHoras { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblBalanceHorasExtra> tblBalanceHorasExtra { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblCalendarioPersonal> tblCalendarioPersonal { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblComunicadoNPersona> tblComunicadoNPersona { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblControlAcceso> tblControlAcceso { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblCuadrantePersonal> tblCuadrantePersonal { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblDiasCuadrante> tblDiasCuadrante { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblDiasLibresPersonal> tblDiasLibresPersonal { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblDocumento> tblDocumento { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblEventoPersona> tblEventoPersona { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblHistoricoNominas> tblHistoricoNominas { get; set; }
        [InverseProperty("idUsuarioAfectaNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidenciaidUsuarioAfectaNavigation { get; set; }
        [InverseProperty("idUsuarioResponsableNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidenciaidUsuarioResponsableNavigation { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblJornada> tblJornada { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblJornadaPersona> tblJornadaPersona { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblLibreMensual> tblLibreMensual { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblLibreSemanal> tblLibreSemanal { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblLlamamiento> tblLlamamiento { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblLogsToken> tblLogsToken { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblNomina_MX> tblNomina_MX { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblNomina_RD> tblNomina_RD { get; set; }
        [InverseProperty("idPersonaResponsableNavigation")]
        public virtual ICollection<tblParteTransporte> tblParteTransporte { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblPedidosExtra> tblPedidosExtra { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblPersonaCoste> tblPersonaCoste { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblPersonaNAreaNLavanderia> tblPersonaNAreaNLavanderia { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblPersonaNMaquina> tblPersonaNMaquina { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblPersonaNTipoContrato> tblPersonaNTipoContrato { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblPersonaTokens> tblPersonaTokens { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatos { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblPersonasNParte> tblPersonasNParte { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblPersonasNParte1> tblPersonasNParte1 { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblRepartoOffice> tblRepartoOffice { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblRevision> tblRevision { get; set; }
        [InverseProperty("idConductorNavigation")]
        public virtual ICollection<tblSalidaReparto> tblSalidaRepartoidConductorNavigation { get; set; }
        [InverseProperty("idEstibador1Navigation")]
        public virtual ICollection<tblSalidaReparto> tblSalidaRepartoidEstibador1Navigation { get; set; }
        [InverseProperty("idEstibador2Navigation")]
        public virtual ICollection<tblSalidaReparto> tblSalidaRepartoidEstibador2Navigation { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblTaquilla_movimiento> tblTaquilla_movimiento { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblTareaPersonaDia> tblTareaPersonaDia { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblUsuario> tblUsuario { get; set; }
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblVideoNPersona> tblVideoNPersonaidPersonaNavigation { get; set; }
        [InverseProperty("idPersonaRemitenteNavigation")]
        public virtual ICollection<tblVideoNPersona> tblVideoNPersonaidPersonaRemitenteNavigation { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("idPersona")]
        public virtual ICollection<tblLicenciaConducir> idLicenciaConducir { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("idPersona")]
        public virtual ICollection<tblMantenimientoPrev> idMantenimientoPrev { get; set; }
        [ForeignKey("idPersonaTransportista")]
        [InverseProperty("idPersonaTransportista")]
        public virtual ICollection<tblParteTransporte> idParteTransporte { get; set; }
    }
}
