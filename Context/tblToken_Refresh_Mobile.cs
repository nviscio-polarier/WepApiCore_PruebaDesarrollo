using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblToken_Refresh_Mobile", Schema = "GestionInterna")]
    public partial class tblToken_Refresh_Mobile
    {
        [Key]
        public Guid idToken { get; set; }
        public int idUsuario { get; set; }
        public DateTimeOffset? fechaExpiracion { get; set; }

        [ForeignKey("idUsuario")]
        [InverseProperty("tblToken_Refresh_Mobile")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
