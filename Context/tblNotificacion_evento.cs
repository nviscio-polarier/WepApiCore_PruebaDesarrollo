using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblNotificacion_Evento", Schema = "GestionInterna")]
    public partial class tblNotificacion_Evento
    {
        [Key]
        public int idNotificacion_Evento { get; set; }
        public int? idNotificacion { get; set; }
        [StringLength(40)]
        public string? idMessage { get; set; }
        [Precision(0)]
        public DateTimeOffset? fecha { get; set; }
        public int? idNotificacion_Estado { get; set; }

        [ForeignKey("idNotificacion")]
        [InverseProperty("tblNotificacion_Evento")]
        public virtual tblNotificacion? idNotificacionNavigation { get; set; }
        [ForeignKey("idNotificacion_Estado")]
        [InverseProperty("tblNotificacion_Evento")]
        public virtual tblNotificacion_Estado? idNotificacion_EstadoNavigation { get; set; }
    }
}
