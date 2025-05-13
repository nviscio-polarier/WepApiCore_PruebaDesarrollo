using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEnergyHub", Schema = "MyRealData")]
    public partial class tblEnergyHub
    {
        public tblEnergyHub()
        {
            tblEstadoEnergyHub = new HashSet<tblEstadoEnergyHub>();
            tblLogConexiones = new HashSet<tblLogConexiones>();
        }

        [Key]
        public int idEnergyHub { get; set; }
        public int idLavanderia { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblEnergyHub")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idEnergyHubNavigation")]
        public virtual ICollection<tblEstadoEnergyHub> tblEstadoEnergyHub { get; set; }
        [InverseProperty("idEnergyHubNavigation")]
        public virtual ICollection<tblLogConexiones> tblLogConexiones { get; set; }
    }
}
