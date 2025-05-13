using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblComunicadoNPersona", Schema = "RRHH")]
    public partial class tblComunicadoNPersona
    {
        [Key]
        public int idPersona { get; set; }
        [Key]
        public int idComunicado { get; set; }
        public DateTimeOffset? fechaAccion { get; set; }
        public DateTimeOffset? fechaDismiss { get; set; }

        [ForeignKey("idComunicado")]
        [InverseProperty("tblComunicadoNPersona")]
        public virtual tblComunicado idComunicadoNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblComunicadoNPersona")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
