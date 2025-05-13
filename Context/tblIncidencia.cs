using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblIncidencia", Schema = "Incidencias")]
    public partial class tblIncidencia
    {
        public tblIncidencia()
        {
            tblAccionesCorrectivas = new HashSet<tblAccionesCorrectivas>();
            tblIncidenciaNParte = new HashSet<tblIncidenciaNParte>();
            tblIncidenciaNReunion = new HashSet<tblIncidenciaNReunion>();
            tblIncidencia_Documento = new HashSet<tblIncidencia_Documento>();
            tblParadaNParteTransporte = new HashSet<tblParadaNParteTransporte>();
        }

        [Key]
        public int idIncidencia { get; set; }
        public string denominacion { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime fechaRegistro { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fechaIncidencia { get; set; }
        public string descripcionIncidencia { get; set; } = null!;
        public bool estado { get; set; }
        public string? descripcionResolucion { get; set; }
        public int? idUsuarioCrea { get; set; }
        public int? idMaquina { get; set; }
        public short idSubTipoIncidencia { get; set; }
        public bool notificarPolarier { get; set; }
        public byte? estadoMaquina { get; set; }
        public int? idUsuarioAfecta { get; set; }
        public int? idEntidad { get; set; }
        public int? idUsuarioResponsable { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaPrevisionResolucion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaCierre { get; set; }
        [StringLength(8)]
        public string? idxIncidencia { get; set; }
        public int? idLavanderia { get; set; }
        [StringLength(8)]
        public string? codigo { get; set; }
        public int? idVehiculo { get; set; }
        public bool isRevisada { get; set; }
        public bool isInformativa { get; set; }
        public int? idUsuarioRevisor { get; set; }
        public int? idDocumento { get; set; }
        public byte? idPoliticaDisciplinaria { get; set; }
        public byte? estadoMaquinaInicial { get; set; }
        public int? peso { get; set; }
        public int? pesoInicial { get; set; }
        public int? idCompañia { get; set; }
        public bool? isApp { get; set; }
        public string? asunto { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblIncidencia")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblIncidencia")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblIncidencia")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idMaquina")]
        [InverseProperty("tblIncidencia")]
        public virtual tblMaquina? idMaquinaNavigation { get; set; }
        [ForeignKey("idPoliticaDisciplinaria")]
        [InverseProperty("tblIncidencia")]
        public virtual tblPoliticaDisciplinaria? idPoliticaDisciplinariaNavigation { get; set; }
        [ForeignKey("idSubTipoIncidencia")]
        [InverseProperty("tblIncidencia")]
        public virtual tblTipoSubIncidencia idSubTipoIncidenciaNavigation { get; set; } = null!;
        [ForeignKey("idUsuarioAfecta")]
        [InverseProperty("tblIncidenciaidUsuarioAfectaNavigation")]
        public virtual tblPersona? idUsuarioAfectaNavigation { get; set; }
        [ForeignKey("idUsuarioCrea")]
        [InverseProperty("tblIncidenciaidUsuarioCreaNavigation")]
        public virtual tblUsuario? idUsuarioCreaNavigation { get; set; }
        [ForeignKey("idUsuarioResponsable")]
        [InverseProperty("tblIncidenciaidUsuarioResponsableNavigation")]
        public virtual tblPersona? idUsuarioResponsableNavigation { get; set; }
        [ForeignKey("idUsuarioRevisor")]
        [InverseProperty("tblIncidenciaidUsuarioRevisorNavigation")]
        public virtual tblUsuario? idUsuarioRevisorNavigation { get; set; }
        [ForeignKey("idVehiculo")]
        [InverseProperty("tblIncidencia")]
        public virtual tblVehiculo? idVehiculoNavigation { get; set; }
        [InverseProperty("idIncidenciaNavigation")]
        public virtual ICollection<tblAccionesCorrectivas> tblAccionesCorrectivas { get; set; }
        [InverseProperty("idIncidenciaNavigation")]
        public virtual ICollection<tblIncidenciaNParte> tblIncidenciaNParte { get; set; }
        [InverseProperty("idIncidenciaNavigation")]
        public virtual ICollection<tblIncidenciaNReunion> tblIncidenciaNReunion { get; set; }
        [InverseProperty("idIncidenciaNavigation")]
        public virtual ICollection<tblIncidencia_Documento> tblIncidencia_Documento { get; set; }
        [InverseProperty("idIncidenciaNavigation")]
        public virtual ICollection<tblParadaNParteTransporte> tblParadaNParteTransporte { get; set; }
    }
}
