using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTallaAlfa", Schema = "RRHH")]
    public partial class tblTallaAlfa
    {
        public tblTallaAlfa()
        {
            tblPersona = new HashSet<tblPersona>();
            tblPersona_PeticionCambioDatos = new HashSet<tblPersona_PeticionCambioDatos>();
        }

        [Key]
        public byte idTallaAlfa { get; set; }
        [StringLength(3)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTallaAlfa_CamisetaNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
        [InverseProperty("idTallaAlfa_CamisetaNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatos { get; set; }
    }
}
