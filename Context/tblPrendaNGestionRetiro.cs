using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNGestionRetiro", Schema = "MyQuality")]
    public partial class tblPrendaNGestionRetiro
    {
        public int idGestionRetiro { get; set; }
        public int? idPrenda { get; set; }
        public int idTipoRetiro { get; set; }
        public int cantidad { get; set; }
        [Key]
        public int idPrendaNGestionRetiro { get; set; }
        public int? idPlantillaPrenda_generica { get; set; }

        [ForeignKey("idGestionRetiro")]
        [InverseProperty("tblPrendaNGestionRetiro")]
        public virtual tblGestionRetiro idGestionRetiroNavigation { get; set; } = null!;
        [ForeignKey("idPlantillaPrenda_generica")]
        [InverseProperty("tblPrendaNGestionRetiro")]
        public virtual tblPlantillaPrenda_generica? idPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNGestionRetiro")]
        public virtual tblPrenda? idPrendaNavigation { get; set; }
        [ForeignKey("idTipoRetiro")]
        [InverseProperty("tblPrendaNGestionRetiro")]
        public virtual tblTipoRetiro idTipoRetiroNavigation { get; set; } = null!;
    }
}
