using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoSolicitudAlta", Schema = "RRHH")]
    public partial class tblEstadoSolicitudAlta
    {
        public tblEstadoSolicitudAlta()
        {
            tblEstadoSolicitudAltaNSolicitudAlta = new HashSet<tblEstadoSolicitudAltaNSolicitudAlta>();
            tblSolicitudAlta = new HashSet<tblSolicitudAlta>();
        }

        [Key]
        public byte idEstadoSolicitudAlta { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idEstadoSolicitudAltaNavigation")]
        public virtual ICollection<tblEstadoSolicitudAltaNSolicitudAlta> tblEstadoSolicitudAltaNSolicitudAlta { get; set; }
        [InverseProperty("idEstadoSolicitudAltaNavigation")]
        public virtual ICollection<tblSolicitudAlta> tblSolicitudAlta { get; set; }
    }
}
