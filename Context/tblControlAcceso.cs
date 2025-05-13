using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblControlAcceso", Schema = "RRHH")]
    public partial class tblControlAcceso
    {
        [Key]
        public int idPersona { get; set; }
        [Key]
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public byte idTipoAcceso { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblControlAcceso")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTipoAcceso")]
        [InverseProperty("tblControlAcceso")]
        public virtual tblTipoAcceso idTipoAccesoNavigation { get; set; } = null!;
    }
}
