using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Keyless]
    [Table("AlexBackup_personaTokens", Schema = "MyRealBonus")]
    public partial class AlexBackup_personaTokens
    {
        public int idPersona { get; set; }
        public DateTimeOffset fecha { get; set; }
        public int idLavanderia { get; set; }
        public int tokens { get; set; }
        public bool isCanje { get; set; }
        public int? idTipoProducto { get; set; }
        public int? idTipoEventoToken { get; set; }
        public int idPersonaToken { get; set; }
        public DateTimeOffset? fechaIniTurno { get; set; }
        public DateTimeOffset? fechaFinTurno { get; set; }
    }
}
