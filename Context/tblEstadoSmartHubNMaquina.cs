using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoSmartHubNMaquina", Schema = "MyRealData")]
    public partial class tblEstadoSmartHubNMaquina
    {
        [Key]
        public int idMaquina { get; set; }
        [Key]
        public DateTimeOffset fechaInicio { get; set; }
        public DateTimeOffset fechaUltimaActualizacion { get; set; }
        public byte idEstadoSmartHub { get; set; }

        [ForeignKey("idEstadoSmartHub")]
        [InverseProperty("tblEstadoSmartHubNMaquina")]
        public virtual tblEstadoSmartHub idEstadoSmartHubNavigation { get; set; } = null!;
        [ForeignKey("idMaquina")]
        [InverseProperty("tblEstadoSmartHubNMaquina")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
    }
}
