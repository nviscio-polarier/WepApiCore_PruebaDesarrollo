using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRecoveryPassword", Schema = "GestionInterna")]
    public partial class tblRecoveryPassword
    {
        [Key]
        public int idUsuario { get; set; }
        [Key]
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public Guid guid { get; set; }

        [ForeignKey("idUsuario")]
        [InverseProperty("tblRecoveryPassword")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
