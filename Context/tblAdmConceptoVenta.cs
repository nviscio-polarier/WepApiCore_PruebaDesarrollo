using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmConceptoVenta", Schema = "Administracion")]
    public partial class tblAdmConceptoVenta
    {
        [Key]
        public int idAdmConceptoVenta { get; set; }
        [Key]
        public int idAdmFacturaVenta { get; set; }
        public int idAdmCuentaContable { get; set; }
        public string? descripcion { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? cantidad { get; set; }
        [Column(TypeName = "decimal(14, 3)")]
        public decimal? precio { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal descuento { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal iva { get; set; }

        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblAdmConceptoVenta")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idAdmFacturaVenta")]
        [InverseProperty("tblAdmConceptoVenta")]
        public virtual tblAdmFacturaVenta idAdmFacturaVentaNavigation { get; set; } = null!;
    }
}
