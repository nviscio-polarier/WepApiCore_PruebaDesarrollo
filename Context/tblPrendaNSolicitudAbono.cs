using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNSolicitudAbono", Schema = "Logistica")]
    public partial class tblPrendaNSolicitudAbono
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idSolicitudAbono { get; set; }
        public int cantidad { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNSolicitudAbono")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
        [ForeignKey("idSolicitudAbono")]
        [InverseProperty("tblPrendaNSolicitudAbono")]
        public virtual tblSolicitudAbono idSolicitudAbonoNavigation { get; set; } = null!;
    }
}
