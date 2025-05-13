using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLogsToken", Schema = "MyRealBonus")]
    public partial class tblLogsToken
    {
        [Key]
        public int idLog { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fechaHora { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string tipoOperacion { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string tabla { get; set; } = null!;
        public int idPersona { get; set; }
        public double? valorAnterior { get; set; }
        public double? valorNuevo { get; set; }
        public bool esError { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string? mensajeError { get; set; }
        [StringLength(4000)]
        [Unicode(false)]
        public string? detalleError { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string? mensaje { get; set; }
        public int? idTipoEventoToken { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblLogsToken")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTipoEventoToken")]
        [InverseProperty("tblLogsToken")]
        public virtual tblTipoEventoToken? idTipoEventoTokenNavigation { get; set; }
    }
}
