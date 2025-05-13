using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRutaExpedicion", Schema = "Logistica")]
    public partial class tblRutaExpedicion
    {
        public tblRutaExpedicion()
        {
            tblEntidadNRutaExpedicion = new HashSet<tblEntidadNRutaExpedicion>();
            tblParadaNRutaExpedicion = new HashSet<tblParadaNRutaExpedicion>();
            tblParteTransporte = new HashSet<tblParteTransporte>();
        }

        [Key]
        public int idRutaExpedicion { get; set; }
        public string? denominacion { get; set; }
        public int idLavanderia { get; set; }
        public bool? activo { get; set; }
        public bool eliminado { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblRutaExpedicion")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idRutaExpedicionNavigation")]
        public virtual ICollection<tblEntidadNRutaExpedicion> tblEntidadNRutaExpedicion { get; set; }
        [InverseProperty("idRutaExpedicionNavigation")]
        public virtual ICollection<tblParadaNRutaExpedicion> tblParadaNRutaExpedicion { get; set; }
        [InverseProperty("idRutaExpedicionNavigation")]
        public virtual ICollection<tblParteTransporte> tblParteTransporte { get; set; }
    }
}
