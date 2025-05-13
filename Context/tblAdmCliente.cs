using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmCliente", Schema = "Administracion")]
    public partial class tblAdmCliente
    {
        public tblAdmCliente()
        {
            tblAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
            tblAdmPedidoCliente = new HashSet<tblAdmPedidoCliente>();
            tblAdmPresupuestoVenta = new HashSet<tblAdmPresupuestoVenta>();
            tblImagenNCliente = new HashSet<tblImagenNCliente>();
        }

        [Key]
        public int idAdmCliente { get; set; }
        public short idEmpresaPolarier { get; set; }
        [StringLength(8)]
        public string codigo { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public string nombreFiscal { get; set; } = null!;
        public string nombreComercial { get; set; } = null!;
        public string CIF { get; set; } = null!;
        public string direccion { get; set; } = null!;
        public bool isEliminado { get; set; }
        public string? tipoRetencion { get; set; }
        [Column(TypeName = "decimal(5, 3)")]
        public decimal? codigoRetencion { get; set; }
        public string? codigoPostal { get; set; }
        public string? poblacion { get; set; }
        public string? provincia { get; set; }
        public bool? isModificableNFactura { get; set; }
        public short? diaPago { get; set; }
        public int? idPais { get; set; }
        public int? idAdmFormaCobro { get; set; }
        public int? idAdmCondicionPago { get; set; }
        public short? idCuentaBancaria { get; set; }
        public byte? idMoneda { get; set; }
        public byte? idIvaNPais { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public string? sociedadGL { get; set; }
        public string? formaPagoMXN { get; set; }
        public string? usoCFDI { get; set; }
        public string? producto { get; set; }
        public string? cantidad { get; set; }
        public string? unidadMedida { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblAdmCliente")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCondicionPago")]
        [InverseProperty("tblAdmCliente")]
        public virtual tblAdmCondicionPago? idAdmCondicionPagoNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblAdmCliente")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idAdmFormaCobro")]
        [InverseProperty("tblAdmCliente")]
        public virtual tblAdmFormaPago? idAdmFormaCobroNavigation { get; set; }
        [ForeignKey("idCuentaBancaria")]
        [InverseProperty("tblAdmCliente")]
        public virtual tblAdmCuentaBancaria? idCuentaBancariaNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmCliente")]
        public virtual tblEmpresasPolarier idEmpresaPolarierNavigation { get; set; } = null!;
        [ForeignKey("idIvaNPais")]
        [InverseProperty("tblAdmCliente")]
        public virtual tblIvaNPais? idIvaNPaisNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAdmCliente")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idPais")]
        [InverseProperty("tblAdmCliente")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [InverseProperty("idAdmClienteNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idAdmClienteNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idAdmClienteNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmClienteNavigation")]
        public virtual ICollection<tblAdmPresupuestoVenta> tblAdmPresupuestoVenta { get; set; }
        [InverseProperty("idAdmClienteNavigation")]
        public virtual ICollection<tblImagenNCliente> tblImagenNCliente { get; set; }
    }
}
