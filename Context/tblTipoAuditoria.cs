using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblTipoAuditoria
    {
        public tblTipoAuditoria()
        {
            tblAuditoria = new HashSet<tblAuditoria>();
            tblPuntoRevision = new HashSet<tblPuntoRevision>();
        }

        [Key]
        public byte idTipoAuditoria { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoAuditoriaNavigation")]
        public virtual ICollection<tblAuditoria> tblAuditoria { get; set; }
        [InverseProperty("idTipoAuditoriaNavigation")]
        public virtual ICollection<tblPuntoRevision> tblPuntoRevision { get; set; }
    }
}
