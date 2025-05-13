using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblNotificacion", Schema = "GestionInterna")]
    public partial class tblNotificacion
    {
        public tblNotificacion()
        {
            tblNotificacion_Evento = new HashSet<tblNotificacion_Evento>();
        }

        [Key]
        public int idNotificacion { get; set; }
        public int? idUsuario { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaEnvio { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaSuccess { get; set; }
        public string? titulo { get; set; }
        public string? texto { get; set; }
        public int? idNotificacion_Estado { get; set; }

        [ForeignKey("idNotificacion_Estado")]
        [InverseProperty("tblNotificacion")]
        public virtual tblNotificacion_Estado? idNotificacion_EstadoNavigation { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("tblNotificacion")]
        public virtual tblUsuario? idUsuarioNavigation { get; set; }
        [InverseProperty("idNotificacionNavigation")]
        public virtual ICollection<tblNotificacion_Evento> tblNotificacion_Evento { get; set; }
    }
}
