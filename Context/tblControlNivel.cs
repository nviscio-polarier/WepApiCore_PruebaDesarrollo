using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblControlNivel", Schema = "Energeticos")]
    public partial class tblControlNivel
    {
        [Key]
        public int idRecursoNivel { get; set; }
        [Column(TypeName = "numeric(14, 2)")]
        public decimal? reposicion { get; set; }
        [Column(TypeName = "numeric(14, 2)")]
        public decimal actual { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        [ForeignKey("idRecursoNivel")]
        [InverseProperty("tblControlNivel")]
        public virtual tblRecursoNivel idRecursoNivelNavigation { get; set; } = null!;
    }
}
