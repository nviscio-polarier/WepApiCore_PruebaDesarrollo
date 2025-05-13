using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblParteTransporte_Localizacion", Schema = "Logistica")]
    public partial class tblParteTransporte_Localizacion
    {
        [Key]
        public int idParteTransporte { get; set; }
        [Key]
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        public string coordenadas { get; set; } = null!;
        public int? idParadaNParteTransporte { get; set; }
        public bool isOffline { get; set; }
        [Column(TypeName = "decimal(6, 2)")]
        public decimal? accuracy { get; set; }
        [Column(TypeName = "decimal(6, 2)")]
        public decimal? heading { get; set; }
        public byte? speed { get; set; }

        [ForeignKey("idParadaNParteTransporte")]
        [InverseProperty("tblParteTransporte_Localizacion")]
        public virtual tblParadaNParteTransporte? idParadaNParteTransporteNavigation { get; set; }
        [ForeignKey("idParteTransporte")]
        [InverseProperty("tblParteTransporte_Localizacion")]
        public virtual tblParteTransporte idParteTransporteNavigation { get; set; } = null!;
    }
}
