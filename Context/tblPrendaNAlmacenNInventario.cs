using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNAlmacenNInventario", Schema = "Inventarios")]
    [Index("idInventario", "idAlmacen", "idPrenda", "idPlantillaPrenda_generica", Name = "UK_tblPrendaNAlmacenNInventario", IsUnique = true)]
    public partial class tblPrendaNAlmacenNInventario
    {
        public int idInventario { get; set; }
        public int idAlmacen { get; set; }
        public int? idPrenda { get; set; }
        public int? cantidad { get; set; }
        public short? parStock { get; set; }
        public bool aplicado { get; set; }
        [Key]
        public int idPrendaNAlmacenNInventario { get; set; }
        public int? idPlantillaPrenda_generica { get; set; }

        [ForeignKey("idInventario,idAlmacen")]
        [InverseProperty("tblPrendaNAlmacenNInventario")]
        public virtual tblAlmacenNInventario id { get; set; } = null!;
        [ForeignKey("idPlantillaPrenda_generica")]
        [InverseProperty("tblPrendaNAlmacenNInventario")]
        public virtual tblPlantillaPrenda_generica? idPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNAlmacenNInventario")]
        public virtual tblPrenda? idPrendaNavigation { get; set; }
    }
}
