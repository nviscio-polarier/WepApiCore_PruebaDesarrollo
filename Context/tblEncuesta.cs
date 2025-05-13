using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEncuesta", Schema = "ControlCalidad")]
    public partial class tblEncuesta
    {
        public tblEncuesta()
        {
            tblRespuesta = new HashSet<tblRespuesta>();
        }

        [Key]
        public int idEncuesta { get; set; }
        public int? idCampañaEncuesta { get; set; }
        public int? idEncuestaPlantilla { get; set; }
        public int? idEntidad { get; set; }
        public int? idUsuario { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaContesta { get; set; }
        public byte? idTipoEncuesta { get; set; }

        [ForeignKey("idCampañaEncuesta")]
        [InverseProperty("tblEncuesta")]
        public virtual tblCampañaEncuesta? idCampañaEncuestaNavigation { get; set; }
        [ForeignKey("idEncuestaPlantilla")]
        [InverseProperty("tblEncuesta")]
        public virtual tblEncuestaPlantilla? idEncuestaPlantillaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblEncuesta")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idTipoEncuesta")]
        [InverseProperty("tblEncuesta")]
        public virtual tblTipoEncuesta? idTipoEncuestaNavigation { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("tblEncuesta")]
        public virtual tblUsuario? idUsuarioNavigation { get; set; }
        [InverseProperty("idEncuestaNavigation")]
        public virtual ICollection<tblRespuesta> tblRespuesta { get; set; }
    }
}
