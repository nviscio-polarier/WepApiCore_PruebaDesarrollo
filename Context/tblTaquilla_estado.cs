using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTaquilla_estado", Schema = "Locker")]
    public partial class tblTaquilla_estado
    {
        [Key]
        public short idTaquilla { get; set; }
        [Key]
        public byte numPosicion { get; set; }
        public int? idVehiculo { get; set; }

        [ForeignKey("idTaquilla")]
        [InverseProperty("tblTaquilla_estado")]
        public virtual tblTaquilla idTaquillaNavigation { get; set; } = null!;
        [ForeignKey("idVehiculo")]
        [InverseProperty("tblTaquilla_estado")]
        public virtual tblVehiculo? idVehiculoNavigation { get; set; }
    }
}
