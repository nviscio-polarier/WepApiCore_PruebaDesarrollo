using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoEventoToken", Schema = "MyRealBonus")]
    public partial class tblTipoEventoToken
    {
        public tblTipoEventoToken()
        {
            tblLogsToken = new HashSet<tblLogsToken>();
            tblPersonaTokens = new HashSet<tblPersonaTokens>();
        }

        [Key]
        public int idTipoEventoToken { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? denominacion { get; set; }

        [InverseProperty("idTipoEventoTokenNavigation")]
        public virtual ICollection<tblLogsToken> tblLogsToken { get; set; }
        [InverseProperty("idTipoEventoToken1")]
        public virtual ICollection<tblPersonaTokens> tblPersonaTokens { get; set; }
    }
}
