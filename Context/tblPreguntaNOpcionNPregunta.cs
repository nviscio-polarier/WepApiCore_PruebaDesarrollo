using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPreguntaNOpcionNPregunta", Schema = "ControlCalidad")]
    public partial class tblPreguntaNOpcionNPregunta
    {
        [Key]
        public int idPregunta { get; set; }
        [Key]
        public short idOpcion { get; set; }
        public int idPreguntaAnidada { get; set; }

        [ForeignKey("idOpcion")]
        [InverseProperty("tblPreguntaNOpcionNPregunta")]
        public virtual tblOpcion idOpcionNavigation { get; set; } = null!;
        [ForeignKey("idPreguntaAnidada")]
        [InverseProperty("tblPreguntaNOpcionNPreguntaidPreguntaAnidadaNavigation")]
        public virtual tblPregunta idPreguntaAnidadaNavigation { get; set; } = null!;
        [ForeignKey("idPregunta")]
        [InverseProperty("tblPreguntaNOpcionNPreguntaidPreguntaNavigation")]
        public virtual tblPregunta idPreguntaNavigation { get; set; } = null!;
    }
}
