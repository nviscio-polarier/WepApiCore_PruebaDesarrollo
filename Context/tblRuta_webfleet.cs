using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Keyless]
    [Table("tblRuta_webfleet", Schema = "Logistica")]
    public partial class tblRuta_webfleet
    {
        public string idRuta { get; set; } = null!;
        [StringLength(30)]
        public string idVehiculo_webfleet { get; set; } = null!;
        public string? idPersona_webfleet { get; set; }
        public int idTipoCombustible_webfleet { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fechaDesde { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fechaHasta { get; set; }
        public int? co2 { get; set; }
        [Column(TypeName = "decimal(10, 3)")]
        public decimal consumoCombustible { get; set; }
        public long distancia { get; set; }
        public int tiempoInactividad { get; set; }
        public int velocidadMaxima { get; set; }
        public int velocidadPromedio { get; set; }
    }
}
