using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCategoriaMaquina", Schema = "Maquinaria")]
    public partial class tblCategoriaMaquina
    {
        public tblCategoriaMaquina()
        {
            tblArticuloMaquinaria = new HashSet<tblArticuloMaquinaria>();
            tblPesoNCategoriaNLavanderia = new HashSet<tblPesoNCategoriaNLavanderia>();
            tblTipoMaquinaNCategoriaMaquina = new HashSet<tblTipoMaquinaNCategoriaMaquina>();
        }

        [Key]
        public int idCategoriaMaquina { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public int? orden { get; set; }
        [StringLength(50)]
        public string? idxGsBase { get; set; }
        public int? idSistemaMaquina { get; set; }

        [ForeignKey("idSistemaMaquina")]
        [InverseProperty("tblCategoriaMaquina")]
        public virtual tblSistemaMaquina? idSistemaMaquinaNavigation { get; set; }
        [InverseProperty("idCategoriaMaquinaNavigation")]
        public virtual ICollection<tblArticuloMaquinaria> tblArticuloMaquinaria { get; set; }
        [InverseProperty("idCategoriaMaquinaNavigation")]
        public virtual ICollection<tblPesoNCategoriaNLavanderia> tblPesoNCategoriaNLavanderia { get; set; }
        [InverseProperty("idCategoriaMaquinaNavigation")]
        public virtual ICollection<tblTipoMaquinaNCategoriaMaquina> tblTipoMaquinaNCategoriaMaquina { get; set; }
    }
}
