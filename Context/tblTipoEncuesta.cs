using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoEncuesta", Schema = "ControlCalidad")]
    public partial class tblTipoEncuesta
    {
        public tblTipoEncuesta()
        {
            tblEncuesta = new HashSet<tblEncuesta>();
            tblEncuestaPlantilla = new HashSet<tblEncuestaPlantilla>();
        }

        [Key]
        public byte idTipoEncuesta { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoEncuestaNavigation")]
        public virtual ICollection<tblEncuesta> tblEncuesta { get; set; }
        [InverseProperty("idTipoEncuestaNavigation")]
        public virtual ICollection<tblEncuestaPlantilla> tblEncuestaPlantilla { get; set; }
    }
}
