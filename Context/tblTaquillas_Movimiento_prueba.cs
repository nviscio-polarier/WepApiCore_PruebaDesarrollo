using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTaquillas_Movimiento_prueba", Schema = "MyRealBonus")]
    public partial class tblTaquillas_Movimiento_prueba
    {
        [Key]
        public int idMovimiento { get; set; }
        public int idTaquilla { get; set; }
        public int idVehiculo { get; set; }
        public int idPersona { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaRecogida { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaDejar { get; set; }
        public int? posicion { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblTaquillas_Movimiento_prueba")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idTaquilla")]
        [InverseProperty("tblTaquillas_Movimiento_prueba")]
        public virtual tblTaquillas_prueba? idTaquillaNavigation { get; set; }
        [ForeignKey("idVehiculo")]
        [InverseProperty("tblTaquillas_Movimiento_prueba")]
        public virtual tblVehiculo? idVehiculoNavigation { get; set; }
    }
}
