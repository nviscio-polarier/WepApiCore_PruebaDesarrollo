using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoFueraServicio", Schema = "MyQuality")]
    public partial class tblTipoFueraServicio
    {
        public tblTipoFueraServicio()
        {
            tblPrendaNMuestreo_FS = new HashSet<tblPrendaNMuestreo_FS>();
        }

        [Key]
        public byte idTipoFS { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoFSNavigation")]
        public virtual ICollection<tblPrendaNMuestreo_FS> tblPrendaNMuestreo_FS { get; set; }
    }
}
