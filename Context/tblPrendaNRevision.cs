using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNRevision", Schema = "Office")]
    [Index("idRevision", Name = "IX_tblPrendaNRevision_idRevision")]
    public partial class tblPrendaNRevision
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idRevision { get; set; }
        public short stock { get; set; }
        public short pedido { get; set; }
        public short repartido { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNRevision")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
        [ForeignKey("idRevision")]
        [InverseProperty("tblPrendaNRevision")]
        public virtual tblRevision idRevisionNavigation { get; set; } = null!;
    }
}
