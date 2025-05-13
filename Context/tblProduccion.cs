using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblProduccion", Schema = "Produccion")]
    [Index("fecha", Name = "IX_tblProduccion_fecha")]
    [Index("idEntidad", "idLavanderia", "fecha", Name = "IX_tblProduccion_idEntidad_idLavanderia_fecha")]
    [Index("idLavanderia", "fecha", Name = "IX_tblProduccion_idLavanderia_fecha")]
    public partial class tblProduccion
    {
        public tblProduccion()
        {
            tblPrendaNProduccion = new HashSet<tblPrendaNProduccion>();
        }

        [Key]
        public int idProduccion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        [StringLength(10)]
        public string? idxGsbs { get; set; }
        public byte idTipoProduccion { get; set; }
        public int? idEntidad { get; set; }
        public int? idCompañia { get; set; }
        public int? idMaquina { get; set; }
        public string? mac { get; set; }
        public int? idProduccionPantalla { get; set; }
        public int? idLavanderia { get; set; }
        public int? idTurno { get; set; }
        public byte? idGrupoPlantillaPrenda_generica { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblProduccion")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblProduccion")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblProduccion")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idMaquina")]
        [InverseProperty("tblProduccion")]
        public virtual tblMaquina? idMaquinaNavigation { get; set; }
        [ForeignKey("idTipoProduccion")]
        [InverseProperty("tblProduccion")]
        public virtual tblTipoProduccion idTipoProduccionNavigation { get; set; } = null!;
        [ForeignKey("idTurno")]
        [InverseProperty("tblProduccion")]
        public virtual tblTurno? idTurnoNavigation { get; set; }
        [InverseProperty("idProduccionNavigation")]
        public virtual ICollection<tblPrendaNProduccion> tblPrendaNProduccion { get; set; }
    }
}
