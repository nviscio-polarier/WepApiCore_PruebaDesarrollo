using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblObjetivosKpi", Schema = "General")]
    public partial class tblObjetivosKpi
    {
        [Key]
        public int idLavanderia { get; set; }
        [Key]
        public byte idTipoKpi { get; set; }
        [Key]
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "decimal(7, 4)")]
        public decimal? kpiOperativo { get; set; }
        [Column(TypeName = "decimal(9, 5)")]
        public decimal costeUd { get; set; }
        [Column(TypeName = "decimal(9, 5)")]
        public decimal? costeTeorico { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblObjetivosKpi")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idTipoKpi")]
        [InverseProperty("tblObjetivosKpi")]
        public virtual tblTipoKpi idTipoKpiNavigation { get; set; } = null!;
    }
}
