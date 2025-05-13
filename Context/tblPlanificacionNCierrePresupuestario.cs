using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPlanificacionNCierrePresupuestario", Schema = "ControlPresupuestario")]
    public partial class tblPlanificacionNCierrePresupuestario
    {
        [Key]
        public int idPlanificacionNCierrePresupuestario { get; set; }
        public int idCierrePresupuestario { get; set; }
        public int idAdmCuentaContable { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal valor { get; set; }
        public byte idMoneda { get; set; }

        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblPlanificacionNCierrePresupuestario")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idCierrePresupuestario")]
        [InverseProperty("tblPlanificacionNCierrePresupuestario")]
        public virtual tblCierrePresupuestario idCierrePresupuestarioNavigation { get; set; } = null!;
        [ForeignKey("idMoneda")]
        [InverseProperty("tblPlanificacionNCierrePresupuestario")]
        public virtual tblMoneda idMonedaNavigation { get; set; } = null!;
    }
}
