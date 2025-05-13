using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMovimiento", Schema = "Inventarios")]
    public partial class tblMovimiento
    {
        public tblMovimiento()
        {
            tblPrendaNMovimiento = new HashSet<tblPrendaNMovimiento>();
        }

        [Key]
        public int idMovimiento { get; set; }
        public int? idCompañia { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public int? idEntidad { get; set; }
        public int idTipoMovimiento { get; set; }
        public string? observaciones { get; set; }
        public string? codigoAlbaran { get; set; }
        public string? codigoFactura { get; set; }
        public bool isApp { get; set; }
        public int? idTipoRetiro { get; set; }
        public byte? idGrupoPlantillaPrenda_generica { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblMovimiento")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblMovimiento")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idGrupoPlantillaPrenda_generica")]
        [InverseProperty("tblMovimiento")]
        public virtual tblGrupoPlantillaPrenda_generica? idGrupoPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("idTipoMovimiento")]
        [InverseProperty("tblMovimiento")]
        public virtual tblTipoMovimiento idTipoMovimientoNavigation { get; set; } = null!;
        [ForeignKey("idTipoRetiro")]
        [InverseProperty("tblMovimiento")]
        public virtual tblTipoRetiro? idTipoRetiroNavigation { get; set; }
        [InverseProperty("idMovimientoNavigation")]
        public virtual ICollection<tblPrendaNMovimiento> tblPrendaNMovimiento { get; set; }
    }
}
