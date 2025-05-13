using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDiasLibresPersonal_Llamamiento", Schema = "RRHH")]
    public partial class tblDiasLibresPersonal_Llamamiento
    {
        [Key]
        public int idDiasLibresPersonal_Llamamiento { get; set; }
        public int idLlamamiento { get; set; }
        public byte? idDiaSemana { get; set; }
        public int? idDiaMes { get; set; }
        public byte? numDia { get; set; }

        [ForeignKey("idLlamamiento")]
        [InverseProperty("tblDiasLibresPersonal_Llamamiento")]
        public virtual tblLlamamiento idLlamamientoNavigation { get; set; } = null!;
    }
}
