using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoPregunta", Schema = "ControlCalidad")]
    public partial class tblTipoPregunta
    {
        public tblTipoPregunta()
        {
            tblPregunta = new HashSet<tblPregunta>();
        }

        [Key]
        public short idTipoPregunta { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoPreguntaNavigation")]
        public virtual ICollection<tblPregunta> tblPregunta { get; set; }
    }
}
