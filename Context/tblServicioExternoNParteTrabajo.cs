using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblServicioExternoNParteTrabajo", Schema = "Assistant")]
    public partial class tblServicioExternoNParteTrabajo
    {
        [Key]
        public int idServicioExterno { get; set; }
        public int idParteTrabajo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public int cantidad { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal precio { get; set; }

        [ForeignKey("idParteTrabajo")]
        [InverseProperty("tblServicioExternoNParteTrabajo")]
        public virtual tblParteTrabajo idParteTrabajoNavigation { get; set; } = null!;
    }
}
