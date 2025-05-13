using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLogAcciones_App", Schema = "GestionInterna")]
    public partial class tblLogAcciones_App
    {
        [Key]
        public int idUsuario { get; set; }
        [Key]
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        public string app { get; set; } = null!;
        [Required]
        public bool? isAndroid { get; set; }
        public string? versionApp { get; set; }

        [ForeignKey("idUsuario")]
        [InverseProperty("tblLogAcciones_App")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
