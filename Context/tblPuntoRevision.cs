using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblPuntoRevision
    {
        public tblPuntoRevision()
        {
            tblPuntoRevisionNAuditoria = new HashSet<tblPuntoRevisionNAuditoria>();
        }

        [Key]
        public short idPuntoRevision { get; set; }
        public short? idSubAmbito { get; set; }
        public string descripcion { get; set; } = null!;
        public byte? idTipoAuditoria { get; set; }
        public bool estado { get; set; }
        public short? idTipoMaquina { get; set; }
        public byte? orden { get; set; }
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? peso { get; set; }

        [ForeignKey("idSubAmbito")]
        [InverseProperty("tblPuntoRevision")]
        public virtual tblSubAmbito? idSubAmbitoNavigation { get; set; }
        [ForeignKey("idTipoAuditoria")]
        [InverseProperty("tblPuntoRevision")]
        public virtual tblTipoAuditoria? idTipoAuditoriaNavigation { get; set; }
        [InverseProperty("idPuntoRevisionNavigation")]
        public virtual ICollection<tblPuntoRevisionNAuditoria> tblPuntoRevisionNAuditoria { get; set; }
    }
}
