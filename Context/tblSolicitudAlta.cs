using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblSolicitudAlta", Schema = "RRHH")]
    public partial class tblSolicitudAlta
    {
        public tblSolicitudAlta()
        {
            tblDocumentoNSolicitudAlta = new HashSet<tblDocumentoNSolicitudAlta>();
            tblEstadoSolicitudAltaNSolicitudAlta = new HashSet<tblEstadoSolicitudAltaNSolicitudAlta>();
        }

        [Key]
        public int idSolicitudAlta { get; set; }
        public byte idEstadoSolicitudAlta { get; set; }
        public DateTimeOffset fecha_reg { get; set; }
        public DateTimeOffset? fecha_validacion { get; set; }
        public bool isUrgente { get; set; }
        public bool hasAltaSS { get; set; }
        public bool isLlamamiento { get; set; }
        public int? idUsuario_validacion { get; set; }

        [ForeignKey("idEstadoSolicitudAlta")]
        [InverseProperty("tblSolicitudAlta")]
        public virtual tblEstadoSolicitudAlta idEstadoSolicitudAltaNavigation { get; set; } = null!;
        [ForeignKey("idSolicitudAlta")]
        [InverseProperty("tblSolicitudAlta")]
        public virtual tblLlamamiento idSolicitudAltaNavigation { get; set; } = null!;
        [ForeignKey("idUsuario_validacion")]
        [InverseProperty("tblSolicitudAlta")]
        public virtual tblUsuario? idUsuario_validacionNavigation { get; set; }
        [InverseProperty("idSolicitudAltaNavigation")]
        public virtual ICollection<tblDocumentoNSolicitudAlta> tblDocumentoNSolicitudAlta { get; set; }
        [InverseProperty("idSolicitudAltaNavigation")]
        public virtual ICollection<tblEstadoSolicitudAltaNSolicitudAlta> tblEstadoSolicitudAltaNSolicitudAlta { get; set; }
    }
}
