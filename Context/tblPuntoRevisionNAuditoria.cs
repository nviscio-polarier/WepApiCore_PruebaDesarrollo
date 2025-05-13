using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblPuntoRevisionNAuditoria
    {
        public tblPuntoRevisionNAuditoria()
        {
            tblNoConformidad = new HashSet<tblNoConformidad>();
        }

        [Key]
        public int idPuntoRevisionNAuditoria { get; set; }
        public short idPuntoRevision { get; set; }
        public short idAuditoria { get; set; }
        public byte idPuntuacion { get; set; }
        public int? idMaquina { get; set; }
        public string descripcion { get; set; } = null!;
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? peso { get; set; }
        public short? idSubAmbito { get; set; }

        [ForeignKey("idAuditoria,idSubAmbito")]
        [InverseProperty("tblPuntoRevisionNAuditoria")]
        public virtual tblSubAmbitoNAuditoria? id { get; set; }
        [ForeignKey("idAuditoria")]
        [InverseProperty("tblPuntoRevisionNAuditoria")]
        public virtual tblAuditoria idAuditoriaNavigation { get; set; } = null!;
        [ForeignKey("idPuntoRevision")]
        [InverseProperty("tblPuntoRevisionNAuditoria")]
        public virtual tblPuntoRevision idPuntoRevisionNavigation { get; set; } = null!;
        [ForeignKey("idPuntuacion")]
        [InverseProperty("tblPuntoRevisionNAuditoria")]
        public virtual tblPuntuacion idPuntuacionNavigation { get; set; } = null!;
        [ForeignKey("idSubAmbito")]
        [InverseProperty("tblPuntoRevisionNAuditoria")]
        public virtual tblSubAmbito? idSubAmbitoNavigation { get; set; }
        [InverseProperty("idPuntoRevisionNAuditoriaNavigation")]
        public virtual ICollection<tblNoConformidad> tblNoConformidad { get; set; }
    }
}
