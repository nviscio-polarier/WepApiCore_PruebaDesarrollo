using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmPresupuestoVenta", Schema = "Administracion")]
    public partial class tblAdmPresupuestoVenta
    {
        public tblAdmPresupuestoVenta()
        {
            tblAdmArticuloNAdmPresupuestoVenta = new HashSet<tblAdmArticuloNAdmPresupuestoVenta>();
            tblAdmPedidoCliente = new HashSet<tblAdmPedidoCliente>();
            tblArticuloNAdmPresupuestoVenta = new HashSet<tblArticuloNAdmPresupuestoVenta>();
        }

        [Key]
        public int idAdmPresupuestoVenta { get; set; }
        [StringLength(12)]
        public string codigo { get; set; } = null!;
        public int? idAdmCliente { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaCreacion { get; set; }
        public byte? idMoneda { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal? tasaCambio { get; set; }
        public int? idAdmFormaPago { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public byte? idIvaNPais { get; set; }
        public byte? idTipoPresupuesto { get; set; }
        public string? observaciones { get; set; }
        public byte? idAdmPresupuestoVenta_Estado { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public bool isCerrado { get; set; }

        [ForeignKey("idAdmCliente")]
        [InverseProperty("tblAdmPresupuestoVenta")]
        public virtual tblAdmCliente? idAdmClienteNavigation { get; set; }
        [ForeignKey("idAdmFormaPago")]
        [InverseProperty("tblAdmPresupuestoVenta")]
        public virtual tblAdmFormaPago? idAdmFormaPagoNavigation { get; set; }
        [ForeignKey("idAdmPresupuestoVenta_Estado")]
        [InverseProperty("tblAdmPresupuestoVenta")]
        public virtual tblAdmPresupuestoVenta_Estado? idAdmPresupuestoVenta_EstadoNavigation { get; set; }
        [ForeignKey("idAdmTipoCambio")]
        [InverseProperty("tblAdmPresupuestoVenta")]
        public virtual tblAdmTipoCambio? idAdmTipoCambioNavigation { get; set; }
        [ForeignKey("idAdmTipoDescuento")]
        [InverseProperty("tblAdmPresupuestoVenta")]
        public virtual tblAdmTipoDescuento? idAdmTipoDescuentoNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmPresupuestoVenta")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [ForeignKey("idIvaNPais")]
        [InverseProperty("tblAdmPresupuestoVenta")]
        public virtual tblIvaNPais? idIvaNPaisNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAdmPresupuestoVenta")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idTipoPresupuesto")]
        [InverseProperty("tblAdmPresupuestoVenta")]
        public virtual tblAdmTipoElemento? idTipoPresupuestoNavigation { get; set; }
        [InverseProperty("idAdmPresupuestoVentaNavigation")]
        public virtual ICollection<tblAdmArticuloNAdmPresupuestoVenta> tblAdmArticuloNAdmPresupuestoVenta { get; set; }
        [InverseProperty("idAdmPresupuestoVentaNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmPresupuestoVentaNavigation")]
        public virtual ICollection<tblArticuloNAdmPresupuestoVenta> tblArticuloNAdmPresupuestoVenta { get; set; }
    }
}
