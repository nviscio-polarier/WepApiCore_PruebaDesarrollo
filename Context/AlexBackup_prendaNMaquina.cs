using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Keyless]
    [Table("AlexBackup_prendaNMaquina", Schema = "MyRealBonus")]
    public partial class AlexBackup_prendaNMaquina
    {
        public int idPrendaNMaquina { get; set; }
        public int idMaquina { get; set; }
        public DateTimeOffset fecha { get; set; }
        public int? numVia { get; set; }
        public byte idFamilia { get; set; }
        public short? idTipoPrenda { get; set; }
        public bool isOffline { get; set; }
    }
}
