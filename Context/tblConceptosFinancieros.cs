using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblConceptosFinancieros", Schema = "Finanzas")]
    public partial class tblConceptosFinancieros
    {
        public tblConceptosFinancieros()
        {
            tblDatosFinancieros = new HashSet<tblDatosFinancieros>();
        }

        [Key]
        public byte idConceptoFinanciero { get; set; }
        public string denominacion { get; set; } = null!;
        public byte? orden { get; set; }

        [InverseProperty("idConceptoFinancieroNavigation")]
        public virtual ICollection<tblDatosFinancieros> tblDatosFinancieros { get; set; }
    }
}
