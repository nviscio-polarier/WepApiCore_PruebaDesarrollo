using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersonaNMaquina", Schema = "MyRealData")]
    public partial class tblPersonaNMaquina
    {
        [Key]
        public int idPersonaNMaquina { get; set; }
        public int idPersona { get; set; }
        public int idMaquina { get; set; }
        public DateTimeOffset? fechaIni { get; set; }
        public DateTimeOffset? fechaFin { get; set; }
        public int? numPos { get; set; }
        public bool isOffline { get; set; }

        [ForeignKey("idMaquina")]
        [InverseProperty("tblPersonaNMaquina")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblPersonaNMaquina")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
