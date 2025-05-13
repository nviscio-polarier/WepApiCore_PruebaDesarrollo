using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNMovimiento", Schema = "Inventarios")]
    public partial class tblPrendaNMovimiento
    {
        public int idMovimiento { get; set; }
        public int? idPrenda { get; set; }
        public int cantidad { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal? precio { get; set; }
        [Key]
        public int idPrendaNMovimiento { get; set; }
        public int? idPlantillaPrenda_generica { get; set; }

        [ForeignKey("idMovimiento")]
        [InverseProperty("tblPrendaNMovimiento")]
        public virtual tblMovimiento idMovimientoNavigation { get; set; } = null!;
        [ForeignKey("idPlantillaPrenda_generica")]
        [InverseProperty("tblPrendaNMovimiento")]
        public virtual tblPlantillaPrenda_generica? idPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNMovimiento")]
        public virtual tblPrenda? idPrendaNavigation { get; set; }
    }
}
