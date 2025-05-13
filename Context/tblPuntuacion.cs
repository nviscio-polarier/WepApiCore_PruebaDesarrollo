using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblPuntuacion
    {
        public tblPuntuacion()
        {
            tblPuntoRevisionNAuditoria = new HashSet<tblPuntoRevisionNAuditoria>();
        }

        [Key]
        public byte idPuntuacion { get; set; }
        public string denominacion { get; set; } = null!;
        public bool estado { get; set; }

        [InverseProperty("idPuntuacionNavigation")]
        public virtual ICollection<tblPuntoRevisionNAuditoria> tblPuntoRevisionNAuditoria { get; set; }
    }
}
