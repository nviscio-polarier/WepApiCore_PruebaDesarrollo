using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaHuespedNRepartoValet", Schema = "Logistica")]
    public partial class tblPrendaHuespedNRepartoValet
    {
        [Key]
        public int idRepartoValet { get; set; }
        [Key]
        public int idPrendaHuesped { get; set; }
        public int? cantidadLavadoSeco { get; set; }
        public int? cantidadPlanchado { get; set; }
        public int? cantidadLavadoPlanchado { get; set; }
        public int? cantidadTintoreria { get; set; }

        [ForeignKey("idPrendaHuesped")]
        [InverseProperty("tblPrendaHuespedNRepartoValet")]
        public virtual tblPrendaHuesped idPrendaHuespedNavigation { get; set; } = null!;
        [ForeignKey("idRepartoValet")]
        [InverseProperty("tblPrendaHuespedNRepartoValet")]
        public virtual tblRepartosValet idRepartoValetNavigation { get; set; } = null!;
    }
}
