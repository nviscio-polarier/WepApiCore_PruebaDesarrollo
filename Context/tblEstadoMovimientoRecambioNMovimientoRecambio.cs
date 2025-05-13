using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoMovimientoRecambioNMovimientoRecambio", Schema = "Assistant")]
    public partial class tblEstadoMovimientoRecambioNMovimientoRecambio
    {
        [Key]
        public byte idEstadoMovimientoRecambio { get; set; }
        [Key]
        public int idMovimientoRecambio { get; set; }
        [Key]
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        public int idUsuario { get; set; }

        [ForeignKey("idEstadoMovimientoRecambio")]
        [InverseProperty("tblEstadoMovimientoRecambioNMovimientoRecambio")]
        public virtual tblEstadoMovimientoRecambio idEstadoMovimientoRecambioNavigation { get; set; } = null!;
        [ForeignKey("idMovimientoRecambio")]
        [InverseProperty("tblEstadoMovimientoRecambioNMovimientoRecambio")]
        public virtual tblMovimientoRecambio idMovimientoRecambioNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblEstadoMovimientoRecambioNMovimientoRecambio")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
