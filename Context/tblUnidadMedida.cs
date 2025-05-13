using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblUnidadMedida", Schema = "Energeticos")]
    public partial class tblUnidadMedida
    {
        public tblUnidadMedida()
        {
            tblRecursoContadoridUnidadMedida_ContadorNavigation = new HashSet<tblRecursoContador>();
            tblRecursoContadoridUnidadMedida_KpiNavigation = new HashSet<tblRecursoContador>();
            tblRecursoNivelidUnidadMedidaInformeNavigation = new HashSet<tblRecursoNivel>();
            tblRecursoNivelidUnidadMedidaNavigation = new HashSet<tblRecursoNivel>();
        }

        [Key]
        public byte idUnidadMedida { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idUnidadMedida_ContadorNavigation")]
        public virtual ICollection<tblRecursoContador> tblRecursoContadoridUnidadMedida_ContadorNavigation { get; set; }
        [InverseProperty("idUnidadMedida_KpiNavigation")]
        public virtual ICollection<tblRecursoContador> tblRecursoContadoridUnidadMedida_KpiNavigation { get; set; }
        [InverseProperty("idUnidadMedidaInformeNavigation")]
        public virtual ICollection<tblRecursoNivel> tblRecursoNivelidUnidadMedidaInformeNavigation { get; set; }
        [InverseProperty("idUnidadMedidaNavigation")]
        public virtual ICollection<tblRecursoNivel> tblRecursoNivelidUnidadMedidaNavigation { get; set; }
    }
}
