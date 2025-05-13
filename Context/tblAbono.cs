using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAbono", Schema = "Logistica")]
    [Index("idEntidad", "idTipoAbono", Name = "IX_tblAbono_idEntidad_idTipoAbono")]
    [Index("idTipoAbono", "idLavanderia", "fecha", Name = "IX_tblAbono_idTipoAbono_idLavanderia_fecha")]
    public partial class tblAbono
    {
        public tblAbono()
        {
            tblPrendaNAbono = new HashSet<tblPrendaNAbono>();
            tblSolicitudAbono = new HashSet<tblSolicitudAbono>();
        }

        [Key]
        public int idAbono { get; set; }
        public int? idEntidad { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }
        public byte? idTipoAbono { get; set; }
        public string? observaciones { get; set; }
        [StringLength(10)]
        public string? codigo { get; set; }
        public int? idLavanderia { get; set; }
        public DateTimeOffset? fechaRegistro { get; set; }
        public byte? idCategoriaAbono { get; set; }

        [ForeignKey("idCategoriaAbono")]
        [InverseProperty("tblAbono")]
        public virtual tblCategoriaAbono? idCategoriaAbonoNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblAbono")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblAbono")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idTipoAbono")]
        [InverseProperty("tblAbono")]
        public virtual tblTipoAbono? idTipoAbonoNavigation { get; set; }
        [InverseProperty("idAbonoNavigation")]
        public virtual ICollection<tblPrendaNAbono> tblPrendaNAbono { get; set; }
        [InverseProperty("idAbonoNavigation")]
        public virtual ICollection<tblSolicitudAbono> tblSolicitudAbono { get; set; }
    }
}
