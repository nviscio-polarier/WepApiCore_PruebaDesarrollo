using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblAuditoria
    {
        public tblAuditoria()
        {
            tblAmbitoNAuditoria = new HashSet<tblAmbitoNAuditoria>();
            tblAreaNAuditoria = new HashSet<tblAreaNAuditoria>();
            tblPuntoRevisionNAuditoria = new HashSet<tblPuntoRevisionNAuditoria>();
            tblSubAmbitoNAuditoria = new HashSet<tblSubAmbitoNAuditoria>();
        }

        [Key]
        public short idAuditoria { get; set; }
        public int idLavanderia { get; set; }
        public byte? idTipoAuditoria { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecha { get; set; }

        [ForeignKey("idTipoAuditoria")]
        [InverseProperty("tblAuditoria")]
        public virtual tblTipoAuditoria? idTipoAuditoriaNavigation { get; set; }
        [InverseProperty("idAuditoriaNavigation")]
        public virtual ICollection<tblAmbitoNAuditoria> tblAmbitoNAuditoria { get; set; }
        [InverseProperty("idAuditoriaNavigation")]
        public virtual ICollection<tblAreaNAuditoria> tblAreaNAuditoria { get; set; }
        [InverseProperty("idAuditoriaNavigation")]
        public virtual ICollection<tblPuntoRevisionNAuditoria> tblPuntoRevisionNAuditoria { get; set; }
        [InverseProperty("idAuditoriaNavigation")]
        public virtual ICollection<tblSubAmbitoNAuditoria> tblSubAmbitoNAuditoria { get; set; }
    }
}
