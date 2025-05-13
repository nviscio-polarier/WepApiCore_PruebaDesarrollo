using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblNivelEstudios", Schema = "RRHH")]
    public partial class tblNivelEstudios
    {
        public tblNivelEstudios()
        {
            tblPersona = new HashSet<tblPersona>();
            tblPersona_PeticionCambioDatos = new HashSet<tblPersona_PeticionCambioDatos>();
        }

        [Key]
        public byte idNivelEstudios { get; set; }
        public int idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblNivelEstudios")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idNivelEstudiosNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
        [InverseProperty("idNivelEstudiosNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatos { get; set; }
    }
}
