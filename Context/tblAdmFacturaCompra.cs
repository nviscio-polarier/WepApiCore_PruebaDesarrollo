using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmFacturaCompra", Schema = "Administracion")]
    public partial class tblAdmFacturaCompra
    {
        public tblAdmFacturaCompra()
        {
            tblAdmConceptoCompra = new HashSet<tblAdmConceptoCompra>();
            idAdmAlbaranCompra = new HashSet<tblAdmAlbaranCompra>();
        }

        [Key]
        public int idAdmFacturaCompra { get; set; }
        public byte? idAdmFactura_Estado { get; set; }
        public byte? idMoneda { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public int? idTipoFactura { get; set; }
        public int? idIncoterm { get; set; }
        [StringLength(12)]
        public string? codigo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? tasaCambio { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        public int? idAdmProveedor { get; set; }
        public int? idAdmFormaPago { get; set; }
        public string? observaciones { get; set; }
        public string? numFacturaProveedor { get; set; }
        public byte? idTipoAlbaran { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int? idAdmCondicionPago { get; set; }
        public bool isCerrado { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCondicionPago")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblAdmCondicionPago? idAdmCondicionPagoNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idAdmFactura_Estado")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblAdmFactura_Estado? idAdmFactura_EstadoNavigation { get; set; }
        [ForeignKey("idAdmFormaPago")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblAdmFormaPago? idAdmFormaPagoNavigation { get; set; }
        [ForeignKey("idAdmProveedor")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblAdmProveedor? idAdmProveedorNavigation { get; set; }
        [ForeignKey("idAdmTipoCambio")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblAdmTipoCambio? idAdmTipoCambioNavigation { get; set; }
        [ForeignKey("idAdmTipoDescuento")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblAdmTipoDescuento? idAdmTipoDescuentoNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [ForeignKey("idIncoterm")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblIncoterm? idIncotermNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idTipoAlbaran")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblAdmTipoElemento? idTipoAlbaranNavigation { get; set; }
        [ForeignKey("idTipoFactura")]
        [InverseProperty("tblAdmFacturaCompra")]
        public virtual tblAdmTipoFactura? idTipoFacturaNavigation { get; set; }
        [InverseProperty("idAdmFacturaCompraNavigation")]
        public virtual ICollection<tblAdmConceptoCompra> tblAdmConceptoCompra { get; set; }

        [ForeignKey("idAdmFacturaCompra")]
        [InverseProperty("idAdmFacturaCompra")]
        public virtual ICollection<tblAdmAlbaranCompra> idAdmAlbaranCompra { get; set; }
    }
}
