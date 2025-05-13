using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tbPeticionCambioDatos_estado", Schema = "RRHH")]
    public partial class tbPeticionCambioDatos_estado
    {
        public tbPeticionCambioDatos_estado()
        {
            tblPersona_PeticionCambioDatos = new HashSet<tblPersona_PeticionCambioDatos>();
        }

        [Key]
        public byte idPeticionCambioDatos_estado { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idPeticionCambioDatos_estadoNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatos { get; set; }
    }
}
