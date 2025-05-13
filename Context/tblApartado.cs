using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblApartado", Schema = "GestionInterna")]
    public partial class tblApartado
    {
        public tblApartado()
        {
            InverseidApartadoPadreNavigation = new HashSet<tblApartado>();
            tblFormulario = new HashSet<tblFormulario>();
        }

        [Key]
        public int idApartado { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public string apartado { get; set; } = null!;
        public string? icon { get; set; }
        public int? idApartadoPadre { get; set; }
        public int? idTraduccion { get; set; }
        public int? orden { get; set; }

        [ForeignKey("idApartadoPadre")]
        [InverseProperty("InverseidApartadoPadreNavigation")]
        public virtual tblApartado? idApartadoPadreNavigation { get; set; }
        [ForeignKey("idTraduccion")]
        [InverseProperty("tblApartado")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idApartadoPadreNavigation")]
        public virtual ICollection<tblApartado> InverseidApartadoPadreNavigation { get; set; }
        [InverseProperty("idApartadoNavigation")]
        public virtual ICollection<tblFormulario> tblFormulario { get; set; }
    }
}
