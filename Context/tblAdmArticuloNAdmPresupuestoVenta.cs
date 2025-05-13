using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmArticuloNAdmPresupuestoVenta", Schema = "Administracion")]
    public partial class tblAdmArticuloNAdmPresupuestoVenta
    {
        [Key]
        public int idAdmArticulo { get; set; }
        [Key]
        public int idAdmPresupuestoVenta { get; set; }
        public int? cantidad { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal? iva { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? precio { get; set; }

        [ForeignKey("idAdmArticulo")]
        [InverseProperty("tblAdmArticuloNAdmPresupuestoVenta")]
        public virtual tblAdmArticulo idAdmArticuloNavigation { get; set; } = null!;
        [ForeignKey("idAdmPresupuestoVenta")]
        [InverseProperty("tblAdmArticuloNAdmPresupuestoVenta")]
        public virtual tblAdmPresupuestoVenta idAdmPresupuestoVentaNavigation { get; set; } = null!;
    }
}
