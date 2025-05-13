using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaExtra", Schema = "MyValet")]
    public partial class tblPrendaExtra
    {
        public tblPrendaExtra()
        {
            tblPrendaExtraNRepartoValet = new HashSet<tblPrendaExtraNRepartoValet>();
        }

        [Key]
        public int idPrendaExtra { get; set; }
        public string? denominacion { get; set; }
        [StringLength(10)]
        public string codigoPrendaExtra { get; set; } = null!;
        public int peso { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precioLavado { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precioPlanchado { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precioLavadoPlanchado { get; set; }
        public int? incLavadoExpress { get; set; }
        public int idLavanderia { get; set; }
        [Column(TypeName = "decimal(10, 4)")]
        public decimal coste { get; set; }
        public bool eliminar { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precioTintoreria { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPrendaExtra")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idPrendaExtraNavigation")]
        public virtual ICollection<tblPrendaExtraNRepartoValet> tblPrendaExtraNRepartoValet { get; set; }
    }
}
