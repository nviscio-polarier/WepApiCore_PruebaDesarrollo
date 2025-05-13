using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEmpresasPolarier", Schema = "Finanzas")]
    public partial class tblEmpresasPolarier
    {
        public tblEmpresasPolarier()
        {
            tblAdmAlbaranCompra = new HashSet<tblAdmAlbaranCompra>();
            tblAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
            tblAdmCentroCoste = new HashSet<tblAdmCentroCoste>();
            tblAdmCliente = new HashSet<tblAdmCliente>();
            tblAdmCuentaBancaria = new HashSet<tblAdmCuentaBancaria>();
            tblAdmElementoPEP = new HashSet<tblAdmElementoPEP>();
            tblAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
            tblAdmPedidoCliente = new HashSet<tblAdmPedidoCliente>();
            tblAdmPedidoProveedor = new HashSet<tblAdmPedidoProveedor>();
            tblAdmPresupuestoVenta = new HashSet<tblAdmPresupuestoVenta>();
            tblAdmProveedor = new HashSet<tblAdmProveedor>();
            tblCierrePresupuestario = new HashSet<tblCierrePresupuestario>();
            tblHistoricoPlanificacion = new HashSet<tblHistoricoPlanificacion>();
            tblLavanderia = new HashSet<tblLavanderia>();
            tblNomina = new HashSet<tblNomina>();
            tblPersona = new HashSet<tblPersona>();
            idUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public short idEmpresaPolarier { get; set; }
        public string denominacion { get; set; } = null!;
        public string ejercicioGsBase { get; set; } = null!;
        public byte idMoneda { get; set; }
        [Column(TypeName = "decimal(6, 5)")]
        public decimal? segAccidenteConvenio { get; set; }
        [StringLength(6)]
        public string? companyCode_SAP { get; set; }
        public string? CIF { get; set; }
        public string? nombreFiscal { get; set; }
        public string? direccion { get; set; }
        public int? idPais { get; set; }
        public string? textoLateral_facturaVenta { get; set; }
        public string? textoInferior_facturaVenta { get; set; }
        public string? numInscripcionSegSocial { get; set; }

        [ForeignKey("idMoneda")]
        [InverseProperty("tblEmpresasPolarier")]
        public virtual tblMoneda idMonedaNavigation { get; set; } = null!;
        [ForeignKey("idPais")]
        [InverseProperty("tblEmpresasPolarier")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmCentroCoste> tblAdmCentroCoste { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmCliente> tblAdmCliente { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmCuentaBancaria> tblAdmCuentaBancaria { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmElementoPEP> tblAdmElementoPEP { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmPresupuestoVenta> tblAdmPresupuestoVenta { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblAdmProveedor> tblAdmProveedor { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblCierrePresupuestario> tblCierrePresupuestario { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblHistoricoPlanificacion> tblHistoricoPlanificacion { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblLavanderia> tblLavanderia { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
        [InverseProperty("idEmpresaPolarierNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }

        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("idEmpresaPolarier")]
        public virtual ICollection<tblUsuario> idUsuario { get; set; }
    }
}
