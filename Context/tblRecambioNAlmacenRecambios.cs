using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRecambioNAlmacenRecambios", Schema = "Assistant")]
    public partial class tblRecambioNAlmacenRecambios
    {
        [Key]
        public int idAlmacen { get; set; }
        [Key]
        public int idRecambio { get; set; }
        public int cantidad { get; set; }
        [StringLength(50)]
        public string? ubicacion { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? precioMedioPonderado { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblRecambioNAlmacenRecambios")]
        public virtual tblAlmacenRecambios idAlmacenNavigation { get; set; } = null!;
        [ForeignKey("idRecambio")]
        [InverseProperty("tblRecambioNAlmacenRecambios")]
        public virtual tblRecambio idRecambioNavigation { get; set; } = null!;
    }
}
