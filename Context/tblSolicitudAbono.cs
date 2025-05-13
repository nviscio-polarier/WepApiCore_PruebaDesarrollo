using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblSolicitudAbono", Schema = "Logistica")]
    public partial class tblSolicitudAbono
    {
        public tblSolicitudAbono()
        {
            tblPrendaNSolicitudAbono = new HashSet<tblPrendaNSolicitudAbono>();
        }

        [Key]
        public int idSolicitudAbono { get; set; }
        public int idEntidad { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        [StringLength(10)]
        public string codigo { get; set; } = null!;
        public byte idCategoriaAbono { get; set; }
        public int idUsuario { get; set; }
        public byte idEstadoSolicitudAbono { get; set; }
        public int? idAbono { get; set; }

        [ForeignKey("idAbono")]
        [InverseProperty("tblSolicitudAbono")]
        public virtual tblAbono? idAbonoNavigation { get; set; }
        [ForeignKey("idCategoriaAbono")]
        [InverseProperty("tblSolicitudAbono")]
        public virtual tblCategoriaAbono idCategoriaAbonoNavigation { get; set; } = null!;
        [ForeignKey("idEntidad")]
        [InverseProperty("tblSolicitudAbono")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idEstadoSolicitudAbono")]
        [InverseProperty("tblSolicitudAbono")]
        public virtual tblEstadoSolicitudAbono idEstadoSolicitudAbonoNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblSolicitudAbono")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
        [InverseProperty("idSolicitudAbonoNavigation")]
        public virtual ICollection<tblPrendaNSolicitudAbono> tblPrendaNSolicitudAbono { get; set; }
    }
}
