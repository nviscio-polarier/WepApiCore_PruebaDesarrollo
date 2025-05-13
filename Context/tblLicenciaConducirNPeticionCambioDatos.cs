using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLicenciaConducirNPeticionCambioDatos", Schema = "RRHH")]
    public partial class tblLicenciaConducirNPeticionCambioDatos
    {
        [Key]
        public int idPeticionCambioDatos { get; set; }
        [Key]
        public byte idLicenciaConducir { get; set; }

        [ForeignKey("idPeticionCambioDatos")]
        [InverseProperty("tblLicenciaConducirNPeticionCambioDatos")]
        public virtual tblPersona_PeticionCambioDatos idPeticionCambioDatosNavigation { get; set; } = null!;
    }
}
