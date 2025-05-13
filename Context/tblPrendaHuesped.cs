using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaHuesped", Schema = "MyValet")]
    public partial class tblPrendaHuesped
    {
        public tblPrendaHuesped()
        {
            tblPrendaHuespedNRepartoValet = new HashSet<tblPrendaHuespedNRepartoValet>();
            tblPrendaNPedidoHuesped = new HashSet<tblPrendaNPedidoHuesped>();
        }

        [Key]
        public int idPrendaHuesped { get; set; }
        public string? denominacion { get; set; }
        [StringLength(10)]
        public string codigoPrendaHuesped { get; set; } = null!;
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
        [InverseProperty("tblPrendaHuesped")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idPrendaHuespedNavigation")]
        public virtual ICollection<tblPrendaHuespedNRepartoValet> tblPrendaHuespedNRepartoValet { get; set; }
        [InverseProperty("idPrendaHuespedNavigation")]
        public virtual ICollection<tblPrendaNPedidoHuesped> tblPrendaNPedidoHuesped { get; set; }
    }
}
