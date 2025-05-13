using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoNotificacion", Schema = "General")]
    public partial class tblTipoNotificacion
    {
        [Key]
        public int idTipoNotificacion { get; set; }
        public string clave { get; set; } = null!;
        public int? idTraduccionDenominacion { get; set; }
        public int? idTraduccionDescripcion { get; set; }
        [StringLength(7)]
        public string? color { get; set; }
        [StringLength(50)]
        public string? icon { get; set; }
        public bool? isInformativa { get; set; }

        [ForeignKey("idTraduccionDenominacion")]
        [InverseProperty("tblTipoNotificacionidTraduccionDenominacionNavigation")]
        public virtual tblTraduccion? idTraduccionDenominacionNavigation { get; set; }
        [ForeignKey("idTraduccionDescripcion")]
        [InverseProperty("tblTipoNotificacionidTraduccionDescripcionNavigation")]
        public virtual tblTraduccion? idTraduccionDescripcionNavigation { get; set; }
    }
}
