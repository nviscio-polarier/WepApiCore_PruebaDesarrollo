using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblJornadaPersona", Schema = "RRHH")]
    [Index("fecha", Name = "IX_tblJornadaPersona_fecha")]
    [Index("fecha", "idLavanderia", Name = "IX_tblJornadaPersona_fecha_idLavanderia")]
    public partial class tblJornadaPersona
    {
        [Key]
        public int idPersona { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horaEntrada { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horaSalida { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan tiempoTipoTrabajo1 { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan tiempoTipoTrabajo2 { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan tiempoTipoTrabajo3 { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? tiempoDescanso { get; set; }
        public int? idTurno { get; set; }
        public int idLavanderia { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan tiempoTipoTrabajo4 { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan tiempoTipoTrabajo5 { get; set; }
        [Precision(0)]
        public DateTimeOffset? reg_horaEntrada { get; set; }
        [Precision(0)]
        public DateTimeOffset? reg_horaSalida { get; set; }
        [Precision(0)]
        public DateTimeOffset? reg_horaInicioDescanso { get; set; }
        [Precision(0)]
        public DateTimeOffset? reg_horaFinDescanso { get; set; }
        public bool? isRegValido { get; set; }
        /// <summary>
        /// 1 - Manual, 2 - Tarjeta, 3 - MyPo
        /// </summary>
        public bool? isRegManual { get; set; }
        /// <summary>
        /// Null - Sin accion, 0 - No añadir, 1 - Añadir
        /// </summary>
        public bool? isRevisado_horaEntrada { get; set; }
        /// <summary>
        /// Null - Sin accion, 0 - No añadir, 1 - Añadir
        /// </summary>
        public bool? isRevisado_horaSalida { get; set; }
        public bool? isRevisado_tiempoDescanso { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horaEntrada_turno { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horaSalida_turno { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? descanso_turno { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horasDiarias { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan tiempoTipoTrabajo6 { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblJornadaPersona")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblJornadaPersona")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTurno")]
        [InverseProperty("tblJornadaPersona")]
        public virtual tblTurno? idTurnoNavigation { get; set; }
    }
}
