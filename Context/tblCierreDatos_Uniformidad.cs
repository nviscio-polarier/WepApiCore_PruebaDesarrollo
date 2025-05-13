using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCierreDatos_Uniformidad", Schema = "General")]
    public partial class tblCierreDatos_Uniformidad
    {
        [Key]
        public int idLavanderia { get; set; }
        [Key]
        public short año { get; set; }
        [Key]
        public byte mes { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaDesde { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaHasta { get; set; }
        public bool isCerrado { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal? precioUnidad_MyUniform { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblCierreDatos_Uniformidad")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
    }
}
