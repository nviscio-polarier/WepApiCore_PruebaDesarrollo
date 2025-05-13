using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoSolicitudAbono", Schema = "Logistica")]
    public partial class tblEstadoSolicitudAbono
    {
        public tblEstadoSolicitudAbono()
        {
            tblSolicitudAbono = new HashSet<tblSolicitudAbono>();
        }

        [Key]
        public byte idEstadoSolicitudAbono { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idEstadoSolicitudAbonoNavigation")]
        public virtual ICollection<tblSolicitudAbono> tblSolicitudAbono { get; set; }
    }
}
