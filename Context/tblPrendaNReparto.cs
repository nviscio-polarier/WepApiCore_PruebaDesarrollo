using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNReparto", Schema = "Logistica")]
    [Index("idReparto", Name = "IX_tblPrendaNReparto_idReparto")]
    public partial class tblPrendaNReparto
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idReparto { get; set; }
        public int cantidad { get; set; }
        public int? rechazo { get; set; }
        public int? retiro { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNReparto")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
        [ForeignKey("idReparto")]
        [InverseProperty("tblPrendaNReparto")]
        public virtual tblReparto idRepartoNavigation { get; set; } = null!;
    }
}
