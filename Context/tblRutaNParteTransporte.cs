using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRutaNParteTransporte", Schema = "Logistica")]
    public partial class tblRutaNParteTransporte
    {
        public int idParteTransporte { get; set; }
        public int? idReparto { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? horaLlegada { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? horaSalida { get; set; }
        public int idEntidad { get; set; }
        [Key]
        public int idRutaNParteTransporte { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblRutaNParteTransporte")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idParteTransporte")]
        [InverseProperty("tblRutaNParteTransporte")]
        public virtual tblParteTransporte idParteTransporteNavigation { get; set; } = null!;
        [ForeignKey("idReparto")]
        [InverseProperty("tblRutaNParteTransporte")]
        public virtual tblReparto? idRepartoNavigation { get; set; }
    }
}
