using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRevisionVehiculo", Schema = "Logistica")]
    public partial class tblRevisionVehiculo
    {
        public tblRevisionVehiculo()
        {
            tblRevisionVehiculoNParteTransporte = new HashSet<tblRevisionVehiculoNParteTransporte>();
        }

        [Key]
        public byte idRevisionVehiculo { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idRevisionVehiculoNavigation")]
        public virtual ICollection<tblRevisionVehiculoNParteTransporte> tblRevisionVehiculoNParteTransporte { get; set; }
    }
}
