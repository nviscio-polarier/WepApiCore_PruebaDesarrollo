using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNEntidad_NuevoPedido", Schema = "General")]
    [Index("idEntidad", "stockDefinido", Name = "IX_tblPrendaNEntidad_NuevoPedido_idEntidad_stockDefinido")]
    public partial class tblPrendaNEntidad_NuevoPedido
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idEntidad { get; set; }
        public int? stockDefinido { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? ratio { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblPrendaNEntidad_NuevoPedido")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNEntidad_NuevoPedido")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
