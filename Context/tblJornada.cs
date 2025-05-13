using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblJornada", Schema = "RRHH")]
    [Index("horaIni", "horaFin", "tiempoDescanso", Name = "IX_tblJornada_horaIni_horaFin_tiempoDescanso")]
    [Index("idPersona", "idLavanderia", Name = "IX_tblJornada_idPersona_idLavanderia")]
    public partial class tblJornada
    {
        public tblJornada()
        {
            tblBalanceHoras = new HashSet<tblBalanceHoras>();
            tblBalanceHorasExtra = new HashSet<tblBalanceHorasExtra>();
            tblCalendarioPersonal = new HashSet<tblCalendarioPersonal>();
            tblEventoPersona = new HashSet<tblEventoPersona>();
        }

        [Key]
        public int idJornada { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public int idPersona { get; set; }
        public int idLavanderia { get; set; }
        public int? idCuadrantePersonal { get; set; }
        public int idTurno { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horaIni { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horaFin { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? tiempoDescanso { get; set; }
        public bool isRegValido { get; set; }
        public bool isRevisado { get; set; }
        public byte? idMotivoIncumplimiento_horaIni { get; set; }
        public byte? idMotivoIncumplimiento_horaFin { get; set; }
        public byte? idMotivoIncumplimiento_tiempoDescanso { get; set; }
        public int? idUsuario_validacion { get; set; }
        [Precision(0)]
        public DateTimeOffset? fecha_validacion { get; set; }
        public byte idTipoTrabajo { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horasDiarias { get; set; }

        [ForeignKey("idCuadrantePersonal")]
        [InverseProperty("tblJornada")]
        public virtual tblCuadrantePersonal? idCuadrantePersonalNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblJornada")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idMotivoIncumplimiento_horaFin")]
        [InverseProperty("tblJornadaidMotivoIncumplimiento_horaFinNavigation")]
        public virtual tblMotivoIncumplimientoJornada? idMotivoIncumplimiento_horaFinNavigation { get; set; }
        [ForeignKey("idMotivoIncumplimiento_horaIni")]
        [InverseProperty("tblJornadaidMotivoIncumplimiento_horaIniNavigation")]
        public virtual tblMotivoIncumplimientoJornada? idMotivoIncumplimiento_horaIniNavigation { get; set; }
        [ForeignKey("idMotivoIncumplimiento_tiempoDescanso")]
        [InverseProperty("tblJornadaidMotivoIncumplimiento_tiempoDescansoNavigation")]
        public virtual tblMotivoIncumplimientoJornada? idMotivoIncumplimiento_tiempoDescansoNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblJornada")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTipoTrabajo")]
        [InverseProperty("tblJornada")]
        public virtual tblTipoTrabajo idTipoTrabajoNavigation { get; set; } = null!;
        [ForeignKey("idTurno")]
        [InverseProperty("tblJornada")]
        public virtual tblTurno idTurnoNavigation { get; set; } = null!;
        [ForeignKey("idUsuario_validacion")]
        [InverseProperty("tblJornada")]
        public virtual tblUsuario? idUsuario_validacionNavigation { get; set; }
        [InverseProperty("idJornadaNavigation")]
        public virtual ICollection<tblBalanceHoras> tblBalanceHoras { get; set; }
        [InverseProperty("idJornadaNavigation")]
        public virtual ICollection<tblBalanceHorasExtra> tblBalanceHorasExtra { get; set; }
        [InverseProperty("idJornadaNavigation")]
        public virtual ICollection<tblCalendarioPersonal> tblCalendarioPersonal { get; set; }
        [InverseProperty("idJornadaNavigation")]
        public virtual ICollection<tblEventoPersona> tblEventoPersona { get; set; }
    }
}
