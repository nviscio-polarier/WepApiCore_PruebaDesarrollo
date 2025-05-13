using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRutaSeccion", Schema = "Inventarios")]
    public partial class tblRutaSeccion
    {
        [Key]
        public int idRutaSeccion { get; set; }
        public int idEntidad { get; set; }
        public int codigo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public bool activo { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblRutaSeccion")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
    }
}
