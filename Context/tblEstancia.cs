using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstancia", Schema = "General")]
    public partial class tblEstancia
    {
        [Key]
        public int idEntidad { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public int? estanciasPrevistas { get; set; }
        public int? estanciasReal { get; set; }
        public int? salidas { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblEstancia")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
    }
}
