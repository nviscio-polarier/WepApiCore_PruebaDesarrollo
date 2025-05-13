using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblClienteNMaquina", Schema = "MyRealData")]
    public partial class tblClienteNMaquina
    {
        [Key]
        public int idClienteNMaquina { get; set; }
        public int idMaquina { get; set; }
        [Precision(0)]
        public DateTimeOffset fechaIni { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaFin { get; set; }
        public int? idCompañia { get; set; }
        public int? idEntidad { get; set; }
        public byte idFamilia { get; set; }
        public short? idTipoPrenda { get; set; }
        public bool isOffline { get; set; }
        public byte? idGrupoPlantillaPrenda_generica { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblClienteNMaquina")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblClienteNMaquina")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idFamilia")]
        [InverseProperty("tblClienteNMaquina")]
        public virtual tblFamilia idFamiliaNavigation { get; set; } = null!;
        [ForeignKey("idGrupoPlantillaPrenda_generica")]
        [InverseProperty("tblClienteNMaquina")]
        public virtual tblGrupoPlantillaPrenda_generica? idGrupoPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("idMaquina")]
        [InverseProperty("tblClienteNMaquina")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idTipoPrenda")]
        [InverseProperty("tblClienteNMaquina")]
        public virtual tblTipoPrenda? idTipoPrendaNavigation { get; set; }
    }
}
