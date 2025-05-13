using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCampañaEncuesta", Schema = "ControlCalidad")]
    public partial class tblCampañaEncuesta
    {
        public tblCampañaEncuesta()
        {
            tblEncuesta = new HashSet<tblEncuesta>();
            tblRespuesta = new HashSet<tblRespuesta>();
        }

        [Key]
        public int idCampañaEncuesta { get; set; }
        public int idEncuestaPlantilla { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaIni { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaFin { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaLimite { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaCompletado { get; set; }

        [ForeignKey("idEncuestaPlantilla")]
        [InverseProperty("tblCampañaEncuesta")]
        public virtual tblEncuestaPlantilla idEncuestaPlantillaNavigation { get; set; } = null!;
        [InverseProperty("idCampañaEncuestaNavigation")]
        public virtual ICollection<tblEncuesta> tblEncuesta { get; set; }
        [InverseProperty("idCampañaEncuestaNavigation")]
        public virtual ICollection<tblRespuesta> tblRespuesta { get; set; }
    }
}
