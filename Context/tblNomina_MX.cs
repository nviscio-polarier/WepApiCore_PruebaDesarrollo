using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblNomina_MX", Schema = "RRHH")]
    public partial class tblNomina_MX
    {
        public tblNomina_MX()
        {
            tblHistoricoAsientoNomina_MX = new HashSet<tblHistoricoAsientoNomina_MX>();
        }

        [Key]
        public int idNomina_MX { get; set; }
        public int idPersona { get; set; }
        public short idTipoNomina_MX { get; set; }
        public string? nombreCompleto { get; set; }
        [Column(TypeName = "date")]
        public DateTime? inicioPeriodo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? finPeriodo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal sueldo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal horasExtras { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal primaVacacional { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal primaDominical { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal bono { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal descansoTrabajado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal aguinaldo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal valesDespensa { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal fondoAhorro { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal otraPercepcion { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal gastosSindicales { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PTU { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal infonavitEmpleado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal fonacotEmpleado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal IMSSEmpleado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal SAREmpleado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ISREmpleado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal subsidioEmpleo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal devolucionPrestamo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal otrasDeducciones { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal descAlimentos { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal totalDeducciones { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal totalSP { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal totalSV { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal percepcionNeta { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal IMSSPatronal { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal infonavitPatronal { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal SARPatronal { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal impuestoEstatalSobreNominas { get; set; }
        public bool contabilizado { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int? idAdmCuentaContable_Sueldo_MX { get; set; }
        public int? idAdmCuentaContable_IMSS_MX { get; set; }
        public int? idAdmCuentaContable_INFONAVIT_MX { get; set; }
        public int? idAdmCuentaContable_SAR_MX { get; set; }
        public int? idAdmCuentaContable_ImpEstatalNominas_MX { get; set; }
        public byte? idTipoTrabajo { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblNomina_MX")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_IMSS_MX")]
        [InverseProperty("tblNomina_MXidAdmCuentaContable_IMSS_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_IMSS_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_INFONAVIT_MX")]
        [InverseProperty("tblNomina_MXidAdmCuentaContable_INFONAVIT_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_INFONAVIT_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_ImpEstatalNominas_MX")]
        [InverseProperty("tblNomina_MXidAdmCuentaContable_ImpEstatalNominas_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_ImpEstatalNominas_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_SAR_MX")]
        [InverseProperty("tblNomina_MXidAdmCuentaContable_SAR_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SAR_MXNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_Sueldo_MX")]
        [InverseProperty("tblNomina_MXidAdmCuentaContable_Sueldo_MXNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_Sueldo_MXNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblNomina_MX")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblNomina_MX")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTipoNomina_MX")]
        [InverseProperty("tblNomina_MX")]
        public virtual tblTipoNomina_MX idTipoNomina_MXNavigation { get; set; } = null!;
        [ForeignKey("idTipoTrabajo")]
        [InverseProperty("tblNomina_MX")]
        public virtual tblTipoTrabajo? idTipoTrabajoNavigation { get; set; }
        [InverseProperty("idNomina_MXNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina_MX> tblHistoricoAsientoNomina_MX { get; set; }
    }
}
