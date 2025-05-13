using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblSesionesVisualizacion", Schema = "MyRealLearning")]
    public partial class tblSesionesVisualizacion
    {
        [Key]
        public int idSesion { get; set; }
        public int idPersonaVideo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fechaInicio { get; set; }
        public double? segundosVisualizados { get; set; }
        public bool? finalizadoYoutube { get; set; }
        public bool? finalizadoManual { get; set; }

        [ForeignKey("idPersonaVideo")]
        [InverseProperty("tblSesionesVisualizacion")]
        public virtual tblVideoNPersona idPersonaVideoNavigation { get; set; } = null!;
    }
}
