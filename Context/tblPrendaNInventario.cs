using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNInventario", Schema = "Inventarios")]
    [Index("idInventario", "idPrenda", "idPlantillaPrenda_generica", Name = "UK_tblPrendaNInventario", IsUnique = true)]
    public partial class tblPrendaNInventario
    {
        public int idInventario { get; set; }
        public int? idPrenda { get; set; }
        [Key]
        public int idPrendaNInventario { get; set; }
        public int? idPlantillaPrenda_generica { get; set; }

        [ForeignKey("idInventario")]
        [InverseProperty("tblPrendaNInventario")]
        public virtual tblInventario idInventarioNavigation { get; set; } = null!;
        [ForeignKey("idPlantillaPrenda_generica")]
        [InverseProperty("tblPrendaNInventario")]
        public virtual tblPlantillaPrenda_generica? idPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNInventario")]
        public virtual tblPrenda? idPrendaNavigation { get; set; }
    }
}
