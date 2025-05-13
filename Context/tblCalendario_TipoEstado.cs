using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCalendario_TipoEstado", Schema = "General")]
    public partial class tblCalendario_TipoEstado
    {
        public tblCalendario_TipoEstado()
        {
            tblCalendarioEntidad_Estados = new HashSet<tblCalendarioEntidad_Estados>();
        }

        [Key]
        public int idTipoEstado { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoEstadoNavigation")]
        public virtual ICollection<tblCalendarioEntidad_Estados> tblCalendarioEntidad_Estados { get; set; }
    }
}
