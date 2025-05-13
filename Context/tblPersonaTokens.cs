using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersonaTokens", Schema = "MyRealBonus")]
    public partial class tblPersonaTokens
    {
        public tblPersonaTokens()
        {
            tblSolicitudesTokens = new HashSet<tblSolicitudesTokens>();
        }

        public int idPersona { get; set; }
        public DateTimeOffset fecha { get; set; }
        public int idLavanderia { get; set; }
        public int tokens { get; set; }
        public bool isCanje { get; set; }
        public int? idTipoProducto { get; set; }
        public int? idTipoEventoToken { get; set; }
        [Key]
        public int idPersonaToken { get; set; }
        public DateTimeOffset? fechaIniTurno { get; set; }
        public DateTimeOffset? fechaFinTurno { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblPersonaTokens")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTipoEventoToken")]
        [InverseProperty("tblPersonaTokens")]
        public virtual tblTipoEventoToken? idTipoEventoToken1 { get; set; }
        [ForeignKey("idTipoEventoToken")]
        [InverseProperty("tblPersonaTokens")]
        public virtual MyRealBonus_tblTipoEventoToken? idTipoEventoTokenNavigation { get; set; }
        [ForeignKey("idTipoProducto")]
        [InverseProperty("tblPersonaTokens")]
        public virtual tblProductosTokens? idTipoProductoNavigation { get; set; }
        [InverseProperty("idPersonaTokenNavigation")]
        public virtual ICollection<tblSolicitudesTokens> tblSolicitudesTokens { get; set; }
    }
}
