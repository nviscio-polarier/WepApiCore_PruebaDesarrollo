using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRepartosValet", Schema = "Logistica")]
    public partial class tblRepartosValet
    {
        public tblRepartosValet()
        {
            tblPrendaEjecutivoNRepartoValet = new HashSet<tblPrendaEjecutivoNRepartoValet>();
            tblPrendaExtraNRepartoValet = new HashSet<tblPrendaExtraNRepartoValet>();
            tblPrendaHuespedNRepartoValet = new HashSet<tblPrendaHuespedNRepartoValet>();
        }

        [Key]
        public int idRepartoValet { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public string? numHabitacion { get; set; }
        public int tipoLavado { get; set; }
        public int? idEntidad { get; set; }
        public string? observaciones { get; set; }
        public int? estado { get; set; }
        [Column(TypeName = "decimal(6, 4)")]
        public decimal? precio { get; set; }
        [StringLength(10)]
        public string codigoRepartosValet { get; set; } = null!;
        public int idLavanderia { get; set; }
        public int idTipoRepartoValet { get; set; }
        public bool isEnviadoCorreo { get; set; }
        public bool eliminado { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal descuento { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblRepartosValet")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblRepartosValet")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idRepartoValetNavigation")]
        public virtual ICollection<tblPrendaEjecutivoNRepartoValet> tblPrendaEjecutivoNRepartoValet { get; set; }
        [InverseProperty("idRepartosValetNavigation")]
        public virtual ICollection<tblPrendaExtraNRepartoValet> tblPrendaExtraNRepartoValet { get; set; }
        [InverseProperty("idRepartoValetNavigation")]
        public virtual ICollection<tblPrendaHuespedNRepartoValet> tblPrendaHuespedNRepartoValet { get; set; }
    }
}
