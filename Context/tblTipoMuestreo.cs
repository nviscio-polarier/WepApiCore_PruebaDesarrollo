using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoMuestreo", Schema = "MyQuality")]
    public partial class tblTipoMuestreo
    {
        public tblTipoMuestreo()
        {
            tblMuestreo = new HashSet<tblMuestreo>();
        }

        [Key]
        public byte idTipoMuestreo { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoMuestreoNavigation")]
        public virtual ICollection<tblMuestreo> tblMuestreo { get; set; }
    }
}
