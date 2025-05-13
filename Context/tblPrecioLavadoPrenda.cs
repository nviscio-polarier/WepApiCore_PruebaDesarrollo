using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrecioLavadoPrenda", Schema = "Costes")]
    public partial class tblPrecioLavadoPrenda
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precio { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrecioLavadoPrenda")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
