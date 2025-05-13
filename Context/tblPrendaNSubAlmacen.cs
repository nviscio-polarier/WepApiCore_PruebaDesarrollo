using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNSubAlmacen", Schema = "Office")]
    public partial class tblPrendaNSubAlmacen
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idSubAlmacen { get; set; }
        public int stock { get; set; }
        public int actual { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNSubAlmacen")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
        [ForeignKey("idSubAlmacen")]
        [InverseProperty("tblPrendaNSubAlmacen")]
        public virtual tblSubAlmacen idSubAlmacenNavigation { get; set; } = null!;
    }
}
