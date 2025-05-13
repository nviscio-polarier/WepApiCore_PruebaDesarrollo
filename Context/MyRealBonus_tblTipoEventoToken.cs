using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("MyRealBonus.tblTipoEventoToken", Schema = "MyRealBonus")]
    public partial class MyRealBonus_tblTipoEventoToken
    {
        public MyRealBonus_tblTipoEventoToken()
        {
            tblPersonaTokens = new HashSet<tblPersonaTokens>();
        }

        [Key]
        public int idTipoEventoToken { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? denominacion { get; set; }

        [InverseProperty("idTipoEventoTokenNavigation")]
        public virtual ICollection<tblPersonaTokens> tblPersonaTokens { get; set; }
    }
}
