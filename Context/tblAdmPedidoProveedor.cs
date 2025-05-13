using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmPedidoProveedor", Schema = "Administracion")]
    public partial class tblAdmPedidoProveedor
    {
        public tblAdmPedidoProveedor()
        {
            tblAdmAlbaranCompra = new HashSet<tblAdmAlbaranCompra>();
            tblAdmArticuloNAdmPedidoProveedor = new HashSet<tblAdmArticuloNAdmPedidoProveedor>();
            tblArticuloNAdmPedidoProveedor = new HashSet<tblArticuloNAdmPedidoProveedor>();
        }

        [Key]
        public int idAdmPedidoProveedor { get; set; }
        public byte? idTipoPedido { get; set; }
        [StringLength(12)]
        public string codigo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime? fechaCreacion { get; set; }
        public byte? idAdmPedido_Estado { get; set; }
        public byte? idMoneda { get; set; }
        public int? idAdmProveedor { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal? tasaCambio { get; set; }
        public int? idAdmFormaPago { get; set; }
        public int? idAdmCentroCoste { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public int? idIncoterm { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaEstimadaRecepcion { get; set; }
        public string? numPresupuestoProveedor { get; set; }
        public string? observaciones { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public bool isCerrado { get; set; }
        public string? direccionEntrega { get; set; }
        public int? idCentroTrabajo { get; set; }
        public int? idLavanderia { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idAdmFormaPago")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblAdmFormaPago? idAdmFormaPagoNavigation { get; set; }
        [ForeignKey("idAdmPedido_Estado")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblAdmPedido_Estado? idAdmPedido_EstadoNavigation { get; set; }
        [ForeignKey("idAdmProveedor")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblAdmProveedor? idAdmProveedorNavigation { get; set; }
        [ForeignKey("idAdmTipoCambio")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblAdmTipoCambio? idAdmTipoCambioNavigation { get; set; }
        [ForeignKey("idAdmTipoDescuento")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblAdmTipoDescuento? idAdmTipoDescuentoNavigation { get; set; }
        [ForeignKey("idCentroTrabajo")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblLavanderia? idCentroTrabajo1 { get; set; }
        [ForeignKey("idCentroTrabajo")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblCentroTrabajo? idCentroTrabajoNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [ForeignKey("idIncoterm")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblIncoterm? idIncotermNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idTipoPedido")]
        [InverseProperty("tblAdmPedidoProveedor")]
        public virtual tblAdmTipoElemento? idTipoPedidoNavigation { get; set; }
        [InverseProperty("idAdmPedidoProveedorNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmPedidoProveedorNavigation")]
        public virtual ICollection<tblAdmArticuloNAdmPedidoProveedor> tblAdmArticuloNAdmPedidoProveedor { get; set; }
        [InverseProperty("idAdmPedidoProveedorNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoProveedor> tblArticuloNAdmPedidoProveedor { get; set; }
    }
}
