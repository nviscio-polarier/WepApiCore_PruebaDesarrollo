using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLecturaContador", Schema = "MyRealData")]
    [Index("fecha", Name = "IX_tblLecturaContador_fecha")]
    public partial class tblLecturaContador
    {
        [Key]
        public int idLecturaContador { get; set; }
        [Precision(0)]
        public DateTimeOffset? fecha { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal valor { get; set; }
        public int? idRecursoContador { get; set; }

        [ForeignKey("idRecursoContador")]
        [InverseProperty("tblLecturaContador")]
        public virtual tblRecursoContador? idRecursoContadorNavigation { get; set; }
    }
}
