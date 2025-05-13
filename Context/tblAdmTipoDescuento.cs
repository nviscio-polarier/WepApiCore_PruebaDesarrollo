using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmTipoDescuento", Schema = "Administracion")]
    public partial class tblAdmTipoDescuento
    {
        public tblAdmTipoDescuento()
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
        public byte idAdmTipoDescuento { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idAdmTipoDescuentoNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmTipoDescuentoNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idAdmTipoDescuentoNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idAdmTipoDescuentoNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idAdmTipoDescuentoNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmTipoDescuentoNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idAdmTipoDescuentoNavigation")]
        public virtual ICollection<tblAdmPresupuestoVenta> tblAdmPresupuestoVenta { get; set; }
    }
}
