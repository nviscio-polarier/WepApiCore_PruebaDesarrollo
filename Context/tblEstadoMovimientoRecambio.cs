using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoMovimientoRecambio", Schema = "Assistant")]
    public partial class tblEstadoMovimientoRecambio
    {
        public tblEstadoMovimientoRecambio()
        {
            tblEstadoMovimientoRecambioNMovimientoRecambio = new HashSet<tblEstadoMovimientoRecambioNMovimientoRecambio>();
            tblMovimientoRecambio = new HashSet<tblMovimientoRecambio>();
        }

        [Key]
        public byte idEstadoMovimientoRecambio { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idEstadoMovimientoRecambioNavigation")]
        public virtual ICollection<tblEstadoMovimientoRecambioNMovimientoRecambio> tblEstadoMovimientoRecambioNMovimientoRecambio { get; set; }
        [InverseProperty("idEstadoMovimientoRecambioNavigation")]
        public virtual ICollection<tblMovimientoRecambio> tblMovimientoRecambio { get; set; }
    }
}
