using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblParadaNRutaExpedicion", Schema = "Logistica")]
    public partial class tblParadaNRutaExpedicion
    {
        [Key]
        public int idParadaNRutaExpedicion { get; set; }
        public int idRutaExpedicion { get; set; }
        public int? idLavanderia { get; set; }
        public int? idEntidad { get; set; }
        public byte orden { get; set; }
        public bool? isCarga { get; set; }
        public string? observaciones { get; set; }
        [Required]
        public bool? activo { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblParadaNRutaExpedicion")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblParadaNRutaExpedicion")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idRutaExpedicion")]
        [InverseProperty("tblParadaNRutaExpedicion")]
        public virtual tblRutaExpedicion idRutaExpedicionNavigation { get; set; } = null!;
    }
}
