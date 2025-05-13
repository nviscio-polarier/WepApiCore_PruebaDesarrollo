using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaPrecioRefact", Schema = "General")]
    public partial class tblPrendaPrecioRefact
    {
        [Key]
        public int idPrenda { get; set; }
        [Column(TypeName = "decimal(6, 4)")]
        public decimal precioRefacturado { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaPrecioRefact")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
