using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Keyless]
    [Table("tblTaquillas_Estado_prueba", Schema = "MyRealBonus")]
    public partial class tblTaquillas_Estado_prueba
    {
        public int idTaquilla { get; set; }
        public int idVehiculo { get; set; }
        public bool? disponible { get; set; }

        [ForeignKey("idTaquilla")]
        public virtual tblTaquillas_prueba idTaquillaNavigation { get; set; } = null!;
        [ForeignKey("idVehiculo")]
        public virtual tblVehiculo idVehiculoNavigation { get; set; } = null!;
    }
}
