using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblBalanceHoras", Schema = "RRHH")]
    public partial class tblBalanceHoras
    {
        [Key]
        public int idBalanceHoras { get; set; }
        public int idPersona { get; set; }
        public int? idJornada { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public int minutos { get; set; }
        public bool isInicio { get; set; }

        [ForeignKey("idJornada")]
        [InverseProperty("tblBalanceHoras")]
        public virtual tblJornada? idJornadaNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblBalanceHoras")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
