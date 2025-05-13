using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblKgLavadosLavadora", Schema = "Produccion")]
    [Index("idMaquina", "fecha", Name = "IX_tblKgLavadosLavadora_idMaquina_fecha")]
    public partial class tblKgLavadosLavadora
    {
        [Key]
        public int idKgLavadosLavadora { get; set; }
        public int? idCompañia { get; set; }
        public int? idEntidad { get; set; }
        public int idMaquina { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public int? cargasEstandard { get; set; }
        public int? cargasRechazo { get; set; }
        public byte? idGrupoPlantillaPrenda_generica { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblKgLavadosLavadora")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblKgLavadosLavadora")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idGrupoPlantillaPrenda_generica")]
        [InverseProperty("tblKgLavadosLavadora")]
        public virtual tblGrupoPlantillaPrenda_generica? idGrupoPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("idMaquina")]
        [InverseProperty("tblKgLavadosLavadora")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
    }
}
