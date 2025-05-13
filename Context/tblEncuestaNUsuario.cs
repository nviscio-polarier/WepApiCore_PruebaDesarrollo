using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEncuestaNUsuario", Schema = "ControlCalidad")]
    public partial class tblEncuestaNUsuario
    {
        [Key]
        public int idEncuesta { get; set; }
        [Key]
        public int idUsuario { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaContesta { get; set; }

        [ForeignKey("idEncuesta")]
        [InverseProperty("tblEncuestaNUsuario")]
        public virtual tblEncuesta idEncuestaNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblEncuestaNUsuario")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
