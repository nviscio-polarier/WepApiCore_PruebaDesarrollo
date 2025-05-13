using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaEjecutivo", Schema = "MyValet")]
    public partial class tblPrendaEjecutivo
    {
        public tblPrendaEjecutivo()
        {
            tblPrendaEjecutivoNRepartoValet = new HashSet<tblPrendaEjecutivoNRepartoValet>();
        }

        [Key]
        public int idPrendaEjecutivo { get; set; }
        public string? denominacion { get; set; }
        [StringLength(10)]
        public string codigoPrendaEjecutivo { get; set; } = null!;
        public int peso { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precioLavadoDoblado { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precioLavadoVaporizado { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precioPlanchado { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precioLavadoSecadoPlanchado { get; set; }
        public int? incLavadoExpress { get; set; }
        public int idLavanderia { get; set; }
        [Column(TypeName = "decimal(10, 4)")]
        public decimal coste { get; set; }
        public bool eliminado { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPrendaEjecutivo")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idPrendaEjecutivoNavigation")]
        public virtual ICollection<tblPrendaEjecutivoNRepartoValet> tblPrendaEjecutivoNRepartoValet { get; set; }
    }
}
