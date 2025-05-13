using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmArticulo", Schema = "Administracion")]
    public partial class tblAdmArticulo
    {
        public tblAdmArticulo()
        {
            tblAdmArticuloNAdmAlbaranCompra = new HashSet<tblAdmArticuloNAdmAlbaranCompra>();
            tblAdmArticuloNAdmAlbaranVenta = new HashSet<tblAdmArticuloNAdmAlbaranVenta>();
            tblAdmArticuloNAdmPedidoCliente = new HashSet<tblAdmArticuloNAdmPedidoCliente>();
            tblAdmArticuloNAdmPedidoProveedor = new HashSet<tblAdmArticuloNAdmPedidoProveedor>();
            tblAdmArticuloNAdmPresupuestoVenta = new HashSet<tblAdmArticuloNAdmPresupuestoVenta>();
        }

        [Key]
        public int idAdmArticulo { get; set; }
        public string? codigoArticulo { get; set; }
        public string? descripcion { get; set; }

        [InverseProperty("idAdmArticuloNavigation")]
        public virtual ICollection<tblAdmArticuloNAdmAlbaranCompra> tblAdmArticuloNAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmArticuloNavigation")]
        public virtual ICollection<tblAdmArticuloNAdmAlbaranVenta> tblAdmArticuloNAdmAlbaranVenta { get; set; }
        [InverseProperty("idAdmArticuloNavigation")]
        public virtual ICollection<tblAdmArticuloNAdmPedidoCliente> tblAdmArticuloNAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmArticuloNavigation")]
        public virtual ICollection<tblAdmArticuloNAdmPedidoProveedor> tblAdmArticuloNAdmPedidoProveedor { get; set; }
        [InverseProperty("idAdmArticuloNavigation")]
        public virtual ICollection<tblAdmArticuloNAdmPresupuestoVenta> tblAdmArticuloNAdmPresupuestoVenta { get; set; }
    }
}
