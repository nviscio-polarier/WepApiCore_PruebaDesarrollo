using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLicenciaConducirNPersona_PeticionCambioDatos", Schema = "RRHH")]
    public partial class tblLicenciaConducirNPersona_PeticionCambioDatos
    {
        [Key]
        public byte idLicenciaConducir { get; set; }
        [Key]
        public int idPersona { get; set; }
        public DateTimeOffset? fechaPeticion { get; set; }
        public DateTimeOffset? fechaRespuesta { get; set; }

        [ForeignKey("idLicenciaConducir")]
        [InverseProperty("tblLicenciaConducirNPersona_PeticionCambioDatos")]
        public virtual tblLicenciaConducir idLicenciaConducirNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblLicenciaConducirNPersona_PeticionCambioDatos")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
