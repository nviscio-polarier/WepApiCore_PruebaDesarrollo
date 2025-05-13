using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCalendarioLavanderia", Schema = "General")]
    public partial class tblCalendarioLavanderia
    {
        [Key]
        public int idLavanderia { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Key]
        public byte idCalendario_Estado { get; set; }

        [ForeignKey("idCalendario_Estado")]
        [InverseProperty("tblCalendarioLavanderia")]
        public virtual tblCalendario_Estado idCalendario_EstadoNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblCalendarioLavanderia")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
    }
}
