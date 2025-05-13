using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRevisionVehiculoNParteTransporte", Schema = "Logistica")]
    public partial class tblRevisionVehiculoNParteTransporte
    {
        [Key]
        public byte idRevisionVehiculo { get; set; }
        [Key]
        public int idParteTransporte { get; set; }
        public string? incidencia { get; set; }
        public bool? estado { get; set; }

        [ForeignKey("idParteTransporte")]
        [InverseProperty("tblRevisionVehiculoNParteTransporte")]
        public virtual tblParteTransporte idParteTransporteNavigation { get; set; } = null!;
        [ForeignKey("idRevisionVehiculo")]
        [InverseProperty("tblRevisionVehiculoNParteTransporte")]
        public virtual tblRevisionVehiculo idRevisionVehiculoNavigation { get; set; } = null!;
    }
}
