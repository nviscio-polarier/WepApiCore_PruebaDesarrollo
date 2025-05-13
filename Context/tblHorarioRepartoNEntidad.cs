using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblHorarioRepartoNEntidad", Schema = "General")]
    public partial class tblHorarioRepartoNEntidad
    {
        [Key]
        public int idEntidad { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horarioInicioReparto { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horarioFinReparto { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblHorarioRepartoNEntidad")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
    }
}
