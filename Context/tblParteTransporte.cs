using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblParteTransporte", Schema = "Logistica")]
    public partial class tblParteTransporte
    {
        public tblParteTransporte()
        {
            tblParadaNParteTransporte = new HashSet<tblParadaNParteTransporte>();
            tblParteTransporte_Localizacion = new HashSet<tblParteTransporte_Localizacion>();
            tblRepartoNParteTransporte = new HashSet<tblRepartoNParteTransporte>();
            tblRevisionVehiculoNParteTransporte = new HashSet<tblRevisionVehiculoNParteTransporte>();
            tblRutaNParteTransporte = new HashSet<tblRutaNParteTransporte>();
            idPersonaTransportista = new HashSet<tblPersona>();
        }

        [Key]
        public int idParteTransporte { get; set; }
        public int idVehiculo { get; set; }
        public int? idPersonaResponsable { get; set; }
        public int? idNivelCombustible { get; set; }
        public int? kmsInicialesVehiculo { get; set; }
        public int? kmsFinalesVehiculo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? horaSalidaLavanderia { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? horaLlegadaLavanderia { get; set; }
        [StringLength(400)]
        public string? observaciones { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecha { get; set; }
        public int? idLavanderia { get; set; }
        public int? idRutaExpedicion { get; set; }
        public string? denoRutaExpedicion { get; set; }
        public int? idUsuarioResponsable { get; set; }
        public byte? idEstado { get; set; }

        [ForeignKey("idEstado")]
        [InverseProperty("tblParteTransporte")]
        public virtual tblParteTransporte_Estado? idEstadoNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblParteTransporte")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idNivelCombustible")]
        [InverseProperty("tblParteTransporte")]
        public virtual tblNivelCombustible? idNivelCombustibleNavigation { get; set; }
        [ForeignKey("idPersonaResponsable")]
        [InverseProperty("tblParteTransporte")]
        public virtual tblPersona? idPersonaResponsableNavigation { get; set; }
        [ForeignKey("idRutaExpedicion")]
        [InverseProperty("tblParteTransporte")]
        public virtual tblRutaExpedicion? idRutaExpedicionNavigation { get; set; }
        [ForeignKey("idUsuarioResponsable")]
        [InverseProperty("tblParteTransporte")]
        public virtual tblUsuario? idUsuarioResponsableNavigation { get; set; }
        [ForeignKey("idVehiculo")]
        [InverseProperty("tblParteTransporte")]
        public virtual tblVehiculo idVehiculoNavigation { get; set; } = null!;
        [InverseProperty("idParteTransporteNavigation")]
        public virtual ICollection<tblParadaNParteTransporte> tblParadaNParteTransporte { get; set; }
        [InverseProperty("idParteTransporteNavigation")]
        public virtual ICollection<tblParteTransporte_Localizacion> tblParteTransporte_Localizacion { get; set; }
        [InverseProperty("idParteTransporteNavigation")]
        public virtual ICollection<tblRepartoNParteTransporte> tblRepartoNParteTransporte { get; set; }
        [InverseProperty("idParteTransporteNavigation")]
        public virtual ICollection<tblRevisionVehiculoNParteTransporte> tblRevisionVehiculoNParteTransporte { get; set; }
        [InverseProperty("idParteTransporteNavigation")]
        public virtual ICollection<tblRutaNParteTransporte> tblRutaNParteTransporte { get; set; }

        [ForeignKey("idParteTransporte")]
        [InverseProperty("idParteTransporte")]
        public virtual ICollection<tblPersona> idPersonaTransportista { get; set; }
    }
}
