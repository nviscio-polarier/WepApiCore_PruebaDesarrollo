using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDenoPrenda", Schema = "General")]
    public partial class tblDenoPrenda
    {
        public tblDenoPrenda()
        {
            tblArticuloLenceria = new HashSet<tblArticuloLenceria>();
            tblPlantillaPrenda_generica = new HashSet<tblPlantillaPrenda_generica>();
            tblPrenda = new HashSet<tblPrenda>();
        }

        [Key]
        public short idDenoPrenda { get; set; }
        [StringLength(2)]
        public string? codigo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public short idTipoPrenda { get; set; }
        public int idTraduccion { get; set; }

        [ForeignKey("idTipoPrenda")]
        [InverseProperty("tblDenoPrenda")]
        public virtual tblTipoPrenda idTipoPrendaNavigation { get; set; } = null!;
        [ForeignKey("idTraduccion")]
        [InverseProperty("tblDenoPrenda")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idDenoPrendaNavigation")]
        public virtual ICollection<tblArticuloLenceria> tblArticuloLenceria { get; set; }
        [InverseProperty("idDenoPrendaNavigation")]
        public virtual ICollection<tblPlantillaPrenda_generica> tblPlantillaPrenda_generica { get; set; }
        [InverseProperty("idDenoPrendaNavigation")]
        public virtual ICollection<tblPrenda> tblPrenda { get; set; }
    }
}
