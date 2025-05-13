using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCuentaContableNTipoTrabajo", Schema = "RRHH")]
    public partial class tblCuentaContableNTipoTrabajo
    {
        [Key]
        public byte idTipoTrabajo { get; set; }
        public int? idAdmCuentaContable_Salario { get; set; }
        public int? idAdmCuentaContable_SSEmpresa { get; set; }
        public int? idAdmCuentaContable_Sueldo_MX { get; set; }
        public int? idAdmCuentaContable_IMSS_MX { get; set; }
        public int? idAdmCuentaContable_INFONAVIT_MX { get; set; }
        public int? idAdmCuentaContable_SAR_MX { get; set; }
        public int? idAdmCuentaContable_ImpEstatalNominas_MX { get; set; }

        [ForeignKey("idAdmCuentaContable_IMSS_MX")]
        [InverseProperty("tblCuentaContableNTipoTrabajoidAdmCuentaContable_IMSS_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_IMSS_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_INFONAVIT_MX")]
        [InverseProperty("tblCuentaContableNTipoTrabajoidAdmCuentaContable_INFONAVIT_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_INFONAVIT_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_ImpEstatalNominas_MX")]
        [InverseProperty("tblCuentaContableNTipoTrabajoidAdmCuentaContable_ImpEstatalNominas_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_ImpEstatalNominas_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_SAR_MX")]
        [InverseProperty("tblCuentaContableNTipoTrabajoidAdmCuentaContable_SAR_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SAR_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_SSEmpresa")]
        [InverseProperty("tblCuentaContableNTipoTrabajoidAdmCuentaContable_SSEmpresaNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SSEmpresaNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_Salario")]
        [InverseProperty("tblCuentaContableNTipoTrabajoidAdmCuentaContable_SalarioNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SalarioNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_Sueldo_MX")]
        [InverseProperty("tblCuentaContableNTipoTrabajoidAdmCuentaContable_Sueldo_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_Sueldo_MXNavigation { get; set; }
        [ForeignKey("idTipoTrabajo")]
        [InverseProperty("tblCuentaContableNTipoTrabajo")]
        public virtual tblTipoTrabajo idTipoTrabajoNavigation { get; set; } = null!;
    }
}
