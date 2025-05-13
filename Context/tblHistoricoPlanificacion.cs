using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblHistoricoPlanificacion", Schema = "ControlPresupuestario")]
    public partial class tblHistoricoPlanificacion
    {
        public tblHistoricoPlanificacion()
        {
            tblHistoricoPartidaContable = new HashSet<tblHistoricoPartidaContable>();
        }

        [Key]
        public int idHistoricoPlanificacion { get; set; }
        public short idEmpresaPolarier { get; set; }
        public int idAdmCuentaContable { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal valorPresupuestado { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblHistoricoPlanificacion")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblHistoricoPlanificacion")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblHistoricoPlanificacion")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblHistoricoPlanificacion")]
        public virtual tblEmpresasPolarier idEmpresaPolarierNavigation { get; set; } = null!;
        [InverseProperty("idHistoricoPlanificacionNavigation")]
        public virtual ICollection<tblHistoricoPartidaContable> tblHistoricoPartidaContable { get; set; }
    }
}
