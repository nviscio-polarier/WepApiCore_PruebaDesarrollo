using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCierrePresupuestario", Schema = "ControlPresupuestario")]
    public partial class tblCierrePresupuestario
    {
        public tblCierrePresupuestario()
        {
            tblPartidaNCierrePresupuestario = new HashSet<tblPartidaNCierrePresupuestario>();
            tblPlanificacionNCierrePresupuestario = new HashSet<tblPlanificacionNCierrePresupuestario>();
        }

        [Key]
        public int idCierrePresupuestario { get; set; }
        public short idEmpresaPolarier { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblCierrePresupuestario")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblCierrePresupuestario")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblCierrePresupuestario")]
        public virtual tblEmpresasPolarier idEmpresaPolarierNavigation { get; set; } = null!;
        [InverseProperty("idCierrePresupuestarioNavigation")]
        public virtual ICollection<tblPartidaNCierrePresupuestario> tblPartidaNCierrePresupuestario { get; set; }
        [InverseProperty("idCierrePresupuestarioNavigation")]
        public virtual ICollection<tblPlanificacionNCierrePresupuestario> tblPlanificacionNCierrePresupuestario { get; set; }
    }
}
