using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblControlContador", Schema = "Energeticos")]
    [Index("fecha", Name = "IX_tblControlContador_fecha")]
    public partial class tblControlContador
    {
        [Key]
        public int idRecursoContador { get; set; }
        [Column(TypeName = "numeric(14, 2)")]
        public decimal actual { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "numeric(14, 2)")]
        public decimal? finalContador { get; set; }
        [Column(TypeName = "numeric(14, 2)")]
        public decimal? inicioNuevoContador { get; set; }
        [Column(TypeName = "numeric(14, 2)")]
        public decimal? diferencia { get; set; }
        public bool sumaInforme { get; set; }

        [ForeignKey("idRecursoContador")]
        [InverseProperty("tblControlContador")]
        public virtual tblRecursoContador idRecursoContadorNavigation { get; set; } = null!;
    }
}
