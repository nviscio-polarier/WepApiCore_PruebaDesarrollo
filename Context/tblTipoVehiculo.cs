using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoVehiculo", Schema = "Logistica")]
    public partial class tblTipoVehiculo
    {
        public tblTipoVehiculo()
        {
            tblVehiculo = new HashSet<tblVehiculo>();
        }

        [Key]
        public byte idTipoVehiculo { get; set; }
        public byte codigo { get; set; }
        [StringLength(20)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoVehiculoNavigation")]
        public virtual ICollection<tblVehiculo> tblVehiculo { get; set; }
    }
}
