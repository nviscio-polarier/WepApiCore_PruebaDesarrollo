using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmArticuloNAdmPedidoProveedor", Schema = "Administracion")]
    public partial class tblAdmArticuloNAdmPedidoProveedor
    {
        [Key]
        public int idAdmArticulo { get; set; }
        [Key]
        public int idAdmPedidoProveedor { get; set; }
        public int? cantidad { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal? iva { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? precio { get; set; }

        [ForeignKey("idAdmArticulo")]
        [InverseProperty("tblAdmArticuloNAdmPedidoProveedor")]
        public virtual tblAdmArticulo idAdmArticuloNavigation { get; set; } = null!;
        [ForeignKey("idAdmPedidoProveedor")]
        [InverseProperty("tblAdmArticuloNAdmPedidoProveedor")]
        public virtual tblAdmPedidoProveedor idAdmPedidoProveedorNavigation { get; set; } = null!;
    }
}
