using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblGrupoEnergetico", Schema = "Energeticos")]
    public partial class tblGrupoEnergetico
    {
        public tblGrupoEnergetico()
        {
            tblRecursoContador = new HashSet<tblRecursoContador>();
        }

        [Key]
        public byte idGrupoEnergetico { get; set; }
        public string denominacion { get; set; } = null!;
        public byte idTipoKpi { get; set; }
        public int idTraduccion { get; set; }

        [ForeignKey("idTipoKpi")]
        [InverseProperty("tblGrupoEnergetico")]
        public virtual tblTipoKpi idTipoKpiNavigation { get; set; } = null!;
        [ForeignKey("idTraduccion")]
        [InverseProperty("tblGrupoEnergetico")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idGrupoEnergeticoNavigation")]
        public virtual ICollection<tblRecursoContador> tblRecursoContador { get; set; }
    }
}
