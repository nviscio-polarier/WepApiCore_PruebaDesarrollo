using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDocumento", Schema = "RRHH")]
    public partial class tblDocumento
    {
        public tblDocumento()
        {
            tblDocumentoNSolicitudAlta = new HashSet<tblDocumentoNSolicitudAlta>();
            tblPersona_PeticionCambioDatosidDocCertificadoDiscapacidadNavigation = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersona_PeticionCambioDatosidDocumentoLicenciaConducirNavigation = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersona_PeticionCambioDatosidFotoDemandanteEmpleoNavigation = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersona_PeticionCambioDatosidFotoDocumentoIdentidad_ANavigation = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersona_PeticionCambioDatosidFotoDocumentoIdentidad_BNavigation = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersona_PeticionCambioDatosidFotoIBANNavigation = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersona_PeticionCambioDatosidFotoNAFNavigation = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersona_PeticionCambioDatosidFotoPerfilNavigation = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersonaidDocCertificadoDiscapacidadNavigation = new HashSet<tblPersona>();
            tblPersonaidDocumentoLicenciaConducirNavigation = new HashSet<tblPersona>();
            tblPersonaidFotoDemandanteEmpleoNavigation = new HashSet<tblPersona>();
            tblPersonaidFotoDocumentoIdentidad_ANavigation = new HashSet<tblPersona>();
            tblPersonaidFotoDocumentoIdentidad_BNavigation = new HashSet<tblPersona>();
            tblPersonaidFotoIBANNavigation = new HashSet<tblPersona>();
            tblPersonaidFotoNAFNavigation = new HashSet<tblPersona>();
            tblPersonaidFotoPerfilNavigation = new HashSet<tblPersona>();
        }

        [Key]
        public int idDocumento { get; set; }
        public DateTimeOffset fecha { get; set; }
        public DateTimeOffset fechaModificacion { get; set; }
        public string denominacion { get; set; } = null!;
        public byte[]? documento { get; set; }
        [StringLength(25)]
        public string? extension { get; set; }
        public short? idCarpetaDocumentos { get; set; }
        public int? idPersona { get; set; }
        public bool? nuevo { get; set; }
        public bool? requerido { get; set; }
        public bool? firmado { get; set; }
        public bool? notificacion { get; set; }
        public DateTimeOffset? fechaFirma { get; set; }
        public bool? isLeido { get; set; }
        public string? subCarpeta { get; set; }
        public byte? idTipoDocumento { get; set; }
        public bool? isVisible { get; set; }
        public int? idUsuario { get; set; }
        public int? idUsuarioModificacion { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblDocumento")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idTipoDocumento")]
        [InverseProperty("tblDocumento")]
        public virtual tblTipoDocumento? idTipoDocumentoNavigation { get; set; }
        [ForeignKey("idUsuarioModificacion")]
        [InverseProperty("tblDocumentoidUsuarioModificacionNavigation")]
        public virtual tblUsuario? idUsuarioModificacionNavigation { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("tblDocumentoidUsuarioNavigation")]
        public virtual tblUsuario? idUsuarioNavigation { get; set; }
        [InverseProperty("idDocumentoNavigation")]
        public virtual ICollection<tblDocumentoNSolicitudAlta> tblDocumentoNSolicitudAlta { get; set; }
        [InverseProperty("idDocCertificadoDiscapacidadNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatosidDocCertificadoDiscapacidadNavigation { get; set; }
        [InverseProperty("idDocumentoLicenciaConducirNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatosidDocumentoLicenciaConducirNavigation { get; set; }
        [InverseProperty("idFotoDemandanteEmpleoNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatosidFotoDemandanteEmpleoNavigation { get; set; }
        [InverseProperty("idFotoDocumentoIdentidad_ANavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatosidFotoDocumentoIdentidad_ANavigation { get; set; }
        [InverseProperty("idFotoDocumentoIdentidad_BNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatosidFotoDocumentoIdentidad_BNavigation { get; set; }
        [InverseProperty("idFotoIBANNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatosidFotoIBANNavigation { get; set; }
        [InverseProperty("idFotoNAFNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatosidFotoNAFNavigation { get; set; }
        [InverseProperty("idFotoPerfilNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatosidFotoPerfilNavigation { get; set; }
        [InverseProperty("idDocCertificadoDiscapacidadNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidDocCertificadoDiscapacidadNavigation { get; set; }
        [InverseProperty("idDocumentoLicenciaConducirNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidDocumentoLicenciaConducirNavigation { get; set; }
        [InverseProperty("idFotoDemandanteEmpleoNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidFotoDemandanteEmpleoNavigation { get; set; }
        [InverseProperty("idFotoDocumentoIdentidad_ANavigation")]
        public virtual ICollection<tblPersona> tblPersonaidFotoDocumentoIdentidad_ANavigation { get; set; }
        [InverseProperty("idFotoDocumentoIdentidad_BNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidFotoDocumentoIdentidad_BNavigation { get; set; }
        [InverseProperty("idFotoIBANNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidFotoIBANNavigation { get; set; }
        [InverseProperty("idFotoNAFNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidFotoNAFNavigation { get; set; }
        [InverseProperty("idFotoPerfilNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidFotoPerfilNavigation { get; set; }
    }
}
