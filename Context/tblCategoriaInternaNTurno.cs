using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCategoriaInternaNTurno", Schema = "RRHH")]
    public partial class tblCategoriaInternaNTurno
    {
        [Key]
        public int idCategoriaInterna { get; set; }
        [Key]
        public int idTurno { get; set; }
        public bool? isOficina { get; set; }

        [ForeignKey("idCategoriaInterna")]
        [InverseProperty("tblCategoriaInternaNTurno")]
        public virtual tblCategoriaInterna idCategoriaInternaNavigation { get; set; } = null!;
        [ForeignKey("idTurno")]
        [InverseProperty("tblCategoriaInternaNTurno")]
        public virtual tblTurno idTurnoNavigation { get; set; } = null!;
    }
}
