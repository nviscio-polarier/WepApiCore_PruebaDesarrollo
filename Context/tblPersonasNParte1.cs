using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersonasNParte", Schema = "Incidencias")]
    public partial class tblPersonasNParte1
    {
        [Key]
        public int idPersonasNParte { get; set; }
        public int idPersona { get; set; }
        public int idParte { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan horas { get; set; }

        [ForeignKey("idParte")]
        [InverseProperty("tblPersonasNParte1")]
        public virtual tblParteTrabajo1 idParteNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblPersonasNParte1")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
