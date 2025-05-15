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
        [Key]
        public int idTipoEventoToken { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? denominacion { get; set; }
    }
}
