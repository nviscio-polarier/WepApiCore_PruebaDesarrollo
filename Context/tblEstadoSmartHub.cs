using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoSmartHub", Schema = "MyRealData")]
    public partial class tblEstadoSmartHub
    {
        public tblEstadoSmartHub()
        {
            tblEstadoSmartHubNMaquina = new HashSet<tblEstadoSmartHubNMaquina>();
        }

        [Key]
        public byte idEstadoSmartHub { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idEstadoSmartHubNavigation")]
        public virtual ICollection<tblEstadoSmartHubNMaquina> tblEstadoSmartHubNMaquina { get; set; }
    }
}
