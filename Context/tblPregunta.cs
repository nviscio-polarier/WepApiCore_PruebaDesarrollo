using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPregunta", Schema = "ControlCalidad")]
    public partial class tblPregunta
    {
        public tblPregunta()
        {
            tblPreguntaNOpcionNPreguntaidPreguntaAnidadaNavigation = new HashSet<tblPreguntaNOpcionNPregunta>();
            tblPreguntaNOpcionNPreguntaidPreguntaNavigation = new HashSet<tblPreguntaNOpcionNPregunta>();
            tblRespuesta = new HashSet<tblRespuesta>();
            idOpcion = new HashSet<tblOpcion>();
        }

        [Key]
        public int idPregunta { get; set; }
        public short idTipoPregunta { get; set; }
        public short? idGrupoPregunta { get; set; }
        public int idEncuestaPlantilla { get; set; }
        public string? descripcion { get; set; }
        public string? pregunta { get; set; }
        public byte? minRango { get; set; }
        public byte? maxRango { get; set; }
        public string? minRangoText { get; set; }
        public string? maxRangoText { get; set; }
        public short orden { get; set; }
        public bool isOpcional { get; set; }

        [ForeignKey("idEncuestaPlantilla")]
        [InverseProperty("tblPregunta")]
        public virtual tblEncuestaPlantilla idEncuestaPlantillaNavigation { get; set; } = null!;
        [ForeignKey("idGrupoPregunta")]
        [InverseProperty("tblPregunta")]
        public virtual tblGrupoPregunta? idGrupoPreguntaNavigation { get; set; }
        [ForeignKey("idTipoPregunta")]
        [InverseProperty("tblPregunta")]
        public virtual tblTipoPregunta idTipoPreguntaNavigation { get; set; } = null!;
        [InverseProperty("idPreguntaAnidadaNavigation")]
        public virtual ICollection<tblPreguntaNOpcionNPregunta> tblPreguntaNOpcionNPreguntaidPreguntaAnidadaNavigation { get; set; }
        [InverseProperty("idPreguntaNavigation")]
        public virtual ICollection<tblPreguntaNOpcionNPregunta> tblPreguntaNOpcionNPreguntaidPreguntaNavigation { get; set; }
        [InverseProperty("idPreguntaNavigation")]
        public virtual ICollection<tblRespuesta> tblRespuesta { get; set; }

        [ForeignKey("idPregunta")]
        [InverseProperty("idPregunta")]
        public virtual ICollection<tblOpcion> idOpcion { get; set; }
    }
}
