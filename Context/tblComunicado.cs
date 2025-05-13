using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblComunicado", Schema = "RRHH")]
    public partial class tblComunicado
    {
        public tblComunicado()
        {
            tblComunicadoNPersona = new HashSet<tblComunicadoNPersona>();
        }

        [Key]
        public int idComunicado { get; set; }
        public DateTimeOffset fecha { get; set; }
        public DateTimeOffset? fechaFin { get; set; }
        public string titulo { get; set; } = null!;
        public string? contenido { get; set; }
        public string? tipo { get; set; }
        [StringLength(50)]
        public string? icon { get; set; }
        [Required]
        public bool? isImportante { get; set; }
        public bool isDismiss { get; set; }
        public string? URL { get; set; }

        [InverseProperty("idComunicadoNavigation")]
        public virtual ICollection<tblComunicadoNPersona> tblComunicadoNPersona { get; set; }
    }
}
