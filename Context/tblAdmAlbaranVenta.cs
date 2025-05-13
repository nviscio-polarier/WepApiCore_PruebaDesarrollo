using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmAlbaranVenta", Schema = "Administracion")]
    public partial class tblAdmAlbaranVenta
    {
        public tblAdmAlbaranVenta()
        {
            tblAdmArticuloNAdmAlbaranVenta = new HashSet<tblAdmArticuloNAdmAlbaranVenta>();
            tblArticuloNAdmAlbaranVenta = new HashSet<tblArticuloNAdmAlbaranVenta>();
            idAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
        }

        [Key]
        public int idAdmAlbaranVenta { get; set; }
        [StringLength(12)]
        public string? codigo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaCreacion { get; set; }
        public byte? idTipoAlbaran { get; set; }
        public byte? idAdmAlbaran_Estado { get; set; }
        public int? idAdmCliente { get; set; }
        public byte? idMoneda { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal? tasaCambio { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public int? idAdmFormaPago { get; set; }
        public int? idTipoFactura { get; set; }
        public byte? idIvaNPais { get; set; }
        public string? observaciones { get; set; }
        public int? idAdmPedidoCliente { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int? idIncoterm { get; set; }
        public bool isCerrado { get; set; }

        [ForeignKey("idAdmAlbaran_Estado")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblAdmAlbaran_Estado? idAdmAlbaran_EstadoNavigation { get; set; }
        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCliente")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblAdmCliente? idAdmClienteNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idAdmFormaPago")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblAdmFormaPago? idAdmFormaPagoNavigation { get; set; }
        [ForeignKey("idAdmPedidoCliente")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblAdmPedidoCliente? idAdmPedidoClienteNavigation { get; set; }
        [ForeignKey("idAdmTipoCambio")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblAdmTipoCambio? idAdmTipoCambioNavigation { get; set; }
        [ForeignKey("idAdmTipoDescuento")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblAdmTipoDescuento? idAdmTipoDescuentoNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [ForeignKey("idIncoterm")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblIncoterm? idIncotermNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idTipoAlbaran")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblAdmTipoElemento? idTipoAlbaranNavigation { get; set; }
        [ForeignKey("idTipoFactura")]
        [InverseProperty("tblAdmAlbaranVenta")]
        public virtual tblAdmTipoFactura? idTipoFacturaNavigation { get; set; }
        [InverseProperty("idAdmAlbaranVentaNavigation")]
        public virtual ICollection<tblAdmArticuloNAdmAlbaranVenta> tblAdmArticuloNAdmAlbaranVenta { get; set; }
        [InverseProperty("idAdmAlbaranVentaNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranVenta> tblArticuloNAdmAlbaranVenta { get; set; }

        [ForeignKey("idAdmAlbaranVenta")]
        [InverseProperty("idAdmAlbaranVenta")]
        public virtual ICollection<tblAdmFacturaVenta> idAdmFacturaVenta { get; set; }
    }
}
