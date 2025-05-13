using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNAlmacen", Schema = "Office")]
    [Index("idAlmacen", Name = "IX_tblPrendaNAlmacen_idAlmacen")]
    public partial class tblPrendaNAlmacen
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idAlmacen { get; set; }
        public int stock { get; set; }
        public int actual { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblPrendaNAlmacen")]
        public virtual tblAlmacen idAlmacenNavigation { get; set; } = null!;
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNAlmacen")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
