using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLibreSemanal", Schema = "RRHH")]
    public partial class tblLibreSemanal
    {
        [Key]
        public int idLibreSemanal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaInicioUnDia { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaInicioDosDias { get; set; }
        public byte tipo { get; set; }
        public byte? frecuenciaUnDia { get; set; }
        public byte? frecuenciaDosDias { get; set; }
        public byte? segundoDia { get; set; }
        public int idPersona { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblLibreSemanal")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
