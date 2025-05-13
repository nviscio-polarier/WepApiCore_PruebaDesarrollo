using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDatosFinancierosNTrimestre", Schema = "Finanzas")]
    public partial class tblDatosFinancierosNTrimestre
    {
        [Key]
        public byte idConceptoFinanciero { get; set; }
        [Key]
        public short año { get; set; }
        [Key]
        public byte idGrupoEmpresarial { get; set; }
        [Key]
        public byte trimestre { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal valor { get; set; }
        public string? observaciones { get; set; }

        [ForeignKey("idConceptoFinanciero,año,idGrupoEmpresarial")]
        [InverseProperty("tblDatosFinancierosNTrimestre")]
        public virtual tblDatosFinancieros tblDatosFinancieros { get; set; } = null!;
    }
}
