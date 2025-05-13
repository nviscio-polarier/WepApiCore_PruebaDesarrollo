using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblNotificacion_Estado", Schema = "GestionInterna")]
    public partial class tblNotificacion_Estado
    {
        public tblNotificacion_Estado()
        {
            tblNotificacion = new HashSet<tblNotificacion>();
            tblNotificacion_Evento = new HashSet<tblNotificacion_Evento>();
        }

        [Key]
        public int idNotificacion_Estado { get; set; }
        public string denominacion { get; set; } = null!;
        public bool isEstadoFinal { get; set; }
        public int idTraduccion { get; set; }

        [InverseProperty("idNotificacion_EstadoNavigation")]
        public virtual ICollection<tblNotificacion> tblNotificacion { get; set; }
        [InverseProperty("idNotificacion_EstadoNavigation")]
        public virtual ICollection<tblNotificacion_Evento> tblNotificacion_Evento { get; set; }
    }
}
