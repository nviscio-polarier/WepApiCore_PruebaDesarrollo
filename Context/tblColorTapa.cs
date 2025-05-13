using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblColorTapa", Schema = "General")]
    public partial class tblColorTapa
    {
        public tblColorTapa()
        {
            tblCantidadNMovimientoElemLog = new HashSet<tblCantidadNMovimientoElemLog>();
            tblPlantillaPrenda_generica = new HashSet<tblPlantillaPrenda_generica>();
            tblPrenda = new HashSet<tblPrenda>();
            tblSacasPendientes = new HashSet<tblSacasPendientes>();
        }

        [Key]
        public byte idColorTapa { get; set; }
        [StringLength(5)]
        public string? codigo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [StringLength(7)]
        public string? codigoHexadecimal { get; set; }

        [InverseProperty("idColorTapaNavigation")]
        public virtual ICollection<tblCantidadNMovimientoElemLog> tblCantidadNMovimientoElemLog { get; set; }
        [InverseProperty("idColorTapaNavigation")]
        public virtual ICollection<tblPlantillaPrenda_generica> tblPlantillaPrenda_generica { get; set; }
        [InverseProperty("idColorTapaNavigation")]
        public virtual ICollection<tblPrenda> tblPrenda { get; set; }
        [InverseProperty("idColorTapaNavigation")]
        public virtual ICollection<tblSacasPendientes> tblSacasPendientes { get; set; }
    }
}
