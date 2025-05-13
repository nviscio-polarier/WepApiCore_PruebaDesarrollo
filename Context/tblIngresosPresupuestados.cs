using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblIngresosPresupuestados", Schema = "Costes")]
    [Index("idLavanderia", "fecha", Name = "IX_tblIngresosPresupuestados_idLavanderia_fecha")]
    public partial class tblIngresosPresupuestados
    {
        [Key]
        public int idEntidad { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal valor { get; set; }
        [Key]
        public int idLavanderia { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblIngresosPresupuestados")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblIngresosPresupuestados")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
    }
}
