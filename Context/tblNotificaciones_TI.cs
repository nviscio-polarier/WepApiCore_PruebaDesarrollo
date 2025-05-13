using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblNotificaciones_TI", Schema = "GestionInterna")]
    public partial class tblNotificaciones_TI
    {
        [Key]
        public int idNotificacion_TI { get; set; }
        public int idAplicacion { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha_Inicio { get; set; }
        [Precision(0)]
        public DateTimeOffset? fecha_Responsable { get; set; }
        [Precision(0)]
        public DateTimeOffset? fecha_Solucion { get; set; }
        public int? idUsuarioResponsable { get; set; }
        public string? codigo { get; set; }
        public string? descripcion { get; set; }

        [ForeignKey("idUsuarioResponsable")]
        [InverseProperty("tblNotificaciones_TI")]
        public virtual tblUsuario? idUsuarioResponsableNavigation { get; set; }
    }
}
