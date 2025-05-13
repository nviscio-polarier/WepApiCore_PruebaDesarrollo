using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersonasNParte", Schema = "Assistant")]
    public partial class tblPersonasNParte
    {
        [Key]
        public int idPersona { get; set; }
        [Key]
        public int idParte { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan horas { get; set; }

        [ForeignKey("idParte")]
        [InverseProperty("tblPersonasNParte")]
        public virtual tblParteTrabajo idParteNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblPersonasNParte")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
