using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmFacturaVenta", Schema = "Administracion")]
    public partial class tblAdmFacturaVenta
    {
        public tblAdmFacturaVenta()
        {
            InverseidReferenciaFacturaVentaNavigation = new HashSet<tblAdmFacturaVenta>();
            tblAdmConceptoVenta = new HashSet<tblAdmConceptoVenta>();
            idAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
        }

        [Key]
        public int idAdmFacturaVenta { get; set; }
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
        [Column(TypeName = "date")]
        public DateTime? fechaVencimiento { get; set; }
        public string? comentario { get; set; }
        public int? idAdmCliente { get; set; }
        public int? idAdmFormaCobro { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuento { get; set; }
        public string? observaciones { get; set; }
        public string? numPedido { get; set; }
        public short? idCuentaBancaria { get; set; }
        public byte? idAdmTipoNCF { get; set; }
        [StringLength(11)]
        public string? NCF { get; set; }
        public byte? idTipoAlbaran { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public byte? idIvaNPais { get; set; }
        public int? idAdmCondicionPago { get; set; }
        public bool isCerrado { get; set; }
        public int? idReferenciaFacturaVenta { get; set; }
        public string? tipoRetencion { get; set; }
        [Column(TypeName = "decimal(5, 3)")]
        public decimal? codigoRetencion { get; set; }
        public bool? aplicaRetencion { get; set; }
        [Column(TypeName = "decimal(10, 5)")]
        public decimal? tasaCambio { get; set; }
        public string? sociedadGL { get; set; }
        public string? formaPagoMXN { get; set; }
        public string? usoCFDI { get; set; }
        public string? producto { get; set; }
        public string? cantidad { get; set; }
        public string? unidadMedida { get; set; }
        [StringLength(12)]
        public string? clvRef1 { get; set; }
        [StringLength(12)]
        public string? clvRef2 { get; set; }
        [StringLength(12)]
        public string? clvRef3 { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCliente")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmCliente? idAdmClienteNavigation { get; set; }
        [ForeignKey("idAdmCondicionPago")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmCondicionPago? idAdmCondicionPagoNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idAdmFactura_Estado")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmFactura_Estado? idAdmFactura_EstadoNavigation { get; set; }
        [ForeignKey("idAdmFormaCobro")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmFormaPago? idAdmFormaCobroNavigation { get; set; }
        [ForeignKey("idAdmTipoCambio")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmTipoCambio? idAdmTipoCambioNavigation { get; set; }
        [ForeignKey("idAdmTipoDescuento")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmTipoDescuento? idAdmTipoDescuentoNavigation { get; set; }
        [ForeignKey("idAdmTipoNCF")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmTipoNCF? idAdmTipoNCFNavigation { get; set; }
        [ForeignKey("idCuentaBancaria")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmCuentaBancaria? idCuentaBancariaNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [ForeignKey("idIncoterm")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblIncoterm? idIncotermNavigation { get; set; }
        [ForeignKey("idIvaNPais")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblIvaNPais? idIvaNPaisNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idReferenciaFacturaVenta")]
        [InverseProperty("InverseidReferenciaFacturaVentaNavigation")]
        public virtual tblAdmFacturaVenta? idReferenciaFacturaVentaNavigation { get; set; }
        [ForeignKey("idTipoAlbaran")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmTipoElemento? idTipoAlbaranNavigation { get; set; }
        [ForeignKey("idTipoFactura")]
        [InverseProperty("tblAdmFacturaVenta")]
        public virtual tblAdmTipoFactura? idTipoFacturaNavigation { get; set; }
        [InverseProperty("idAdmFacturaVentaNavigation")]
        public virtual tblTimbradoMXNAdmFacturaVenta tblTimbradoMXNAdmFacturaVenta { get; set; } = null!;
        [InverseProperty("idReferenciaFacturaVentaNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> InverseidReferenciaFacturaVentaNavigation { get; set; }
        [InverseProperty("idAdmFacturaVentaNavigation")]
        public virtual ICollection<tblAdmConceptoVenta> tblAdmConceptoVenta { get; set; }

        [ForeignKey("idAdmFacturaVenta")]
        [InverseProperty("idAdmFacturaVenta")]
        public virtual ICollection<tblAdmAlbaranVenta> idAdmAlbaranVenta { get; set; }
    }
}
