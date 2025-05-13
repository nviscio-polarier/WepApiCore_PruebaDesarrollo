using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblSubTurno", Schema = "RRHH")]
    public partial class tblSubTurno
    {
        [Key]
        public int idSubTurno { get; set; }
        public int idTurno { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [Column(TypeName = "time(0)")]
        public TimeSpan horaEntrada { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan horaSalida { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan descanso { get; set; }
        [StringLength(2)]
        public string? abreviatura { get; set; }

        [ForeignKey("idTurno")]
        [InverseProperty("tblSubTurno")]
        public virtual tblTurno idTurnoNavigation { get; set; } = null!;
    }
}
