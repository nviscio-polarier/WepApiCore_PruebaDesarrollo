using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblFormularioNUsuario", Schema = "GestionInterna")]
    public partial class tblFormularioNUsuario
    {
        [Key]
        public int idFormulario { get; set; }
        [Key]
        public int idUsuario { get; set; }
        [Required]
        public bool? isEscritura { get; set; }
        [Required]
        public bool? isBorrado { get; set; }

        [ForeignKey("idFormulario")]
        [InverseProperty("tblFormularioNUsuario")]
        public virtual tblFormulario idFormularioNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblFormularioNUsuario")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
