using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDatosSalariales_historico_salarioBase", Schema = "RRHH")]
    public partial class tblDatosSalariales_historico_salarioBase
    {
        [Key]
        public int idPersona { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal salarioBase { get; set; }

        [ForeignKey("idPersona")]
        [InverseProperty("tblDatosSalariales_historico_salarioBase")]
        public virtual tblDatosSalariales idPersonaNavigation { get; set; } = null!;
    }
}
