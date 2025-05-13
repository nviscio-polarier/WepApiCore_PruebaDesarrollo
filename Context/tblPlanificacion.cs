using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPlanificacion", Schema = "ControlPresupuestario")]
    public partial class tblPlanificacion
    {
        [Key]
        public int idPlanificacion { get; set; }
        public short idEmpresaPolarier { get; set; }
        public int idAdmCuentaContable { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public byte idMoneda { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal valorPresupuestado { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblPlanificacion")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblPlanificacion")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblPlanificacion")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblPlanificacion")]
        public virtual tblEmpresasPolarier idEmpresaPolarierNavigation { get; set; } = null!;
        [ForeignKey("idMoneda")]
        [InverseProperty("tblPlanificacion")]
        public virtual tblMoneda idMonedaNavigation { get; set; } = null!;
    }
}
