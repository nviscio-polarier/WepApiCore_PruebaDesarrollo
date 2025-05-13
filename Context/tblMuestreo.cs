using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMuestreo", Schema = "MyQuality")]
    public partial class tblMuestreo
    {
        public tblMuestreo()
        {
            tblPrendaNMuestreo = new HashSet<tblPrendaNMuestreo>();
            tblPrendaNMuestreo_FS = new HashSet<tblPrendaNMuestreo_FS>();
        }

        [Key]
        public int idMuestreo { get; set; }
        public int idLavanderia { get; set; }
        public byte idTipoMuestreo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblMuestreo")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idTipoMuestreo")]
        [InverseProperty("tblMuestreo")]
        public virtual tblTipoMuestreo idTipoMuestreoNavigation { get; set; } = null!;
        [InverseProperty("idMuestreoNavigation")]
        public virtual ICollection<tblPrendaNMuestreo> tblPrendaNMuestreo { get; set; }
        [InverseProperty("idMuestreoNavigation")]
        public virtual ICollection<tblPrendaNMuestreo_FS> tblPrendaNMuestreo_FS { get; set; }
    }
}
