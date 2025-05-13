using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersonaNMantenimientoPrev", Schema = "Assistant")]
    public partial class tblPersonaNMantenimientoPrev
    {
        [Key]
        public int idMantenimientoPrev { get; set; }
        [Key]
        public int idPersona { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan? tiempoRegistrado { get; set; }

        [ForeignKey("idMantenimientoPrev")]
        [InverseProperty("tblPersonaNMantenimientoPrev")]
        public virtual tblMantenimientoPrev idMantenimientoPrevNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblPersonaNMantenimientoPrev")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
