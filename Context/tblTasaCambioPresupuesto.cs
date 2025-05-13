using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTasaCambioPresupuesto", Schema = "ControlPresupuestario")]
    public partial class tblTasaCambioPresupuesto
    {
        [Key]
        public byte idMonedaDestino { get; set; }
        [Key]
        public short año { get; set; }
        [Column(TypeName = "decimal(8, 5)")]
        public decimal? tasaCambio { get; set; }

        [ForeignKey("idMonedaDestino")]
        [InverseProperty("tblTasaCambioPresupuesto")]
        public virtual tblMoneda idMonedaDestinoNavigation { get; set; } = null!;
    }
}
