using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPesoNCategoriaNLavanderia", Schema = "Maquinaria")]
    public partial class tblPesoNCategoriaNLavanderia
    {
        [Key]
        public int idCategoriaMaquina { get; set; }
        [Key]
        public int idLavanderia { get; set; }
        [Column(TypeName = "numeric(3, 0)")]
        public decimal? peso { get; set; }

        [ForeignKey("idCategoriaMaquina")]
        [InverseProperty("tblPesoNCategoriaNLavanderia")]
        public virtual tblCategoriaMaquina idCategoriaMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPesoNCategoriaNLavanderia")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
    }
}
