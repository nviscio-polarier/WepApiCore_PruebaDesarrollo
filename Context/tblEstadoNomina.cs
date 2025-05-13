using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoNomina", Schema = "RRHH")]
    public partial class tblEstadoNomina
    {
        public tblEstadoNomina()
        {
            tblEstadoNominaNNomina = new HashSet<tblEstadoNominaNNomina>();
            tblNomina = new HashSet<tblNomina>();
        }

        [Key]
        public byte idEstadoNomina { get; set; }
        public int idTraduccion { get; set; }
        public string denominacion { get; set; } = null!;

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblEstadoNomina")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idEstadoNominaNavigation")]
        public virtual ICollection<tblEstadoNominaNNomina> tblEstadoNominaNNomina { get; set; }
        [InverseProperty("idEstadoNominaNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
    }
}
