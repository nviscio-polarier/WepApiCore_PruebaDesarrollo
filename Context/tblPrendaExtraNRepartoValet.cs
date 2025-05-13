using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaExtraNRepartoValet", Schema = "Logistica")]
    public partial class tblPrendaExtraNRepartoValet
    {
        [Key]
        public int idRepartosValet { get; set; }
        [Key]
        public int idPrendaExtra { get; set; }
        public int? cantidadLavadoSeco { get; set; }
        public int? cantidadPlanchado { get; set; }
        public int? cantidadLavadoPlanchado { get; set; }
        public int? cantidadTintoreria { get; set; }

        [ForeignKey("idPrendaExtra")]
        [InverseProperty("tblPrendaExtraNRepartoValet")]
        public virtual tblPrendaExtra idPrendaExtraNavigation { get; set; } = null!;
        [ForeignKey("idRepartosValet")]
        [InverseProperty("tblPrendaExtraNRepartoValet")]
        public virtual tblRepartosValet idRepartosValetNavigation { get; set; } = null!;
    }
}
