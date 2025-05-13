using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCuentaContableNCentroTrabajo", Schema = "RRHH")]
    public partial class tblCuentaContableNCentroTrabajo
    {
        [Key]
        public int idCentroTrabajo { get; set; }
        public int? idAdmCuentaContable_Salario { get; set; }
        public int? idAdmCuentaContable_SSEmpresa { get; set; }
        public int? idAdmCuentaContable_Sueldo_MX { get; set; }
        public int? idAdmCuentaContable_IMSS_MX { get; set; }
        public int? idAdmCuentaContable_INFONAVIT_MX { get; set; }
        public int? idAdmCuentaContable_SAR_MX { get; set; }
        public int? idAdmCuentaContable_ImpEstatalNominas_MX { get; set; }

        [ForeignKey("idAdmCuentaContable_IMSS_MX")]
        [InverseProperty("tblCuentaContableNCentroTrabajoidAdmCuentaContable_IMSS_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_IMSS_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_INFONAVIT_MX")]
        [InverseProperty("tblCuentaContableNCentroTrabajoidAdmCuentaContable_INFONAVIT_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_INFONAVIT_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_ImpEstatalNominas_MX")]
        [InverseProperty("tblCuentaContableNCentroTrabajoidAdmCuentaContable_ImpEstatalNominas_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_ImpEstatalNominas_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_SAR_MX")]
        [InverseProperty("tblCuentaContableNCentroTrabajoidAdmCuentaContable_SAR_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SAR_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_SSEmpresa")]
        [InverseProperty("tblCuentaContableNCentroTrabajoidAdmCuentaContable_SSEmpresaNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SSEmpresaNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_Salario")]
        [InverseProperty("tblCuentaContableNCentroTrabajoidAdmCuentaContable_SalarioNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SalarioNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_Sueldo_MX")]
        [InverseProperty("tblCuentaContableNCentroTrabajoidAdmCuentaContable_Sueldo_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_Sueldo_MXNavigation { get; set; }
        [ForeignKey("idCentroTrabajo")]
        [InverseProperty("tblCuentaContableNCentroTrabajo")]
        public virtual tblCentroTrabajo idCentroTrabajoNavigation { get; set; } = null!;
    }
}
