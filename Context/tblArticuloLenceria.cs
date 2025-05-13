using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblArticuloLenceria", Schema = "Administracion")]
    public partial class tblArticuloLenceria
    {
        public tblArticuloLenceria()
        {
            tblArticuloNAdmAlbaranCompra = new HashSet<tblArticuloNAdmAlbaranCompra>();
            tblArticuloNAdmAlbaranVenta = new HashSet<tblArticuloNAdmAlbaranVenta>();
            tblArticuloNAdmPedidoCliente = new HashSet<tblArticuloNAdmPedidoCliente>();
            tblArticuloNAdmPedidoProveedor = new HashSet<tblArticuloNAdmPedidoProveedor>();
            tblArticuloNAdmPresupuestoVenta = new HashSet<tblArticuloNAdmPresupuestoVenta>();
        }

        [Key]
        public int idArticuloLenceria { get; set; }
        public short idDenoPrenda { get; set; }
        [StringLength(6)]
        public string codigo { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public int? idAdmCuentaContableCompra { get; set; }
        public int? idAdmCuentaContableVenta { get; set; }
        public bool eliminado { get; set; }

        [ForeignKey("idAdmCuentaContableCompra")]
        [InverseProperty("tblArticuloLenceriaidAdmCuentaContableCompraNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContableCompraNavigation { get; set; }
        [ForeignKey("idAdmCuentaContableVenta")]
        [InverseProperty("tblArticuloLenceriaidAdmCuentaContableVentaNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContableVentaNavigation { get; set; }
        [ForeignKey("idDenoPrenda")]
        [InverseProperty("tblArticuloLenceria")]
        public virtual tblDenoPrenda idDenoPrendaNavigation { get; set; } = null!;
        [InverseProperty("idArticuloLenceriaNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranCompra> tblArticuloNAdmAlbaranCompra { get; set; }
        [InverseProperty("idArticuloLenceriaNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranVenta> tblArticuloNAdmAlbaranVenta { get; set; }
        [InverseProperty("idArticuloLenceriaNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoCliente> tblArticuloNAdmPedidoCliente { get; set; }
        [InverseProperty("idArticuloLenceriaNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoProveedor> tblArticuloNAdmPedidoProveedor { get; set; }
        [InverseProperty("idArticuloLenceriaNavigation")]
        public virtual ICollection<tblArticuloNAdmPresupuestoVenta> tblArticuloNAdmPresupuestoVenta { get; set; }
    }
}
