using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEventoPersona_Estado", Schema = "MyRealData")]
    public partial class tblEventoPersona_Estado
    {
        [Key]
        public byte idEventoPersona_estado { get; set; }
        public string denominacion { get; set; } = null!;
        public int idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblEventoPersona_Estado")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
    }
}
