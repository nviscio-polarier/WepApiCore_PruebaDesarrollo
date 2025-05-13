using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Keyless]
    [Table("AlexBackup_solicitudesTokens", Schema = "MyRealBonus")]
    public partial class AlexBackup_solicitudesTokens
    {
        public int idRevision { get; set; }
        public int idPersonaToken { get; set; }
        public DateTimeOffset fechaSolicitud { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaAprobacion { get; set; }
        public int? idPersona { get; set; }
    }
}
