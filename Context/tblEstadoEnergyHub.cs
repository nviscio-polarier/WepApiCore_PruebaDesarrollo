using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoEnergyHub", Schema = "MyRealData")]
    public partial class tblEstadoEnergyHub
    {
        [Key]
        public int idEstadoEnergyHub { get; set; }
        public string denominacion { get; set; } = null!;
        [Precision(0)]
        public DateTimeOffset? fecha { get; set; }
        public int? idEnergyHub { get; set; }

        [ForeignKey("idEnergyHub")]
        [InverseProperty("tblEstadoEnergyHub")]
        public virtual tblEnergyHub? idEnergyHubNavigation { get; set; }
    }
}
