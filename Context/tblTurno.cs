using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTurno", Schema = "RRHH")]
    public partial class tblTurno
    {
        public tblTurno()
        {
            InverseidTurnoPadreNavigation = new HashSet<tblTurno>();
            tblCategoriaInternaNTurno = new HashSet<tblCategoriaInternaNTurno>();
            tblCuadrantePersonal = new HashSet<tblCuadrantePersonal>();
            tblDiasCuadrante = new HashSet<tblDiasCuadrante>();
            tblJornada = new HashSet<tblJornada>();
            tblJornadaPersona = new HashSet<tblJornadaPersona>();
            tblLlamamiento = new HashSet<tblLlamamiento>();
            tblPersona = new HashSet<tblPersona>();
            tblProduccion = new HashSet<tblProduccion>();
            tblSubTurno = new HashSet<tblSubTurno>();
        }

        [Key]
        public int idTurno { get; set; }
        public int idLavanderia { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [StringLength(10)]
        public string? abreviatura { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan horaEntrada { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan horaSalida { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan descanso { get; set; }
        public int? idTurnoPadre { get; set; }
        public bool? activo { get; set; }
        public bool eliminado { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblTurno")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idTurnoPadre")]
        [InverseProperty("InverseidTurnoPadreNavigation")]
        public virtual tblTurno? idTurnoPadreNavigation { get; set; }
        [InverseProperty("idTurnoPadreNavigation")]
        public virtual ICollection<tblTurno> InverseidTurnoPadreNavigation { get; set; }
        [InverseProperty("idTurnoNavigation")]
        public virtual ICollection<tblCategoriaInternaNTurno> tblCategoriaInternaNTurno { get; set; }
        [InverseProperty("idTurnoNavigation")]
        public virtual ICollection<tblCuadrantePersonal> tblCuadrantePersonal { get; set; }
        [InverseProperty("idTurnoNavigation")]
        public virtual ICollection<tblDiasCuadrante> tblDiasCuadrante { get; set; }
        [InverseProperty("idTurnoNavigation")]
        public virtual ICollection<tblJornada> tblJornada { get; set; }
        [InverseProperty("idTurnoNavigation")]
        public virtual ICollection<tblJornadaPersona> tblJornadaPersona { get; set; }
        [InverseProperty("idTurnoNavigation")]
        public virtual ICollection<tblLlamamiento> tblLlamamiento { get; set; }
        [InverseProperty("idTurnoNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
        [InverseProperty("idTurnoNavigation")]
        public virtual ICollection<tblProduccion> tblProduccion { get; set; }
        [InverseProperty("idTurnoNavigation")]
        public virtual ICollection<tblSubTurno> tblSubTurno { get; set; }
    }
}
