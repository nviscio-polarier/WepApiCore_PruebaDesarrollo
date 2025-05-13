using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblVideoNPersona", Schema = "MyRealLearning")]
    public partial class tblVideoNPersona
    {
        public tblVideoNPersona()
        {
            tblSesionesVisualizacion = new HashSet<tblSesionesVisualizacion>();
        }

        [Key]
        public int idPersonaVideo { get; set; }
        public int? idPersona { get; set; }
        public int? idPersonaRemitente { get; set; }
        public int? idVideo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaEnvio { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaAbierto { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaFinalizado { get; set; }
        public int? idEstado { get; set; }
        [Column(TypeName = "text")]
        public string? comentario { get; set; }

        [ForeignKey("idEstado")]
        [InverseProperty("tblVideoNPersona")]
        public virtual tblEstadosVideo? idEstadoNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblVideoNPersonaidPersonaNavigation")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idPersonaRemitente")]
        [InverseProperty("tblVideoNPersonaidPersonaRemitenteNavigation")]
        public virtual tblPersona? idPersonaRemitenteNavigation { get; set; }
        [ForeignKey("idVideo")]
        [InverseProperty("tblVideoNPersona")]
        public virtual tblVideo? idVideoNavigation { get; set; }
        [InverseProperty("idPersonaVideoNavigation")]
        public virtual ICollection<tblSesionesVisualizacion> tblSesionesVisualizacion { get; set; }
    }
}
