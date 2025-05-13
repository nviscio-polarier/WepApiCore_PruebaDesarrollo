using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPlantillaPrenda_generica_historico_peso", Schema = "General")]
    public partial class tblPlantillaPrenda_generica_historico_peso
    {
        [Key]
        public int idPlantillaPrenda_generica { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public int peso { get; set; }

        [ForeignKey("idPlantillaPrenda_generica")]
        [InverseProperty("tblPlantillaPrenda_generica_historico_peso")]
        public virtual tblPlantillaPrenda_generica idPlantillaPrenda_genericaNavigation { get; set; } = null!;
    }
}
