using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblSubAmbitoNAuditoria
    {
        public tblSubAmbitoNAuditoria()
        {
            tblPuntoRevisionNAuditoria = new HashSet<tblPuntoRevisionNAuditoria>();
        }

        [Key]
        public short idAuditoria { get; set; }
        [Key]
        public short idSubAmbito { get; set; }
        public byte idAmbito { get; set; }
        public string denominacion { get; set; } = null!;
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? peso { get; set; }

        [ForeignKey("idAuditoria,idAmbito")]
        [InverseProperty("tblSubAmbitoNAuditoria")]
        public virtual tblAmbitoNAuditoria idA { get; set; } = null!;
        [ForeignKey("idAmbito")]
        [InverseProperty("tblSubAmbitoNAuditoria")]
        public virtual tblAmbito idAmbitoNavigation { get; set; } = null!;
        [ForeignKey("idAuditoria")]
        [InverseProperty("tblSubAmbitoNAuditoria")]
        public virtual tblAuditoria idAuditoriaNavigation { get; set; } = null!;
        [ForeignKey("idSubAmbito")]
        [InverseProperty("tblSubAmbitoNAuditoria")]
        public virtual tblSubAmbito idSubAmbitoNavigation { get; set; } = null!;
        [InverseProperty("id")]
        public virtual ICollection<tblPuntoRevisionNAuditoria> tblPuntoRevisionNAuditoria { get; set; }
    }
}
