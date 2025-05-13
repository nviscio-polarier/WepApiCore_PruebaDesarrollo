using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmAlbaranCompra", Schema = "Administracion")]
    public partial class tblAdmAlbaranCompra
    {
        public tblAdmAlbaranCompra()
        {
            tblAdmArticuloNAdmAlbaranCompra = new HashSet<tblAdmArticuloNAdmAlbaranCompra>();
            tblArticuloNAdmAlbaranCompra = new HashSet<tblArticuloNAdmAlbaranCompra>();
            idAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
        }

        [Key]
        public int idAdmAlbaranCompra { get; set; }
        [StringLength(12)]
        public string codigo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime? fechaCreacion { get; set; }
        public byte? idTipoAlbaran { get; set; }
        public byte? idAdmAlbaran_Estado { get; set; }
        public int? idAdmProveedor { get; set; }
        public byte? idMoneda { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal? tasaCambio { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public int? idAdmFormaPago { get; set; }
        public int? idIncoterm { get; set; }
        public int? idAdmPedidoProveedor { get; set; }
        public string? observaciones { get; set; }
        public string? numAlbaranProveedor { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public bool isCerrado { get; set; }

        [ForeignKey("idAdmAlbaran_Estado")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblAdmAlbaran_Estado? idAdmAlbaran_EstadoNavigation { get; set; }
        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idAdmFormaPago")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblAdmFormaPago? idAdmFormaPagoNavigation { get; set; }
        [ForeignKey("idAdmPedidoProveedor")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblAdmPedidoProveedor? idAdmPedidoProveedorNavigation { get; set; }
        [ForeignKey("idAdmProveedor")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblAdmProveedor? idAdmProveedorNavigation { get; set; }
        [ForeignKey("idAdmTipoCambio")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblAdmTipoCambio? idAdmTipoCambioNavigation { get; set; }
        [ForeignKey("idAdmTipoDescuento")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblAdmTipoDescuento? idAdmTipoDescuentoNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [ForeignKey("idIncoterm")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblIncoterm? idIncotermNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idTipoAlbaran")]
        [InverseProperty("tblAdmAlbaranCompra")]
        public virtual tblAdmTipoElemento? idTipoAlbaranNavigation { get; set; }
        [InverseProperty("idAdmAlbaranCompraNavigation")]
        public virtual ICollection<tblAdmArticuloNAdmAlbaranCompra> tblAdmArticuloNAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmAlbaranCompraNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranCompra> tblArticuloNAdmAlbaranCompra { get; set; }

        [ForeignKey("idAdmAlbaranCompra")]
        [InverseProperty("idAdmAlbaranCompra")]
        public virtual ICollection<tblAdmFacturaCompra> idAdmFacturaCompra { get; set; }
    }
}
