using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTareaPersonaDia", Schema = "Assistant")]
    [Index("idPersona", Name = "IX_tblTareaPersonaDia_idPersona")]
    [Index("idTareaMaquina", Name = "IX_tblTareaPersonaDia_idTareaMaquina")]
    public partial class tblTareaPersonaDia
    {
        [Key]
        public int idTareaMaquina { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public int idPersona { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblTareaPersonaDia")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTareaMaquina")]
        [InverseProperty("tblTareaPersonaDia")]
        public virtual tblTareaMaquina idTareaMaquinaNavigation { get; set; } = null!;
    }
}
