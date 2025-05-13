using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoKpi", Schema = "General")]
    public partial class tblTipoKpi
    {
        public tblTipoKpi()
        {
            tblGrupoEnergetico = new HashSet<tblGrupoEnergetico>();
            tblObjetivosKpi = new HashSet<tblObjetivosKpi>();
            tblTipoKpi_Observaciones = new HashSet<tblTipoKpi_Observaciones>();
        }

        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [Key]
        public byte idTipoKpi { get; set; }

        [InverseProperty("idTipoKpiNavigation")]
        public virtual ICollection<tblGrupoEnergetico> tblGrupoEnergetico { get; set; }
        [InverseProperty("idTipoKpiNavigation")]
        public virtual ICollection<tblObjetivosKpi> tblObjetivosKpi { get; set; }
        [InverseProperty("idTipoKpiNavigation")]
        public virtual ICollection<tblTipoKpi_Observaciones> tblTipoKpi_Observaciones { get; set; }
    }
}
