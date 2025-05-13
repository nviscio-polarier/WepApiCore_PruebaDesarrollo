using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblKgLavadosTunel", Schema = "Produccion")]
    [Index("idMaquina", "fecha", Name = "IX_tblKgLavadosTunel_idMaquina_fecha")]
    public partial class tblKgLavadosTunel
    {
        [Key]
        public int idKgLavadosTunel { get; set; }
        public int? idCompañia { get; set; }
        public int? idEntidad { get; set; }
        public int idMaquina { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? kgProd { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? kgRech { get; set; }
        public byte? idGrupoPlantillaPrenda_generica { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblKgLavadosTunel")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblKgLavadosTunel")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idGrupoPlantillaPrenda_generica")]
        [InverseProperty("tblKgLavadosTunel")]
        public virtual tblGrupoPlantillaPrenda_generica? idGrupoPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("idMaquina")]
        [InverseProperty("tblKgLavadosTunel")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
    }
}
