using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTareaMaquina", Schema = "Assistant")]
    [Index("idFrecuenciaMantenimiento", Name = "IX_tblTareaMaquina_idFrecuenciaMantenimiento")]
    [Index("idMaquina", "idFrecuenciaMantenimiento", Name = "IX_tblTareaMaquina_idMaquina_idFrecuenciaMantenimiento")]
    public partial class tblTareaMaquina
    {
        public tblTareaMaquina()
        {
            tblTareaPersonaDia = new HashSet<tblTareaPersonaDia>();
        }

        [Key]
        public int idTareaMaquina { get; set; }
        public int idMaquina { get; set; }
        public string denominacion { get; set; } = null!;
        public byte idFrecuenciaMantenimiento { get; set; }
        public byte? idDiaSemana { get; set; }
        public byte? semana { get; set; }
        public byte? idMes { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan tiempo { get; set; }
        public byte idResponsableTarea { get; set; }
        public bool parada { get; set; }
        [StringLength(5)]
        public string? idxTarea { get; set; }
        public bool eliminado { get; set; }

        [ForeignKey("idDiaSemana")]
        [InverseProperty("tblTareaMaquina")]
        public virtual tblDiaSemana? idDiaSemanaNavigation { get; set; }
        [ForeignKey("idFrecuenciaMantenimiento")]
        [InverseProperty("tblTareaMaquina")]
        public virtual tblFrecuenciaMantenimiento idFrecuenciaMantenimientoNavigation { get; set; } = null!;
        [ForeignKey("idMaquina")]
        [InverseProperty("tblTareaMaquina")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idMes")]
        [InverseProperty("tblTareaMaquina")]
        public virtual tblMes? idMesNavigation { get; set; }
        [ForeignKey("idResponsableTarea")]
        [InverseProperty("tblTareaMaquina")]
        public virtual tblResponsableTarea idResponsableTareaNavigation { get; set; } = null!;
        [InverseProperty("idTareaMaquinaNavigation")]
        public virtual ICollection<tblTareaPersonaDia> tblTareaPersonaDia { get; set; }
    }
}
