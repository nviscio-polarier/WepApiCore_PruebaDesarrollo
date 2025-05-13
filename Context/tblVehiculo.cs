using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblVehiculo", Schema = "Logistica")]
    public partial class tblVehiculo
    {
        public tblVehiculo()
        {
            tblIncidencia = new HashSet<tblIncidencia>();
            tblParteTransporte = new HashSet<tblParteTransporte>();
            tblSalidaReparto = new HashSet<tblSalidaReparto>();
            tblTaquilla_estado = new HashSet<tblTaquilla_estado>();
            tblTaquilla_movimiento = new HashSet<tblTaquilla_movimiento>();
            idLavanderia = new HashSet<tblLavanderia>();
        }

        [Key]
        public int idVehiculo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [StringLength(20)]
        public string matricula { get; set; } = null!;
        public short? capacidadBacs { get; set; }
        public byte? idTipoVehiculo { get; set; }
        public bool eliminado { get; set; }
        public string? codigoRFID { get; set; }
        [StringLength(30)]
        public string? idVehiculo_webfleet { get; set; }
        public string? idTipoVehiculo_webfleet { get; set; }

        [ForeignKey("idTipoVehiculo")]
        [InverseProperty("tblVehiculo")]
        public virtual tblTipoVehiculo? idTipoVehiculoNavigation { get; set; }
        [InverseProperty("idVehiculoNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidencia { get; set; }
        [InverseProperty("idVehiculoNavigation")]
        public virtual ICollection<tblParteTransporte> tblParteTransporte { get; set; }
        [InverseProperty("idVehiculoNavigation")]
        public virtual ICollection<tblSalidaReparto> tblSalidaReparto { get; set; }
        [InverseProperty("idVehiculoNavigation")]
        public virtual ICollection<tblTaquilla_estado> tblTaquilla_estado { get; set; }
        [InverseProperty("idVehiculoNavigation")]
        public virtual ICollection<tblTaquilla_movimiento> tblTaquilla_movimiento { get; set; }

        [ForeignKey("idVehiculo")]
        [InverseProperty("idVehiculo")]
        public virtual ICollection<tblLavanderia> idLavanderia { get; set; }
    }
}
