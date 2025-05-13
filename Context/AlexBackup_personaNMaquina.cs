using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Keyless]
    [Table("AlexBackup_personaNMaquina", Schema = "MyRealBonus")]
    public partial class AlexBackup_personaNMaquina
    {
        public int idPersonaNMaquina { get; set; }
        public int idPersona { get; set; }
        public int idMaquina { get; set; }
        public DateTimeOffset? fechaIni { get; set; }
        public DateTimeOffset? fechaFin { get; set; }
        public int? numPos { get; set; }
        public bool isOffline { get; set; }
    }
}
