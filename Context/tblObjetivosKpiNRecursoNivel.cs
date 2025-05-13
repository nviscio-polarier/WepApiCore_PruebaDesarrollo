using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblObjetivosKpiNRecursoNivel", Schema = "General")]
    public partial class tblObjetivosKpiNRecursoNivel
    {
        [Key]
        public int idLavanderia { get; set; }
        [Key]
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        [Key]
        public int idRecursoNivel { get; set; }
        [Column(TypeName = "decimal(9, 5)")]
        public decimal costeUd { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal kpiOperativo { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblObjetivosKpiNRecursoNivel")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idRecursoNivel")]
        [InverseProperty("tblObjetivosKpiNRecursoNivel")]
        public virtual tblRecursoNivel idRecursoNivelNavigation { get; set; } = null!;
    }
}
