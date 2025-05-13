using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLibreMensual", Schema = "RRHH")]
    public partial class tblLibreMensual
    {
        [Key]
        public int idLibreMensual { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaIni1 { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaFin1 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaIni2 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaFin2 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaIni3 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaFin3 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaIni4 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaFin4 { get; set; }
        public int idPersona { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblLibreMensual")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
