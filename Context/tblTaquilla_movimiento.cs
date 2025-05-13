using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTaquilla_movimiento", Schema = "Locker")]
    public partial class tblTaquilla_movimiento
    {
        [Key]
        public int idTaquillaMovimiento { get; set; }
        public short idTaquilla { get; set; }
        public byte numPosicion { get; set; }
        public int idPersona { get; set; }
        public int? idVehiculo { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        public bool isRecogida { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblTaquilla_movimiento")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTaquilla")]
        [InverseProperty("tblTaquilla_movimiento")]
        public virtual tblTaquilla idTaquillaNavigation { get; set; } = null!;
        [ForeignKey("idVehiculo")]
        [InverseProperty("tblTaquilla_movimiento")]
        public virtual tblVehiculo? idVehiculoNavigation { get; set; }
    }
}
