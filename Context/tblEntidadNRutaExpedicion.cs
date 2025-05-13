using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEntidadNRutaExpedicion", Schema = "Logistica")]
    public partial class tblEntidadNRutaExpedicion
    {
        [Key]
        public int idEntidad { get; set; }
        [Key]
        public int idRutaExpedicion { get; set; }
        public short orden { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblEntidadNRutaExpedicion")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idRutaExpedicion")]
        [InverseProperty("tblEntidadNRutaExpedicion")]
        public virtual tblRutaExpedicion idRutaExpedicionNavigation { get; set; } = null!;
    }
}
