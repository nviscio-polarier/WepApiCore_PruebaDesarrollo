using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRespuesta", Schema = "ControlCalidad")]
    public partial class tblRespuesta
    {
        [Key]
        public int idRespuesta { get; set; }
        public int? idEncuesta { get; set; }
        public int? idUsuario { get; set; }
        public int idPregunta { get; set; }
        public short? idOpcion { get; set; }
        public string? descripcionPregunta { get; set; }
        public string pregunta { get; set; } = null!;
        public string? descripcionOpcion { get; set; }
        public string? texto { get; set; }
        public byte? minRango { get; set; }
        public byte? maxRango { get; set; }
        public byte? valor { get; set; }
        public int? idCampañaEncuesta { get; set; }
        public int? idEntidad { get; set; }

        [ForeignKey("idCampañaEncuesta")]
        [InverseProperty("tblRespuesta")]
        public virtual tblCampañaEncuesta? idCampañaEncuestaNavigation { get; set; }
        [ForeignKey("idEncuesta")]
        [InverseProperty("tblRespuesta")]
        public virtual tblEncuesta? idEncuestaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblRespuesta")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idOpcion")]
        [InverseProperty("tblRespuesta")]
        public virtual tblOpcion? idOpcionNavigation { get; set; }
        [ForeignKey("idPregunta")]
        [InverseProperty("tblRespuesta")]
        public virtual tblPregunta idPreguntaNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblRespuesta")]
        public virtual tblUsuario? idUsuarioNavigation { get; set; }
    }
}
