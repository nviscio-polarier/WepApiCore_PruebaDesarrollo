using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCierreRecambioNAlmacen", Schema = "Assistant")]
    public partial class tblCierreRecambioNAlmacen
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Key]
        public int idAlmacen { get; set; }
        [Key]
        public int idRecambio { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal precioMedio { get; set; }
        public int cantidad { get; set; }
        [StringLength(50)]
        public string? ubicacion { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblCierreRecambioNAlmacen")]
        public virtual tblAlmacenRecambios idAlmacenNavigation { get; set; } = null!;
        [ForeignKey("idRecambio")]
        [InverseProperty("tblCierreRecambioNAlmacen")]
        public virtual tblRecambio idRecambioNavigation { get; set; } = null!;
    }
}
