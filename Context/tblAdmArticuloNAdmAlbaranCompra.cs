using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmArticuloNAdmAlbaranCompra", Schema = "Administracion")]
    public partial class tblAdmArticuloNAdmAlbaranCompra
    {
        [Key]
        public int idAdmArticulo { get; set; }
        [Key]
        public int idAdmAlbaranCompra { get; set; }
        public int? cantidad { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal? iva { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? precio { get; set; }

        [ForeignKey("idAdmAlbaranCompra")]
        [InverseProperty("tblAdmArticuloNAdmAlbaranCompra")]
        public virtual tblAdmAlbaranCompra idAdmAlbaranCompraNavigation { get; set; } = null!;
        [ForeignKey("idAdmArticulo")]
        [InverseProperty("tblAdmArticuloNAdmAlbaranCompra")]
        public virtual tblAdmArticulo idAdmArticuloNavigation { get; set; } = null!;
    }
}
