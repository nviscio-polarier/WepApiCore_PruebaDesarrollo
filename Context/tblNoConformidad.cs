using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblNoConformidad
    {
        public tblNoConformidad()
        {
            tblFoto = new HashSet<tblFoto>();
        }

        [Key]
        public int idNoConformidad { get; set; }
        public int idPuntoRevisionNAuditoria { get; set; }
        public string descripcion { get; set; } = null!;
        public string? accionCorrectora { get; set; }
        public byte? idGrado { get; set; }
        public byte? idEstado { get; set; }
        public string? responsable { get; set; }
        public string? observaciones { get; set; }
        public string? responsable2 { get; set; }
        public string? responsable3 { get; set; }
        public bool? necesitaRecambio { get; set; }

        [ForeignKey("idEstado")]
        [InverseProperty("tblNoConformidad")]
        public virtual tblEstado? idEstadoNavigation { get; set; }
        [ForeignKey("idGrado")]
        [InverseProperty("tblNoConformidad")]
        public virtual tblNoConformidad_Grado? idGradoNavigation { get; set; }
        [ForeignKey("idPuntoRevisionNAuditoria")]
        [InverseProperty("tblNoConformidad")]
        public virtual tblPuntoRevisionNAuditoria idPuntoRevisionNAuditoriaNavigation { get; set; } = null!;
        [InverseProperty("idNoConformidadNavigation")]
        public virtual ICollection<tblFoto> tblFoto { get; set; }
    }
}
