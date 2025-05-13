using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaEjecutivoNRepartoValet", Schema = "MyValet")]
    public partial class tblPrendaEjecutivoNRepartoValet
    {
        [Key]
        public int idRepartoValet { get; set; }
        [Key]
        public int idPrendaEjecutivo { get; set; }
        public int? cantidadLavadoDoblado { get; set; }
        public int? cantidadLavadoVaporizado { get; set; }
        public int? cantidadPlanchado { get; set; }
        public int? cantidadLavadoSecadoPlanchado { get; set; }

        [ForeignKey("idPrendaEjecutivo")]
        [InverseProperty("tblPrendaEjecutivoNRepartoValet")]
        public virtual tblPrendaEjecutivo idPrendaEjecutivoNavigation { get; set; } = null!;
        [ForeignKey("idRepartoValet")]
        [InverseProperty("tblPrendaEjecutivoNRepartoValet")]
        public virtual tblRepartosValet idRepartoValetNavigation { get; set; } = null!;
    }
}
