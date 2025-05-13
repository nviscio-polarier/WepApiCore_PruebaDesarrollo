using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMarcaTapa", Schema = "General")]
    public partial class tblMarcaTapa
    {
        public tblMarcaTapa()
        {
            tblCantidadNMovimientoElemLog = new HashSet<tblCantidadNMovimientoElemLog>();
            tblPlantillaPrenda_generica = new HashSet<tblPlantillaPrenda_generica>();
            tblPrenda = new HashSet<tblPrenda>();
        }

        [Key]
        public byte idMarcaTapa { get; set; }
        public string denominacion { get; set; } = null!;
        [StringLength(10)]
        public string? marca { get; set; }

        [InverseProperty("idMarcaTapaNavigation")]
        public virtual ICollection<tblCantidadNMovimientoElemLog> tblCantidadNMovimientoElemLog { get; set; }
        [InverseProperty("idMarcaTapaNavigation")]
        public virtual ICollection<tblPlantillaPrenda_generica> tblPlantillaPrenda_generica { get; set; }
        [InverseProperty("idMarcaTapaNavigation")]
        public virtual ICollection<tblPrenda> tblPrenda { get; set; }
    }
}
