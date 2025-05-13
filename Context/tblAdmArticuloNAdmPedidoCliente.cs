using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmArticuloNAdmPedidoCliente", Schema = "Administracion")]
    public partial class tblAdmArticuloNAdmPedidoCliente
    {
        [Key]
        public int idAdmArticulo { get; set; }
        [Key]
        public int idAdmPedidoCliente { get; set; }
        public int? cantidad { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal? iva { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? precio { get; set; }

        [ForeignKey("idAdmArticulo")]
        [InverseProperty("tblAdmArticuloNAdmPedidoCliente")]
        public virtual tblAdmArticulo idAdmArticuloNavigation { get; set; } = null!;
        [ForeignKey("idAdmPedidoCliente")]
        [InverseProperty("tblAdmArticuloNAdmPedidoCliente")]
        public virtual tblAdmPedidoCliente idAdmPedidoClienteNavigation { get; set; } = null!;
    }
}
