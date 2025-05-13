using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblIvaNPais", Schema = "Finanzas")]
    public partial class tblIvaNPais
    {
        public tblIvaNPais()
        {
            tblAdmCliente = new HashSet<tblAdmCliente>();
            tblAdmConceptoCompra = new HashSet<tblAdmConceptoCompra>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
            tblAdmPedidoCliente = new HashSet<tblAdmPedidoCliente>();
            tblAdmPresupuestoVenta = new HashSet<tblAdmPresupuestoVenta>();
            tblArticuloNAdmAlbaranCompra = new HashSet<tblArticuloNAdmAlbaranCompra>();
            tblArticuloNAdmPedidoProveedor = new HashSet<tblArticuloNAdmPedidoProveedor>();
        }

        [Key]
        public byte idIvaNPais { get; set; }
        public int idPais { get; set; }
        public byte idAdmIva { get; set; }
        [StringLength(2)]
        public string? indicadorIVA_Compra { get; set; }
        public string? descripcionIVA_Compra { get; set; }
        [StringLength(2)]
        public string? indicadorIVA_Venta { get; set; }
        public string? descripcionIVA_Venta { get; set; }

        [ForeignKey("idAdmIva")]
        [InverseProperty("tblIvaNPais")]
        public virtual tblAdmIva idAdmIvaNavigation { get; set; } = null!;
        [ForeignKey("idPais")]
        [InverseProperty("tblIvaNPais")]
        public virtual tblPais idPaisNavigation { get; set; } = null!;
        [InverseProperty("idIvaNPaisNavigation")]
        public virtual ICollection<tblAdmCliente> tblAdmCliente { get; set; }
        [InverseProperty("idIvaNPaisNavigation")]
        public virtual ICollection<tblAdmConceptoCompra> tblAdmConceptoCompra { get; set; }
        [InverseProperty("idIvaNPaisNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idIvaNPaisNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idIvaNPaisNavigation")]
        public virtual ICollection<tblAdmPresupuestoVenta> tblAdmPresupuestoVenta { get; set; }
        [InverseProperty("idIvaNPaisNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranCompra> tblArticuloNAdmAlbaranCompra { get; set; }
        [InverseProperty("idIvaNPaisNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoProveedor> tblArticuloNAdmPedidoProveedor { get; set; }
    }
}
