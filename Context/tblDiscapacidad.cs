using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDiscapacidad", Schema = "RRHH")]
    public partial class tblDiscapacidad
    {
        public tblDiscapacidad()
        {
            tblPersona = new HashSet<tblPersona>();
            tblPersona_PeticionCambioDatos = new HashSet<tblPersona_PeticionCambioDatos>();
        }

        [Key]
        public byte idDiscapacidad { get; set; }
        public int idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblDiscapacidad")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idDiscapacidadNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
        [InverseProperty("idDiscapacidadNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatos { get; set; }
    }
}
