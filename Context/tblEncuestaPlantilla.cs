using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEncuestaPlantilla", Schema = "ControlCalidad")]
    public partial class tblEncuestaPlantilla
    {
        public tblEncuestaPlantilla()
        {
            tblCampañaEncuesta = new HashSet<tblCampañaEncuesta>();
            tblEncuesta = new HashSet<tblEncuesta>();
            tblPregunta = new HashSet<tblPregunta>();
        }

        [Key]
        public int idEncuestaPlantilla { get; set; }
        public string denominacion { get; set; } = null!;
        public byte? idTipoEncuesta { get; set; }

        [ForeignKey("idTipoEncuesta")]
        [InverseProperty("tblEncuestaPlantilla")]
        public virtual tblTipoEncuesta? idTipoEncuestaNavigation { get; set; }
        [InverseProperty("idEncuestaPlantillaNavigation")]
        public virtual ICollection<tblCampañaEncuesta> tblCampañaEncuesta { get; set; }
        [InverseProperty("idEncuestaPlantillaNavigation")]
        public virtual ICollection<tblEncuesta> tblEncuesta { get; set; }
        [InverseProperty("idEncuestaPlantillaNavigation")]
        public virtual ICollection<tblPregunta> tblPregunta { get; set; }
    }
}
