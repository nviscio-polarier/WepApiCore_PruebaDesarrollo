using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmTipoElemento", Schema = "Administracion")]
    public partial class tblAdmTipoElemento
    {
        public tblAdmTipoElemento()
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
        public byte idAdmTipoElemento { get; set; }
        public string? denominacion { get; set; }
        [StringLength(4)]
        public string? prefijo { get; set; }

        [InverseProperty("idTipoAlbaranNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idTipoAlbaranNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idTipoAlbaranNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idTipoAlbaranNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idTipoPedidoNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idTipoPedidoNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idTipoPresupuestoNavigation")]
        public virtual ICollection<tblAdmPresupuestoVenta> tblAdmPresupuestoVenta { get; set; }
    }
}
