using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLog", Schema = "GestionInterna")]
    public partial class tblLog
    {
        [Key]
        public Guid idToken { get; set; }
        public int idUsuario { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }

        [ForeignKey("idUsuario")]
        [InverseProperty("tblLog")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
