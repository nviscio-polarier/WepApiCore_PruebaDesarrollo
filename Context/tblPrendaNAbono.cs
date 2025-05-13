using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNAbono", Schema = "Logistica")]
    [Index("idAbono", Name = "IX_tblPrendaNAbono_idLavanderia_fecha")]
    public partial class tblPrendaNAbono
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idAbono { get; set; }
        public int cantidad { get; set; }

        [ForeignKey("idAbono")]
        [InverseProperty("tblPrendaNAbono")]
        public virtual tblAbono idAbonoNavigation { get; set; } = null!;
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNAbono")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
