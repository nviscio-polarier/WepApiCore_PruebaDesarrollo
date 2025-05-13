using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNMaquina", Schema = "MyRealData")]
    [Index("idMaquina", "idFamilia", Name = "IX_tblPrendaNMaquina_idMaquina_familia")]
    [Index("idMaquina", "fecha", Name = "IX_tblPrendaNMaquina_idMaquina_fecha")]
    public partial class tblPrendaNMaquina
    {
        [Key]
        public int idPrendaNMaquina { get; set; }
        public int idMaquina { get; set; }
        public DateTimeOffset fecha { get; set; }
        public int? numVia { get; set; }
        public byte idFamilia { get; set; }
        public short? idTipoPrenda { get; set; }
        public bool isOffline { get; set; }

        [ForeignKey("idFamilia")]
        [InverseProperty("tblPrendaNMaquina")]
        public virtual tblFamilia idFamiliaNavigation { get; set; } = null!;
        [ForeignKey("idMaquina")]
        [InverseProperty("tblPrendaNMaquina")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idTipoPrenda")]
        [InverseProperty("tblPrendaNMaquina")]
        public virtual tblTipoPrenda? idTipoPrendaNavigation { get; set; }
    }
}
