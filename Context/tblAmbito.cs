using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblAmbito
    {
        public tblAmbito()
        {
            tblAmbitoNAuditoria = new HashSet<tblAmbitoNAuditoria>();
            tblSubAmbito = new HashSet<tblSubAmbito>();
            tblSubAmbitoNAuditoria = new HashSet<tblSubAmbitoNAuditoria>();
        }

        [Key]
        public byte idAmbito { get; set; }
        public byte idArea { get; set; }
        public string denominacion { get; set; } = null!;
        public bool estado { get; set; }
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? peso { get; set; }

        [ForeignKey("idArea")]
        [InverseProperty("tblAmbito")]
        public virtual tblArea idAreaNavigation { get; set; } = null!;
        [InverseProperty("idAmbitoNavigation")]
        public virtual ICollection<tblAmbitoNAuditoria> tblAmbitoNAuditoria { get; set; }
        [InverseProperty("idAmbitoNavigation")]
        public virtual ICollection<tblSubAmbito> tblSubAmbito { get; set; }
        [InverseProperty("idAmbitoNavigation")]
        public virtual ICollection<tblSubAmbitoNAuditoria> tblSubAmbitoNAuditoria { get; set; }
    }
}
