using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblAreaNAuditoria
    {
        public tblAreaNAuditoria()
        {
            tblAmbitoNAuditoria = new HashSet<tblAmbitoNAuditoria>();
        }

        [Key]
        public short idAuditoria { get; set; }
        [Key]
        public byte idArea { get; set; }
        public string denominacion { get; set; } = null!;
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? peso { get; set; }

        [ForeignKey("idArea")]
        [InverseProperty("tblAreaNAuditoria")]
        public virtual tblArea idAreaNavigation { get; set; } = null!;
        [ForeignKey("idAuditoria")]
        [InverseProperty("tblAreaNAuditoria")]
        public virtual tblAuditoria idAuditoriaNavigation { get; set; } = null!;
        [InverseProperty("idA")]
        public virtual ICollection<tblAmbitoNAuditoria> tblAmbitoNAuditoria { get; set; }
    }
}
