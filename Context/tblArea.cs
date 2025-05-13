using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblArea
    {
        public tblArea()
        {
            tblAmbito = new HashSet<tblAmbito>();
            tblAmbitoNAuditoria = new HashSet<tblAmbitoNAuditoria>();
            tblAreaNAuditoria = new HashSet<tblAreaNAuditoria>();
        }

        [Key]
        public byte idArea { get; set; }
        public string denominacion { get; set; } = null!;
        public bool estado { get; set; }
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? peso { get; set; }

        [InverseProperty("idAreaNavigation")]
        public virtual ICollection<tblAmbito> tblAmbito { get; set; }
        [InverseProperty("idAreaNavigation")]
        public virtual ICollection<tblAmbitoNAuditoria> tblAmbitoNAuditoria { get; set; }
        [InverseProperty("idAreaNavigation")]
        public virtual ICollection<tblAreaNAuditoria> tblAreaNAuditoria { get; set; }
    }
}
