using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPorcentajeValorPrendaNEntidad", Schema = "Inventarios")]
    public partial class tblPorcentajeValorPrendaNEntidad
    {
        [Key]
        public int idEntidad { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal porcentajeValor { get; set; }
        [Key]
        public byte numMeses { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblPorcentajeValorPrendaNEntidad")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
    }
}
