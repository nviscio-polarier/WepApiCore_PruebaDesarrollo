using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNUsuarioNEntidad", Schema = "General")]
    public partial class tblPrendaNUsuarioNEntidad
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idUsuario { get; set; }
        [Key]
        public int idEntidad { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblPrendaNUsuarioNEntidad")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNUsuarioNEntidad")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblPrendaNUsuarioNEntidad")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
