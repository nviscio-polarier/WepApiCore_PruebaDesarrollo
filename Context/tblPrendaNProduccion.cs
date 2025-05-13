using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNProduccion", Schema = "Produccion")]
    [Index("hora", Name = "IX_tblPrendaNProduccion_hora")]
    [Index("idProduccion", Name = "IX_tblPrendaNProduccion_idProduccion")]
    public partial class tblPrendaNProduccion
    {
        public tblPrendaNProduccion()
        {
            tblRechazoNProduccion = new HashSet<tblRechazoNProduccion>();
        }

        [Key]
        public int idPrendaNProduccion { get; set; }
        public int? idPrenda { get; set; }
        public int idProduccion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? hora { get; set; }
        public int? idLineaPantalla { get; set; }
        public int Producido { get; set; }
        public int Rechazo { get; set; }
        public int Retiro { get; set; }
        public int? unidadesRepartidas { get; set; }
        public int? idPlantillaPrenda_generica { get; set; }

        [ForeignKey("idPlantillaPrenda_generica")]
        [InverseProperty("tblPrendaNProduccion")]
        public virtual tblPlantillaPrenda_generica? idPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNProduccion")]
        public virtual tblPrenda? idPrendaNavigation { get; set; }
        [ForeignKey("idProduccion")]
        [InverseProperty("tblPrendaNProduccion")]
        public virtual tblProduccion idProduccionNavigation { get; set; } = null!;
        [InverseProperty("idPrendaNProduccionNavigation")]
        public virtual ICollection<tblRechazoNProduccion> tblRechazoNProduccion { get; set; }
    }
}
