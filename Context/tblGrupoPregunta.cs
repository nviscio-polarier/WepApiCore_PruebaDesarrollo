using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblGrupoPregunta", Schema = "ControlCalidad")]
    public partial class tblGrupoPregunta
    {
        public tblGrupoPregunta()
        {
            tblPregunta = new HashSet<tblPregunta>();
        }

        [Key]
        public short idGrupoPregunta { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idGrupoPreguntaNavigation")]
        public virtual ICollection<tblPregunta> tblPregunta { get; set; }
    }
}
