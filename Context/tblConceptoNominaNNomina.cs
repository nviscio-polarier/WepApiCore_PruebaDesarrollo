using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblConceptoNominaNNomina", Schema = "RRHH")]
    public partial class tblConceptoNominaNNomina
    {
        [Key]
        public int idNomina { get; set; }
        [Key]
        public short idConceptoNomina { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? cantidad { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precioUnitario { get; set; }
        public string? observaciones { get; set; }
        public int idUsuario_validacion { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha_validacion { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? importe { get; set; }

        [ForeignKey("idConceptoNomina")]
        [InverseProperty("tblConceptoNominaNNomina")]
        public virtual tblConceptoNomina idConceptoNominaNavigation { get; set; } = null!;
        [ForeignKey("idNomina")]
        [InverseProperty("tblConceptoNominaNNomina")]
        public virtual tblNomina idNominaNavigation { get; set; } = null!;
    }
}
