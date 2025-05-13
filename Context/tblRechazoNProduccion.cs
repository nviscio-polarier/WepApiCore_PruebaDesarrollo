using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRechazoNProduccion", Schema = "Produccion")]
    [Index("idPrendaNProduccion", Name = "IX_tblRechazoNProduccion_idPrendaNProduccion")]
    public partial class tblRechazoNProduccion
    {
        [Key]
        public int idRechazoNProduccion { get; set; }
        public int idPrendaNProduccion { get; set; }
        public byte idTipoRechazo { get; set; }
        public int Cantidad { get; set; }

        [ForeignKey("idPrendaNProduccion")]
        [InverseProperty("tblRechazoNProduccion")]
        public virtual tblPrendaNProduccion idPrendaNProduccionNavigation { get; set; } = null!;
        [ForeignKey("idTipoRechazo")]
        [InverseProperty("tblRechazoNProduccion")]
        public virtual tblTipoRechazo idTipoRechazoNavigation { get; set; } = null!;
    }
}
