using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMotivoIncumplimientoJornada", Schema = "RRHH")]
    public partial class tblMotivoIncumplimientoJornada
    {
        public tblMotivoIncumplimientoJornada()
        {
            tblJornadaidMotivoIncumplimiento_horaFinNavigation = new HashSet<tblJornada>();
            tblJornadaidMotivoIncumplimiento_horaIniNavigation = new HashSet<tblJornada>();
            tblJornadaidMotivoIncumplimiento_tiempoDescansoNavigation = new HashSet<tblJornada>();
        }

        [Key]
        public byte idMotivoIncumplimientoJornada { get; set; }
        public string denominacion { get; set; } = null!;
        public string? traduccion { get; set; }
        public int? idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblMotivoIncumplimientoJornada")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idMotivoIncumplimiento_horaFinNavigation")]
        public virtual ICollection<tblJornada> tblJornadaidMotivoIncumplimiento_horaFinNavigation { get; set; }
        [InverseProperty("idMotivoIncumplimiento_horaIniNavigation")]
        public virtual ICollection<tblJornada> tblJornadaidMotivoIncumplimiento_horaIniNavigation { get; set; }
        [InverseProperty("idMotivoIncumplimiento_tiempoDescansoNavigation")]
        public virtual ICollection<tblJornada> tblJornadaidMotivoIncumplimiento_tiempoDescansoNavigation { get; set; }
    }
}
