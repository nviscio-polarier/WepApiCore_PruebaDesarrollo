using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCalendarioCentroTrabajo", Schema = "General")]
    public partial class tblCalendarioCentroTrabajo
    {
        [Key]
        public int idCentroTrabajo { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Key]
        public byte idCalendario_Estado { get; set; }

        [ForeignKey("idCalendario_Estado")]
        [InverseProperty("tblCalendarioCentroTrabajo")]
        public virtual tblCalendario_Estado idCalendario_EstadoNavigation { get; set; } = null!;
        [ForeignKey("idCentroTrabajo")]
        [InverseProperty("tblCalendarioCentroTrabajo")]
        public virtual tblCentroTrabajo idCentroTrabajoNavigation { get; set; } = null!;
    }
}
