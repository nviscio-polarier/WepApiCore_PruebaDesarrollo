using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmPedidoCliente", Schema = "Administracion")]
    public partial class tblAdmPedidoCliente
    {
        public tblAdmPedidoCliente()
        {
            tblAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
            tblAdmArticuloNAdmPedidoCliente = new HashSet<tblAdmArticuloNAdmPedidoCliente>();
            tblArticuloNAdmPedidoCliente = new HashSet<tblArticuloNAdmPedidoCliente>();
        }

        [Key]
        public int idAdmPedidoCliente { get; set; }
        [StringLength(12)]
        public string codigo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime? fechaCreacion { get; set; }
        public byte? idAdmPedido_Estado { get; set; }
        public byte? idMoneda { get; set; }
        public int? idAdmCliente { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal? tasaCambio { get; set; }
        public string? numPedidoCliente { get; set; }
        public int? idAdmFormaPago { get; set; }
        public byte? idTipoPedido { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idIncoterm { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public byte? idIvaNPais { get; set; }
        public string? observaciones { get; set; }
        public int? idAdmPresupuestoVenta { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public bool isCerrado { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCliente")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblAdmCliente? idAdmClienteNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idAdmFormaPago")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblAdmFormaPago? idAdmFormaPagoNavigation { get; set; }
        [ForeignKey("idAdmPedido_Estado")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblAdmPedido_Estado? idAdmPedido_EstadoNavigation { get; set; }
        [ForeignKey("idAdmPresupuestoVenta")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblAdmPresupuestoVenta? idAdmPresupuestoVentaNavigation { get; set; }
        [ForeignKey("idAdmTipoCambio")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblAdmTipoCambio? idAdmTipoCambioNavigation { get; set; }
        [ForeignKey("idAdmTipoDescuento")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblAdmTipoDescuento? idAdmTipoDescuentoNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [ForeignKey("idIncoterm")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblIncoterm? idIncotermNavigation { get; set; }
        [ForeignKey("idIvaNPais")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblIvaNPais? idIvaNPaisNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idTipoPedido")]
        [InverseProperty("tblAdmPedidoCliente")]
        public virtual tblAdmTipoElemento? idTipoPedidoNavigation { get; set; }
        [InverseProperty("idAdmPedidoClienteNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idAdmPedidoClienteNavigation")]
        public virtual ICollection<tblAdmArticuloNAdmPedidoCliente> tblAdmArticuloNAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmPedidoClienteNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoCliente> tblArticuloNAdmPedidoCliente { get; set; }
    }
}
