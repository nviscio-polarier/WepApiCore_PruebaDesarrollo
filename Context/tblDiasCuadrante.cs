using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDiasCuadrante", Schema = "RRHH")]
    public partial class tblDiasCuadrante
    {
        [Key]
        public int idDiasCuadrante { get; set; }
        public int? idPersona { get; set; }
        public int? idTurno { get; set; }
        public int? idTipoDiaCuadrante { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecha { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblDiasCuadrante")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idTipoDiaCuadrante")]
        [InverseProperty("tblDiasCuadrante")]
        public virtual tblTipoDiaCuadrante? idTipoDiaCuadranteNavigation { get; set; }
        [ForeignKey("idTurno")]
        [InverseProperty("tblDiasCuadrante")]
        public virtual tblTurno? idTurnoNavigation { get; set; }
    }
}
