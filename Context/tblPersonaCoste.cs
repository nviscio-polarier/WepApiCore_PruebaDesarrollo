using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersonaCoste", Schema = "RRHH")]
    public partial class tblPersonaCoste
    {
        [Key]
        public int idPersona { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal costeHora { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblPersonaCoste")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
