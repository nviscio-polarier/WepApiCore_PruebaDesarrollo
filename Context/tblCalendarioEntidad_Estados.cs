using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCalendarioEntidad_Estados", Schema = "General")]
    public partial class tblCalendarioEntidad_Estados
    {
        public tblCalendarioEntidad_Estados()
        {
            tblCalendarioEntidad = new HashSet<tblCalendarioEntidad>();
        }

        [Key]
        public byte idEstado { get; set; }
        public string? denominacion { get; set; }
        public int idTraduccion { get; set; }
        [StringLength(7)]
        public string colorHexa { get; set; } = null!;
        public int? idTipoEstado { get; set; }

        [ForeignKey("idTipoEstado")]
        [InverseProperty("tblCalendarioEntidad_Estados")]
        public virtual tblCalendario_TipoEstado? idTipoEstadoNavigation { get; set; }
        [ForeignKey("idTraduccion")]
        [InverseProperty("tblCalendarioEntidad_Estados")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idEstadoNavigation")]
        public virtual ICollection<tblCalendarioEntidad> tblCalendarioEntidad { get; set; }
    }
}
