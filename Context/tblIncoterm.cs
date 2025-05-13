using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblIncoterm", Schema = "Logistica")]
    public partial class tblIncoterm
    {
        public tblIncoterm()
        {
            tblAdmAlbaranCompra = new HashSet<tblAdmAlbaranCompra>();
            tblAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
            tblAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
            tblAdmPedidoCliente = new HashSet<tblAdmPedidoCliente>();
            tblAdmPedidoProveedor = new HashSet<tblAdmPedidoProveedor>();
            tblEnvioidIncotermClienteNavigation = new HashSet<tblEnvio>();
            tblEnvioidIncotermProvNavigation = new HashSet<tblEnvio>();
        }

        [Key]
        public int idIncoterm { get; set; }
        [StringLength(5)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idIncotermNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idIncotermNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idIncotermNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idIncotermNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idIncotermNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idIncotermNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idIncotermClienteNavigation")]
        public virtual ICollection<tblEnvio> tblEnvioidIncotermClienteNavigation { get; set; }
        [InverseProperty("idIncotermProvNavigation")]
        public virtual ICollection<tblEnvio> tblEnvioidIncotermProvNavigation { get; set; }
    }
}
