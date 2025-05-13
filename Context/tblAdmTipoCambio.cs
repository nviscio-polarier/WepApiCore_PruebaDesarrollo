using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmTipoCambio", Schema = "Administracion")]
    public partial class tblAdmTipoCambio
    {
        public tblAdmTipoCambio()
        {
            tblAdmAlbaranCompra = new HashSet<tblAdmAlbaranCompra>();
            tblAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
            tblAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
            tblAdmPedidoCliente = new HashSet<tblAdmPedidoCliente>();
            tblAdmPedidoProveedor = new HashSet<tblAdmPedidoProveedor>();
            tblAdmPresupuestoVenta = new HashSet<tblAdmPresupuestoVenta>();
        }

        [Key]
        public byte idAdmTipoCambio { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idAdmTipoCambioNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmTipoCambioNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idAdmTipoCambioNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idAdmTipoCambioNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idAdmTipoCambioNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmTipoCambioNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idAdmTipoCambioNavigation")]
        public virtual ICollection<tblAdmPresupuestoVenta> tblAdmPresupuestoVenta { get; set; }
    }
}
