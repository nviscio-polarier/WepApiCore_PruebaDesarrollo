using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTasaCambio", Schema = "General")]
    public partial class tblTasaCambio
    {
        [Key]
        public byte idMonedaOrigen { get; set; }
        [Key]
        public byte idMonedaDestino { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "decimal(8, 5)")]
        public decimal? tasaCambio { get; set; }

        [ForeignKey("idMonedaDestino")]
        [InverseProperty("tblTasaCambioidMonedaDestinoNavigation")]
        public virtual tblMoneda idMonedaDestinoNavigation { get; set; } = null!;
        [ForeignKey("idMonedaOrigen")]
        [InverseProperty("tblTasaCambioidMonedaOrigenNavigation")]
        public virtual tblMoneda idMonedaOrigenNavigation { get; set; } = null!;
    }
}
