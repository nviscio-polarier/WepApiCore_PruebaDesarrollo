using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNMuestreo_FS", Schema = "MyQuality")]
    public partial class tblPrendaNMuestreo_FS
    {
        [Key]
        public int idPrendaNMuestreoFS { get; set; }
        public int idMuestreo { get; set; }
        public int idCompañia { get; set; }
        public int idPrenda { get; set; }
        public int año { get; set; }
        public byte mes { get; set; }
        public byte idTipoFS { get; set; }
        public int cantidad { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblPrendaNMuestreo_FS")]
        public virtual tblCompañia idCompañiaNavigation { get; set; } = null!;
        [ForeignKey("idMuestreo")]
        [InverseProperty("tblPrendaNMuestreo_FS")]
        public virtual tblMuestreo idMuestreoNavigation { get; set; } = null!;
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNMuestreo_FS")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
        [ForeignKey("idTipoFS")]
        [InverseProperty("tblPrendaNMuestreo_FS")]
        public virtual tblTipoFueraServicio idTipoFSNavigation { get; set; } = null!;
    }
}
