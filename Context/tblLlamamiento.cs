using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLlamamiento", Schema = "RRHH")]
    public partial class tblLlamamiento
    {
        public tblLlamamiento()
        {
            tblDiasLibresPersonal_Llamamiento = new HashSet<tblDiasLibresPersonal_Llamamiento>();
        }

        [Key]
        public int idLlamamiento { get; set; }
        public int? idLavanderia { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaIni { get; set; }
        public byte? idTipoTrabajo { get; set; }
        public int idCategoriaInterna { get; set; }
        public int? idTurno { get; set; }
        public byte idFormatoDiasLibres { get; set; }
        [Required]
        public bool? activo { get; set; }
        public byte? codigoLlamamiento { get; set; }
        public int? idPersona { get; set; }
        public short idTipoContrato { get; set; }
        public int? idCentroTrabajo { get; set; }
        public bool isNuevaAlta { get; set; }
        public short? numDiasPeriodoPrueba { get; set; }

        [ForeignKey("idCategoriaInterna")]
        [InverseProperty("tblLlamamiento")]
        public virtual tblCategoriaInterna idCategoriaInternaNavigation { get; set; } = null!;
        [ForeignKey("idCentroTrabajo")]
        [InverseProperty("tblLlamamiento")]
        public virtual tblCentroTrabajo? idCentroTrabajoNavigation { get; set; }
        [ForeignKey("idFormatoDiasLibres")]
        [InverseProperty("tblLlamamiento")]
        public virtual tblFormatoDiasLibres idFormatoDiasLibresNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblLlamamiento")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblLlamamiento")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idTipoContrato")]
        [InverseProperty("tblLlamamiento")]
        public virtual tblTipoContrato idTipoContratoNavigation { get; set; } = null!;
        [ForeignKey("idTipoTrabajo")]
        [InverseProperty("tblLlamamiento")]
        public virtual tblTipoTrabajo? idTipoTrabajoNavigation { get; set; }
        [ForeignKey("idTurno")]
        [InverseProperty("tblLlamamiento")]
        public virtual tblTurno? idTurnoNavigation { get; set; }
        [InverseProperty("idSolicitudAltaNavigation")]
        public virtual tblSolicitudAlta tblSolicitudAlta { get; set; } = null!;
        [InverseProperty("idLlamamientoNavigation")]
        public virtual ICollection<tblDiasLibresPersonal_Llamamiento> tblDiasLibresPersonal_Llamamiento { get; set; }
    }
}
