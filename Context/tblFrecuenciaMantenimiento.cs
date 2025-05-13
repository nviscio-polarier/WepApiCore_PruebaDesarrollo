using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblFrecuenciaMantenimiento", Schema = "Assistant")]
    public partial class tblFrecuenciaMantenimiento
    {
        public tblFrecuenciaMantenimiento()
        {
            tblTareaMaquina = new HashSet<tblTareaMaquina>();
        }

        [Key]
        public byte idFrecuenciaMantenimiento { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public int? idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblFrecuenciaMantenimiento")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idFrecuenciaMantenimientoNavigation")]
        public virtual ICollection<tblTareaMaquina> tblTareaMaquina { get; set; }
    }
}
