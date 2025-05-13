using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblArticuloLogistico", Schema = "Administracion")]
    public partial class tblArticuloLogistico
    {
        public tblArticuloLogistico()
        {
            tblArticuloNAdmAlbaranCompra = new HashSet<tblArticuloNAdmAlbaranCompra>();
            tblArticuloNAdmAlbaranVenta = new HashSet<tblArticuloNAdmAlbaranVenta>();
            tblArticuloNAdmPedidoCliente = new HashSet<tblArticuloNAdmPedidoCliente>();
            tblArticuloNAdmPedidoProveedor = new HashSet<tblArticuloNAdmPedidoProveedor>();
            tblArticuloNAdmPresupuestoVenta = new HashSet<tblArticuloNAdmPresupuestoVenta>();
        }

        [Key]
        public int idArticuloLogistico { get; set; }
        public string? denominacion { get; set; }
        public int? idAdmCuentaContableCompra { get; set; }
        public int? idAdmCuentaContableVenta { get; set; }
        public bool? eliminado { get; set; }

        [ForeignKey("idAdmCuentaContableCompra")]
        [InverseProperty("tblArticuloLogisticoidAdmCuentaContableCompraNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContableCompraNavigation { get; set; }
        [ForeignKey("idAdmCuentaContableVenta")]
        [InverseProperty("tblArticuloLogisticoidAdmCuentaContableVentaNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContableVentaNavigation { get; set; }
        [InverseProperty("idArticuloLogisticoNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranCompra> tblArticuloNAdmAlbaranCompra { get; set; }
        [InverseProperty("idArticuloLogisticoNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranVenta> tblArticuloNAdmAlbaranVenta { get; set; }
        [InverseProperty("idArticuloLogisticoNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoCliente> tblArticuloNAdmPedidoCliente { get; set; }
        [InverseProperty("idArticuloLogisticoNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoProveedor> tblArticuloNAdmPedidoProveedor { get; set; }
        [InverseProperty("idArticuloLogisticoNavigation")]
        public virtual ICollection<tblArticuloNAdmPresupuestoVenta> tblArticuloNAdmPresupuestoVenta { get; set; }
    }
}
