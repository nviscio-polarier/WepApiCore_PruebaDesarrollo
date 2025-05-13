using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRecambio", Schema = "Assistant")]
    public partial class tblRecambio
    {
        public tblRecambio()
        {
            tblArticuloNAdmAlbaranCompra = new HashSet<tblArticuloNAdmAlbaranCompra>();
            tblArticuloNAdmAlbaranVenta = new HashSet<tblArticuloNAdmAlbaranVenta>();
            tblArticuloNAdmPedidoCliente = new HashSet<tblArticuloNAdmPedidoCliente>();
            tblArticuloNAdmPedidoProveedor = new HashSet<tblArticuloNAdmPedidoProveedor>();
            tblArticuloNAdmPresupuestoVenta = new HashSet<tblArticuloNAdmPresupuestoVenta>();
            tblCierreRecambioNAlmacen = new HashSet<tblCierreRecambioNAlmacen>();
            tblRecambioNAlmacenRecambios = new HashSet<tblRecambioNAlmacenRecambios>();
            tblRecambioNMovimientoRecambio = new HashSet<tblRecambioNMovimientoRecambio>();
            tblRecambioNParteTrabajo = new HashSet<tblRecambioNParteTrabajo>();
            tblRecambioNProveedor = new HashSet<tblRecambioNProveedor>();
            tblStockMinimoNAlmacenRecambios = new HashSet<tblStockMinimoNAlmacenRecambios>();
        }

        [Key]
        public int idRecambio { get; set; }
        public string denominacion { get; set; } = null!;
        [Required]
        public bool? activo { get; set; }
        public bool eliminado { get; set; }
        public string? referencia { get; set; }
        public short? idProveedor { get; set; }
        public string? referenciaInterna { get; set; }
        public string? descripcionArticulo { get; set; }
        public int? peso { get; set; }

        [ForeignKey("idProveedor")]
        [InverseProperty("tblRecambio")]
        public virtual tblProveedor? idProveedorNavigation { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranCompra> tblArticuloNAdmAlbaranCompra { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranVenta> tblArticuloNAdmAlbaranVenta { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoCliente> tblArticuloNAdmPedidoCliente { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoProveedor> tblArticuloNAdmPedidoProveedor { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblArticuloNAdmPresupuestoVenta> tblArticuloNAdmPresupuestoVenta { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblCierreRecambioNAlmacen> tblCierreRecambioNAlmacen { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblRecambioNAlmacenRecambios> tblRecambioNAlmacenRecambios { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblRecambioNMovimientoRecambio> tblRecambioNMovimientoRecambio { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblRecambioNParteTrabajo> tblRecambioNParteTrabajo { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblRecambioNProveedor> tblRecambioNProveedor { get; set; }
        [InverseProperty("idRecambioNavigation")]
        public virtual ICollection<tblStockMinimoNAlmacenRecambios> tblStockMinimoNAlmacenRecambios { get; set; }
    }
}
