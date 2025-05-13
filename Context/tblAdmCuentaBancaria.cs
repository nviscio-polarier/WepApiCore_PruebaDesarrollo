using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmCuentaBancaria", Schema = "Administracion")]
    public partial class tblAdmCuentaBancaria
    {
        public tblAdmCuentaBancaria()
        {
            tblAdmCliente = new HashSet<tblAdmCliente>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
        }

        [Key]
        public short idCuentaBancaria { get; set; }
        public byte idAdmBanco { get; set; }
        public short idEmpresaPolarier { get; set; }
        [StringLength(5)]
        public string codigo { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public string IBAN { get; set; } = null!;
        public string? SWIFT { get; set; }
        public byte? idMoneda { get; set; }
        public string? idCuenta_SAP { get; set; }

        [ForeignKey("idAdmBanco")]
        [InverseProperty("tblAdmCuentaBancaria")]
        public virtual tblAdmBanco idAdmBancoNavigation { get; set; } = null!;
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmCuentaBancaria")]
        public virtual tblEmpresasPolarier idEmpresaPolarierNavigation { get; set; } = null!;
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAdmCuentaBancaria")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [InverseProperty("idCuentaBancariaNavigation")]
        public virtual ICollection<tblAdmCliente> tblAdmCliente { get; set; }
        [InverseProperty("idCuentaBancariaNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
    }
}
