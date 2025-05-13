using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCierreFactEntidad", Schema = "General")]
    public partial class tblCierreFactEntidad
    {
        [Key]
        public int idEntidad { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fechaDesde { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fechaHasta { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblCierreFactEntidad")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
    }
}
