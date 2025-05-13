using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDiasLibresPersonal", Schema = "RRHH")]
    public partial class tblDiasLibresPersonal
    {
        [Key]
        public int idDiasLibresPersonal { get; set; }
        public int? idPersona { get; set; }
        public byte? idDiaSemana { get; set; }
        public int? idDiaMes { get; set; }
        public byte? numDia { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblDiasLibresPersonal")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
    }
}
