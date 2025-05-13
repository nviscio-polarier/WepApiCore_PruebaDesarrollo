using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblAmbitoNAuditoria
    {
        public tblAmbitoNAuditoria()
        {
            tblSubAmbitoNAuditoria = new HashSet<tblSubAmbitoNAuditoria>();
        }

        [Key]
        public short idAuditoria { get; set; }
        [Key]
        public byte idAmbito { get; set; }
        public byte idArea { get; set; }
        public string denominacion { get; set; } = null!;
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? peso { get; set; }

        [ForeignKey("idAuditoria,idArea")]
        [InverseProperty("tblAmbitoNAuditoria")]
        public virtual tblAreaNAuditoria idA { get; set; } = null!;
        [ForeignKey("idAmbito")]
        [InverseProperty("tblAmbitoNAuditoria")]
        public virtual tblAmbito idAmbitoNavigation { get; set; } = null!;
        [ForeignKey("idArea")]
        [InverseProperty("tblAmbitoNAuditoria")]
        public virtual tblArea idAreaNavigation { get; set; } = null!;
        [ForeignKey("idAuditoria")]
        [InverseProperty("tblAmbitoNAuditoria")]
        public virtual tblAuditoria idAuditoriaNavigation { get; set; } = null!;
        [InverseProperty("idA")]
        public virtual ICollection<tblSubAmbitoNAuditoria> tblSubAmbitoNAuditoria { get; set; }
    }
}
