using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDatosFinancieros", Schema = "Finanzas")]
    public partial class tblDatosFinancieros
    {
        public tblDatosFinancieros()
        {
            tblDatosFinancierosNTrimestre = new HashSet<tblDatosFinancierosNTrimestre>();
        }

        [Key]
        public byte idConceptoFinanciero { get; set; }
        [Key]
        public short año { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal minVal { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal maxVal { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal inicioRango1 { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal finRango1 { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal inicioRango2 { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal finRango2 { get; set; }
        [Key]
        public byte idGrupoEmpresarial { get; set; }
        public string? observaciones { get; set; }

        [ForeignKey("idConceptoFinanciero")]
        [InverseProperty("tblDatosFinancieros")]
        public virtual tblConceptosFinancieros idConceptoFinancieroNavigation { get; set; } = null!;
        [ForeignKey("idGrupoEmpresarial")]
        [InverseProperty("tblDatosFinancieros")]
        public virtual tblGrupoEmpresarial idGrupoEmpresarialNavigation { get; set; } = null!;
        [InverseProperty("tblDatosFinancieros")]
        public virtual ICollection<tblDatosFinancierosNTrimestre> tblDatosFinancierosNTrimestre { get; set; }
    }
}
