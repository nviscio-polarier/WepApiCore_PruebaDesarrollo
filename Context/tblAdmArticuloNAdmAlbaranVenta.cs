using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmArticuloNAdmAlbaranVenta", Schema = "Administracion")]
    public partial class tblAdmArticuloNAdmAlbaranVenta
    {
        [Key]
        public int idAdmArticulo { get; set; }
        [Key]
        public int idAdmAlbaranVenta { get; set; }
        public int? cantidad { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal? iva { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? precio { get; set; }

        [ForeignKey("idAdmAlbaranVenta")]
        [InverseProperty("tblAdmArticuloNAdmAlbaranVenta")]
        public virtual tblAdmAlbaranVenta idAdmAlbaranVentaNavigation { get; set; } = null!;
        [ForeignKey("idAdmArticulo")]
        [InverseProperty("tblAdmArticuloNAdmAlbaranVenta")]
        public virtual tblAdmArticulo idAdmArticuloNavigation { get; set; } = null!;
    }
}
