using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCuadrantePersonal", Schema = "RRHH")]
    [Index("idCalendario_Estado", Name = "IX_tblCuadrantePersonal_idEstado")]
    [Index("idPersona", "fecha", "idCalendario_Estado", Name = "IX_tblCuadrantePersonal_idPersona_fecha_idEstado")]
    [Index("fecha", "idPersona", Name = "UC_fecha_idPersona", IsUnique = true)]
    public partial class tblCuadrantePersonal
    {
        public tblCuadrantePersonal()
        {
            tblCalendarioPersonal = new HashSet<tblCalendarioPersonal>();
            tblJornada = new HashSet<tblJornada>();
        }

        [Key]
        public int idCuadrantePersonal { get; set; }
        public int idPersona { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public byte? idCalendario_Estado { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horaEntrada { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? horaSalida { get; set; }
        public int? idTurno { get; set; }
        public short? idPosicionNAreaLavanderiaNLavanderia { get; set; }
        public int idLavanderia { get; set; }
        public bool isCorregido { get; set; }
        public int? idUsuario_validacion { get; set; }
        [Precision(0)]
        public DateTimeOffset? fecha_validacion { get; set; }

        [ForeignKey("idCalendario_Estado")]
        [InverseProperty("tblCuadrantePersonal")]
        public virtual tblCalendario_Estado? idCalendario_EstadoNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblCuadrantePersonal")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblCuadrantePersonal")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idPosicionNAreaLavanderiaNLavanderia")]
        [InverseProperty("tblCuadrantePersonal")]
        public virtual tblPosicionNAreaLavanderiaNLavanderia? idPosicionNAreaLavanderiaNLavanderiaNavigation { get; set; }
        [ForeignKey("idTurno")]
        [InverseProperty("tblCuadrantePersonal")]
        public virtual tblTurno? idTurnoNavigation { get; set; }
        [ForeignKey("idUsuario_validacion")]
        [InverseProperty("tblCuadrantePersonal")]
        public virtual tblUsuario? idUsuario_validacionNavigation { get; set; }
        [InverseProperty("idCuadrantePersonalNavigation")]
        public virtual ICollection<tblCalendarioPersonal> tblCalendarioPersonal { get; set; }
        [InverseProperty("idCuadrantePersonalNavigation")]
        public virtual ICollection<tblJornada> tblJornada { get; set; }
    }
}
