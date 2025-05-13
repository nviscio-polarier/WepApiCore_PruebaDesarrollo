using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCalendarioEntidad", Schema = "General")]
    public partial class tblCalendarioEntidad
    {
        [Key]
        public int idEntidad { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Key]
        public byte idEstado { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblCalendarioEntidad")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idEstado")]
        [InverseProperty("tblCalendarioEntidad")]
        public virtual tblCalendarioEntidad_Estados idEstadoNavigation { get; set; } = null!;
    }
}
