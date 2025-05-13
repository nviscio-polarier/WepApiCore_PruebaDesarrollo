using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoSolicitudAltaNSolicitudAlta", Schema = "RRHH")]
    public partial class tblEstadoSolicitudAltaNSolicitudAlta
    {
        [Key]
        public int idSolicitudAlta { get; set; }
        [Key]
        public byte idEstadoSolicitudAlta { get; set; }
        [Key]
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        public int? idUsuario { get; set; }
        public string? observaciones { get; set; }

        [ForeignKey("idEstadoSolicitudAlta")]
        [InverseProperty("tblEstadoSolicitudAltaNSolicitudAlta")]
        public virtual tblEstadoSolicitudAlta idEstadoSolicitudAltaNavigation { get; set; } = null!;
        [ForeignKey("idSolicitudAlta")]
        [InverseProperty("tblEstadoSolicitudAltaNSolicitudAlta")]
        public virtual tblSolicitudAlta idSolicitudAltaNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblEstadoSolicitudAltaNSolicitudAlta")]
        public virtual tblUsuario? idUsuarioNavigation { get; set; }
    }
}
