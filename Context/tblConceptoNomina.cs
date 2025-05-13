using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblConceptoNomina", Schema = "RRHH")]
    public partial class tblConceptoNomina
    {
        public tblConceptoNomina()
        {
            tblConceptoNominaNNomina = new HashSet<tblConceptoNominaNNomina>();
            tblConceptoNominaNNomina_Gestoria = new HashSet<tblConceptoNominaNNomina_Gestoria>();
        }

        [Key]
        public short idConceptoNomina { get; set; }
        public int? idTraduccion { get; set; }
        public string codigo { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public bool isDevengo { get; set; }
        public bool isConceptoVariable { get; set; }
        public bool isInternoCalculado { get; set; }
        public bool isModificable { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblConceptoNomina")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idConceptoNominaNavigation")]
        public virtual ICollection<tblConceptoNominaNNomina> tblConceptoNominaNNomina { get; set; }
        [InverseProperty("idConceptoNominaNavigation")]
        public virtual ICollection<tblConceptoNominaNNomina_Gestoria> tblConceptoNominaNNomina_Gestoria { get; set; }
    }
}
