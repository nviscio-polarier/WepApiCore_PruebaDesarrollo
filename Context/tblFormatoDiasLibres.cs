using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblFormatoDiasLibres", Schema = "RRHH")]
    public partial class tblFormatoDiasLibres
    {
        public tblFormatoDiasLibres()
        {
            tblLlamamiento = new HashSet<tblLlamamiento>();
            tblPersona = new HashSet<tblPersona>();
        }

        [Key]
        public byte idFormatoDiasLibres { get; set; }
        public string? denominacion { get; set; }
        public int? idTraduccion { get; set; }
        public int? numDiasLibres { get; set; }
        public bool? isMensual { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblFormatoDiasLibres")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idFormatoDiasLibresNavigation")]
        public virtual ICollection<tblLlamamiento> tblLlamamiento { get; set; }
        [InverseProperty("idFormatoDiasLibresNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
    }
}
