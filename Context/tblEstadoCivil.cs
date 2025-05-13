using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoCivil", Schema = "RRHH")]
    public partial class tblEstadoCivil
    {
        public tblEstadoCivil()
        {
            tblPersona = new HashSet<tblPersona>();
            tblPersona_PeticionCambioDatos = new HashSet<tblPersona_PeticionCambioDatos>();
        }

        [Key]
        public byte idEstadoCivil { get; set; }
        public int idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblEstadoCivil")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idEstadoCivilNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
        [InverseProperty("idEstadoCivilNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatos { get; set; }
    }
}
