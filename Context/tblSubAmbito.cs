using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblSubAmbito
    {
        public tblSubAmbito()
        {
            tblPuntoRevision = new HashSet<tblPuntoRevision>();
            tblPuntoRevisionNAuditoria = new HashSet<tblPuntoRevisionNAuditoria>();
            tblSubAmbitoNAuditoria = new HashSet<tblSubAmbitoNAuditoria>();
        }

        [Key]
        public short idSubAmbito { get; set; }
        public byte idAmbito { get; set; }
        public string denominacion { get; set; } = null!;
        public bool estado { get; set; }
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? peso { get; set; }

        [ForeignKey("idAmbito")]
        [InverseProperty("tblSubAmbito")]
        public virtual tblAmbito idAmbitoNavigation { get; set; } = null!;
        [InverseProperty("idSubAmbitoNavigation")]
        public virtual ICollection<tblPuntoRevision> tblPuntoRevision { get; set; }
        [InverseProperty("idSubAmbitoNavigation")]
        public virtual ICollection<tblPuntoRevisionNAuditoria> tblPuntoRevisionNAuditoria { get; set; }
        [InverseProperty("idSubAmbitoNavigation")]
        public virtual ICollection<tblSubAmbitoNAuditoria> tblSubAmbitoNAuditoria { get; set; }
    }
}
