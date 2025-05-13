using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblOpcion", Schema = "ControlCalidad")]
    public partial class tblOpcion
    {
        public tblOpcion()
        {
            tblPreguntaNOpcionNPregunta = new HashSet<tblPreguntaNOpcionNPregunta>();
            tblRespuesta = new HashSet<tblRespuesta>();
            idPregunta = new HashSet<tblPregunta>();
        }

        [Key]
        public short idOpcion { get; set; }
        public string descripcion { get; set; } = null!;
        public string? icon { get; set; }

        [InverseProperty("idOpcionNavigation")]
        public virtual ICollection<tblPreguntaNOpcionNPregunta> tblPreguntaNOpcionNPregunta { get; set; }
        [InverseProperty("idOpcionNavigation")]
        public virtual ICollection<tblRespuesta> tblRespuesta { get; set; }

        [ForeignKey("idOpcion")]
        [InverseProperty("idOpcion")]
        public virtual ICollection<tblPregunta> idPregunta { get; set; }
    }
}
