using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblGrupoArticulos", Schema = "Administracion")]
    public partial class tblGrupoArticulos
    {
        public tblGrupoArticulos()
        {
            tblArticuloNAdmAlbaranCompra = new HashSet<tblArticuloNAdmAlbaranCompra>();
            tblArticuloNAdmAlbaranVenta = new HashSet<tblArticuloNAdmAlbaranVenta>();
            tblArticuloNAdmPedidoCliente = new HashSet<tblArticuloNAdmPedidoCliente>();
            tblArticuloNAdmPedidoProveedor = new HashSet<tblArticuloNAdmPedidoProveedor>();
            tblArticuloNAdmPresupuestoVenta = new HashSet<tblArticuloNAdmPresupuestoVenta>();
        }

        [Key]
        public int idGrupoArticulos { get; set; }
        public string codigo { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public bool isEliminado { get; set; }
        public int? idAdmCuentaContableCompra { get; set; }

        [ForeignKey("idAdmCuentaContableCompra")]
        [InverseProperty("tblGrupoArticulos")]
        public virtual tblAdmCuentaContable? idAdmCuentaContableCompraNavigation { get; set; }
        [InverseProperty("idGrupoArticulosNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranCompra> tblArticuloNAdmAlbaranCompra { get; set; }
        [InverseProperty("idGrupoArticulosNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranVenta> tblArticuloNAdmAlbaranVenta { get; set; }
        [InverseProperty("idGrupoArticulosNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoCliente> tblArticuloNAdmPedidoCliente { get; set; }
        [InverseProperty("idGrupoArticulosNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoProveedor> tblArticuloNAdmPedidoProveedor { get; set; }
        [InverseProperty("idGrupoArticulosNavigation")]
        public virtual ICollection<tblArticuloNAdmPresupuestoVenta> tblArticuloNAdmPresupuestoVenta { get; set; }
    }
}
