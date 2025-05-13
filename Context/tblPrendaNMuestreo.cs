using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNMuestreo", Schema = "MyQuality")]
    public partial class tblPrendaNMuestreo
    {
        [Key]
        public int idPrendaNMuestreo { get; set; }
        public int idMuestreo { get; set; }
        public int idPrenda { get; set; }
        public short limpio { get; set; }
        public short manchado { get; set; }
        public short FueraServicio { get; set; }
        public int? idEntidad { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public int? idCompañia { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblPrendaNMuestreo")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblPrendaNMuestreo")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idMuestreo")]
        [InverseProperty("tblPrendaNMuestreo")]
        public virtual tblMuestreo idMuestreoNavigation { get; set; } = null!;
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNMuestreo")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
