using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmFormaPago", Schema = "Administracion")]
    public partial class tblAdmFormaPago
    {
        public tblAdmFormaPago()
        {
            tblAdmAlbaranCompra = new HashSet<tblAdmAlbaranCompra>();
            tblAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
            tblAdmCliente = new HashSet<tblAdmCliente>();
            tblAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
            tblAdmPedidoCliente = new HashSet<tblAdmPedidoCliente>();
            tblAdmPedidoProveedor = new HashSet<tblAdmPedidoProveedor>();
            tblAdmPresupuestoVenta = new HashSet<tblAdmPresupuestoVenta>();
            tblAdmProveedor = new HashSet<tblAdmProveedor>();
        }

        [Key]
        public int idAdmFormaPago { get; set; }
        [StringLength(8)]
        public string codigo { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public int idPais { get; set; }
        [Required]
        public bool? isCuentaBancariaRequired { get; set; }

        [ForeignKey("idPais")]
        [InverseProperty("tblAdmFormaPago")]
        public virtual tblPais idPaisNavigation { get; set; } = null!;
        [InverseProperty("idAdmFormaPagoNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmFormaPagoNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idAdmFormaCobroNavigation")]
        public virtual ICollection<tblAdmCliente> tblAdmCliente { get; set; }
        [InverseProperty("idAdmFormaPagoNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idAdmFormaCobroNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idAdmFormaPagoNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmFormaPagoNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idAdmFormaPagoNavigation")]
        public virtual ICollection<tblAdmPresupuestoVenta> tblAdmPresupuestoVenta { get; set; }
        [InverseProperty("idAdmFormaPagoNavigation")]
        public virtual ICollection<tblAdmProveedor> tblAdmProveedor { get; set; }
    }
}
