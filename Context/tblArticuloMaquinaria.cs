using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblArticuloMaquinaria", Schema = "Administracion")]
    public partial class tblArticuloMaquinaria
    {
        public tblArticuloMaquinaria()
        {
            tblArticuloNAdmAlbaranCompra = new HashSet<tblArticuloNAdmAlbaranCompra>();
            tblArticuloNAdmAlbaranVenta = new HashSet<tblArticuloNAdmAlbaranVenta>();
            tblArticuloNAdmPedidoCliente = new HashSet<tblArticuloNAdmPedidoCliente>();
            tblArticuloNAdmPedidoProveedor = new HashSet<tblArticuloNAdmPedidoProveedor>();
            tblArticuloNAdmPresupuestoVenta = new HashSet<tblArticuloNAdmPresupuestoVenta>();
        }

        [Key]
        public int idArticuloMaquinaria { get; set; }
        public int idCategoriaMaquina { get; set; }
        [StringLength(6)]
        public string codigo { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public int? idAdmCuentaContableCompra { get; set; }
        public int? idAdmCuentaContableVenta { get; set; }
        public bool eliminado { get; set; }

        [ForeignKey("idAdmCuentaContableCompra")]
        [InverseProperty("tblArticuloMaquinariaidAdmCuentaContableCompraNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContableCompraNavigation { get; set; }
        [ForeignKey("idAdmCuentaContableVenta")]
        [InverseProperty("tblArticuloMaquinariaidAdmCuentaContableVentaNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContableVentaNavigation { get; set; }
        [ForeignKey("idCategoriaMaquina")]
        [InverseProperty("tblArticuloMaquinaria")]
        public virtual tblCategoriaMaquina idCategoriaMaquinaNavigation { get; set; } = null!;
        [InverseProperty("idArticuloMaquinariaNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranCompra> tblArticuloNAdmAlbaranCompra { get; set; }
        [InverseProperty("idArticuloMaquinariaNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranVenta> tblArticuloNAdmAlbaranVenta { get; set; }
        [InverseProperty("idArticuloMaquinariaNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoCliente> tblArticuloNAdmPedidoCliente { get; set; }
        [InverseProperty("idArticuloMaquinariaNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoProveedor> tblArticuloNAdmPedidoProveedor { get; set; }
        [InverseProperty("idArticuloMaquinariaNavigation")]
        public virtual ICollection<tblArticuloNAdmPresupuestoVenta> tblArticuloNAdmPresupuestoVenta { get; set; }
    }
}
