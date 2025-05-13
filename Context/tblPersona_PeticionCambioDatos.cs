using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersona_PeticionCambioDatos", Schema = "RRHH")]
    public partial class tblPersona_PeticionCambioDatos
    {
        public tblPersona_PeticionCambioDatos()
        {
            idLicenciaConducir = new HashSet<tblLicenciaConducir>();
        }

        [Key]
        public int idPeticionCambioDatos { get; set; }
        public int idPersona { get; set; }
        public byte idPeticionCambioDatos_estado { get; set; }
        public DateTimeOffset fechaPeticion { get; set; }
        public DateTimeOffset? fechaRespuesta { get; set; }
        public bool? notificacion { get; set; }
        public string? nombre { get; set; }
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
        public string? nombre_tutor { get; set; }
        public string? apellidos_tutor { get; set; }
        public byte? idTipoDocumentoIdentidad_tutor { get; set; }
        public string? numDocumentoIdentidad_tutor { get; set; }
        public int? idFotoDemandanteEmpleo { get; set; }
        public int? idDocCertificadoDiscapacidad { get; set; }
        public bool? vehiculoPropio { get; set; }
        public int? idDocumentoLicenciaConducir { get; set; }

        [ForeignKey("idComunidadAutonoma")]
        [InverseProperty("tblPersona_PeticionCambioDatos")]
        public virtual tblComunidadAutonoma? idComunidadAutonomaNavigation { get; set; }
        [ForeignKey("idDiscapacidad")]
        [InverseProperty("tblPersona_PeticionCambioDatos")]
        public virtual tblDiscapacidad? idDiscapacidadNavigation { get; set; }
        [ForeignKey("idDocCertificadoDiscapacidad")]
        [InverseProperty("tblPersona_PeticionCambioDatosidDocCertificadoDiscapacidadNavigation")]
        public virtual tblDocumento? idDocCertificadoDiscapacidadNavigation { get; set; }
        [ForeignKey("idDocumentoLicenciaConducir")]
        [InverseProperty("tblPersona_PeticionCambioDatosidDocumentoLicenciaConducirNavigation")]
        public virtual tblDocumento? idDocumentoLicenciaConducirNavigation { get; set; }
        [ForeignKey("idEstadoCivil")]
        [InverseProperty("tblPersona_PeticionCambioDatos")]
        public virtual tblEstadoCivil? idEstadoCivilNavigation { get; set; }
        [ForeignKey("idFotoDemandanteEmpleo")]
        [InverseProperty("tblPersona_PeticionCambioDatosidFotoDemandanteEmpleoNavigation")]
        public virtual tblDocumento? idFotoDemandanteEmpleoNavigation { get; set; }
        [ForeignKey("idFotoDocumentoIdentidad_A")]
        [InverseProperty("tblPersona_PeticionCambioDatosidFotoDocumentoIdentidad_ANavigation")]
        public virtual tblDocumento? idFotoDocumentoIdentidad_ANavigation { get; set; }
        [ForeignKey("idFotoDocumentoIdentidad_B")]
        [InverseProperty("tblPersona_PeticionCambioDatosidFotoDocumentoIdentidad_BNavigation")]
        public virtual tblDocumento? idFotoDocumentoIdentidad_BNavigation { get; set; }
        [ForeignKey("idFotoIBAN")]
        [InverseProperty("tblPersona_PeticionCambioDatosidFotoIBANNavigation")]
        public virtual tblDocumento? idFotoIBANNavigation { get; set; }
        [ForeignKey("idFotoNAF")]
        [InverseProperty("tblPersona_PeticionCambioDatosidFotoNAFNavigation")]
        public virtual tblDocumento? idFotoNAFNavigation { get; set; }
        [ForeignKey("idFotoPerfil")]
        [InverseProperty("tblPersona_PeticionCambioDatosidFotoPerfilNavigation")]
        public virtual tblDocumento? idFotoPerfilNavigation { get; set; }
        [ForeignKey("idGenero")]
        [InverseProperty("tblPersona_PeticionCambioDatos")]
        public virtual tblGenero? idGeneroNavigation { get; set; }
        [ForeignKey("idNivelEstudios")]
        [InverseProperty("tblPersona_PeticionCambioDatos")]
        public virtual tblNivelEstudios? idNivelEstudiosNavigation { get; set; }
        [ForeignKey("idPais")]
        [InverseProperty("tblPersona_PeticionCambioDatos")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblPersona_PeticionCambioDatos")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idPeticionCambioDatos_estado")]
        [InverseProperty("tblPersona_PeticionCambioDatos")]
        public virtual tbPeticionCambioDatos_estado idPeticionCambioDatos_estadoNavigation { get; set; } = null!;
        [ForeignKey("idTallaAlfa_Camiseta")]
        [InverseProperty("tblPersona_PeticionCambioDatos")]
        public virtual tblTallaAlfa? idTallaAlfa_CamisetaNavigation { get; set; }
        [ForeignKey("idTipoDocumentoIdentidad")]
        [InverseProperty("tblPersona_PeticionCambioDatosidTipoDocumentoIdentidadNavigation")]
        public virtual tblTipoDocumentoIdentidad? idTipoDocumentoIdentidadNavigation { get; set; }
        [ForeignKey("idTipoDocumentoIdentidad_tutor")]
        [InverseProperty("tblPersona_PeticionCambioDatosidTipoDocumentoIdentidad_tutorNavigation")]
        public virtual tblTipoDocumentoIdentidad? idTipoDocumentoIdentidad_tutorNavigation { get; set; }

        [ForeignKey("idPeticionCambioDatos")]
        [InverseProperty("idPeticionCambioDatos")]
        public virtual ICollection<tblLicenciaConducir> idLicenciaConducir { get; set; }
    }
}
