using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblParadaNParteTransporte", Schema = "Logistica")]
    public partial class tblParadaNParteTransporte
    {
        public tblParadaNParteTransporte()
        {
            tblParteTransporte_Localizacion = new HashSet<tblParteTransporte_Localizacion>();
        }

        [Key]
        public int idParadaNParteTransporte { get; set; }
        public int idParteTransporte { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaLlegada { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaSalida { get; set; }
        public int? idLavanderia { get; set; }
        public int? idEntidad { get; set; }
        public byte orden { get; set; }
        public bool? isCarga { get; set; }
        public int? idIncidencia { get; set; }
        public int? idMotivoPausa { get; set; }
        public string? observaciones { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaOmitido { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaCancelado { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaPospuesto { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblParadaNParteTransporte")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idIncidencia")]
        [InverseProperty("tblParadaNParteTransporte")]
        public virtual tblIncidencia? idIncidenciaNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblParadaNParteTransporte")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idMotivoPausa")]
        [InverseProperty("tblParadaNParteTransporte")]
        public virtual tblMotivoPausa? idMotivoPausaNavigation { get; set; }
        [ForeignKey("idParteTransporte")]
        [InverseProperty("tblParadaNParteTransporte")]
        public virtual tblParteTransporte idParteTransporteNavigation { get; set; } = null!;
        [InverseProperty("idParadaNParteTransporteNavigation")]
        public virtual ICollection<tblParteTransporte_Localizacion> tblParteTransporte_Localizacion { get; set; }
    }
}
